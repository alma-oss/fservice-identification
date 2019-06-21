#load ".fake/build.fsx/intellisense.fsx"
open Fake.Core
open Fake.DotNet
open Fake.IO
open Fake.IO.FileSystemOperators
open Fake.IO.Globbing.Operators
open Fake.Core.TargetOperators

// =============================
// === F# / Library template ===
// =============================

let tee f a =
    f a
    a

let runDotNet cmd workingDir =
    let options =
        DotNet.Options.withWorkingDirectory workingDir
        >> DotNet.Options.withRedirectOutput true

    DotNet.exec options cmd ""

let runDotNetOrFail cmd workingDir =
    runDotNet cmd workingDir
    |> tee (fun result ->
        if result.ExitCode <> 0 then failwithf "'dotnet %s' failed in %s" cmd workingDir
    )

let sourceDir = "."
let nugetServer = sprintf "http://development-nugetserver-common-stable.service.devel1-services.consul:%i"
let apiKey = "123456"

let sources = sprintf "-s %s -s https://api.nuget.org/v3/index.json"

let nugetServerUrl p =
    match p.Context.Arguments with
    | head::_ ->
        if head.StartsWith "http" then head
        else head |> int |> nugetServer
    | _ -> failwithf "Release target requires nuget server url or port"

let runDotNetInSrc cmd = runDotNet cmd sourceDir
let runDotNetInSrcOrFail cmd = runDotNetOrFail cmd sourceDir

let installOrUpdateTool tool =
    let toolCommand action =
        sprintf "tool %s --global %s" action tool

    match runDotNetInSrc (toolCommand "install") with
    | { ExitCode = code } when code <> 0 -> runDotNetInSrcOrFail (toolCommand "update")
    | __ -> __
    |> ignore

Target.create "Clean" (fun _ ->
    !! "**/bin"
    ++ "**/obj"
    |> Shell.cleanDirs
)

Target.create "Build" (fun p ->
    let nugetServerUrl = nugetServerUrl p

    runDotNet (sprintf "restore --no-cache %s" (sources nugetServerUrl)) sourceDir |> ignore
    runDotNet "build --no-restore" sourceDir |> ignore

    !! "**/*.*proj"
    -- "example/**/*.*proj"
    |> Seq.iter (DotNet.build id)
)

Target.create "Lint" (fun p ->
    installOrUpdateTool "dotnet-fsharplint"

    let checkResult (result: ProcessResult) =
        let rec check: string list -> unit = function
            | [] -> failwithf "Lint does not yield a summary."
            | head::rest ->
                if head.Contains("Summary") then
                    match head.Replace("= ", "").Replace(" =", "").Replace("=", "").Replace("Summary: ", "") with
                    | "0 warnings" -> ()
                    | warnings ->
                        if p.Context.Arguments |> List.contains "no-lint"
                        then Trace.traceErrorfn "Lint ends up with %s." warnings
                        else failwithf "Lint ends up with %s." warnings
                else check rest
        result.Messages
        |> List.rev
        |> check

    !! "**/*.fsproj"
    |> Seq.map (sprintf "fsharplint -f %s" >> runDotNetInSrcOrFail)
    |> Seq.iter checkResult
)

Target.create "Tests" (fun _ ->
    runDotNetOrFail "run" "./tests" |> ignore
)

Target.create "Release" (fun p ->
    let nugetServerUrl = nugetServerUrl p

    runDotNet "pack" sourceDir |> ignore

    let pushToNuget path =
        sourceDir
        |> runDotNet (sprintf "nuget push %s -s %s -k %s" path nugetServerUrl apiKey)
        |> ignore

    !! "**/bin/**/*.nupkg"
    |> Seq.iter (fun path ->
        path
        |> tee pushToNuget
        |> Shell.moveFile "release"
    )
)

Target.create "Watch" (fun _ ->
    runDotNetInSrcOrFail "watch run" |> ignore
)

Target.create "Run" (fun _ ->
    runDotNetInSrcOrFail "run" |> ignore
)

"Clean"
    ==> "Build"
    ==> "Lint"
    ==> "Release" <=> "Tests" <=> "Watch" <=> "Run"

Target.runOrDefaultWithArguments "Build"
