// Learn more about F# at http://fsharp.org

open OrderProcessor

[<EntryPoint>]
let main argv =
    argv
    |> (fun a -> if a.Length > 0 then a.[0] else "")
    |> OrderService.main
    0 
