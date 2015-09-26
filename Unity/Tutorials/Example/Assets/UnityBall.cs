using UnityEngine;

public class UnityBall : MonoBehaviour 
{


  
  

  public bool ClickedOver
  {
    get
    {
      RaycastHit hit;
      var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      if (Physics.Raycast(ray, out hit, 100))
      {
        if (this.gameObject.name.Equals(hit.collider.gameObject.name))
        {
          //Debug.Log(this.gameObject.name + " selected");
          return true;
        }
        else
          return false;
      }
      else { return false; }
    }
  }

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
  bool destroyed;
  public bool Destroyed
  {
    get { return destroyed; }
    set
    {
      destroyed = value;
      if (destroyed)
        GameObject.Destroy(gameObject);
    }
  }

  public Color Color
  {
    get { return gameObject.GetComponent<Renderer>().material.color; }
    set { gameObject.GetComponent<Renderer>().material.color = value; }
  }
	
  public static UnityBall Find(string model)
  {
    return GameObject.Find(model).GetComponent<UnityBall>();
  }


  public Vector3 MousePosition
  {
    get
    {

      Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      RaycastHit hit;
      Physics.Raycast(ray, out hit);
      return new Vector3(hit.point.x, hit.point.y, 0.0f);


    }
  }

  private Collider plane;
  public static UnityBall Instantiate(Vector3 p)
  {

  
    var cube = GameObject.Instantiate(Resources.Load("Ball"), p - (new Vector3(Random.value * 2.0f, Random.value * 2.0f, 0.0f)), Quaternion.identity) as GameObject;

    var res = cube.GetComponent<UnityBall>();
    //res.plane = plane.collider;
    return res;
  }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                            