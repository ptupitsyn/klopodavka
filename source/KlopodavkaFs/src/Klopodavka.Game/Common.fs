module Klopodavka.Game.Common

type Result<'T> =
    | Success of 'T
    | Failure of string
    
let succeed x =
    Success x
    
let fail message =
    Failure message
    
let bind fn res =
    match res with
        | Success x -> fn x
        | Failure err -> Failure err