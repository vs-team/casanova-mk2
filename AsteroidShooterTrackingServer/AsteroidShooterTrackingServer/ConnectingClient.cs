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
        List<int> list = new List<int>(4);
        list.Add(15);
        list.Add(22);
        list.Add(45);
        list.Add(69);
        NetworkUtils.Send<List<int>>(list, this.netClient);
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
            case NetIncomingMessageType.Error:
              break;
            case NetIncomingMessageType.StatusChanged:
              break;
            case NetIncomingMessageType.UnconnectedData:
              break;
            case NetIncomingMessageType.ConnectionApproval:
              break;
            case NetIncomingMessageType.Data:
              switch ((NetworkUtils.AsteroidShooterMessage)message.ReadInt32())
              {
                case NetworkUtils.AsteroidShooterMessage.Vector3Message:
                  Vector3 v = NetworkUtils.Receive<Vector3>(message);
                  Console.Clear();
                  Console.WriteLine("[" + v.x + "," + v.y + "," + v.z + "]");
                  break;
                case NetworkUtils.AsteroidShooterMessage.ListIntegerMessage:
                  List<int> list = NetworkUtils.Receive<List<int>>(message);
                  Console.Clear();
                  Console.WriteLine("[");
                  for (int i = 0; i < list.Count; i++)
                  {
                    Console.WriteLine(list[i]);
                  }
                  Console.WriteLine("]");
                  break;
                default:
                  throw new ArgumentException("Unsupported network data");
              }
              break;
            case NetIncomingMessageType.Receipt:
              break;
            case NetIncomingMessageType.DiscoveryRequest:
              break;
            case NetIncomingMessageType.DiscoveryResponse:
              break;
            case NetIncomingMessageType.VerboseDebugMessage:
              break;
            case NetIncomingMessageType.DebugMessage:
              break;
            case NetIncomingMessageType.WarningMessage:
              break;
            case NetIncomingMessageType.ErrorMessage:
              break;
            case NetIncomingMessageType.NatIntroductionSuccess:
              break;
            case NetIncomingMessageType.ConnectionLatencyUpdated:
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
