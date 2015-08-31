using System.Collections;
using UnityEngine;

/// <summary>
/// Title screen script
/// </summary>
public class MenuScript : MonoBehaviour
{
  string address = "https://dl-web.dropbox.com/get/Files/tracks.csv?_subject_uid=36697746&w=AACPRmayaXa0NXhO5P0OqbIOyFqqggLIwcX4waLTvRPJtQ&dl=1";
  string text;
  void Start()
  {
    ////@"Assets\\Resources\\tracks.csv"
    //if (Application.isWebPlayer)
    //{
    //  Debug.Log("Downloading csv..");
    //  WWW www = new WWW(address);
    //  Debug.Log(www);
    //  yield return www;
    //  text = www.text;
    //  Debug.Log("Downloaded: " + text);

    //}
    //else
    //{

      //text = System.IO.File.ReadAllText(System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "tracks.csv"));
      text =
        "chain8cL_diff,0,1\r\nchain11aG_same,1,0\r\n\r\nchain8cL_diff,0,1\r\nchain11aG_same,1,0\r\n";
    //}

    Track.TrackManager.Init(text);


  }

  void OnGUI()
  {
    const int buttonWidth = 84;
    const int buttonHeight = 60;

    // Determine the button's place on screen
    // Center in X, 2/3 of the height in Y
    Rect buttonRect = new Rect(
          Screen.width / 2 - (buttonWidth / 2),
          (2 * Screen.height / 2.5f) - (buttonHeight / 2),
          buttonWidth,
          buttonHeight
        );

    // Draw a button to start the game
    if (GUI.Button(buttonRect, "Start!"))
    {
      // On Click, load the first level.
      // "Stage1" is the name of the first scene we created.
      Application.LoadLevel("dyslexia");
    }

  }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    