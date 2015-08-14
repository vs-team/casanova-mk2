module Stetris

(*
Cose da fare:
  Creare un menu con salvataggi e caricamenti
  Le costanti ripetute vanno richiamate da un file esterno
  La creazione dei blocchi va effettuata da un file esterno
*)


open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Media
open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework.Input
open Casanova
open Casanova.Core
open Casanova.Coroutines
open Casanova.Utilities
open Casanova.Math
open Casanova.Game
open Casanova.Drawing
open Casanova.StandardLibrary
open Casanova.StandardLibrary.Core

[<CasanovaWorld>]
type World = {
  Blocks_Layer        : SpriteLayer
  Background          : DrawableSprite
  ScoreAppereance     : DrawableText
  GameInfo            : DrawableText
  Timer               : Rule<float32<s>>
  Movable_Figure      : Rule<Figure>
  Immovable_Blocks    : RuleList<ImmovableBlock>
  Will_Collide        : Rule<bool>
  Borders             : List<ImmovableBlock>
  Score               : Rule<float32>
  Difficult           : Rule<float32>
  
  } with

  ///////////////////////////////////////////////////////////////////BACKGROUND////////////////////////////////////////////////////////////
  static member BackgroundCreate (world:World, dt:float32<s>) = 
                              DrawableSprite.Create (
                                                      world.Blocks_Layer,
                                                      Vector2.Create(0.0f, -0.0f),
                                                      Vector2.Create(1000.0f, 1000.0f),
                                                      "Stetris.png"
                                                    )

  ///////////////////////////////////////////////////////////////////SCORE/////////////////////////////////////////////////////////////////
  static member ScoreAppereanceStringRule(self:World) =
    "Score:\n" + (!self.Score |> string) + "\nLines:\n" + (!self.Difficult |> string)

  ///////////////////////////////////////////////////////////////////GAMEINFO//////////////////////////////////////////////////////////////
  static member GameInfoStringRule (world:World, dt:float32<s>) =
    if (Seq.exists (fun (immblock : ImmovableBlock) -> immblock.Position.Y < -500.0f<m>) !world.Immovable_Blocks) then
          "You lose!"
    else ""

  ///////////////////////////////////////////////////////////////////TIMER/////////////////////////////////////////////////////////////////
  static member TimerRule (world:World, dt: float32) = 
    if !world.Timer > 1.0f<s> then
        0.0f<s>
    else !world.Timer + dt * 1.f<_> * max 1.0f ((sqrt (!world.Difficult + 1.0f)) / 2.5f)

      
  ///////////////////////////////////////////////////////////////////IMMOVABLE_BLOCKS//////////////////////////////////////////////////////
  static member Immovable_BlocksRule (world:World, dt: float32) =
      if !world.Will_Collide && !world.Timer > 1.0f<s> then 
          seq{
            yield! !world.Immovable_Blocks
            for b in (!world.Movable_Figure).Blocks do
              yield ImmovableBlock.Create (world.Blocks_Layer,
                                            Vector2.Create((!b.RelativePosition).X,
                                                          (!b.RelativePosition).Y)
                                                      + !(!world.Movable_Figure).Center * 1.0f<_>,
                                            !b.Sprite.Size,
                                            !b.Sprite.Rotation,
                                            !b.Sprite.Origin,
                                            !b.Sprite.Path,
                                            !b.Sprite.Color)
              }
      else
        let dictionary = Seq.groupBy (fun b -> b.Position.Y) !world.Immovable_Blocks
        if Seq.exists (fun elem -> Seq.length (snd elem) = 11) dictionary then
          let not_in_full_lines =
                seq{
                    for (line, blocks) in dictionary do
                      if (Seq.length blocks) < 11 then
                        yield! blocks
                  }
          let rows_to_be_cancel =
              seq{
                  for (line, blocks) in dictionary do
                    if (Seq.length blocks) = 11 then
                      yield line
                }
          seq{
              for b in not_in_full_lines do
                  let length = Seq.filter(fun row -> row > b.Position.Y) rows_to_be_cancel |> Seq.length                
                  
                  let fall = (!world.Movable_Figure).Dimension * float32 length

                  yield ImmovableBlock.Create (!b.Sprite.Layer,
                                                Vector2.Create(b.Position.X,
                                                              b.Position.Y + fall),
                                                !b.Sprite.Size,
                                                !b.Sprite.Rotation,
                                                !b.Sprite.Origin,
                                                !b.Sprite.Path,
                                                b.Color)
            }
        else !world.Immovable_Blocks

  ///////////////////////////////////////////////////////////////////WILL_COLLIDE//////////////////////////////////////////////////////////
  static member Will_CollideRule (world:World, dt:float32<s>) : bool =
      let actual_position b = Vector2<m>.Create ((!(!world.Movable_Figure).Center).X + (!b.RelativePosition).X,
                                                 (!(!world.Movable_Figure).Center).Y + (!b.RelativePosition).Y)
      
      let exists_in_column immblocklist = 
                                  Seq.exists 
                                    (fun (b : Block) ->
                                      Seq.exists 
                                        (fun (closeimblock : ImmovableBlock) -> closeimblock.Position.Y >= (actual_position b).Y + (!world.Movable_Figure).Dimension)
                                        (Seq.filter (fun imblock -> 
                                                                  Vector2<m>.Distance(actual_position b, imblock.Position) <= (!world.Movable_Figure).Dimension)
                                            immblocklist))            
                                    (!world.Movable_Figure).Blocks
      if exists_in_column world.Borders || exists_in_column !world.Immovable_Blocks then
        let res = true
        res
      else false
          
  ///////////////////////////////////////////////////////////////////SCORE/////////////////////////////////////////////////////////////////
  static member ScoreRule (world:World, dt: float32) =
    let dictionary = Seq.groupBy (fun b -> b.Position.Y) !world.Immovable_Blocks
    let full_lines = Seq.filter (fun elem -> Seq.length (snd elem) = 11) dictionary 
    match Seq.length full_lines with
      |1 -> !world.Score + 1.0f
      |2 -> !world.Score + 2.5f
      |3 -> !world.Score + 4.0f
      |4 -> !world.Score + 6.0f
      |_ -> !world.Score

  ///////////////////////////////////////////////////////////////////DIFFICULT/////////////////////////////////////////////////////////////
  static member DifficultRule (world:World, dt: float32) =
    let dictionary = Seq.groupBy (fun b -> b.Position.Y) !world.Immovable_Blocks
    let full_lines = Seq.filter (fun elem -> Seq.length (snd elem) = 11) dictionary 
    if not (Seq.isEmpty full_lines) then 
      !world.Difficult + float32 (Seq.length full_lines)
    else !world.Difficult


///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



and [<CasanovaEntity>] ImmovableBlock = {
  Position             : Vector2<m>
  Color                : Color
  Sprite               : DrawableSprite
  } with
  static member Create (layer: SpriteLayer, position:Vector2<pixel>, size:Vector2<pixel>, 
                        rotation:float32, origin:Vector2<pixel>, path:string, color:Color) =
    {
      Position  = position * 1.f<_>
      Color     = color
      Sprite    = 
        DrawableSprite.Create(
                          layer,
                          position,
                          size,
                          rotation,
                          origin,
                          path,
                          color
                        )
    }

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    

and [<CasanovaEntity>] Figure = {
  Dimension           : float32<m>
  Center              : Rule<Vector2<m>>
  Blocks              : RuleList<Block>
  Color               : Color
  } with

  /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
  static member CenterRule (world:World, self:Figure, dt:float32<s>) =
    if !world.Will_Collide && !world.Timer > 1.0f<s> then !self.Center
    elif !world.Timer > 1.0f<s> then
      Vector2<m>.Create((!self.Center).X, (!self.Center).Y + (!world.Movable_Figure).Dimension)
    elif Seq.isEmpty !self.Blocks then
      Vector2.Create(0.0f<m>, -600.0f<m>)
    else !self.Center

  /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
  static member BlocksRule (world:World, self:Figure, dt:float32<s>) =
    if !world.Will_Collide && !world.Timer > 1.0f<s> then
      Seq.empty
    elif (Seq.isEmpty !self.Blocks) then
        let create (x : int ) (y: int) (color : Color) = 
                                Block.Create (var(Vector2<m>.Create((float32 x) * 50.f<m>,(float32 y) * 50.f<m>)),
                                              Vector2<m>.Create(0.f<m>,0.f<m>),
                                              color,
                                              world.Blocks_Layer)


        let random = System.Random()
        let random_figure =
                match random.Next(0,7) with
                |0 -> let color = Color.Aquamarine
                      seq{ //I
                          yield create 2 0 color
                          yield create 1 0 color
                          yield create 0 0 color
                          yield create -1 0 color
                          }
                |1 -> let color = Color.Orange
                      seq{ //L
                          yield create 1 0 color
                          yield create 0 0 color
                          yield create -1 0 color
                          yield create -1 1 color
                          }
                |2 -> let color = Color.BlueViolet
                      seq{ //J
                          yield create 1 1 color
                          yield create 0 1 color
                          yield create -1 1 color
                          yield create -1 0 color
                          }
                |3 -> let color = Color.Red
                      seq{ //S
                          yield create -1 0 color
                          yield create 0 1 color
                          yield create 0 0 color
                          yield create 1 1 color
                          }
                |4 -> let color = Color.Green
                      seq{ //Z
                          yield create -1 1 color
                          yield create 0 1 color
                          yield create 0 0 color
                          yield create 1 0 color
                          }
                |5 -> let color = Color.Purple
                      seq{ //T
                          yield create 1 0 color
                          yield create -1 0 color
                          yield create 0 1 color
                          yield create 0 0 color
                          }
                |6 -> let color = Color.Yellow
                      seq{ //O
                          yield create 1 1 color
                          yield create 1 0 color
                          yield create 0 1 color
                          yield create 0 0 color
                          }

        random_figure

    else !self.Blocks
     
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

and [<CasanovaEntity>] Block = {
  RelativePosition    : Var<Vector2<m>>
  Sprite              : DrawableSprite
  } with
  static member SpritePositionRule (world:World, self:Block, dt:float32<s>) =
    Vector2<m>.Create ((!self.RelativePosition).X + (!(!world.Movable_Figure).Center).X,
                      (!self.RelativePosition).Y + (!(!world.Movable_Figure).Center).Y)

  static member Create (relpos: Var<Vector2<m>>, position:Vector2<m>, color:Color, layer: SpriteLayer) =
      {
          RelativePosition  = relpos
          Sprite            = 
                        DrawableSprite.Create(
                                              layer,
                                              position * 1.f<_>,
                                              Vector2.Create(50.0f),
                                              0.0f,
                                              Vector2.Create(0.0f),
                                              "prism.png",
                                              color
                                             )
      }



///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
let start_game (args : StartGameArgs) =
  

  let world =

    let figure = {
      Dimension         = 50.0f<m>
      Center            = Rule.Create (Vector2<m>.Create(0.0f<m>, 0.0f<m>))
      Blocks            = RuleList.Create (fun () -> Seq.empty)
      Color             = Color.White
    }

    let borders = 
        seq{
          
          let layer = args.DefaultLayer
          let size = Vector2.Create(48.0f)
          let rotation = 0.0f
          let origin = Vector2.Create(0.01f)
          let path = "prism.png"
          let color = Color(185, 135, 76)

          for x=(-6) to 6 do
            yield ImmovableBlock.Create ( layer,
                                          Vector2.Create((float32 x) * figure.Dimension,
                                                         -12.0f * figure.Dimension),
                                          size,
                                          rotation,
                                          origin,
                                          path,
                                          color)

            yield ImmovableBlock.Create ( layer,
                                          Vector2.Create((float32 x) * 1.0f * figure.Dimension,
                                                         9.0f * figure.Dimension),
                                          size,
                                          rotation,
                                          origin,
                                          path,
                                          color)
          for y=(-14) to 9 do
            yield ImmovableBlock.Create ( layer,
                                          Vector2.Create(-6.0f * figure.Dimension,
                                                        (float32 y) * 1.0f * figure.Dimension),
                                          size,
                                          rotation,
                                          origin,
                                          path,
                                          color)
            yield ImmovableBlock.Create ( layer,
                                          Vector2.Create(6.0f * figure.Dimension,
                                                        (float32 y) * figure.Dimension),
                                          size,
                                          rotation,
                                          origin,
                                          path,
                                          color)
        } |> Seq.toList
    { 
      Blocks_Layer      = args.DefaultLayer
      Background        = DrawableSprite.Create (
                                            args.DefaultLayer,
                                            Vector2.Create(0.0f, 0.0f),
                                            Vector2.Create(1000.0f, 1000.0f),
                                            "city2.png"
                                          )
      ScoreAppereance   = DrawableText.Create (args.DefaultLayer, Vector2.Create(-480.0f, -480.0f), Vector2<pixel>.Create(400.0f, 200.0f))

      GameInfo          = DrawableText.Create (args.DefaultLayer,
                                    Vector2.Create(-425.0f, -125.0f),
                                    Vector2.Create(500.0f, 250.0f))
      Timer             = Rule.Create 0.0f<s>
      Movable_Figure    = Rule.Create figure
      Immovable_Blocks  = RuleList.Create (fun () -> Seq.empty)
      Will_Collide      = Rule.Create false
      Borders           = borders
      Score             = Rule.Create 0.0f
      Difficult         = Rule.Create 0.0f
    }

  // Internal requirement. Leave *before* scripts.
  let inline (!) x = immediate_lookup x
  

///////////////////////////////////////////////////////////

  let main =  yield_

  let input =
    [
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
      wait_key_down Keys.Down => 
        co{
            world.Timer := !world.Timer + 1.0f<s>
          }
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
      wait_key_down Keys.Left =>
        co{
            let actual_position b = Vector2<m>.Create ((!(!world.Movable_Figure).Center).X + (!b.RelativePosition).X,
                                                       (!(!world.Movable_Figure).Center).Y + (!b.RelativePosition).Y)

            let close_blocks b immblocklist = 
                                        Seq.filter (fun (imblock : ImmovableBlock) -> 
                                              Vector2<m>.Distance((actual_position b), imblock.Position) <= (!world.Movable_Figure).Dimension)
                                            immblocklist

            let exists_at_left immblocklist =
                                        Seq.exists (fun (b : Block) -> 
                                                      Seq.exists
                                                        (fun (closeimblock : ImmovableBlock) ->
                                                                closeimblock.Position.X - (actual_position b).X <= -(!world.Movable_Figure).Dimension)
                                                        (close_blocks b immblocklist))
                                          (!world.Movable_Figure).Blocks

            if (not (exists_at_left world.Borders || exists_at_left world.Immovable_Blocks)) then
              (!world.Movable_Figure).Center := !(!world.Movable_Figure).Center - Vector2<m>.Create(50.0f<m>, 0.0f<m>)
            do! wait_key_up Keys.Left .||> wait 0.1f<s>
        }
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
      wait_key_press Keys.Right =>
        co{
            let actual_position b = Vector2<m>.Create ((!(!world.Movable_Figure).Center).X + (!b.RelativePosition).X,
                                                       (!(!world.Movable_Figure).Center).Y + (!b.RelativePosition).Y)

            let close_blocks b immblocklist = 
                                        Seq.filter (fun imblock -> 
                                              Vector2<m>.Distance((actual_position b), imblock.Position) <= (!world.Movable_Figure).Dimension)
                                            immblocklist

            let exists_at_right immblocklist =
                                      Seq.exists (fun b -> 
                                                    Seq.exists
                                                      (fun (closeimblock : ImmovableBlock) ->
                                                              closeimblock.Position.X - (actual_position b).X >= (!world.Movable_Figure).Dimension)
                                                      (close_blocks b immblocklist))
                                        (!world.Movable_Figure).Blocks

            if (not (exists_at_right world.Borders || exists_at_right world.Immovable_Blocks)) then
              (!world.Movable_Figure).Center := !(!world.Movable_Figure).Center + Vector2<m>.Create(50.0f<m>, 0.0f<m>)
        }

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
      wait_key_press Keys.Up =>
        co{
             let rotated_position b = Vector2<m>.Create ((!(!world.Movable_Figure).Center).X + (!b.RelativePosition).Y,
                                                        (!(!world.Movable_Figure).Center).Y - (!b.RelativePosition).X)
             
             let will_collide b im_block_list =
                  Seq.exists (fun imblock -> 
                                  Vector2<m>.Distance(rotated_position b, imblock.Position) < (!world.Movable_Figure).Dimension)
                      im_block_list
             
             if not (Seq.exists (fun b -> will_collide b (!world.Immovable_Blocks)) (!world.Movable_Figure).Blocks) then
               if not (Seq.exists (fun b -> will_collide b world.Borders) (!world.Movable_Figure).Blocks) then
                 for b in (!world.Movable_Figure).Blocks do
                    b.RelativePosition := Vector2<m>.Create((!b.RelativePosition).Y, -(!b.RelativePosition).X)

        }
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////                 
      wait_key_press Keys.Escape => args.Quit //Esc Key -> The game closes
    ]
  world, main, input

[<CasanovaEntryPoint>]
let Run() =
  let game = Game.Create(start_game, 500, 500, false, "Stetris") 
  game.Run()
