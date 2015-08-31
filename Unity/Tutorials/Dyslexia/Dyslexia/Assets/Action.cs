using UnityEngine;
using System.Collections;

public class Action : MonoBehaviour
{

  // Use this for initialization
  void Start()
  {

  }

  // Update is called once per frame
  void OnMouseDown()
  {
    //if (Input.GetKeyDown(KeyCode.Space))
    //{
    if (Input.GetMouseButtonDown(0))
    {
      Application.LoadLevel("dyslexia");
      //GetComponent(SpriteRenderer).sprite = newSprite;
    }   
    //}
  }

}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                              