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
    public enum AsteroidShooterMessage { Vector3Message, ListIntegerMessage }

    public static void Send<T>(T element, NetClient client)
    {
      NetOutgoingMessage message = BuildMessage<T>(element, client);
      client.SendMessage(message, NetDeliveryMethod.ReliableOrdered);
    }

    private static NetOutgoingMessage BuildMessage<T>(T element, NetClient client)
    {
      Console.WriteLine(element.GetType().Name);
      NetOutgoingMessage message = client.CreateMessage();
      Console.WriteLine(typeof(T));
      if (typeof(T) == typeof(Vector3))
      {
        Vector3 v = (Vector3)(object)element;
        AsteroidShooterMessage header = AsteroidShooterMessage.Vector3Message;
        float x = v.x;
        float y = v.y;
        float z = v.z;
        message.Write((int)header);
        message.Write(x);
        message.Write(y);
        message.Write(z);
        return message;
      }
      else if (element.GetType().Name == "List`1")
      {     
        List<int> list = (List<int>)(object)element;
        AsteroidShooterMessage header = AsteroidShooterMessage.ListIntegerMessage;
        message.Write((int)header);
        message.Write(list.Count);
        for (int i = 0; i < list.Count; i++)
        {
          NetOutgoingMessage partialMessage = BuildMessage<int>(list[i], client);
          message.Write(partialMessage);
        }
        return message;
      }
      else if (typeof(T) == typeof(int))
      {
        int n = (int)(object)element;
        message.Write(n);
        return message;
      }
      else
      {
        throw new ArgumentException("Unsupported send type");
      }
    }

    public static T Receive<T>(NetIncomingMessage message)
    {
      if (typeof(T) == typeof(Vector3))
      {
        float x = message.ReadFloat();
        float y = message.ReadFloat();
        float z = message.ReadFloat();
        Vector3 v = new Vector3(x, y, z);
        return (T)Convert.ChangeType(v, typeof(T));
      }
      else if (typeof(T) == typeof(List<int>))
      {
        int length = message.ReadInt32();
        List<int> list = new List<int>(length);
        for (int i = 0; i < length; i++)
        {
          int element = Receive<int>(message);
          list.Add(element);
        }
        return (T)Convert.ChangeType(list, typeof(T));
      }
      else if (typeof(T) == typeof(int))
      {
        return (T)Convert.ChangeType(message.ReadInt32(), typeof(T));
      }
      else
      {
        throw new ArgumentException("Unsupported receive type");
      }
    }
  }
}
