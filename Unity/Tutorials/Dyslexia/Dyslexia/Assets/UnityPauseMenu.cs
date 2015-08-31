using UnityEngine;

/// <summary>
/// Title screen script
/// </summary>
public class UnityPauseMenu : MonoBehaviour
{
  //void OnGUI()
  //{
  //  const int buttonWidth = 84;
  //  const int buttonHeight = 60;

  //  // Determine the button's place on screen
  //  // Center in X, 2/3 of the height in Y
  //  Rect resume_rec = new Rect(
  //        Screen.width / 2 - (buttonWidth / 2),
  //        (Screen.height / 2f) - (buttonHeight / 2),
  //        buttonWidth,
  //        buttonHeight
  //      );
    
  //  Rect quit_rec = new Rect(
  //    Screen.width / 2 - (buttonWidth / 2),
  //    (Screen.height / 2f) + 2 * (buttonHeight / 2),
  //    buttonWidth,
  //    buttonHeight
  //  );

  //  Rect skip_rec = new Rect(
  //      Screen.width / 2 - (buttonWidth / 2),
  //      (Screen.height / 2f) - 4 * (buttonHeight / 2),
  //      buttonWidth,
  //      buttonHeight
  //    );

  //  // Draw a button to start the game
  //  if (GUI.Button(resume_rec, "Resume"))
  //  {
  //    resume = true;
  //    //// On Click, load the first level.
  //    //// "Stage1" is the name of the first scene we created.
  //    //Application.LoadLevel("dyslexia");
  //  }
  //  if (GUI.Button(quit_rec, "Quit"))
  //  {
  //    //// On Click, load the first level.
  //    //// "Stage1" is the name of the first scene we created.
  //    Time.timeScale = 1;
  //    Application.LoadLevel("Menu");
  //  }
  //  if (GUI.Button(skip_rec, "Skip"))
  //  {
  //    //// On Click, load the first level.
  //    //// "Stage1" is the name of the first scene we created.
  //    Time.timeScale = 1;
  //    Application.LoadLevel("dyslexia");
  //  }
  //}

  private bool resume = false;

  public bool Resume
  {
    get { return resume; }
    set { resume = value; }
  }


  bool destroyed = false;

  public bool Destroyed
  {
    get { return destroyed; }
    set
    {
      //Resume._Resume
      destroyed = value;
      if (destroyed)
        GameObject.Destroy(gameObject);
    }
  }

  public static UnityPauseMenu Instantiate()
  {
    Application.LoadLevelAdditiveAsync("Pause");

    //var lo = GameObject.Instantiate(Resources.Load("PauseMenu")) as GameObject;
    //UnityPauseMenu unityPauseMenu = lo.gameObject.GetComponent<UnityPauseMenu>();
    //return unityPauseMenu;
    return new UnityPauseMenu();
  }

}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                