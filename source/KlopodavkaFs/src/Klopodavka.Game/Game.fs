﻿module Klopodavka.Game.Game
open Klopodavka.Game

[<Literal>]
let DefaultClopsPerTurn = 10

let createGame(): GameState =
    {
        Board = Board.createBoard()
        CurrentPlayer = Red
        ClopsPerTurn = DefaultClopsPerTurn
        ClopsLeft = DefaultClopsPerTurn
    }

let otherPlayer player =
    if (player = Red) then Blue else Red


let makeMove game x y =
    let nextClop game =
        let lastClop = game.ClopsLeft = 1
        let clopsLeft = if lastClop then game.ClopsPerTurn else game.ClopsLeft - 1
        let player = if lastClop then otherPlayer(game.CurrentPlayer) else game.CurrentPlayer
        { game with CurrentPlayer = player; ClopsLeft = clopsLeft }
        
    let board = Board.makeMove game.Board game.CurrentPlayer x y
    
    match board with
        | Some(tiles) -> Some({ (nextClop game) with Board = tiles  })
        | _ -> None
    

