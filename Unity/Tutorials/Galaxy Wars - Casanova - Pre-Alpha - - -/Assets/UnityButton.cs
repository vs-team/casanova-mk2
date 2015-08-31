using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Casanova.Prelude;
using System.Collections.Generic;


public class UnityButton : MonoBehaviour
{

  private bool isPressed = false;

  public bool IsPressed { get { return isPressed; } set { isPressed = value; } }

  public static UnityButton Find(string model)
  {
    var x = GameObject.Find(model).GetComponent<UnityButton>();
    return x;
  }

  public void Pressed()
  {
    IsPressed = true;
    Debug.Log("Pressed Void");
  }
}                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               