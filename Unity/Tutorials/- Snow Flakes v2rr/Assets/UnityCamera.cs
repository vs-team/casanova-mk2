using UnityEngine;
using System.Collections;

public class UnityCamera : MonoBehaviour
{

  public Vector3 Position
  {
    get { return this.transform.position; }
  }
  public Quaternion Rotation
  {
    get { return this.transform.rotation; }
    set { this.transform.rotation = value; }
  }
  public Vector3 Forward
  {
    get { return this.transform.forward; }
  }
  public static UnityCamera Find()
  {
    return GameObject.Find("/Main Camera").GetComponent<UnityCamera>();
  }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                            