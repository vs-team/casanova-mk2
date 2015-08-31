using UnityEngine;
using System.Collections;

public class UnityPlanet : MonoBehaviour
{
  public bool LeftDown = false;
  public bool RightDown = false;
  public TextMesh OwnerText, NumArmiesText;

  void Start()
  {
    OwnerText = this.gameObject.transform.FindChild("Owner").GetComponent<TextMesh>();
    NumArmiesText = this.gameObject.transform.FindChild("NumArmies").GetComponent<TextMesh>();
  }

  void OnMouseOver()
  {
    if(Input.GetMouseButtonDown(1))
      RightDown = true;
  }

  void OnMouseDown()
  {
    if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
      RightDown = true;
    else
      LeftDown = true;
  }
}
