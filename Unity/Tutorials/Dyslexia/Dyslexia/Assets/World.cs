#pragma warning disable 162,108,618
using Casanova.Prelude;
using System.Linq;
using System;
using System.Collections.Generic;
using Track;
using UnityEngine;
public class World : MonoBehaviour{
public static int frame;
void Update () { Update(Time.deltaTime, this); 
 frame++; }
public bool JustEntered = true;

public void Start()
	{		UnityAudio = UnityAudio.Find();
		StopGame = false;
		RunningTutorial = true;
		PauseMenu = (new Nothing<PauseMenu>());
		PauseButton = new PauseButton();
		Fox = new Animal("Fox","fox_walking",4,"fox_singing",15,new UnityEngine.Vector3(-0.4f,-7.9f,-0.7f),new UnityEngine.Vector3(-0.4f,-7.9f,-0.7f),new UnityEngine.Vector3(-0.4f,-7.9f,-0.7f));
		Foreground = UnityBackground.Find("GroundMid",0f);
		CurrentTrack = (new Nothing<Track.Item>());
		CurrentCheckpoint = (new Nothing<UnityEngine.Transform>());
		Button = new Button();
		Bird = new Animal("Bird","bird_flying",4,"bird_singing",12,new UnityEngine.Vector3(3f,-7.9f,2f),new UnityEngine.Vector3(-3f,-7.9f,2f),new UnityEngine.Vector3(3f,-7.9f,2f));
		Background = UnityBackground.Find("GroundBack",0f);
		AnimationEnd = false;
		
}
		public System.Boolean AnimationEnd;
	public UnityBackground Background;
	public Animal Bird;
	public Button Button;
	public System.Single ClipLength{  get { return UnityAudio.ClipLength; }
  set{UnityAudio.ClipLength = value; }
 }
	public Option<UnityEngine.Transform> CurrentCheckpoint;
	public Option<Track.Item> CurrentTrack;
	public UnityBackground Foreground;
	public Animal Fox;
	public PauseButton __PauseButton;
	public PauseButton PauseButton{  get { return  __PauseButton; }
  set{ __PauseButton = value;
 if(!value.JustEntered) __PauseButton = value; 
 else{ value.JustEntered = false;
}
 }
 }
	public Option<PauseMenu> PauseMenu;
	public System.Boolean RunningTutorial;
	public System.Boolean StopGame;
	public UnityAudio UnityAudio;
	public UnityEngine.Animation animation{  get { return UnityAudio.animation; }
 }
	public UnityEngine.AudioSource audio{  get { return UnityAudio.audio; }
 }
	public UnityEngine.Camera camera{  get { return UnityAudio.camera; }
 }
	public UnityEngine.Collider collider{  get { return UnityAudio.collider; }
 }
	public UnityEngine.Collider2D collider2D{  get { return UnityAudio.collider2D; }
 }
	public UnityEngine.ConstantForce constantForce{  get { return UnityAudio.constantForce; }
 }
	public System.Boolean enabled{  get { return UnityAudio.enabled; }
  set{UnityAudio.enabled = value; }
 }
	public UnityEngine.GameObject gameObject{  get { return UnityAudio.gameObject; }
 }
	public UnityEngine.GUIElement guiElement{  get { return UnityAudio.guiElement; }
 }
	public UnityEngine.GUIText guiText{  get { return UnityAudio.guiText; }
 }
	public UnityEngine.GUITexture guiTexture{  get { return UnityAudio.guiTexture; }
 }
	public UnityEngine.HideFlags hideFlags{  get { return UnityAudio.hideFlags; }
  set{UnityAudio.hideFlags = value; }
 }
	public UnityEngine.HingeJoint hingeJoint{  get { return UnityAudio.hingeJoint; }
 }
	public UnityEngine.Light light{  get { return UnityAudio.light; }
 }
	public System.String name{  get { return UnityAudio.name; }
  set{UnityAudio.name = value; }
 }
	public UnityEngine.ParticleEmitter particleEmitter{  get { return UnityAudio.particleEmitter; }
 }
	public UnityEngine.ParticleSystem particleSystem{  get { return UnityAudio.particleSystem; }
 }
	public UnityEngine.Renderer renderer{  get { return UnityAudio.renderer; }
 }
	public UnityEngine.Rigidbody rigidbody{  get { return UnityAudio.rigidbody; }
 }
	public UnityEngine.Rigidbody2D rigidbody2D{  get { return UnityAudio.rigidbody2D; }
 }
	public System.String tag{  get { return UnityAudio.tag; }
  set{UnityAudio.tag = value; }
 }
	public UnityEngine.Transform transform{  get { return UnityAudio.transform; }
 }
	public System.Boolean useGUILayout{  get { return UnityAudio.useGUILayout; }
  set{UnityAudio.useGUILayout = value; }
 }
	public System.DateTime ___t000;
	public System.DateTime ___t100;
	public System.TimeSpan ___dt00;
	public Track.Item ___item10;
	public System.Int32 counter12;
	public System.Single count_down2;
	public System.String ___texture10;
	public System.Int32 ___i10;
	public System.Int32 counter19;
	public System.Single count_down1;
	public Track.Item ___item110;
	public System.Int32 counter5;
	public System.Collections.Generic.List<System.Collections.Generic.List<Track.Item>> ___elems10;
	public System.Single count_down4;
	public System.Single count_down3;
	public Track.Item ___item21;
	public UnityEngine.Transform ___item32;
	public System.Int32 counter1;
	public void Update(float dt, World world) {


		Bird.Update(dt, world);
		Button.Update(dt, world);
		Fox.Update(dt, world);
		PauseButton.Update(dt, world);
		this.Rule0(dt, world);
		this.Rule1(dt, world);
		this.Rule2(dt, world);
		this.Rule3(dt, world);
		this.Rule4(dt, world);
		this.Rule5(dt, world);
	}




	int s0=-1;
	public void Rule0(float dt, World world){ 
	switch (s0)
	{

	case -1:
	if(!(!(RunningTutorial)))
	{

	s0 = -1;
return;	}else
	{

	goto case 7;	}
	case 7:
	if(!(((CurrentTrack.IsSome) && (Fox.Sing))))
	{

	s0 = 7;
return;	}else
	{

	goto case 6;	}
	case 6:
	if(!(Bird.Sing))
	{

	s0 = 6;
return;	}else
	{

	goto case 5;	}
	case 5:
	if(!(!(Bird.Sing)))
	{

	s0 = 5;
return;	}else
	{

	goto case 4;	}
	case 4:
	___t000 = DateTime.Now;
	goto case 3;
	case 3:
	if(!(((((Button.Yes) || (Button.No))) && (!(Bird.Sing)))))
	{

	s0 = 3;
return;	}else
	{

	goto case 2;	}
	case 2:
	___t100 = DateTime.Now;
	___dt00 = ___t100.Subtract(___t000);
	CurrentTrack.Value.Answer = Button.Yes;
	CurrentTrack.Value.ResponceTime = ___dt00.TotalSeconds;
	s0 = -1;
return;	
	default: return;}}
	

	int s1=-1;
	public void Rule1(float dt, World world){ 
	switch (s1)
	{

	case -1:
	
	counter12 = -1;
	if((((TrackManager.CurrentTutorial).Count) == (0)))
	{

	goto case 4;	}else
	{

	___item10 = (TrackManager.CurrentTutorial)[0];
	goto case 12;	}
	case 12:
	counter12 = ((counter12) + (1));
	if((((((TrackManager.CurrentTutorial).Count) == (counter12))) || (((counter12) > ((TrackManager.CurrentTutorial).Count)))))
	{

	goto case 4;	}else
	{

	___item10 = (TrackManager.CurrentTutorial)[counter12];
	goto case 13;	}
	case 13:
	CurrentTrack = (new Just<Track.Item>(___item10));
	AnimationEnd = true;
	Button.Yes = Button.Yes;
	Button.Index = Button.Index;
	Button.AnimationNumber = Button.AnimationNumber;
	Button.Texture = Button.Texture;
	RunningTutorial = true;
	StopGame = StopGame;
	s1 = 29;
return;
	case 29:
	if(!(Bird.Sing))
	{

	s1 = 29;
return;	}else
	{

	goto case 28;	}
	case 28:
	if(!(!(Bird.Sing)))
	{

	s1 = 28;
return;	}else
	{

	goto case 26;	}
	case 26:
	count_down2 = 2f;
	goto case 27;
	case 27:
	if(((count_down2) > (0f)))
	{

	count_down2 = ((count_down2) - (dt));
	s1 = 27;
return;	}else
	{

	goto case 25;	}
	case 25:
	if(___item10.IsSame)
	{

	___texture10 = "yes_frames";	}else
	{

	___texture10 = "no_frames";	}
	CurrentTrack = CurrentTrack;
	AnimationEnd = AnimationEnd;
	Button.Yes = Button.Yes;
	Button.Index = 0;
	Button.AnimationNumber = 5;
	Button.Texture = ___texture10;
	RunningTutorial = RunningTutorial;
	StopGame = StopGame;
	s1 = 18;
return;
	case 18:
	
	counter19 = -1;
	if((((Enumerable.Range(0,((1) + (((((Button.AnimationNumber) - (1))) - (0))))).ToList<System.Int32>()).Count) == (0)))
	{

	goto case 17;	}else
	{

	___i10 = (Enumerable.Range(0,((1) + (((((Button.AnimationNumber) - (1))) - (0))))).ToList<System.Int32>())[0];
	goto case 19;	}
	case 19:
	counter19 = ((counter19) + (1));
	if((((((Enumerable.Range(0,((1) + (((((Button.AnimationNumber) - (1))) - (0))))).ToList<System.Int32>()).Count) == (counter19))) || (((counter19) > ((Enumerable.Range(0,((1) + (((((Button.AnimationNumber) - (1))) - (0))))).ToList<System.Int32>()).Count)))))
	{

	goto case 17;	}else
	{

	___i10 = (Enumerable.Range(0,((1) + (((((Button.AnimationNumber) - (1))) - (0))))).ToList<System.Int32>())[counter19];
	goto case 20;	}
	case 20:
	CurrentTrack = CurrentTrack;
	AnimationEnd = AnimationEnd;
	Button.Yes = Button.Yes;
	Button.Index = ___i10;
	Button.AnimationNumber = Button.AnimationNumber;
	Button.Texture = Button.Texture;
	RunningTutorial = RunningTutorial;
	StopGame = StopGame;
	s1 = 21;
return;
	case 21:
	count_down1 = 0.15f;
	goto case 22;
	case 22:
	if(((count_down1) > (0f)))
	{

	count_down1 = ((count_down1) - (dt));
	s1 = 22;
return;	}else
	{

	s1 = 19;
return;	}
	case 17:
	CurrentTrack = (new Nothing<Track.Item>());
	AnimationEnd = true;
	Button.Yes = true;
	Button.Index = 0;
	Button.AnimationNumber = 1;
	Button.Texture = "both_up";
	RunningTutorial = RunningTutorial;
	StopGame = StopGame;
	s1 = 16;
return;
	case 16:
	CurrentTrack = CurrentTrack;
	AnimationEnd = AnimationEnd;
	Button.Yes = false;
	Button.Index = Button.Index;
	Button.AnimationNumber = Button.AnimationNumber;
	Button.Texture = Button.Texture;
	RunningTutorial = RunningTutorial;
	StopGame = StopGame;
	s1 = 15;
return;
	case 15:
	if(!(!(AnimationEnd)))
	{

	s1 = 15;
return;	}else
	{

	goto case 14;	}
	case 14:
	CurrentTrack = CurrentTrack;
	AnimationEnd = true;
	Button.Yes = Button.Yes;
	Button.Index = Button.Index;
	Button.AnimationNumber = Button.AnimationNumber;
	Button.Texture = Button.Texture;
	RunningTutorial = RunningTutorial;
	StopGame = StopGame;
	s1 = 12;
return;
	case 4:
	
	counter5 = -1;
	if((((TrackManager.CurrentExperiment).Count) == (0)))
	{

	goto case 3;	}else
	{

	___item110 = (TrackManager.CurrentExperiment)[0];
	goto case 5;	}
	case 5:
	counter5 = ((counter5) + (1));
	if((((((TrackManager.CurrentExperiment).Count) == (counter5))) || (((counter5) > ((TrackManager.CurrentExperiment).Count)))))
	{

	goto case 3;	}else
	{

	___item110 = (TrackManager.CurrentExperiment)[counter5];
	goto case 6;	}
	case 6:
	CurrentTrack = (new Just<Track.Item>(___item110));
	AnimationEnd = true;
	Button.Yes = Button.Yes;
	Button.Index = Button.Index;
	Button.AnimationNumber = Button.AnimationNumber;
	Button.Texture = Button.Texture;
	RunningTutorial = RunningTutorial;
	StopGame = StopGame;
	s1 = 9;
return;
	case 9:
	CurrentTrack = CurrentTrack;
	AnimationEnd = true;
	Button.Yes = Button.Yes;
	Button.Index = Button.Index;
	Button.AnimationNumber = Button.AnimationNumber;
	Button.Texture = Button.Texture;
	RunningTutorial = false;
	StopGame = StopGame;
	s1 = 8;
return;
	case 8:
	if(!(!(AnimationEnd)))
	{

	s1 = 8;
return;	}else
	{

	goto case 7;	}
	case 7:
	CurrentTrack = CurrentTrack;
	AnimationEnd = true;
	Button.Yes = Button.Yes;
	Button.Index = Button.Index;
	Button.AnimationNumber = Button.AnimationNumber;
	Button.Texture = Button.Texture;
	RunningTutorial = RunningTutorial;
	StopGame = StopGame;
	s1 = 5;
return;
	case 3:
	___elems10 = TrackManager.ItemsToPlay;
	Track.TrackManager.SaveAll();
	Track.TrackManager.MoveNextExperiment();
	UnityEngine.Application.LoadLevel("End");
	s1 = -1;
return;	
	default: return;}}
	

	int s2=-1;
	public void Rule2(float dt, World world){ 
	switch (s2)
	{

	case -1:
	if(!(AnimationEnd))
	{

	s2 = -1;
return;	}else
	{

	goto case 21;	}
	case 21:
	AnimationEnd = AnimationEnd;
	Fox.Walk = true;
	Fox.Sing = false;
	Bird.Sing = false;
	Foreground.RotatingVelocity = -7f;
	Background.RotatingVelocity = -2f;
	s2 = 20;
return;
	case 20:
	if(!(((CurrentCheckpoint.IsSome) && (((1.5f) > (UnityEngine.Vector3.Distance(Fox.Position,CurrentCheckpoint.Value.position)))))))
	{

	s2 = 20;
return;	}else
	{

	goto case 19;	}
	case 19:
	AnimationEnd = AnimationEnd;
	Fox.Walk = false;
	Fox.Sing = false;
	Bird.Sing = false;
	Foreground.RotatingVelocity = 0f;
	Background.RotatingVelocity = 0f;
	s2 = 18;
return;
	case 18:
	if(!(!(Bird.Walk)))
	{

	s2 = 18;
return;	}else
	{

	goto case 17;	}
	case 17:
	AnimationEnd = AnimationEnd;
	Fox.Walk = false;
	Fox.Sing = true;
	Bird.Sing = false;
	Foreground.RotatingVelocity = 0f;
	Background.RotatingVelocity = 0f;
	s2 = 16;
return;
	case 16:
	UnityAudio.Play(CurrentTrack.Value.ItemName);
	count_down4 = ((ClipLength) / (2f));
	goto case 15;
	case 15:
	if(((count_down4) > (0f)))
	{

	count_down4 = ((count_down4) - (dt));
	s2 = 15;
return;	}else
	{

	goto case 13;	}
	case 13:
	AnimationEnd = AnimationEnd;
	Fox.Walk = false;
	Fox.Sing = false;
	Bird.Sing = true;
	Foreground.RotatingVelocity = Foreground.RotatingVelocity;
	Background.RotatingVelocity = Background.RotatingVelocity;
	s2 = 11;
return;
	case 11:
	count_down3 = ((ClipLength) / (2f));
	goto case 12;
	case 12:
	if(((count_down3) > (0f)))
	{

	count_down3 = ((count_down3) - (dt));
	s2 = 12;
return;	}else
	{

	goto case 10;	}
	case 10:
	AnimationEnd = AnimationEnd;
	Fox.Walk = false;
	Fox.Sing = false;
	Bird.Sing = false;
	Foreground.RotatingVelocity = Foreground.RotatingVelocity;
	Background.RotatingVelocity = Background.RotatingVelocity;
	s2 = 9;
return;
	case 9:
	___item21 = CurrentTrack.Value;
	goto case 8;
	case 8:
	if(!(((Button.Yes) || (Button.No))))
	{

	s2 = 8;
return;	}else
	{

	goto case 5;	}
	case 5:
	if(((___item21.Answer) || (RunningTutorial)))
	{

	goto case 3;	}else
	{

	goto case 4;	}
	case 3:
	UnityAudio.Play("cheer");
	s2 = 2;
return;
	case 4:
	UnityAudio.Play("boo");
	s2 = 2;
return;
	case 2:
	if(!(Bird.Walk))
	{

	s2 = 2;
return;	}else
	{

	goto case 1;	}
	case 1:
	if(!(!(Bird.Walk)))
	{

	s2 = 1;
return;	}else
	{

	goto case 0;	}
	case 0:
	AnimationEnd = false;
	Fox.Walk = Fox.Walk;
	Fox.Sing = Fox.Sing;
	Bird.Sing = Bird.Sing;
	Foreground.RotatingVelocity = Foreground.RotatingVelocity;
	Background.RotatingVelocity = Background.RotatingVelocity;
	s2 = -1;
return;	
	default: return;}}
	

	int s3=-1;
	public void Rule3(float dt, World world){ 
	switch (s3)
	{

	case -1:
	
	counter1 = -1;
	if((((Foreground.CheckPoints).Count) == (0)))
	{

	s3 = -1;
return;	}else
	{

	___item32 = (Foreground.CheckPoints)[0];
	goto case 1;	}
	case 1:
	counter1 = ((counter1) + (1));
	if((((((Foreground.CheckPoints).Count) == (counter1))) || (((counter1) > ((Foreground.CheckPoints).Count)))))
	{

	s3 = -1;
return;	}else
	{

	___item32 = (Foreground.CheckPoints)[counter1];
	goto case 2;	}
	case 2:
	if(!(!(StopGame)))
	{

	s3 = 2;
return;	}else
	{

	goto case 14;	}
	case 14:
	CurrentCheckpoint = (new Just<UnityEngine.Transform>(___item32));
	Bird.Walk = true;
	Bird.Position = Bird.Position;
	Bird.Destination = Bird.Destination;
	s3 = 13;
return;
	case 13:
	if(!(Fox.Walk))
	{

	s3 = 13;
return;	}else
	{

	goto case 12;	}
	case 12:
	if(!(!(Fox.Walk)))
	{

	s3 = 12;
return;	}else
	{

	goto case 11;	}
	case 11:
	CurrentCheckpoint = CurrentCheckpoint;
	Bird.Walk = Bird.Walk;
	Bird.Position = Bird.Enter;
	Bird.Destination = ___item32.position;
	s3 = 10;
return;
	case 10:
	if(!(((0.1f) > (UnityEngine.Vector3.Distance(Bird.Position,___item32.position)))))
	{

	s3 = 10;
return;	}else
	{

	goto case 9;	}
	case 9:
	CurrentCheckpoint = CurrentCheckpoint;
	Bird.Walk = false;
	Bird.Position = Bird.Destination;
	Bird.Destination = Bird.Destination;
	s3 = 8;
return;
	case 8:
	if(!(Bird.Sing))
	{

	s3 = 8;
return;	}else
	{

	goto case 7;	}
	case 7:
	if(!(!(Bird.Sing)))
	{

	s3 = 7;
return;	}else
	{

	goto case 6;	}
	case 6:
	if(!(((Button.Yes) || (Button.No))))
	{

	s3 = 6;
return;	}else
	{

	goto case 5;	}
	case 5:
	CurrentCheckpoint = CurrentCheckpoint;
	Bird.Walk = true;
	Bird.Position = Bird.Position;
	Bird.Destination = Bird.Exit;
	s3 = 4;
return;
	case 4:
	if(!(((0.1f) > (UnityEngine.Vector3.Distance(Bird.Position,Bird.Exit)))))
	{

	s3 = 4;
return;	}else
	{

	goto case 3;	}
	case 3:
	CurrentCheckpoint = (new Nothing<UnityEngine.Transform>());
	Bird.Walk = false;
	Bird.Position = Bird.Exit;
	Bird.Destination = Bird.Destination;
	s3 = 1;
return;	
	default: return;}}
	

	int s4=-1;
	public void Rule4(float dt, World world){ 
	switch (s4)
	{

	case -1:
	if(!(((PauseMenu.IsSome) && (Resume._Resume))))
	{

	s4 = -1;
return;	}else
	{

	goto case 1;	}
	case 1:
	UnityAudio.Resume();
	Resume.Destroyed = true;
	PauseMenu = (new Nothing<PauseMenu>());
	Time.timeScale = 1f;
	s4 = -1;
return;	
	default: return;}}
	

	int s5=-1;
	public void Rule5(float dt, World world){ 
	switch (s5)
	{

	case -1:
	if(!(PauseButton.Pressed))
	{

	s5 = -1;
return;	}else
	{

	goto case 2;	}
	case 2:
	PauseMenu = (new Just<PauseMenu>(new PauseMenu()));
	Time.timeScale = 0f;
	s5 = 1;
return;
	case 1:
	UnityAudio.Pause();
	goto case 0;
	case 0:
	if(!(!(PauseButton.Pressed)))
	{

	s5 = 0;
return;	}else
	{

	s5 = -1;
return;	}	
	default: return;}}
	





}
public class Button{
public int frame;
public bool JustEntered = true;
	public int ID;
public Button()
	{JustEntered = false;
 frame = World.frame;
		Yes = false;
		UnityYesNo = UnityYesNo.Find(1);
		No = false;
		
}
		public System.Int32 AnimationNumber{  get { return UnityYesNo.AnimationNumber; }
  set{UnityYesNo.AnimationNumber = value; }
 }
	public System.Int32 Index{  get { return UnityYesNo.Index; }
  set{UnityYesNo.Index = value; }
 }
	public System.Boolean IsNoPressed{  get { return UnityYesNo.IsNoPressed; }
 }
	public System.Boolean IsYesPressed{  get { return UnityYesNo.IsYesPressed; }
 }
	public System.Boolean No;
	public System.String Texture{  get { return UnityYesNo.Texture; }
  set{UnityYesNo.Texture = value; }
 }
	public UnityYesNo UnityYesNo;
	public System.Boolean Yes;
	public UnityEngine.Animation animation{  get { return UnityYesNo.animation; }
 }
	public UnityEngine.AudioSource audio{  get { return UnityYesNo.audio; }
 }
	public UnityEngine.Camera camera{  get { return UnityYesNo.camera; }
 }
	public System.Int32 colCount{  get { return UnityYesNo.colCount; }
  set{UnityYesNo.colCount = value; }
 }
	public UnityEngine.Collider collider{  get { return UnityYesNo.collider; }
 }
	public UnityEngine.Collider2D collider2D{  get { return UnityYesNo.collider2D; }
 }
	public UnityEngine.ConstantForce constantForce{  get { return UnityYesNo.constantForce; }
 }
	public System.Boolean enabled{  get { return UnityYesNo.enabled; }
  set{UnityYesNo.enabled = value; }
 }
	public UnityEngine.GameObject gameObject{  get { return UnityYesNo.gameObject; }
 }
	public UnityEngine.GUIElement guiElement{  get { return UnityYesNo.guiElement; }
 }
	public UnityEngine.GUIText guiText{  get { return UnityYesNo.guiText; }
 }
	public UnityEngine.GUITexture guiTexture{  get { return UnityYesNo.guiTexture; }
 }
	public UnityEngine.HideFlags hideFlags{  get { return UnityYesNo.hideFlags; }
  set{UnityYesNo.hideFlags = value; }
 }
	public UnityEngine.HingeJoint hingeJoint{  get { return UnityYesNo.hingeJoint; }
 }
	public UnityEngine.Light light{  get { return UnityYesNo.light; }
 }
	public System.String name{  get { return UnityYesNo.name; }
  set{UnityYesNo.name = value; }
 }
	public UnityEngine.Transform noButton{  get { return UnityYesNo.noButton; }
  set{UnityYesNo.noButton = value; }
 }
	public UnityEngine.ParticleEmitter particleEmitter{  get { return UnityYesNo.particleEmitter; }
 }
	public UnityEngine.ParticleSystem particleSystem{  get { return UnityYesNo.particleSystem; }
 }
	public UnityEngine.Renderer renderer{  get { return UnityYesNo.renderer; }
 }
	public UnityEngine.Rigidbody rigidbody{  get { return UnityYesNo.rigidbody; }
 }
	public UnityEngine.Rigidbody2D rigidbody2D{  get { return UnityYesNo.rigidbody2D; }
 }
	public System.String tag{  get { return UnityYesNo.tag; }
  set{UnityYesNo.tag = value; }
 }
	public UnityEngine.Transform transform{  get { return UnityYesNo.transform; }
 }
	public System.Boolean useGUILayout{  get { return UnityYesNo.useGUILayout; }
  set{UnityYesNo.useGUILayout = value; }
 }
	public UnityEngine.Transform yesButton{  get { return UnityYesNo.yesButton; }
  set{UnityYesNo.yesButton = value; }
 }
	public System.Single count_down5;
	public System.Single count_down6;
	public void Update(float dt, World world) {
frame = World.frame;

		this.Rule0(dt, world);
		this.Rule1(dt, world);
	}





	int s0=-1;
	public void Rule0(float dt, World world){ 
	switch (s0)
	{

	case -1:
	Texture = "both_up";
	Yes = false;
	s0 = 3;
return;
	case 3:
	if(!(((((!(world.RunningTutorial)) && (UnityEngine.Input.GetMouseButtonDown(0)))) && (IsYesPressed))))
	{

	s0 = 3;
return;	}else
	{

	goto case 2;	}
	case 2:
	Texture = "Yes_down";
	Yes = true;
	s0 = 0;
return;
	case 0:
	count_down5 = 0.1f;
	goto case 1;
	case 1:
	if(((count_down5) > (0f)))
	{

	count_down5 = ((count_down5) - (dt));
	s0 = 1;
return;	}else
	{

	s0 = -1;
return;	}	
	default: return;}}
	

	int s1=-1;
	public void Rule1(float dt, World world){ 
	switch (s1)
	{

	case -1:
	Texture = "both_up";
	No = false;
	s1 = 3;
return;
	case 3:
	if(!(((((!(world.RunningTutorial)) && (UnityEngine.Input.GetMouseButtonDown(0)))) && (IsNoPressed))))
	{

	s1 = 3;
return;	}else
	{

	goto case 2;	}
	case 2:
	Texture = "No_down";
	No = true;
	s1 = 0;
return;
	case 0:
	count_down6 = 0.1f;
	goto case 1;
	case 1:
	if(((count_down6) > (0f)))
	{

	count_down6 = ((count_down6) - (dt));
	s1 = 1;
return;	}else
	{

	s1 = -1;
return;	}	
	default: return;}}
	





}
public class PauseButton{
public int frame;
public bool JustEntered = true;
	public int ID;
public PauseButton()
	{JustEntered = false;
 frame = World.frame;
		UnityPause = UnityPause.Find();
		Pressed = false;
		
}
		public System.Boolean IsPausePressed{  get { return UnityPause.IsPausePressed; }
 }
	public System.Boolean Pressed;
	public System.String Texture{  get { return UnityPause.Texture; }
  set{UnityPause.Texture = value; }
 }
	public UnityPause UnityPause;
	public UnityEngine.Animation animation{  get { return UnityPause.animation; }
 }
	public UnityEngine.AudioSource audio{  get { return UnityPause.audio; }
 }
	public UnityEngine.Camera camera{  get { return UnityPause.camera; }
 }
	public UnityEngine.Collider collider{  get { return UnityPause.collider; }
 }
	public UnityEngine.Collider2D collider2D{  get { return UnityPause.collider2D; }
 }
	public UnityEngine.ConstantForce constantForce{  get { return UnityPause.constantForce; }
 }
	public System.Boolean enabled{  get { return UnityPause.enabled; }
  set{UnityPause.enabled = value; }
 }
	public UnityEngine.GameObject gameObject{  get { return UnityPause.gameObject; }
 }
	public UnityEngine.GUIElement guiElement{  get { return UnityPause.guiElement; }
 }
	public UnityEngine.GUIText guiText{  get { return UnityPause.guiText; }
 }
	public UnityEngine.GUITexture guiTexture{  get { return UnityPause.guiTexture; }
 }
	public UnityEngine.HideFlags hideFlags{  get { return UnityPause.hideFlags; }
  set{UnityPause.hideFlags = value; }
 }
	public UnityEngine.HingeJoint hingeJoint{  get { return UnityPause.hingeJoint; }
 }
	public UnityEngine.Light light{  get { return UnityPause.light; }
 }
	public System.String name{  get { return UnityPause.name; }
  set{UnityPause.name = value; }
 }
	public UnityEngine.ParticleEmitter particleEmitter{  get { return UnityPause.particleEmitter; }
 }
	public UnityEngine.ParticleSystem particleSystem{  get { return UnityPause.particleSystem; }
 }
	public UnityEngine.Transform pauseButton{  get { return UnityPause.pauseButton; }
  set{UnityPause.pauseButton = value; }
 }
	public UnityEngine.Renderer renderer{  get { return UnityPause.renderer; }
 }
	public UnityEngine.Rigidbody rigidbody{  get { return UnityPause.rigidbody; }
 }
	public UnityEngine.Rigidbody2D rigidbody2D{  get { return UnityPause.rigidbody2D; }
 }
	public System.String tag{  get { return UnityPause.tag; }
  set{UnityPause.tag = value; }
 }
	public UnityEngine.Transform transform{  get { return UnityPause.transform; }
 }
	public System.Boolean useGUILayout{  get { return UnityPause.useGUILayout; }
  set{UnityPause.useGUILayout = value; }
 }
	public System.Single count_down7;
	public void Update(float dt, World world) {
frame = World.frame;

		this.Rule0(dt, world);

	}





	int s0=-1;
	public void Rule0(float dt, World world){ 
	switch (s0)
	{

	case -1:
	Texture = "PauseButton";
	Pressed = false;
	s0 = 3;
return;
	case 3:
	if(!(((UnityEngine.Input.GetMouseButtonDown(0)) && (IsPausePressed))))
	{

	s0 = 3;
return;	}else
	{

	goto case 2;	}
	case 2:
	Texture = "PauseButton_down";
	Pressed = true;
	s0 = 0;
return;
	case 0:
	count_down7 = 0.1f;
	goto case 1;
	case 1:
	if(((count_down7) > (0f)))
	{

	count_down7 = ((count_down7) - (dt));
	s0 = 1;
return;	}else
	{

	s0 = -1;
return;	}	
	default: return;}}
	






}
public class PauseMenu{
public int frame;
public bool JustEntered = true;
	public int ID;
public PauseMenu()
	{JustEntered = false;
 frame = World.frame;
		UnityPauseMenu = UnityPauseMenu.Instantiate();
		
}
		public System.Boolean Destroyed{  get { return UnityPauseMenu.Destroyed; }
  set{UnityPauseMenu.Destroyed = value; }
 }
	public System.Boolean Resume{  get { return UnityPauseMenu.Resume; }
  set{UnityPauseMenu.Resume = value; }
 }
	public UnityPauseMenu UnityPauseMenu;
	public UnityEngine.Animation animation{  get { return UnityPauseMenu.animation; }
 }
	public UnityEngine.AudioSource audio{  get { return UnityPauseMenu.audio; }
 }
	public UnityEngine.Camera camera{  get { return UnityPauseMenu.camera; }
 }
	public UnityEngine.Collider collider{  get { return UnityPauseMenu.collider; }
 }
	public UnityEngine.Collider2D collider2D{  get { return UnityPauseMenu.collider2D; }
 }
	public UnityEngine.ConstantForce constantForce{  get { return UnityPauseMenu.constantForce; }
 }
	public System.Boolean enabled{  get { return UnityPauseMenu.enabled; }
  set{UnityPauseMenu.enabled = value; }
 }
	public UnityEngine.GameObject gameObject{  get { return UnityPauseMenu.gameObject; }
 }
	public UnityEngine.GUIElement guiElement{  get { return UnityPauseMenu.guiElement; }
 }
	public UnityEngine.GUIText guiText{  get { return UnityPauseMenu.guiText; }
 }
	public UnityEngine.GUITexture guiTexture{  get { return UnityPauseMenu.guiTexture; }
 }
	public UnityEngine.HideFlags hideFlags{  get { return UnityPauseMenu.hideFlags; }
  set{UnityPauseMenu.hideFlags = value; }
 }
	public UnityEngine.HingeJoint hingeJoint{  get { return UnityPauseMenu.hingeJoint; }
 }
	public UnityEngine.Light light{  get { return UnityPauseMenu.light; }
 }
	public System.String name{  get { return UnityPauseMenu.name; }
  set{UnityPauseMenu.name = value; }
 }
	public UnityEngine.ParticleEmitter particleEmitter{  get { return UnityPauseMenu.particleEmitter; }
 }
	public UnityEngine.ParticleSystem particleSystem{  get { return UnityPauseMenu.particleSystem; }
 }
	public UnityEngine.Renderer renderer{  get { return UnityPauseMenu.renderer; }
 }
	public UnityEngine.Rigidbody rigidbody{  get { return UnityPauseMenu.rigidbody; }
 }
	public UnityEngine.Rigidbody2D rigidbody2D{  get { return UnityPauseMenu.rigidbody2D; }
 }
	public System.String tag{  get { return UnityPauseMenu.tag; }
  set{UnityPauseMenu.tag = value; }
 }
	public UnityEngine.Transform transform{  get { return UnityPauseMenu.transform; }
 }
	public System.Boolean useGUILayout{  get { return UnityPauseMenu.useGUILayout; }
  set{UnityPauseMenu.useGUILayout = value; }
 }
	public void Update(float dt, World world) {
frame = World.frame;



	}











}
public class Animal{
public int frame;
public bool JustEntered = true;
private System.String animal;
private System.String walkTexture;
private System.Int32 walkTextureFrames;
private System.String singTexture;
private System.Int32 singTextureFrames;
private UnityEngine.Vector3 enter;
private UnityEngine.Vector3 exit;
private UnityEngine.Vector3 destination;
	public int ID;
public Animal(System.String animal, System.String walkTexture, System.Int32 walkTextureFrames, System.String singTexture, System.Int32 singTextureFrames, UnityEngine.Vector3 enter, UnityEngine.Vector3 exit, UnityEngine.Vector3 destination)
	{JustEntered = false;
 frame = World.frame;
		WalkTextureFrames = walkTextureFrames;
		WalkTexture = walkTexture;
		Walk = false;
		UnityAnimal = UnityAnimal.Find(animal,walkTextureFrames,enter);
		SingTextureFrames = singTextureFrames;
		SingTexture = singTexture;
		Sing = false;
		Exit = exit;
		Enter = enter;
		Destination = destination;
		
}
		public System.String AnimalTexture{  get { return UnityAnimal.AnimalTexture; }
  set{UnityAnimal.AnimalTexture = value; }
 }
	public System.Int32 AnimationIndex{  get { return UnityAnimal.AnimationIndex; }
  set{UnityAnimal.AnimationIndex = value; }
 }
	public System.Int32 AnimationNumber{  get { return UnityAnimal.AnimationNumber; }
  set{UnityAnimal.AnimationNumber = value; }
 }
	public UnityEngine.Vector3 Destination;
	public UnityEngine.Vector3 Enter;
	public UnityEngine.Vector3 Exit;
	public UnityEngine.Vector3 Position{  get { return UnityAnimal.Position; }
  set{UnityAnimal.Position = value; }
 }
	public System.Boolean Sing;
	public System.String SingTexture;
	public System.Int32 SingTextureFrames;
	public UnityAnimal UnityAnimal;
	public UnityEngine.Vector3 Velocity{  get { return UnityAnimal.Velocity; }
  set{UnityAnimal.Velocity = value; }
 }
	public System.Boolean Walk;
	public System.String WalkTexture;
	public System.Int32 WalkTextureFrames;
	public UnityEngine.Animation animation{  get { return UnityAnimal.animation; }
 }
	public UnityEngine.AudioSource audio{  get { return UnityAnimal.audio; }
 }
	public UnityEngine.Camera camera{  get { return UnityAnimal.camera; }
 }
	public System.Int32 colCount{  get { return UnityAnimal.colCount; }
  set{UnityAnimal.colCount = value; }
 }
	public UnityEngine.Collider collider{  get { return UnityAnimal.collider; }
 }
	public UnityEngine.Collider2D collider2D{  get { return UnityAnimal.collider2D; }
 }
	public UnityEngine.ConstantForce constantForce{  get { return UnityAnimal.constantForce; }
 }
	public System.Boolean enabled{  get { return UnityAnimal.enabled; }
  set{UnityAnimal.enabled = value; }
 }
	public UnityEngine.GameObject gameObject{  get { return UnityAnimal.gameObject; }
 }
	public UnityEngine.GUIElement guiElement{  get { return UnityAnimal.guiElement; }
 }
	public UnityEngine.GUIText guiText{  get { return UnityAnimal.guiText; }
 }
	public UnityEngine.GUITexture guiTexture{  get { return UnityAnimal.guiTexture; }
 }
	public UnityEngine.HideFlags hideFlags{  get { return UnityAnimal.hideFlags; }
  set{UnityAnimal.hideFlags = value; }
 }
	public UnityEngine.HingeJoint hingeJoint{  get { return UnityAnimal.hingeJoint; }
 }
	public UnityEngine.Light light{  get { return UnityAnimal.light; }
 }
	public System.String name{  get { return UnityAnimal.name; }
  set{UnityAnimal.name = value; }
 }
	public UnityEngine.ParticleEmitter particleEmitter{  get { return UnityAnimal.particleEmitter; }
 }
	public UnityEngine.ParticleSystem particleSystem{  get { return UnityAnimal.particleSystem; }
 }
	public UnityEngine.Renderer renderer{  get { return UnityAnimal.renderer; }
 }
	public UnityEngine.Rigidbody rigidbody{  get { return UnityAnimal.rigidbody; }
 }
	public UnityEngine.Rigidbody2D rigidbody2D{  get { return UnityAnimal.rigidbody2D; }
 }
	public System.String tag{  get { return UnityAnimal.tag; }
  set{UnityAnimal.tag = value; }
 }
	public UnityEngine.Transform transform{  get { return UnityAnimal.transform; }
 }
	public System.Boolean useGUILayout{  get { return UnityAnimal.useGUILayout; }
  set{UnityAnimal.useGUILayout = value; }
 }
	public System.Int32 ___i01;
	public System.Int32 counter13;
	public System.Single count_down9;
	public System.Int32 ___i02;
	public System.Int32 counter3;
	public System.Single count_down8;
	public System.Single count_down10;
	public void Update(float dt, World world) {
frame = World.frame;		this.Rule2(dt, world);

		this.Rule0(dt, world);
		this.Rule1(dt, world);
	}

	public void Rule2(float dt, World world) 
	{
	Position = (Position) + ((Velocity) * (dt));
	}
	




	int s0=-1;
	public void Rule0(float dt, World world){ 
	switch (s0)
	{

	case -1:
	AnimationIndex = 0;
	AnimationNumber = WalkTextureFrames;
	AnimalTexture = WalkTexture;
	s0 = 18;
return;
	case 18:
	if(!(Walk))
	{

	s0 = 18;
return;	}else
	{

	goto case 10;	}
	case 10:
	if(!(Walk))
	{

	goto case 9;	}else
	{

	goto case 11;	}
	case 11:
	
	counter13 = -1;
	if((((Enumerable.Range(0,((1) + (((((WalkTextureFrames) - (1))) - (0))))).ToList<System.Int32>()).Count) == (0)))
	{

	s0 = 10;
return;	}else
	{

	___i01 = (Enumerable.Range(0,((1) + (((((WalkTextureFrames) - (1))) - (0))))).ToList<System.Int32>())[0];
	goto case 13;	}
	case 13:
	counter13 = ((counter13) + (1));
	if((((((Enumerable.Range(0,((1) + (((((WalkTextureFrames) - (1))) - (0))))).ToList<System.Int32>()).Count) == (counter13))) || (((counter13) > ((Enumerable.Range(0,((1) + (((((WalkTextureFrames) - (1))) - (0))))).ToList<System.Int32>()).Count)))))
	{

	s0 = 10;
return;	}else
	{

	___i01 = (Enumerable.Range(0,((1) + (((((WalkTextureFrames) - (1))) - (0))))).ToList<System.Int32>())[counter13];
	goto case 14;	}
	case 14:
	AnimationIndex = ___i01;
	AnimationNumber = AnimationNumber;
	AnimalTexture = AnimalTexture;
	s0 = 15;
return;
	case 15:
	count_down9 = 0.15f;
	goto case 16;
	case 16:
	if(((count_down9) > (0f)))
	{

	count_down9 = ((count_down9) - (dt));
	s0 = 16;
return;	}else
	{

	s0 = 13;
return;	}
	case 9:
	AnimationIndex = 0;
	AnimationNumber = SingTextureFrames;
	AnimalTexture = SingTexture;
	s0 = 8;
return;
	case 8:
	if(!(Sing))
	{

	s0 = 8;
return;	}else
	{

	goto case 0;	}
	case 0:
	if(!(Sing))
	{

	s0 = -1;
return;	}else
	{

	goto case 1;	}
	case 1:
	
	counter3 = -1;
	if((((Enumerable.Range(0,((1) + (((((SingTextureFrames) - (1))) - (0))))).ToList<System.Int32>()).Count) == (0)))
	{

	s0 = 0;
return;	}else
	{

	___i02 = (Enumerable.Range(0,((1) + (((((SingTextureFrames) - (1))) - (0))))).ToList<System.Int32>())[0];
	goto case 3;	}
	case 3:
	counter3 = ((counter3) + (1));
	if((((((Enumerable.Range(0,((1) + (((((SingTextureFrames) - (1))) - (0))))).ToList<System.Int32>()).Count) == (counter3))) || (((counter3) > ((Enumerable.Range(0,((1) + (((((SingTextureFrames) - (1))) - (0))))).ToList<System.Int32>()).Count)))))
	{

	s0 = 0;
return;	}else
	{

	___i02 = (Enumerable.Range(0,((1) + (((((SingTextureFrames) - (1))) - (0))))).ToList<System.Int32>())[counter3];
	goto case 4;	}
	case 4:
	AnimationIndex = ___i02;
	AnimationNumber = AnimationNumber;
	AnimalTexture = AnimalTexture;
	s0 = 5;
return;
	case 5:
	count_down8 = 0.08f;
	goto case 6;
	case 6:
	if(((count_down8) > (0f)))
	{

	count_down8 = ((count_down8) - (dt));
	s0 = 6;
return;	}else
	{

	s0 = 3;
return;	}	
	default: return;}}
	

	int s1=-1;
	public void Rule1(float dt, World world){ 
	switch (s1)
	{

	case -1:
	count_down10 = 0f;
	goto case 6;
	case 6:
	if(((count_down10) > (0f)))
	{

	count_down10 = ((count_down10) - (dt));
	s1 = 6;
return;	}else
	{

	goto case 2;	}
	case 2:
	if(((UnityEngine.Vector3.Distance(Position,Destination)) > (0.1f)))
	{

	goto case 0;	}else
	{

	goto case 1;	}
	case 0:
	Velocity = ((Destination) - (Position));
	s1 = -1;
return;
	case 1:
	Velocity = Vector3.zero;
	s1 = -1;
return;	
	default: return;}}
	





}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                   