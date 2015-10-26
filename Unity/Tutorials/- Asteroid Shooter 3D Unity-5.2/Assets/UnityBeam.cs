using UnityEngine;
using System.Collections;

public class UnityBeam : MonoBehaviour {

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

  public static UnityBeam Instantiate()
  {
    var s = GameObject.FindGameObjectWithTag("Ship") as GameObject;
    var beamcannon = s.transform.FindChild("Beam");

    var beam = GameObject.Instantiate(Resources.Load("Laser"), beamcannon.position, Quaternion.identity) as GameObject;
    return beam.GetComponent<UnityBeam>();
  }


}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                          