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

    let toLower (string: string) = string.ToLower()

[<RequireQualifiedAccess>]
module internal SimpleType =
    let map (fromType: 'SimpleType -> 'a) (toType: 'b -> 'SimpleType) (f: 'a -> 'b) = fromType >> f >> toType
