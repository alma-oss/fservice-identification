namespace Lmc.ServiceIdentification

[<AutoOpen>]
module internal Pattern =
    let [<Literal>] Any = "*"

[<RequireQualifiedAccess>]
module internal String =
    open System

    let (|IsEmpty|_|) = function
        | null -> Some IsEmpty
        | empty when empty |> String.IsNullOrWhiteSpace -> Some IsEmpty
        | _ -> None
