using UnityEngine;
using System.Collections;

public class UnityCamera : MonoBehaviour 
{
	public Quaternion Rotation
	{
		get{ return this.gameObject.transform.rotation; }
		set
		{
			this.gameObject.transform.rotation = value;
		}
	}

	public Vector3 Position
	{
		get { return this.gameObject.transform.position; }
		set { this.gameObject.transform.position = value; }
	}

	public Vector3 Forward
	{
		get { return this.gameObject.transform.forward; }
		set { this.gameObject.transform.forward  = value; }
	}

	public Vector3 Right
	{
		get { return this.gameObject.transform.right;}
		set { this.gameObject.transform.right = value; }
	}

	public static UnityCamera CreateMainCamera()
	{
		return Camera.main.gameObject.GetComponent<UnityCamera> ();
	}
}                                                                                                                                                                  