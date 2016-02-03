#pragma warning disable 162,108,618
using Casanova.Prelude;
using System.Linq;
using System;
using System.Collections.Generic;
using GameNetworking;using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
namespace Prototype {public class World{
public static int frame;

public bool JustEntered = true;

public int Net_ID = 0;
public void Start()
	{
Lidgren.Network.NetPeerConfiguration config = new Lidgren.Network.NetPeerConfiguration("AsteroidShooter");
NetworkAPI.Client = new Lidgren.Network.NetClient(config);
NetworkAPI.Client.Start();
NetworkAPI.Client.Connect("127.0.0.1", 5432);
this.Connected = false;
		Ships = (

(new Cons<Ship>(new Ship(new Microsoft.Xna.Framework.Vector2(0f,0f)),(new Empty<Ship>()).ToList<Ship>())).ToList<Ship>()).ToList<Ship>();
		Input = Microsoft.Xna.Framework.Input.Keyboard.GetState();
		Connected = false;
		
foreach(var entity in Ships) {
                                    NetworkAPI.ShipInfos.Add(entity.Net_ID, new NetworkInfo<Ship>(entity, true));;
                                 }}
		public System.Boolean Connected;
	public Microsoft.Xna.Framework.Input.KeyboardState Input;
	public List<Ship> Ships;
	public List<Ship> ___ships20;
	public Option<List<Ship>> wait1;

System.DateTime init_time = System.DateTime.Now;
	public void Update(float dt, World world) {
var t = System.DateTime.Now;NetworkAPI.DispatchMessages(NetworkAPI.Client);		this.Rule3(dt, world);

		for(int x0 = 0; x0 < Ships.Count; x0++) { 
			Ships[x0].Update(dt, world);
		}
		this.Rule0(dt, world);
		this.Rule1(dt, world);
		this.Rule2(dt, world);
	}

	public void Rule3(float dt, World world) 
	{
	Input = Microsoft.Xna.Framework.Input.Keyboard.GetState();
	}
	




	int s0=-1;
	public void Rule0(float dt, World world){ if (NetworkAPI.ReceivedMessages.ContainsKey(new Tuple<NetworkAPI.MessageType, NetworkAPI.EntityType, int>(NetworkAPI.MessageType.NewConnection, 0, 0)))
{switch (s0)
	{

	case -1:
	foreach (var entity in Ships)
        {
          if (NetworkAPI.ShipInfos.ContainsKey(entity.Net_ID) && NetworkAPI.ShipInfos[entity.Net_ID].IsLocal)
          {
            Lidgren.Network.NetOutgoingMessage entityMessage = NetworkAPI.CreateWorldShipsMessage(entity, NetworkAPI.Client, this.Net_ID, 2);
            NetworkAPI.Client.SendMessage(entityMessage, Lidgren.Network.NetDeliveryMethod.ReliableOrdered);
            NetworkAPI.ReceivedMessages.Remove(new Tuple<NetworkAPI.MessageType, NetworkAPI.EntityType, int>(NetworkAPI.MessageType.NewConnection, 0, 0));
          }
        }
	Ships = Ships;
	s0 = -1;
return;	
	default: return;}}}
	

	int s1=-1;
	public void Rule1(float dt, World world){ if (!Connected)
{
Lidgren.Network.NetOutgoingMessage message = NetworkAPI.Client.CreateMessage();
message.Write((int)NetworkAPI.MessageType.NewConnection);
message.Write(0);
message.Write(0);
message.Write(0);
message.Write(0);
NetworkAPI.Client.SendMessage(message, Lidgren.Network.NetDeliveryMethod.ReliableOrdered);
switch (s1)
	{

	case -1:
	foreach (var entity in Ships)
                  {
                    if (NetworkAPI.ShipInfos.ContainsKey(entity.Net_ID) && NetworkAPI.ShipInfos[entity.Net_ID].IsLocal)
                    {
                      Lidgren.Network.NetOutgoingMessage entityMessage = NetworkAPI.CreateWorldShipsMessage(entity, NetworkAPI.Client, this.Net_ID, 2);
                      NetworkAPI.Client.SendMessage(entityMessage, Lidgren.Network.NetDeliveryMethod.ReliableOrdered);
                    }
                  }
	Ships = Ships;
	Connected = true;
	s1 = -1;
return;	
	default: return;}
Connected = true;
}}
	

	int s2=-1;
	public void Rule2(float dt, World world){ switch (s2)
	{

	case -1:
	
	wait1 = NetworkAPI.ReceiveWorldShipsMessage(this.Net_ID);;
	if(wait1.IsNone)
	{

	s2 = -1;
return;	}else
	{

	___ships20 = wait1.Value;
	goto case 0;	}
	case 0:
	Ships = (Ships).Concat(___ships20).ToList<Ship>();
	s2 = -1;
return;	
	default: return;}}
	





}
public class Ship{
public int frame;
public bool JustEntered = true;
private Microsoft.Xna.Framework.Vector2 p;
public int Net_ID = NetworkAPI.NextID;	public int ID;
public Ship(Microsoft.Xna.Framework.Vector2 p)
	{JustEntered = false;
 frame = World.frame;
		Position = p;
		Connected = false;
		Color = new Microsoft.Xna.Framework.Color(0,0,155);
		
}
		public Microsoft.Xna.Framework.Color Color;
	public System.Boolean Connected;
	public Microsoft.Xna.Framework.Vector2 Position;
	public Microsoft.Xna.Framework.Vector2 ___vy10;
	public Microsoft.Xna.Framework.Vector2 ___p10;
	public Microsoft.Xna.Framework.Vector2 ___vy21;
	public Microsoft.Xna.Framework.Vector2 ___v20;
	public void Update(float dt, World world) {
frame = World.frame;

		this.Rule0(dt, world);
		this.Rule1(dt, world);
		this.Rule2(dt, world);
	}





	int s0=-1;
	public void Rule0(float dt, World world){ if (NetworkAPI.ShipInfos.ContainsKey(this.Net_ID) && !NetworkAPI.ShipInfos[this.Net_ID].IsLocal)
{switch (s0)
	{

	case -1:
	NetworkAPI.UpdateShipPositionMessage(this.Net_ID);
	s0 = -1;
return;	
	default: return;}}}
	

	int s1=-1;
	public void Rule1(float dt, World world){ if (NetworkAPI.ShipInfos.ContainsKey(this.Net_ID) && NetworkAPI.ShipInfos[this.Net_ID].IsLocal)
{switch (s1)
	{

	case -1:
	___vy10 = new Microsoft.Xna.Framework.Vector2(300f,0f);
	goto case 3;
	case 3:
	if(!(world.Input.IsKeyDown(Keys.D)))
	{

	s1 = 3;
return;	}else
	{

	goto case 2;	}
	case 2:
	___p10 = ((Position) + (((___vy10) * (dt))));
	Lidgren.Network.NetOutgoingMessage message = NetworkAPI.UpdateShipPositionMessage(this, NetworkAPI.Client, Net_ID, 2);
NetworkAPI.Client.SendMessage(message, Lidgren.Network.NetDeliveryMethod.UnreliableSequenced);
	Position = ___p10;
	s1 = -1;
return;	
	default: return;}}}
	

	int s2=-1;
	public void Rule2(float dt, World world){ if (NetworkAPI.ShipInfos.ContainsKey(this.Net_ID) && NetworkAPI.ShipInfos[this.Net_ID].IsLocal)
{switch (s2)
	{

	case -1:
	___vy21 = new Microsoft.Xna.Framework.Vector2(-300f,0f);
	goto case 3;
	case 3:
	if(!(world.Input.IsKeyDown(Keys.A)))
	{

	s2 = 3;
return;	}else
	{

	goto case 2;	}
	case 2:
	___v20 = ((Position) + (((___vy21) * (dt))));
	Lidgren.Network.NetOutgoingMessage message = NetworkAPI.UpdateShipPositionMessage(this, NetworkAPI.Client, Net_ID, 2);
NetworkAPI.Client.SendMessage(message, Lidgren.Network.NetDeliveryMethod.UnreliableSequenced);
	Position = ___v20;
	s2 = -1;
return;	
	default: return;}}}
	





}
}