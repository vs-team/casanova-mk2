using UnityEngine;
using System.Collections;

public class UnityCamera : MonoBehaviour
{

  public Vector3 Position
  {
    get { return gameObject.transform.position; }
    set { gameObject.transform.position = value; }
  }
  public Quaternion Rotation
  {
    get { return gameObject.transform.rotation; }
    set { gameObject.transform.rotation = value; }

  }
  public static UnityCamera Find()
  {
    var camera = GameObject.Find("/Main Camera").GetComponent<UnityCamera>();
    //camera.Position = position;
    return camera;
  }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             