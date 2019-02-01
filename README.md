F-Service-Identification
========================

Library for Service Identification types.

## Types
_based on https://stash.int.lmc.cz/projects/ARCHI/repos/service-mesh/browse _

Hierarchy:
```
domain ─┐─────────┐───────────┐──────────┐
context ┘ service │           │          │
purpose ──────────┘ processor │          │
version ──────────────────────┘ instance │ box
zone ───┐ spot                           │
bucket ─┘────────────────────────────────┘
```

Tree like hierarchy:
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

## Release
1. Increment version in `src/TODO.fsproj`
2. Run `$ fake build target release`
3. Move TODO package (`TODO.VERSION.nupkg`) from `./release` dir to the NugetServer packages dir
4. Update `CHANGELOG.md`

## Development
### Requirements
- [dotnet core](https://dotnet.microsoft.com/learn/dotnet/hello-world-tutorial)
- [FAKE](https://fake.build/fake-gettingstarted.html)

### Build
```bash
fake build
```

### Watch
```bash
fake build target watch
```
