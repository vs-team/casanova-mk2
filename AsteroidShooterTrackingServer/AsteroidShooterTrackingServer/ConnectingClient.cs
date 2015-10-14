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
    public NetClient netClient;

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
        Vector3 v1 = new Vector3(1.0f, -3.0f, 2.5f);
        Vector3 v2 = new Vector3(1.0f, 1.5f, 2.5f);
        Vector3 v3 = new Vector3(1.0f, 1.0f, -2.5f);
        List<Vector3> vectorList = new List<Vector3>(3);
        vectorList.Add(v1);
        vectorList.Add(v2);
        vectorList.Add(v3);
        NetworkUtils.Send<Vector3>(vectorList, this.netClient, NetworkUtils.BuildMessage);
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
              switch ((NetworkUtils.AsteroidShooterMessage)message.ReadInt32())
              {
                case NetworkUtils.AsteroidShooterMessage.CompositeType:
                  switch ((NetworkUtils.AsteroidShooterMessage)message.ReadInt32())
                  {
                    case NetworkUtils.AsteroidShooterMessage.Vector3Message:
                      Vector3 v = NetworkUtils.Receive<Vector3>(message, NetworkUtils.ReceiveVector3);
                      Console.Clear();
                      Console.WriteLine("[" + v.x + "," + v.y + "," + v.z + "]");
                      break;
                    case NetworkUtils.AsteroidShooterMessage.ListMessage:
                      List<Vector3> list = NetworkUtils.ReceiveList<Vector3>(message, NetworkUtils.ReceiveVector3);
                      Console.Clear();
                      Console.Write("[");
                      for (int i = 0; i < list.Count; i++)
                      {
                        Console.Write(i == list.Count - 1 ? list[i].ToString() : list[i].ToString() + ",");
                      }
                      Console.Write("]\n");
                      break;
                    default:
                      throw new ArgumentException("Unsupported network data");
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
