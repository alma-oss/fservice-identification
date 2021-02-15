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

[<AutoOpen>]
module internal Regexp =
    open System.Text.RegularExpressions

    // http://www.fssnip.net/29/title/Regular-expression-active-pattern
    let (|Regex|_|) pattern input =
        let m = Regex.Match(input, pattern)
        if m.Success then Some (List.tail [ for g in m.Groups -> g.Value ])
        else None

[<RequireQualifiedAccess>]
module internal SimpleType =
    let map (fromType: 'SimpleType -> 'a) (toType: 'b -> 'SimpleType) (f: 'a -> 'b) = fromType >> f >> toType

    let parseStrict pattern ok empty invalid = function
        | String.IsEmpty -> Error empty
        | Regex pattern [ value ] -> Ok (ok value)
        | invalidValue -> Error (invalid invalidValue)
