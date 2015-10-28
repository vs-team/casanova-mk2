using UnityEngine;
using System.Collections;

public class UnityBackground : MonoBehaviour {
 
	// Use this for initialization
	void Start () {
		transform.GetComponent<Renderer>().material.color = new Color(0.4f, 0.4f, 0.4f, 0.5f);
	}
	
	public Vector2 ScrollingVelocity = Vector2.zero;
	
	// Update is called once per frame
	void Update () 
	{
	  transform.GetComponent<Renderer>().material.mainTextureOffset += ScrollingVelocity * Time.deltaTime;
		//Debug.Log(gameObject.GetComponent<MeshRenderer>().renderer.);
	}
}
                                                                                                                                                                                                                                                                                                                                                                                                                