namespace Klopodavka.Game.Tests

open Expecto
open Klopodavka.Game

module GameTests =
    [<Tests>]
    let tests =
        testList "Game module tests" [
        testCase "createGame returns board with base tiles" <| fun _ ->
            let board = Game.createBoard
            Expect.equal Game.BoardWidth (board.GetLength 0) "Board width is correct"
            Expect.equal Game.BoardHeight (board.GetLength 1) "Board height is correct"
        ]
