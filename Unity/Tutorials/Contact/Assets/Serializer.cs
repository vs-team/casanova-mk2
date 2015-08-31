using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[Serializable]
public class SerializableMessage
{
  public int Sender { get; set; }
  public int Receiver { get; set; }
  public List<int> Symbols { get; set; }
  public List<int> Images { get; set; }

  public SerializableMessage(int sender, int receiver, IEnumerable<int> symbols, IEnumerable<int> images)
  {
    this.Sender = sender;
    this.Receiver = receiver;
    List<int> ls = new List<int>(symbols.Count());
    List<int> li = new List<int>(images.Count());
    foreach (var symbol in symbols)
    {
      ls.Add(symbol);
    }
    foreach (var image in images)
    {
      li.Add(image); 
    }
  }
}

[Serializable]
public class SerializableTrade
{
  public int Uranium { get; set; }
  public int Plutonium { get; set; }
  public int Oil { get; set; }
  public int Iron { get; set; }
  public int Receiver { get; set; }

  public SerializableTrade(int u, int p, int o, int i, int receiver)
  {
    this.Uranium = u;
    this.Plutonium = p;
    this.Oil = o;
    this.Iron = i;
    this.Receiver = receiver;
  }

}

[Serializable]
public class SerializableVector3
{
  public float X { get; set; }
  public float Y { get; set; }
  public float Z { get; set; }

  public SerializableVector3(float x, float y, float z)
  {
    this.X = x;
    this.Y = y;
    this.Z = z;
  }
}


[Serializable]
public class SerializableWorld
{
  public List<SerializableVector3> DronePositions { get; set; }
  public List<SerializableMessage> Messages { get; set; }
  public List<SerializableTrade> Trades { get; set; }
  public int ActivePlayerIndex { get; set; }
}

public class Serializer
{
  public static void SaveGame(IEnumerable<Vector3> dronePosition, IEnumerable<SerializableMessage> messages, IEnumerable<SerializableTrade> trades, int playerIndex, string filePath)
  {
    string directory = System.IO.Path.GetDirectoryName(filePath);
    if (!System.IO.Directory.Exists(directory))
      System.IO.Directory.CreateDirectory(directory);
    System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
    System.IO.Stream stream = new System.IO.FileStream(filePath, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None);
    SerializableWorld world = new SerializableWorld();
    List<SerializableVector3> lp = new List<SerializableVector3>(dronePosition.Count());
    List<SerializableMessage> lm = new List<SerializableMessage>(messages.Count());
    List<SerializableTrade> lt = new List<SerializableTrade>(trades.Count());
    foreach (var position in dronePosition)
    {
      lp.Add(new SerializableVector3(position.x, position.y, position.z));
    }
    foreach (var message in messages)
    {
      lm.Add(message);
    }
    foreach (var trade in trades)
    {
      lt.Add(trade);
    }
    world.DronePositions = lp;
    world.Messages = lm;
    world.Trades = lt;
    world.ActivePlayerIndex = playerIndex;
    formatter.Serialize(stream, world);
    stream.Close();
  }

  public static SerializableWorld LoadGame(string path)
  {
    System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
    System.IO.Stream stream = new System.IO.FileStream(path, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read);
    SerializableWorld world = (SerializableWorld)formatter.Deserialize(stream);
    stream.Close();
    return world;
  }
}                                                                                                                                                                                                                                                                                                                                                                            