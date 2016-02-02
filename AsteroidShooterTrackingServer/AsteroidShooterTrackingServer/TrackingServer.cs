using System;
using Lidgren.Network;
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
      int oldConnection = Server.Connections.Count;
      while (true)
      {
        if (oldConnection < Server.Connections.Count)
          Console.WriteLine("New client connected");
        else if (oldConnection > Server.Connections.Count)
          Console.WriteLine("Client Disconnected");
        oldConnection = Server.Connections.Count;
        NetIncomingMessage message = this.Server.ReadMessage();

        if (message != null)
        {
          switch (message.MessageType)
          {
            case NetIncomingMessageType.Data:
              NetOutgoingMessage sendMessage = this.Server.CreateMessage();
              sendMessage.Write(message);
              for (int i = 0; i < Server.Connections.Count; i++)
              {
                if (message.SenderConnection != Server.Connections[i])
                {
                  this.Server.SendToAll(sendMessage, NetDeliveryMethod.ReliableOrdered);
                  //Console.WriteLine("Message forwarded");
                }
              }
              break;
            default:
              break;
          }
        }
        else
        {
        }
        //System.Threading.Thread.Sleep((int)(1f / 30f * 1000));
      }
    }

    public NetServer Server { get { return netServer; } }
  }
}
