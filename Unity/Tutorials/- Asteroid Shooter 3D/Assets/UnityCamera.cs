using UnityEngine;
using System.Collections;

public class UnityCamera : MonoBehaviour
{


  public Vector3 Position
  {
    get
    { return gameObject.transform.position; }
    set { gameObject.transform.position = value; }
  }

  public Quaternion Rotation
  {
    get
    { return gameObject.transform.rotation; }
    set { gameObject.transform.rotation = value; }
  }

  public static UnityCamera Find(Vector3 pos)
  {
    var camera = GameObject.FindGameObjectsWithTag("MainCamera")[0].GetComponent<UnityCamera>();
    camera.Position = pos;
    return camera;
  }



}
                                                                 