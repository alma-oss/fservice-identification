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

// Errors

[<RequireQualifiedAccess>]
type ServicePartError =
    | Domain of DomainError
    | Context of ContextError

[<RequireQualifiedAccess>]
type ServiceError =
    | InvalidFormat of string
    | ServicePart of ServicePartError list

[<RequireQualifiedAccess>]
type ProcessorPartError =
    | Domain of DomainError
    | Context of ContextError
    | Purpose of PurposeError

[<RequireQualifiedAccess>]
type ProcessorError =
    | InvalidFormat of string
    | ProcessorPart of ProcessorPartError list

[<RequireQualifiedAccess>]
type InstancePartError =
    | Domain of DomainError
    | Context of ContextError
    | Purpose of PurposeError
    | Version of VersionError

[<RequireQualifiedAccess>]
type InstanceError =
    | InvalidFormat of string
    | InstancePart of InstancePartError list

[<RequireQualifiedAccess>]
type SpotPartError =
    | Zone of ZoneError
    | Bucket of BucketError

[<RequireQualifiedAccess>]
type SpotError =
    | InvalidFormat of string
    | SpotPart of SpotPartError list

[<RequireQualifiedAccess>]
type ServiceIdentificationError =
    | InvalidFormat of string
    | ServiceError of ServiceError
    | ProcessorError of ProcessorError
    | InstanceError of InstanceError
