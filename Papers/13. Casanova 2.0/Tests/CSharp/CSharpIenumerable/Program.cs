using System.Collections.Generic;
using System.Collections;
using UnityEngine;
namespace CSharpIenumerable
{
  class Ball
  {
    public Vector2 P { get; set; }
    public Vector2 V { get; set; }

    public Ball()
    {
      P = Vector2.zero;
      V = Vector2.zero;
      p_rule = PRule();
      v_rule = VRule();
    }

    IEnumerator PRule()
    {
      while (true)
      {
        P = P + V;
        yield return null;
      }
    }
    IEnumerator VRule()
    {
      while (true)
      {
        while (P.x <= 500) yield return null;
        P = new Vector2(500, P.y);
        V *= -1.0f;
        yield return null;
        while (P.x > -500) yield return null;
        P = new Vector2(-500, P.y);
        V *= -1.0f;
        yield return null;
      }
    }

    public IEnumerator p_rule, v_rule;

    public void Update(float dt)
    {
      p_rule.MoveNext();
      v_rule.MoveNext();
    }
  }

  class Game
  {
    List<Ball> balls = new List<Ball>();
    public Game()
    {
      for (int i = 0; i < 10000; i++)
      {
        balls.Add(new Ball());
      }
    }

    public void Update(float dt)
    {
      for (int i = 0; i < balls.Count; i++)
      {
        balls[i].Update(dt);
      }
    }
  }
  class Program
  {


    static void Main(string[] args)
    {
      var sw = new System.Diagnostics.Stopwatch();
      var g = new Game();
      var numFrames = 500;
      sw.Start();
      for (int j = 0; j < numFrames; j++)
      {
          g.Update(0.1f);
      }
      sw.Stop();

      System.Console.WriteLine(sw.ElapsedMilliseconds);
      System.Console.WriteLine((float)sw.ElapsedMilliseconds/numFrames);
    }
  }
}
