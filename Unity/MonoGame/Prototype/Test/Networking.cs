using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;
using Utilities;
using Prototype;

namespace GameNetworking
{
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
    public enum MessageType { NewConnection, Create, Update }
    public enum EntityType { World, Ship }

    public struct MessageInfo
    {
      public int EntityID;
      public NetIncomingMessage Message;

      public MessageInfo(int id, NetIncomingMessage message)
      {
        EntityID = id;
        Message = message;
      }
    }

    public static int NextID
    {
      get
      {
        int r = Utilities.Random.RandInt(0, 1000000000);
        return r.GetHashCode();
      }
    }

    public static Dictionary<int, NetworkInfo<Ship>> ShipInfos = new Dictionary<int, NetworkInfo<Ship>>();
    public static Dictionary<Tuple<MessageType, EntityType, int>, MessageInfo> ReceivedMessages = new Dictionary<Tuple<MessageType, EntityType, int>, MessageInfo>();


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

    /* Order of fields: position, color R, color G, color B */

    public static NetOutgoingMessage CreateShipMessage(Ship ship, NetClient client)
    {
      NetOutgoingMessage message = client.CreateMessage();
      message.Write((int)MessageType.Create);
      message.Write((int)EntityType.Ship);
      message.Write(ship.ID);
      NetOutgoingMessage posMessage = NetworkUtils.BuildMessage(ship.Position, client);
      message.Write(posMessage);
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
      NetOutgoingMessage posMessage;
      message.Write(fieldID);
      switch (fieldID)
      {
        case 0:
          posMessage = NetworkUtils.BuildMessage(ship.Position, client);
          message.Write(posMessage);
          break;
        case 1:
          message.Write(ship.Color.R);
          message.Write(ship.Color.G);
          message.Write(ship.Color.B);
          break;
        default:
          throw new System.ArgumentException("Unsupported field in Ship: " + fieldID);
      }
      return message;
    }

    public static List<Ship> ReceiveShipMessage()
    {
      List<Ship> ships = new List<Ship>();
      List<int> processedKeys = new List<int>();
      foreach (var kv in ReceivedMessages)
      {
        if (kv.Key.Item1 == MessageType.Create && kv.Key.Item2 == EntityType.Ship)
        {
          MessageInfo info = ReceivedMessages[kv.Key];
          processedKeys.Add(kv.Key.Item3);
          if (!ShipInfos.ContainsKey(info.EntityID))
          {
            NetIncomingMessage message = info.Message;
            float x = message.ReadFloat();
            float y = message.ReadFloat();
            byte r = message.ReadByte();
            byte g = message.ReadByte();
            byte b = message.ReadByte();
            Microsoft.Xna.Framework.Vector2 position = new Microsoft.Xna.Framework.Vector2(x, y);
            Microsoft.Xna.Framework.Color color = new Microsoft.Xna.Framework.Color(r, g, b);
            Ship ship = new Ship(position);
            ship.Color = color;
            NetworkInfo<Ship> shipInfo = new NetworkInfo<Ship>(ship, false);
            ShipInfos.Add(info.EntityID, shipInfo);
            ships.Add(ship);
          }
        }     
      }
      foreach(int k in processedKeys)
        ReceivedMessages.Remove(new Tuple<MessageType, EntityType, int>(MessageType.Create, EntityType.Ship, k));
      return ships;
    }

    public static void UpdateShipMessage(int id)
    {
      if (ReceivedMessages.ContainsKey(new Tuple<MessageType, EntityType, int>(MessageType.Update, EntityType.Ship, id)))
      {
        MessageInfo info = ReceivedMessages[new Tuple<MessageType, EntityType, int>(MessageType.Update, EntityType.Ship, id)];
        NetIncomingMessage message = info.Message;
        int fieldId = message.ReadInt32();
        switch (fieldId)
        {
          case 0:
            float x = message.ReadFloat();
            float y = message.ReadFloat();
            ShipInfos[info.EntityID].Entity.Position = new Microsoft.Xna.Framework.Vector2(x, y);
            break;
          case 1:
            byte r = message.ReadByte();
            byte g = message.ReadByte();
            byte b = message.ReadByte();
            ShipInfos[info.EntityID].Entity.Color = new Microsoft.Xna.Framework.Color(r, g, b);
            break;
          default:
            throw new ArgumentException("Unknown received field id in Ship: " + fieldId);
        }
        ReceivedMessages.Remove(new Tuple<MessageType, EntityType, int>(MessageType.Update, EntityType.Ship, id));
      }
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
          ReceivedMessages[new Tuple<MessageType, EntityType, int>(messageType, entityType, entityId)] = new MessageInfo(entityId, messages[i]);
        }
      }
    }
  }
}
