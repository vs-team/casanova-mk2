#pragma warning disable 162,108
using UnityEngine;
using Casanova.Prelude;
using System.Linq;
using System;
using System.Collections.Generic;
public enum RuleResult { Done, Working }
namespace Casanova.Prelude
{
  public class Tuple<T,E> {
		  public T Item1 { get; set;}
		  public E Item2 { get; set;} 
		  public Tuple(T item1, E item2)
		  {
						  Item1 = item1;
						  Item2 = item2;
			}
	}
  public static class MyExtensions
  {
    //public T this[List<T> list]
    //{
    //  get { return list.ElementAt(0); }
    //}

    public static T Head<T>(this List<T> list) { return list.ElementAt(0); }
    public static T Head<T>(this IEnumerable<T> list) { return list.ElementAt(0); }

    public static int Length<T>(this List<T> list) { return list.Count; }
    public static int Length<T>(this IEnumerable<T> list) { return list.ToList<T>().Count; }
  }
  public class Cons<T> : IEnumerable<T>
  {
    public class Enumerator : IEnumerator<T>
    {
      public Enumerator(Cons<T> parent)
      {
        firstEnumerated = 0;
        this.parent = parent;
        tailEnumerator = parent.tail.GetEnumerator();
      }

      byte firstEnumerated;
      Cons<T> parent;
      IEnumerator<T> tailEnumerator;
      public T Current
      {
        get
        {
          if (firstEnumerated == 0) return default(T);
          if (firstEnumerated == 1) return parent.head;
          else return tailEnumerator.Current;
        }
      }

      object System.Collections.IEnumerator.Current { get { throw new Exception("Empty sequence has no elements"); } }
      public bool MoveNext()
      {
        if (firstEnumerated == 0)
        {
          if (parent.head == null) return false;
          firstEnumerated++;
          return true;
        }
        if (firstEnumerated == 1) firstEnumerated++;
        return tailEnumerator.MoveNext();
      }

      public void Reset() { firstEnumerated = 0; tailEnumerator.Reset(); }
      public void Dispose() { }
    }

    T head;
    IEnumerable<T> tail;
    public Cons(T head, IEnumerable<T> tail)
    {
      this.head = head;
      this.tail = tail;
    }


    public IEnumerator<T> GetEnumerator()
    {
      return new Enumerator(this);
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
      return new Enumerator(this);
    }
  }

  public class Empty<T> : IEnumerable<T>
  {
    public Empty() { }
    public class Enumerator : IEnumerator<T>
    {
      public T Current { get { throw new Exception("Empty sequence has no elements"); } }
      object System.Collections.IEnumerator.Current { get { throw new Exception("Empty sequence has no elements"); } }
      public bool MoveNext() { return false; }
      public void Reset() { }
      public void Dispose() { }
    }

    public IEnumerator<T> GetEnumerator()
    {
      return new Enumerator();
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
      return new Enumerator();
    }
  }

  public abstract class Option<T> : IComparable, IEquatable<Option<T>>
  {
    public bool IsSome;
    public bool IsNone { get { return !IsSome; } }
    protected abstract Just<T> Some { get; }

    public U Match<U>(Func<T,U> f, Func<U> g) {
      if (this.IsSome)
        return f(this.Some.Value);
      else
        return g();
    }

    public bool Equals(Option<T> b)
    {
      return this.Match(
        x => b.Match(
                y => x.Equals(y),
                () => false),
        () => b.Match(
                y => false,
                () => true));
    }

    public override bool Equals(System.Object other)
    {
      if (other == null) return false;
      if (other is Option<T>)
      {
        var other1 = other as Option<T>;
        return this.Equals(other1);
      }
      return false;
    }
    public override int GetHashCode() { return this.GetHashCode(); }


    public static bool operator ==(Option<T> a, Option<T> b)
    {
      if ((System.Object)a == null || (System.Object)b == null) return System.Object.Equals(a, b);
      return a.Equals(b);
    }

    public static bool operator !=(Option<T> a, Option<T> b)
    {
      if ((System.Object)a == null || (System.Object)b == null) return System.Object.Equals(a, b);
      return !(a.Equals(b));
    }
    
    public int CompareTo(object obj)
    {
      if (obj == null) return 1;
      if (obj is Option<T>)
      {
        var obj1 = obj as Option<T>;
        if (this.Equals(obj1)) return 0;
      }
      return -1;
    }

    abstract public T Value { get; }
    public override string ToString()
    {
      return "Option<" + typeof(T).ToString() + ">";
    }
  }

  public class Just<T> : Option<T>
  {
    T elem;
    public Just(T elem) { this.elem = elem; this.IsSome = true; }

    protected override Just<T> Some { get { return this; } }
    public override T Value { get { return elem; } }
  }

  public class Nothing<T> : Option<T>
  {
    public Nothing() { this.IsSome = false; }

    protected override Just<T> Some { get { return null; } }
    public override T Value { get { throw new Exception("Cant get a value from a None object"); } }

  }

  public class Utils
  {
    public static T IfThenElse<T>(Func<bool> c, Func<T> t, Func<T> e)
    {
      if (c())
        return t();
      else
        return e();
    }
  }

}

public class FastStack
{
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

 
 public class World : MonoBehaviour{
void Update () { Update(Time.deltaTime, this); }
public void Start()
	{		UnityBob = UnityBob.Find();
		}
		public UnityBob UnityBob;
	public System.Collections.Generic.List<UnityEngine.Vector3> Checkpoints{  get { return UnityBob.Checkpoints; }
 }
	public UnityEngine.Vector3 Velocity{  get { return UnityBob.Velocity; }
  set{UnityBob.Velocity = value; }
 }
	public UnityEngine.Vector3 Position{  get { return UnityBob.Position; }
 }
	public BobAnimation CurrentAnimation{  set{UnityBob.CurrentAnimation = value; }
 }
	public System.Boolean useGUILayout{  get { return UnityBob.useGUILayout; }
  set{UnityBob.useGUILayout = value; }
 }
	public System.Boolean enabled{  get { return UnityBob.enabled; }
  set{UnityBob.enabled = value; }
 }
	public UnityEngine.Transform transform{  get { return UnityBob.transform; }
 }
	public UnityEngine.Rigidbody rigidbody{  get { return UnityBob.rigidbody; }
 }
	public UnityEngine.Rigidbody2D rigidbody2D{  get { return UnityBob.rigidbody2D; }
 }
	public UnityEngine.Camera camera{  get { return UnityBob.camera; }
 }
	public UnityEngine.Light light{  get { return UnityBob.light; }
 }
	public UnityEngine.Animation animation{  get { return UnityBob.animation; }
 }
	public UnityEngine.ConstantForce constantForce{  get { return UnityBob.constantForce; }
 }
	public UnityEngine.Renderer renderer{  get { return UnityBob.renderer; }
 }
	public UnityEngine.AudioSource audio{  get { return UnityBob.audio; }
 }
	public UnityEngine.GUIText guiText{  get { return UnityBob.guiText; }
 }
	public UnityEngine.NetworkView networkView{  get { return UnityBob.networkView; }
 }
	public UnityEngine.GUITexture guiTexture{  get { return UnityBob.guiTexture; }
 }
	public UnityEngine.Collider collider{  get { return UnityBob.collider; }
 }
	public UnityEngine.Collider2D collider2D{  get { return UnityBob.collider2D; }
 }
	public UnityEngine.HingeJoint hingeJoint{  get { return UnityBob.hingeJoint; }
 }
	public UnityEngine.ParticleEmitter particleEmitter{  get { return UnityBob.particleEmitter; }
 }
	public UnityEngine.ParticleSystem particleSystem{  get { return UnityBob.particleSystem; }
 }
	public UnityEngine.GameObject gameObject{  get { return UnityBob.gameObject; }
 }
	public System.String tag{  get { return UnityBob.tag; }
  set{UnityBob.tag = value; }
 }
	public System.String name{  get { return UnityBob.name; }
  set{UnityBob.name = value; }
 }
	public UnityEngine.HideFlags hideFlags{  get { return UnityBob.hideFlags; }
  set{UnityBob.hideFlags = value; }
 }
	public UnityEngine.Vector3 ___c00;
	public System.Int32 counter1;
	public System.Collections.Generic.List<UnityEngine.Vector3> list0;
	public UnityEngine.Vector3 ___dir000;
	public System.Single count_down1;
	public void Update(float dt, World world) {


		this.Rule0(dt, world);

	}




	int s0=-1;
	public void Rule0(float dt, World world){ 
	switch (s0)
	{

	case -1:
	
	counter1 = -1;
	list0 = Checkpoints;
	if(((list0.Count) == (0)))
	{

	s0 = -1;
return;	}else
	{

	___c00 = list0[0];
	goto case 1;	}
	case 1:
	counter1 = ((counter1) + (1));
	if(((((list0.Count) == (counter1))) || (((counter1) > (list0.Count)))))
	{

	s0 = -1;
return;	}else
	{

	___c00 = list0[counter1];
	goto case 2;	}
	case 2:
	___dir000 = ((___c00) - (Position));
	UnityBob.Velocity = ___dir000;
	UnityBob.CurrentAnimation = BobAnimation.Walk;
	s0 = 6;
return;
	case 6:
	if(!(((0f) > (UnityEngine.Vector3.Dot(___dir000,(___c00) - (Position))))))
	{

	s0 = 6;
return;	}else
	{

	goto case 5;	}
	case 5:
	UnityBob.Velocity = Vector3.zero;
	UnityBob.CurrentAnimation = BobAnimation.Idle;
	s0 = 3;
return;
	case 3:
	count_down1 = 1f;
	goto case 4;
	case 4:
	if(((count_down1) > (0f)))
	{

	count_down1 = ((count_down1) - (dt));
	s0 = 4;
return;	}else
	{

	s0 = 1;
return;	}	
	default: return;}}
	






}
        