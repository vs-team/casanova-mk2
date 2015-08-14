using System.Linq;
using System;
using System.Collections.Generic;
public enum RuleResult { Done, Working }


public class FastStack
{
  public List<int> a;
  public int[] Elements;
  public int Top;

  public FastStack(int elems)
  {
    Top = 0;
    Elements = new int[elems];
  }

  public void Clear() { Top = 0; }
  public void Push(int x) { Elements[Top++] = x; }
}

public class RuleTable
{
  public RuleTable(int elems)
  {
    ActiveIndices = new FastStack(elems);
    SupportStack = new FastStack(elems);
    ActiveSlots = new bool[elems];
  }

  public FastStack ActiveIndices;
  public FastStack SupportStack;
  public bool[] ActiveSlots;

  public void Add(int i)
  {
    if (!ActiveSlots[i])
    {
      ActiveSlots[i] = true;
      ActiveIndices.Push(i);
    }
  }
}


class World
{
  public World(int t)
  {
    NotifySlotPBall0 = new Dictionary<int, HashSet<Ball>>();
    var seed = new Random(0);
    List<Ball> ___balls00;
    ___balls00 = (
      from ___i00 in Enumerable.Range(0, 10000)
      select new Ball(seed.Next(1, t), (ushort)___i00)).ToList();
    Balls = ___balls00;
    BallsWithActiveRules = new Dictionary<int, Tuple<Ball, RuleTable>>();
    foreach (var b in Balls)
    {
      b.Init();
    }
  }
  public List<Ball> Balls;
  //static public List<RuleTable> ActiveRules = new List<RuleTable>();
  static public Dictionary<int, HashSet<Ball>> NotifySlotPBall0 = new Dictionary<int, HashSet<Ball>>();

  public static int frame = 0;

  static public Dictionary<int, Tuple<Ball, RuleTable>> BallsWithActiveRules;
  static public List<int> BallsWithActiveRulesToRemove = new List<int>();
  public void Update(float dt)
  {

    for (int x0 = 0; x0 < Balls.Count; x0++)
      Balls[x0].Update(dt);
    
    if (BallsWithActiveRules.Count > 0)
    {
      foreach (var x in BallsWithActiveRules)
      {
        if (x.Value.Item1.Frame != frame)
        {
          NotifySlotPBall0.Remove(x.Value.Item1.ID);
          BallsWithActiveRulesToRemove.Add(x.Value.Item1.ID);
        }
        else x.Value.Item1.UpdateSuspendedRules(dt, BallsWithActiveRulesToRemove, x.Value.Item2);
      }
      if (BallsWithActiveRulesToRemove.Count > 0)
      {
        for (int i = 0; i < BallsWithActiveRulesToRemove.Count; i++)
          BallsWithActiveRules.Remove(BallsWithActiveRulesToRemove[i]);
      }
    }
    frame++;
  }
}


class Ball
{
  public int Frame;
  public Ball(System.Int32 time, ushort ID)
  {
    Frame = World.frame;
    this.ID = ID;
    X = (1);
    Time = (time);
  }
  public System.Boolean _Run;
  public System.Int32 Time;
  public System.Int32 X;
  public System.Single count_down0;
  public System.Single count_down4;
  public System.Single count_down1;

  public void Init()
  {
    World.NotifySlotPBall0.Add(ID, new HashSet<Ball>());

    World.NotifySlotPBall0[ID].Add(this);

    Run = (false);
  }

  public ushort ID;
  public System.Boolean Run
  {
    get { return _Run; }
    set
    {
      _Run = value;
      for (int i = 0; i < World.NotifySlotPBall0[ID].Count; i++)
      {
        var entity = World.NotifySlotPBall0[ID].ElementAt(i);
        if (!World.BallsWithActiveRules.ContainsKey(ID))
            World.BallsWithActiveRules.Add(ID, new Tuple<Ball, RuleTable>(this, new RuleTable(1)));

        if (entity.Frame == World.frame) World.BallsWithActiveRules[entity.ID].Item2.Add(0);
        else
        {
          World.NotifySlotPBall0.Remove(entity.ID);
        }
      }
    }
  }
  public void Update(float dt)
  {
    this.Frame = World.frame;
    this.Rule1(dt);
  }

  public void UpdateSuspendedRules(float dt, List<int> toRemove, RuleTable ActiveRules)
  {
    if (ActiveRules.ActiveIndices.Top > 0)
    {
      for (int i = 0; i < ActiveRules.ActiveIndices.Top; i++)
      {
        switch (ActiveRules.ActiveIndices.Elements[i])
        {
          case 0:
            if (this.Rule0(dt) == RuleResult.Done)
              ActiveRules.ActiveSlots[i] = false;
            else
              ActiveRules.SupportStack.Push(i);
            break;
          default:
            break;
        }
      }
      ActiveRules.ActiveIndices.Clear();
      var tmp = ActiveRules.SupportStack;
      ActiveRules.SupportStack = ActiveRules.ActiveIndices;
      ActiveRules.ActiveIndices = tmp;
      if (ActiveRules.ActiveIndices.Top == 0)
        toRemove.Add(ID);
    }
  }


  int s1 = -1;
  public void Rule1(float dt)
  {
    switch (s1)
    {

      case -1:
        count_down4 = (Time);
        goto case 5;
      case 5:
        if ((((count_down4)) > ((0f))))
        {

          count_down4 = ((count_down4)) - ((dt));
          s1 = 5;
          return;
        }
        {

          goto case 3;
        }
      case 3:
        Run = (true);
        s1 = 1;
        return;
      case 1:
        count_down1 = (10);
        goto case 2;
      case 2:
        if ((((count_down1)) > ((0f))))
        {

          count_down1 = ((count_down1)) - ((dt));
          s1 = 2;
          return;
        }
        {

          goto case 0;
        }
      case 0:
        Run = (false);
        s1 = -1;
        return;
      default: return;
    }
  }



  int s0 = -1;
  public RuleResult Rule0(float dt)
  {
    switch (s0)
    {

      case -1:
        if (!((Run)))
        {

          s0 = -1;
          return RuleResult.Done;
        }
        {

          goto case 2;
        }
      case 2:
        X = ((X)) + ((1));
        s0 = 0;
        return RuleResult.Working;
      case 0:
        count_down0 = (1f);
        goto case 1;
      case 1:
        if ((((count_down0)) > ((0f))))
        {

          count_down0 = ((count_down0)) - ((dt));
          s0 = 1;
          return RuleResult.Working;
        }
        {

          s0 = -1;
          return RuleResult.Working;
        }
      default: return RuleResult.Done;
    }
  }

}

