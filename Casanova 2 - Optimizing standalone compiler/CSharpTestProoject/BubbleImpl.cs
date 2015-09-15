//using System.Linq;
//using System;
//using System.Collections.Generic;
//using System.Collections;
//using System.Collections.ObjectModel;
//using System.Globalization;

//public enum RuleResult { Done, Working }

//class World
//{
//  static public Ball initBall;
//  bool frameId;
//  public World(int t)
//  {
//    initBall = null;
//    frameId = false;
//    System.Random ___seed00;
//    ___seed00 = new System.Random(0);
//    List<Ball> ___entities00;
//    ___entities00 = (
//      from ___i00 in Enumerable.Range(0, 10000)
//      select new Ball(___seed00.Next((1), (t)), (ushort)___i00)).ToList();
//    Entities = ___entities00;
//    initBall = new Ball(___seed00.Next((1), (t)), ushort.MaxValue);
//    initBall.prev = initBall;
//    initBall.next = initBall;
//  }

//  public List<Ball> Entities;

//  public void Update(float dt)
//  {

//    for (int x0 = 0; x0 < Entities.Count; x0++)
//      Entities[x0].Update(dt, frameId);

//    var currentBall = initBall.next;
//    while (currentBall.id != initBall.id)
//    {
//      if (currentBall.id > currentBall.next.id)
//      {
//        currentBall.next.prev = currentBall.prev;
//        currentBall.next.next.prev = currentBall;
//        currentBall.prev.next = currentBall.next;
//        currentBall.prev = currentBall.next;
//        currentBall.next = currentBall.prev.next;
//        currentBall.prev.next = currentBall;
//        //var one = currentBall.prev;
//        //var two = currentBall;
//        //var three = currentBall.next;
//        //var four = currentBall.next.next;
//        //two.next = four;
//        //two.prev = three;
//        //four.prev = two;
//        //three.next = two;
//        //one.next = three;
//        //three.prev = one;
//        //currentBall = three;
//      }

//      if (currentBall.UpdateSuspendedRules(dt) || currentBall.LastFrameUpdated != frameId)
//      {
//        currentBall.prev.next = currentBall.next;
//        currentBall.next.prev = currentBall.prev;
//        currentBall.HasDynamicRulesActive = false;
//      }
//      else
//      {
//        currentBall = currentBall.next;
//      }
//    }

//    frameId = !frameId;
//  }
//}
//public class Ball
//{
//  //public BallNode myNode;
//  public Ball prev, next;
//  public bool HasDynamicRulesActive = false;

//  public bool LastFrameUpdated;
//  public ushort id;

//  public Ball(System.Int32 time, ushort id)
//  {
//    LastFrameUpdated = false;
//    HasDynamicRulesActive = false;
//    this.id = id;
//    X = (1);
//    _Run = (false);
//    Time = (time);
//  }
//  public System.Boolean _Run;
//  public System.Int32 Time;
//  public System.Int32 X;
//  public System.Single count_down0;
//  public System.Single count_down4;
//  public System.Single count_down1;


//  public System.Boolean Run
//  {
//    get { return _Run; }
//    set
//    {
//      //World.BallHashtable.ActivateBall(this);
//      if (!HasDynamicRulesActive)
//      {
//        HasDynamicRulesActive = true;

//        //var tmp = World.initBall.next;
//        //World.initBall.next = this;
//        //tmp.prev = this;
//        //this.prev = World.initBall;
//        //this.next = tmp;
//        var tmp = World.initBall.prev;
//        World.initBall.prev = this;
//        tmp.next = this;
//        this.next = World.initBall;
//        this.prev = tmp;

//        _Run = value;
//      }
//    }
//  }
//  public void Update(float dt, bool frameid)
//  {
//    LastFrameUpdated = frameid;
//    this.Rule1(dt);
//  }
//  public bool UpdateSuspendedRules(float dt)
//  {
//    return this.Rule0(dt) == RuleResult.Done;
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


