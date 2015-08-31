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
      var resultsBackground = GameObject.Find("/ResultsBackground");

      var right = resultsBackground.transform.FindChild("Right");
      var wrong = resultsBackground.transform.FindChild("Wrong");

      var right_score = right_answers / total;
      var wrong_score = 1 - right_answers;

      var right_result_bar = right.transform.FindChild("ResultsBar");
      right_result_bar.transform.localScale = new Vector3((float)700.0 * right_score, right_result_bar.transform.localScale.y, right_result_bar.transform.localScale.z);
      var right_result_bar_end = right.transform.FindChild("ResultsBarEnd");
      if(right_score < 0.8)
        right_result_bar_end.transform.position += new Vector3((float)right_result_bar.renderer.bounds.size.x, 0, 0);

      

      var wrong_result_bar = wrong.transform.FindChild("ResultsBar");
      wrong_result_bar.transform.localScale = new Vector3((float)700.0 * wrong_score, wrong_result_bar.transform.localScale.y, wrong_result_bar.transform.localScale.z);
      var wrong_result_bar_end = wrong.transform.FindChild("ResultsBarEnd");
      if(wrong_score < 0.8)
        wrong_result_bar_end.transform.position += new Vector3((float)wrong_result_bar.renderer.bounds.size.x, 0, 0);

      //var right = GameObject.Find("/Right");
      //right.transform.position = new Vector3(right.transform.position.x, Mathf.Lerp(-6, -2, right_answers / total), right.transform.position.z);

      //var wrong = GameObject.Find("/Wrong");
      //wrong.transform.position = new Vector3(wrong.transform.position.x, Mathf.Lerp(-6, -2, 1 - right_answers / total), wrong.transform.position.z);
    }


  }
  void OnGUI()
  {
    //const int buttonWidth = 100;
    //const int buttonHeight = 60;

    //// Determine the button's place on screen
    //// Center in X, 2/3 of the height in Y
    //Rect buttonRect = new Rect(
    //      Screen.width / 2 - (buttonWidth * 3),
    //      (2 * Screen.height / 2.5f) - (buttonHeight / 2),
    //      buttonWidth,
    //      buttonHeight
    //    );

    //// Draw a button to start the game
    //if (GUI.Button(buttonRect, "Back to main"))
    //{
    //  Track.TrackManager.SaveResultsToFile();
    //  // On Click, load the first level.
    //  // "Stage1" is the name of the first scene we created.
    //  Application.LoadLevel("Menu");
    //}

    //Debug.Log(Track.TrackManager.ExperimentNumber + " vs " + Track.TrackManager.TotExperiments);
    //if (Track.TrackManager.ExperimentNumber <= Track.TrackManager.TotExperiments)
    //{

    //  buttonRect = new Rect(
    //    Screen.width / 2 - (buttonWidth * 3),
    //    (2 * Screen.height / 3.5f) - (buttonHeight / 2),
    //    buttonWidth,
    //    buttonHeight
    //  );

    //  // Draw a button to start the game
    //  if (GUI.Button(buttonRect, "Continue"))
    //  {
    //    // On Click, load the first level.
    //    // "Stage1" is the name of the first scene we created.
    //    Application.LoadLevel("dyslexia");
    //  }
    //}
  }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             