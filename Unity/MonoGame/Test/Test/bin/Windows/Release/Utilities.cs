using Lidgren.Network;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using AsteroidShooter;
using GameNetworking;

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
    public bool IsLocal { get; set; }

    public NetworkInfo()
    {
      Entity = default(T);
      IsLocal = false;
    }

    public NetworkInfo(T entity, bool local)
    {
      Entity = entity;
      IsLocal = local;
    }
  }

  public static class NetworkAPI
  {
    public static NetClient Client;
    public enum MessageType { Create, Update }
    public enum EntityType { World, Ship, Asteroid, Projectile }

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
    public static Dictionary<EntityType, Dictionary<MessageType, Dictionary<int, NetIncomingMessage>>> ReceivedMessages = new Dictionary<EntityType, Dictionary<MessageType, Dictionary<int, NetIncomingMessage>>>();


    /*protocol:
      -- Full synchronization
      item 0 = message type
      item 1 = entity type
      item 2 = entity id
      rest = fields

      -- Partial synchronization
      item 0 = message type
      item 1 = entity type
      item 2 = entity id
      item 3 = field id
      rest = field value
    */


    /* Order of fields: Asteroids, Ships */

    public static NetOutgoingMessage CreateWorldMessage(World world, NetClient client)
    {
      NetOutgoingMessage message = client.CreateMessage();
      message.Write((int)MessageType.Create);
      message.Write((int)EntityType.World);
      message.Write(world.ID);
      NetOutgoingMessage asteroidsMessage = NetworkUtils.BuildMessage<Asteroid>(world.Asteroids, client, CreateAsteroidMessage);
      NetOutgoingMessage shipsMessage = NetworkUtils.BuildMessage<Ship>(world.Ships, client, CreateShipMessage);
      message.Write(asteroidsMessage);
      message.Write(shipsMessage);
      return message;
    }

    public static NetOutgoingMessage CreateWorldMessage(World world, NetClient client, int fieldId)
    {
      NetOutgoingMessage message = client.CreateMessage();
      message.Write((int)MessageType.Update);
      message.Write((int)EntityType.World);
      message.Write(world.ID);
      NetOutgoingMessage asteroidsMessage;
      NetOutgoingMessage shipsMessage;
      message.Write(fieldId);
      switch (fieldId)
      {
        case 0:
          asteroidsMessage = NetworkUtils.BuildMessage<Asteroid>(world.Asteroids, client, CreateAsteroidMessage);
          message.Write(asteroidsMessage);
          break;
        case 1:
          shipsMessage = NetworkUtils.BuildMessage<Ship>(world.Ships, client, CreateShipMessage);
          message.Write(shipsMessage);
          break;
        default:
          throw new System.ArgumentException("Unsupported field in World" + fieldId);
      }
      return message;
    }

    /* Order of fields: score, projectile list, position, health, color R, color G, color B */

    public static NetOutgoingMessage CreateShipMessage(Ship ship, NetClient client)
    {
      NetOutgoingMessage message = client.CreateMessage();
      message.Write((int)MessageType.Create);
      message.Write((int)EntityType.Ship);
      message.Write(ship.ID);
      NetOutgoingMessage projMessage = NetworkUtils.BuildMessage<Projectile>(ship.Projectiles, client, CreateProjectileMessage);
      NetOutgoingMessage posMessage = NetworkUtils.BuildMessage(ship.Position, client);
      message.Write(ship.Score);
      message.Write(projMessage);
      message.Write(posMessage);
      message.Write(ship.Health);
      message.Write(ship.Color.R);
      message.Write(ship.Color.G);
      message.Write(ship.Color.B);
      return message;
    }

    public static NetOutgoingMessage CreateShipMessage(Ship ship, NetClient client, int fieldID)
    {
      NetOutgoingMessage message = client.CreateMessage();
      message.Write((int)MessageType.Update);
      message.Write((int)EntityType.Ship);
      message.Write(ship.ID);
      NetOutgoingMessage projectilesMessage;
      NetOutgoingMessage posMessage;
      message.Write(fieldID);
      switch(fieldID)
      {
        case 0:
          message.Write(ship.Score);
          break;
        case 1:
          projectilesMessage = NetworkUtils.BuildMessage<Projectile>(ship.Projectiles, client, CreateProjectileMessage);
          message.Write(projectilesMessage);
          break;
        case 2:
          posMessage = NetworkUtils.BuildMessage(ship.Position, client);
          message.Write(posMessage);
          break;
        case 3:
          message.Write(ship.Health);
          break;
        case 4:
          message.Write(ship.Color.R);
          message.Write(ship.Color.G);
          message.Write(ship.Color.B);
          break;
        default:
          throw new System.ArgumentException("Unsupported field in Ship: " + fieldID);
      }
      return message;
    }


    /* Order of fields: position, owner ref */

    public static NetOutgoingMessage CreateProjectileMessage(Projectile proj, NetClient client)
    {
      NetOutgoingMessage message = client.CreateMessage();
      message.Write((int)MessageType.Create);
      message.Write((int)EntityType.Projectile);
      message.Write(proj.ID);
      NetOutgoingMessage vectorMessage = NetworkUtils.BuildMessage(proj.Position, client);
      message.Write(vectorMessage);
      message.Write(proj.Owner.ID);
      return message;
    }

    public static NetOutgoingMessage CreateProjectileMessage(Projectile proj, NetClient client, int fieldId)
    {
      NetOutgoingMessage message = client.CreateMessage();
      message.Write((int)MessageType.Update);
      message.Write((int)EntityType.Projectile);
      message.Write(proj.ID);
      NetOutgoingMessage posMessage;
      message.Write(fieldId);
      switch(fieldId)
      {
        case 0:
          posMessage = NetworkUtils.BuildMessage(proj.Position, client);
          message.Write(posMessage);
          break;
        default:
          throw new System.ArgumentException("Unsupported field in Projectile: " + fieldId);
      }
      return message;
    }

    /* Order of fields: position, damage*/
    public static NetOutgoingMessage CreateAsteroidMessage(Asteroid asteroid, NetClient client)
    {
      NetOutgoingMessage message = client.CreateMessage();
      message.Write((int)MessageType.Create);
      message.Write((int)EntityType.Asteroid);
      message.Write(asteroid.ID);
      NetOutgoingMessage vectorMessage = NetworkUtils.BuildMessage(asteroid.Position, client);
      message.Write(vectorMessage);
      message.Write(asteroid.Damage);
      return message;
    }
    
    public static NetOutgoingMessage CreateAsteroidMessage(Asteroid asteroid, NetClient client, int fieldId)
    {
      NetOutgoingMessage message = client.CreateMessage();
      message.Write((int)MessageType.Update);
      message.Write((int)EntityType.Asteroid);
      message.Write(asteroid.ID);
      NetOutgoingMessage posMessage;
      message.Write(fieldId);
      switch(fieldId)
      {
        case 0:
          posMessage = NetworkUtils.BuildMessage(asteroid.Position, client);
          message.Write(posMessage);
          break;
        case 1:
          message.Write(asteroid.Damage);
          break;
        default:
          throw new System.ArgumentException("Unsupported field in Asteroid: " + fieldId);
      }
      return message;
    }

    public static bool ContainsRightMessage(EntityType entityType, MessageType messageType)
    {
      return ReceivedMessages.ContainsKey(entityType) && ReceivedMessages[entityType].ContainsKey(messageType);
    }

    public static NetIncomingMessage FindRightMessage(EntityType entityType, MessageType messageType, int id)
    {
      return ReceivedMessages[entityType][messageType][id];
    }

    public static Asteroid ReceiveNewAsteroidMessage(int id)
    {
      NetIncomingMessage message = FindRightMessage(EntityType.Asteroid, MessageType.Create, id);
      Vector2 position = NetworkUtils.ReceiveVector2(message);
      int damage = message.ReadInt32();
      Asteroid asteroid = new Asteroid(position);
      asteroid.Damage = damage;
      AsteroidInfos.Add(id, new NetworkInfo<Asteroid>(asteroid, false));
      return asteroid;
    }

    public static Asteroid ReceiveUpdatedAsteroidMessage(int id)
    {

    }

    public static void DispatchMessages(NetClient client)
    {
      List<NetIncomingMessage> messages = new List<NetIncomingMessage>();
      client.ReadMessages(messages);
      for (int i = 0; i < messages.Count; i++)
      {
        if (messages[i].MessageType == NetIncomingMessageType.Data)
        {
          MessageType messageType = (MessageType)messages[i].ReadInt32();
          EntityType entityType = (EntityType)messages[i].ReadInt32();
          int entityId = messages[i].ReadInt32();
          ReceivedMessages[entityType][messageType][entityId] = messages[i];
        }
      }
    }
  }
}