using UnityEngine;
using System.Collections.Generic;

public class UnityPlayer : MonoBehaviour {

  public string Name;

  public static List<UnityPlayer> FindAll()
  {

    GameObject[] players;
    players = GameObject.FindGameObjectsWithTag("Player");
    List<UnityPlayer> new_players = new List<UnityPlayer>();
    for (int i = 0; i < players.Length; i++)
    {
      var p = players[i].GetComponent<UnityPlayer>();
      new_players.Add(p);
    }
    return new_players;
  }
}
                                                                                                                                                                                                           