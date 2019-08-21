namespace Klopodavka.Game

type Player = Red | Blue

type Tile =
    | Empty
    | Base of player: Player
    | Alive of player: Player
    | Squashed of player: Player

type Tiles = Tiles of Tile[,]
