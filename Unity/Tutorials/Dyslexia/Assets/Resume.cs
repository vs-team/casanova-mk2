using UnityEngine;
using System.Collections;

public class Resume : MonoBehaviour {

  public static bool _Resume = false;
	// Use this for initialization
	void Start () {
    _Resume = false;
    Destroyed = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

  void OnMouseDown()
  {
    _Resume = true;
    Destroyed = true;
  }

  public static bool destroyed = false;

  public static bool Destroyed
  {
    get { return destroyed; }
    set
    {
      destroyed = value;
      if (destroyed)
      {
        var gameObject = GameObject.Find("/GameObject");

        GameObject.Destroy(gameObject);

      }
    }
  }

}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        