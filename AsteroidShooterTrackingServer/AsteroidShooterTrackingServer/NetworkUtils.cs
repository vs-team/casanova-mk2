using System;
using UnityEngine;
using Lidgren.Network;
using System.Collections.Generic;
using ApplicationClient;
using System.Reflection;
using System.Linq;
using System.Text;

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
    public enum AtomicDataType { IntType, FloatType, StringType, BoolType}
    public enum CompositeDataType { Vector3Type}
    public enum MessageType { BasicType, CompositeType}
    public enum DataFormat { List, Element}

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
            else if (elT == typeof(Vector3))
            {
                return new Pair<bool, int>(true, ((int)CompositeDataType.Vector3Type));
            }
            else
            {
                throw new ArgumentException("Unsupported Identity Type");
            }

        }

    public static void Send<T>(T element, NetClient client, Func<T, NetClient, bool, NetOutgoingMessage> messageFunction)
    {
      NetOutgoingMessage message = messageFunction(element, ConnectingClient, true);
      client.SendMessage(message, NetDeliveryMethod.ReliableOrdered);
    }

    public static void Send<T>(List<T> list, NetClient client, Func<T, NetClient, bool, NetOutgoingMessage> messageFunction)
    {
      NetOutgoingMessage message = BuildMessage<T>(list, client, false, messageFunction);
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
        message.Write((int)DataFormat.Element);
        message.Write((int)MessageType.CompositeType);
        message.Write((int)CompositeDataType.Vector3Type);
      }
      message.Write(v.x);
      message.Write(v.y);
      message.Write(v.z);
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
            Pair<bool, int> pair = getTypeIdentity<T>(list.ElementAtOrDefault(1)); //geen idee of dit kan/mag!!!!!! Wouter
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
            if (e == true)
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

        public static Vector3 ReceiveVector3(NetIncomingMessage message)
    {
      float x = message.ReadFloat();
      float y = message.ReadFloat();
      float z = message.ReadFloat();
      return new Vector3(x, y, z);
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