#pragma warning disable 162,108,618
using Casanova.Prelude;
using System.Linq;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using Utilities;
using GameNetworking;
namespace Prototype
{
  public class World
  {
    public static int frame;
    public bool Connected;
    public int ID;

    public bool JustEntered = true;


    public void Start()
    {
      //NETWORKING INITIALIZATION
      Lidgren.Network.NetPeerConfiguration config = new Lidgren.Network.NetPeerConfiguration("AsteroidShooter");
      NetworkAPI.Client = new Lidgren.Network.NetClient(config);
      NetworkAPI.Client.Start();
      NetworkAPI.Client.Connect("127.0.0.1", 5432);
      Connected = false;
      ID = 0;
      Ships = (

  (new Cons<Ship>(new Ship(new Microsoft.Xna.Framework.Vector2(Utilities.Random.RandFloat(0f, 600f), 1000f)), (new Empty<Ship>()).ToList<Ship>())).ToList<Ship>()).ToList<Ship>();
      Input = Microsoft.Xna.Framework.Input.Keyboard.GetState();
      NetworkAPI.ShipInfos.Add(Ships[0].ID, new NetworkInfo<Ship>(Ships[0], true));

    }
    public Microsoft.Xna.Framework.Input.KeyboardState Input;
    public List<Ship> Ships;

    System.DateTime init_time = System.DateTime.Now;
    public void Update(float dt, World world)
    {
      //connect
      if (!Connected)
      {
        Lidgren.Network.NetOutgoingMessage message = NetworkAPI.Client.CreateMessage();
        message.Write((int)NetworkAPI.MessageType.NewConnection);
        message.Write(0);
        message.Write(0);
        NetworkAPI.Client.SendMessage(message, Lidgren.Network.NetDeliveryMethod.ReliableOrdered);
        Connected = true;
      }

      //NETWORKING CODE
      NetworkAPI.DispatchMessages(NetworkAPI.Client);
      List<Ship> ships = NetworkAPI.ReceiveShipMessage();
      if (ships.Count > 0)
        Ships.AddRange(ships);

      var t = System.DateTime.Now; this.Rule0(dt, world);

      for (int x0 = 0; x0 < Ships.Count; x0++)
      {
        Ships[x0].Update(dt, world);
      }


    }

    public void Rule0(float dt, World world)
    {
      Input = Microsoft.Xna.Framework.Input.Keyboard.GetState();
    }











  }
  public class Ship
  {
    public int frame;
    public bool JustEntered = true;
    private Microsoft.Xna.Framework.Vector2 p;
    public int ID;
    public bool Connected;
    public Ship(Microsoft.Xna.Framework.Vector2 p)
    {
      ID = GetHashCode();
      Connected = false;
      JustEntered = false;
      frame = World.frame;
      Position = p;
      Color = new Microsoft.Xna.Framework.Color(Utilities.Random.RandInt(0, 256), Utilities.Random.RandInt(0, 256), Utilities.Random.RandInt(0, 256));
    }
    public Microsoft.Xna.Framework.Color Color;
    public Microsoft.Xna.Framework.Vector2 Position;
    public Microsoft.Xna.Framework.Vector2 ___vy00;
    public Microsoft.Xna.Framework.Vector2 ___vy11;
    public void Update(float dt, World world)
    {
      //connect
      if (NetworkAPI.ReceivedMessages.ContainsKey(new Tuple<NetworkAPI.MessageType, NetworkAPI.EntityType, int>(NetworkAPI.MessageType.NewConnection, 0, 0)))
      {
        if (NetworkAPI.ShipInfos[ID].IsLocal)
        {
          Lidgren.Network.NetOutgoingMessage message = NetworkAPI.CreateShipMessage(this, NetworkAPI.Client);
          NetworkAPI.Client.SendMessage(message, Lidgren.Network.NetDeliveryMethod.ReliableOrdered);
          NetworkAPI.ReceivedMessages.Remove(new Tuple<NetworkAPI.MessageType, NetworkAPI.EntityType, int>(NetworkAPI.MessageType.NewConnection, 0, 0));
        }
      }
      else if (!Connected && NetworkAPI.ShipInfos.ContainsKey(ID) && NetworkAPI.ShipInfos[ID].IsLocal)
      {
        Lidgren.Network.NetOutgoingMessage message = NetworkAPI.CreateShipMessage(this, NetworkAPI.Client);
        NetworkAPI.Client.SendMessage(message, Lidgren.Network.NetDeliveryMethod.ReliableOrdered);
        Connected = true;
      }

      //slave
      if (NetworkAPI.ShipInfos.ContainsKey(ID) && !NetworkAPI.ShipInfos[ID].IsLocal)
        NetworkAPI.UpdateShipMessage(ID);

      frame = World.frame;

      //master
      if (NetworkAPI.ShipInfos.ContainsKey(ID) && NetworkAPI.ShipInfos[ID].IsLocal)
      {
        this.Rule0(dt, world);
        this.Rule1(dt, world);
      }

    }





    int s0 = -1;
    public void Rule0(float dt, World world)
    {
      switch (s0)
      {

        case -1:
          ___vy00 = new Microsoft.Xna.Framework.Vector2(300f, 0f);
          goto case 1;
        case 1:
          if (!(world.Input.IsKeyDown(Keys.D)))
          {

            s0 = 1;
            return;
          }
          else
          {

            goto case 0;
          }
        case 0:
          Lidgren.Network.NetOutgoingMessage message = NetworkAPI.CreateShipMessage(this, NetworkAPI.Client, 0);
          NetworkAPI.Client.SendMessage(message, Lidgren.Network.NetDeliveryMethod.ReliableOrdered);
          Position = ((Position) + (((___vy00) * (dt))));
          s0 = -1;
          return;
        default: return;
      }
    }


    int s1 = -1;
    public void Rule1(float dt, World world)
    {
      switch (s1)
      {

        case -1:
          ___vy11 = new Microsoft.Xna.Framework.Vector2(-300f, 0f);
          goto case 1;
        case 1:
          if (!(world.Input.IsKeyDown(Keys.A)))
          {

            s1 = 1;
            return;
          }
          else
          {

            goto case 0;
          }
        case 0:
          Lidgren.Network.NetOutgoingMessage message = NetworkAPI.CreateShipMessage(this, NetworkAPI.Client, 0);
          NetworkAPI.Client.SendMessage(message, Lidgren.Network.NetDeliveryMethod.ReliableOrdered);
          Position = ((Position) + (((___vy11) * (dt))));
          s1 = -1;
          return;
        default: return;
      }
    }






  }
}