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
module Version =
    let value (Version version) = version

[<RequireQualifiedAccess>]
module Zone =
    let value (Zone zone) = zone

[<RequireQualifiedAccess>]
module Bucket =
    let value (Bucket bucket) = bucket

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

    let service (processor: Processor) =
        {
            Domain = processor.Domain
            Context = processor.Context
        }

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

[<RequireQualifiedAccess>]
module Box =
    let createFromValues domain context purpose version zone bucket =
        {
            Domain = domain
            Context = context
            Purpose = purpose
            Version = version
            Zone = zone
            Bucket = bucket
        }

    let createFromStrings domain context purpose version zone bucket =
        createFromValues
            (Domain domain)
            (Context context)
            (Purpose purpose)
            (Version version)
            (Zone zone)
            (Bucket bucket)

    let instance box =
        {
            Domain = box.Domain
            Context = box.Context
            Purpose = box.Purpose
            Version = box.Version
        }

    let spot box =
        {
            Zone = box.Zone
            Bucket = box.Bucket
        }

[<RequireQualifiedAccess>]
module ServiceIdentification =
    let parse (separator: string) (serviceIdentificationString: string) =
        match serviceIdentificationString.Split(separator) with
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
