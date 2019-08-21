namespace Klopodavka.Game.Tests

open Expecto
open Klopodavka.Game

module GameTests =
    [<Tests>]
    let tests =
        testList "Game module tests" [

            testCase "createBoard returns board of correct size" <| fun _ ->
                let board = Game.createBoard
                Expect.equal (Game.width board) Game.BoardWidth "Board width is correct"
                Expect.equal (Game.height board) Game.BoardHeight "Board height is correct"

            testCase "createBoard returns board with base tiles" <| fun _ ->
                let board = Game.createBoard
                Expect.equal (Game.tile board 29 5) (Base Red) "Red base is at 29, 5"
                Expect.equal (Game.tile board 5 35) (Base Blue) "Blue base is at 5, 35"

            testCase "createBoard returns board with empty tiles except bases" <| fun _ ->
                let board = Game.createBoard
                let tiles =
                    Seq.allPairs (seq {0 .. Game.BoardWidth - 1}) (seq {0 .. Game.BoardHeight - 1})
                    |> Seq.map (fun coord -> Game.tile board (fst coord) (snd coord))
                    |> Seq.except [Base Blue; Base Red]
                Expect.allEqual tiles Empty "All tiles are empty except bases"

        ]
