using System.Linq;
using System;
using UnityEngine;
using System.Collections.Generic;


public class PointGenerator
{

  public static List<Vector3> GeneratePoints(int points_number, int y_min, int y_max, int x_min, int x_max)
  {
    List<Vector3> points = new List<Vector3>();
    var seed = new System.Random(10);
    var absstepmax = 50;
    var ymin = y_min;
    var ymax = y_max;
    var x = 0;
    var y = 5;
    for (int i = 1; i <= points_number; i++)
    {
      y = y + (seed.Next(1, 2 * absstepmax + 1) - absstepmax - 1);
      y = Math.Max(ymin, Math.Min(ymax, y));

      x = x + (seed.Next(1, 2 * absstepmax + 1) - absstepmax - 1);
      x = Math.Max(x_min, Math.Min(x_max, x));

      points.Add(new Vector3(x, y, 0.0f));
    }
    return points;
  }
  public static List<Vector3> GeneratePoints(int points_number)
  {
    return GeneratePoints(points_number, -100, 100, -100, 100);
  }
}                                                                                                                                                                                                                                                                                                                                                                                                                                                       