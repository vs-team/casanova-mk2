using System;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
//using AsteroidShooter;
using Utilities;

namespace GameNetworking
{

  public class Pair<A, B>
  {
    A first;
    B second;

    public Pair(A a, B b)
    {
      first = a;
      second = b;
    }

    public A getFst
    {
      get { return first; }
    }

    public B getSnd
    {
      get { return second; }
    }
  }

  public static class NetworkUtils
  {
    public enum AtomicDataType { IntType, FloatType, StringType, BoolType }
    public enum CompositeDataType { Vector2Type, ShipType, ProjType, AsteroidType, NetInfoType }
    public enum MessageType { BasicType, CompositeType }
    public enum DataFormat { List, Element }

    public static Pair<bool, int> getTypeIdentity<T>(T element)
    {
      var elT = element.GetType();
      if (elT == typeof(int))
      {
        return new Pair<bool, int>(false, ((int)AtomicDataType.IntType));
      }
      else if (elT == typeof(bool))
      {
        return new Pair<bool, int>(false, ((int)AtomicDataType.BoolType));
      }
      else if (elT == typeof(string))
      {
        return new Pair<bool, int>(false, ((int)AtomicDataType.StringType));
      }
      else if (elT == typeof(float))
      {
        return new Pair<bool, int>(false, ((int)AtomicDataType.FloatType));
      }
      else if (elT == typeof(Vector2))
      {
        return new Pair<bool, int>(true, ((int)CompositeDataType.Vector2Type));
      }
      //else if (elT == typeof(Ship))
      //{
      //  return new Pair<bool, int>(true, ((int)CompositeDataType.ShipType));
      //}
      //else if (elT == typeof(Projectile))
      //{
      //  return new Pair<bool, int>(true, ((int)CompositeDataType.ProjType));
      //}
      //else if (elT == typeof(Asteroid))
      //{
      //  return new Pair<bool, int>(true, ((int)CompositeDataType.AsteroidType));
      //}
      else
      {
        throw new ArgumentException("Unsupported Identity Type");
      }

    }

    public static void Send<T>(T element, NetClient client, Func<T, NetClient, bool, NetOutgoingMessage> messageFunction)
    {
      NetOutgoingMessage message = messageFunction(element, client, true);
      client.SendMessage(message, NetDeliveryMethod.UnreliableSequenced);
    }

    public static void Send<T>(List<T> list, NetClient client, Func<T, NetClient, bool, NetOutgoingMessage> messageFunction)
    {
      NetOutgoingMessage message = BuildMessage<T>(list, client, false, messageFunction);
      client.SendMessage(message, NetDeliveryMethod.UnreliableSequenced);
    }

    public static T Receive<T>(NetIncomingMessage message, Func<NetIncomingMessage, T> receiveFunction)
    {
      return receiveFunction(message);
    }

    public static NetOutgoingMessage BuildMessage(Vector2 v, NetClient client, bool createHeader)
    {
      NetOutgoingMessage message = client.CreateMessage();
      if (createHeader)
      {
        message.Write((int)DataFormat.Element);
        message.Write((int)MessageType.CompositeType);
        message.Write((int)CompositeDataType.Vector2Type);
      }
      message.Write(v.X);
      message.Write(v.Y);
      return message;
    }

    public static NetOutgoingMessage BuildMessage(float f, NetClient client, bool createHeader)
    {
      NetOutgoingMessage message = client.CreateMessage();

      if (createHeader)
      {
        message.Write((int)DataFormat.Element);
        message.Write((int)MessageType.BasicType);
        message.Write((int)AtomicDataType.FloatType);
      }
      message.Write(f);
      return message;
    }

    public static NetOutgoingMessage BuildMessage(string s, NetClient client, bool createHeader)
    {
      NetOutgoingMessage message = client.CreateMessage();
      if (createHeader)
      {
        message.Write((int)DataFormat.Element);
        message.Write((int)MessageType.BasicType);
        message.Write((int)AtomicDataType.StringType);
      }
      message.Write(s);
      return message;
    }

    public static NetOutgoingMessage BuildMessage(int i, NetClient client, bool createHeader)
    {
      NetOutgoingMessage message = client.CreateMessage();
      if (createHeader)
      {
        message.Write((int)DataFormat.Element);
        message.Write((int)MessageType.BasicType);
        message.Write((int)AtomicDataType.IntType);
      }
      message.Write(i);
      return message;
    }

    public static NetOutgoingMessage BuildMessage<T>(List<T> list, NetClient client, bool e, Func<T, NetClient, bool, NetOutgoingMessage> messageFunction)
    {
      NetOutgoingMessage message = client.CreateMessage();
      message.Write((int)DataFormat.List);
      Pair<bool, int> pair = getTypeIdentity<T>(list.ElementAtOrDefault(1));
      if (pair.getFst)
      {
        message.Write((int)MessageType.CompositeType);
        message.Write(pair.getSnd);
      }
      else
      {
        message.Write((int)MessageType.BasicType);
        message.Write(pair.getSnd);
      }
      message.Write(list.Count);
      for (int i = 0; i < list.Count; i++)
      {
        NetOutgoingMessage tempMessage = messageFunction(list[i], client, e);
        message.Write(tempMessage);
      }
      return message;
    }

    public static NetOutgoingMessage BuildMessage<T>(T element, NetClient client, bool e)
    {
      NetOutgoingMessage message = client.CreateMessage();
      message.Write((int)DataFormat.Element);
      if (e)
      {
        Pair<bool, int> pair = getTypeIdentity<T>(element);
        if (pair.getFst)
        {
          message.Write((int)MessageType.CompositeType);
          message.Write(pair.getSnd);
        }
        else
        {
          message.Write((int)MessageType.BasicType);
          message.Write(pair.getSnd);
        }
      }
      NetOutgoingMessage finalMessage = NetworkUtils.BuildMessage(element, client, e);
      message.Write(finalMessage);
      return message;
    }

    //public static NetOutgoingMessage BuildMessage(Ship ship, NetClient client, bool createHeader)
    //{
    //  NetOutgoingMessage message = client.CreateMessage();
    //  if (createHeader)
    //  {
    //    message.Write((int)DataFormat.Element);
    //    message.Write((int)MessageType.CompositeType);
    //    message.Write((int)CompositeDataType.ShipType);
    //  }
    //  NetOutgoingMessage infoMessage = BuildMessage(NetworkAPI.ShipInfos[ship.Id], client, false);
    //  NetOutgoingMessage tempMessage = BuildMessage(ship.Position, client, false);
    //  message.Write(infoMessage);
    //  message.Write(tempMessage);
    //  message.Write(ship.Score);
    //  message.Write(ship.Color.R);
    //  message.Write(ship.Color.G);
    //  message.Write(ship.Color.B);
    //  message.Write(ship.Health);
    //  message.Write(ship.Id);
    //  return message;
    //}

    //public static NetOutgoingMessage BuildMessage(Projectile proj, NetClient client, bool createHeader)
    //{
    //  NetOutgoingMessage message = client.CreateMessage();
    //  if (createHeader)
    //  {
    //    message.Write((int)DataFormat.Element);
    //    message.Write((int)MessageType.CompositeType);
    //    message.Write((int)CompositeDataType.ProjType);
    //  }
    //  NetOutgoingMessage infoMessage = BuildMessage(NetworkAPI.ProjectileInfos[proj.Id], client, false);
    //  NetOutgoingMessage tempMessage = BuildMessage(proj.Position, client, false);
    //  message.Write(infoMessage);
    //  message.Write(tempMessage);
    //  message.Write(proj.Owner.Id);
    //  message.Write(proj.Id);
    //  return message;
    //}

    //public static NetOutgoingMessage BuildMessage(Asteroid asteroid, NetClient client, bool createHeader)
    //{
    //  NetOutgoingMessage message = client.CreateMessage();
    //  if (createHeader)
    //  {
    //    message.Write((int)DataFormat.Element);
    //    message.Write((int)MessageType.CompositeType);
    //    message.Write((int)CompositeDataType.AsteroidType);
    //  }
    //  NetOutgoingMessage infoMessage = BuildMessage(NetworkAPI.AsteroidInfos[asteroid.Id], client, false);
    //  NetOutgoingMessage tempMessage = BuildMessage(asteroid.Position, client, false);
    //  message.Write(infoMessage);
    //  message.Write(tempMessage);
    //  message.Write(asteroid.Damage);
    //  message.Write(asteroid.Id);
    //  return message;
    //}

    public static NetOutgoingMessage SendNetworkInfo<T>(T element, NetworkInfo<T> info, NetClient client, bool createHeader)
    {
      NetOutgoingMessage message = client.CreateMessage();
      if (createHeader)
      {
        message.Write((int)DataFormat.Element);
        message.Write((int)MessageType.CompositeType);
        message.Write((int)CompositeDataType.NetInfoType);
      }
      Pair<bool, int> pair = getTypeIdentity(element);
      message.Write(pair.getSnd);
      message.Write(info.Connected);
      message.Write(info.Disconnected);
      message.Write(info.EntityName);
      return message;
    }

    public static NetworkInfo<T> ReceiveNetworkInfo<T>(NetIncomingMessage message)
    {
      bool connected = message.ReadBoolean();
      bool disconnected = message.ReadBoolean();
      string name = message.ReadString();
      NetworkInfo<T> info = new NetworkInfo<T>();
      info.Connected = connected;
      info.Disconnected = disconnected;
      return info;
    }

    //public static Ship ReceiveShip(NetIncomingMessage message)
    //{
    //  Ship ship = new Ship(Vector2.Zero);
    //  NetworkInfo<Ship> info = ReceiveNetworkInfo<Ship>(message);
    //  Vector2 position = ReceiveVector2(message);
    //  int score = message.ReadInt32();
    //  byte r = message.ReadByte();
    //  byte g = message.ReadByte();
    //  byte b = message.ReadByte();
    //  Color color = new Color(r, g, b);
    //  int health = message.ReadInt32();
    //  int id = message.ReadInt32();
    //  ship.Position = position;
    //  ship.Score = score;
    //  ship.Color = color;
    //  ship.Health = health;
    //  ship.Id = id;
    //  if (!NetworkAPI.ShipInfos.ContainsKey(id))
    //  {
    //    NetworkAPI.ShipInfos[id] = info;
    //  }
    //  return ship;
    //}

    //public static Projectile ReceiveProjectile(NetIncomingMessage message, World world)
    //{
    //  Projectile proj = new Projectile(Vector2.Zero, null);
    //  NetworkInfo info = ReceiveNetworkInfo(message);
    //  Vector2 position = ReceiveVector2(message);
    //  int ownerId = message.ReadInt32();
    //  int id = message.ReadInt32();
    //  proj.Position = position;
    //  proj.Owner = world;
    //}

    public static Vector2 ReceiveVector2(NetIncomingMessage message)
    {
      float x = message.ReadFloat();
      float y = message.ReadFloat();
      return new Vector2(x, y);
    }

    public static float ReceiveFloat(NetIncomingMessage message)
    {
      float f = message.ReadFloat();
      return f;
    }

    public static int ReceiveInt(NetIncomingMessage message)
    {
      int i = message.ReadInt32();
      return i;
    }

    public static string ReceiveString(NetIncomingMessage message)
    {
      string s = message.ReadString();
      return s;
    }

    public static List<T> ReceiveList<T>(NetIncomingMessage message, Func<NetIncomingMessage, T> decodeFunction)
    {
      int length = message.ReadInt32();
      List<T> list = new List<T>(length);
      for (int i = 0; i < length; i++)
      {
        list.Add(decodeFunction(message));
      }
      return list;
    }
  }
}


//Uitbreiden van het pair, Pair(A, Pair(B, C) => A = boolean isComposite, B = int DataType, C = Corresponding messagefunction for type