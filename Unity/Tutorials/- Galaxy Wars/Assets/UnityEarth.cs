using UnityEngine;
using System.Collections;

public class UnityEarth : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	public Vector2 ScrollingVelocity = Vector2.zero;

  // Update is called once per frame
  void Update () {
	  transform.Rotate(ScrollingVelocity * Time.deltaTime);

  }
}
                                                                                                                                                                                                                                                 