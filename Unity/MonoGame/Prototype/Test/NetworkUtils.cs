using System;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using Utilities;

namespace GameNetworking
{

  public static class NetworkUtils
  {

    public static void Send<T>(T element, NetClient client, Func<T, NetClient, NetOutgoingMessage> messageFunction)
    {
      NetOutgoingMessage message = messageFunction(element, client);
      client.SendMessage(message, NetDeliveryMethod.ReliableOrdered);
    }

    public static void Send<T>(List<T> list, NetClient client, Func<T, NetClient, NetOutgoingMessage> messageFunction)
    {
      NetOutgoingMessage message = BuildMessage<T>(list, client, messageFunction);
      client.SendMessage(message, NetDeliveryMethod.ReliableOrdered);
    }

    public static T Receive<T>(NetIncomingMessage message, Func<NetIncomingMessage, T> receiveFunction)
    {
      return receiveFunction(message);
    }

    public static NetOutgoingMessage BuildMessage(Vector2 v, NetClient client)
    {
      NetOutgoingMessage message = client.CreateMessage();
      message.Write(v.X);
      message.Write(v.Y);
      return message;
    }

    public static NetOutgoingMessage BuildMessage(float f, NetClient client)
    {
      NetOutgoingMessage message = client.CreateMessage();
      message.Write(f);
      return message;
    }

    public static NetOutgoingMessage BuildMessage(string s, NetClient client)
    {
      NetOutgoingMessage message = client.CreateMessage();
      message.Write(s);
      return message;
    }

    public static NetOutgoingMessage BuildMessage(int i, NetClient client)
    {
      NetOutgoingMessage message = client.CreateMessage();
      message.Write(i);
      return message;
    }

    public static NetOutgoingMessage BuildMessage<T>(List<T> list, NetClient client, Func<T, NetClient, NetOutgoingMessage> messageFunction)
    {
      NetOutgoingMessage message = client.CreateMessage();
      message.Write(list.Count);
      for (int i = 0; i < list.Count; i++)
      {
        NetOutgoingMessage tempMessage = messageFunction(list[i], client);
        message.Write(tempMessage);
      }
      return message;
    }

    public static NetOutgoingMessage BuildMessage<T>(T element, NetClient client)
    {
      NetOutgoingMessage message = client.CreateMessage();
      NetOutgoingMessage finalMessage = NetworkUtils.BuildMessage(element, client);
      message.Write(finalMessage);
      return message;
    }

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