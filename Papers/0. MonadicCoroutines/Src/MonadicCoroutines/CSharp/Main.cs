using System;
using System.Collections.Generic;
using System.Linq;
using Script;

namespace CSharp
{
    class MainClass
    {
        static IEnumerable<ResultValue<int>> fibonacci(int n)
        {
            if (n == 0)
            {
                yield return ResultValue<int>.new_result(0);
            }
            else
            {
                if (n == 1)
                {
                    yield return ResultValue<int>.new_result(1);
                }
                else
                {
                    var n1 = 0;
                    yield return ResultValue<int>.new_yield();
                    foreach (var x in fibonacci(n - 1))
                    {
                        if (x.type == ResultType.Result)
                        {
                            n1 = x.result;
                            break;
                        }
                        yield return x.Cast<int>(i => i);
                    }

                    var n2 = 0;
                    yield return ResultValue<int>.new_yield();
                    foreach (var x in fibonacci(n - 2))
                    {
                        if (x.type == ResultType.Result)
                        {
                            n2 = x.result;
                            break;
                        }
                        yield return x.Cast<int>(i => i);
                    }
                    yield return ResultValue<int>.new_result(n1 + n2);
                }
            }
        }

        static IEnumerable<ResultValue<Unit>> log(int i)
        {
            //Console.WriteLine("log " + i);
            yield return ResultValue<Unit>.new_wait(2.0f);
            foreach (var x in log(i + 1))
            {
                yield return x;
            }
        }

        public static void FibonacciTest()
        {
            fibonacci(25).parallel_first_(log(0)).ignore_().Run();
        }

        private static IEnumerable<IEnumerable<ResultValue<Unit>>> many_fibonaccis(int n)
        {
            //yield return log(0);
            for (int i = 0; i <= n; i++)
            {
                yield return fibonacci(i + 5).ignore_();
            }
        }

        public static void ManyFibonacciTest()
        {
            many_fibonaccis(15).ToArray().parallel_many_().ignore_().Run();
            //Console.WriteLine(res.ToString() + "\n");      
        }


        public class Entity { public float Position; }
        public class State { public List<Entity> Entities = new List<Entity>(); }

        public static IEnumerable<ResultValue<T>> AddShip<T>(State state, Entity ship, Func<Entity, IEnumerable<ResultValue<T>>> run_ship)
        {
            var res = default(T);
            state.Entities.Add(ship);
            foreach (var x in run_ship(ship))
            {
                if (x.type == ResultType.Result)
                {
                    res = x.result;
                    break;
                }
                yield return x.Cast<T>(i => i);
            }
            state.Entities.Remove(ship);
            yield return ResultValue<T>.new_result(res);
        }

        public static IEnumerable<ResultValue<Unit>> SimpleShip(Entity self)
        {
            while (self.Position > 0.0f)
            {
                self.Position -= 0.1f;
                //yield return ResultValue<Unit>.new_wait(0.1f);
                yield return ResultValue<Unit>.new_yield();
            }
        }

        public static IEnumerable<ResultValue<Unit>> ManyShips(State state, int n)
        {
            if (n > 0)
            {
                foreach (var x in AddShip<Unit>(state, new Entity() { Position = 100.0f - n }, SimpleShip).parallel_(ManyShips(state, n - 1)).ignore_())
                    yield return x.Cast<Unit>(i => i);
            }
        }

        public static IEnumerable<ResultValue<Unit>> LogShips(State state)
        {
            //Console.WriteLine("there are " + state.Entities.Count + " ships...");
            yield return ResultValue<Unit>.new_wait(2.0f);
            foreach (ResultValue<Unit> x in LogShips(state))
                yield return x.Cast<Unit>(i => i);
        }

        public static void StateTest()
        {
            var state = new State();
            ManyShips(state, 200).parallel_first_(LogShips(state)).ignore_().Run();
        }

        public static void Main(string[] args)
        {
            FibonacciTest();
            ManyFibonacciTest();
            StateTest();
        }
    }
}

