namespace Lmc.ServiceIdentification

type Create() =
    // Simple types
    static member Domain(value: string) = value |> Domain.parseStrict
    static member Domain(service: Service) = service.Domain
    static member Domain(processor: Processor) = processor.Domain
    static member Domain(instance: Instance) = instance.Domain
    static member Domain(box: Box) = box.Domain

    static member Context(value: string) = value |> Context.parseStrict
    static member Context(service: Service) = service.Context
    static member Context(processor: Processor) = processor.Context
    static member Context(instance: Instance) = instance.Context
    static member Context(box: Box) = box.Context

    static member Purpose(value: string) = value |> Purpose.parseStrict
    static member Purpose(processor: Processor) = processor.Purpose
    static member Purpose(instance: Instance) = instance.Purpose
    static member Purpose(box: Box) = box.Purpose

    static member Version(value: string) = value |> Version.parseStrict
    static member Version(instance: Instance) = instance.Version
    static member Version(box: Box) = box.Version

    static member Zone(value: string) = value |> Zone.parseStrict
    static member Zone(spot: Spot) = spot.Zone
    static member Zone(box: Box) = box.Zone

    static member Bucket(value: string) = value |> Bucket.parseStrict
    static member Bucket(spot: Spot) = spot.Bucket
    static member Bucket(box: Box) = box.Bucket

    // Complex types
    static member Service(service: string) = service |> Service.parseStrict "-"
    static member Service(service: string, separator: char) = service |> Service.parseStrict (string separator)
    static member Service(domain: string, context: string) = (sprintf "%s-%s" domain context) |> Service.parseStrict "-"
    static member Service(Domain domain, context: string) = (sprintf "%s-%s" domain context) |> Service.parseStrict "-"
    static member Service(domain: string, Context context) = (sprintf "%s-%s" domain context) |> Service.parseStrict "-"
    static member Service(domain, context) = Service.createFromValues domain context
    static member Service(processor: Processor) = processor |> Processor.service
    static member Service(instance: Instance) = instance |> Instance.service
    static member Service(box: Box) = box |> Box.instance |> Instance.service

    static member Processor(processor: string) = processor |> Processor.parseStrict "-"
    static member Processor(processor: string, separator: char) = processor |> Processor.parseStrict (string separator)
    static member Processor(domain: string, context: string, purpose: string) = (sprintf "%s-%s-%s" domain context purpose) |> Processor.parseStrict "-"
    static member Processor(Domain domain, context: string, purpose: string) = (sprintf "%s-%s-%s" domain context purpose) |> Processor.parseStrict "-"
    static member Processor(domain: string, Context context, purpose: string) = (sprintf "%s-%s-%s" domain context purpose) |> Processor.parseStrict "-"
    static member Processor(Domain domain, Context context, purpose: string) = (sprintf "%s-%s-%s" domain context purpose) |> Processor.parseStrict "-"
    static member Processor(domain: string, context: string, Purpose purpose) = (sprintf "%s-%s-%s" domain context purpose) |> Processor.parseStrict "-"
    static member Processor(Domain domain, context: string, Purpose purpose) = (sprintf "%s-%s-%s" domain context purpose) |> Processor.parseStrict "-"
    static member Processor(domain: string, Context context, Purpose purpose) = (sprintf "%s-%s-%s" domain context purpose) |> Processor.parseStrict "-"
    static member Processor(domain, context, purpose) = Processor.createFromValues domain context purpose
    static member Processor({ Domain = domain; Context = context }: Service, purpose) = Processor.createFromValues domain context purpose
    static member Processor({ Domain = Domain domain; Context = Context context }: Service, purpose: string) = (sprintf "%s-%s-%s" domain context purpose) |> Processor.parseStrict "-"
    static member Processor(instance: Instance) = instance |> Instance.processor
    static member Processor(box: Box) = box |> Box.instance |> Instance.processor

    static member Instance(instance: string) = instance |> Instance.parseStrict "-"
    static member Instance(instance: string, separator: char) = instance |> Instance.parseStrict (string separator)
    static member Instance(domain: string, context: string, purpose: string, version: string) = (sprintf "%s-%s-%s-%s" domain context purpose version) |> Instance.parseStrict "-"
    static member Instance(Domain domain, context: string, purpose: string, version: string) = (sprintf "%s-%s-%s-%s" domain context purpose version) |> Instance.parseStrict "-"
    static member Instance(domain: string, Context context, purpose: string, version: string) = (sprintf "%s-%s-%s-%s" domain context purpose version) |> Instance.parseStrict "-"
    static member Instance(Domain domain, Context context, purpose: string, version: string) = (sprintf "%s-%s-%s-%s" domain context purpose version) |> Instance.parseStrict "-"
    static member Instance(domain: string, context: string, Purpose purpose, version: string) = (sprintf "%s-%s-%s-%s" domain context purpose version) |> Instance.parseStrict "-"
    static member Instance(Domain domain, context: string, Purpose purpose, version: string) = (sprintf "%s-%s-%s-%s" domain context purpose version) |> Instance.parseStrict "-"
    static member Instance(domain: string, Context context, Purpose purpose, version: string) = (sprintf "%s-%s-%s-%s" domain context purpose version) |> Instance.parseStrict "-"
    static member Instance(domain: string, context: string, purpose: string, Version version) = (sprintf "%s-%s-%s-%s" domain context purpose version) |> Instance.parseStrict "-"
    static member Instance(Domain domain, context: string, purpose: string, Version version) = (sprintf "%s-%s-%s-%s" domain context purpose version) |> Instance.parseStrict "-"
    static member Instance(domain: string, Context context, purpose: string, Version version) = (sprintf "%s-%s-%s-%s" domain context purpose version) |> Instance.parseStrict "-"
    static member Instance(Domain domain, Context context, purpose: string, Version version) = (sprintf "%s-%s-%s-%s" domain context purpose version) |> Instance.parseStrict "-"
    static member Instance(domain: string, context: string, Purpose purpose, Version version) = (sprintf "%s-%s-%s-%s" domain context purpose version) |> Instance.parseStrict "-"
    static member Instance(Domain domain, context: string, Purpose purpose, Version version) = (sprintf "%s-%s-%s-%s" domain context purpose version) |> Instance.parseStrict "-"
    static member Instance(domain: string, Context context, Purpose purpose, Version version) = (sprintf "%s-%s-%s-%s" domain context purpose version) |> Instance.parseStrict "-"
    static member Instance(domain, context, purpose, version) = Instance.createFromValues domain context purpose version
    static member Instance({ Domain = Domain domain; Context = Context context }: Service, purpose: string, version: string) = (sprintf "%s-%s-%s-%s" domain context purpose version) |> Instance.parseStrict "-"
    static member Instance({ Domain = Domain domain; Context = Context context }: Service, Purpose purpose, version: string) = (sprintf "%s-%s-%s-%s" domain context purpose version) |> Instance.parseStrict "-"
    static member Instance({ Domain = Domain domain; Context = Context context }: Service, purpose: string, Version version) = (sprintf "%s-%s-%s-%s" domain context purpose version) |> Instance.parseStrict "-"
    static member Instance({ Domain = domain; Context = context }: Service, purpose, version) = Instance.createFromValues domain context purpose version
    static member Instance({ Domain = domain; Context = context; Purpose = purpose }: Processor, version) = Instance.createFromValues domain context purpose version
    static member Instance({ Domain = Domain domain; Context = Context context; Purpose = Purpose purpose }: Processor, version: string) = (sprintf "%s-%s-%s-%s" domain context purpose version) |> Instance.parseStrict "-"
    static member Instance(box: Box) = box |> Box.instance

    static member Spot(spot: string) = spot |> Spot.parseStrict "."
    static member Spot(spot: string, separator: char) = spot |> Spot.parseStrict (string separator)
    static member Spot(zone: string, bucket: string) = (sprintf "%s,%s" zone bucket) |> Spot.parseStrict ","
    static member Spot(Zone zone, bucket: string) = (sprintf "%s,%s" zone bucket) |> Spot.parseStrict ","
    static member Spot(zone: string, Bucket bucket) = (sprintf "%s,%s" zone bucket) |> Spot.parseStrict ","
    static member Spot(Zone zone, Bucket bucket) = (sprintf "%s,%s" zone bucket) |> Spot.parseStrict ","
    static member Spot(box: Box) = box |> Box.spot

    static member Box(box: string) = box |> Box.parseStrict "-"
    static member Box(box: string, separator: char) = box |> Box.parseStrict (string separator)
    // ... + zone, bucket
    static member Box(domain: string, context: string, purpose: string, version: string, zone: string, bucket: string) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box(Domain domain, context: string, purpose: string, version: string, zone: string, bucket: string) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box(domain: string, Context context, purpose: string, version: string, zone: string, bucket: string) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box(Domain domain, Context context, purpose: string, version: string, zone: string, bucket: string) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box(domain: string, context: string, Purpose purpose, version: string, zone: string, bucket: string) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box(Domain domain, context: string, Purpose purpose, version: string, zone: string, bucket: string) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box(domain: string, Context context, Purpose purpose, version: string, zone: string, bucket: string) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box(domain: string, context: string, purpose: string, Version version, zone: string, bucket: string) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box(Domain domain, context: string, purpose: string, Version version, zone: string, bucket: string) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box(domain: string, Context context, purpose: string, Version version, zone: string, bucket: string) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box(Domain domain, Context context, purpose: string, Version version, zone: string, bucket: string) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box(domain: string, context: string, Purpose purpose, Version version, zone: string, bucket: string) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box(Domain domain, context: string, Purpose purpose, Version version, zone: string, bucket: string) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box(domain: string, Context context, Purpose purpose, Version version, zone: string, bucket: string) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box(domain: string, context: string, purpose: string, version: string, Zone zone, bucket: string) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box(Domain domain, context: string, purpose: string, version: string, Zone zone, bucket: string) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box(domain: string, Context context, purpose: string, version: string, Zone zone, bucket: string) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box(Domain domain, Context context, purpose: string, version: string, Zone zone, bucket: string) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box(domain: string, context: string, Purpose purpose, version: string, Zone zone, bucket: string) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box(Domain domain, context: string, Purpose purpose, version: string, Zone zone, bucket: string) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box(domain: string, Context context, Purpose purpose, version: string, Zone zone, bucket: string) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box(domain: string, context: string, purpose: string, Version version, Zone zone, bucket: string) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box(Domain domain, context: string, purpose: string, Version version, Zone zone, bucket: string) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box(domain: string, Context context, purpose: string, Version version, Zone zone, bucket: string) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box(Domain domain, Context context, purpose: string, Version version, Zone zone, bucket: string) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box(domain: string, context: string, Purpose purpose, Version version, Zone zone, bucket: string) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box(Domain domain, context: string, Purpose purpose, Version version, Zone zone, bucket: string) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box(domain: string, Context context, Purpose purpose, Version version, Zone zone, bucket: string) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box(domain: string, context: string, purpose: string, version: string, zone: string, Bucket bucket) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box(Domain domain, context: string, purpose: string, version: string, zone: string, Bucket bucket) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box(domain: string, Context context, purpose: string, version: string, zone: string, Bucket bucket) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box(Domain domain, Context context, purpose: string, version: string, zone: string, Bucket bucket) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box(domain: string, context: string, Purpose purpose, version: string, zone: string, Bucket bucket) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box(Domain domain, context: string, Purpose purpose, version: string, zone: string, Bucket bucket) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box(domain: string, Context context, Purpose purpose, version: string, zone: string, Bucket bucket) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box(domain: string, context: string, purpose: string, Version version, zone: string, Bucket bucket) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box(Domain domain, context: string, purpose: string, Version version, zone: string, Bucket bucket) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box(domain: string, Context context, purpose: string, Version version, zone: string, Bucket bucket) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box(Domain domain, Context context, purpose: string, Version version, zone: string, Bucket bucket) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box(domain: string, context: string, Purpose purpose, Version version, zone: string, Bucket bucket) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box(Domain domain, context: string, Purpose purpose, Version version, zone: string, Bucket bucket) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box(domain: string, Context context, Purpose purpose, Version version, zone: string, Bucket bucket) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box(domain: string, context: string, purpose: string, version: string, Zone zone, Bucket bucket) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box(Domain domain, context: string, purpose: string, version: string, Zone zone, Bucket bucket) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box(domain: string, Context context, purpose: string, version: string, Zone zone, Bucket bucket) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box(Domain domain, Context context, purpose: string, version: string, Zone zone, Bucket bucket) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box(domain: string, context: string, Purpose purpose, version: string, Zone zone, Bucket bucket) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box(Domain domain, context: string, Purpose purpose, version: string, Zone zone, Bucket bucket) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box(domain: string, Context context, Purpose purpose, version: string, Zone zone, Bucket bucket) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box(domain: string, context: string, purpose: string, Version version, Zone zone, Bucket bucket) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box(Domain domain, context: string, purpose: string, Version version, Zone zone, Bucket bucket) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box(domain: string, Context context, purpose: string, Version version, Zone zone, Bucket bucket) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box(Domain domain, Context context, purpose: string, Version version, Zone zone, Bucket bucket) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box(domain: string, context: string, Purpose purpose, Version version, Zone zone, Bucket bucket) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box(Domain domain, context: string, Purpose purpose, Version version, Zone zone, Bucket bucket) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box(domain: string, Context context, Purpose purpose, Version version, Zone zone, Bucket bucket) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box(domain, context, purpose, version, zone, bucket) = Box.createFromValues domain context purpose version zone bucket
    // service + ... + zone, bucket
    static member Box({ Domain = Domain domain; Context = Context context }: Service, purpose: string, version: string, zone: string, bucket: string) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box({ Domain = Domain domain; Context = Context context }: Service, Purpose purpose, version: string, zone: string, bucket: string) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box({ Domain = Domain domain; Context = Context context }: Service, purpose: string, Version version, zone: string, bucket: string) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box({ Domain = Domain domain; Context = Context context }: Service, purpose: string, version: string, Zone zone, bucket: string) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box({ Domain = Domain domain; Context = Context context }: Service, Purpose purpose, version: string, Zone zone, bucket: string) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box({ Domain = Domain domain; Context = Context context }: Service, purpose: string, Version version, Zone zone, bucket: string) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box({ Domain = Domain domain; Context = Context context }: Service, purpose: string, version: string, zone: string, Bucket bucket) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box({ Domain = Domain domain; Context = Context context }: Service, Purpose purpose, version: string, zone: string, Bucket bucket) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box({ Domain = Domain domain; Context = Context context }: Service, purpose: string, Version version, zone: string, Bucket bucket) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box({ Domain = Domain domain; Context = Context context }: Service, purpose: string, version: string, Zone zone, Bucket bucket) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box({ Domain = Domain domain; Context = Context context }: Service, Purpose purpose, version: string, Zone zone, Bucket bucket) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box({ Domain = Domain domain; Context = Context context }: Service, purpose: string, Version version, Zone zone, Bucket bucket) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box({ Domain = domain; Context = context }: Service, purpose, version, zone, bucket) = Box.createFromValues domain context purpose version zone bucket
    // procesor + ... + zone, bucket
    static member Box({ Domain = Domain domain; Context = Context context; Purpose = Purpose purpose }: Processor, version: string, zone: string, bucket: string) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box({ Domain = Domain domain; Context = Context context; Purpose = Purpose purpose }: Processor, Version version, zone: string, bucket: string) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box({ Domain = Domain domain; Context = Context context; Purpose = Purpose purpose }: Processor, version: string, Zone zone, bucket: string) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box({ Domain = Domain domain; Context = Context context; Purpose = Purpose purpose }: Processor, Version version, Zone zone, bucket: string) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box({ Domain = Domain domain; Context = Context context; Purpose = Purpose purpose }: Processor, version: string, zone: string, Bucket bucket) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box({ Domain = Domain domain; Context = Context context; Purpose = Purpose purpose }: Processor, Version version, zone: string, Bucket bucket) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box({ Domain = Domain domain; Context = Context context; Purpose = Purpose purpose }: Processor, version: string, Zone zone, Bucket bucket) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box({ Domain = domain; Context = context; Purpose = purpose }: Processor, version, zone, bucket) = Box.createFromValues domain context purpose version zone bucket
    // instance + zone, bucket
    static member Box({ Domain = Domain domain; Context = Context context; Purpose = Purpose purpose; Version = Version version }: Instance, zone: string, bucket: string) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box({ Domain = Domain domain; Context = Context context; Purpose = Purpose purpose; Version = Version version }: Instance, Zone zone, bucket: string) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box({ Domain = Domain domain; Context = Context context; Purpose = Purpose purpose; Version = Version version }: Instance, zone: string, Bucket bucket) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box({ Domain = domain; Context = context; Purpose = purpose; Version = version }: Instance, zone, bucket) = Box.createFromValues domain context purpose version zone bucket
    // ... + spot
    static member Box(domain: string, context: string, purpose: string, version: string, { Zone = Zone zone; Bucket = Bucket bucket }: Spot) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box(Domain domain, context: string, purpose: string, version: string, { Zone = Zone zone; Bucket = Bucket bucket }: Spot) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box(domain: string, Context context, purpose: string, version: string, { Zone = Zone zone; Bucket = Bucket bucket }: Spot) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box(Domain domain, Context context, purpose: string, version: string, { Zone = Zone zone; Bucket = Bucket bucket }: Spot) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box(domain: string, context: string, Purpose purpose, version: string, { Zone = Zone zone; Bucket = Bucket bucket }: Spot) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box(Domain domain, context: string, Purpose purpose, version: string, { Zone = Zone zone; Bucket = Bucket bucket }: Spot) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box(domain: string, Context context, Purpose purpose, version: string, { Zone = Zone zone; Bucket = Bucket bucket }: Spot) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box(Domain domain, Context context, Purpose purpose, version: string, { Zone = Zone zone; Bucket = Bucket bucket }: Spot) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box(domain: string, context: string, purpose: string, Version version, { Zone = Zone zone; Bucket = Bucket bucket }: Spot) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box(Domain domain, context: string, purpose: string, Version version, { Zone = Zone zone; Bucket = Bucket bucket }: Spot) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box(domain: string, Context context, purpose: string, Version version, { Zone = Zone zone; Bucket = Bucket bucket }: Spot) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box(Domain domain, Context context, purpose: string, Version version, { Zone = Zone zone; Bucket = Bucket bucket }: Spot) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box(domain: string, context: string, Purpose purpose, Version version, { Zone = Zone zone; Bucket = Bucket bucket }: Spot) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box(Domain domain, context: string, Purpose purpose, Version version, { Zone = Zone zone; Bucket = Bucket bucket }: Spot) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box(domain: string, Context context, Purpose purpose, Version version, { Zone = Zone zone; Bucket = Bucket bucket }: Spot) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box(domain, context, purpose, version, { Zone = zone; Bucket = bucket }: Spot) = Box.createFromValues domain context purpose version zone bucket
    // service + ... + spot
    static member Box({ Domain = Domain domain; Context = Context context }: Service, purpose: string, version: string, { Zone = Zone zone; Bucket = Bucket bucket }: Spot) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box({ Domain = Domain domain; Context = Context context }: Service, Purpose purpose, version: string, { Zone = Zone zone; Bucket = Bucket bucket }: Spot) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box({ Domain = Domain domain; Context = Context context }: Service, purpose: string, Version version, { Zone = Zone zone; Bucket = Bucket bucket }: Spot) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box({ Domain = domain; Context = context }: Service, purpose, version, { Zone = zone; Bucket = bucket }: Spot) = Box.createFromValues domain context purpose version zone bucket
    // processor + ... + spot
    static member Box({ Domain = Domain domain; Context = Context context; Purpose = Purpose purpose }: Processor, version: string, { Zone = Zone zone; Bucket = Bucket bucket }: Spot) = (sprintf "%s-%s@%s-%s-%s-%s" zone bucket domain context purpose version) |> Box.parseStrict "-"
    static member Box({ Domain = domain; Context = context; Purpose = purpose }: Processor, version, { Zone = zone; Bucket = bucket }: Spot) = Box.createFromValues domain context purpose version zone bucket
    // instance + spot
    static member Box(instance: Instance, spot: Spot) = Box.ofInstance instance spot.Zone spot.Bucket
