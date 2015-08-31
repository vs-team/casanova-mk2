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
	{		List<Planet> ___planets00;
		___planets00 = (

(Enumerable.Range(1,50).ToList<System.Int32>()).Select(__ContextSymbol0 => new { ___i00 = __ContextSymbol0 })
.Select(__ContextSymbol1 => new {___pos00 = new UnityEngine.Vector3(UnityEngine.Random.Range(-140f,140f),0f,UnityEngine.Random.Range(-140f,140f)), prev = __ContextSymbol1 })
.Select(__ContextSymbol2 => new {___mass02 = UnityEngine.Random.Range(3f,350f), prev = __ContextSymbol2 })
.Select(__ContextSymbol3 => new {___rotv02 = new UnityEngine.Vector3(UnityEngine.Random.Range(-50f,50f),UnityEngine.Random.Range(-50f,50f),UnityEngine.Random.Range(-50f,50f)), prev = __ContextSymbol3 })
.Select(__ContextSymbol4 => new Planet(__ContextSymbol4.prev.prev.___pos00,__ContextSymbol4.prev.___mass02,Vector3.zero,__ContextSymbol4.___rotv02))
.ToList<Planet>()).ToList<Planet>();
		MainCamera = new GameCamera();
		Planets = ___planets00;
		}
		public List<Planet> Planets;
	public GameCamera MainCamera;
	public System.Single ___posx10;
	public System.Single ___posz10;
	public System.Single ___posx11;
	public System.Single ___posz11;
	public System.Single ___mass10;
	public UnityEngine.Vector3 ___position10;
	public UnityEngine.Vector3 ___velocity10;
	public UnityEngine.Vector3 ___rotv10;
	public Planet ___newPlanet10;
	public System.Single count_down1;
	public System.Single ___posx12;
	public System.Single ___posz12;
	public System.Single ___posx13;
	public System.Single ___posz13;
	public System.Single ___mass11;
	public UnityEngine.Vector3 ___position11;
	public UnityEngine.Vector3 ___velocity11;
	public UnityEngine.Vector3 ___rotv11;
	public Planet ___newPlanet11;
	public void Update(float dt, World world) {
		for(int x0 = 0; x0 < Planets.Count; x0++) { 
			Planets[x0].Update(dt, world);
		}
		MainCamera.Update(dt, world);
		this.Rule0(dt, world);

		this.Rule1(dt, world);

	}
	public void Rule0(float dt, World world) 
	{
	Planets = (

(Planets).Select(__ContextSymbol5 => new { ___p00 = __ContextSymbol5 })
.Where(__ContextSymbol6 => !(__ContextSymbol6.___p00.OutOfBounds))
.Select(__ContextSymbol7 => __ContextSymbol7.___p00)
.ToList<Planet>()).ToList<Planet>();
	}
	




	int s1=-1;
	public void Rule1(float dt, World world){ 
	switch (s1)
	{

	case -1:
	if(!(((((20) > (Planets.Count))) || (true))))
	{

	s1 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	if(((20) > (Planets.Count)))
	{

	goto case 2;	}else
	{

	if(true)
	{

	goto case 3;	}else
	{

	s1 = 0;
return;	}	}
	case 2:
	___posx10 = UnityEngine.Random.Range(175f,200f);
	___posz10 = UnityEngine.Random.Range(175f,200f);
	if(((UnityEngine.Random.value) > (0.5f)))
	{

	___posx11 = ___posx10;	}else
	{

	___posx11 = ((___posx10) * (-1f));	}
	if(((UnityEngine.Random.value) > (0.5f)))
	{

	___posz11 = ___posz10;	}else
	{

	___posz11 = ((___posz10) * (-1f));	}
	___mass10 = UnityEngine.Random.Range(3f,350f);
	___position10 = new UnityEngine.Vector3(___posx11,0f,___posz11);
	___velocity10 = ((UnityEngine.Vector3.Normalize((___position10) * (-1f))) * (UnityEngine.Random.Range(1f,6f)));
	___rotv10 = new UnityEngine.Vector3(UnityEngine.Random.Range(-50f,50f),UnityEngine.Random.Range(-50f,50f),UnityEngine.Random.Range(-50f,50f));
	___newPlanet10 = new Planet(___position10,___mass10,___velocity10,___rotv10);
	Planets = new Cons<Planet>(___newPlanet10, (Planets)).ToList<Planet>();
	s1 = -1;
return;
	case 3:
	count_down1 = 1f;
	goto case 25;
	case 25:
	if(((count_down1) > (0f)))
	{

	count_down1 = ((count_down1) - (dt));
	s1 = 25;
return;	}else
	{

	goto case 23;	}
	case 23:
	___posx12 = UnityEngine.Random.Range(175f,200f);
	___posz12 = UnityEngine.Random.Range(175f,200f);
	if(((UnityEngine.Random.value) > (0.5f)))
	{

	___posx13 = ___posx12;	}else
	{

	___posx13 = ((___posx12) * (-1f));	}
	if(((UnityEngine.Random.value) > (0.5f)))
	{

	___posz13 = ___posz12;	}else
	{

	___posz13 = ((___posz12) * (-1f));	}
	___mass11 = UnityEngine.Random.Range(3f,350f);
	___position11 = new UnityEngine.Vector3(___posx13,0f,___posz13);
	___velocity11 = ((UnityEngine.Vector3.Normalize((___position11) * (-1f))) * (UnityEngine.Random.Range(1f,6f)));
	___rotv11 = new UnityEngine.Vector3(UnityEngine.Random.Range(-50f,50f),UnityEngine.Random.Range(-50f,50f),UnityEngine.Random.Range(-50f,50f));
	___newPlanet11 = new Planet(___position11,___mass11,___velocity11,___rotv11);
	Planets = new Cons<Planet>(___newPlanet11, (Planets)).ToList<Planet>();
	s1 = -1;
return;	
	default: return;}}
	






}
public class GameCamera{
	public int ID;
public GameCamera()
	{		UnityCamera = UnityCamera.CreateMainCamera();
		}
		public UnityCamera UnityCamera;
	public UnityEngine.Vector3 CameraPosition{  get { return UnityCamera.CameraPosition; }
  set{UnityCamera.CameraPosition = value; }
 }
	public System.Single CameraSize{  get { return UnityCamera.CameraSize; }
  set{UnityCamera.CameraSize = value; }
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
	public System.Single ___adjustment10;
	public System.Single ___adjustment11;
	public System.Single ___adjustment12;
	public System.Single ___adjustment13;
	public System.Single ___sensitivity00;
	public System.Single ___sensitivity11;
	public void Update(float dt, World world) {


		this.Rule0(dt, world);
		this.Rule1(dt, world);
	}



	int s000=-1;
	public void parallelMethod000(float dt, World world){ 
	switch (s000)
	{

	case -1:
	if(!(((UnityEngine.Input.GetKey(KeyCode.DownArrow)) && (!(((CameraSize) > (75f)))))))
	{

	s000 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	UnityCamera.CameraSize = System.Math.Min(75f,(CameraSize) + (___sensitivity00));
	s000 = -1;
return;	
	default: return;}}
	

	int s001=-1;
	public void parallelMethod001(float dt, World world){ 
	switch (s001)
	{

	case -1:
	if(!(((UnityEngine.Input.GetKey(KeyCode.UpArrow)) && (((((CameraSize) > (5f))) || (((CameraSize) == (5f))))))))
	{

	s001 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	UnityCamera.CameraSize = System.Math.Max(5f,(CameraSize) - (___sensitivity00));
	s001 = -1;
return;	
	default: return;}}
	

	int s110=-1;
	public void parallelMethod110(float dt, World world){ 
	switch (s110)
	{

	case -1:
	if(!(((UnityEngine.Input.GetKey(KeyCode.A)) && (((CameraPosition.x) > (-100f))))))
	{

	s110 = -1;
return;	}else
	{

	goto case 1;	}
	case 1:
	___adjustment10 = System.Math.Max(-100f,(___sensitivity11) * (-1f));
	UnityCamera.CameraPosition = ((CameraPosition) + (new UnityEngine.Vector3(___adjustment10,0f,0f)));
	s110 = -1;
return;	
	default: return;}}
	

	int s111=-1;
	public void parallelMethod111(float dt, World world){ 
	switch (s111)
	{

	case -1:
	if(!(((UnityEngine.Input.GetKey(KeyCode.D)) && (((100f) > (CameraPosition.x))))))
	{

	s111 = -1;
return;	}else
	{

	goto case 1;	}
	case 1:
	___adjustment11 = System.Math.Min(100f,___sensitivity11);
	UnityCamera.CameraPosition = ((CameraPosition) + (new UnityEngine.Vector3(___sensitivity11,0f,0f)));
	s111 = -1;
return;	
	default: return;}}
	

	int s112=-1;
	public void parallelMethod112(float dt, World world){ 
	switch (s112)
	{

	case -1:
	if(!(((UnityEngine.Input.GetKey(KeyCode.S)) && (((CameraPosition.z) > (-100f))))))
	{

	s112 = -1;
return;	}else
	{

	goto case 1;	}
	case 1:
	___adjustment12 = System.Math.Max(-100f,(___sensitivity11) * (-1f));
	UnityCamera.CameraPosition = ((CameraPosition) + (new UnityEngine.Vector3(0f,0f,___adjustment12)));
	s112 = -1;
return;	
	default: return;}}
	

	int s113=-1;
	public void parallelMethod113(float dt, World world){ 
	switch (s113)
	{

	case -1:
	if(!(((UnityEngine.Input.GetKey(KeyCode.W)) && (((100f) > (CameraPosition.z))))))
	{

	s113 = -1;
return;	}else
	{

	goto case 1;	}
	case 1:
	___adjustment13 = System.Math.Min(100f,___sensitivity11);
	UnityCamera.CameraPosition = ((CameraPosition) + (new UnityEngine.Vector3(0f,0f,___adjustment13)));
	s113 = -1;
return;	
	default: return;}}
	

	int s0=-1;
	public void Rule0(float dt, World world){ 
	switch (s0)
	{

	case -1:
	___sensitivity00 = 0.5f;
	goto case 0;
	case 0:
	this.parallelMethod000(dt,world);
	this.parallelMethod001(dt,world);
	s0 = 0;
return;	
	default: return;}}
	

	int s1=-1;
	public void Rule1(float dt, World world){ 
	switch (s1)
	{

	case -1:
	___sensitivity11 = 1f;
	goto case 0;
	case 0:
	this.parallelMethod110(dt,world);
	this.parallelMethod111(dt,world);
	this.parallelMethod112(dt,world);
	this.parallelMethod113(dt,world);
	s1 = 0;
return;	
	default: return;}}
	





}
public class Planet{
private UnityEngine.Vector3 pos;
private System.Single m;
private UnityEngine.Vector3 velocity;
private UnityEngine.Vector3 rotationVelocity;
	public int ID;
public Planet(UnityEngine.Vector3 pos, System.Single m, UnityEngine.Vector3 velocity, UnityEngine.Vector3 rotationVelocity)
	{		UnityPlanet = UnityPlanet.Instantiate(pos,m);
		OutOfBounds = false;
		Acceleration = Vector3.zero;
		Mass = m;
		RotationVelocity = rotationVelocity;
		Velocity = velocity;
		}
		public UnityPlanet UnityPlanet;
	public UnityEngine.Vector3 Position{  get { return UnityPlanet.Position; }
  set{UnityPlanet.Position = value; }
 }
	public UnityEngine.Quaternion Rotation{  get { return UnityPlanet.Rotation; }
  set{UnityPlanet.Rotation = value; }
 }
	public System.Boolean Destroyed{  get { return UnityPlanet.Destroyed; }
  set{UnityPlanet.Destroyed = value; }
 }
	public System.Boolean useGUILayout{  get { return UnityPlanet.useGUILayout; }
  set{UnityPlanet.useGUILayout = value; }
 }
	public System.Boolean enabled{  get { return UnityPlanet.enabled; }
  set{UnityPlanet.enabled = value; }
 }
	public UnityEngine.Transform transform{  get { return UnityPlanet.transform; }
 }
	public UnityEngine.Rigidbody rigidbody{  get { return UnityPlanet.rigidbody; }
 }
	public UnityEngine.Rigidbody2D rigidbody2D{  get { return UnityPlanet.rigidbody2D; }
 }
	public UnityEngine.Camera camera{  get { return UnityPlanet.camera; }
 }
	public UnityEngine.Light light{  get { return UnityPlanet.light; }
 }
	public UnityEngine.Animation animation{  get { return UnityPlanet.animation; }
 }
	public UnityEngine.ConstantForce constantForce{  get { return UnityPlanet.constantForce; }
 }
	public UnityEngine.Renderer renderer{  get { return UnityPlanet.renderer; }
 }
	public UnityEngine.AudioSource audio{  get { return UnityPlanet.audio; }
 }
	public UnityEngine.GUIText guiText{  get { return UnityPlanet.guiText; }
 }
	public UnityEngine.NetworkView networkView{  get { return UnityPlanet.networkView; }
 }
	public UnityEngine.GUITexture guiTexture{  get { return UnityPlanet.guiTexture; }
 }
	public UnityEngine.Collider collider{  get { return UnityPlanet.collider; }
 }
	public UnityEngine.Collider2D collider2D{  get { return UnityPlanet.collider2D; }
 }
	public UnityEngine.HingeJoint hingeJoint{  get { return UnityPlanet.hingeJoint; }
 }
	public UnityEngine.ParticleEmitter particleEmitter{  get { return UnityPlanet.particleEmitter; }
 }
	public UnityEngine.ParticleSystem particleSystem{  get { return UnityPlanet.particleSystem; }
 }
	public UnityEngine.GameObject gameObject{  get { return UnityPlanet.gameObject; }
 }
	public System.String tag{  get { return UnityPlanet.tag; }
  set{UnityPlanet.tag = value; }
 }
	public System.String name{  get { return UnityPlanet.name; }
  set{UnityPlanet.name = value; }
 }
	public UnityEngine.HideFlags hideFlags{  get { return UnityPlanet.hideFlags; }
  set{UnityPlanet.hideFlags = value; }
 }
	public UnityEngine.Vector3 Velocity;
	public UnityEngine.Vector3 RotationVelocity;
	public UnityEngine.Vector3 Acceleration;
	public System.Single Mass;
	public System.Boolean OutOfBounds;
	public void Update(float dt, World world) {
		this.Rule0(dt, world);
		this.Rule2(dt, world);
		this.Rule3(dt, world);
		this.Rule4(dt, world);
		this.Rule5(dt, world);
		this.Rule1(dt, world);

	}

	public void Rule0(float dt, World world) 
	{
	System.Single ___g00;
	___g00 = (6.673f) * (UnityEngine.Mathf.Pow(10f,-3f));
	List<UnityEngine.Vector3> ___accelerations00;
	___accelerations00 = (

(world.Planets).Select(__ContextSymbol8 => new { ___planet00 = __ContextSymbol8 })
.Where(__ContextSymbol9 => !(((__ContextSymbol9.___planet00) == (this))))
.Select(__ContextSymbol10 => new {___r00 = UnityEngine.Vector3.Distance(this.Position,__ContextSymbol10.___planet00.Position), prev = __ContextSymbol10 })
.Select(__ContextSymbol11 => new {___acc00 = ((___g00) * (__ContextSymbol11.prev.___planet00.Mass)) / ((__ContextSymbol11.___r00) * (__ContextSymbol11.___r00)), prev = __ContextSymbol11 })
.Select(__ContextSymbol12 => (UnityEngine.Vector3.Normalize((__ContextSymbol12.prev.prev.___planet00.Position) - (this.Position))) * (__ContextSymbol12.___acc00))
.ToList<UnityEngine.Vector3>()).ToList<UnityEngine.Vector3>();
	if(((___accelerations00.Count) > (0)))
		{
		Acceleration = (

(___accelerations00).Select(__ContextSymbol13 => new { ___a00 = __ContextSymbol13 })
.Select(__ContextSymbol14 => __ContextSymbol14.___a00)
.Aggregate( (acc, __x) => acc + __x));
		}else
		{
		Acceleration = Vector3.zero;
		}
	}
	

	public void Rule2(float dt, World world) 
	{
	OutOfBounds = ((((((((Position.x) > (250f))) || (((Position.z) > (250f))))) || (((-250f) > (Position.x))))) || (((-250f) > (Position.z))));
	}
	

	public void Rule3(float dt, World world) 
	{
	UnityPlanet.Rotation = (UnityEngine.Quaternion.Euler((RotationVelocity) * (dt))) * (Rotation);
	}
	

	public void Rule4(float dt, World world) 
	{
	Velocity = (Velocity) + ((Acceleration) * (dt));
	}
	

	public void Rule5(float dt, World world) 
	{
	UnityPlanet.Position = (Position) + ((Velocity) * (dt));
	}
	



	int s1=-1;
	public void Rule1(float dt, World world){ 
	switch (s1)
	{

	case -1:
	if(!(OutOfBounds))
	{

	s1 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	UnityPlanet.Destroyed = true;
	s1 = -1;
return;	
	default: return;}}
	






}
   