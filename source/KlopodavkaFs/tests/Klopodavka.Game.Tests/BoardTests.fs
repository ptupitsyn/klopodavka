module Klopodavka.Game.Tests.Board

open Expecto
open Klopodavka.Game
open Klopodavka.Game.Board

[<Tests>]
let tests =
    testList "Board tests" [
        // TODO: FsCheck

        testCase "createBoard returns board of correct size" <| fun _ ->
            let board = createBoard()
            let w, h = size board
            Expect.equal w DefaultWidth "Board width is default"
            Expect.equal h DefaultHeight "Board height is default"

        testCase "createBoard returns board with base tiles" <| fun _ ->
            let board = createBoard()
            Expect.equal (tile board 29 5) (Base Red) "Red base is at 29, 5"
            Expect.equal (tile board 5 35) (Base Blue) "Blue base is at 5, 35"

        testCase "createBoard returns board with empty tiles except bases" <| fun _ ->
            let board = createBoard()
            let w, h = size board
            let tiles =
                Seq.allPairs (seq { 0..w - 1 }) (seq { 0..h - 1 })
                |> Seq.map (fun (x, y) -> tile board x y)
                |> Seq.except [ Base Blue; Base Red ]
            Expect.allEqual tiles Empty "All tiles are empty except bases"

        testCase "neighbors returns 3 tiles for top left corner" <| fun _ ->
            let res = neighbors 30 30 0 0
            Expect.sequenceEqual res [ (0, 1); (1, 0); (1, 1) ] "3 cells in the corner"

        testCase "neighbors returns 5 tiles for sides" <| fun _ ->
            let res = neighbors DefaultWidth DefaultHeight (DefaultWidth - 1) 10
            Expect.sequenceEqual res [ (32, 9); (32, 10); (32, 11); (33, 9); (33, 11) ] "6 cells on the right side"

        testCase "neighbors returns 8 tiles for mid field" <| fun _ ->
            let res = neighbors DefaultWidth DefaultHeight 5 6
            Expect.sequenceEqual res [ (4, 5); (4, 6); (4, 7); (5, 5); (5, 7); (6, 5); (6, 6); (6, 7) ] "9 cells in the middle"

        testCase "moves returns 8 tiles around the base for empty field" <| fun _ ->
            let board = createBoard()
            let w, h = size board
            let bx, by = basePosition Red w h
            let expected = neighbors w h bx by
            let actual = moves board Red

            Expect.sequenceEqual actual expected "base neighbors"

        testCase "moves returns neighbors for all connected tiles" <| fun _ ->
            let board = createBoard()
            let w, h = size board
            let bx, by = basePosition Red w h
            let (Tiles t) = board
            for i = 1 to 5 do
                t.[bx - i, by + i] <- if i % 2 = 0 then Alive Red else Squashed Red

            let isRed (x, y) = match (tile board x y) with
                                                        | Alive Red | Squashed Red | Base Red -> true
                                                        | _ -> false

            let hasNeighbor (x, y) = neighbors w h x y |> Seq.exists isRed

            let expected = Seq.allPairs (seq { 0..w - 1 }) (seq { 0..h - 1 })
                           |> Seq.where hasNeighbor
                           |> Seq.where (fun (x, y) -> tile board x y = Empty)
                           |> Seq.sort

            let actual = moves board Red |> Seq.sort

            Expect.sequenceEqual actual expected "connected tile neighbors"
            
        testCase "validateMove returns true for neighbors of connected tiles" <| fun _ ->
            let board = createBoard()
            let w, h = size board
            let bx, by = basePosition Red w h
            let validMove = validateMove board Red bx (by + 1)
            Expect.equal true validMove "Base tile neighbor is a valid move"

        testCase "validateMove returns false for base tile" <| fun _ ->
            let board = createBoard()
            let w, h = size board
            let bx, by = basePosition Red w h
            let validMove = validateMove board Red bx by
            Expect.isFalse validMove "Base tile is not a valid move"
            
        testCase "makeMove returns None for invalid move" <| fun _ ->
            Expect.isTrue false "TODO"
            
        testCase "makeMove returns new Tiles for valid move" <| fun _ ->
            Expect.isTrue false "TODO"
            
    ]
