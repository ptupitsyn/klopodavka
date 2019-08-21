namespace Klopodavka.Game

module Game =
    [<Literal>]
    let BoardWidth = 34

    [<Literal>]
    let BoardHeight = 40

    [<Literal>]
    let BaseOffset = 5

    let createBoard =
        let arr = Array2D.create BoardWidth BoardHeight Empty
        arr.[BoardWidth - BaseOffset, BaseOffset] <- Base Red
        arr.[BaseOffset, BoardHeight - BaseOffset] <- Base Blue
        arr |> Board

    let tiles (Board board) = board

    let tile (board: Board) x y = (tiles board).[x, y]

    let width (board: Board) = (tiles board).GetLength(0)

    let height (board: Board) = (tiles board).GetLength(1)

    // let get

    // let getMoves Board Player =

