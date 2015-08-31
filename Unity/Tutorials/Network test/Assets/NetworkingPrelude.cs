using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public static class NetworkingPrelude
{
  public static void SerializeBase(Single data, BitStream stream)
  {
    Single _data = data;
    stream.Serialize(ref _data);
  }
  public static void SerializeBase(bool data, BitStream stream)
  {
    bool _data = data;
    stream.Serialize(ref _data);
  }
  public static void SerializeBase(Vector3 data, BitStream stream)
  {
    Vector3 _data = data;
    stream.Serialize(ref _data);
  }

  public static void SerializeBase(Quaternion data, BitStream stream)
  {
    Quaternion _data = data;
    stream.Serialize(ref _data);
  }

  public static void SerializeBase(Int32 data, BitStream stream)
  {
    Int32 _data = data;
    stream.Serialize(ref _data);
  }



  public static Single FloatReceiveBase(BitStream stream)
  {
    Single _data = 0;
    stream.Serialize(ref _data);
    return _data;
  }
  public static bool BoolReceiveBase(BitStream stream)
  {
    bool _data = true;
    stream.Serialize(ref _data);
    return _data;
  }
  public static Vector3 Vector3ReceiveBase(BitStream stream)
  {
    Vector3 _data = Vector3.zero;
    stream.Serialize(ref _data);
    return _data;
  }

  public static Quaternion QuaternionReceiveBase(BitStream stream)
  {
    Quaternion _data = Quaternion.identity;
    stream.Serialize(ref _data);
    return _data;
  }

  public static Int32 IntReceiveBase(BitStream stream)
  {
    Int32 _data = 0;
    stream.Serialize(ref _data);
    return _data;
  }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     