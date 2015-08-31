using UnityEngine;

public class UnityCube : CasanovaBehavior 
{
 

  public static UnityCube Instantiate(Vector3 position)
  {
    //Debug.Log(position);
    var cube = (GameObject.Instantiate(Resources.Load("Cube"), position, Quaternion.identity) as GameObject).GetComponent<UnityCube>();
    cube.Position = position;
    return cube;

  }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            