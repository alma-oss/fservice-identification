# Examples

All example code for this skill lives here. Examples are ordered from simplest to
most complete and each is self-contained. Placeholder parts are neutral
(`alpha`, `api`, `worker`, `v2`, `north`, `blue`).

## Basic parsing

`parseStrict` validates each part and returns a `Result`.

```fsharp
open Alma.ServiceIdentification

// Ok { Domain = Domain "alpha"; Context = Context "api" }
let service = Service.parseStrict "-" "alpha-api"

// Ok (full Instance record)
let instance = Instance.parseStrict "-" "alpha-api-worker-v2"
```

## Lenient parsing

`parse` returns an `option`, checking only the segment count (no per-part regex).

```fsharp
open Alma.ServiceIdentification

// Some (ByProcessor { Domain = ...; Context = ...; Purpose = ... })
let identification = ServiceIdentification.parse "-" "alpha-api-worker"

// None — wrong number of segments
let invalid = ServiceIdentification.parse "-" "alpha"
```

## Programmatic construction

Build from already-typed parts (`createFromValues`, total) or from raw strings
(`createFromStrings`, returns `option`).

```fsharp
open Alma.ServiceIdentification

let processor =
    Processor.createFromValues (Domain "alpha") (Context "api") (Purpose "worker")

// Some Service — empty/wildcard parts are rejected
let maybeService = Service.createFromStrings ("alpha", "api")
```

## Rendering back to string

`concat` for most types (explicit separator); `Box.value` for the `@`-joined box form.

```fsharp
open Alma.ServiceIdentification

let instance =
    Instance.createFromValues (Domain "alpha") (Context "api") (Purpose "worker") (Version "v2")

// "alpha-api-worker-v2"
let text = instance |> Instance.concat "-"

let box =
    Box.ofInstance instance (Zone "north") (Bucket "blue")

// "north-blue@alpha-api-worker-v2"
let boxText = box |> Box.value
```

## Create factory

A single entry point that accepts mixed string and typed arguments.

```fsharp
open Alma.ServiceIdentification

let fromString  = Create.Service("alpha-api")              // Result<Service, _>
let fromParts   = Create.Service("alpha", "api")           // Result<Service, _>
let fromMixed   = Create.Processor(Domain "alpha", "api", "worker")
let spotWithSep = Create.Spot("north,blue", ',')           // default separator is '.'
let box         = Create.Box("north-blue@alpha-api-worker-v2")
```

## Handling structured errors

Match on the error DU instead of stringifying it.

```fsharp
open Alma.ServiceIdentification

match Service.parseStrict "-" "alpha-123" with
| Ok service -> printfn "ok: %A" service
| Error (ServiceError.InvalidFormat raw) ->
    printfn "wrong segment count: %s" raw
| Error (ServiceError.ServicePart parts) ->
    parts
    |> List.iter (function
        | ServicePartError.Domain err  -> printfn "domain: %A" err
        | ServicePartError.Context err -> printfn "context: %A" err)
```

## Projection and promotion

Project downward with getters; promote upward with `ofX` helpers.

```fsharp
open Alma.ServiceIdentification

let instance =
    Instance.createFromValues (Domain "alpha") (Context "api") (Purpose "worker") (Version "v2")

// Project: Instance -> Service
let service = instance |> Instance.service

// Promote: Service -> Instance, supplying only the missing parts
let rebuilt = Instance.ofService service (Purpose "worker") (Version "v2")
```

## Matching against a pattern

```fsharp
open Alma.ServiceIdentification

let value   = ServiceIdentification.parse "-" "alpha-api-worker" |> Option.get
let pattern = ServiceIdentification.parse "-" "alpha-api" |> Option.get

// true — a processor matches its broader service pattern
let matches = value |> ServiceIdentification.isMatching pattern

// Box-level matching with wildcards via BoxPattern
let box        = Create.Box("north-blue@alpha-api-worker-v2") |> function Ok b -> b | Error _ -> failwith "bad"
let boxPattern = box |> Box.instance |> BoxPattern.ofInstance   // zone/bucket = Any
let boxMatches = boxPattern |> BoxPattern.isMatching box
```

## Table-driven test

Mirrors the library's data-provider test style (using Expecto).

```fsharp
open Expecto
open Alma.ServiceIdentification

[<Tests>]
let parseTests =
    testCase "parseStrict service" <| fun _ ->
        [
            "alpha-api", Ok { Domain = Domain "alpha"; Context = Context "api" }
            "alpha",     Error (ServiceError.InvalidFormat "alpha")
        ]
        |> List.iter (fun (input, expected) ->
            Expect.equal (Service.parseStrict "-" input) expected input)
```

## Full workflow

Parse untrusted input, project, promote, and render.

```fsharp
open Alma.ServiceIdentification

let describe (raw: string) =
    match Instance.parseStrict "-" raw with
    | Error err -> sprintf "rejected: %A" err
    | Ok instance ->
        let service = instance |> Instance.service |> Service.concat "-"
        let box     = Box.ofInstance instance (Zone "north") (Bucket "blue") |> Box.value
        sprintf "service=%s box=%s" service box

// "service=alpha-api box=north-blue@alpha-api-worker-v2"
describe "alpha-api-worker-v2"
```
