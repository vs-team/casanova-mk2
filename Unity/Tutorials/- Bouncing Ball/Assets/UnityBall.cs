using UnityEngine;
using System.Collections;

public class UnityBall : MonoBehaviour
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
      destroyed = value;
      if (destroyed)
        GameObject.Destroy(gameObject);
    }
  }


  public static UnityBall Instantiate(Vector3 direction, Vector3 position)
  {
    var s = GameObject.Instantiate(Resources.Load("Ball"), position , Quaternion.identity) as GameObject;
    var rigidS = s.GetComponent<Rigidbody>();
    rigidS.AddForce(direction * 0.7f);
    return s.GetComponent<UnityBall>();

  }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                