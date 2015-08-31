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
	{		MainCamera = new MainCamera();
		}
		public MainCamera MainCamera;
	public void Update(float dt, World world) {
		MainCamera.Update(dt, world);




	}










}
public class MainCamera{
	public int ID;
public MainCamera()
	{		UnityCamera = UnityCamera.Find();
		}
		public UnityCamera UnityCamera;
	public UnityEngine.Quaternion Rotation{  get { return UnityCamera.Rotation; }
  set{UnityCamera.Rotation = value; }
 }
	public UnityEngine.Vector3 Acceleration{  set{UnityCamera.Acceleration = value; }
 }
	public UnityEngine.Vector3 Forward{  get { return UnityCamera.Forward; }
  set{UnityCamera.Forward = value; }
 }
	public UnityEngine.Vector3 Right{  get { return UnityCamera.Right; }
  set{UnityCamera.Right = value; }
 }
	public System.Boolean useGUILayout{  get { return UnityCamera.useGUILayout; }
  set{UnityCamera.useGUILayout = value; }
 }
	public System.Boolean enabled{  get { return UnityCamera.enabled; }
  set{UnityCamera.enabled = value; }
 }
	public UnityEngine.Transform transform{  get { return UnityCamera.transform; }
 }
	public UnityEngine.Rigidbody rigidbody{  get { return UnityCamera.rigidbody; }
 }
	public UnityEngine.Rigidbody2D rigidbody2D{  get { return UnityCamera.rigidbody2D; }
 }
	public UnityEngine.Camera camera{  get { return UnityCamera.camera; }
 }
	public UnityEngine.Light light{  get { return UnityCamera.light; }
 }
	public UnityEngine.Animation animation{  get { return UnityCamera.animation; }
 }
	public UnityEngine.ConstantForce constantForce{  get { return UnityCamera.constantForce; }
 }
	public UnityEngine.Renderer renderer{  get { return UnityCamera.renderer; }
 }
	public UnityEngine.AudioSource audio{  get { return UnityCamera.audio; }
 }
	public UnityEngine.GUIText guiText{  get { return UnityCamera.guiText; }
 }
	public UnityEngine.NetworkView networkView{  get { return UnityCamera.networkView; }
 }
	public UnityEngine.GUITexture guiTexture{  get { return UnityCamera.guiTexture; }
 }
	public UnityEngine.Collider collider{  get { return UnityCamera.collider; }
 }
	public UnityEngine.Collider2D collider2D{  get { return UnityCamera.collider2D; }
 }
	public UnityEngine.HingeJoint hingeJoint{  get { return UnityCamera.hingeJoint; }
 }
	public UnityEngine.ParticleEmitter particleEmitter{  get { return UnityCamera.particleEmitter; }
 }
	public UnityEngine.ParticleSystem particleSystem{  get { return UnityCamera.particleSystem; }
 }
	public UnityEngine.GameObject gameObject{  get { return UnityCamera.gameObject; }
 }
	public System.String tag{  get { return UnityCamera.tag; }
  set{UnityCamera.tag = value; }
 }
	public System.String name{  get { return UnityCamera.name; }
  set{UnityCamera.name = value; }
 }
	public UnityEngine.HideFlags hideFlags{  get { return UnityCamera.hideFlags; }
  set{UnityCamera.hideFlags = value; }
 }
	public System.Single ___s00;
	public void Update(float dt, World world) {
		this.Rule1(dt, world);

		this.Rule0(dt, world);

	}

	public void Rule1(float dt, World world) 
	{
	UnityCamera.Rotation = ((UnityEngine.Quaternion.Euler(0f,(UnityEngine.Input.GetAxis("Mouse X")) * (4f),0f)) * (UnityCamera.Rotation)) * (UnityEngine.Quaternion.Euler((UnityEngine.Input.GetAxis("Mouse Y")) * (-4f),0f,0f));
	}
	


	int s000=-1;
	public void parallelMethod000(float dt, World world){ 
	switch (s000)
	{

	case -1:
	if(!(UnityEngine.Input.GetKey(KeyCode.W)))
	{

	s000 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	UnityCamera.Acceleration = ((Forward) * (___s00));
	s000 = -1;
return;	
	default: return;}}
	

	int s001=-1;
	public void parallelMethod001(float dt, World world){ 
	switch (s001)
	{

	case -1:
	if(!(UnityEngine.Input.GetKey(KeyCode.S)))
	{

	s001 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	UnityCamera.Acceleration = ((((Forward) * (-1f))) * (___s00));
	s001 = -1;
return;	
	default: return;}}
	

	int s002=-1;
	public void parallelMethod002(float dt, World world){ 
	switch (s002)
	{

	case -1:
	if(!(UnityEngine.Input.GetKey(KeyCode.D)))
	{

	s002 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	UnityCamera.Acceleration = ((Right) * (___s00));
	s002 = -1;
return;	
	default: return;}}
	

	int s003=-1;
	public void parallelMethod003(float dt, World world){ 
	switch (s003)
	{

	case -1:
	if(!(UnityEngine.Input.GetKey(KeyCode.A)))
	{

	s003 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	UnityCamera.Acceleration = ((((Right) * (-1f))) * (___s00));
	s003 = -1;
return;	
	default: return;}}
	

	int s0=-1;
	public void Rule0(float dt, World world){ 
	switch (s0)
	{

	case -1:
	___s00 = 750f;
	goto case 0;
	case 0:
	this.parallelMethod000(dt,world);
	this.parallelMethod001(dt,world);
	this.parallelMethod002(dt,world);
	this.parallelMethod003(dt,world);
	s0 = 0;
return;	
	default: return;}}
	






}
                                                     