namespace Klopodavka.Game

type Player = { Name: string; Id: uint16; }

type BoardTile =
    | Empty
    | Base of player: Player
    | Alive of player: Player
    | Squashed of player: Player


