using System;
using System.Collections.Generic;
using System.Linq;

namespace Script
{
  public struct Pair<A,B>
  {
    public A a;
    public B b;
  }
  
  public struct Either<A,B>
  {
    public A a;
    public B b;
    public bool has_a;
    public bool has_b;
    public static Either<A,B> mk_left(A a) { return new Either<A, B>() { a = a, has_a = true, has_b = false }; }
    public static Either<A,B> mk_right(B b) { return new Either<A, B>() { b = b, has_a = false, has_b = true }; }
  }
  
  public struct Unit {}
  
  public enum ResultType 
  {
    Result,
    Wait,
    Yield
  }
  
  public class ResultValue<T> {  
    public T result;
    public float wait;
    public ResultType type;
    public static ResultValue<T> new_result(T result) {
      Script.count();
      return new ResultValue<T>()
      {
        result = result,
        wait = 0.0f,
        type = ResultType.Result
      };
    }
    public static ResultValue<T> new_wait(float t) {
      Script.count();
      return new ResultValue<T>()
      {
        result = default(T),
        wait = t,
        type = ResultType.Wait
      };
    }
    public static ResultValue<T> new_yield()
    {
      Script.count();
      return new ResultValue<T>()
      {
        result = default(T),
        wait = 0.0f,
        type = ResultType.Yield
      };          
    }
    public ResultValue<T1> Cast<T1>(Func<T,T1> cast) {
      Script.count();
      return new ResultValue<T1>()
      {
        result = cast(this.result),
        wait = this.wait,
        type = this.type
      };
    }
  }

  public static class Script
  {
    static private int steps = 0;
    static private DateTime start = DateTime.Now;
    static public void reset() {steps = 0; start = DateTime.Now;}
    static public void count() {steps++;}
    static public void summarize() { 
      var t = (DateTime.Now - start).TotalSeconds;
      Console.WriteLine("performed " + steps + " in " + t + " seconds: " + steps / t + " steps/seconds"); 
    }
    
    static public T Run<T>(this IEnumerable<ResultValue<T>> p)
    {
      reset();
      var result = default(T);
      foreach(var x in p)
        if(x.type == ResultType.Result)
        {
          result = x.result;
          break;
        } else if (x.type == ResultType.Wait)
        {
          var start = DateTime.Now;
          while(true)
          {
            var now = DateTime.Now;
            if((now - start).TotalSeconds >= x.wait)
              break;
          }
        }
      summarize();
      return result;
    }
  
    static public IEnumerable<ResultValue<Pair<T1,T2>>> parallel_<T1, T2>(this IEnumerable<ResultValue<T1>> p1, IEnumerable<ResultValue<T2>> p2)
    {
        var e1 = p1.GetEnumerator();
        var e2 = p2.GetEnumerator();

        var result1 = default(T1);
        var done_e1 = false;
        var waiting_e1 = 0.0f;
        var done_e2 = false;
        var waiting_e2 = 0.0f;
        var result2 = default(T2);
        var start = DateTime.Now;
        while (!done_e1 || !done_e2)
        {
          if(waiting_e1 <= 0.0f && done_e1 == false)
          {
            if(e1.MoveNext())
            {
              var r1 = e1.Current;
              if(r1.type == ResultType.Wait)
                  waiting_e1 = r1.wait;
              else if (r1.type == ResultType.Result)
              {
                  done_e1 = true;
                  result1 = r1.result;
              }
            } else 
              done_e1 = true;
          }
        
          if(waiting_e2 <= 0.0f && done_e2 == false)
          {
            if(e2.MoveNext())
            {
              var r2 = e2.Current;
              if(r2.type == ResultType.Wait)
                  waiting_e2 = r2.wait;
              else if (r2.type == ResultType.Result)
              {
                  done_e2 = true;
                  result2 = r2.result;
              }
            } else 
              done_e2 = true;
          }
                  
          var now = DateTime.Now;
          var dt = (now - start).TotalSeconds;
          start = now;
          waiting_e1 -= (float)dt;
          waiting_e2 -= (float)dt;
          
          yield return ResultValue<Pair<T1,T2>>.new_yield();
        }
        yield return ResultValue<Pair<T1,T2>>.new_result(new Pair<T1,T2>(){ a = result1, b = result2 });
    }

    static public IEnumerable<ResultValue<Either<T1,T2>>> parallel_first_<T1, T2>(this IEnumerable<ResultValue<T1>> p1, IEnumerable<ResultValue<T2>> p2)
    {
        var e1 = p1.GetEnumerator();
        var e2 = p2.GetEnumerator();

        var result1 = default(T1);
        var done_e1 = false;
        var waiting_e1 = 0.0f;
        var done_e2 = false;
        var waiting_e2 = 0.0f;
        var result2 = default(T2);
        var start = DateTime.Now;
        while (!done_e1 && !done_e2)
        {
          if(waiting_e1 <= 0.0f && done_e1 == false)
          {
            if(e1.MoveNext())
            {
              var r1 = e1.Current;
              if(r1.type == ResultType.Wait)
                  waiting_e1 = r1.wait;
              else if (r1.type == ResultType.Result)
              {
                  done_e1 = true;
                  result1 = r1.result;
              }
            } else 
              done_e1 = true;
          }
          yield return ResultValue<Either<T1,T2>>.new_yield();        
        
          if(waiting_e2 <= 0.0f && done_e2 == false)
          {
            if(e2.MoveNext())
            {
              var r2 = e2.Current;
              if(r2.type == ResultType.Wait)
                  waiting_e2 = r2.wait;
              else if (r2.type == ResultType.Result)
              {
                  done_e2 = true;
                  result2 = r2.result;
              }
            } else 
              done_e2 = true;
          }
          yield return ResultValue<Either<T1,T2>>.new_yield();
        
          var now = DateTime.Now;
          var dt = (now - start).TotalSeconds;
          start = now;
          waiting_e1 -= (float)dt;
          waiting_e2 -= (float)dt;
          
        }
        if(done_e1)
          yield return ResultValue<Either<T1,T2>>.new_result(Either<T1,T2>.mk_left(result1));
        else if(done_e2)
          yield return ResultValue<Either<T1,T2>>.new_result(Either<T1,T2>.mk_right(result2));
    }

    static public IEnumerable<ResultValue<Unit>> ignore_<T>(this IEnumerable<ResultValue<T>> p)
    {
      foreach (ResultValue<T> x in p) {
        yield return x.Cast<Unit>(v => new Unit());
      }       
    }

    static private IEnumerable<ResultValue<Unit>> parallel_many_<T>(this IEnumerable<ResultValue<T>>[] ps, int min)
    {
      if(min >= ps.Length)
        return new ResultValue<Unit>[]{};
      else
        return ignore_(parallel_(ps[min], parallel_many_(ps, min+1)));
    }

    static public IEnumerable<ResultValue<Unit>> parallel_many_<T>(this IEnumerable<ResultValue<T>>[] ps)
    {
      return parallel_many_(ps,0);
    }
  }
}