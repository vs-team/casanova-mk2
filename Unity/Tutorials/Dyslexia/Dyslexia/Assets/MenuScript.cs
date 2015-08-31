using System.Collections;
using System.Xml;
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
    //@"Assets\\Resources\\tracks.csv"
    //if (false)
    //{
    ////  Debug.Log("Downloading csv..");
    ////  WWW www = new WWW(address);
    ////  Debug.Log(www);
    ////  yield return www;
    ////  text = www.text;
    ////  Debug.Log("Downloaded: " + text);
    //  yield return "";
    //}
    //else
    //{

    var file = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "tracks.xml");

    UnityEngine.Debug.Log(file);
    XmlDocument doc = new XmlDocument();
    doc.Load(file);
    //text = "chain8cL_diff,0,1\r\nchain11aG_same,1,0\r\n\r\nchain8cL_diff,0,1\r\nchain11aG_same,1,0\r\nend\r\n";
    //}

    var XMLTests = new Track.XMLTests();
    foreach (XmlNode test in doc.DocumentElement.ChildNodes)
    {
      var XMLTest = new Track.XMLTest();
      foreach (XmlNode tutorial_or_actual_test in test.ChildNodes)
      {
		if (tutorial_or_actual_test.Name.ToLower() == "tutorial")
          XMLTest.TutorialTracks.Add(new Track.XMLTutorial(tutorial_or_actual_test.InnerText)); //or loop through its children as well
        else
          XMLTest.ActualTracks.Add(new Track.XMLActual(tutorial_or_actual_test.InnerText)); //or loop through its children as well
      }
      XMLTests.Tests.Add(XMLTest);
    }

    Track.TrackManager.Init(XMLTests);
  }


}

  //void OnGUI()
  //{
  //  const int buttonWidth = 84;
  //  const int buttonHeight = 60;

  //  // Determine the button's place on screen
  //  // Center in X, 2/3 of the height in Y
  //  Rect buttonRect = new Rect(
  //        Screen.width / 2 - (buttonWidth / 2),
  //        (2 * Screen.height / 2.5f) - (buttonHeight / 2),
  //        buttonWidth,
  //        buttonHeight
  //      );

  //  // Draw a button to start the game
  //  if (GUI.Button(buttonRect, "Start!"))
  //  {
  //    // On Click, load the first level.
  //    // "Stage1" is the name of the first scene we created.
      
  //  }

  //}

                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    