using UnityEngine;

public class UnityAsteroid : MonoBehaviour 
{

  public Vector3 Position
  {
    get { return gameObject.transform.position; }
    set { gameObject.transform.position = value; }
  }

  bool destroyed = false;  
  public bool Destroyed
  {
    get { return destroyed; }
    set
    {
      if (value && !destroyed)
      {
        //World.objects_to_remove.Add(gameObject);
        destroyed = true;
        Debug.Log("Destroyed asteroid");
        //World.elems_to_destroy.Add(gameObject);
        GameObject.Destroy(gameObject);
      }
    }
  }

  public static UnityAsteroid Instantiate(Vector3 position)
  {
    return (GameObject.Instantiate(Resources.Load("Asteroid"), position, Quaternion.identity) as GameObject).GetComponent<UnityAsteroid>();

  }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                              