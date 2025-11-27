F-Service-Identification
========================

Library for Service Identification types.

## Install

Add following into `paket.references`
```
Alma.ServiceIdentification
```

**Note**: You can also use this library in a Fable project.

## Types

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

## Development
### Requirements
- [dotnet core](https://dotnet.microsoft.com/learn/dotnet/hello-world-tutorial)

### Build
```bash
./build.sh build
```

### Tests
```bash
./build.sh -t tests
```
