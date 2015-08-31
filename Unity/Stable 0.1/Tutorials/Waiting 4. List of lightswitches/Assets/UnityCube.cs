using UnityEngine;
public class UnityCube : MonoBehaviour
{
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

  public static UnityCube Instantiate()
  {
    var cube = GameObject.Instantiate(Resources.Load("Cube"), UnityEngine.Random.insideUnitSphere, Quaternion.identity) as GameObject;
    return cube.GetComponent<UnityCube>();
  }
}
                                                                                                                 