//using System.Linq;
//using System;
//using System.Collections.Generic;
//using System.Collections;
//using System.Collections.ObjectModel;
//using System.Globalization;

//class BallTree
//{
//  public BallNode root;
//  BallNode smallestBall;


//  public int elems(BallNode node)
//  {
//    if (node == null) return 0;
//    else
//    {
//      return height(node.right) + height(node.left) + 1;
//    }
//  }


//  public int height(BallNode node)
//  {
//    if (node == null) return 0;
//    else
//    {
//      var left = height(node.left);
//      var right = height(node.right);
//      var max = left;
//      if (right > max) max = right;
//      return max + 1;
//    }
//  }

//  public void add(Ball b)
//  {
//    var ball_id = b.id;
//    if (root == null)
//    {
//      smallestBall = new BallNode(b, null);
//      b.myNode = smallestBall;
//      root = smallestBall;
//    }
//    else
//    {
//      var current = root;
//      var parent = root.parent;
//      while (current != null)
//      {
//        parent = current;
//        if (current.id > ball_id)
//          current = current.left;
//        else if (current.id < ball_id)
//          current = current.right;
//        else return;
//      }
//      var ball = new BallNode(b, parent);
//      b.myNode = ball;
//      if (parent.id > ball_id)
//      {
//        if (smallestBall != null && smallestBall.id > ball.id)
//          smallestBall = ball;
//        parent.left = ball;
//        while (parent.parent != null && parent.id > ball.id)
//          parent = parent.parent;

//        var tmp = parent.value.next;
//        parent.value.next = ball.value;
//        ball.value.prev = parent.value;
//        if (tmp != null)
//          tmp.prev = ball.value;
//        ball.value.next = tmp;

//      }
//      else
//      {
//        parent.right = ball;
//        var tmp = parent.value.next;
//        parent.value.next = ball.value;
//        ball.value.prev = parent.value;
//        if (tmp != null)
//          tmp.prev = ball.value;
//        ball.value.next = tmp;
//      }
//    }
//  }

//  public Ball removeBall(BallNode node)
//  {
//    Ball next;
//    var id = node.id;
//    if (node.left == null)
//    {
//      if (node.parent == null)
//      {
//        root = node.right;
//        if (node.right != null)
//          node.right.parent = null;
//      }
//      else if (node.parent.left != null && node.id == node.parent.left.id)
//      {
//        node.parent.left = node.right;
//        if (node.right != null)
//          node.right.parent = node.parent;
//      }
//      else
//      {
//        node.parent.right = node.right;
//        if (node.right != null)
//          node.right.parent = node.parent;
//      }
//      node.value.myNode = null;
//    }
//    else if (node.right == null)
//    {
//      if (node.parent == null)
//      {
//        root = node.left;
//        if (node.left != null)
//          node.left.parent = null;
//      }
//      else if (node.parent.left != null && node.id == node.parent.left.id)
//      {
//        node.parent.left = node.left;
//        if (node.left != null)
//          node.left.parent = node.parent;
//      }
//      else
//      {
//        node.parent.right = node.left;
//        if (node.left != null)
//          node.left.parent = node.parent;
//      }
//      node.value.myNode = null;
//    }
//    else
//    {
//      var currentNode = node.left;
//      while (currentNode.right != null)
//      {
//        currentNode = currentNode.right;
//      }
//      if (currentNode.parent.id != node.id)
//      {
//        currentNode.parent.right = null;
//        currentNode.parent.right = currentNode.left;
//        if (currentNode.left != null)
//          currentNode.left.parent = currentNode.parent;
//      }
//      if (node.value.prev != null)
//        node.value.prev.next = node.value.next;
//      if (node.value.next != null)
//        node.value.next.prev = node.value.prev;
//      if (smallestBall.id == node.id)
//        smallestBall = node.value.next.myNode;

//      next = node.value.next;
//      node.value.next = null;
//      node.value.prev = null;

//      node.value = currentNode.value;
//      node.id = currentNode.id;
//      currentNode.value.myNode = node;
//      return next;
//    }
//    if (node.value.prev != null)
//      node.value.prev.next = node.value.next;
//    if (node.value.next != null)
//      node.value.next.prev = node.value.prev;
//    if (smallestBall.id == node.id)
//      if (node.value.next != null)
//        smallestBall = node.value.next.myNode;//findBall(node.value.next);
//      else smallestBall = null;

//    next = node.value.next;
//    node.value.next = null;
//    node.value.prev = null;
//    return next;
//  }


//  ////invariant: the ball must exists
//  //public BallNode findBall(Ball ball)
//  //{
//  //  if (root == null || ball == null)
//  //    return null;
//  //  else
//  //  {
//  //    var current = root;
//  //    var id = ball.id;
//  //    while (current.id != id)
//  //    {
//  //      if (current.id > id)
//  //        current = current.left;
//  //      if (current.id < id)
//  //        current = current.right;
//  //    }
//  //    return current;
//  //  }

//  //}

//  public void traverseBalls(Single dt, int frameID)
//  {
//    if (smallestBall != null)
//    {
//      var ball = smallestBall.value;
//      while (ball != null)
//      {
//        if (ball.UpdateSuspendedRules(dt) || ball.LastFrameUpdated < frameID)
//        {
//          //ball.HasDynamicRulesActive = false;
//          var ball_node = ball.myNode; //findBall(ball);
//          ball = removeBall(ball_node);
//        }
//        else
//        {
//          ball = ball.next;
//        }
//      }
//    }
//  }
//}

//public class BallNode
//{
//  public Ball value;
//  public int id;
//  public BallNode parent, left, right;

//  public BallNode(Ball b, BallNode parent)
//  {
//    this.parent = parent;
//    value = b;
//    this.id = b.id;
//  }
//}


//public enum RuleResult { Done, Working }

//class World
//{
//  static public BallTree tree;
//  int frameId;
//  public World(int t)
//  {
//    tree = new BallTree();
//    frameId = 0;
//    System.Random ___seed00;
//    ___seed00 = new System.Random(0);
//    List<Ball> ___entities00;
//    ___entities00 = (
//      from ___i00 in Enumerable.Range(0, 1000000)
//      select new Ball(___seed00.Next((1), (t)), ___i00)).ToList();

//    //var head = ___entities00[0];
//    //for (int i = 1; i < ___entities00.Count - 1; i++)
//    //{
//    //  var current = ___entities00[i];
//    //  var prev = ___entities00[i - 1];
//    //  var next = ___entities00[i + 1];
//    //  current.prev = prev;
//    //  prev.next = current;
//    //  current.next = next;
//    //  next.prev = current;
//    //}

//    Entities = ___entities00;
//  }

//  public List<Ball> Entities;

//  public void Update(float dt)
//  {
//    for (int x0 = 0; x0 < Entities.Count; x0++)
//      Entities[x0].Update(dt, frameId);
//    tree.traverseBalls(dt, frameId);
//    frameId++;
//  }
//}
//public class Ball
//{
//  public BallNode myNode;
//  public Ball prev, next;

//  //public bool HasDynamicRulesActive = false;
//  public int LastFrameUpdated;
//  public int id;

//  public Ball(System.Int32 time, int id)
//  {
//    LastFrameUpdated = 0;
//    //HasDynamicRulesActive = false;
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
//      //HasDynamicRulesActive = true;
//      World.tree.add(this);
//      _Run = value;
//    }
//  }
//  public void Update(float dt, int frameid)
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



