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

[<RequireQualifiedAccessAttribute>]
module Domain =
    let value (Domain domain) = domain

[<RequireQualifiedAccessAttribute>]
module Context =
    let value (Context context) = context

[<RequireQualifiedAccessAttribute>]
module Purpose =
    let value (Purpose purpose) = purpose

[<RequireQualifiedAccessAttribute>]
module Version =
    let value (Version version) = version

[<RequireQualifiedAccessAttribute>]
module Zone =
    let value (Zone zone) = zone

[<RequireQualifiedAccessAttribute>]
module Bucket =
    let value (Bucket bucket) = bucket

[<RequireQualifiedAccessAttribute>]
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

[<RequireQualifiedAccessAttribute>]
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
