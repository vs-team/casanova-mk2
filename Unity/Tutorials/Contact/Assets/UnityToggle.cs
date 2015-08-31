using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UnityToggle : MonoBehaviour 
{

  public bool IsOn
  {
    get { return this.gameObject.GetComponent<Toggle>().isOn; }
    set { this.gameObject.GetComponent<Toggle>().isOn = value; }
  }

	public static UnityToggle GetToggle(string name)
  {
    return GameObject.Find(name).GetComponent<UnityToggle>();
  }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 