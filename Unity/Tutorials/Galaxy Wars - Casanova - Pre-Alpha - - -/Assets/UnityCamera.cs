using UnityEngine;
using System.Collections;

public class UnityCamera : MonoBehaviour 
{

  public Vector3 Position
	{
		get { return this.gameObject.transform.position; }
		set { this.gameObject.transform.position = value; }
	}
	
  public Vector3 Rotation
  {
    get { return this.gameObject.transform.rotation.eulerAngles; }
    set { this.gameObject.transform.rotation = Quaternion.Euler(value); }
  }
	
  public bool Hide
  {
    get { return this.gameObject.renderer.enabled; }
    set { this.gameObject.renderer.enabled = value; }
  }

	public float CameraSize
	{
		get { return this.gameObject.camera.orthographicSize; }
		set { this.gameObject.camera.orthographicSize = value; }
	}
	
  public Vector3 Forward
  {
    get { return this.gameObject.transform.forward; }
    set { this.gameObject.transform.forward = value; }
  }

  public Vector3 Right
  {
    get { return this.gameObject.transform.right; }
    set { this.gameObject.transform.right = value; }
  }

  public Vector3 Up
  {
    get { return this.gameObject.transform.up; }
    set { this.gameObject.transform.up = value; }
  }
	
	public static UnityCamera CreateMainCamera(Vector3 position)
	{
    var camera = GameObject.Find("/MainCamera").GetComponent<UnityCamera>();
    camera.Position = position;
    return camera;
	}
}                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  