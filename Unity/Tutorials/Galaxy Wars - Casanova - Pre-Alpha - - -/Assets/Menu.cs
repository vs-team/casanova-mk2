#pragma warning disable 162,108,618
using Casanova.Prelude;
using System.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace Menu {public class Menu : MonoBehaviour{
public static int frame;
void Update () { Update(Time.deltaTime, this); 
 frame++; }
public bool JustEntered = true;


public void Start()
	{
		UnityMenu = new UnityMenu();
		StartButton = new ButtonGUI("Canvas/Play");
		PreviousMButton = (new Nothing<ButtonGUI>());
		PlusButton = (new Nothing<ButtonGUI>());
		NextMButton = (new Nothing<ButtonGUI>());
		LessButton = (new Nothing<ButtonGUI>());
		BackButton = new ButtonGUI("Canvas/Back");
		
}
		public System.Int32 AmountOfPlayers{  get { return UnityMenu.AmountOfPlayers; }
  set{UnityMenu.AmountOfPlayers = value; }
 }
	public ButtonGUI __BackButton;
	public ButtonGUI BackButton{  get { return  __BackButton; }
  set{ __BackButton = value;
 if(!value.JustEntered) __BackButton = value; 
 else{ value.JustEntered = false;
}
 }
 }
	public System.Int32 CurrentScreenNumber{  get { return UnityMenu.CurrentScreenNumber; }
  set{UnityMenu.CurrentScreenNumber = value; }
 }
	public Option<ButtonGUI> __LessButton;
	public Option<ButtonGUI> LessButton{  get { return  __LessButton; }
  set{ __LessButton = value;
 if(value.IsSome){if(!value.Value.JustEntered) __LessButton = value; 
 else{ value.Value.JustEntered = false;
}
} }
 }
	public System.String MapName{  get { return UnityMenu.MapName; }
  set{UnityMenu.MapName = value; }
 }
	public System.Int32 MapSelectNumber{  get { return UnityMenu.MapSelectNumber; }
  set{UnityMenu.MapSelectNumber = value; }
 }
	public UnityEngine.Texture MapSelecter{  get { return UnityMenu.MapSelecter; }
  set{UnityMenu.MapSelecter = value; }
 }
	public Option<ButtonGUI> __NextMButton;
	public Option<ButtonGUI> NextMButton{  get { return  __NextMButton; }
  set{ __NextMButton = value;
 if(value.IsSome){if(!value.Value.JustEntered) __NextMButton = value; 
 else{ value.Value.JustEntered = false;
}
} }
 }
	public System.String PlayerAmount{  get { return UnityMenu.PlayerAmount; }
  set{UnityMenu.PlayerAmount = value; }
 }
	public Option<ButtonGUI> __PlusButton;
	public Option<ButtonGUI> PlusButton{  get { return  __PlusButton; }
  set{ __PlusButton = value;
 if(value.IsSome){if(!value.Value.JustEntered) __PlusButton = value; 
 else{ value.Value.JustEntered = false;
}
} }
 }
	public Option<ButtonGUI> __PreviousMButton;
	public Option<ButtonGUI> PreviousMButton{  get { return  __PreviousMButton; }
  set{ __PreviousMButton = value;
 if(value.IsSome){if(!value.Value.JustEntered) __PreviousMButton = value; 
 else{ value.Value.JustEntered = false;
}
} }
 }
	public ButtonGUI __StartButton;
	public ButtonGUI StartButton{  get { return  __StartButton; }
  set{ __StartButton = value;
 if(!value.JustEntered) __StartButton = value; 
 else{ value.JustEntered = false;
}
 }
 }
	public UnityMenu UnityMenu;
	public UnityEngine.Animation animation{  get { return UnityMenu.animation; }
 }
	public UnityEngine.AudioSource audio{  get { return UnityMenu.audio; }
 }
	public UnityEngine.Camera camera{  get { return UnityMenu.camera; }
 }
	public UnityEngine.Collider collider{  get { return UnityMenu.collider; }
 }
	public UnityEngine.Collider2D collider2D{  get { return UnityMenu.collider2D; }
 }
	public UnityEngine.ConstantForce constantForce{  get { return UnityMenu.constantForce; }
 }
	public System.Boolean enabled{  get { return UnityMenu.enabled; }
  set{UnityMenu.enabled = value; }
 }
	public UnityEngine.GameObject gameObject{  get { return UnityMenu.gameObject; }
 }
	public UnityEngine.GUIElement guiElement{  get { return UnityMenu.guiElement; }
 }
	public UnityEngine.GUIText guiText{  get { return UnityMenu.guiText; }
 }
	public UnityEngine.GUITexture guiTexture{  get { return UnityMenu.guiTexture; }
 }
	public UnityEngine.HideFlags hideFlags{  get { return UnityMenu.hideFlags; }
  set{UnityMenu.hideFlags = value; }
 }
	public UnityEngine.HingeJoint hingeJoint{  get { return UnityMenu.hingeJoint; }
 }
	public UnityEngine.Light light{  get { return UnityMenu.light; }
 }
	public System.String name{  get { return UnityMenu.name; }
  set{UnityMenu.name = value; }
 }
	public UnityEngine.ParticleEmitter particleEmitter{  get { return UnityMenu.particleEmitter; }
 }
	public UnityEngine.ParticleSystem particleSystem{  get { return UnityMenu.particleSystem; }
 }
	public UnityEngine.Renderer renderer{  get { return UnityMenu.renderer; }
 }
	public UnityEngine.Rigidbody rigidbody{  get { return UnityMenu.rigidbody; }
 }
	public UnityEngine.Rigidbody2D rigidbody2D{  get { return UnityMenu.rigidbody2D; }
 }
	public System.String tag{  get { return UnityMenu.tag; }
  set{UnityMenu.tag = value; }
 }
	public UnityEngine.Transform transform{  get { return UnityMenu.transform; }
 }
	public System.Boolean useGUILayout{  get { return UnityMenu.useGUILayout; }
  set{UnityMenu.useGUILayout = value; }
 }
	public ButtonGUI ___LessB00;
	public ButtonGUI ___PlusB10;
	public ButtonGUI ___NextM20;
	public ButtonGUI ___PreviousM30;
	public System.Int32 ___Minus40;
	public System.Int32 ___Plus40;
	public System.Int32 ___previous90;
	public System.Int32 ___next100;

System.DateTime init_time = System.DateTime.Now;
	public void Update(float dt, Menu world) {
var t = System.DateTime.Now;

		this.Rule0(dt, world);
		this.Rule1(dt, world);
		this.Rule2(dt, world);
		this.Rule3(dt, world);
		this.Rule4(dt, world);
		this.Rule5(dt, world);
		this.Rule6(dt, world);
		this.Rule7(dt, world);
		this.Rule8(dt, world);
		this.Rule9(dt, world);
		this.Rule10(dt, world);
	}





	int s0=-1;
	public void Rule0(float dt, Menu world){ 
	switch (s0)
	{

	case -1:
	if(((CurrentScreenNumber) == (1)))
	{

	goto case 0;	}else
	{

	goto case 1;	}
	case 0:
	___LessB00 = new ButtonGUI("Canvas/LessP");
	UnityEngine.Debug.Log("LessButton Created");
	LessButton = (new Just<ButtonGUI>(___LessB00));
	s0 = 3;
return;
	case 3:
	if(!(false))
	{

	s0 = 3;
return;	}else
	{

	s0 = -1;
return;	}
	case 1:
	UnityEngine.Debug.Log("LessButton Destroyed");
	LessButton = (new Nothing<ButtonGUI>());
	s0 = -1;
return;	
	default: return;}}
	

	int s1=-1;
	public void Rule1(float dt, Menu world){ 
	switch (s1)
	{

	case -1:
	if(((CurrentScreenNumber) == (1)))
	{

	goto case 9;	}else
	{

	goto case 10;	}
	case 9:
	___PlusB10 = new ButtonGUI("Canvas/MoreP");
	UnityEngine.Debug.Log("PlusButton Created");
	PlusButton = (new Just<ButtonGUI>(___PlusB10));
	s1 = 12;
return;
	case 12:
	if(!(false))
	{

	s1 = 12;
return;	}else
	{

	s1 = -1;
return;	}
	case 10:
	UnityEngine.Debug.Log("PlusButton Destroyed");
	PlusButton = (new Nothing<ButtonGUI>());
	s1 = -1;
return;	
	default: return;}}
	

	int s2=-1;
	public void Rule2(float dt, Menu world){ 
	switch (s2)
	{

	case -1:
	if(((CurrentScreenNumber) == (1)))
	{

	goto case 18;	}else
	{

	goto case 19;	}
	case 18:
	___NextM20 = new ButtonGUI("Canvas/NextM");
	UnityEngine.Debug.Log("NextMButton Created");
	NextMButton = (new Just<ButtonGUI>(___NextM20));
	s2 = 21;
return;
	case 21:
	if(!(false))
	{

	s2 = 21;
return;	}else
	{

	s2 = -1;
return;	}
	case 19:
	UnityEngine.Debug.Log("NextMButton Destroyed");
	NextMButton = (new Nothing<ButtonGUI>());
	s2 = -1;
return;	
	default: return;}}
	

	int s3=-1;
	public void Rule3(float dt, Menu world){ 
	switch (s3)
	{

	case -1:
	if(((CurrentScreenNumber) == (1)))
	{

	goto case 27;	}else
	{

	goto case 28;	}
	case 27:
	___PreviousM30 = new ButtonGUI("Canvas/PreviousM");
	UnityEngine.Debug.Log("PreviousM Created");
	PreviousMButton = (new Just<ButtonGUI>(___PreviousM30));
	s3 = 30;
return;
	case 30:
	if(!(false))
	{

	s3 = 30;
return;	}else
	{

	s3 = -1;
return;	}
	case 28:
	UnityEngine.Debug.Log("PreviousM Destroyed");
	PreviousMButton = (new Nothing<ButtonGUI>());
	s3 = -1;
return;	
	default: return;}}
	

	int s4=-1;
	public void Rule4(float dt, Menu world){ 
	switch (s4)
	{

	case -1:
	if(!(((LessButton.IsSome) && (PlusButton.IsSome))))
	{

	s4 = -1;
return;	}else
	{

	goto case 14;	}
	case 14:
	if(!(((LessButton.Value.IsPressed) || (PlusButton.Value.IsPressed))))
	{

	s4 = 14;
return;	}else
	{

	goto case 13;	}
	case 13:
	___Minus40 = ((AmountOfPlayers) - (1));
	___Plus40 = ((AmountOfPlayers) + (1));
	if(PlusButton.Value.IsPressed)
	{

	goto case 7;	}else
	{

	goto case 0;	}
	case 7:
	if(((4) > (AmountOfPlayers)))
	{

	goto case 9;	}else
	{

	s4 = 0;
return;	}
	case 9:
	UnityEngine.Debug.Log(("Players become ") + (___Plus40));
	AmountOfPlayers = ___Plus40;
	LessButton.Value.IsPressed = false;
	PlusButton.Value.IsPressed = false;
	s4 = 0;
return;
	case 0:
	if(LessButton.Value.IsPressed)
	{

	goto case 1;	}else
	{

	s4 = -1;
return;	}
	case 1:
	if(((AmountOfPlayers) > (1)))
	{

	goto case 3;	}else
	{

	s4 = -1;
return;	}
	case 3:
	UnityEngine.Debug.Log(("Players become ") + (___Minus40));
	AmountOfPlayers = ___Minus40;
	LessButton.Value.IsPressed = false;
	PlusButton.Value.IsPressed = false;
	s4 = -1;
return;	
	default: return;}}
	

	int s5=-1;
	public void Rule5(float dt, Menu world){ 
	switch (s5)
	{

	case -1:
	PlayerAmount = PlayerAmount;
	s5 = -1;
return;	
	default: return;}}
	

	int s6=-1;
	public void Rule6(float dt, Menu world){ 
	switch (s6)
	{

	case -1:
	if(!(((PreviousMButton.IsSome) && (NextMButton.IsSome))))
	{

	s6 = -1;
return;	}else
	{

	goto case 11;	}
	case 11:
	if(!(((PreviousMButton.Value.IsPressed) || (NextMButton.Value.IsPressed))))
	{

	s6 = 11;
return;	}else
	{

	goto case 10;	}
	case 10:
	UnityEngine.Debug.Log(MapSelectNumber);
	if(PreviousMButton.Value.IsPressed)
	{

	goto case 4;	}else
	{

	goto case 0;	}
	case 4:
	if(((MapSelectNumber) == (1)))
	{

	goto case 5;	}else
	{

	goto case 6;	}
	case 5:
	MapSelectNumber = MapSelectNumber;
	PreviousMButton.Value.IsPressed = false;
	NextMButton.Value.IsPressed = false;
	s6 = 0;
return;
	case 6:
	MapSelectNumber = ((MapSelectNumber) - (1));
	PreviousMButton.Value.IsPressed = false;
	NextMButton.Value.IsPressed = false;
	s6 = 0;
return;
	case 0:
	if(NextMButton.Value.IsPressed)
	{

	goto case 1;	}else
	{

	s6 = -1;
return;	}
	case 1:
	MapSelectNumber = ((MapSelectNumber) + (1));
	PreviousMButton.Value.IsPressed = false;
	NextMButton.Value.IsPressed = false;
	s6 = -1;
return;	
	default: return;}}
	

	int s7=-1;
	public void Rule7(float dt, Menu world){ 
	switch (s7)
	{

	case -1:
	UnityEngine.Debug.Log(MapName);
	MapName = MapName;
	s7 = -1;
return;	
	default: return;}}
	

	int s8=-1;
	public void Rule8(float dt, Menu world){ 
	switch (s8)
	{

	case -1:
	MapSelecter = MapSelecter;
	s8 = -1;
return;	
	default: return;}}
	

	int s9=-1;
	public void Rule9(float dt, Menu world){ 
	switch (s9)
	{

	case -1:
	if(!(BackButton.IsPressed))
	{

	s9 = -1;
return;	}else
	{

	goto case 2;	}
	case 2:
	if(((CurrentScreenNumber) == (0)))
	{

	goto case 0;	}else
	{

	goto case 1;	}
	case 0:
	UnityEngine.Debug.Log("Quit");
	CurrentScreenNumber = -1;
	BackButton.IsPressed = false;
	s9 = -1;
return;
	case 1:
	___previous90 = ((CurrentScreenNumber) - (1));
	UnityEngine.Debug.Log(("Going to previous scene") + (___previous90));
	CurrentScreenNumber = ___previous90;
	BackButton.IsPressed = false;
	s9 = -1;
return;	
	default: return;}}
	

	int s10=-1;
	public void Rule10(float dt, Menu world){ 
	switch (s10)
	{

	case -1:
	if(!(StartButton.IsPressed))
	{

	s10 = -1;
return;	}else
	{

	goto case 2;	}
	case 2:
	___next100 = ((CurrentScreenNumber) + (1));
	UnityEngine.Debug.Log(("Going to next scene: ") + (___next100));
	CurrentScreenNumber = ___next100;
	StartButton.IsPressed = false;
	s10 = -1;
return;	
	default: return;}}
	





}
public class ButtonGUI{
public int frame;
public bool JustEntered = true;
private System.String n;
	public int ID;
public ButtonGUI(System.String n)
	{JustEntered = false;
 frame = Menu.frame;
		UnityButton = UnityButton.Find(n);
		
}
		public System.Boolean IsPressed{  get { return UnityButton.IsPressed; }
  set{UnityButton.IsPressed = value; }
 }
	public UnityButton UnityButton;
	public UnityEngine.Animation animation{  get { return UnityButton.animation; }
 }
	public UnityEngine.AudioSource audio{  get { return UnityButton.audio; }
 }
	public UnityEngine.Camera camera{  get { return UnityButton.camera; }
 }
	public UnityEngine.Collider collider{  get { return UnityButton.collider; }
 }
	public UnityEngine.Collider2D collider2D{  get { return UnityButton.collider2D; }
 }
	public UnityEngine.ConstantForce constantForce{  get { return UnityButton.constantForce; }
 }
	public System.Boolean enabled{  get { return UnityButton.enabled; }
  set{UnityButton.enabled = value; }
 }
	public UnityEngine.GameObject gameObject{  get { return UnityButton.gameObject; }
 }
	public UnityEngine.GUIElement guiElement{  get { return UnityButton.guiElement; }
 }
	public UnityEngine.GUIText guiText{  get { return UnityButton.guiText; }
 }
	public UnityEngine.GUITexture guiTexture{  get { return UnityButton.guiTexture; }
 }
	public UnityEngine.HideFlags hideFlags{  get { return UnityButton.hideFlags; }
  set{UnityButton.hideFlags = value; }
 }
	public UnityEngine.HingeJoint hingeJoint{  get { return UnityButton.hingeJoint; }
 }
	public UnityEngine.Light light{  get { return UnityButton.light; }
 }
	public System.String name{  get { return UnityButton.name; }
  set{UnityButton.name = value; }
 }
	public UnityEngine.ParticleEmitter particleEmitter{  get { return UnityButton.particleEmitter; }
 }
	public UnityEngine.ParticleSystem particleSystem{  get { return UnityButton.particleSystem; }
 }
	public UnityEngine.Renderer renderer{  get { return UnityButton.renderer; }
 }
	public UnityEngine.Rigidbody rigidbody{  get { return UnityButton.rigidbody; }
 }
	public UnityEngine.Rigidbody2D rigidbody2D{  get { return UnityButton.rigidbody2D; }
 }
	public System.String tag{  get { return UnityButton.tag; }
  set{UnityButton.tag = value; }
 }
	public UnityEngine.Transform transform{  get { return UnityButton.transform; }
 }
	public System.Boolean useGUILayout{  get { return UnityButton.useGUILayout; }
  set{UnityButton.useGUILayout = value; }
 }
	public void Update(float dt, Menu world) {
frame = Menu.frame;



	}











}
}                                              