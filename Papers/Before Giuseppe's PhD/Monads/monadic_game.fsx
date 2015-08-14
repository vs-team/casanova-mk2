type Projectile = Unit
type Explosion = Projectile
type Ship = Unit
type InputState = Unit
type 'a Entity = 'a

type Projectile=Entity<Geometry,GameState*DeltaTime*InputState>
let rec projectile g=Entity(g,(fun (_,dt,_) -> Read(projectile (moveForward dt g))))

type Player=Entity<Ship,GameState*DeltaTime*InputState>
let rec player p=Entity(p, player (updatePlayer p))
let updatePlayerGeometry =
  state{ // Geometry -> Ship x GameState x DeltaTime x InputState
    if is.IsKeyDown(W) then do! move_forward
    if is.IsKeyDown(A) then do! move_left
    if is.IsKeyDown(D) then do! move_right
    if is.IsKeyDown(S) then do! move_back
  }

let updateShip =
  state{ // Ship -> GameState x DeltaTime x InputState
    do! update_geometry
    let! own_hits = hits
    do! update_bridges hits
    do! update_weapons
  }

type GameState={Ships:Property<List<Entity<Ship>>>;
                Projectiles:Property<List<Projectile>>;
                Explosions:Property<List<Explosion>>;
                Hits:Map<Ship,List<Projectile>>}

let updateProjectiles =
 state{ // GameState -> DeltaTime x InputState
  let! gs = GameState
  let! ps = gs.Projectiles
  let! ss = gs.Ships
  for p in ps do
    let! hs = ss |> filter (collide p)
    if hs |> Seq.isEmpty then
      do! replace p (p.Update) // p.Update ha tipo State<Projectile>
    else
      do! explode p
      for s in hs do
        do! add_hit p s
 }

let updateProjectiles =
 state{ // GameState -> DeltaTime x InputState
  let! gs = GameState
  let! ss = gs.Ships
  for s in ss do
    if s.Destroyed then
      do! explode s
    else
      do! replace s (s.Update) // s.Update ha tipo State<Ship>
}

let updateGameState =
  state{ // GameState, DeltaTime x InputState
    do! updateProjectiles
    do! updateShips
  } (gs,is,dt,Map.Empty,[ ])  |>snd


(*la nave del giocatore si muove rispetto all^' input e (sempre rispetto all^' input)
genera i proiettili del giocatore;la monade di stato di updatePlayer ha
uno stato specifico,ossia la nave*)
type Player=Entity (Ship,UpdatingState,List Projectile)
let rec player p=Entity(p,updatePlayer (player)p)
let rec updatePlayer  k p=
  state{
    for b in p.Bridges do
      do! replace b b.Update
  } 

