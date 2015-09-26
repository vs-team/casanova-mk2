using UnityEngine;
using System.Collections.Generic;

public class UnityCube : MonoBehaviour 
{

  public Color Color
  {
    get { return gameObject.GetComponent<Renderer>().material.color; }
    set { gameObject.GetComponent<Renderer>().material.color = value; }
  }

  public Vector3 Position
  {
    get { return gameObject.transform.position; }
    set { gameObject.transform.position = value; }
    //set 
    //{
    //  Vector3 pos = gameObject.transform.position;
    //  gameObject.transform.position = new Vector3(value, pos.y, pos.z);
    //}
  }

  

  public static UnityCube Find(string model)
  {

    return GameObject.Find(model).GetComponent<UnityCube>();
  }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                