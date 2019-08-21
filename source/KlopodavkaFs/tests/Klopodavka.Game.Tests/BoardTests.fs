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
                |> Seq.map (fun (x, y) -> tile board x y)
                |> Seq.except [Base Blue; Base Red]
            Expect.allEqual tiles Empty "All tiles are empty except bases"

        testCase "neighbors returns 3 tiles for top left corner" <| fun _ ->
            let board = createBoard
            let res = neighbors board 0 0
            Expect.sequenceEqual res [(0, 1); (1, 0); (1, 1)] "3 cells in the corner"

        testCase "neighbors returns 5 tiles for sides" <| fun _ ->
            let board = createBoard
            let res = neighbors board (DefaultWidth - 1) 10
            Expect.sequenceEqual res [(32, 9); (32, 10); (32, 11); (33, 9); (33, 11)] "6 cells on the right side"

        testCase "neighbors returns 9 tiles for mid field" <| fun _ ->
            let board = createBoard
            let res = neighbors board 5 6
            Expect.sequenceEqual res [(4, 5); (4, 6); (4, 7); (5, 5); (5, 7); (6, 5); (6, 6); (6, 7)] "9 cells in the middle"

    ]
