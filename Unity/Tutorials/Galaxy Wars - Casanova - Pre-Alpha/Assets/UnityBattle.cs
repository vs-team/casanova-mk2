using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnityBattle : MonoBehaviour {

  private bool destroyed;
  public bool Destroyed
  {
    get { return destroyed; }
    set
    {
      destroyed = value;
      if (destroyed)
        GameObject.Destroy(gameObject);
    }
  }
  private Vector3 target;
  public static UnityBattle Instantiate(Vector3 target)
  {
    var battleprefab = GameObject.Instantiate(Resources.Load("Prefabs/UnityBattle"), Vector3.zero, Quaternion.identity) as GameObject;
    var battle = battleprefab.GetComponent<UnityBattle>();
    battle.target = target;
    return battle;  
  }
  //*
  Animation.Animation battle_animation; //comment this, compile casanova, uncomment

  void Start()
  {
    battle_animation = new Animation.Animation(target);
    battle_animation.Start();
  }

 

  void Update()
  {
    Debug.Log("UPDATE: ATT. SHIPS " + battle_animation.AttackingShips.Count);
    battle_animation.Update(Time.deltaTime, battle_animation);
  }
  //*/  //comment out till here
  public bool StopAnimation
  {
    set {
      stopAnimation = value;
      //battle_animation.StopAnimation = value; 
    }
    get { return stopAnimation; }
  }
  bool stopAnimation;
  public List<UnityShip> UnityAttackingShips
  {
    set
    {
      //*
      List<Animation.Ship> lst = new List<Animation.Ship>(); //comment this, compile casanova, uncomment
      for (int i = 0; i < value.Count; i++)
      {
        var ship = battle_animation.AttackingShips.Find(s => s.UnityShip == value[i]);

        if (ship == null)
        {
          lst.Add(new Animation.Ship(value[i]));
        }
        else
        {
          lst.Add(ship);          
          Debug.Log("FOUND SHIP");

        }

      }
      battle_animation.AttackingShips.Clear();
      battle_animation.AttackingShips.AddRange(lst);
      Debug.Log("SET: ATT. SHIPS " + battle_animation.AttackingShips.Count); //comment out till here
      // */
    }
  }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                              