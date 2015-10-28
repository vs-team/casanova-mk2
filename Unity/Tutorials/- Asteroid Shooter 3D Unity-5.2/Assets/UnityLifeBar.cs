using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UnityLifeBar : MonoBehaviour
{

  // Use this for initialization
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {

  }

  int life = 3;

  public int Life
  {
    get { return life; }
    set
    {
      life = value;
      switch (life)
      {
        case 01:
          lifebar.text = "<3";
          break;
        case 02:
          lifebar.text = "<3<3";
          break;
        case 03:
          lifebar.text = "<3<3<3";
          break;
        default:
          lifebar.text = ":'(";
          break;
      }
    }
  }

  public bool GameOver
  {
    set
    {
      if (value)
      {
        Application.LoadLevel("GameOver");
      }
    }
  }

  Text lifebar;

  public static UnityLifeBar Find()
  {
    var s = GameObject.FindGameObjectWithTag("LifeBar") as GameObject;

    //LifeBar text
    var script = s.GetComponent<UnityLifeBar>();
    script.lifebar = s.GetComponent<Text>();


    return script;
  }


}
                                                   