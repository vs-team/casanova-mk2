using System;
using Lidgren.Network;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApplicationServer
{
  public class TrackingServer
  {
    private NetServer netServer;
    

    public TrackingServer(int port, string applicationName)
    {
      NetPeerConfiguration config = new NetPeerConfiguration(applicationName);
      config.Port = port;
      netServer = new NetServer(config);
      Console.WriteLine("Server " + applicationName + " started succesfully");
      
    }

    public void Forward()
    {
      while (true)
      {
        Console.WriteLine(this.netServer.ConnectionsCount);
        NetIncomingMessage message = this.Server.ReadMessage();

        if (message != null)
        {
          switch (message.MessageType)
          {
            case NetIncomingMessageType.Data:
              NetOutgoingMessage sendMessage = this.Server.CreateMessage();
              sendMessage.Write(message);
              this.Server.SendToAll(sendMessage, NetDeliveryMethod.ReliableOrdered);
              Console.WriteLine("Message forwarded");
              Console.WriteLine(this.netServer.ConnectionsCount);
              Console.Clear();
              break;
            default:
              break;
          }
        }
        else
        {
        }
      }
    }

    public NetServer Server { get { return netServer; } }
  }
}
