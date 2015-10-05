using UnityEngine;
using System.Collections;

public class UnityCamera : MonoBehaviour {

	// Use this for initialization
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
  public Vector3 Right
  {
    get { return this.transform.right; }
  }
  public Vector3 Backward
  {
    get { return this.transform.forward * -1; }
  }
  public Vector3 Forward
  {
    get { return this.transform.forward; }
  }
  public Vector3 Left
  {
    get { return this.transform.right *-1; }
  }
  public Vector3 Up
  {
    get { return this.transform.up; }
  }
  public Vector3 Down
  {
    get { return this.transform.up * -1; }
  }

  public static UnityCamera Find()
  {
    var camera = GameObject.Find("/Main Camera").GetComponent<UnityCamera>();
    //camera.Position = position;
    return camera;
  }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                              