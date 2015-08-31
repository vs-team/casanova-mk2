using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public enum BobAnimation { Idle = 0, Walk = 1 }

public class UnityBob : MonoBehaviour
{
  CharacterController controller;
  Animation animation;

  public List<Vector3> Checkpoints { get; private set; }
  private Vector3 velocity;

  public Vector3 Velocity
  {
    get { return velocity; }
    set {
      velocity = new Vector3(value.x, 0.0f, value.z);
      velocity = Vector3.Normalize(velocity) * 0.1f;
    }
  }
  
  public Vector3 Position { get { return transform.position; } }
  public BobAnimation CurrentAnimation
  {
    set
    {
      animation.CrossFade(value.ToString().ToLower());
    }
  }

  void Start()
  {
    controller = this.GetComponent<CharacterController>();
    animation = this.GetComponent<Animation>();

    Velocity = Vector3.zero;

    var checkpoints = this.transform.FindChild("Checkpoints");
    Checkpoints =
      (from c in checkpoints.Cast<Transform>()
       select c.transform.position).ToList();
  }

  void Update()
  {
    if (velocity.sqrMagnitude > 0.0f)
      transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(velocity, Vector3.up), Time.deltaTime * 360f);
    controller.Move(Velocity);
  }

  public static UnityBob Find()
  {
    var cube = GameObject.Find("/Bob");
    return cube.GetComponent<UnityBob>();
  }
}
                                                                                                                      