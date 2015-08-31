using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class UnityBackground : MonoBehaviour {
 
	// Use this for initialization
	void Start () {
	}

  private List<Transform> checkPoints = new List<Transform>();

  public List<Transform> CheckPoints
  {
    get { return  checkPoints; }
    set {  checkPoints = value; }
  }
  
	
	public float rotatingVelocity = 0;
  public float RotatingVelocity { get { return rotatingVelocity; } set { rotatingVelocity = value; } }
	// Update is called once per frame
	void Update () 
	{
		transform.Rotate(Vector3.up * RotatingVelocity * Time.deltaTime);
		//Debug.Log(gameObject.GetComponent<MeshRenderer>().renderer.);
	}


  public static UnityBackground Find(string elem, float rotatingVelocity)
  {
    
    var background = GameObject.Find(elem).GetComponent<UnityBackground>();
    var items = new List<Transform>();
    foreach (Transform item in background.transform)
    {
      int n = 0;
      if (System.Int32.TryParse(item.name, out n)) { items.Add(item); }
    }
    var new_list = items.OrderBy(x => System.Int32.Parse(x.name)).Reverse().ToList();    
    background.CheckPoints.AddRange(new_list.Select(x => x.gameObject.transform).ToList());
    background.RotatingVelocity = rotatingVelocity;
    return background;

  }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         