using UnityEngine;
using System.Collections;

public class GameOverMenu : MonoBehaviour
{

  // Use this for initialization
  void Start()
  {

  }


  // Update is called once per frame
  public void ChangeToScene(string sceneToChangeTo)
  {
    Application.LoadLevel(sceneToChangeTo);

  }
}
                                                      