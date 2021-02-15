namespace Lmc.ServiceIdentification

[<RequireQualifiedAccess>]
module Domain =
    let [<Literal>] Pattern = @"^([a-zA-Z]+)$"
    let parseStrict = SimpleType.parseStrict Pattern Domain DomainError.Empty DomainError.InvalidFormat

    let value (Domain domain) = domain
    let map = SimpleType.map value Domain
    let lower = map String.toLower

[<RequireQualifiedAccess>]
module Context =
    let [<Literal>] Pattern = @"^([a-zA-Z]+)$"
    let parseStrict = SimpleType.parseStrict Pattern Context ContextError.Empty ContextError.InvalidFormat

    let value (Context context) = context
    let map = SimpleType.map value Context
    let lower = map String.toLower

[<RequireQualifiedAccess>]
module Purpose =
    let [<Literal>] Pattern = @"^([a-zA-Z]+)$"
    let parseStrict = SimpleType.parseStrict Pattern Purpose PurposeError.Empty PurposeError.InvalidFormat

    let value (Purpose purpose) = purpose
    let map = SimpleType.map value Purpose
    let lower = map String.toLower

[<RequireQualifiedAccess>]
module PurposePattern =
    let value = function
        | PurposePattern.Purpose purpose -> purpose |> Purpose.value
        | PurposePattern.Any -> Any

[<RequireQualifiedAccess>]
module Version =
    let [<Literal>] Pattern = @"^([a-zA-Z]+[a-zA-Z0-9]*)$"
    let parseStrict = SimpleType.parseStrict Pattern Version VersionError.Empty VersionError.InvalidFormat

    let value (Version version) = version
    let map = SimpleType.map value Version
    let lower = map String.toLower

[<RequireQualifiedAccess>]
module VersionPattern =
    let value = function
        | VersionPattern.Version version -> version |> Version.value
        | VersionPattern.Any -> Any

[<RequireQualifiedAccess>]
module Zone =
    let [<Literal>] Pattern = @"^([a-zA-Z]+)$"
    let parseStrict = SimpleType.parseStrict Pattern Zone ZoneError.Empty ZoneError.InvalidFormat

    let value (Zone zone) = zone
    let map = SimpleType.map value Zone
    let lower = map String.toLower

[<RequireQualifiedAccess>]
module ZonePattern =
    let value = function
        | ZonePattern.Zone zone -> zone |> Zone.value
        | ZonePattern.Any -> Any

[<RequireQualifiedAccess>]
module Bucket =
    let [<Literal>] Pattern = @"^([a-zA-Z]+)$"
    let parseStrict = SimpleType.parseStrict Pattern Bucket BucketError.Empty BucketError.InvalidFormat

    let value (Bucket bucket) = bucket
    let map = SimpleType.map value Bucket
    let lower = map String.toLower

[<RequireQualifiedAccess>]
module BucketPattern =
    let value = function
        | BucketPattern.Bucket bucket -> bucket |> Bucket.value
        | BucketPattern.Any -> Any
