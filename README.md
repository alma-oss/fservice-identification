F-Service-Identification
========================

[![NuGet](https://img.shields.io/nuget/v/Alma.ServiceIdentification.svg)](https://www.nuget.org/packages/Alma.ServiceIdentification)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Alma.ServiceIdentification.svg)](https://www.nuget.org/packages/Alma.ServiceIdentification)
[![Tests](https://github.com/alma-oss/fservice-identification/actions/workflows/tests.yaml/badge.svg)](https://github.com/alma-oss/fservice-identification/actions/workflows/tests.yaml)

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
