using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using AsteroidShooter;

namespace Test
{
  /// <summary>
  /// This is the main type for your game.
  /// </summary>
  public class Game1 : Game
  {
    GraphicsDeviceManager graphics;
    SpriteBatch spriteBatch;
    Texture2D shipTexture, greenLaser, rockAsteroid;
    World world;

    public Game1()
    {
      graphics = new GraphicsDeviceManager(this);
      Content.RootDirectory = "Content";
    }

    /// <summary>
    /// Allows the game to perform any initialization it needs to before starting to run.
    /// This is where it can query for any required services and load any non-graphic
    /// related content.  Calling base.Initialize will enumerate through any components
    /// and initialize them as well.
    /// </summary>
    protected override void Initialize()
    {
      // TODO: Add your initialization logic here
      world = new World();
      world.Start();
      base.Initialize();
    }

    /// <summary>
    /// LoadContent will be called once per game and is the place to load
    /// all of your content.
    /// </summary>
    protected override void LoadContent()
    {
      // Create a new SpriteBatch, which can be used to draw textures.
      spriteBatch = new SpriteBatch(GraphicsDevice);
      shipTexture = Content.Load<Texture2D>("starship.png");
      greenLaser = Content.Load<Texture2D>("laser_green.png");
      rockAsteroid = Content.Load<Texture2D>("asteroid.png");

      // TODO: use this.Content to load your game content here
    }

    /// <summary>
    /// UnloadContent will be called once per game and is the place to unload
    /// game-specific content.
    /// </summary>
    protected override void UnloadContent()
    {
      // TODO: Unload any non ContentManager content here
    }

    /// <summary>
    /// Allows the game to run logic such as updating the world,
    /// checking for collisions, gathering input, and playing audio.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Update(GameTime gameTime)
    {
      if (Keyboard.GetState().IsKeyDown(Keys.Escape))
      {
        this.Exit();
      }

      // TODO: Add your update logic here
      float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
      world.Update(dt, world);
      base.Update(gameTime);
    }

    /// <summary>
    /// This is called when the game should draw itself.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Draw(GameTime gameTime)
    {
      GraphicsDevice.Clear(Color.CornflowerBlue);
      float screenscale =
        (float)GraphicsDevice.Viewport.Width / GraphicsDevice.DisplayMode.Width;
      Matrix scale = Matrix.CreateScale(screenscale, screenscale, 1);

      spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, scale);

      //draw ship
      spriteBatch.Draw(shipTexture, world.Ship.Position);

      //draw projectiles
      foreach (Projectile projectile in world.Ship.Projectiles)
        spriteBatch.Draw(greenLaser, projectile.Position, null, Color.White, MathHelper.PiOver2, Vector2.Zero, 1f, SpriteEffects.None, 0f);

      //draw asteroids
      foreach (Asteroid asteroid in world.Asteroids)
        spriteBatch.Draw(rockAsteroid, asteroid.Position, null, Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0f);

      spriteBatch.End();

      // TODO: Add your drawing code here

      base.Draw(gameTime);
    }
  }
}
