module Klopodavka.Game.Game
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
    { (nextClop game) with Board = board  }
    

let rows gameState =
    let arr = Board.tiles gameState.Board |> Array2D.copy
    Board.moves gameState.Board gameState.CurrentPlayer
        |> Seq.iter  (fun (x, y) -> arr.[x, y] <- Available)
        
    let w, h = arr.GetLength(0), arr.GetLength(1)
    { 0 .. h - 1 }|> Seq.map (
                             fun y -> { 0 .. w - 1 } |> Seq.map (
                                                                fun x -> (arr.[x, y], x, y)
                                                            )
                         )
