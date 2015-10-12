using System;
using Lidgren.Network;
using UnityEngine;
using GameNetworking;
using ApplicationServer;
using ApplicationClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Program
{
  public class Program
  {
    public static void Main(string[] args)
    {
      try
      {
        Console.WriteLine("1 Server \n2 Reader client \n3 Sender Client");
        char menu = (char)(Console.Read());
        if (menu == '1')
        {
          TrackingServer server = new TrackingServer(5432, "AsteroidShooter");
          server.Server.Start();
          server.Forward();
        }
        else if (menu == '2')
        {
          ConnnectingClient client = ConnnectingClient.Create("127.0.0.1");
          client.RunReader();
        }
        else
        {
          ConnnectingClient client = ConnnectingClient.Create("127.0.0.1");
          client.RunSender();
        }
      }
      catch(Exception e)
      {
        Console.WriteLine(e.Message);
        System.Threading.Thread.Sleep(300000);
      }
    }
  }
}
