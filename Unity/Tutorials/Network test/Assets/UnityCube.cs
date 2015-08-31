using UnityEngine;

public class UnityCube : MonoBehaviour 
{

  public int id;

  public void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
  {
    if (stream.isWriting)
    {
      //Debug.Log("Writing: " + id);
      NetworkingPrelude.SerializeBase(id, stream);
    }
    else
    {
      id = NetworkingPrelude.IntReceiveBase(stream);

      if (!Game.World.Entities.ContainsKey(id))
        Game.World.Entities.Add(id, this);

      //Debug.Log("Reading: " + id);
    }
  }


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
  public Vector3 Position
  {
    get { return gameObject.transform.position; }
    set { gameObject.transform.position = value; }
  }



  public static UnityCube Instantiate(Vector3 position, Color color, int id)
  {
    UnityCube cube = null;
    if (Network.isServer)
    {
      cube = (Network.Instantiate(Resources.Load("Cube"), position, Quaternion.identity, 0) as GameObject).GetComponent<UnityCube>();
      cube.id = id;
      Game.World.Entities.Add(id, cube);
      cube.gameObject.renderer.material.color = color;
    }
    else
    {
      if (Game.World.Entities.ContainsKey(id))
      {
        cube = Game.World.Entities[id];
        //Debug.Log("Found: " + id);
      }
      //else
      //{
      //  foreach (var item in Game.World.Entities)
      //  {
      //    Debug.Log("DICk: " + item.Key + " Value: " + item.Value);
          
      //  }
      //  Debug.Log("not found" + id);

        

      //}
    }
    return cube;

  }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               