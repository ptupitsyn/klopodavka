module Klopodavka.Game.Board

[<Literal>]
let DefaultWidth = 34

[<Literal>]
let DefaultHeight = 40

[<Literal>]
let BaseOffset = 5

let createBoard =
    let arr = Array2D.create DefaultWidth DefaultHeight Empty
    arr.[DefaultWidth - BaseOffset, BaseOffset] <- Base Red
    arr.[BaseOffset, DefaultHeight - BaseOffset] <- Base Blue
    arr |> Tiles

let tiles (Tiles board) = board

let tile board x y = (tiles board).[x, y]

let width board = (tiles board).GetLength(0)

let height board = (tiles board).GetLength(1)


