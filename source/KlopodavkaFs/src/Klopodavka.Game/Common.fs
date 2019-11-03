module Klopodavka.Game.Common

let bind fn res =
    match res with
        | Some x -> fn x
        | _ -> None
  
let apply fn res =
    match res with
    | Some(res) -> fn res |> Some
    | _ -> None

let lift fn res =
    apply fn res
    