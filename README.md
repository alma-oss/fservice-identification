F-Service-Identification
========================

Library for Service Identification types.

## Types
_based on_ https://stash.int.lmc.cz/projects/ARCHI/repos/service-mesh/browse

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
1. Increment version in `src/ServiceIdentification.fsproj`
2. Update `CHANGELOG.md`
3. Commit new version and tag it
4. Run `$ fake build target release`

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
