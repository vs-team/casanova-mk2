using System;
using Lidgren.Network;
using UnityEngine;
using GameNetworking;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApplicationClient
{
  public class ConnnectingClient
  {
    public static NetClient netClient;

    public ConnnectingClient(string applicationName)
    {
      NetPeerConfiguration config = new NetPeerConfiguration(applicationName);
      netClient = new NetClient(config);
    }

    public NetClient Client { get { return netClient; } }

    public static ConnnectingClient Create(string address)
    {
      ConnnectingClient client = new ConnnectingClient("AsteroidShooter");
      client.Client.Start();
      client.Client.Connect(address, 5432);
      return client;
    }

    public void RunSender()
    {
      while (true)
      {
        Console.WriteLine("Sending...");
        Console.Clear();
        int Spawn = 1;
        Vector3 v1 = new Vector3(1.0f, -3.0f, 2.5f);
        NetworkUtils.Send<int>(Spawn, netClient, NetworkUtils.BuildMessage);
        NetworkUtils.Send<Vector3>(v1, netClient, NetworkUtils.BuildMessage);
      }
    }

    public void RunReader()
    {
      while (true)
      {
        NetIncomingMessage message = this.Client.ReadMessage();
        if (message != null)
        {
          switch (message.MessageType)
          {
            case NetIncomingMessageType.Data:
              switch ((NetworkUtils.DataFormat)message.ReadInt32())
              {
                case NetworkUtils.DataFormat.Element:
                  switch ((NetworkUtils.MessageType)message.ReadInt32())
                  {
                    case NetworkUtils.MessageType.BasicType:
                      switch ((NetworkUtils.AtomicDataType)message.ReadInt32())
                      {
                        case NetworkUtils.AtomicDataType.FloatType:
                          float f = NetworkUtils.Receive<float>(message, NetworkUtils.ReceiveFloat);
                          Console.Write(f);
                          break;
                        case NetworkUtils.AtomicDataType.IntType:
                          int i = NetworkUtils.Receive<int>(message, NetworkUtils.ReceiveInt);
                          break;
                        case NetworkUtils.AtomicDataType.StringType:
                          string s = NetworkUtils.Receive<string>(message, NetworkUtils.ReceiveString);
                          break;
                        default:
                          throw new ArgumentException("Unsupported basic data type.");
                      }
                      break;
                    case NetworkUtils.MessageType.CompositeType:
                      switch ((NetworkUtils.CompositeDataType)message.ReadInt32())
                      {
                        case NetworkUtils.CompositeDataType.Vector3Type:
                          Vector3 v = NetworkUtils.Receive<Vector3>(message, NetworkUtils.ReceiveVector3);
                          Debug.Log(v);
                          break;
                        default:
                          throw new ArgumentException("Unssuported composite data type");
                      }
                      break;
                    default:
                      throw new ArgumentException("Unsupported network data");
                  }
                  break;
                case NetworkUtils.DataFormat.List:
                  switch ((NetworkUtils.MessageType)message.ReadInt32())
                  {
                    case NetworkUtils.MessageType.BasicType:
                      switch ((NetworkUtils.AtomicDataType)message.ReadInt32())
                      {
                        case NetworkUtils.AtomicDataType.FloatType:
                          List<float> l = NetworkUtils.ReceiveList<float>(message, NetworkUtils.ReceiveFloat);
                          break;
                        case NetworkUtils.AtomicDataType.IntType:
                          List<int> i = NetworkUtils.ReceiveList<int>(message, NetworkUtils.ReceiveInt);
                          break;
                        case NetworkUtils.AtomicDataType.StringType:
                          List<string> s = NetworkUtils.ReceiveList<string>(message, NetworkUtils.ReceiveString);
                          break;
                        default:
                          throw new ArgumentException("Unsupported Atomic type in List");
                      }
                      break;
                    case NetworkUtils.MessageType.CompositeType:
                      switch ((NetworkUtils.CompositeDataType)message.ReadInt32())
                      {
                        case NetworkUtils.CompositeDataType.Vector3Type:
                          List<Vector3> l = NetworkUtils.ReceiveList<Vector3>(message, NetworkUtils.ReceiveVector3);
                          for (int i = 0; i < l.Count; i++)
                          {
                            Console.Write("[" + l[i].x + "," + l[i].y + "," + l[i].z + "] \n");
                          }
                          break;
                        default:
                          throw new ArgumentException("Unsupported Composite type in List");
                      }
                      break;
                    default:
                      break;
                  }
                  break;
                default:
                  throw new ArgumentException("Undefined network data category");
              }

              break;
            default:
              break;
          }
        }
        else
        {
        }
        System.Threading.Thread.Sleep(10);
      }
    }
  }
}
