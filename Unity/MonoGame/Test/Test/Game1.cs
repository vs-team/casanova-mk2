using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Lidgren.Network;
using AsteroidShooter;
using Utilities;

namespace Test
{
  /// <summary>
  /// This is the main type for your game.
  /// </summary>
  public class Game1 : Game
  {
    GraphicsDeviceManager graphics;
    SpriteBatch spriteBatch;
    Texture2D shipTexture, greenLaser, rockAsteroid, background;
    World world;
    SpriteFont arial;
    System.Random r = new System.Random();


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
      NetPeerConfiguration config = new NetPeerConfiguration("AsteroidShooter");
      NetworkAPI.Client = new NetClient(config);
      NetworkAPI.Client.Start();
      NetworkAPI.Client.Connect("127.0.0.1", 5432);
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
      background = Content.Load<Texture2D>("nebula.jpg");
      arial = Content.Load<SpriteFont>("Arial");

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

      //debug
      //if (Keyboard.GetState().IsKeyDown(Keys.LeftControl) && Keyboard.GetState().IsKeyDown(Keys.D))
      //{
      //  System.Console.WriteLine("Entity\tId\tConnected\tDisconnected\tLocal");
      //  foreach (System.Collections.Generic.KeyValuePair<int, NetworkInfo> kv in NetworkAPI.NetworkInfos)
      //  {
      //    System.Console.WriteLine(kv.Value.EntityName + "\t" + kv.Key + "\t" + kv.Value.Connected + "\t" + kv.Value.Disconnected + "\t" + kv.Value.IsLocal);
      //  }
      //}

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

      //draw background
      spriteBatch.Draw(background, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

      //draw scores
      for (int i = 0; i < world.Ships.Count; i++)
      {
        spriteBatch.DrawString(arial, "Player " + i + ":" + world.Ships[i].Score.ToString(), new Vector2(GraphicsDevice.DisplayMode.Width * 0.8f, 100f * i), Color.Red, 0f, Vector2.Zero, 5f, SpriteEffects.None, 0f);
        spriteBatch.DrawString(arial, "Health " + i + ":" + world.Ships[i].Health.ToString(), new Vector2(GraphicsDevice.DisplayMode.Width * 0.05f, 50f * i), Color.Red, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0f);
      }

      //draw ship
      foreach (Ship ship in world.Ships)
      {
        spriteBatch.Draw(shipTexture, ship.Position, null, ship.Color);
        //draw projectiles
        foreach (Projectile projectile in ship.Projectiles)
          spriteBatch.Draw(greenLaser, projectile.Position, null, Color.White, MathHelper.PiOver2, Vector2.Zero, 2f, SpriteEffects.None, 0f);
      }
      
      //draw asteroids
      foreach (Asteroid asteroid in world.Asteroids)
        spriteBatch.Draw(rockAsteroid, asteroid.Position, null, Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0f);

      spriteBatch.End();

      // TODO: Add your drawing code here

      base.Draw(gameTime);
    }
  }
}
