module Klopodavka.Game.Game
open Klopodavka.Game

[<Literal>]
let DefaultClopsPerTurn = 7

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
    let tiles = Board.tiles gameState.Board
    let w, h = Board.size gameState.Board
    let avail = Array2D.create w h false
    
    Board.moves gameState.Board gameState.CurrentPlayer
        |> Seq.iter (fun (x, y) -> avail.[x, y] <- true)
        
    { 0 .. h - 1 }|> Seq.map (
                             fun y -> { 0 .. w - 1 } |> Seq.map (
                                                                fun x -> (x, y, tiles.[x, y], avail.[x, y])
                                                            )
                         )
