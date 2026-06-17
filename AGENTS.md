# AGENTS.md — Alma.ServiceIdentification

This repo ships Agent Skill for the `Alma.ServiceIdentification` library. Compatible agents discover it automatically; see `.agents/skills/fservice-identification/SKILL.md`.

## Project Purpose

`Alma.ServiceIdentification` provides strongly-typed F# types for service identifiers built from six parts — Domain, Context, Purpose, Version, Zone, Bucket. It parses loosely-formatted identifier strings into validated, composable domain types (`Service`, `Processor`, `Instance`, `Spot`, `Box`) and renders them back, returning structured parse errors and supporting pattern-based matching via `BoxPattern`. The package ships its `.fs` sources so it is consumable from both .NET and Fable projects.

## Tech Stack

- **Language:** F# (net10.0)
- **Package manager:** Paket
- **Build system:** FAKE (`build/build.fsproj`) invoked via `build.sh`
- **Test framework:** Expecto (+ YoloDev.Expecto.TestSdk)
- **NuGet package id:** Alma.ServiceIdentification
- **Repository URL:** https://github.com/alma-oss/fservice-identification

## Key Dependencies

- **FSharp.Core `~> 10.0`** — F# core library (only runtime dependency).
- **Expecto** — test runner/assertion framework (tests project only).
- **YoloDev.Expecto.TestSdk** — `dotnet test` adapter for Expecto (tests project only).

## Commands

```bash
# Restore tools + paket deps (build.sh does this automatically)
dotnet tool restore
dotnet tool run paket restore

# Default build chain (Clean -> AssemblyInfo -> Build -> Lint -> Tests -> Release)
./build.sh

# Build only
./build.sh build

# Run tests
./build.sh -t tests

# Lint
./build.sh -t lint

# Publish to NuGet (used by CI on tag; skips lint validation)
./build.sh -t publish no-lint
```

## Project Structure

```
build.sh                         # Entry point: restores tools/paket, runs FAKE
paket.dependencies               # Paket manifest (root + Build group)
fsharplint.json                  # FSharpLint config (genericTypesNames disabled)
CHANGELOG.md                     # Release notes (Unreleased on top, [**BC**] markers)
build/                           # FAKE build project (targets, specs, helpers)
src/Alma.ServiceIdentification/
  ServiceIdentification.fsproj   # Library project (PackageId, Version, Fable Content)
  Utils.fs                       # internal helpers: String, Regex, SimpleType.parseStrict
  Types.fs                       # All type/error/pattern definitions
  SimpleTypes.fs                 # Modules for Domain/Context/Purpose/Version/Zone/Bucket
  ComposedTypes.fs               # Service/Processor/Instance/Spot modules + Matching
  BoxTypes.fs                    # Box/BoxPattern modules, isMatching, (|Matching|_|)
  Create.fs                      # Create static factory (overloaded entry point)
tests/
  MatchingServiceIdentification.fs  # Matching tests
  Create.fs                      # Create factory tests
  Tests.fs                       # Test entry/registration (Expecto)
.github/workflows/               # CI: pr-check, tests, publish
```

## Architecture

The library models a six-part identifier hierarchy (domain/context → service; +purpose → processor; +version → instance; zone/bucket → spot; all → box). Core components:

1. **Simple types** (`Types.fs`, `SimpleTypes.fs`) — single-case DUs over `string` (`Domain`, `Context`, `Purpose`, `Version`, `Zone`, `Bucket`), each regex-validated via a `[<Literal>] Pattern`.
2. **Error types** (`Types.fs`) — per-part `[<RequireQualifiedAccess>]` error DUs (`Empty` | `InvalidFormat of string`) composed into part-error lists for each composed type.
3. **Pattern types** — `PurposePattern` / `VersionPattern` / `ZonePattern` / `BucketPattern`, each `| value | Any` (wildcard `*`).
4. **Composed types** — records `Service`, `Processor`, `Instance`, `Spot`, `Box`, and the pattern record `BoxPattern`.
5. **`ServiceIdentification`** — DU unifying depths: `ByService | ByProcessor | ByInstance`.
6. **`Create`** (`Create.fs`) — overloaded static factory giving one entry point (`Create.Service`, `Create.Box`, …) accepting mixed string/typed arguments.
7. **Matching** — `Box.isMatching` and the `(|Matching|_|)` active pattern (`BoxTypes.fs`).

### Pattern: Result-based parsing via a shared combinator

Every simple type parses through `SimpleType.parseStrict` (`Utils.fs`), which pairs a `[<Literal>]` regex with `Ok` / `Empty` / `InvalidFormat` constructors. Composed types aggregate these into structured `Result<_, _Error>` values rather than throwing, so failures carry per-part detail.

## Conventions

- Every per-type module uses `[<RequireQualifiedAccess>]`; the `SimpleTypes` and `Utils` modules use `[<AutoOpen>]`.
- Internal helpers (`String`, `Regexp`, `SimpleType`, `Pattern`) are marked `internal`; `InternalsVisibleTo "tests"` is emitted in AssemblyInfo.
- Regex validation strings are `[<Literal>] Pattern` constants per type.
- Parsing returns `Result<_, _Error>`; errors are structured DUs carrying the offending value — never strings.
- Naming mirrors the type (`Domain.value`, `Domain.map`, `Domain.lower`, `*.parseStrict`).
- FSharpLint runs on all `.fsproj` incl. `build/build.fsproj`; only the `genericTypesNames` rule is disabled (`fsharplint.json`).

## CI/CD

| Workflow | Trigger | What it does |
|----------|---------|--------------|
| `tests.yaml` | `pull_request` + nightly cron (`0 3 * * *`) | Sets up .NET 10, runs `./build.sh -t tests` on ubuntu-latest |
| `pr-check.yaml` | `pull_request` | Blocks fixup commits; runs ShellCheck (ignores SC1090) |
| `publish.yaml` | push tag matching `[0-9]+.[0-9]+.[0-9]+` | Sets up .NET 10, runs `./build.sh -t publish no-lint` with the `NUGET_API_KEY` secret |

## Release Process

1. Increment `<Version>` in `src/Alma.ServiceIdentification/ServiceIdentification.fsproj`.
2. Update `CHANGELOG.md` (move items out of `## Unreleased` into a new dated version section; mark breaking changes with `[**BC**]`).
3. Commit and push a tag of the form `MAJOR.MINOR.PATCH` (e.g. `11.0.0`); the publish workflow then packs and pushes to NuGet.

## Pitfalls

- **`.fsproj` `Compile` order is significant** (F# compiles top-down): `Utils → Types → SimpleTypes → ComposedTypes → BoxTypes → Create`. Reordering breaks the build.
- **The `Content Include="*.fsproj; *.fs;" PackagePath="fable\"` item ships sources for Fable consumers** — removing it breaks Fable usage even though the .NET build still works.
- **Error DUs must stay structured** (`InvalidFormat of string` carries the bad value); flattening them to strings loses the per-part detail consumers rely on.
- **Separators are literal and type-specific** in `Create` / `Box` / `Spot` rendering (`-`, `@`, `,`, default Spot `.`). Changing them silently breaks parse/round-trip.
- **`[<RequireQualifiedAccess>]` is required** on type modules to avoid clashes with the like-named DU cases; dropping it causes ambiguity errors.
- **`fsharplint.json` disables only `genericTypesNames`** — keep that exception narrow rather than broadening lint suppression.
- **Publishing is tag-driven**: bump the `.fsproj` version *before* tagging, or the package version won't match the tag.
