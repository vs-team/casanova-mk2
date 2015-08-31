using UnityEngine;

public class UnityAudio : MonoBehaviour {
  

  public void Play(string clip)
  {
    if (audio.isPlaying) audio.Stop();
    Debug.Log("Running:" + clip);
    audio.clip = Resources.Load("Tracks/" + clip) as AudioClip;
    clipLength = audio.clip.length;
    audio.Play();
  }
  

  public void Pause()
  {
    if (audio.isPlaying)
    {
      audio.Pause();
      paused = true;
    }
  }

  bool paused = false;

  public void Resume()
  {
    if (paused)
    {
      audio.Play();
      paused = false;
    }
  }

  private float clipLength;

  public float ClipLength
  {
    get { return clipLength; }
    set { clipLength = value; }
  }
  


  public static UnityAudio Find()
  {

    return GameObject.Find("Audio").GetComponent<UnityAudio>();
  }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           