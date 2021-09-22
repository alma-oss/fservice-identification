namespace Lmc.ServiceIdentification

[<AutoOpen>]
module BoxTypes =

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

        let createFromStrings = function
            | IsDomain domain, IsContext context, IsPurpose purpose, IsVersion version, IsZone zone, IsBucket bucket -> Some (createFromValues domain context purpose version zone bucket)
            | _ -> None

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

        let parseStrict (separator: string) (value: string) =
            match value.Split "@" |> Seq.toList with
            | [ spot; instance ] ->
                let instance = instance |> Instance.parseStrict separator
                let spot = spot |> Spot.parseStrict separator

                match instance, spot with
                | Ok instance, Ok spot -> ofInstance instance spot.Zone spot.Bucket |> Ok
                | Error instanceError, Error spotError -> Error (BoxError.InstanceAndSpotError (instanceError, spotError))
                | Error instanceError, _ -> Error (BoxError.InstanceError instanceError)
                | _, Error spotError -> Error (BoxError.SpotError spotError)
            | _ -> Error (BoxError.InvalidFormat value)

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

        let lower (box: Box): Box =
            {
                Domain = box.Domain |> Domain.lower
                Context = box.Context |> Context.lower
                Purpose = box.Purpose |> Purpose.lower
                Version = box.Version |> Version.lower
                Zone = box.Zone |> Zone.lower
                Bucket = box.Bucket |> Bucket.lower
            }

        let value (box: Box) =
            let spot = box |> spot |> Spot.concat "-"
            let instance = box |> instance |> Instance.concat "-"
            $"{spot}@{instance}"

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

        let createFromValues domain context purpose version zone bucket =
            {
                Domain = domain
                Context = context
                Purpose = purpose
                Version = version
                Zone = zone
                Bucket = bucket
            }

        let createFromStrings strings =
            let ofService domain context = Service.createFromValues domain context |> ofService |> Some
            let ofProcessor domain context purpose = Processor.createFromValues domain context purpose |> ofProcessor |> Some
            let ofInstance domain context purpose version = Instance.createFromValues domain context purpose version |> ofInstance |> Some

            let withZone zone = Option.map (fun (boxPattern: BoxPattern) -> { boxPattern with Zone = ZonePattern.Zone zone })
            let withBucket bucket = Option.map (fun (boxPattern: BoxPattern) -> { boxPattern with Bucket = BucketPattern.Bucket bucket })
            let withSpot zone bucket = withZone zone >> withBucket bucket

            match strings with
            | IsDomain domain, IsContext context, Any,               Any,               Any,         Any             -> ofService domain context
            | IsDomain domain, IsContext context, IsPurpose purpose, Any,               Any,         Any             -> ofProcessor domain context purpose
            | IsDomain domain, IsContext context, IsPurpose purpose, IsVersion version, Any,         Any             -> ofInstance domain context purpose version

            | IsDomain domain, IsContext context, Any,               Any,               IsZone zone, Any             -> ofService domain context                  |> withZone zone
            | IsDomain domain, IsContext context, IsPurpose purpose, Any,               IsZone zone, Any             -> ofProcessor domain context purpose        |> withZone zone
            | IsDomain domain, IsContext context, IsPurpose purpose, IsVersion version, IsZone zone, Any             -> ofInstance domain context purpose version |> withZone zone

            | IsDomain domain, IsContext context, Any,               Any,               Any,         IsBucket bucket -> ofService domain context                  |> withBucket bucket
            | IsDomain domain, IsContext context, IsPurpose purpose, Any,               Any,         IsBucket bucket -> ofProcessor domain context purpose        |> withBucket bucket
            | IsDomain domain, IsContext context, IsPurpose purpose, IsVersion version, Any,         IsBucket bucket -> ofInstance domain context purpose version |> withBucket bucket

            | IsDomain domain, IsContext context, Any,               Any,               IsZone zone, IsBucket bucket -> ofService domain context                  |> withSpot zone bucket
            | IsDomain domain, IsContext context, IsPurpose purpose, Any,               IsZone zone, IsBucket bucket -> ofProcessor domain context purpose        |> withSpot zone bucket
            | IsDomain domain, IsContext context, IsPurpose purpose, IsVersion version, IsZone zone, IsBucket bucket -> ofInstance domain context purpose version |> withSpot zone bucket

            | _ -> None

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
