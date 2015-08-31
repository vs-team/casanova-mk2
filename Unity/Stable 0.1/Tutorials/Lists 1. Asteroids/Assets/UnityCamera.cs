using UnityEngine;
using System.Collections;

public class UnityCamera : MonoBehaviour 
{

	public Vector3 CameraPosition
	{
		get { return this.gameObject.transform.position; }
		set { this.gameObject.transform.position = value; }
	}
	
	public float CameraSize
	{
		get { return this.gameObject.camera.orthographicSize; }
		set { this.gameObject.camera.orthographicSize = value; }
	}
	
	
	public static UnityCamera CreateMainCamera()
	{
		return GameObject.Find ("/GameCamera").GetComponent<UnityCamera>();
	}
}                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 