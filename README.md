F-Service-Identification
========================

Library for Service Identification types.

## Install

Add following into `paket.dependencies`
```
git ssh://git@bitbucket.lmc.cz:7999/archi/nuget-server.git master Packages: /nuget/
# LMC Nuget dependencies:
nuget Lmc.ServiceIdentification
```

Add following into `paket.references`
```
Lmc.ServiceIdentification
```

**Note**: You can also use this library in a Fable project.

## Types
_based on_ https://bitbucket.lmc.cz/projects/ARCHI/repos/service-mesh/browse

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
1. Increment version in `ServiceIdentification.fsproj`
2. Update `CHANGELOG.md`
3. Commit new version and tag it
4. Run `$ fake build target release`
5. Go to `nuget-server` repo, run `faket build target copyAll` and push new versions

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
