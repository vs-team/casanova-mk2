using UnityEngine;
using System.Collections;
using Lidgren.Network;

public class UnityShip : MonoBehaviour {

    bool hit;
	public static UnityShip Instantiate(Vector3 pos)
    {
        GameObject ship = GameObject.Instantiate(Resources.Load("Ship"), pos, Quaternion.identity) as GameObject;
        ship.GetComponent<UnityShip>().hit = false;
        return ship.GetComponent<UnityShip>();
    }

    public Vector3 Position
    {
        get { return this.gameObject.transform.position; }
        set { this.gameObject.transform.position = value; }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Asteroid")
        {
            hit = true;
        }
    }

    public bool Hit
    {
        get { return hit; }
        set { hit = value; }
    }

}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  