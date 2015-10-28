using UnityEngine;
using System.Collections;

public class UnityCamera : MonoBehaviour 
{
  private bool quit;

  public bool Quit
  {
		set { this.quit = true; Application.Quit(); }
  }

	public Vector3 CameraPosition
	{
		get { return this.gameObject.transform.position; }
		set { this.gameObject.transform.position = value; }
	}
	
	public float CameraSize
	{
		get { return this.gameObject.GetComponent<Camera>().orthographicSize; }
		set { this.gameObject.GetComponent<Camera>().orthographicSize = value; }
	}
	
	
	public static UnityCamera CreateMainCamera()
	{
		return GameObject.Find ("/GameCamera").GetComponent<UnityCamera>();
	}
}                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  