namespace Lmc.ServiceIdentification

[<AutoOpen>]
module internal Matching =
    let (|IsDomain|_|) = function
        | String.IsEmpty | Any -> None
        | domain -> Some (IsDomain (Domain domain))

    let (|IsContext|_|) = function
        | String.IsEmpty | Any -> None
        | context -> Some (IsContext (Context context))

    let (|IsPurpose|_|) = function
        | String.IsEmpty | Any -> None
        | purpose -> Some (IsPurpose (Purpose purpose))

    let (|IsVersion|_|) = function
        | String.IsEmpty | Any -> None
        | version -> Some (IsVersion (Version version))

    let (|IsZone|_|) = function
        | String.IsEmpty | Any -> None
        | zone -> Some (IsZone (Zone zone))

    let (|IsBucket|_|) = function
        | String.IsEmpty | Any -> None
        | bucket -> Some (IsBucket (Bucket bucket))

[<RequireQualifiedAccess>]
module Service =
    let parse (separator: string) (serviceString: string) =
        match serviceString.Split(separator) with
        | [| IsDomain domain; IsContext context |] ->
            Some {
                Domain = domain
                Context = context
            }
        | _ -> None

    let createFromValues domain context: Service =
        {
            Domain = domain
            Context = context
        }

    let createFromStrings = function
        | IsDomain domain, IsContext context -> Some (createFromValues domain context)
        | _ -> None

    let concat separator (service: Service) =
        [
            service.Domain |> Domain.value
            service.Context |> Context.value
        ]
        |> String.concat separator

    let domain ({ Domain = domain }: Service) = domain
    let context ({ Context = context }: Service) = context

    let lower (service: Service) =
        {
            Domain = service.Domain |> Domain.lower
            Context = service.Context |> Context.lower
        }

[<RequireQualifiedAccess>]
module Processor =
    let parse (separator: string) (processorString: string): Processor option =
        match processorString.Split(separator) with
        | [| IsDomain domain; IsContext context; IsPurpose purpose |] ->
            Some {
                Domain = domain
                Context = context
                Purpose = purpose
            }
        | _ -> None

    let createFromValues domain context purpose: Processor =
        {
            Domain = domain
            Context = context
            Purpose = purpose
        }

    let createFromStrings = function
        | IsDomain domain, IsContext context, IsPurpose purpose -> Some (createFromValues domain context purpose)
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

    let lower (processor: Processor) =
        {
            Domain = processor.Domain |> Domain.lower
            Context = processor.Context |> Context.lower
            Purpose = processor.Purpose |> Purpose.lower
        }

[<RequireQualifiedAccess>]
module Instance =
    let parse (separator: string) (instanceString: string) =
        match instanceString.Split(separator) with
        | [| IsDomain domain; IsContext context; IsPurpose purpose; IsVersion version |] ->
            Some {
                Domain = domain
                Context = context
                Purpose = purpose
                Version = version
            }
        | _ -> None

    let createFromValues domain context purpose version: Instance =
        {
            Domain = domain
            Context = context
            Purpose = purpose
            Version = version
        }

    let createFromStrings = function
        | IsDomain domain, IsContext context, IsPurpose purpose, IsVersion version -> Some (createFromValues domain context purpose version)
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

    let lower (instance: Instance) =
        {
            Domain = instance.Domain |> Domain.lower
            Context = instance.Context |> Context.lower
            Purpose = instance.Purpose |> Purpose.lower
            Version = instance.Version |> Version.lower
        }

[<RequireQualifiedAccess>]
module Spot =
    let parse (separator: string) (spotString: string) =
        match spotString.Split(separator) with
        | [| IsZone zone; IsBucket bucket |] ->
            Some {
                Zone = zone
                Bucket = bucket
            }
        | _ -> None

    let createFromValues zone bucket: Spot =
        {
            Zone = zone
            Bucket = bucket
        }

    let createFromStrings = function
        | IsZone zone, IsBucket bucket -> Some (createFromValues zone bucket)
        | _ -> None

    let zone ({ Zone = zone }: Spot) = zone
    let bucket ({ Bucket = bucket }: Spot) = bucket

    let lower (spot: Spot) =
        {
            Zone = spot.Zone |> Zone.lower
            Bucket = spot.Bucket |> Bucket.lower
        }

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

    let lower = function
        | ByService service -> service |> Service.lower |> ByService
        | ByProcessor processor -> processor |> Processor.lower |> ByProcessor
        | ByInstance instance -> instance |> Instance.lower |> ByInstance
