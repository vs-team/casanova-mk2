using System;
using Lidgren.Network;
using UnityEngine;
using ApplicationServer;
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
        Console.WriteLine("Insert server name");
        string name = Console.ReadLine();
        Console.WriteLine("Insert port");
        int port = System.Convert.ToInt32(Console.ReadLine());
        TrackingServer server = new TrackingServer(port, name);
        server.Server.Start();
        server.Forward();
      }
      catch (Exception e)
      {
        Console.WriteLine(e.Message);
      }
    }
  }
}
