module Klopodavka.Game.Board
open Klopodavka.Game

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

let neighbors board x y =
    let offs = [-1; 0; 1]
    Seq.allPairs offs offs
    |> Seq.except [(0, 0)]
    |> Seq.map (fun (a, b) -> (x + a, y + b))
    |> Seq.where (fun (a, b) -> a >= 0 && b >= 0 && a < width board && b < height board)


