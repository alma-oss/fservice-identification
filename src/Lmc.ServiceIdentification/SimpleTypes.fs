namespace Lmc.ServiceIdentification

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
        | PurposePattern.Any -> Any

[<RequireQualifiedAccess>]
module Version =
    let value (Version version) = version

[<RequireQualifiedAccess>]
module VersionPattern =
    let value = function
        | VersionPattern.Version version -> version |> Version.value
        | VersionPattern.Any -> Any

[<RequireQualifiedAccess>]
module Zone =
    let value (Zone zone) = zone

[<RequireQualifiedAccess>]
module ZonePattern =
    let value = function
        | ZonePattern.Zone zone -> zone |> Zone.value
        | ZonePattern.Any -> Any

[<RequireQualifiedAccess>]
module Bucket =
    let value (Bucket bucket) = bucket

[<RequireQualifiedAccess>]
module BucketPattern =
    let value = function
        | BucketPattern.Bucket bucket -> bucket |> Bucket.value
        | BucketPattern.Any -> Any
