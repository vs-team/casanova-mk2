using UnityEngine;
using System.Collections;

public class UnityCamera : MonoBehaviour 
{
	private Vector3 prevPos;
	private Vector3 curPos;
	
	public Quaternion Rotation
	{
		get{ return this.gameObject.transform.rotation; }
		set
		{
			this.gameObject.transform.rotation = value;
		}
	} 
	
	public Vector3 Acceleration
	{
		set { this.gameObject.transform.parent.rigidbody.AddForce(value); }
	}
	

	public Vector3 Forward
	{
		get { return this.gameObject.transform.forward; }
		set { this.gameObject.transform.forward  = value; }
	}
	
	public void Start()
	{
		Screen.showCursor = false;
	}
 
	public Vector3 Right
	{
		get { return this.gameObject.transform.right;}
		set { this.gameObject.transform.right = value; }
	}

	public static UnityCamera Find()
	{
		return Camera.main.gameObject.GetComponent<UnityCamera> ();
	}
}                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                   