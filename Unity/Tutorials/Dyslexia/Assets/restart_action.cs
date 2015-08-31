using UnityEngine;
using System.Collections;

public class restart_action : MonoBehaviour {

	// Use this for initialization
	void Start () {
    if (Track.TrackManager.ExperimentNumber > Track.TrackManager.TotExperiments)
    {
      this.renderer.enabled = false;
      this.enabled = false;
    }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
  void OnMouseDown()
  {
    Time.timeScale = 1;
    if(enabled)
      Application.LoadLevel("dyslexia");
  }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                