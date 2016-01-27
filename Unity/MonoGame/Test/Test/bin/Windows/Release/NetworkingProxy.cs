using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Utilities;


  public class NetworkWorld
  {
    public List<NetworkShip> Ships { get; set; }
    public List<NetworkAsteroid> Asteroids { get; set; }
    int Id;

    public NetworkWorld()
    {
      Ships = new List<NetworkShip>();
      Ships.Add(new NetworkShip(new Vector2(Random.RandFloat(250.0f, 1500.0f), 1000.0f)));
      Asteroids = new List<NetworkAsteroid>();
      Id = NetworkAPI.NextID;
    }
  }

  public class NetworkShip
  {
    public Vector2 Position { get; set; }
    public Color Color { get; set; }
    public int Health { get; set; }
    public List<NetworkProjectile> Projectiles { get; set; }
    int Id;

    public NetworkShip(Vector2 position)
    {
      Position = position;
      Color = new Color(Random.RandInt(0, 256), Random.RandInt(0, 256), Random.RandInt(0, 256));
      Health = 100;
      Projectiles = new List<NetworkProjectile>();
      Id = NetworkAPI.NextID;
    }
  }

  public class NetworkProjectile
  {
    public Vector2 Position { get; set; }
    int Id;

    public NetworkProjectile(Vector2 position)
    {
      Position = position;
      Id = NetworkAPI.NextID;
    }
  }

  public class NetworkAsteroid
  {
    public Vector2 Position { get; set; }
    public float Damage { get; set; }
    int Id;

    public NetworkAsteroid(Vector2 position, float damage)
    {
      Position = position;
      Damage = damage;
      Id = NetworkAPI.NextID;
    }
  }
