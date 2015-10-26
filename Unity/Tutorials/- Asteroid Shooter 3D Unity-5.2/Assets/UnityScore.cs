using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UnityScore : MonoBehaviour
{

  // Use this for initialization
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {

  }


  int score = 0;

  public int Score
  {
    get { return score; }
    set
    {

      score = value;
      scorename.text = score.ToString();

    }
  }

  Text scorename;

  public static UnityScore Find()
  {
    var s = GameObject.FindGameObjectWithTag("ScoreField") as GameObject;


    var script = s.GetComponent<UnityScore>();
    script.scorename = s.GetComponent<Text>();
    return script;
  }

}
                                                                                                                                          