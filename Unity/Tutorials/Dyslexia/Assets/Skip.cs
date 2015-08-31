using UnityEngine;
using System.Collections;

public class Skip : MonoBehaviour {

	// Use this for initialization
	void Start () {
    if (Track.TrackManager.ExperimentNumber > Track.TrackManager.TotExperiments)
    {
      this.renderer.enabled = false;
      this.enabled = false;
	  Track.TrackManager.SaveResultsToFile();
    }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
  void OnMouseDown()
  {
    Time.timeScale = 1;
    Track.TrackManager.SaveAll();
    Track.TrackManager.MoveNextExperiment();
      
      Application.LoadLevel("End");
    }
  
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                