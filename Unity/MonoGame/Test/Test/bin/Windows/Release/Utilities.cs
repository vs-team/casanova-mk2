using Lidgren.Network;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using AsteroidShooter;

namespace Utilities
{

  public static class Random
  {
    private static System.Random generator = new System.Random();

    public static float RandFloat(float min, float max)
    {
      return min + (float)generator.NextDouble() * (max - min);
    }

    public static int RandInt(int min, int max)
    {
      return generator.Next(min, max);
    }
  }

  public class NetworkInfo<T>
  {
    public T Entity { get; set; }
    public bool Connected { get; set; }
    public bool Disconnected { get; set; }
    public bool IsLocal { get; set; }
    public string EntityName { get; set; }

    public NetworkInfo()
    {
      Entity = default(T);
      Connected = false;
      Disconnected = false;
      IsLocal = false;
      EntityName = "";
    }

    public NetworkInfo(T entity, string name, bool local)
    {
      Entity = entity;
      Connected = false;
      Disconnected = false;
      IsLocal = local;
      EntityName = name;
    }
  }

  public class NetworkAPI
  {
    public static NetClient Client;
    public static int NextID
    {
      get
      {
        int r = Random.RandInt(0, 1000000000);
        return r.GetHashCode();
      }
    }

    public static Dictionary<int, NetworkInfo<Ship>> ShipInfos = new Dictionary<int, NetworkInfo<Ship>>();
    public static Dictionary<int, NetworkInfo<Projectile>> ProjectileInfos = new Dictionary<int, NetworkInfo<Projectile>>();
    public static Dictionary<int, NetworkInfo<Asteroid>> AsteroidInfos = new Dictionary<int, NetworkInfo<Asteroid>>();

    public static void AddShipInfo(int id, NetworkInfo<Ship> info)
    {
      ShipInfos[id] = info;
    }

    public static void AddProjectileInfo(int id, NetworkInfo<Projectile> info)
    {
      ProjectileInfos[id] = info;
    }
    
    public static void AddAsteroidInfo(int id, NetworkInfo<Asteroid> info)
    {
      AsteroidInfos[id] = info;
    }
  }
}