// SAMPLE 1:
// initial definition with "classic" Casanova from previous papers

type World = {
  Asteroids   : List<Asteroid>
  Projectiles : List<Projectile>
  Cannon      : Cannon
}
rule Asteroids(world:World,dt:float<s>) =
  [a | a <- world.Asteroids, a.Position.Y > 0.0<m>]
rule Projectiles(world:World,dt:float<s>) =
  [p | p <- world.Projectiles, p.Position.Y < 100.0<m>]

type Asteroid = {
  Position    : Vector2<m>
  Velocity    : Vector2<m/s>
  Colliders   : List<Projectile>
}
rule Position(world:World,self:Asteroid,dt:float<s>) = 
  Position + Velocity * dt
rule Colliders(world:World,self:Asteroid,dt:float<s>) = 
  [x | x <- world.Projectiles, 
       distance(x.Position,self.Position) < 10.0<m>)]

type Projectile = {
  Position    : Vector2<m>
  Velocity    : Vector2<m/s>
  Colliders   : List<Asteroid>
}
rule Position(world:World,self:Projectile,dt:float<s>) = 
  Position + Velocity * dt
rule Colliders(world:World,self:Projectile,dt:float<s>) = 
  [x | x <- world.Asteroids, 
       distance(x.Position,self.Position) < 10.0<m>]

type Cannon = {
  X           : Var<float<m>>
}

let initial_world = {
  Asteroids   = []
  Projectiles = []
  Cannon      = { X = 50.0<m> }
}

let rec main (world:World) = 
  repeat {
    wait (random(0.1<s>,1.0<s>))
    world.Asteroids.Add(
      {
        Position  = Vector2(random(0.0<m>,100.0<m>), 100.0<m>)
        Velocity  = Vector2(0.0<m>, random(1.0<m>,-10.0<m>))
        Colliders = []
      }
  }

let input world = 
  {
    if IsKeyDown(Keys.Space) then
      return Some()
    else
      return None
  } ==> fun () -> {
    world.Projectiles.Add(
      {
        Position  = Vector2(world.Cannon.X, 0.0<m>)
        Velocity  = Vector2(0.0<m>, 20.0<m>)
        Colliders = []
      }        
    wait 0.1<s>
  },
  {
    if IsKeyDown(Keys.Left) then
      return Some(-1.0<m>)
    elif IsKeyDown(Keys.Right) then
      return Some(1.0<m>)
    else
      None
  } ==> fun dx -> {
    world.Cannon.X <- world.Cannon.X + dx
  }


// SAMPLE 2:  
// code reuse with contracts (interface + type class) for asteroids and projectiles
contract Collider<'Collidee:Collider,
                  GetColliders:World->List<'Collidee>> = {
  Position    : Vector2<m>
  Velocity    : Vector2<m/s>
  Colliders   : List<'Collidee>
}
rule Position(world:World,self:Asteroid,dt:float<s>) = 
  Position + Velocity * dt
rule Colliders(world:World,self:Asteroid,dt:float<s>) = 
  [x | x <- GetColliders(world), 
       distance(x.Position,self.Position) < 10.0<m>]

type Asteroid   = Collider<Projectile, fun world -> world.Projectiles>
type Projectile = Collider<Asteroid, fun world -> world.Asteroids>


// SAMPLE 3:
// simple 2D rendering for background, cannon, etc.
// assumes same definitions as above
// DrawableSprite (and DrawableText) are intrinsic definitions
// NOTE: 2D rendering may also use Z attribute for Z-ordering 
// to define what appears above what (0 = close, 1 = far)
// rendering respects order of fields when Z is equal

type World = {
  Background    : DrawableSprite
  ...
}

type Cannon = {
  ...
  Sprite        : DrawableSprite
}
rule Sprite(world:World,self:Cannon,dt:float<s>) =
  { Position = Vector2(self.X,10.0)
    Scale    = self.Sprite.Scale
    Rotation = self.Sprite.Rotation
    Path     = self.Sprite.Path
    Tint     = self.Sprite.Tint
    Z        = self.Sprite.Z }

contract Collider<'Collidee:Collider,
                  GetColliders:World->List<'Collidee>> = {
  ...
  Sprite        : DrawableSprite
}
rule Sprite(world:World, self:Cannon,dt:float<s>) = 
  ... // same as Cannon above

let initial_world = {
  Background  = { 
      Position = Vector2.Zero
      Scale    = Vector2.One
      Rotation = 0.0
      Path     = "Sprites\Background.jpg"
      Tint     = Color.White
      Z        = 1.0 } 
    }
  Asteroids   = []
  Projectiles = []
  Cannon      = { X      = 50.0<m>
                  Sprite = { 
                    Position = Vector2.Zero
                    Scale    = Vector2.One
                    Rotation = 0.0
                    Path     = "Sprites\Cannon.jpg"
                    Tint     = Color.White
                    Z        = 0.0 } 
                }
}


// SAMPLE 4:
// new syntax for rules; can update just one attribute in a rule
// rules defined this way must not overlap attributes
// introducing default parameters for drawable entities

type Cannon = {
  ...
  Sprite        : DrawableSprite
}
rule Sprite.Position(world:World,self:Cannon,dt:float<s>) =
  Vector2(self.X,10.0)


// SAMPLE 5:
// until now we have yet to specify the mapping from simulation meters 
// to pixels; we introduce sprite layers which contain sets of sprites
// rendered together with specific options such as alpha blending,
// alpha testing, cull options, a projection matrix and other attributes
// that may depend on the underlying API (unlocked by compiler setting?
// many default values?)
// NOTE: SpriteLayer.Transform may be 3D View * Projection for 3D drawing

type World = {
  Sprites       : SpriteLayer
  ...
}

// all 2D entities have a new field of type SpriteLayer (set only once if it does not change, otherwise use rules just as before)
let initial_world = 
  let sprites_layer = { 
      Transform   = Matrix.CreateScale(0.01) * Matrix.CreateOrthogonal(1024.0,768.0)
      AlphaBlend  = true
      AlphaTest   = true
      ... // various other rendering attributes; may depend on underlying API
    }
  {
    Sprites     = sprites_layer
    Background  = { 
        ...
        Layer   = sprites_layer } 
      }
    Asteroids   = []
    Projectiles = []
    Cannon      = { X      = 50.0<m>
                    Sprite = { 
                      ...
                      Layer   = sprites_layer } 
                  }
  }


// SAMPLE 6:
// simple 3D rendering with models and meshes
// we ignore 2D rendering for the moment, just 3D
// NOTE: Camera performs the actual rendering;
// when Draw routine encounters a DrawableModel then
// it simply adds this model to a hierarchy of models
// inside the camera; the models inside each Camera are
// then rendered after all entities have been processed;
// Camera also performs culling of its models

type World = {
  MainCamera    : Camera
  ...
}

type Cannon = {
  Model         : DrawableModel
  ...
}
rule Model.Position(world:World,self:Cannon,dt:float<s>) =
  Vector3(self.X,10.0,0.0)


let initial_world = 
  let camera = { 
      View        = Matrix.CreateLookAt(...)
      Projection  = Matrix.CreatePerspective(...)
    }
  {
    MainCamera  = camera
    Asteroids   = []
    Projectiles = []
    Cannon      = { X      = 50.0<m>
                    Model  = { 
                      ...
                      Camera   = camera } 
                  }
  }


// SAMPLE 7:
// we use layers to mix different rendering parameters
// for example, we could use a layer for rendering 2D sprites,
// another for rendering 3D models with transparency, another
// for rendering 3D models without transparency and a final one
// to draw the GUI in 2D; a layer field is added to the Camera
// to specify which layer its models are rendered with
// layers are rendered in the order they are found in the state
// NOTE: render targets are optional fields in all layers, and may also
// be rendered as a DrawableRenderTarget, which changes the Path field
// into a RenderTarget field

type World = {
  Sprites     : SpritesLayer
  Models      : EffectLayer
  UI          : SpritesLayer
  MainCamera  : Camera
  ...
}

...

let initial_world = 
  let sprites_layer = ...
  let ui_layer = ...
  let models_layer = {
      Effect      = "Effects\SimpleEffect.fx"
      Technique   = "Technique0"
      Pass        = "Pass0"
      ... // effect parameters
      AlphaBlend  = false
      ... // various other rendering options; use defaults or "verbose" compiler options
    }

  let camera = { 
    ...
    Layer    = models_layer }

  {
    Sprites     = sprites_layer
    Models      = models_layer
    UI          = ui_layer
    MainCamera  = camera
    ...
  }
