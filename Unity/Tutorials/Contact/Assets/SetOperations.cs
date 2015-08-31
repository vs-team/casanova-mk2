using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SetOperations<T> where T : System.IComparable
{
  public static IEnumerable<T> difference(IEnumerable<T> l1, IEnumerable<T> l2)
  {
    List<T> d = new List<T>();
    foreach (var x in l1)
    {
      foreach (var y in l2)
      {
        if (x.CompareTo(y) != 0)
        {
          d.Add(x);
        }
      }
    }
    return d;
  }

  public static IEnumerable<T> union(IEnumerable<T> l1, IEnumerable<T> l2)
  {
    List<T> u = new List<T>();
    foreach (var x in l1)
    {
      u.Add(x);
    }
    foreach (var y in l2)
    {
      u.Add(y);
    }
    return u;
  }

  public static IEnumerable<T> intersection(IEnumerable<T> l1, IEnumerable<T> l2)
  {
    List<T> i = new List<T>();
    foreach (var x in l1)
    {
      foreach (var y in l2)
      {
        if (x.CompareTo(y) == 0)
          i.Add(x);
      }
    }
    return i;
  }
}                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               