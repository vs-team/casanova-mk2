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
  private bool quit;

  public Vector3 Velocity
  {
    get { return velocity; }
    set
    {
      velocity = new Vector3(value.x, 0.0f, value.z);
      velocity = Vector3.Normalize(velocity) * 0.1f;
    }
  }

  public bool Quit { set { this.quit = true; Application.Quit(); } }

  public Vector3 Position
  {
    get { return transform.position; }
    set { transform.position = value; }
  }
  public BobAnimation CurrentAnimation
  {
    set
    {
      animation.CrossFade(value.ToString().ToLower());
    }
  }

  public Vector3 Forward
  {
    get { return this.transform.forward; }
  }

  void Start()
  {
    quit = false;
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

  private Light spotlight;
  private Renderer sphere;

  private bool selected;
  public bool Selected
  {

    get
    {
      return selected;
    }
    set
    {
      selected = value;
      sphere.enabled = value;
      spotlight.enabled = value;
    }
  }

  public bool IsHit
  {
    get
    {
      Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      RaycastHit hit;

      if (Physics.Raycast(ray, out hit))
        if (this.gameObject.Equals(hit.collider.gameObject))
          return true;
        else
          return false;
      else
        return false;
    }
  }

  private bool mouseHover;
  public bool MouseHover
  {
    get
    { return mouseHover; }
    set
    {
      if (!selected)
      {
        mouseHover = value;
        spotlight.enabled = value;
      }
    }
  }

  public static UnityBob Find(string bobby)
  {
    var object_bob = GameObject.Find("Bob" + bobby);
    UnityBob bob = object_bob.GetComponent<UnityBob>();

    var light = bob.transform.FindChild("Spotlight");
    Light spot = light.GetComponent<Light>();
    bob.spotlight = spot;

    var spher = bob.transform.FindChild("Sphere");
    Renderer sphera = spher.GetComponent<Renderer>();

    bob.sphere = sphera;
    return bob;
  }
}                                                                                          