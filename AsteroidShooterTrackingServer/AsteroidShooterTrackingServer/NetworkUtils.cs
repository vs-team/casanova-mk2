using System;
using UnityEngine;
using Lidgren.Network;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;

namespace GameNetworking
{
  public static class NetworkUtils
  {
    public enum AsteroidShooterMessage { BasicType, CompositeType, Vector3Message, ListMessage }

    public static void Send<T>(T element, NetClient client, Func<T, NetClient, bool, NetOutgoingMessage> messageFunction)
    {
      NetOutgoingMessage message = messageFunction(element, client, true);
      client.SendMessage(message, NetDeliveryMethod.ReliableOrdered);
    }

    public static void Send<T>(List<T> list, NetClient client, Func<T, NetClient, bool, NetOutgoingMessage> messageFunction)
    {
      NetOutgoingMessage message = BuildMessage<T>(list, client, messageFunction);
      client.SendMessage(message, NetDeliveryMethod.ReliableOrdered);
    }

    public static T Receive<T>(NetIncomingMessage message, Func<NetIncomingMessage, T> receiveFunction)
    {
      return receiveFunction(message);
    }

    public static NetOutgoingMessage BuildMessage(Vector3 v, NetClient client, bool createHeader)
    {
      NetOutgoingMessage message = client.CreateMessage();
      if (createHeader)
      {
        message.Write((int)AsteroidShooterMessage.CompositeType);
        message.Write((int)AsteroidShooterMessage.Vector3Message);
      }
      message.Write(v.x);
      message.Write(v.y);
      message.Write(v.z);
      return message;
    }

    public static NetOutgoingMessage BuildMessage<T>(List<T> list, NetClient client, Func<T, NetClient, bool, NetOutgoingMessage> messageFunction)
    {
      NetOutgoingMessage message = client.CreateMessage();
      message.Write((int)AsteroidShooterMessage.CompositeType);
      message.Write((int)AsteroidShooterMessage.ListMessage);
      message.Write(list.Count);
      for (int i = 0; i < list.Count; i++)
      {
        NetOutgoingMessage tempMessage = messageFunction(list[i], client, false);
        message.Write(tempMessage);
      }
      return message;
    }

    public static Vector3 ReceiveVector3(NetIncomingMessage message)
    {
      float x = message.ReadFloat();
      float y = message.ReadFloat();
      float z = message.ReadFloat();
      return new Vector3(x, y, z);
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
