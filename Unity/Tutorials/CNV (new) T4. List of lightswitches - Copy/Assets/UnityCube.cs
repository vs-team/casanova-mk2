using UnityEngine;
public class UnityCube : MonoBehaviour
{

  public Vector3 Position
  {
    get { return gameObject.transform.position; }
    set { gameObject.transform.position = value; }
    //set 
    //{
    //  Vector3 pos = gameObject.transform.position;
    //  gameObject.transform.position = new Vector3(value, pos.y, pos.z);
    //}
  }
  public Color Color
  {
    set { gameObject.renderer.material.color = value; }
  }

  bool destroyed = false;
public float Scale{
		get{
			return gameObject.transform.localScale.x;
				}
		set{
			gameObject.transform.localScale = new Vector3(value, value, value);
				}
	}
  public bool Destroyed
  {
    get { return destroyed; }
    set { 
      destroyed = value;
      if (destroyed)
        GameObject.Destroy(gameObject);
    }
  }

  public static UnityCube Instantiate(Vector3 p, System.Collections.Generic.IEnumerable<int> elems)
  {
    System.Random r = new System.Random(0);    
    //var p1 = new Vector3(p.x, p.y (10 * ), p.z);
    var cube = GameObject.Instantiate(Resources.Load("Cube"), p, Quaternion.identity) as GameObject;
    return cube.GetComponent<UnityCube>();
  }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                      