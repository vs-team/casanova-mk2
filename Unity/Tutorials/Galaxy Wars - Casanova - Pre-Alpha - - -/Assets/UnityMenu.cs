using UnityEngine;
using System.Collections;
using Casanova.Prelude;
using System.Collections.Generic;
using UnityEngine.UI;

public class UnityMenu : MonoBehaviour
{

  public string PlayerAmount
  {
    get
    {
      return GameObject.Find("Canvas/AmountOfP").GetComponent<Text>().text;
    }
    set
    {
      GameObject.Find("Canvas/AmountOfP").GetComponent<Text>().text = AmountOfPlayers.ToString();
    }
  }

  public Texture MapSelecter
  {
    get
    {
      return GameObject.Find("Canvas/MapSelect").GetComponent<RawImage>().texture;
    }
    set
    {
      Object[] bla = Resources.LoadAll("MapsPreview", typeof (Texture)); // list of all maps
      int z = bla.Length;
      var x = MapSelectNumber % z;
      Texture p = (Texture)bla[x];
      MapName = p.name;
      GameObject.Find("Canvas/MapSelect").GetComponent<RawImage>().texture = p;
    }
  }
  public static int MapSelectNumber = 2;
  public static string MapName = "Map1";

  public static int AmountOfPlayers = 2;

  public int CurrentScreenNumber
  {
    get { return Application.loadedLevel; }
    set
    {
      if (value != -1) { Application.LoadLevel(value); }
      else { Application.Quit(); }
    }
  }
}
                                                                                                                                                                                                                                                                                                                                                                                                      