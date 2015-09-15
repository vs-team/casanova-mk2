using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace CSharpTestProoject
{
  class Program
  {



    static void Main(string[] args)
    {

      var a = (new List<Vector3>()).Average(x => 1);
      var c = Vector3.zero;
      var z = c.magnitude;

      var max_k = 10;
      var numFrames = 100000;
      var experiments = new double[2, max_k];
      var dt = 0.0166f;
      var counter = 0;
      var sw = new System.Diagnostics.Stopwatch();

      for (int N = 100; N < 11000; N += 2500)
        for (double k = 0.001; k < 0.5; k += 0.1)
        {
          //var alpha = 1.0 / k;

          //World g;
          //g = new World(N);
          //g.alpha = NotOptimizedProject.A.alpha = (float)alpha;

          //sw.Start();
          //for (int j = 0; j < numFrames; j++)
          //{
          //  g.Update(dt, g);
          //}

          //sw.Stop();
          //var optimized_performance = (float)sw.ElapsedMilliseconds / numFrames;
          //sw.Reset();
          //GC.Collect();

          //NotOptimizedProject.World g1;
          //g1 = new NotOptimizedProject.World(N);
          //sw.Start();
          //for (int j = 0; j < numFrames; j++)
          //{
          //  g1.Update(dt, g1);
          //}

          //sw.Stop();
          //var unoptimized_performance = (float)sw.ElapsedMilliseconds / numFrames;
          //sw.Reset();


          //var alphaTP = "alpha= " + k + "; ";
          //var oneOverLambda = ""; //"1/lambda= " + (A.delta_tu).ToString("0##.##");
          //var sec = "  sec= " + (1 / k).ToString("0##.#");
          //var totActiv = "  total rule time per entity = " + (g.total_rule_time / g.total_rule_counter);
          //var perf = " %= " + ((unoptimized_performance / optimized_performance - 1) * 100).ToString("0##.#");


          //System.Console.WriteLine(alphaTP + oneOverLambda + totActiv + perf + sec);

          //experiments[1, counter] = unoptimized_performance - optimized_performance;

          ////experiments[3, counter] = k;
          ////counter++;

          //GC.Collect();
        }

      using (TextWriter tw = new StreamWriter("RES.txt"))
      {
        for (int i = 0; i < 2; i++)
        {
          tw.Write("(");
          for (int j = 0; j < max_k; j++)
          {
            if (j + 1 == max_k)
              tw.Write(experiments[i, j] + " ");
            else
              tw.Write(experiments[i, j] + ", ");
          }
          tw.Write(")");
          tw.WriteLine();
        }
      }



    }
  }
}
