using UnityEngine;
using System.Collections;

public class UnityParticleSystem : MonoBehaviour
{

  private float speed;
  public float Speed
  {
    //gameObject.particleSystem.
    set {
      speed= value; 
    
    }
  }

  public static UnityParticleSystem Find()
  {
    return GameObject.Find("/ParticleSystem").GetComponent<UnityParticleSystem>();
  }

  void LateUpdate()
  {

    ParticleSystem.Particle[] p = new ParticleSystem.Particle[GetComponent<ParticleSystem>().particleCount + 1];
    int l = GetComponent<ParticleSystem>().GetParticles(p);

    int i = 0;
    while (i < l)
    {
      p[i].velocity = new Vector3(p[i].velocity.x, p[i].velocity.y, speed);
      i++;
    }

    GetComponent<ParticleSystem>().SetParticles(p, l);

  }

}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            