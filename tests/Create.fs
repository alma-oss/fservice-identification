module Alma.ServiceIdentification.Test.Create

open Expecto
open Alma.ServiceIdentification

type SimpleTypeTest<'SimpleType> = {
    Description: string
    Expected: 'SimpleType
    Acutal: 'SimpleType
}

let testProvider provider =
    provider
    |> List.iter (fun ({Description = description; Acutal = acutal; Expected = expected }) ->
        Expect.equal acutal expected description
    )

[<Tests>]
let createServiceIdentificationTests =
    testList "Create service identification" [
        let service: Service = { Domain = Domain "domain"; Context = Context "context" }
        let processor: Processor = { Domain = Domain "domain"; Context = Context "context"; Purpose = Purpose "purpose" }
        let instance: Instance = { Domain = Domain "domain"; Context = Context "context"; Purpose = Purpose "purpose"; Version = Version "version" }
        let spot: Spot = { Zone = Zone "zone"; Bucket = Bucket "bucket" }
        let box: Box = { Domain = Domain "domain"; Context = Context "context"; Purpose = Purpose "purpose"; Version = Version "version"; Zone = Zone "zone"; Bucket = Bucket "bucket" }

        testCase "simple types" <| fun _ ->
            testProvider [
                { Description = "Domain from string"; Acutal = Create.Domain("domain"); Expected = Ok (Domain "domain") }
                { Description = "Domain from string"; Acutal = Create.Domain(""); Expected = Error DomainError.Empty }
            ]
            testProvider [
                { Description = "Context from string"; Acutal = Create.Context("context"); Expected = Ok (Context "context") }
                { Description = "Context from string"; Acutal = Create.Context(""); Expected = Error ContextError.Empty}
            ]
            testProvider [
                { Description = "Purpose from string"; Acutal = Create.Purpose("purpose"); Expected = Ok (Purpose "purpose") }
                { Description = "Purpose from string"; Acutal = Create.Purpose(""); Expected = Error PurposeError.Empty}
            ]
            testProvider [
                { Description = "Version from string"; Acutal = Create.Version("version"); Expected = Ok (Version "version") }
                { Description = "Version from string"; Acutal = Create.Version(""); Expected = Error VersionError.Empty}
            ]
            testProvider [
                { Description = "Zone from string"; Acutal = Create.Zone("zone"); Expected = Ok (Zone "zone") }
                { Description = "Zone from string"; Acutal = Create.Zone(""); Expected = Error ZoneError.Empty }
            ]
            testProvider [
                { Description = "Bucket from string"; Acutal = Create.Bucket("bucket"); Expected = Ok (Bucket "bucket") }
                { Description = "Bucket from string"; Acutal = Create.Bucket(""); Expected = Error BucketError.Empty }
            ]

            testProvider [
                { Description = "Domain from service"; Acutal = Create.Domain(service); Expected = Domain "domain" }
                { Description = "Domain from processor"; Acutal = Create.Domain(processor); Expected = Domain "domain" }
                { Description = "Domain from instance"; Acutal = Create.Domain(instance); Expected = Domain "domain" }
                { Description = "Domain from box"; Acutal = Create.Domain(box); Expected = Domain "domain" }
            ]
            testProvider [
                { Description = "Context from service"; Acutal = Create.Context(service); Expected = Context "context" }
                { Description = "Context from processor"; Acutal = Create.Context(processor); Expected = Context "context" }
                { Description = "Context from instance"; Acutal = Create.Context(instance); Expected = Context "context" }
                { Description = "Context from box"; Acutal = Create.Context(box); Expected = Context "context" }
            ]
            testProvider [
                { Description = "Purpose from processor"; Acutal = Create.Purpose(processor); Expected = Purpose "purpose" }
                { Description = "Purpose from instance"; Acutal = Create.Purpose(instance); Expected = Purpose "purpose" }
                { Description = "Purpose from box"; Acutal = Create.Purpose(box); Expected = Purpose "purpose" }
            ]
            testProvider [
                { Description = "Version from instance"; Acutal = Create.Version(instance); Expected = Version "version" }
                { Description = "Version from box"; Acutal = Create.Version(box); Expected = Version "version" }
            ]
            testProvider [
                { Description = "Zone from spot"; Acutal = Create.Zone(spot); Expected = Zone "zone" }
                { Description = "Zone from box"; Acutal = Create.Zone(box); Expected = Zone "zone" }
            ]

        testCase "complex types" <| fun _ ->
            testProvider [
                { Description = "Service from string"; Acutal = Create.Service("domain-context"); Expected = Ok service }
                { Description = "Service from string with dot"; Acutal = Create.Service("domain.context", '.'); Expected = Ok service }
                { Description = "Service from string values"; Acutal = Create.Service("domain", "context"); Expected = Ok service }
                { Description = "Service from string mixed values 1"; Acutal = Create.Service(Domain "domain", "context"); Expected = Ok service }
                { Description = "Service from string mixed values 2"; Acutal = Create.Service("domain", Context "context"); Expected = Ok service }
            ]
            testProvider [
                { Description = "Service from string mixed service values"; Acutal = Create.Service(Domain "domain", Context "context"); Expected = service }
                { Description = "Service from string mixed processor"; Acutal = Create.Service(processor); Expected = service }
                { Description = "Service from string mixed instance"; Acutal = Create.Service(instance); Expected = service }
                { Description = "Service from string mixed box"; Acutal = Create.Service(box); Expected = service }
            ]

            testProvider [
                { Description = "Box from string values"; Acutal = Create.Box("domain", "context", "purpose", "version", "zone", "bucket"); Expected = Ok box }
                { Description = "Box from mixed values 1"; Acutal = Create.Box(Domain "domain", "context", Purpose "purpose", "version", "zone", "bucket"); Expected = Ok box }
                { Description = "Box from mixed values 2"; Acutal = Create.Box(Domain "domain", "context", Purpose "purpose", "version", spot); Expected = Ok box }
                { Description = "Box from mixed values 3"; Acutal = Create.Box(service, Purpose "purpose", "version", spot); Expected = Ok box }
                { Description = "Box from mixed values 4"; Acutal = Create.Box(processor, "version", spot); Expected = Ok box }
                { Description = "Box from mixed values 5"; Acutal = Create.Box(instance, "zone", "bucket"); Expected = Ok box }
            ]
            testProvider [
                { Description = "Box from service + ... + spot"; Acutal = Create.Box(service, Purpose "purpose", Version "version", spot); Expected = box }
                { Description = "Box from processor + values"; Acutal = Create.Box(processor, Version "version", Zone "zone", Bucket "bucket"); Expected = box }
                { Description = "Box from instance + spot"; Acutal = Create.Box(instance, spot); Expected = box }
            ]
    ]
