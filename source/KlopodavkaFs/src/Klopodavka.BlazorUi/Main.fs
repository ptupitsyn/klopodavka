module Klopodavka.BlazorUi.Client.Main

open Elmish
open Bolero
open Bolero.Html
open Bolero.Remoting.Client
open Bolero.Templating.Client
open Klopodavka.Game

/// Routing endpoints definition.
type Page =
    | [<EndPoint "/">] Home

/// The Elmish application's model.
type Model =
    {
        page: Page
        error: string option
        username: string
        password: string
        signedInAs: option<string>
        signInFailed: bool
        gameState: GameState
    }


let initModel =
    {
        page = Home
        error = None
        username = ""
        password = ""
        signedInAs = None
        signInFailed = false
        gameState = Game.createGame()        
    }

/// The Elmish application's update messages.
type Message =
    | SetPage of Page
    | NewGame
    | MakeMove of int * int
    | Error of exn
    | ClearError

let update message model =
    match message with
    | SetPage page ->
        { model with page = page }, Cmd.none

    | NewGame ->
        { model with gameState = Game.createGame() }, Cmd.none
    | MakeMove (x, y) ->
        printf "Move: %i %i" x y
        let newState = Game.makeMove model.gameState x y
        match newState with
            | Some state -> { model with gameState = state }, Cmd.none
            | _ -> model, Cmd.none

    | Error exn ->
        { model with error = Some exn.Message }, Cmd.none
    | ClearError ->
        { model with error = None }, Cmd.none

/// Connects the routing system to the Elmish application.
let router = Router.infer SetPage (fun model -> model.page)

type Main = Template<"wwwroot/main.html">

let cellSize = 20

let renderTile (tile: Tile) =
    match tile with
        | Base Red -> "🏠", "red"
        | Base Blue -> "🏠", "blue"
        | Alive Red -> "·", "red"
        | Alive Blue -> "·", "blue"
        | Squashed Red -> "X", "red"
        | Squashed Blue -> "X", "blue"
        | Empty -> "", ""

let homePage model dispatch =
    Main.Home()
        .NewGame(fun _ -> dispatch NewGame)
        .GameInfo(b [] [text (sprintf "Player: %O, Clicks left: %O" model.gameState.CurrentPlayer model.gameState.ClopsLeft)])
        .GameBoard(table [] [
            forEach (Board.rows model.gameState.Board) <| fun row ->
                tr [] [
                    forEach row <| fun (tile, x, y) ->
                        let symbol, color = renderTile tile
                        td [
                            attr.style (sprintf "background-color: %s" color)
                            on.click (fun _ -> dispatch (MakeMove (x, y)))
                        ] [
                            text symbol
                        ]
                ]
        ])
        .Elt()

let view model dispatch =
    Main()
        .Body(
            cond model.page <| function
            | Home -> homePage model dispatch
        )
        .Error(
            cond model.error <| function
            | None -> empty
            | Some err ->
                Main.ErrorNotification()
                    .Text(err)
                    .Hide(fun _ -> dispatch ClearError)
                    .Elt()
        )
        .Elt()

type MyApp() =
    inherit ProgramComponent<Model, Message>()

    override this.Program =
        Program.mkProgram (fun _ -> initModel, Cmd.none) update view
        // |> Program.withConsoleTrace        
        |> Program.withRouter router
#if DEBUG
        |> Program.withHotReload
#endif
