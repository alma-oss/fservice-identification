module Alma.ServiceIdentification.Test.MatchingServiceIdentification

open Expecto
open Alma.ServiceIdentification

let orFail = function
    | Some value -> value
    | None -> failtestf "Value is not in correct format."

let ``consents-consentor`` = "consents-consentor" |> Service.parse "-" |> orFail |> ByService
let ``consents-consentor-common`` = "consents-consentor-common" |> Processor.parse "-" |> orFail |> ByProcessor
let ``consents-consentor-common-latest`` = "consents-consentor-common-latest" |> Instance.parse "-" |> orFail |> ByInstance
let ``consents-mailer`` = "consents-mailer" |> Service.parse "-" |> orFail |> ByService

let provideServiceIdentifications =
    [
        // serviceIdentification, pattern, should match, description
        (``consents-consentor``, ``consents-consentor``, true, "service ~ service")
        (``consents-consentor``, ``consents-consentor-common``, false, "service ~ processor")
        (``consents-consentor``, ``consents-consentor-common-latest``, false, "service ~ instance")
        (``consents-consentor-common``, ``consents-consentor``, true, "processor ~ service")
        (``consents-consentor-common``, ``consents-consentor-common``, true, "processor ~ processor")
        (``consents-consentor-common``, ``consents-consentor-common-latest``, false, "processor ~ instance")
        (``consents-consentor-common-latest``, ``consents-consentor``, true, "instance ~ service")
        (``consents-consentor-common-latest``, ``consents-consentor-common``, true, "instance ~ processor")
        (``consents-consentor-common-latest``, ``consents-consentor-common-latest``, true, "instance ~ instance")
        // different services
        (``consents-mailer``, ``consents-consentor``, false, "mailer !~ consentor service")
        (``consents-mailer``, ``consents-consentor-common``, false, "mailer !~ consentor processor")
        (``consents-mailer``, ``consents-consentor-common-latest``, false, "mailer !~ consentor instance")
        (``consents-consentor``, ``consents-mailer``, false, "consentor service !~ mailer")
        (``consents-consentor-common``, ``consents-mailer``, false, "consentor processor !~ mailer")
        (``consents-consentor-common-latest``, ``consents-mailer``, false, "consentor instance !~ mailer")
    ]

[<Tests>]
let matchingServiceIdentificationTests =
    testList "Deployment - matching upstream dependency" [
        testCase "match service identification with env default upstream dependency" <| fun _ ->
            provideServiceIdentifications
            |> List.iter (fun (serviceIdentification, pattern, expected, description) ->
                Expect.equal (serviceIdentification |> ServiceIdentification.isMatching pattern) expected description
            )
    ]
