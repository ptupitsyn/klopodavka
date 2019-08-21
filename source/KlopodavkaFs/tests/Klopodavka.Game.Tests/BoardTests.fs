module Klopodavka.Game.Tests.Board

open Expecto
open Klopodavka.Game
open Klopodavka.Game.Board

[<Tests>]
let tests =
    testList "Board tests" [

        testCase "createBoard returns board of correct size" <| fun _ ->
            let board = createBoard
            Expect.equal (width board) DefaultWidth "Board width is default"
            Expect.equal (height board) DefaultHeight "Board height is default"

        testCase "createBoard returns board with base tiles" <| fun _ ->
            let board = createBoard
            Expect.equal (tile board 29 5) (Base Red) "Red base is at 29, 5"
            Expect.equal (tile board 5 35) (Base Blue) "Blue base is at 5, 35"

        testCase "createBoard returns board with empty tiles except bases" <| fun _ ->
            let board = createBoard
            let tiles =
                Seq.allPairs (seq {0 .. width board - 1}) (seq {0 .. height board - 1})
                |> Seq.map (fun coord -> tile board (fst coord) (snd coord))
                |> Seq.except [Base Blue; Base Red]
            Expect.allEqual tiles Empty "All tiles are empty except bases"

    ]
