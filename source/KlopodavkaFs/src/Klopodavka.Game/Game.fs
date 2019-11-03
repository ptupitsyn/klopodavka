module Klopodavka.Game.Game
open Klopodavka.Game
    // TODO: functions for game management - create new, track current turn, victory conditions
    
[<Literal>]
let DefaultClopsPerTurn = 10

let createGame(): GameState =
    {
        Board = Board.createBoard()
        CurrentPlayer = Red
        ClopsPerTurn = DefaultClopsPerTurn
        ClopsLeft = DefaultClopsPerTurn
    }


