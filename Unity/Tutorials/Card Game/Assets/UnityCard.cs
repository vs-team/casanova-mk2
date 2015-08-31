using UnityEngine;
using System.Collections;
using System.IO;
using Casanova.Prelude;

public class UnityCard : MonoBehaviour {

	public static UnityCard Instantiate(Tuple<string, string> CardWords)
  {
    var unity_card = GameObject.Instantiate(Resources.Load("Card"), new Vector3(0.0f,0.0f,0.0f),Quaternion.identity)as GameObject;
    unity_card.transform.GetChild(0).GetComponent<TextMesh>().text = CardWords.Item1;
    unity_card.transform.GetChild(1).GetComponent<TextMesh>().text = CardWords.Item2;
    return unity_card.GetComponent<UnityCard>();
    
       
    
    

  }


  public Vector3 Position
  {
    get{return gameObject.transform.position;}
    set{gameObject.transform.position = value;}
  }
 

  public Quaternion Rotation
  {
    get { return gameObject.transform.rotation; }
    set { gameObject.transform.rotation = value; }
  }
  
 
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                   