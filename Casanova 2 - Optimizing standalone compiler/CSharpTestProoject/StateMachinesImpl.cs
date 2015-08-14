//using System.Linq;
//using System;
//using System.Collections.Generic;
//public enum RuleResult { Done, Working }


//public class FastStack
//{
//  public int[] Elements;
//  public int Top;

//  public FastStack(int elems)
//  {
//    Elements = new int[elems];
//  }

//  public void Clear() { Top = 0; }
//  public void Push(int x) { Elements[Top++] = x; }
//}

//public class RuleTable
//{
//  public RuleTable(int elems)
//  {
//    Active = new Func<float, RuleResult>[elems];
//    ActiveIndices = new FastStack(elems);
//    SupportStack = new FastStack(elems);
//    ActiveSlots = new bool[elems];
//  }

//  Func<float, RuleResult>[] Active;
//  FastStack ActiveIndices;
//  FastStack SupportStack;
//  bool[] ActiveSlots;

//  public void Add(Func<float, RuleResult> r, int i)
//  {
//    if (!ActiveSlots[i])
//    {
//      Active[i] = r;
//      ActiveSlots[i] = true;
//      ActiveIndices.Push(i);
//    }
//  }

//  public void StepAll(float dt)
//  {
//    for (int i = 0; i < ActiveIndices.Top; i++)
//    {
//      if (Active[i](dt) == RuleResult.Done)
//        ActiveSlots[i] = false;
//      else
//        SupportStack.Push(i);
//    }
//    ActiveIndices.Clear();
//    var tmp = SupportStack;
//    SupportStack = ActiveIndices;
//    ActiveIndices = tmp;
//  }
//}


//class World
//{
//  public World(int t)
//  {
//    System.Random ___seed00;
//    ___seed00 = new System.Random();
//    List<A> ___entities00;
//    ___entities00 = (
//      from ___i00 in Enumerable.Range(1, 10000)
//      select new A(___seed00.Next((1), (t)))).ToList();
//    Entities = ___entities00;
//  }
//  public List<A> Entities;
//  public void Update(float dt)
//  {
//    for (int x0 = 0; x0 < Entities.Count; x0++)
//    {
//      Entities[x0].Update(dt);
//    }




//  }






//}
//class A
//{
//  public A(System.Int32 time)
//  {
//    X = (1);
//    Run = (false);
//    Time = (time);
//  }
//  public System.Boolean _Run;
//  public List<A> NotifySlotRunA0 = new List<A>();
//  public System.Int32 Time;
//  public System.Int32 X;
//  public System.Single count_down0;
//  public System.Single count_down4;
//  public System.Single count_down1;
//  public RuleTable ActiveRules = new RuleTable(1);
//  public System.Boolean Run
//  {
//    get { return _Run; }
//    set
//    {
//      _Run = value;
//    }
//  }
//  public void Update(float dt)
//  {

//    this.Rule1(dt);
//    this.Rule0(dt);
//  }


//  int s1 = -1;
//  public void Rule1(float dt)
//  {
//    switch (s1)
//    {

//      case -1:
//        count_down4 = (Time);
//        goto case 5;
//      case 5:
//        if ((((count_down4)) > ((0f))))
//        {

//          count_down4 = ((count_down4)) - ((dt));
//          s1 = 5;
//          return;
//        }
//        {

//          goto case 3;
//        }
//      case 3:
//        Run = (true);
//        s1 = 1;
//        return;
//      case 1:
//        count_down1 = (10);
//        goto case 2;
//      case 2:
//        if ((((count_down1)) > ((0f))))
//        {

//          count_down1 = ((count_down1)) - ((dt));
//          s1 = 2;
//          return;
//        }
//        {

//          goto case 0;
//        }
//      case 0:
//        Run = (false);
//        s1 = -1;
//        return;
//      default: return;
//    }
//  }



//  int s0 = -1;
//  public RuleResult Rule0(float dt)
//  {
//    switch (s0)
//    {

//      case -1:
//        if (!((Run)))
//        {

//          s0 = -1;
//          return RuleResult.Done;
//        }
//        {

//          goto case 2;
//        }
//      case 2:
//        X = ((X)) + ((1));
//        s0 = 0;
//        return RuleResult.Working;
//      case 0:
//        count_down0 = (1f);
//        goto case 1;
//      case 1:
//        if ((((count_down0)) > ((0f))))
//        {

//          count_down0 = ((count_down0)) - ((dt));
//          s0 = 1;
//          return RuleResult.Working;
//        }
//        {

//          s0 = -1;
//          return RuleResult.Working;
//        }
//      default: return RuleResult.Done;
//    }
//  }



//}
