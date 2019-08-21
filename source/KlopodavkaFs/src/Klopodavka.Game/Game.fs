namespace Klopodavka.Game

module Game =
    [<Literal>]
    let BoardWidth = 34

    [<Literal>]
    let BoardHeight = 40

    [<Literal>]
    let BaseOffset = 5

    let createBoard =
        let arr: Board = Array2D.create BoardWidth BoardHeight Empty
        arr.[BoardWidth - BaseOffset, BaseOffset] <- Base Red
        arr.[BaseOffset, BoardHeight - BaseOffset] <- Base Blue
        arr
