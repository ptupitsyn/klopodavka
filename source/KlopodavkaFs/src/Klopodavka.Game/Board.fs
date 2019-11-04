module Klopodavka.Game.Board
open Klopodavka.Game

[<Literal>]
let DefaultWidth = 30

[<Literal>]
let DefaultHeight = 30

[<Literal>]
let BaseOffset = 5

[<Literal>]
let MinSize = 20

let tiles (Tiles board) = board

let tile board x y = (tiles board).[x, y]

let size board =
    let arr = tiles board
    (arr.GetLength(0), arr.GetLength(1))

let basePosition player width height =
    match player with
        | Red -> (width - BaseOffset, BaseOffset)
        | Blue -> (BaseOffset, height - BaseOffset)

let createBoard() =
    let arr = Array2D.create DefaultWidth DefaultHeight Empty

    let redX, redY = basePosition Red DefaultWidth DefaultHeight
    arr.[redX, redY] <- Base Red

    let blueX, blueY = basePosition Blue DefaultWidth DefaultHeight
    arr.[blueX, blueY] <- Base Blue

    arr |> Tiles

let neighbors w h x y =
    if w < MinSize then invalidArg "w" "out of range"
    if h < MinSize then invalidArg "h" "out of range"
    if x < 0 || x >= w then invalidArg "x" "out of range"
    if y < 0 || y >= h then invalidArg "x" "out of range"

    let offs = [ -1; 0; 1 ]
    Seq.allPairs offs offs
    |> Seq.map (fun (a, b) -> (x + a, y + b))
    |> Seq.where (fun (a, b) -> a >= 0 && b >= 0 && a < w && b < h && (a, b) <> (x, y))

let moves board player =
    let w, h = size board
    let visited = Array2D.create w h false

    let rec loop tiles = seq {
        for pos in tiles do
            let x, y = pos
            if not visited.[x, y] then
                visited.[x, y] <- true
                let t = tile board x y
                let loopNeighbors () = loop (neighbors w h x y)

                match t with
                | Empty -> yield pos
                | Alive p when p <> player -> yield pos
                | Base p when p = player -> yield! loopNeighbors()
                | Alive p when p = player -> yield! loopNeighbors()
                | Squashed p when p = player -> yield! loopNeighbors()
                | _ -> ()

    }

    let bx, by = basePosition player w h
    loop [bx, by]

let validateMove board player x y =
    let validMoves = moves board player
    Seq.contains (x, y) validMoves
    
let makeMove board player x y =
    let newBoard = Array2D.copy (tiles board)
    let oldVal = newBoard.[x, y]
    newBoard.[x, y] <- if oldVal = Empty then Alive player else Squashed player
    Tiles newBoard
    