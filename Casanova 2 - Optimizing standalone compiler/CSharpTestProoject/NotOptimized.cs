using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotOptimizedProject
{

  public class World
  {
    public Random ___seed00;
    public World(int N)
    {
      AS = null;

      List<A> ___aas00;
      ___aas00 = (
        from ___i00 in Enumerable.Range(1, N)
        select new A((ushort)___i00)).ToList();
      AS = ___aas00;
      ___seed00 = new Random(0);
    }
    public List<A> AS;

    public void Update(float dt, World world)
    {
      for (int x0 = 0; x0 < AS.Count; x0++)
      {
        AS[x0].Update(dt, world);
      }
    }



  }
  public class A
  {
    public A(ushort ID)
    {
      this.ID = ID;
    }


    public System.Boolean _N;
    public System.Single count_down1;
    public ushort ID;
    public System.Boolean Y;

    public System.Boolean N
    {
      get { return _N; }
      set
      {
        _N = value;
      }
    }


    public void Update(float dt, World world)
    {
      this.Rule1(dt, world);
      this.Rule0(dt, world);
    }

    int s1 = -1;
    public void Rule1(float dt, World world)
    {
      switch (s1)
      {

        case -1:
          count_down1 = (float)(-alpha * Math.Log(1 - world.___seed00.NextDouble()));
          goto case 3;
        case 3:
          if ((((count_down1)) > ((0f))))
          {

            count_down1 = ((count_down1)) - ((dt));
            s1 = 3;
            return;
          }
          else
          {

            goto case 1;
          }
        case 1:
          N = (true);
          s1 = 0;
          return;
        case 0:
          N = (false);
          s1 = -1;
          return;
        default: return;
      }
    }


    public static float alpha = 0;
    public static double delta_tu = 0.0;
    int s0 = -1;
    public RuleResult Rule0(float dt, World world)
    {
      switch (s0)
      {

        case -1:
          if (!N)
          {
            s0 = -1;
            return RuleResult.Done;
          }
          else
          {
            goto case 0;
          }
        case 0:
          Y = (Y);
          s0 = -1;
          return RuleResult.Working;

        default: return RuleResult.Done;
      }
    }
  }
}
