using UnityEngine;
using System.Collections;

public class TextField : MonoBehaviour {

  public static TextField Instantiate()
  {
    var text_field = GameObject.Instantiate(Resources.Load("InputBox"), new Vector3(0.0f, 0.0f, 0.0f),Quaternion.identity)as GameObject;
    return text_field.GetComponent<TextField>();
  }
  
  public string Text
  {
    get { return this.GetComponent<TextMesh>().text; }
    set { this.GetComponent<TextMesh>().text = value; }

  }
  //// Use this for initialization
  //void Start () {
	
  //}
	
  //// Update is called once per frame
  //void Update () {
	
	//}
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                   