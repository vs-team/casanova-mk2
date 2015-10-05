using UnityEngine;
using System.Collections;

public class UnityBoid : MonoBehaviour
{

  public Vector3 Position
  {
    get { return gameObject.transform.position; }
    set { gameObject.transform.position = value; }
  }

  public Vector3 Scale
  {
    get { return gameObject.GetComponent<Renderer>().bounds.size; }
  }

  public static UnityBoid Instantiate(Vector3 location)
  {
    var b = GameObject.Instantiate(Resources.Load("Boid"), location, Quaternion.identity) as GameObject;
    return b.GetComponent<UnityBoid>();
  }

}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                