using UnityEngine;

/// <summary>
/// Title screen script
/// </summary>
public class EndScript : MonoBehaviour
{
  void Start()
  {
    //Track.TrackManager.Init(@"Assets\\Resources\\tracks.csv");
    int total = 0;

    if (Track.TrackManager.JustRunExperiment != null)
    {
      total = Track.TrackManager.JustRunExperiment.Count;

      int right_answers = 0;
      foreach (var item in Track.TrackManager.JustRunExperiment)
      {
        if (item.Answer) right_answers++;
      }
      var right = GameObject.Find("/Right");
      right.transform.position = new Vector3(right.transform.position.x, Mathf.Lerp(-6, -2, right_answers / total), right.transform.position.z);

      var wrong = GameObject.Find("/Wrong");
      wrong.transform.position = new Vector3(wrong.transform.position.x, Mathf.Lerp(-6, -2, 1 - right_answers / total), wrong.transform.position.z);
    }


  }
  void OnGUI()
  {
    const int buttonWidth = 100;
    const int buttonHeight = 60;

    // Determine the button's place on screen
    // Center in X, 2/3 of the height in Y
    Rect buttonRect = new Rect(
          Screen.width / 2 - (buttonWidth * 3),
          (2 * Screen.height / 2.5f) - (buttonHeight / 2),
          buttonWidth,
          buttonHeight
        );

    // Draw a button to start the game
    if (GUI.Button(buttonRect, "Back to main"))
    {
      Track.TrackManager.SaveResultsToFile();
      // On Click, load the first level.
      // "Stage1" is the name of the first scene we created.
      Application.LoadLevel("Menu");
    }

    Debug.Log(Track.TrackManager.ExperimentNumber + " vs " + Track.TrackManager.TotExperiments);
    if (Track.TrackManager.ExperimentNumber <= Track.TrackManager.TotExperiments)
    {

      buttonRect = new Rect(
        Screen.width / 2 - (buttonWidth * 3),
        (2 * Screen.height / 3.5f) - (buttonHeight / 2),
        buttonWidth,
        buttonHeight
      );

      // Draw a button to start the game
      if (GUI.Button(buttonRect, "Continue"))
      {
        // On Click, load the first level.
        // "Stage1" is the name of the first scene we created.
        Application.LoadLevel("dyslexia");
      }
    }
  }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             