using UnityEngine;
using System.Collections;

public class UnityPlayerHealth : MonoBehaviour
{

  public static UnityPlayerHealth Find(string name)
  {
    return GameObject.Find(name).GetComponent<UnityPlayerHealth>();
  }

  public bool Active
  {
    set { this.gameObject.SetActive(value); }
  }

}
                                                                                                                                                                                                                                                                                                                                                                                             