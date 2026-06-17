# Anti-Patterns

Each entry is **mistake → why it is wrong → fix**.

## Constructing simple types from raw input

- **Mistake:** wrapping an untrusted string directly, e.g. `Domain rawString`.
- **Why:** the DU constructor performs no validation, so the regex contract for that part is silently bypassed and an invalid value flows into the rest of the system.
- **Fix:** go through the parsing path (`Domain.parseStrict` or the matching `Create` overload) and handle the `Result`. See `examples.md` → Basic parsing.

## Treating `parse` as validating

- **Mistake:** using `parse` (or `ServiceIdentification.parse`) on untrusted input and assuming a `Some` result means the parts are well-formed.
- **Why:** `parse` only checks segment count and rejects empty/wildcard parts; it skips the per-part regex, so malformed parts pass through wrapped as-is.
- **Fix:** use `parseStrict` whenever the input must actually be valid; reserve `parse` for already-trusted data. See `examples.md` → Lenient parsing.

## Assuming every separator is `-`

- **Mistake:** calling `Create.Spot("zone.bucket")` style code expecting a `-` split, or hardcoding `-` everywhere.
- **Why:** `Service`/`Processor`/`Instance`/`Box` default to `-`, but the `Spot` factory defaults to `.`, and `Box` additionally splits the spot from the instance on `@`. A wrong assumption yields an `InvalidFormat` error.
- **Fix:** pass the separator explicitly when it differs from the type's default, and use `Box`'s `zone-bucket@domain-context-purpose-version` shape. See `examples.md` → Rendering back to string.

## Expecting `Version` to share the other parts' charset

- **Mistake:** assuming `Version` accepts the same letters-only input as `Domain`/`Context`/`Purpose`/`Zone`/`Bucket`, or rejecting a version like `v2`.
- **Why:** the other parts allow letters only, whereas `Version` allows digits after a leading letter — the validation rules genuinely differ.
- **Fix:** account for the alphanumeric-after-letter rule when generating or testing version strings.

## Inspecting errors as text

- **Mistake:** converting a parse error to a string and pattern-matching on the message.
- **Why:** errors are structured DUs carrying per-part detail; stringifying them discards which parts failed and why.
- **Fix:** match on the error DU cases (the `InvalidFormat` case vs. the part-list case and its entries). See `examples.md` → Handling structured errors.

## Re-parsing data you already hold typed

- **Mistake:** concatenating typed parts back into a string only to parse them again when you need a deeper or shallower shape.
- **Why:** it reintroduces a failure path for data that is already valid and wastes work.
- **Fix:** use the projection getters and the `ofService`/`ofProcessor`/`ofInstance` promotion helpers. See `examples.md` → Projection and promotion.
