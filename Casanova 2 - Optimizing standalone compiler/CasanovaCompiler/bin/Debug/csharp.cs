using System.Linq;
using System;
using System.Collections.Generic;
public enum RuleResult { Done, Working }


public class FastStack {
      public int[] Elements;
      public int Top;

      public FastStack(int elems){
        Elements = new int[elems];
      }

      public void Clear() { Top = 0; }
      public void Push(int x) { Elements[Top++] = x; }
    }

public class RuleTable {
      public RuleTable(int elems){
        Active = new Func<float, RuleResult>[elems];
        ActiveIndices = new FastStack(elems);
        SupportStack = new FastStack(elems);
        ActiveSlots = new bool[elems];
      }

      Func<float, RuleResult>[] Active;
      FastStack ActiveIndices;
      FastStack SupportStack;
      bool[] ActiveSlots;

      public void Add(Func<float, RuleResult> r, int i) {
        if(!ActiveSlots[i]) {
          Active[i] = r;
          ActiveSlots[i] = true;
          ActiveIndices.Push(i);
        }
      }

      public void StepAll(float dt) {
        for (int i = 0; i < ActiveIndices.Top; i++ ){
          if(Active[i](dt) == RuleResult.Done)
            ActiveSlots[i] = false;
          else
            SupportStack.Push(i);
        }
        ActiveIndices.Clear();
        var tmp = SupportStack;
        SupportStack = ActiveIndices;
        ActiveIndices = tmp;
      }
    }

 
 class World{
	public World()
	
		{
		List<Ball> ___balls00;
		___balls00 = (
			from ___i00 in Enumerable.Range(1,1000000)
			select new Ball()).ToList();
		Balls = ___balls00;
				}
		public List<Ball> Balls;
	public void Update(float dt) {
		for(int x0 = 0; x0 < Balls.Count; x0++) { 
			Balls[x0].Update(dt);
		}


	}






}
class Ball{
	public Ball()
	
		{
		V = (0.1f);
		P = (0f);
				}
		public System.Single _P;
	public System.Single V;
	public System.Single P{ 
		get { return _P; } 
		set { 
			_P = value;
		}
	}
	public void Update(float dt) {
this.Rule0(dt);
		this.Rule1(dt);

	}
	public void Rule1(float dt) 
	{
	P = ((P)) + ((V));
	}
	




	int s0=-1;
	public RuleResult Rule0(float dt){ 
	switch (s0)
	{

	case -1:
	if(!((((P)) > ((500f)))))
	{

	s0 = -1;
return RuleResult.Done;	}
	{

	goto case 2;	}
	case 2:
	V = ((V)) * ((-1f));
	s0 = 1;
return RuleResult.Working;
	case 1:
	if(!((((-500f)) > ((P)))))
	{

	s0 = 1;
return RuleResult.Working;	}
	{

	goto case 0;	}
	case 0:
	V = ((V)) * ((-1f));
	s0 = -1;
return RuleResult.Working;	
	default: return RuleResult.Done;}}
	


}
