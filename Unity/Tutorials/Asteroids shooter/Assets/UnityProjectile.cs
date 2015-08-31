using UnityEngine;

public class UnityProjectile : MonoBehaviour 
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
        GameObject.Destroy(gameObject);
        //World.elems_to_destroy.Add(gameObject);
        destroyed = true;
        UnityEngine.Debug.Log("Destroyed");
      }
    }
  }

  public static UnityProjectile Instantiate(Vector3 position)
  {
    return (GameObject.Instantiate(Resources.Load("Projectile"), position, Quaternion.identity) as GameObject).GetComponent<UnityProjectile>();

  }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  