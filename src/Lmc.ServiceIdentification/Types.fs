namespace Lmc.ServiceIdentification

// domain ─┐─────────┐───────────┐──────────┐
// context ┘ service │           │          │
// purpose ──────────┘ processor │          │
// version ──────────────────────┘ instance │ box
// zone ───┐ spot                           │
// bucket ─┘────────────────────────────────┘

type Domain = Domain of string
type Context = Context of string
type Purpose = Purpose of string
type Version = Version of string
type Zone = Zone of string
type Bucket = Bucket of string

// Errors

[<RequireQualifiedAccess>]
type DomainError =
    | Empty
    | InvalidFormat of string

[<RequireQualifiedAccess>]
type ContextError =
    | Empty
    | InvalidFormat of string

[<RequireQualifiedAccess>]
type PurposeError =
    | Empty
    | InvalidFormat of string

[<RequireQualifiedAccess>]
type VersionError =
    | Empty
    | InvalidFormat of string

[<RequireQualifiedAccess>]
type ZoneError =
    | Empty
    | InvalidFormat of string

[<RequireQualifiedAccess>]
type BucketError =
    | Empty
    | InvalidFormat of string

// Patterns

[<RequireQualifiedAccess>]
type PurposePattern =
    | Purpose of Purpose
    | Any

[<RequireQualifiedAccess>]
type VersionPattern =
    | Version of Version
    | Any

[<RequireQualifiedAccess>]
type ZonePattern =
    | Zone of Zone
    | Any

[<RequireQualifiedAccess>]
type BucketPattern =
    | Bucket of Bucket
    | Any

//
// Composed types
//

type Service = {
    Domain: Domain
    Context: Context
}

type Processor = {
    Domain: Domain
    Context: Context
    Purpose: Purpose
}

type Instance = {
    Domain: Domain
    Context: Context
    Purpose: Purpose
    Version: Version
}

type Spot = {
    Zone: Zone
    Bucket: Bucket
}

type Box = {
    Domain: Domain
    Context: Context
    Purpose: Purpose
    Version: Version
    Zone: Zone
    Bucket: Bucket
}

type BoxPattern = {
    Domain: Domain
    Context: Context
    Purpose: PurposePattern
    Version: VersionPattern
    Zone: ZonePattern
    Bucket: BucketPattern
}

type ServiceIdentification =
    | ByService of Service
    | ByProcessor of Processor
    | ByInstance of Instance
