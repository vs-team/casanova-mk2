using UnityEngine;

public class UnityCube : MonoBehaviour
{
  Lidgren.Network.NetClient client;
      
  public Color Color
  {
    set { gameObject.GetComponent<Renderer>().material.color = value; }
  }

  public static UnityCube Find()
  {
    return GameObject.Find("/Cube").GetComponent<UnityCube>();
  }
}
                                                                                                            