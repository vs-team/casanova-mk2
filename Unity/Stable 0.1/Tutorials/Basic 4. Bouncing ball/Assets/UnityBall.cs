using UnityEngine;

public class UnityBall : MonoBehaviour 
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

  public static UnityBall Find()
  {
    return GameObject.Find("/Ball").GetComponent<UnityBall>();
  }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            