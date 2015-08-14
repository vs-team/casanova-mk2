using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bouncing_balls_test
{
  class Program
  {
    static void Main(string[] args)
    {
      var world = new World();
      world.Start();

      var t = new System.Diagnostics.Stopwatch();

      t.Reset();
      t.Start();
      var num_frames = 500;
      for (int i = 0; i < num_frames; i++)
      {
        world.Update(0.1f, world);
      }
      t.Stop();


      System.Console.WriteLine(t.ElapsedMilliseconds);

      System.Console.WriteLine((float)t.ElapsedMilliseconds / num_frames);

    }
  }
}
