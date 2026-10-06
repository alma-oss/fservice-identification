---
name: fservice-identification
description: Use whenever generating or reviewing F# (or Fable) code that parses, validates, composes, or matches service identifiers built from Domain, Context, Purpose, Version, Zone, Bucket. Trigger on Service, Processor, Instance, Spot, Box, BoxPattern, ServiceIdentification, the Create factory, parseStrict/parse, isMatching, concat/value, or mentions of the Alma.ServiceIdentification NuGet package and the alma-oss/fservice-identification repo.
---

# F-Service-Identification

Library: [alma-oss/fservice-identification](https://github.com/alma-oss/fservice-identification)
NuGet: `Alma.ServiceIdentification`

## Purpose

Provides strongly-typed F# types for identifying services, their processors, instances, and physical placements ("boxes"). It turns loosely-formatted identifier strings into validated, composable domain types and back, with structured parse errors and pattern-based matching. Works in both .NET and Fable projects.

## Type Hierarchy

```
domain ─┐─────────┐───────────┐──────────┐
context ┘ service │           │          │
purpose ──────────┘ processor │          │
version ──────────────────────┘ instance │ box
zone ───┐ spot                           │
bucket ─┘────────────────────────────────┘
```

Tree view:

```
               box
            /       \
    instance         spot
    /       \       /    \
processor  version  zone  bucket
/         \
service  purpose
/       \
domain  context
```

### Simple types

All parts match `[a-zA-Z]+` (letters only, no separators), except `Version`, which allows trailing digits: `[a-zA-Z]+[a-zA-Z0-9]*`.

| Type    | Role in the identifier            | Example   |
|---------|-----------------------------------|-----------|
| Domain  | Top-level platform/product namespace | `platform` |
| Context | System / architectural unit       | `userManager` |
| Purpose | Sub-service / feature within the system | `account` |
| Version | Deployment version of the instance | `v1` |
| Zone    | Geographic / infrastructure zone  | `eu` |
| Bucket  | Cluster / namespace bucket within the zone | `primary` |

### Composed types

Parts within `Service`/`Processor`/`Instance`/`Spot` are joined with `-`; `Box` joins its `Spot` and `Instance` with `@` (rendered `zone-bucket@domain-context-purpose-version`).

| Type      | Composition                          | Rendered format                                | Example |
|-----------|--------------------------------------|------------------------------------------------|---------|
| Service   | Domain + Context                     | `domain-context`                               | `platform-userManager` |
| Processor | Domain + Context + Purpose           | `domain-context-purpose`                       | `platform-userManager-account` |
| Instance  | Domain + Context + Purpose + Version | `domain-context-purpose-version`               | `platform-userManager-account-v1` |
| Spot      | Zone + Bucket                        | `zone-bucket`                                  | `eu-primary` |
| Box       | Spot @ Instance                      | `zone-bucket@domain-context-purpose-version`   | `eu-primary@platform-userManager-account-v1` |

> Note: the rendered separator is fixed (`-` between parts, `@` between Spot and Instance). The `parseStrict` / `Create` overloads accept a configurable separator for *parsing*, but `*.value` / `Box.value` always render with these literals.

## When to Use

- Parsing identifier strings (e.g. `domain-context-purpose-version`) into typed values.
- Building identifiers programmatically from parts and rendering them back to strings.
- Promoting/projecting between identifier shapes (Service → Processor → Instance → Box).
- Matching a concrete identifier against a broader pattern.

## When NOT to Use

- General-purpose string parsing unrelated to this identifier hierarchy.
- Defining application/business entities — these types only model the identifier itself.

## Validation Rules

- Every part except `Version` must match `[a-zA-Z]+` — no hyphens, underscores, or digits. `Version` is `[a-zA-Z]+[a-zA-Z0-9]*` (must start with a letter; digits allowed afterwards).
- The separator between parts is `-`; the separator between `Spot` and `Instance` in a `Box` is `@`.
- Parsing returns `Result<_, _Error>`; a violation yields a structured error DU (`Empty` | `InvalidFormat of string`) carrying the offending value per part — never a flattened string.

## Main Concepts

- **Simple types** — single-case DU wrappers over `string`, each regex-validated: `Domain`, `Context`, `Purpose`, `Version`, `Zone`, `Bucket`.
- **Pattern types** — `PurposePattern`, `VersionPattern`, `ZonePattern`, `BucketPattern`; each is either a concrete value or `Any` (wildcard).
- **Composed types** — records assembled from simple types: `Service`, `Processor`, `Instance`, `Spot`, `Box`, and the pattern record `BoxPattern`.
- **ServiceIdentification** — a DU (`ByService` | `ByProcessor` | `ByInstance`) unifying the three identifier depths.
- **Per-type modules** — each type has a `[<RequireQualifiedAccess>]` module exposing parsing, construction, projection, rendering, and casing helpers.
- **Create** — an overloaded static factory class offering a single entry point (`Create.Service`, `Create.Box`, …) accepting mixed string and typed arguments.
- **Matching** — `ServiceIdentification.isMatching` and `BoxPattern.isMatching` / the `(|Matching|_|)` active pattern test a value against a pattern.
- **Structured errors** — parsing failures are returned as nested error DUs carrying per-part detail, not strings.

## Related Libraries

Fable-compatible: the package ships its `.fs` sources so it can be consumed directly in a Fable project.

## Keywords for Search

service identification, Domain, Context, Purpose, Version, Zone, Bucket, Service, Processor, Instance, Spot, Box, BoxPattern, ServiceIdentification, parseStrict, parse, createFromValues, createFromStrings, concat, isMatching, Create factory, single-case DU, Fable, Alma.ServiceIdentification

## Reference Files

For composition principles and recommended API usage, read `references/preferred-patterns.md`. For known pitfalls and incorrect assumptions, read `references/anti-patterns.md`. For worked code examples, read `references/examples.md`.
