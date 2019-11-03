module Klopodavka.Game.Tests.Game

open Expecto
open Klopodavka.Game
open Klopodavka.Game.Game

[<Tests>]
let tests =
    testList "Game tests" [
        
        testCase "createGame returns new GameState with bases and initial state" <| fun _ ->
            let game = createGame()
            Expect.equal Red game.CurrentPlayer "First player is Red"
            Expect.equal DefaultClopsPerTurn game.ClopsPerTurn "ClopsPerTurn is default"
            Expect.equal DefaultClopsPerTurn game.ClopsLeft "ClopsLeft is default"

        testCase "makeMove returns None for invalid move" <| fun _ ->
            Expect.isTrue false "TODO"
            
        testCase "makeMove returns new Game for valid move" <| fun _ ->
            Expect.isTrue false "TODO"
            
    ]
