# Preferred Patterns

## Core Principles

- **Parse, don't validate.** The simple types are single-case discriminated unions over `string`. Never trust a raw string at the type boundary — convert it into the wrapped type through a parsing function so the regex contract is enforced exactly once, at the edge.
- **Pick the right depth.** Model only as much of the identifier as you actually have: `Service` (domain + context), `Processor` (+ purpose), `Instance` (+ version), `Spot` (zone + bucket), `Box` (instance + spot). Promote upward only when you genuinely gain the extra part.
- **Keep the wildcard in patterns, not in values.** `Any` belongs to the `*Pattern` types and to `BoxPattern`; concrete identifiers should always carry real values.

## Recommended API Usage

Each per-type module exposes a consistent surface:

- `parseStrict` — splits an input string and returns a `Result` with a structured error on failure. Use this whenever the input is untrusted and you need to know *why* it was rejected. See `examples.md` → Basic parsing.
- `parse` — lenient counterpart returning an `option`; it rejects only empty/wildcard parts and the wrong number of segments, and does **not** apply the per-part regex. Use only when input is already known-good. See `examples.md` → Lenient parsing.
- `createFromValues` — builds a record directly from already-typed parts (total, no failure path). See `examples.md` → Programmatic construction.
- `createFromStrings` — builds from a tuple of raw strings, returning `option`; empty and wildcard parts are rejected via the shared active patterns.
- `concat` / `value` — render a typed value back to its string form. `value` is reserved for `Box`, which uses the `@`-joined format; the other types use `concat` with an explicit separator. See `examples.md` → Rendering back to string.
- `value` / field getters (`domain`, `context`, `purpose`, …) — unwrap a part from a composed type.
- `lower` — produce a lower-cased copy of every part.

Prefer the `Create` factory when call sites mix raw strings and already-typed parts; it resolves the correct overload and returns a `Result` for parse-style calls and a plain value for compose-style calls. See `examples.md` → Create factory.

## Error Handling

Failures are nested discriminated unions, not strings. A composed-type error is either an `InvalidFormat` case (wrong segment count) or a part-list case carrying one entry per failed part (each itself wrapping the simple-type error). Match on these cases to report precise, per-part diagnostics rather than collapsing them to text. See `examples.md` → Handling structured errors.

## Composition

- Project downward with the field getters / projection helpers (e.g. a `Box` exposes its contained instance and spot; an `Instance` exposes its service and processor).
- Build upward with the `ofService` / `ofProcessor` / `ofInstance` helpers, supplying only the missing parts. This avoids re-parsing data you already hold in typed form. See `examples.md` → Projection and promotion.

## Integration with Other Libraries

The package distributes its source files, so the same API is available unchanged under Fable. Keep usage to the public modules and the `Create` factory so behavior stays identical across .NET and Fable targets.

## Naming Conventions

- Modules are `[<RequireQualifiedAccess>]` — always qualify calls (`Service.parseStrict`, `Box.value`).
- Simple-type case label equals the type name (`Domain` wraps via `Domain`), so deconstruct with the same identifier.
- Pattern wildcard is the `Any` case.

## Testing Recommendations

- Drive cases through a table of `(input, expected)` records and assert equality, mirroring the library's own data-provider test style. See `examples.md` → Table-driven test.
- Cover both the success and the structured-error branch of `parseStrict`.
- For matching logic, assert each `(value, pattern, expectedBool)` combination across the identifier depths.
