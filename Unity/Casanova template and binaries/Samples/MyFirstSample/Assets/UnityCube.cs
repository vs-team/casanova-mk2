using UnityEngine;
using System.Collections;

public class UnityCube : MonoBehaviour {
  public Vector3 Position
  {
    get { return gameObject.transform.position; }
    set { gameObject.transform.position = value; }
  }

  public Vector3 Scale
  {
    get { return gameObject.transform.localScale; }
    set { gameObject.transform.localScale = value; }
  }


  private bool destroyed;
  public bool Destroyed
  {
    get { return destroyed; }
    set {
      destroyed = value;
      if(destroyed)
        GameObject.Destroy(gameObject);
      }
  }

  public Color Color
  {
    set { gameObject.GetComponent<Renderer>().material.color = value; }
  }
  public static UnityCube Instantiate(Color color)
  {
    var _object = GameObject.Instantiate(Resources.Load("MyCube"), 
                                         Random.insideUnitSphere, 
                                         Quaternion.identity) as GameObject;
    var cube = _object.GetComponent<UnityCube>();
    cube.Color = color;
    return cube;
  }
  public static UnityCube Find()
  {
    GameObject cube_game_object = GameObject.Find("MyCube");
    return cube_game_object.GetComponent<UnityCube>();
  }
}
                                                                                                                                                                                                                          