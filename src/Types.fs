namespace ServiceIdentification

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

[<RequireQualifiedAccess>]
module Domain =
    let value (Domain domain) = domain

[<RequireQualifiedAccess>]
module Context =
    let value (Context context) = context

[<RequireQualifiedAccess>]
module Purpose =
    let value (Purpose purpose) = purpose

[<RequireQualifiedAccess>]
module PurposePattern =
    let value = function
        | PurposePattern.Purpose purpose -> purpose |> Purpose.value
        | PurposePattern.Any -> "*"

[<RequireQualifiedAccess>]
module Version =
    let value (Version version) = version

[<RequireQualifiedAccess>]
module VersionPattern =
    let value = function
        | VersionPattern.Version version -> version |> Version.value
        | VersionPattern.Any -> "*"

[<RequireQualifiedAccess>]
module Zone =
    let value (Zone zone) = zone

[<RequireQualifiedAccess>]
module ZonePattern =
    let value = function
        | ZonePattern.Zone zone -> zone |> Zone.value
        | ZonePattern.Any -> "*"

[<RequireQualifiedAccess>]
module Bucket =
    let value (Bucket bucket) = bucket

[<RequireQualifiedAccess>]
module BucketPattern =
    let value = function
        | BucketPattern.Bucket bucket -> bucket |> Bucket.value
        | BucketPattern.Any -> "*"

[<RequireQualifiedAccess>]
module Service =
    let parse (separator: string) (serviceString: string) =
        match serviceString.Split(separator) with
        | [| domain; context |] ->
            Some {
                Domain = Domain domain
                Context = Context context
            }
        | _ -> None

    let concat separator (service: Service) =
        [
            service.Domain |> Domain.value
            service.Context |> Context.value
        ]
        |> String.concat separator

    let domain ({ Domain = domain }: Service) = domain
    let context ({ Context = context }: Service) = context

[<RequireQualifiedAccess>]
module Processor =
    let parse (separator: string) (processorString: string) =
        match processorString.Split(separator) with
        | [| domain; context; purpose |] ->
            Some {
                Domain = Domain domain
                Context = Context context
                Purpose = Purpose purpose
            }
        | _ -> None

    let ofService (service: Service) purpose =
        {
            Domain = service.Domain
            Context = service.Context
            Purpose = purpose
        }

    let service (processor: Processor) =
        {
            Domain = processor.Domain
            Context = processor.Context
        }

    let purpose ({ Purpose = purpose }: Processor) = purpose

    let concat separator (processor: Processor) =
        [
            processor.Domain |> Domain.value
            processor.Context |> Context.value
            processor.Purpose |> Purpose.value
        ]
        |> String.concat separator

[<RequireQualifiedAccess>]
module Instance =
    let parse (separator: string) (instanceString: string) =
        match instanceString.Split(separator) with
        | [| domain; context; purpose; version |] ->
            Some {
                Domain = Domain domain
                Context = Context context
                Purpose = Purpose purpose
                Version = Version version
            }
        | _ -> None

    let ofService (service: Service) purpose version =
        {
            Domain = service.Domain
            Context = service.Context
            Purpose = purpose
            Version = version
        }

    let ofProcessor (processor: Processor) version =
        {
            Domain = processor.Domain
            Context = processor.Context
            Purpose = processor.Purpose
            Version = version
        }

    let concat separator (instance: Instance) =
        [
            instance.Domain |> Domain.value
            instance.Context |> Context.value
            instance.Purpose |> Purpose.value
            instance.Version |> Version.value
        ]
        |> String.concat separator

    let service (instance: Instance) =
        {
            Domain = instance.Domain
            Context = instance.Context
        }

    let processor (instance: Instance) =
        {
            Domain = instance.Domain
            Context = instance.Context
            Purpose = instance.Purpose
        }

    let version ({ Version = version }: Instance) = version

[<RequireQualifiedAccess>]
module Spot =
    let parse (separator: string) (spotString: string) =
        match spotString.Split(separator) with
        | [| zone; bucket |] ->
            Some {
                Zone = Zone zone
                Bucket = Bucket bucket
            }
        | _ -> None

    let zone ({ Zone = zone }: Spot) = zone
    let bucket ({ Bucket = bucket }: Spot) = bucket

[<RequireQualifiedAccess>]
module Box =
    let createFromValues domain context purpose version zone bucket: Box =
        {
            Domain = domain
            Context = context
            Purpose = purpose
            Version = version
            Zone = zone
            Bucket = bucket
        }

    let createFromStrings domain context purpose version zone bucket: Box =
        createFromValues
            (Domain domain)
            (Context context)
            (Purpose purpose)
            (Version version)
            (Zone zone)
            (Bucket bucket)

    let ofService (service: Service) purpose version zone bucket: Box =
        {
            Domain = service.Domain
            Context = service.Context
            Purpose = purpose
            Version = version
            Zone = zone
            Bucket = bucket
        }

    let ofProcessor (processor: Processor) version zone bucket: Box =
        {
            Domain = processor.Domain
            Context = processor.Context
            Purpose = processor.Purpose
            Version = version
            Zone = zone
            Bucket = bucket
        }

    let ofInstance (instance: Instance) zone bucket: Box =
        {
            Domain = instance.Domain
            Context = instance.Context
            Purpose = instance.Purpose
            Version = instance.Version
            Zone = zone
            Bucket = bucket
        }

    let instance (box: Box) =
        {
            Domain = box.Domain
            Context = box.Context
            Purpose = box.Purpose
            Version = box.Version
        }

    let spot (box: Box) =
        {
            Zone = box.Zone
            Bucket = box.Bucket
        }

[<RequireQualifiedAccess>]
module BoxPattern =
    let ofService (service: Service) =
        {
            Domain = service.Domain
            Context = service.Context
            Purpose = PurposePattern.Any
            Version = VersionPattern.Any
            Zone = ZonePattern.Any
            Bucket = BucketPattern.Any
        }

    let ofProcessor (processor: Processor) =
        {
            Domain = processor.Domain
            Context = processor.Context
            Purpose = PurposePattern.Purpose processor.Purpose
            Version = VersionPattern.Any
            Zone = ZonePattern.Any
            Bucket = BucketPattern.Any
        }

    let ofInstance (instance: Instance) =
        {
            Domain = instance.Domain
            Context = instance.Context
            Purpose = PurposePattern.Purpose instance.Purpose
            Version = VersionPattern.Version instance.Version
            Zone = ZonePattern.Any
            Bucket = BucketPattern.Any
        }

    let ofBox (box: Box) =
        {
            Domain = box.Domain
            Context = box.Context
            Purpose = PurposePattern.Purpose box.Purpose
            Version = VersionPattern.Version box.Version
            Zone = ZonePattern.Zone box.Zone
            Bucket = BucketPattern.Bucket box.Bucket
        }

    let ofServiceIdentification = function
        | ByService service -> ofService service
        | ByProcessor processor -> ofProcessor processor
        | ByInstance instance -> ofInstance instance

    let isMatching ({ Domain = domain; Context = context; Purpose = purpose; Version = version; Zone = zone; Bucket = bucket }: Box): BoxPattern -> bool = function
        | { Domain = d; Context = c; Purpose = PurposePattern.Purpose p; Version = VersionPattern.Version v; Zone = ZonePattern.Zone z; Bucket = BucketPattern.Bucket b }   // instance + spot (box)
            when d = domain && c = context && p = purpose && v = version && z = zone && b = bucket -> true
        | { Domain = d; Context = c; Purpose = PurposePattern.Purpose p; Version = VersionPattern.Version v; Zone = ZonePattern.Zone z; Bucket = BucketPattern.Any }        // instance + zone
            when d = domain && c = context && p = purpose && v = version && z = zone -> true
        | { Domain = d; Context = c; Purpose = PurposePattern.Purpose p; Version = VersionPattern.Version v; Zone = ZonePattern.Any; Bucket = BucketPattern.Bucket b }      // instance + bucket
            when d = domain && c = context && p = purpose && v = version && b = bucket -> true
        | { Domain = d; Context = c; Purpose = PurposePattern.Purpose p; Version = VersionPattern.Version v; Zone = ZonePattern.Any; Bucket = BucketPattern.Any }           // instance
            when d = domain && c = context && p = purpose && v = version -> true
        | { Domain = d; Context = c; Purpose = PurposePattern.Purpose p; Version = VersionPattern.Any; Zone = ZonePattern.Zone z; Bucket = BucketPattern.Bucket b }         // processor + spot
            when d = domain && c = context && p = purpose && z = zone && b = bucket -> true
        | { Domain = d; Context = c; Purpose = PurposePattern.Purpose p; Version = VersionPattern.Any; Zone = ZonePattern.Zone z; Bucket = BucketPattern.Any }              // processor + zone
            when d = domain && c = context && p = purpose && z = zone -> true
        | { Domain = d; Context = c; Purpose = PurposePattern.Purpose p; Version = VersionPattern.Any; Zone = ZonePattern.Any; Bucket = BucketPattern.Bucket b }            // processor + bucket
            when d = domain && c = context && p = purpose && b = bucket -> true
        | { Domain = d; Context = c; Purpose = PurposePattern.Purpose p; Version = VersionPattern.Any; Zone = ZonePattern.Any; Bucket = BucketPattern.Any }                 // processor
            when d = domain && c = context && p = purpose -> true
        | { Domain = d; Context = c; Purpose = PurposePattern.Any; Version = VersionPattern.Any; Zone = ZonePattern.Zone z; Bucket = BucketPattern.Bucket b }               // service + spot
            when d = domain && c = context && z = zone && b = bucket -> true
        | { Domain = d; Context = c; Purpose = PurposePattern.Any; Version = VersionPattern.Any; Zone = ZonePattern.Zone z; Bucket = BucketPattern.Any }                    // service + zone
            when d = domain && c = context && z = zone -> true
        | { Domain = d; Context = c; Purpose = PurposePattern.Any; Version = VersionPattern.Any; Zone = ZonePattern.Any; Bucket = BucketPattern.Bucket b }                  // service + bucket
            when d = domain && c = context && b = bucket -> true
        | { Domain = d; Context = c; Purpose = PurposePattern.Any; Version = VersionPattern.Any; Zone = ZonePattern.Any; Bucket = BucketPattern.Any }                       // service
            when d = domain && c = context -> true
        | _ -> false

    let (|Matching|_|) box pattern =
        if pattern |> isMatching box then Some ()
        else None

[<RequireQualifiedAccess>]
module ServiceIdentification =
    let service = function
        | ByService service -> service
        | ByProcessor processor -> processor |> Processor.service
        | ByInstance instance -> instance |> Instance.service

    let parse (separator: string) (serviceIdentificationString: string) =
        match serviceIdentificationString.Split separator with
        | [| domain; context; purpose; version |] ->
            ByInstance {
                Domain = Domain domain
                Context = Context context
                Purpose = Purpose purpose
                Version = Version version
            }
            |> Some
        | [| domain; context; purpose |] ->
            ByProcessor {
                Domain = Domain domain
                Context = Context context
                Purpose = Purpose purpose
            }
            |> Some
        | [| domain; context |] ->
            ByService {
                Domain = Domain domain
                Context = Context context
            }
            |> Some
        | _ -> None

    let concat separator = function
        | ByService s -> s |> Service.concat separator
        | ByProcessor p -> p |> Processor.concat separator
        | ByInstance i -> i |> Instance.concat separator

    let isMatching pattern = function
        | ByInstance instance ->
            match pattern with
            | ByInstance i -> instance = i
            | ByProcessor p -> instance.Domain = p.Domain && instance.Context = p.Context && instance.Purpose = p.Purpose
            | ByService s -> instance.Domain = s.Domain && instance.Context = s.Context
        | ByProcessor processor ->
            match pattern with
            | ByInstance _ -> false
            | ByProcessor p -> processor = p
            | ByService s -> processor.Domain = s.Domain && processor.Context = s.Context
        | ByService service ->
            match pattern with
            | ByInstance _ -> false
            | ByProcessor _ -> false
            | ByService s -> service.Domain = s.Domain && service.Context = s.Context
