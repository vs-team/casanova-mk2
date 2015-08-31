#pragma warning disable 162,108,618
using Casanova.Prelude;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Generic;
using UnityEngine;
namespace Game {public class World : MonoBehaviour{
public static int frame;
void Update () { Update(Time.deltaTime, this); 
 frame++; }
public bool JustEntered = true;


public void Start()
	{
		GameUtils.SetSeed(GameSettings.MapSeed);
		GameConstants ___constants00;
		___constants00 = new GameConstants();
		List<System.Int32> ___symbolSet00;
		if(___constants00.SharedSymbols)
			{
			___symbolSet00 = (

(GameUtils.randomDistinctIntList(10,0,___constants00.SymbolCount)).Select(__ContextSymbol0 => new { ___x00 = __ContextSymbol0 })
.Select(__ContextSymbol1 => __ContextSymbol1.___x00)
.ToList<System.Int32>()).ToList<System.Int32>();
			}else
			{
			___symbolSet00 = (

Enumerable.Empty<System.Int32>()).ToList<System.Int32>();
			}
		List<Player> ___players00;
		___players00 = (

(Enumerable.Range(1,(1) + ((___constants00.PlayerCount) - (1))).ToList<System.Int32>()).Select(__ContextSymbol3 => new { ___i00 = __ContextSymbol3 })
.Select(__ContextSymbol4 => new Player((__ContextSymbol4.___i00) - (1),___constants00,___symbolSet00))
.ToList<Player>()).ToList<Player>();
		System.Int32 ___maxStarSystems00;
		___maxStarSystems00 = UnityEngine.Mathf.Max(___constants00.MaxStarSystems,___constants00.PlayerCount);
		List<UnityEngine.Vector3> ___randomPositions00;
		___randomPositions00 = (

(Enumerable.Range(1,(1) + ((___maxStarSystems00) - (1))).ToList<System.Int32>()).Select(__ContextSymbol5 => new { ___i01 = __ContextSymbol5 })
.Select(__ContextSymbol6 => new {___minr00 = ((___constants00.MinDistanceFromStar) + ((___constants00.MaxPlanetsPerSystem) * (___constants00.MinDistanceBetweenPlanets))) + (((__ContextSymbol6.___i01) - (1)) * (___constants00.MinDistanceBetweenStarSystems)), prev = __ContextSymbol6 })
.Select(__ContextSymbol7 => new {___posx00 = UnityEngine.Random.Range(__ContextSymbol7.___minr00,(__ContextSymbol7.___minr00) + (20f)), prev = __ContextSymbol7 })
.Select(__ContextSymbol8 => new {___posz00 = UnityEngine.Random.Range(__ContextSymbol8.prev.___minr00,(__ContextSymbol8.prev.___minr00) + (20f)), prev = __ContextSymbol8 })
.Select(__ContextSymbol9 => new {___posx01 = Utils.IfThenElse<System.Single>((()=> ((UnityEngine.Random.Range(0f,1f)) > (0.5f))), (()=>	__ContextSymbol9.prev.___posx00
),(()=>	(__ContextSymbol9.prev.___posx00) * (-1f)
)), prev = __ContextSymbol9 })
.Select(__ContextSymbol10 => new {___posz01 = Utils.IfThenElse<System.Single>((()=> ((UnityEngine.Random.Range(0f,1f)) > (0.5f))), (()=>	__ContextSymbol10.prev.___posz00
),(()=>	(__ContextSymbol10.prev.___posz00) * (-1f)
)), prev = __ContextSymbol10 })
.Select(__ContextSymbol11 => new UnityEngine.Vector3(__ContextSymbol11.prev.___posx01,0f,__ContextSymbol11.___posz01))
.ToList<UnityEngine.Vector3>()).ToList<UnityEngine.Vector3>();
		System.Collections.Generic.List<System.Int32> ___randomSystemIndices00;
		___randomSystemIndices00 = GameUtils.randomDistinctIntList(___constants00.PlayerCount,0,___maxStarSystems00);
		List<StarSystem> ___generatedSystems00;
		___generatedSystems00 = (

(Enumerable.Range(1,(1) + ((___randomPositions00.Count) - (1))).ToList<System.Int32>()).Select(__ContextSymbol12 => new { ___i02 = __ContextSymbol12 })
.Select(__ContextSymbol13 => new {___owner00 = Utils.IfThenElse<Option<Player>>((()=> GameUtils.Contains(___randomSystemIndices00,(__ContextSymbol13.___i02) - (1))), (()=>	(new Just<Player>(___players00[GameUtils.FindIndex(___randomSystemIndices00,(__ContextSymbol13.___i02) - (1))]))
),(()=>	(new Nothing<Player>())
)), prev = __ContextSymbol13 })
.Select(__ContextSymbol14 => new StarSystem(___randomPositions00[(__ContextSymbol14.prev.___i02) - (1)],___constants00,__ContextSymbol14.___owner00))
.ToList<StarSystem>()).ToList<StarSystem>();
		Player ___firstPlayer00;
		___firstPlayer00 = ___players00[0];
		WritingLog = false;
		Winner = (new Nothing<Player>());
		TurnDuration = ___constants00.TurnDuration;
		TimerText = new CnvText("UICanvas/TimerCanvas/TimerText");
		StarSystems = ___generatedSystems00;
		SelectedPlanet = (new Nothing<Planet>());
		Players = ___players00;
		Minimap = new Minimap();
		MainCamera = new GameCamera();
		Log = LogGenerator.CreateLog();
		GamePaused = false;
		GameOver = false;
		GameConstants = ___constants00;
		GUI = new GameUI(___constants00,___players00);
		CurrentPlayer = ___firstPlayer00;
		BusyDrones = false;
		
}
		public System.Boolean BusyDrones;
	public Player __CurrentPlayer;
	public Player CurrentPlayer{  get { return  __CurrentPlayer; }
  set{ __CurrentPlayer = value;
 if(!value.JustEntered) __CurrentPlayer = value; 
 else{ value.JustEntered = false;
}
 }
 }
	public GameUI GUI;
	public GameConstants GameConstants;
	public System.Boolean GameOver;
	public System.Boolean GamePaused;
	public LogGenerator Log;
	public GameCamera MainCamera;
	public Minimap Minimap;
	public List<Player> __Players;
	public List<Player> Players{  get { return  __Players; }
  set{ __Players = value;
 foreach(var e in value){if(e.JustEntered){ e.JustEntered = false;
}
} }
 }
	public Option<Planet> __SelectedPlanet;
	public Option<Planet> SelectedPlanet{  get { return  __SelectedPlanet; }
  set{ __SelectedPlanet = value;
 if(value.IsSome){if(!value.Value.JustEntered) __SelectedPlanet = value; 
 else{ value.Value.JustEntered = false;
}
} }
 }
	public List<StarSystem> StarSystems;
	public CnvText TimerText;
	public System.Int32 TurnDuration;
	public Option<Player> __Winner;
	public Option<Player> Winner{  get { return  __Winner; }
  set{ __Winner = value;
 if(value.IsSome){if(!value.Value.JustEntered) __Winner = value; 
 else{ value.Value.JustEntered = false;
}
} }
 }
	public System.Boolean WritingLog;
	public System.Single count_down1;
	public System.Int32 ___remainingTime00;
	public System.Int32 ___currentPlayerIndex00;
	public Player ___currentPlayer00;
	public System.Int32 ___currentPlayerIndex11;
	public Player ___currentPlayer11;
	public Option<Planet> ___prevSelection40;
	public List<Player> ___winnerList50;
	public System.Single count_down2;
	public System.Int32 ___sentUranium100;
	public System.Int32 ___sentPlutonium100;
	public System.Int32 ___sentOil100;
	public System.Int32 ___sentIron100;
	public Player ___receiver100;
	public List<Player> ___receiver111;
	public List<System.Int32> ___images110;
	public List<System.Int32> ___echo110;
	public Player ___receiver112;

System.DateTime init_time = System.DateTime.Now;
	public void Update(float dt, World world) {
var t = System.DateTime.Now;		this.Rule2(dt, world);
		this.Rule3(dt, world);
		GUI.Update(dt, world);
		GameConstants.Update(dt, world);
		MainCamera.Update(dt, world);
		Minimap.Update(dt, world);
		for(int x0 = 0; x0 < Players.Count; x0++) { 
			Players[x0].Update(dt, world);
		}
		for(int x0 = 0; x0 < StarSystems.Count; x0++) { 
			StarSystems[x0].Update(dt, world);
		}
		TimerText.Update(dt, world);
		this.Rule0(dt, world);
		this.Rule1(dt, world);
		this.Rule4(dt, world);
		this.Rule5(dt, world);
		this.Rule6(dt, world);
		this.Rule7(dt, world);
		this.Rule8(dt, world);
		this.Rule9(dt, world);
		this.Rule10(dt, world);
		this.Rule11(dt, world);
		this.Rule12(dt, world);
		this.Rule13(dt, world);
		this.Rule14(dt, world);
	}

	public void Rule2(float dt, World world) 
	{
	List<Drone> ___busyDrones20;
	___busyDrones20 = (

(CurrentPlayer.Drones).Select(__ContextSymbol15 => new { ___drone20 = __ContextSymbol15 })
.Where(__ContextSymbol16 => __ContextSymbol16.___drone20.Busy)
.Select(__ContextSymbol17 => __ContextSymbol17.___drone20)
.ToList<Drone>()).ToList<Drone>();
	BusyDrones = ((___busyDrones20.Count) > (0));
	}
	

	public void Rule3(float dt, World world) 
	{
	TimerText.Text = ((CurrentPlayer.Name) + (" : ")) + (GameUtils.IntToString(TurnDuration));
	}
	



	int s0=-1;
	public void Rule0(float dt, World world){ 
	switch (s0)
	{

	case -1:
	if(!(!(GamePaused)))
	{

	s0 = -1;
return;	}else
	{

	goto case 11;	}
	case 11:
	TurnDuration = TurnDuration;
	CurrentPlayer = CurrentPlayer;
	s0 = 9;
return;
	case 9:
	count_down1 = 1f;
	goto case 10;
	case 10:
	if(((count_down1) > (0f)))
	{

	count_down1 = ((count_down1) - (dt));
	s0 = 10;
return;	}else
	{

	goto case 8;	}
	case 8:
	___remainingTime00 = ((TurnDuration) - (1));
	if(((0) > (___remainingTime00)))
	{

	goto case 0;	}else
	{

	goto case 1;	}
	case 0:
	if(!(!(BusyDrones)))
	{

	s0 = 0;
return;	}else
	{

	goto case 5;	}
	case 5:
	___currentPlayerIndex00 = ((((CurrentPlayer.Index) + (1))) % (Players.Count));
	___currentPlayer00 = (Players)[___currentPlayerIndex00];
	TurnDuration = GameConstants.TurnDuration;
	CurrentPlayer = ___currentPlayer00;
	s0 = -1;
return;
	case 1:
	TurnDuration = ___remainingTime00;
	CurrentPlayer = CurrentPlayer;
	s0 = -1;
return;	
	default: return;}}
	

	int s1=-1;
	public void Rule1(float dt, World world){ 
	switch (s1)
	{

	case -1:
	if(!(this.GUI.QuickBar.SkipTurnButton.Clicked))
	{

	s1 = -1;
return;	}else
	{

	goto case 2;	}
	case 2:
	___currentPlayerIndex11 = ((((CurrentPlayer.Index) + (1))) % (Players.Count));
	___currentPlayer11 = (Players)[___currentPlayerIndex11];
	TurnDuration = GameConstants.TurnDuration;
	CurrentPlayer = ___currentPlayer11;
	s1 = -1;
return;	
	default: return;}}
	

	int s4=-1;
	public void Rule4(float dt, World world){ 
	switch (s4)
	{

	case -1:
	if(!(((GamePaused) && (SelectedPlanet.IsSome))))
	{

	s4 = -1;
return;	}else
	{

	goto case 3;	}
	case 3:
	___prevSelection40 = SelectedPlanet;
	SelectedPlanet = (new Nothing<Planet>());
	s4 = 1;
return;
	case 1:
	if(!(!(GamePaused)))
	{

	s4 = 1;
return;	}else
	{

	goto case 0;	}
	case 0:
	SelectedPlanet = ___prevSelection40;
	s4 = -1;
return;	
	default: return;}}
	

	int s5=-1;
	public void Rule5(float dt, World world){ 
	switch (s5)
	{

	case -1:
	___winnerList50 = (

(Players).Select(__ContextSymbol18 => new { ___player50 = __ContextSymbol18 })
.Select(__ContextSymbol19 => new {___completedObjectives50 = (

(__ContextSymbol19.___player50.Objectives).Select(__ContextSymbol20 => new { ___objective50 = __ContextSymbol20,prev = __ContextSymbol19 })
.Where(__ContextSymbol21 => __ContextSymbol21.___objective50.Completed)
.Select(__ContextSymbol22 => __ContextSymbol22.___objective50)
.ToList<Objective>()).ToList<Objective>(), prev = __ContextSymbol19 })
.Where(__ContextSymbol23 => ((((__ContextSymbol23.prev.___player50.Objectives.Count) > (0))) && (((__ContextSymbol23.prev.___player50.Objectives.Count) == (__ContextSymbol23.___completedObjectives50.Count)))))
.Select(__ContextSymbol24 => __ContextSymbol24.prev.___player50)
.ToList<Player>()).ToList<Player>();
	if(((___winnerList50.Count) > (0)))
	{

	goto case 5;	}else
	{

	goto case 6;	}
	case 5:
	Winner = (new Just<Player>(___winnerList50.Head()));
	s5 = -1;
return;
	case 6:
	Winner = (new Nothing<Player>());
	s5 = -1;
return;	
	default: return;}}
	

	int s6=-1;
	public void Rule6(float dt, World world){ 
	switch (s6)
	{

	case -1:
	if(!(this.GUI.FadeScreen.FadeOutOver))
	{

	s6 = -1;
return;	}else
	{

	goto case 4;	}
	case 4:
	GameOver = true;
	s6 = 2;
return;
	case 2:
	count_down2 = 0.1f;
	goto case 3;
	case 3:
	if(((count_down2) > (0f)))
	{

	count_down2 = ((count_down2) - (dt));
	s6 = 3;
return;	}else
	{

	goto case 1;	}
	case 1:
	if(!(!(Log.WritingLog)))
	{

	s6 = 1;
return;	}else
	{

	goto case 0;	}
	case 0:
	UnityEngine.Application.LoadLevel("menu");
	s6 = -1;
return;	
	default: return;}}
	

	int s7=-1;
	public void Rule7(float dt, World world){ 
	switch (s7)
	{

	case -1:
	if(!(world.GUI.MainMenu.GameMenu.Clicked))
	{

	s7 = -1;
return;	}else
	{

	goto case 2;	}
	case 2:
	Log.WritingLog = true;
	s7 = 1;
return;
	case 1:
	LogGenerator.SaveLog(Log,"Log.csv");
	Log.WritingLog = false;
	s7 = -1;
return;	
	default: return;}}
	

	int s8=-1;
	public void Rule8(float dt, World world){ 
	switch (s8)
	{

	case -1:
	if(!(world.GUI.MainMenu.Exit.Clicked))
	{

	s8 = -1;
return;	}else
	{

	goto case 2;	}
	case 2:
	Log.WritingLog = true;
	s8 = 1;
return;
	case 1:
	LogGenerator.SaveLog(Log,"Log.csv");
	Log.WritingLog = false;
	s8 = -1;
return;	
	default: return;}}
	

	int s9=-1;
	public void Rule9(float dt, World world){ 
	switch (s9)
	{

	case -1:
	if(!(this.GUI.FadeScreen.FadeOutOver))
	{

	s9 = -1;
return;	}else
	{

	goto case 3;	}
	case 3:
	Log.WritingLog = true;
	s9 = 2;
return;
	case 2:
	LogGenerator.SaveLog(Log,"Log.csv");
	Log.WritingLog = false;
	s9 = 0;
return;
	case 0:
	if(!(false))
	{

	s9 = 0;
return;	}else
	{

	s9 = -1;
return;	}	
	default: return;}}
	

	int s10=-1;
	public void Rule10(float dt, World world){ 
	switch (s10)
	{

	case -1:
	if(!(this.GUI.LogMenu.SendButton.Clicked))
	{

	s10 = -1;
return;	}else
	{

	goto case 8;	}
	case 8:
	___sentUranium100 = GameUtils.StringToInt(world.GUI.LogMenu.UraniumInputField.Text);
	___sentPlutonium100 = GameUtils.StringToInt(world.GUI.LogMenu.PlutoniumInputField.Text);
	___sentOil100 = GameUtils.StringToInt(world.GUI.LogMenu.OilInputField.Text);
	___sentIron100 = GameUtils.StringToInt(world.GUI.LogMenu.IronInputField.Text);
	___receiver100 = world.GUI.LogMenu.SelectedMessage.Value.Sender;
	Log.WritingLog = true;
	s10 = 2;
return;
	case 2:
	LogGenerator.Write(Log,((((((((((((((world.CurrentPlayer.Name) + (",")) + ("Send resources,")) + (___receiver100.Name)) + (",")) + ("Uranium: ")) + (GameUtils.IntToString(___sentUranium100))) + (" Plutonium: ")) + (GameUtils.IntToString(___sentPlutonium100))) + (" Oil: ")) + (GameUtils.IntToString(___sentOil100))) + (" Iron: ")) + (GameUtils.IntToString(___sentIron100))) + (",")) + (GameUtils.Date()));
	Log.WritingLog = false;
	s10 = 0;
return;
	case 0:
	if(!(!(this.GUI.LogMenu.SendButton.Clicked)))
	{

	s10 = 0;
return;	}else
	{

	s10 = -1;
return;	}	
	default: return;}}
	

	int s11=-1;
	public void Rule11(float dt, World world){ 
	switch (s11)
	{

	case -1:
	if(!(this.GUI.MessageMenu.SendButton.Clicked))
	{

	s11 = -1;
return;	}else
	{

	goto case 7;	}
	case 7:
	___receiver111 = (

(world.Players).Select(__ContextSymbol25 => new { ___player111 = __ContextSymbol25 })
.Where(__ContextSymbol26 => ((__ContextSymbol26.___player111.Name) == (world.GUI.MessageMenu.Receivers.SelectionName)))
.Select(__ContextSymbol27 => __ContextSymbol27.___player111)
.ToList<Player>()).ToList<Player>();
	___images110 = (

(world.GUI.MessageMenu.ImageSelection).Select(__ContextSymbol28 => new { ___image110 = __ContextSymbol28 })
.Where(__ContextSymbol29 => __ContextSymbol29.___image110.Selected)
.Select(__ContextSymbol30 => __ContextSymbol30.___image110.ImageIndex)
.ToList<System.Int32>()).ToList<System.Int32>();
	___echo110 = (

(world.GUI.MessageMenu.MessageEcho).Select(__ContextSymbol31 => new { ___e110 = __ContextSymbol31 })
.Select(__ContextSymbol32 => __ContextSymbol32.___e110)
.ToList<System.Int32>()).ToList<System.Int32>();
	___receiver112 = ___receiver111.Head();
	Log.WritingLog = true;
	s11 = 2;
return;
	case 2:
	LogGenerator.Write(Log,((((((((((world.CurrentPlayer.Name) + (",")) + ("Send message,")) + (___receiver112.Name)) + (",")) + ("Message = ")) + (GameUtils.IntListToString(___echo110))) + (" Images = ")) + (GameUtils.IntListToString(___images110))) + (",")) + (GameUtils.Date()));
	Log.WritingLog = false;
	s11 = 0;
return;
	case 0:
	if(!(!(this.GUI.MessageMenu.SendButton.Clicked)))
	{

	s11 = 0;
return;	}else
	{

	s11 = -1;
return;	}	
	default: return;}}
	

	int s12=-1;
	public void Rule12(float dt, World world){ 
	switch (s12)
	{

	case -1:
	if(!(this.GUI.PlanetMenu.ScanForResources.Clicked))
	{

	s12 = -1;
return;	}else
	{

	goto case 3;	}
	case 3:
	Log.WritingLog = true;
	s12 = 2;
return;
	case 2:
	LogGenerator.Write(Log,(((world.CurrentPlayer.Name) + (",")) + ("Scan planet,None,None,")) + (GameUtils.Date()));
	Log.WritingLog = false;
	s12 = 0;
return;
	case 0:
	if(!(!(this.GUI.PlanetMenu.ScanForResources.Clicked)))
	{

	s12 = 0;
return;	}else
	{

	s12 = -1;
return;	}	
	default: return;}}
	

	int s13=-1;
	public void Rule13(float dt, World world){ 
	switch (s13)
	{

	case -1:
	if(!(this.GUI.PlanetMenu.Mine.Clicked))
	{

	s13 = -1;
return;	}else
	{

	goto case 3;	}
	case 3:
	Log.WritingLog = true;
	s13 = 2;
return;
	case 2:
	LogGenerator.Write(Log,(((world.CurrentPlayer.Name) + (",")) + ("Mine resources,None,None,")) + (GameUtils.Date()));
	Log.WritingLog = false;
	s13 = 0;
return;
	case 0:
	if(!(!(this.GUI.PlanetMenu.Mine.Clicked)))
	{

	s13 = 0;
return;	}else
	{

	s13 = -1;
return;	}	
	default: return;}}
	

	int s14=-1;
	public void Rule14(float dt, World world){ 
	switch (s14)
	{

	case -1:
	if(!(this.GUI.PlanetMenu.CancelOrders.Clicked))
	{

	s14 = -1;
return;	}else
	{

	goto case 3;	}
	case 3:
	Log.WritingLog = true;
	s14 = 2;
return;
	case 2:
	LogGenerator.Write(Log,(((world.CurrentPlayer.Name) + (",")) + ("Cancel drone order,None,None,")) + (GameUtils.Date()));
	Log.WritingLog = false;
	s14 = 0;
return;
	case 0:
	if(!(!(this.GUI.PlanetMenu.CancelOrders.Clicked)))
	{

	s14 = 0;
return;	}else
	{

	s14 = -1;
return;	}	
	default: return;}}
	





}
public class GameConstants{
public int frame;
public bool JustEntered = true;
	public int ID;
public GameConstants()
	{JustEntered = false;
 frame = World.frame;
		UraniumConversionValue = 10;
		TurnDuration = GameSettings.TurnDuration;
		TotalObjectiveResources = GameSettings.WinTotalResources;
		SymbolCount = 30;
		SharedSymbols = true;
		PreviewIconSize = 40f;
		PlutoniumConversionValue = 20;
		PlayerSymbolCount = 4;
		PlayerCount = GameSettings.PlayerCount;
		OilConversionValue = 1;
		MiningRate = GameSettings.MiningRate;
		MinPlanetsPerSystem = 3;
		MinDistanceFromStar = 3f;
		MinDistanceBetweenStarSystems = 25f;
		MinDistanceBetweenPlanets = 5f;
		MessagePreviewSize = 21;
		MessageMaximumCost = GameUtils.IntToFloat(GameSettings.MaxMessageCost);
		MaximumStartingResources = GameSettings.StartingResources;
		MaxStarSystems = GameSettings.MapSize;
		MaxPlanetsPerSystem = 6;
		IronConversionValue = 3;
		ImageSelectionSize = 4;
		
}
		public System.Int32 ImageSelectionSize;
	public System.Int32 IronConversionValue;
	public System.Int32 MaxPlanetsPerSystem;
	public System.Int32 MaxStarSystems;
	public System.Int32 MaximumStartingResources;
	public System.Single MessageMaximumCost;
	public System.Int32 MessagePreviewSize;
	public System.Single MinDistanceBetweenPlanets;
	public System.Single MinDistanceBetweenStarSystems;
	public System.Single MinDistanceFromStar;
	public System.Int32 MinPlanetsPerSystem;
	public System.Int32 MiningRate;
	public System.Int32 OilConversionValue;
	public System.Int32 PlayerCount;
	public System.Int32 PlayerSymbolCount;
	public System.Int32 PlutoniumConversionValue;
	public System.Single PreviewIconSize;
	public System.Boolean SharedSymbols;
	public System.Int32 SymbolCount;
	public System.Int32 TotalObjectiveResources;
	public System.Int32 TurnDuration;
	public System.Int32 UraniumConversionValue;
	public void Update(float dt, World world) {
frame = World.frame;



	}











}
public class GameUI{
public int frame;
public bool JustEntered = true;
private GameConstants constants;
private List<Player> players;
	public int ID;
public GameUI(GameConstants constants, List<Player> players)
	{JustEntered = false;
 frame = World.frame;
		VictoryWindow = new VictoryWindow();
		ResourceBar = new ResourceBar();
		QuickBar = new QuickBar();
		PlanetMenu = new PlanetMenu();
		OnGUI = false;
		ObjectiveWindow = new ObjectiveWindow();
		MessageMenu = new MessageMenu(constants,players);
		MainMenu = new MainMenu();
		LogMenu = new LogMenu(constants,players);
		FadeScreen = new FadeScreen();
		
}
		public FadeScreen FadeScreen;
	public LogMenu LogMenu;
	public MainMenu MainMenu;
	public MessageMenu MessageMenu;
	public ObjectiveWindow ObjectiveWindow;
	public System.Boolean OnGUI;
	public PlanetMenu PlanetMenu;
	public QuickBar QuickBar;
	public ResourceBar ResourceBar;
	public VictoryWindow VictoryWindow;
	public void Update(float dt, World world) {
frame = World.frame;		this.Rule0(dt, world);

		FadeScreen.Update(dt, world);
		LogMenu.Update(dt, world);
		MainMenu.Update(dt, world);
		MessageMenu.Update(dt, world);
		ObjectiveWindow.Update(dt, world);
		PlanetMenu.Update(dt, world);
		QuickBar.Update(dt, world);
		ResourceBar.Update(dt, world);
		VictoryWindow.Update(dt, world);


	}

	public void Rule0(float dt, World world) 
	{
	OnGUI = ((((((((((MainMenu.Active) && (MainMenu.OnMenu))) || (((PlanetMenu.Active) && (PlanetMenu.OnMenu))))) || (((MessageMenu.Active) && (MessageMenu.OnMenu))))) || (((LogMenu.Active) && (LogMenu.OnMenu))))) || (((QuickBar.Active) && (QuickBar.OnMenu))));
	}
	










}
public class SelectableImage{
public int frame;
public bool JustEntered = true;
private System.String n;
private System.Int32 imageIndex;
	public int ID;
public SelectableImage(System.String n, System.Int32 imageIndex)
	{JustEntered = false;
 frame = World.frame;
		Selected = false;
		ImageIndex = imageIndex;
		CnvSelectionBox = new CnvSelectionBox(n);
		
}
		public CnvSelectionBox CnvSelectionBox;
	public System.Int32 ImageIndex;
	public System.Boolean Selected;
	public System.String ___stringIndex20;
	public void Update(float dt, World world) {
frame = World.frame;

		this.Rule0(dt, world);
		this.Rule1(dt, world);
		this.Rule2(dt, world);
		this.Rule3(dt, world);
		this.Rule4(dt, world);
	}





	int s0=-1;
	public void Rule0(float dt, World world){ 
	switch (s0)
	{

	case -1:
	if(!(((((world.GameConstants.ImageSelectionSize) > (ImageIndex))) && (CnvSelectionBox.Clicked))))
	{

	s0 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	Selected = !(Selected);
	CnvSelectionBox.Clicked = false;
	s0 = -1;
return;	
	default: return;}}
	

	int s1=-1;
	public void Rule1(float dt, World world){ 
	switch (s1)
	{

	case -1:
	if(!(((((world.GameConstants.ImageSelectionSize) > (ImageIndex))) && (!(Selected)))))
	{

	s1 = -1;
return;	}else
	{

	goto case 1;	}
	case 1:
	CnvSelectionBox.ImageName = (("Images/image") + (GameUtils.IntToString((ImageIndex) + (1))));
	s1 = 0;
return;
	case 0:
	if(!(Selected))
	{

	s1 = 0;
return;	}else
	{

	s1 = -1;
return;	}	
	default: return;}}
	

	int s2=-1;
	public void Rule2(float dt, World world){ 
	switch (s2)
	{

	case -1:
	if(!(((((world.GameConstants.ImageSelectionSize) > (ImageIndex))) && (Selected))))
	{

	s2 = -1;
return;	}else
	{

	goto case 2;	}
	case 2:
	___stringIndex20 = (("Images/image") + (GameUtils.IntToString((ImageIndex) + (1))));
	CnvSelectionBox.ImageName = ((___stringIndex20) + ("_selected"));
	s2 = 0;
return;
	case 0:
	if(!(!(Selected)))
	{

	s2 = 0;
return;	}else
	{

	s2 = -1;
return;	}	
	default: return;}}
	

	int s3=-1;
	public void Rule3(float dt, World world){ 
	switch (s3)
	{

	case -1:
	CnvSelectionBox.Active = ((world.GameConstants.ImageSelectionSize) > (ImageIndex));
	s3 = 0;
return;
	case 0:
	if(!(((((ImageIndex) > (world.GameConstants.ImageSelectionSize))) || (((ImageIndex) == (world.GameConstants.ImageSelectionSize))))))
	{

	s3 = 0;
return;	}else
	{

	s3 = -1;
return;	}	
	default: return;}}
	

	int s4=-1;
	public void Rule4(float dt, World world){ 
	switch (s4)
	{

	case -1:
	if(!(((world.GUI.MessageMenu.CloseButton.Clicked) || (world.GUI.MessageMenu.ClearButton.Clicked))))
	{

	s4 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	Selected = false;
	s4 = -1;
return;	
	default: return;}}
	





}
public class KeyboardButton{
public int frame;
public bool JustEntered = true;
private System.String n;
private System.Int32 symbolIndex;
	public int ID;
public KeyboardButton(System.String n, System.Int32 symbolIndex)
	{JustEntered = false;
 frame = World.frame;
		SymbolIndex = symbolIndex;
		CnvButton = new CnvButton(n);
		
}
		public CnvButton CnvButton;
	public System.Int32 SymbolIndex;
	public Player ___currentPlayer02;
	public System.Collections.Generic.List<System.Int32> ___playerSymbols00;
	public System.Int32 ___newImageIndex00;
	public System.String ___stringIndex01;
	public System.String ___newImageName00;
	public System.Collections.Generic.List<System.Int32> ___playerSymbols11;
	public void Update(float dt, World world) {
frame = World.frame;

		this.Rule0(dt, world);
		this.Rule1(dt, world);
		this.Rule2(dt, world);
	}





	int s0=-1;
	public void Rule0(float dt, World world){ 
	switch (s0)
	{

	case -1:
	if(!(((world.GameConstants.PlayerSymbolCount) > (SymbolIndex))))
	{

	s0 = -1;
return;	}else
	{

	goto case 6;	}
	case 6:
	___currentPlayer02 = world.CurrentPlayer;
	___playerSymbols00 = ___currentPlayer02.Symbols;
	___newImageIndex00 = (___playerSymbols00)[SymbolIndex];
	___stringIndex01 = GameUtils.IntToString((___newImageIndex00) + (1));
	___newImageName00 = (("Symbols/symbol") + (___stringIndex01));
	CnvButton.ImageName = ___newImageName00;
	s0 = 0;
return;
	case 0:
	if(!(!(((___currentPlayer02) == (world.CurrentPlayer)))))
	{

	s0 = 0;
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
	if(!(((((world.GameConstants.PlayerSymbolCount) > (SymbolIndex))) && (CnvButton.Clicked))))
	{

	s1 = -1;
return;	}else
	{

	goto case 2;	}
	case 2:
	if(!(((world.GUI.MessageMenu.MessageEcho.Count) > (((world.GameConstants.MessagePreviewSize) - (1))))))
	{

	goto case 0;	}else
	{

	goto case 1;	}
	case 0:
	___playerSymbols11 = world.CurrentPlayer.Symbols;
	world.GUI.MessageMenu.MessageEcho = (world.GUI.MessageMenu.MessageEcho).Concat((

(new Cons<System.Int32>(___playerSymbols11[SymbolIndex],(new Empty<System.Int32>()).ToList<System.Int32>())).ToList<System.Int32>()).ToList<System.Int32>()).ToList<System.Int32>();
	CnvButton.Clicked = false;
	s1 = -1;
return;
	case 1:
	world.GUI.MessageMenu.MessageEcho = world.GUI.MessageMenu.MessageEcho;
	CnvButton.Clicked = false;
	s1 = -1;
return;	
	default: return;}}
	

	int s2=-1;
	public void Rule2(float dt, World world){ 
	switch (s2)
	{

	case -1:
	CnvButton.Active = ((world.GameConstants.PlayerSymbolCount) > (SymbolIndex));
	s2 = 0;
return;
	case 0:
	if(!(((((SymbolIndex) > (world.GameConstants.PlayerSymbolCount))) || (((SymbolIndex) == (world.GameConstants.PlayerSymbolCount))))))
	{

	s2 = 0;
return;	}else
	{

	s2 = -1;
return;	}	
	default: return;}}
	





}
public class PreviewIcon{
public int frame;
public bool JustEntered = true;
private System.String n;
private System.Int32 index;
private Option<IconInstantiation> instantiate;
	public int ID;
public PreviewIcon(System.String n, System.Int32 index, Option<IconInstantiation> instantiate)
	{JustEntered = false;
 frame = World.frame;
		IconIndex = index;
		CnvIcon = new CnvIcon(n,instantiate);
		
}
		public CnvIcon CnvIcon;
	public System.Int32 IconIndex;
	public List<System.Int32> ___echo11;
	public System.Int32 ___symbolIndex10;
	public System.String ___spriteName10;
	public void Update(float dt, World world) {
frame = World.frame;

		this.Rule0(dt, world);
		this.Rule1(dt, world);
		this.Rule2(dt, world);
	}





	int s0=-1;
	public void Rule0(float dt, World world){ 
	switch (s0)
	{

	case -1:
	if(!(((((IconIndex) > (world.GUI.MessageMenu.MessageEcho.Count))) || (((IconIndex) == (world.GUI.MessageMenu.MessageEcho.Count))))))
	{

	s0 = -1;
return;	}else
	{

	goto case 1;	}
	case 1:
	CnvIcon.Active = false;
	s0 = 0;
return;
	case 0:
	if(!(((world.GUI.MessageMenu.MessageEcho.Count) > (IconIndex))))
	{

	s0 = 0;
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
	if(!(((world.GUI.MessageMenu.MessageEcho.Count) > (IconIndex))))
	{

	s1 = -1;
return;	}else
	{

	goto case 4;	}
	case 4:
	___echo11 = world.GUI.MessageMenu.MessageEcho;
	___symbolIndex10 = (___echo11)[IconIndex];
	___spriteName10 = (("Symbols/symbol") + (GameUtils.IntToString((___symbolIndex10) + (1))));
	CnvIcon.ImageName = ___spriteName10;
	CnvIcon.Active = true;
	s1 = 0;
return;
	case 0:
	if(!(((((IconIndex) > (world.GUI.MessageMenu.MessageEcho.Count))) || (((IconIndex) == (world.GUI.MessageMenu.MessageEcho.Count))))))
	{

	s1 = 0;
return;	}else
	{

	s1 = -1;
return;	}	
	default: return;}}
	

	int s2=-1;
	public void Rule2(float dt, World world){ 
	switch (s2)
	{

	case -1:
	CnvIcon.Active = false;
	s2 = 0;
return;
	case 0:
	if(!(false))
	{

	s2 = 0;
return;	}else
	{

	s2 = -1;
return;	}	
	default: return;}}
	





}
public class FadeScreen{
public int frame;
public bool JustEntered = true;
	public int ID;
public FadeScreen()
	{JustEntered = false;
 frame = World.frame;
		FadePanel = new CnvPanel("FadeCanvas/FadeScreen");
		FadeOutOver = false;
		
}
		public System.Boolean FadeOutOver;
	public CnvPanel FadePanel;
	public System.Single ___lifetime01;
	public void Update(float dt, World world) {
frame = World.frame;

		FadePanel.Update(dt, world);
		this.Rule0(dt, world);

	}





	int s0=-1;
	public void Rule0(float dt, World world){ 
	switch (s0)
	{

	case -1:
	if(!(world.GUI.VictoryWindow.VictoryCinematicOver))
	{

	s0 = -1;
return;	}else
	{

	goto case 5;	}
	case 5:
	___lifetime01 = 2f;
	goto case 2;
	case 2:
	if(!(((1f) > (FadePanel.Alpha))))
	{

	goto case 1;	}else
	{

	goto case 3;	}
	case 3:
	FadePanel.Alpha = ((FadePanel.Alpha) + (((((1f) / (___lifetime01))) * (dt))));
	FadeOutOver = false;
	s0 = 2;
return;
	case 1:
	FadePanel.Alpha = 1f;
	FadeOutOver = true;
	s0 = 0;
return;
	case 0:
	if(!(false))
	{

	s0 = 0;
return;	}else
	{

	s0 = -1;
return;	}	
	default: return;}}
	






}
public class VictoryWindow{
public int frame;
public bool JustEntered = true;
	public int ID;
public VictoryWindow()
	{JustEntered = false;
 frame = World.frame;
		VictoryText = new CnvText("UICanvas/VictoryCanvas/Text");
		VictoryCinematicOver = false;
		
}
		public System.Boolean VictoryCinematicOver;
	public CnvText VictoryText;
	public System.Single ___lifetime02;
	public System.Single count_down4;
	public System.Single count_down3;
	public void Update(float dt, World world) {
frame = World.frame;

		VictoryText.Update(dt, world);
		this.Rule0(dt, world);
		this.Rule1(dt, world);
	}





	int s0=-1;
	public void Rule0(float dt, World world){ 
	switch (s0)
	{

	case -1:
	if(!(world.Winner.IsSome))
	{

	s0 = -1;
return;	}else
	{

	goto case 15;	}
	case 15:
	___lifetime02 = 0.5f;
	VictoryText.Text = ((world.Winner.Value.Name) + (" has won the game!"));
	VictoryText.Alpha = 0f;
	VictoryCinematicOver = false;
	s0 = 11;
return;
	case 11:
	if(!(((1f) > (VictoryText.Alpha))))
	{

	goto case 10;	}else
	{

	goto case 12;	}
	case 12:
	VictoryText.Text = VictoryText.Text;
	VictoryText.Alpha = ((VictoryText.Alpha) + (((((1f) / (___lifetime02))) * (dt))));
	VictoryCinematicOver = false;
	s0 = 11;
return;
	case 10:
	VictoryText.Text = VictoryText.Text;
	VictoryText.Alpha = 1f;
	VictoryCinematicOver = false;
	s0 = 8;
return;
	case 8:
	count_down4 = 1f;
	goto case 9;
	case 9:
	if(((count_down4) > (0f)))
	{

	count_down4 = ((count_down4) - (dt));
	s0 = 9;
return;	}else
	{

	goto case 5;	}
	case 5:
	if(!(((VictoryText.Alpha) > (0f))))
	{

	goto case 4;	}else
	{

	goto case 6;	}
	case 6:
	VictoryText.Text = VictoryText.Text;
	VictoryText.Alpha = ((VictoryText.Alpha) - (((((1f) / (___lifetime02))) * (dt))));
	VictoryCinematicOver = false;
	s0 = 5;
return;
	case 4:
	VictoryText.Text = VictoryText.Text;
	VictoryText.Alpha = 0f;
	VictoryCinematicOver = false;
	s0 = 2;
return;
	case 2:
	count_down3 = 0.5f;
	goto case 3;
	case 3:
	if(((count_down3) > (0f)))
	{

	count_down3 = ((count_down3) - (dt));
	s0 = 3;
return;	}else
	{

	goto case 1;	}
	case 1:
	VictoryText.Text = VictoryText.Text;
	VictoryText.Alpha = VictoryText.Alpha;
	VictoryCinematicOver = true;
	s0 = 0;
return;
	case 0:
	if(!(false))
	{

	s0 = 0;
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
	VictoryText.Alpha = 0f;
	s1 = 0;
return;
	case 0:
	if(!(false))
	{

	s1 = 0;
return;	}else
	{

	s1 = -1;
return;	}	
	default: return;}}
	





}
public class ObjectiveWindow{
public int frame;
public bool JustEntered = true;
	public int ID;
public ObjectiveWindow()
	{JustEntered = false;
 frame = World.frame;
		ObjectiveText = new CnvText("UICanvas/ObjectiveCanvas/ObjectiveText");
		
}
		public CnvText ObjectiveText;
	public System.Single ___lifetime03;
	public List<System.String> ___textList00;
	public System.String ___objectiveText00;
	public System.Single count_down5;
	public Player ___currentPlayer13;
	public void Update(float dt, World world) {
frame = World.frame;

		ObjectiveText.Update(dt, world);
		this.Rule0(dt, world);
		this.Rule1(dt, world);
		this.Rule2(dt, world);
	}





	int s0=-1;
	public void Rule0(float dt, World world){ 
	switch (s0)
	{

	case -1:
	___lifetime03 = 0.5f;
	goto case 15;
	case 15:
	if(!(UnityEngine.Input.GetKeyDown(KeyCode.O)))
	{

	s0 = 15;
return;	}else
	{

	goto case 14;	}
	case 14:
	___textList00 = (

(world.CurrentPlayer.Objectives).Select(__ContextSymbol34 => new { ___objective01 = __ContextSymbol34 })
.Select(__ContextSymbol35 => __ContextSymbol35.___objective01.Text)
.ToList<System.String>()).ToList<System.String>();
	___objectiveText00 = GameUtils.ConcatenateStrings(___textList00);
	if(((ObjectiveText.Alpha) == (0f)))
	{

	goto case 0;	}else
	{

	goto case 1;	}
	case 0:
	ObjectiveText.Text = ___objectiveText00;
	ObjectiveText.Alpha = 0f;
	s0 = 4;
return;
	case 4:
	if(!(((1f) > (ObjectiveText.Alpha))))
	{

	goto case 3;	}else
	{

	goto case 5;	}
	case 5:
	ObjectiveText.Text = ObjectiveText.Text;
	ObjectiveText.Alpha = ((ObjectiveText.Alpha) + (((((1f) / (___lifetime03))) * (dt))));
	s0 = 4;
return;
	case 3:
	ObjectiveText.Text = ObjectiveText.Text;
	ObjectiveText.Alpha = 1f;
	s0 = -1;
return;
	case 1:
	ObjectiveText.Text = ___objectiveText00;
	ObjectiveText.Alpha = ObjectiveText.Alpha;
	s0 = 9;
return;
	case 9:
	if(!(((ObjectiveText.Alpha) > (0f))))
	{

	goto case 8;	}else
	{

	goto case 10;	}
	case 10:
	ObjectiveText.Text = ObjectiveText.Text;
	ObjectiveText.Alpha = ((ObjectiveText.Alpha) - (((((1f) / (___lifetime03))) * (dt))));
	s0 = 9;
return;
	case 8:
	ObjectiveText.Text = ObjectiveText.Text;
	ObjectiveText.Alpha = 0f;
	s0 = -1;
return;	
	default: return;}}
	

	int s1=-1;
	public void Rule1(float dt, World world){ 
	switch (s1)
	{

	case -1:
	count_down5 = 0f;
	goto case 4;
	case 4:
	if(((count_down5) > (0f)))
	{

	count_down5 = ((count_down5) - (dt));
	s1 = 4;
return;	}else
	{

	goto case 2;	}
	case 2:
	___currentPlayer13 = world.CurrentPlayer;
	goto case 1;
	case 1:
	if(!(!(((___currentPlayer13) == (world.CurrentPlayer)))))
	{

	s1 = 1;
return;	}else
	{

	goto case 0;	}
	case 0:
	ObjectiveText.Alpha = 0f;
	s1 = -1;
return;	
	default: return;}}
	

	int s2=-1;
	public void Rule2(float dt, World world){ 
	switch (s2)
	{

	case -1:
	ObjectiveText.Alpha = 0f;
	s2 = 0;
return;
	case 0:
	if(!(false))
	{

	s2 = 0;
return;	}else
	{

	s2 = -1;
return;	}	
	default: return;}}
	





}
public class QuickBar{
public int frame;
public bool JustEntered = true;
	public int ID;
public QuickBar()
	{JustEntered = false;
 frame = World.frame;
		UnityMenu = UnityMenu.GetMenu("UICanvas/QuickBarCanvas");
		SkipTurnButton = new CnvButton("UICanvas/QuickBarCanvas/SkipTurnButton");
		MessageButton = new CnvButton("UICanvas/QuickBarCanvas/MessageButton");
		LogButton = new CnvButton("UICanvas/QuickBarCanvas/LogButton");
		HomeButton = new CnvButton("UICanvas/QuickBarCanvas/HomeButton");
		DroneButton = new CnvButton("UICanvas/QuickBarCanvas/DroneButton");
		
}
		public System.Boolean Active{  get { return UnityMenu.Active; }
  set{UnityMenu.Active = value; }
 }
	public CnvButton DroneButton;
	public CnvButton HomeButton;
	public CnvButton LogButton;
	public CnvButton MessageButton;
	public System.Boolean OnMenu{  get { return UnityMenu.OnMenu; }
  set{UnityMenu.OnMenu = value; }
 }
	public CnvButton SkipTurnButton;
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
	public UnityEngine.Vector3 ___cameraPosition10;
	public Player ___currentPlayer24;
	public UnityEngine.Vector3 ___cameraPosition61;
	public void Update(float dt, World world) {
frame = World.frame;		this.Rule0(dt, world);
		this.Rule3(dt, world);
		DroneButton.Update(dt, world);
		HomeButton.Update(dt, world);
		LogButton.Update(dt, world);
		MessageButton.Update(dt, world);
		SkipTurnButton.Update(dt, world);
		this.Rule1(dt, world);
		this.Rule2(dt, world);
		this.Rule4(dt, world);
		this.Rule5(dt, world);
		this.Rule6(dt, world);
	}

	public void Rule0(float dt, World world) 
	{
	List<Message> ___playerMessages00;
	___playerMessages00 = world.CurrentPlayer.ReceivedMessages;
	List<Message> ___unreadMessages00;
	___unreadMessages00 = (

(___playerMessages00).Select(__ContextSymbol36 => new { ___message00 = __ContextSymbol36 })
.Where(__ContextSymbol37 => !(__ContextSymbol37.___message00.Read))
.Select(__ContextSymbol38 => __ContextSymbol38.___message00)
.ToList<Message>()).ToList<Message>();
	System.String ___spriteName01;
	if(((___unreadMessages00.Count) > (0)))
		{
		___spriteName01 = "ResourcesUI/envelope_new";
		}else
		{
		___spriteName01 = "ResourcesUI/envelope";
		}
	LogButton.ImageName = ___spriteName01;
	}
	

	public void Rule3(float dt, World world) 
	{
	List<Drone> ___busyDrones31;
	___busyDrones31 = (

(world.CurrentPlayer.Drones).Select(__ContextSymbol39 => new { ___drone31 = __ContextSymbol39 })
.Where(__ContextSymbol40 => __ContextSymbol40.___drone31.Busy)
.Select(__ContextSymbol41 => __ContextSymbol41.___drone31)
.ToList<Drone>()).ToList<Drone>();
	SkipTurnButton.Disabled = ((___busyDrones31.Count) > (0));
	}
	



	int s1=-1;
	public void Rule1(float dt, World world){ 
	switch (s1)
	{

	case -1:
	if(!(DroneButton.Clicked))
	{

	s1 = -1;
return;	}else
	{

	goto case 2;	}
	case 2:
	___cameraPosition10 = world.MainCamera.CameraPosition;
	goto case 1;
	case 1:
	if(!(!(((___cameraPosition10) == (world.MainCamera.CameraPosition)))))
	{

	s1 = 1;
return;	}else
	{

	goto case 0;	}
	case 0:
	DroneButton.Clicked = false;
	s1 = -1;
return;	
	default: return;}}
	

	int s2=-1;
	public void Rule2(float dt, World world){ 
	switch (s2)
	{

	case -1:
	if(!(SkipTurnButton.Clicked))
	{

	s2 = -1;
return;	}else
	{

	goto case 2;	}
	case 2:
	___currentPlayer24 = world.CurrentPlayer;
	goto case 1;
	case 1:
	if(!(!(((___currentPlayer24) == (world.CurrentPlayer)))))
	{

	s2 = 1;
return;	}else
	{

	goto case 0;	}
	case 0:
	SkipTurnButton.Clicked = false;
	s2 = -1;
return;	
	default: return;}}
	

	int s4=-1;
	public void Rule4(float dt, World world){ 
	switch (s4)
	{

	case -1:
	if(!(((MessageButton.Clicked) && (world.GUI.MessageMenu.Active))))
	{

	s4 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	MessageButton.Clicked = false;
	s4 = -1;
return;	
	default: return;}}
	

	int s5=-1;
	public void Rule5(float dt, World world){ 
	switch (s5)
	{

	case -1:
	if(!(((LogButton.Clicked) && (world.GUI.LogMenu.Active))))
	{

	s5 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	LogButton.Clicked = false;
	s5 = -1;
return;	
	default: return;}}
	

	int s6=-1;
	public void Rule6(float dt, World world){ 
	switch (s6)
	{

	case -1:
	if(!(HomeButton.Clicked))
	{

	s6 = -1;
return;	}else
	{

	goto case 2;	}
	case 2:
	___cameraPosition61 = world.MainCamera.CameraPosition;
	goto case 1;
	case 1:
	if(!(!(((___cameraPosition61) == (world.MainCamera.CameraPosition)))))
	{

	s6 = 1;
return;	}else
	{

	goto case 0;	}
	case 0:
	HomeButton.Clicked = false;
	s6 = -1;
return;	
	default: return;}}
	





}
public class TextLogIcon{
public int frame;
public bool JustEntered = true;
private System.String n;
private System.Int32 index;
private Option<IconInstantiation> instantiate;
	public int ID;
public TextLogIcon(System.String n, System.Int32 index, Option<IconInstantiation> instantiate)
	{JustEntered = false;
 frame = World.frame;
		IconIndex = index;
		CnvIcon = new CnvIcon(n,instantiate);
		
}
		public CnvIcon CnvIcon;
	public System.Int32 IconIndex;
	public Message ___selectedMessage10;
	public List<System.Int32> ___messageContent10;
	public System.Int32 ___symbolIndex11;
	public System.String ___spriteName12;
	public void Update(float dt, World world) {
frame = World.frame;

		this.Rule0(dt, world);
		this.Rule1(dt, world);
		this.Rule2(dt, world);
	}





	int s0=-1;
	public void Rule0(float dt, World world){ 
	switch (s0)
	{

	case -1:
	if(!(((world.GUI.LogMenu.SelectedMessage.IsNone) || (((((IconIndex) > (world.GUI.LogMenu.SelectedMessage.Value.MessageContent.Count))) || (((IconIndex) == (world.GUI.LogMenu.SelectedMessage.Value.MessageContent.Count))))))))
	{

	s0 = -1;
return;	}else
	{

	goto case 1;	}
	case 1:
	CnvIcon.Active = false;
	s0 = 0;
return;
	case 0:
	if(!(((world.GUI.LogMenu.SelectedMessage.IsSome) && (((world.GUI.LogMenu.SelectedMessage.Value.MessageContent.Count) > (IconIndex))))))
	{

	s0 = 0;
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
	if(!(((world.GUI.LogMenu.SelectedMessage.IsSome) && (((world.GUI.LogMenu.SelectedMessage.Value.MessageContent.Count) > (IconIndex))))))
	{

	s1 = -1;
return;	}else
	{

	goto case 5;	}
	case 5:
	___selectedMessage10 = world.GUI.LogMenu.SelectedMessage.Value;
	___messageContent10 = ___selectedMessage10.MessageContent;
	___symbolIndex11 = (___messageContent10)[IconIndex];
	___spriteName12 = (("Symbols/symbol") + (GameUtils.IntToString((___symbolIndex11) + (1))));
	CnvIcon.ImageName = ___spriteName12;
	CnvIcon.Active = true;
	s1 = 0;
return;
	case 0:
	if(!(((world.GUI.LogMenu.SelectedMessage.IsNone) || (!(((___selectedMessage10) == (world.GUI.LogMenu.SelectedMessage.Value)))))))
	{

	s1 = 0;
return;	}else
	{

	s1 = -1;
return;	}	
	default: return;}}
	

	int s2=-1;
	public void Rule2(float dt, World world){ 
	switch (s2)
	{

	case -1:
	CnvIcon.Active = false;
	s2 = 0;
return;
	case 0:
	if(!(false))
	{

	s2 = 0;
return;	}else
	{

	s2 = -1;
return;	}	
	default: return;}}
	





}
public class ImageLogIcon{
public int frame;
public bool JustEntered = true;
private System.String n;
private System.Int32 index;
private Option<IconInstantiation> instantiate;
	public int ID;
public ImageLogIcon(System.String n, System.Int32 index, Option<IconInstantiation> instantiate)
	{JustEntered = false;
 frame = World.frame;
		IconIndex = index;
		CnvIcon = new CnvIcon(n,instantiate);
		
}
		public CnvIcon CnvIcon;
	public System.Int32 IconIndex;
	public Message ___selectedMessage11;
	public List<System.Int32> ___images11;
	public System.Int32 ___symbolIndex12;
	public System.String ___spriteName13;
	public void Update(float dt, World world) {
frame = World.frame;

		this.Rule0(dt, world);
		this.Rule1(dt, world);
		this.Rule2(dt, world);
	}





	int s0=-1;
	public void Rule0(float dt, World world){ 
	switch (s0)
	{

	case -1:
	if(!(((world.GUI.LogMenu.SelectedMessage.IsNone) || (((((IconIndex) > (world.GUI.LogMenu.SelectedMessage.Value.Images.Count))) || (((IconIndex) == (world.GUI.LogMenu.SelectedMessage.Value.Images.Count))))))))
	{

	s0 = -1;
return;	}else
	{

	goto case 1;	}
	case 1:
	CnvIcon.Active = false;
	s0 = 0;
return;
	case 0:
	if(!(((world.GUI.LogMenu.SelectedMessage.IsSome) && (((world.GUI.LogMenu.SelectedMessage.Value.Images.Count) > (IconIndex))))))
	{

	s0 = 0;
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
	if(!(((world.GUI.LogMenu.SelectedMessage.IsSome) && (((world.GUI.LogMenu.SelectedMessage.Value.Images.Count) > (IconIndex))))))
	{

	s1 = -1;
return;	}else
	{

	goto case 5;	}
	case 5:
	___selectedMessage11 = world.GUI.LogMenu.SelectedMessage.Value;
	___images11 = ___selectedMessage11.Images;
	___symbolIndex12 = (___images11)[IconIndex];
	___spriteName13 = (("Images/image") + (GameUtils.IntToString((___symbolIndex12) + (1))));
	CnvIcon.ImageName = ___spriteName13;
	CnvIcon.Active = true;
	s1 = 0;
return;
	case 0:
	if(!(((world.GUI.LogMenu.SelectedMessage.IsNone) || (!(((___selectedMessage11) == (world.GUI.LogMenu.SelectedMessage.Value)))))))
	{

	s1 = 0;
return;	}else
	{

	s1 = -1;
return;	}	
	default: return;}}
	

	int s2=-1;
	public void Rule2(float dt, World world){ 
	switch (s2)
	{

	case -1:
	CnvIcon.Active = false;
	s2 = 0;
return;
	case 0:
	if(!(false))
	{

	s2 = 0;
return;	}else
	{

	s2 = -1;
return;	}	
	default: return;}}
	





}
public class LogMenu{
public int frame;
public bool JustEntered = true;
private GameConstants constants;
private List<Player> players;
	public int ID;
public LogMenu(GameConstants constants, List<Player> players)
	{JustEntered = false;
 frame = World.frame;
		CnvFrame ___messageFrame00;
		___messageFrame00 = new CnvFrame("UICanvas/LogWindowCanvas/FrameCanvas/MessagePreviewFrame/Background",(new Just<System.Single>(5f)),(new Just<System.Single>(5f)));
		CnvFrame ___imageFrame00;
		___imageFrame00 = new CnvFrame("UICanvas/LogWindowCanvas/FrameCanvas/ImagePreviewFrame/Background",(new Just<System.Single>(5f)),(new Just<System.Single>(5f)));
		System.Int32 ___symbolsPerLine00;
		___symbolsPerLine00 = GameUtils.FloatToInt(UnityEngine.Mathf.Floor(((___messageFrame00.Width) - ((2f) * (___messageFrame00.OffsetX))) / (40f)));
		System.Int32 ___imagesPerLine00;
		___imagesPerLine00 = GameUtils.FloatToInt(UnityEngine.Mathf.Floor(((___messageFrame00.Width) - ((2f) * (___messageFrame00.OffsetX))) / (40f)));
		UnityEngine.Vector2 ___messageOrigin00;
		___messageOrigin00 = ___messageFrame00.Origin;
		UnityEngine.Vector2 ___imageOrigin00;
		___imageOrigin00 = ___imageFrame00.Origin;
		List<TextLogIcon> ___messagePreview00;
		___messagePreview00 = (

(Enumerable.Range(1,(1) + ((constants.MessagePreviewSize) - (1))).ToList<System.Int32>()).Select(__ContextSymbol43 => new { ___i03 = __ContextSymbol43 })
.Select(__ContextSymbol44 => new {___spriteName04 = "Symbols/symbol1", prev = __ContextSymbol44 })
.Select(__ContextSymbol45 => new {___cellX00 = ((__ContextSymbol45.prev.___i03) - (1)) % (___symbolsPerLine00), prev = __ContextSymbol45 })
.Select(__ContextSymbol46 => new {___cellY00 = ((__ContextSymbol46.prev.prev.___i03) - (1)) / (___symbolsPerLine00), prev = __ContextSymbol46 })
.Select(__ContextSymbol47 => new {___offsetX01 = ((___messageOrigin00.x) + (___messageFrame00.OffsetX)) + ((__ContextSymbol47.prev.___cellX00) * (constants.PreviewIconSize)), prev = __ContextSymbol47 })
.Select(__ContextSymbol48 => new {___offsetY01 = (___messageOrigin00.y) - ((___messageFrame00.OffsetY) + ((__ContextSymbol48.prev.___cellY00) * (constants.PreviewIconSize))), prev = __ContextSymbol48 })
.Select(__ContextSymbol49 => new {___parentPath00 = "UICanvas/LogWindowCanvas/FrameCanvas/MessagePreviewFrame", prev = __ContextSymbol49 })
.Select(__ContextSymbol50 => new {___instantiationParams00 = new IconInstantiation(new UnityEngine.Vector3(__ContextSymbol50.prev.prev.___offsetX01,__ContextSymbol50.prev.___offsetY01,0f),__ContextSymbol50.prev.prev.prev.prev.prev.___spriteName04,__ContextSymbol50.___parentPath00), prev = __ContextSymbol50 })
.Select(__ContextSymbol51 => new TextLogIcon("SymbolIcon",(__ContextSymbol51.prev.prev.prev.prev.prev.prev.prev.___i03) - (1),(new Just<IconInstantiation>(__ContextSymbol51.___instantiationParams00))))
.ToList<TextLogIcon>()).ToList<TextLogIcon>();
		System.Int32 ___symbolsPerLine01;
		___symbolsPerLine01 = GameUtils.FloatToInt(UnityEngine.Mathf.Floor(((___imageFrame00.Width) - ((2f) * (___imageFrame00.OffsetX))) / (40f)));
		System.Int32 ___imagesPerLine01;
		___imagesPerLine01 = GameUtils.FloatToInt(UnityEngine.Mathf.Floor(((___imageFrame00.Width) - ((2f) * (___imageFrame00.OffsetX))) / (40f)));
		List<ImageLogIcon> ___imagePreview00;
		___imagePreview00 = (

(Enumerable.Range(1,(1) + ((constants.ImageSelectionSize) - (1))).ToList<System.Int32>()).Select(__ContextSymbol52 => new { ___i04 = __ContextSymbol52 })
.Select(__ContextSymbol53 => new {___spriteName05 = "Images/image1", prev = __ContextSymbol53 })
.Select(__ContextSymbol54 => new {___cellX01 = ((__ContextSymbol54.prev.___i04) - (1)) % (___symbolsPerLine01), prev = __ContextSymbol54 })
.Select(__ContextSymbol55 => new {___cellY01 = ((__ContextSymbol55.prev.prev.___i04) - (1)) / (___symbolsPerLine01), prev = __ContextSymbol55 })
.Select(__ContextSymbol56 => new {___offsetX02 = ((___imageOrigin00.x) + (___imageFrame00.OffsetX)) + ((__ContextSymbol56.prev.___cellX01) * (constants.PreviewIconSize)), prev = __ContextSymbol56 })
.Select(__ContextSymbol57 => new {___offsetY02 = (___imageOrigin00.y) - ((___imageFrame00.OffsetY) + ((__ContextSymbol57.prev.___cellY01) * (constants.PreviewIconSize))), prev = __ContextSymbol57 })
.Select(__ContextSymbol58 => new {___parentPath01 = "UICanvas/LogWindowCanvas/FrameCanvas/ImagePreviewFrame", prev = __ContextSymbol58 })
.Select(__ContextSymbol59 => new {___instantiationParams01 = new IconInstantiation(new UnityEngine.Vector3(__ContextSymbol59.prev.prev.___offsetX02,__ContextSymbol59.prev.___offsetY02,0f),__ContextSymbol59.prev.prev.prev.prev.prev.___spriteName05,__ContextSymbol59.___parentPath01), prev = __ContextSymbol59 })
.Select(__ContextSymbol60 => new ImageLogIcon("ImageIcon",(__ContextSymbol60.prev.prev.prev.prev.prev.prev.prev.___i04) - (1),(new Just<IconInstantiation>(__ContextSymbol60.___instantiationParams01))))
.ToList<ImageLogIcon>()).ToList<ImageLogIcon>();
		UraniumInputField = new CnvInputField("UICanvas/LogWindowCanvas/UraniumInputField");
		UnityMenu = UnityMenu.GetMenu("UICanvas/LogWindowCanvas");
		Trades = (

Enumerable.Empty<Trade>()).ToList<Trade>();
		SendButton = new CnvButton("UICanvas/LogWindowCanvas/SendButton");
		SelectedMessage = (new Nothing<Message>());
		PlutoniumInputField = new CnvInputField("UICanvas/LogWindowCanvas/PlutoniumInputField");
		OilInputField = new CnvInputField("UICanvas/LogWindowCanvas/OilInputField");
		MessagePreview = ___messagePreview00;
		MessageList = new CnvComboList("UICanvas/LogWindowCanvas/MessageLog");
		IronInputField = new CnvInputField("UICanvas/LogWindowCanvas/IronInputField");
		ImagePreview = ___imagePreview00;
		CloseButton = new CnvButton("UICanvas/LogWindowCanvas/CloseButton");
		
}
		public System.Boolean Active{  get { return UnityMenu.Active; }
  set{UnityMenu.Active = value; }
 }
	public CnvButton CloseButton;
	public List<ImageLogIcon> ImagePreview;
	public CnvInputField IronInputField;
	public CnvComboList MessageList;
	public List<TextLogIcon> MessagePreview;
	public CnvInputField OilInputField;
	public System.Boolean OnMenu{  get { return UnityMenu.OnMenu; }
  set{UnityMenu.OnMenu = value; }
 }
	public CnvInputField PlutoniumInputField;
	public Option<Message> SelectedMessage;
	public CnvButton SendButton;
	public List<Trade> Trades;
	public UnityMenu UnityMenu;
	public CnvInputField UraniumInputField;
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
	public System.String ___messageName30;
	public List<Message> ___selectedMessageList30;
	public Message ___selectedMessage32;
	public List<Message> ___currentMessages40;
	public Message ___message42;
	public System.Int32 counter34;
	public System.Int32 ___sentUranium51;
	public System.Int32 ___sentPlutonium51;
	public System.Int32 ___sentOil51;
	public System.Int32 ___sentIron51;
	public Player ___receiver53;
	public GameResources ___sentResources50;
	public Trade ___newTrade50;
	public System.Single count_down6;
	public List<Trade> ___playerTrades60;
	public List<Trade> ___filteredTrades60;
	public System.Int32 ___playerOil80;
	public System.Int32 ___playerIron90;
	public System.Int32 ___playerPlutonium100;
	public System.Int32 ___playerUranium110;
	public void Update(float dt, World world) {
frame = World.frame;		this.Rule7(dt, world);

		CloseButton.Update(dt, world);
		for(int x0 = 0; x0 < ImagePreview.Count; x0++) { 
			ImagePreview[x0].Update(dt, world);
		}
		IronInputField.Update(dt, world);
		MessageList.Update(dt, world);
		for(int x0 = 0; x0 < MessagePreview.Count; x0++) { 
			MessagePreview[x0].Update(dt, world);
		}
		OilInputField.Update(dt, world);
		PlutoniumInputField.Update(dt, world);
		SendButton.Update(dt, world);
		for(int x0 = 0; x0 < Trades.Count; x0++) { 
			Trades[x0].Update(dt, world);
		}
		UraniumInputField.Update(dt, world);
		this.Rule0(dt, world);
		this.Rule1(dt, world);
		this.Rule2(dt, world);
		this.Rule3(dt, world);
		this.Rule4(dt, world);
		this.Rule5(dt, world);
		this.Rule6(dt, world);
		this.Rule8(dt, world);
		this.Rule9(dt, world);
		this.Rule10(dt, world);
		this.Rule11(dt, world);
		this.Rule12(dt, world);
	}

	public void Rule7(float dt, World world) 
	{
	SendButton.Disabled = ((SelectedMessage.IsNone) || (((((((!(((GameUtils.StringToInt(UraniumInputField.Text)) > (0)))) && (!(((GameUtils.StringToInt(PlutoniumInputField.Text)) > (0)))))) && (!(((GameUtils.StringToInt(OilInputField.Text)) > (0)))))) && (!(((GameUtils.StringToInt(IronInputField.Text)) > (0)))))));
	}
	




	int s0=-1;
	public void Rule0(float dt, World world){ 
	switch (s0)
	{

	case -1:
	if(!(world.GUI.QuickBar.LogButton.Clicked))
	{

	s0 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	Active = true;
	s0 = -1;
return;	
	default: return;}}
	

	int s1=-1;
	public void Rule1(float dt, World world){ 
	switch (s1)
	{

	case -1:
	if(!(CloseButton.Clicked))
	{

	s1 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	Active = false;
	CloseButton.Clicked = false;
	s1 = -1;
return;	
	default: return;}}
	

	int s2=-1;
	public void Rule2(float dt, World world){ 
	switch (s2)
	{

	case -1:
	if(!(((MessageList.SelectionName) == (""))))
	{

	s2 = -1;
return;	}else
	{

	goto case 1;	}
	case 1:
	SelectedMessage = (new Nothing<Message>());
	s2 = 0;
return;
	case 0:
	if(!(!(((MessageList.SelectionName) == ("")))))
	{

	s2 = 0;
return;	}else
	{

	s2 = -1;
return;	}	
	default: return;}}
	

	int s3=-1;
	public void Rule3(float dt, World world){ 
	switch (s3)
	{

	case -1:
	if(!(!(((MessageList.SelectionName) == ("")))))
	{

	s3 = -1;
return;	}else
	{

	goto case 4;	}
	case 4:
	___messageName30 = MessageList.SelectionName;
	___selectedMessageList30 = (

(world.CurrentPlayer.ReceivedMessages).Select(__ContextSymbol62 => new { ___message31 = __ContextSymbol62 })
.Where(__ContextSymbol63 => ((__ContextSymbol63.___message31.Name) == (MessageList.SelectionName)))
.Select(__ContextSymbol64 => __ContextSymbol64.___message31)
.ToList<Message>()).ToList<Message>();
	___selectedMessage32 = ___selectedMessageList30.Head();
	SelectedMessage = (new Just<Message>(___selectedMessage32));
	s3 = 0;
return;
	case 0:
	if(!(!(((___messageName30) == (MessageList.SelectionName)))))
	{

	s3 = 0;
return;	}else
	{

	s3 = -1;
return;	}	
	default: return;}}
	

	int s4=-1;
	public void Rule4(float dt, World world){ 
	switch (s4)
	{

	case -1:
	ComboList.Reset(MessageList.ComboList);
	___currentMessages40 = world.CurrentPlayer.ReceivedMessages;
	
	counter34 = -1;
	if((((___currentMessages40).Count) == (0)))
	{

	goto case 1;	}else
	{

	___message42 = (___currentMessages40)[0];
	goto case 3;	}
	case 3:
	counter34 = ((counter34) + (1));
	if((((((___currentMessages40).Count) == (counter34))) || (((counter34) > ((___currentMessages40).Count)))))
	{

	goto case 1;	}else
	{

	___message42 = (___currentMessages40)[counter34];
	goto case 4;	}
	case 4:
	ComboList.AddButton(MessageList.ComboList,___message42.Name);
	s4 = 3;
return;
	case 1:
	MessageList = MessageList;
	s4 = 0;
return;
	case 0:
	if(!(!(((___currentMessages40) == (world.CurrentPlayer.ReceivedMessages)))))
	{

	s4 = 0;
return;	}else
	{

	s4 = -1;
return;	}	
	default: return;}}
	

	int s5=-1;
	public void Rule5(float dt, World world){ 
	switch (s5)
	{

	case -1:
	if(!(SendButton.Clicked))
	{

	s5 = -1;
return;	}else
	{

	goto case 9;	}
	case 9:
	___sentUranium51 = GameUtils.StringToInt(UraniumInputField.Text);
	___sentPlutonium51 = GameUtils.StringToInt(PlutoniumInputField.Text);
	___sentOil51 = GameUtils.StringToInt(OilInputField.Text);
	___sentIron51 = GameUtils.StringToInt(IronInputField.Text);
	___receiver53 = SelectedMessage.Value.Sender;
	___sentResources50 = new GameResources(___sentUranium51,___sentPlutonium51,___sentOil51,___sentIron51);
	___newTrade50 = new Trade(___receiver53,___sentResources50);
	count_down6 = dt;
	goto case 2;
	case 2:
	if(((count_down6) > (0f)))
	{

	count_down6 = ((count_down6) - (dt));
	s5 = 2;
return;	}else
	{

	goto case 0;	}
	case 0:
	Trades = new Cons<Trade>(___newTrade50, (Trades)).ToList<Trade>();
	SendButton.Clicked = false;
	s5 = -1;
return;	
	default: return;}}
	

	int s6=-1;
	public void Rule6(float dt, World world){ 
	switch (s6)
	{

	case -1:
	if(!(!(world.Log.WritingLog)))
	{

	s6 = -1;
return;	}else
	{

	goto case 2;	}
	case 2:
	___playerTrades60 = (

(world.Players).Select(__ContextSymbol65 => new { ___player62 = __ContextSymbol65 })
.SelectMany(__ContextSymbol66=> (__ContextSymbol66.___player62.ReceivedTrades).Select(__ContextSymbol67 => new { ___trade60 = __ContextSymbol67,
                                                      prev = __ContextSymbol66 })
.Select(__ContextSymbol68 => __ContextSymbol68.___trade60)
.ToList<Trade>())).ToList<Trade>();
	___filteredTrades60 = (

(Trades).Select(__ContextSymbol69 => new { ___t160 = __ContextSymbol69 })
.Select(__ContextSymbol70 => new {___receivedTrades60 = (

(___playerTrades60).Select(__ContextSymbol71 => new { ___t260 = __ContextSymbol71,prev = __ContextSymbol70 })
.Where(__ContextSymbol72 => ((__ContextSymbol72.prev.___t160) == (__ContextSymbol72.___t260)))
.Select(__ContextSymbol73 => __ContextSymbol73.prev.___t160)
.ToList<Trade>()).ToList<Trade>(), prev = __ContextSymbol70 })
.Where(__ContextSymbol74 => ((__ContextSymbol74.___receivedTrades60.Count) == (0)))
.Select(__ContextSymbol75 => __ContextSymbol75.prev.___t160)
.ToList<Trade>()).ToList<Trade>();
	Trades = ___filteredTrades60;
	s6 = -1;
return;	
	default: return;}}
	

	int s8=-1;
	public void Rule8(float dt, World world){ 
	switch (s8)
	{

	case -1:
	___playerOil80 = world.CurrentPlayer.Resources.Oil;
	OilInputField.MaxValue = ___playerOil80;
	OilInputField.MinValue = 0;
	s8 = 0;
return;
	case 0:
	if(!(!(((___playerOil80) == (world.CurrentPlayer.Resources.Oil)))))
	{

	s8 = 0;
return;	}else
	{

	s8 = -1;
return;	}	
	default: return;}}
	

	int s9=-1;
	public void Rule9(float dt, World world){ 
	switch (s9)
	{

	case -1:
	___playerIron90 = world.CurrentPlayer.Resources.Iron;
	IronInputField.MaxValue = ___playerIron90;
	IronInputField.MinValue = 0;
	s9 = 0;
return;
	case 0:
	if(!(!(((___playerIron90) == (world.CurrentPlayer.Resources.Iron)))))
	{

	s9 = 0;
return;	}else
	{

	s9 = -1;
return;	}	
	default: return;}}
	

	int s10=-1;
	public void Rule10(float dt, World world){ 
	switch (s10)
	{

	case -1:
	___playerPlutonium100 = world.CurrentPlayer.Resources.Plutonium;
	PlutoniumInputField.MaxValue = ___playerPlutonium100;
	PlutoniumInputField.MinValue = 0;
	s10 = 0;
return;
	case 0:
	if(!(!(((___playerPlutonium100) == (world.CurrentPlayer.Resources.Plutonium)))))
	{

	s10 = 0;
return;	}else
	{

	s10 = -1;
return;	}	
	default: return;}}
	

	int s11=-1;
	public void Rule11(float dt, World world){ 
	switch (s11)
	{

	case -1:
	___playerUranium110 = world.CurrentPlayer.Resources.Uranium;
	UraniumInputField.MaxValue = ___playerUranium110;
	UraniumInputField.MinValue = 0;
	s11 = 0;
return;
	case 0:
	if(!(!(((___playerUranium110) == (world.CurrentPlayer.Resources.Uranium)))))
	{

	s11 = 0;
return;	}else
	{

	s11 = -1;
return;	}	
	default: return;}}
	

	int s12=-1;
	public void Rule12(float dt, World world){ 
	switch (s12)
	{

	case -1:
	Active = false;
	s12 = 0;
return;
	case 0:
	if(!(false))
	{

	s12 = 0;
return;	}else
	{

	s12 = -1;
return;	}	
	default: return;}}
	





}
public class MessageMenu{
public int frame;
public bool JustEntered = true;
private GameConstants constants;
private List<Player> players;
	public int ID;
public MessageMenu(GameConstants constants, List<Player> players)
	{JustEntered = false;
 frame = World.frame;
		CnvFrame ___messageFrame01;
		___messageFrame01 = new CnvFrame("UICanvas/MessageWindowCanvas/FrameCanvas/MessagePreviewFrame/Background",(new Just<System.Single>(5f)),(new Just<System.Single>(5f)));
		System.Int32 ___symbolsPerLine02;
		___symbolsPerLine02 = GameUtils.FloatToInt(UnityEngine.Mathf.Floor(((___messageFrame01.Width) - ((2f) * (___messageFrame01.OffsetX))) / (40f)));
		UnityEngine.Vector2 ___origin00;
		___origin00 = ___messageFrame01.Origin;
		List<KeyboardButton> ___keyboard00;
		___keyboard00 = (

(Enumerable.Range(1,(1) + ((10) - (1))).ToList<System.Int32>()).Select(__ContextSymbol79 => new { ___i06 = __ContextSymbol79 })
.Select(__ContextSymbol80 => new {___name00 = ("UICanvas/MessageWindowCanvas/KeyboardButton") + (GameUtils.IntToString((__ContextSymbol80.___i06) - (1))), prev = __ContextSymbol80 })
.Select(__ContextSymbol81 => new KeyboardButton(__ContextSymbol81.___name00,(__ContextSymbol81.prev.___i06) - (1)))
.ToList<KeyboardButton>()).ToList<KeyboardButton>();
		List<SelectableImage> ___imageSelection00;
		___imageSelection00 = (

(Enumerable.Range(1,(1) + ((40) - (1))).ToList<System.Int32>()).Select(__ContextSymbol82 => new { ___i07 = __ContextSymbol82 })
.Select(__ContextSymbol83 => new {___name01 = ("UICanvas/MessageWindowCanvas/Image") + (GameUtils.IntToString((__ContextSymbol83.___i07) - (1))), prev = __ContextSymbol83 })
.Select(__ContextSymbol84 => new SelectableImage(__ContextSymbol84.___name01,(__ContextSymbol84.prev.___i07) - (1)))
.ToList<SelectableImage>()).ToList<SelectableImage>();
		List<PreviewIcon> ___preview00;
		___preview00 = (

(Enumerable.Range(1,(1) + ((constants.MessagePreviewSize) - (1))).ToList<System.Int32>()).Select(__ContextSymbol85 => new { ___i08 = __ContextSymbol85 })
.Select(__ContextSymbol86 => new {___spriteName06 = "Symbols/symbol1", prev = __ContextSymbol86 })
.Select(__ContextSymbol87 => new {___cellX02 = ((__ContextSymbol87.prev.___i08) - (1)) % (___symbolsPerLine02), prev = __ContextSymbol87 })
.Select(__ContextSymbol88 => new {___cellY02 = ((__ContextSymbol88.prev.prev.___i08) - (1)) / (___symbolsPerLine02), prev = __ContextSymbol88 })
.Select(__ContextSymbol89 => new {___offsetX03 = ((___origin00.x) + (___messageFrame01.OffsetX)) + ((__ContextSymbol89.prev.___cellX02) * (constants.PreviewIconSize)), prev = __ContextSymbol89 })
.Select(__ContextSymbol90 => new {___offsetY03 = (___origin00.y) - ((___messageFrame01.OffsetY) + ((__ContextSymbol90.prev.___cellY02) * (constants.PreviewIconSize))), prev = __ContextSymbol90 })
.Select(__ContextSymbol91 => new {___parentPath02 = "UICanvas/MessageWindowCanvas/FrameCanvas/MessagePreviewFrame", prev = __ContextSymbol91 })
.Select(__ContextSymbol92 => new {___instantiationParams02 = new IconInstantiation(new UnityEngine.Vector3(__ContextSymbol92.prev.prev.___offsetX03,__ContextSymbol92.prev.___offsetY03,0f),__ContextSymbol92.prev.prev.prev.prev.prev.___spriteName06,__ContextSymbol92.___parentPath02), prev = __ContextSymbol92 })
.Select(__ContextSymbol93 => new PreviewIcon("SymbolIcon",(__ContextSymbol93.prev.prev.prev.prev.prev.prev.prev.___i08) - (1),(new Just<IconInstantiation>(__ContextSymbol93.___instantiationParams02))))
.ToList<PreviewIcon>()).ToList<PreviewIcon>();
		List<System.String> ___playerNames00;
		___playerNames00 = (

(players).Select(__ContextSymbol94 => new { ___p01 = __ContextSymbol94 })
.Select(__ContextSymbol95 => __ContextSymbol95.___p01.Name)
.ToList<System.String>()).ToList<System.String>();
		UraniumInputField = new CnvInputField("UICanvas/MessageWindowCanvas/UraniumInputField");
		UnityMenu = UnityMenu.GetMenu("UICanvas/MessageWindowCanvas");
		SendButton = new CnvButton("UICanvas/MessageWindowCanvas/SendButton");
		Receivers = new CnvComboBox("UICanvas/MessageWindowCanvas/Receivers");
		PlutoniumInputField = new CnvInputField("UICanvas/MessageWindowCanvas/PlutoniumInputField");
		Payment = 0f;
		OutgoingMessages = (

Enumerable.Empty<Message>()).ToList<Message>();
		OilInputField = new CnvInputField("UICanvas/MessageWindowCanvas/OilInputField");
		MessagePreview = ___preview00;
		MessageFrame = ___messageFrame01;
		MessageEcho = (

Enumerable.Empty<System.Int32>()).ToList<System.Int32>();
		KeyboardButtons = ___keyboard00;
		IronInputField = new CnvInputField("UICanvas/MessageWindowCanvas/IronInputField");
		ImageSelection = ___imageSelection00;
		DeleteButton = new CnvButton("UICanvas/MessageWindowCanvas/DeleteButton");
		CurrentSelection = (

Enumerable.Empty<System.Int32>()).ToList<System.Int32>();
		CostIcon = new CnvIcon("UICanvas/MessageWindowCanvas/MessageCostIcon",(new Nothing<IconInstantiation>()));
		Cost = 0f;
		CloseButton = new CnvButton("UICanvas/MessageWindowCanvas/CloseButton");
		ClearButton = new CnvButton("UICanvas/MessageWindowCanvas/ClearButton");
		
}
		public System.Boolean Active{  get { return UnityMenu.Active; }
  set{UnityMenu.Active = value; }
 }
	public CnvButton ClearButton;
	public CnvButton CloseButton;
	public System.Single Cost;
	public CnvIcon CostIcon;
	public List<System.Int32> CurrentSelection;
	public CnvButton DeleteButton;
	public List<SelectableImage> ImageSelection;
	public CnvInputField IronInputField;
	public List<KeyboardButton> KeyboardButtons;
	public List<System.Int32> MessageEcho;
	public CnvFrame MessageFrame;
	public List<PreviewIcon> MessagePreview;
	public CnvInputField OilInputField;
	public System.Boolean OnMenu{  get { return UnityMenu.OnMenu; }
  set{UnityMenu.OnMenu = value; }
 }
	public List<Message> OutgoingMessages;
	public System.Single Payment;
	public CnvInputField PlutoniumInputField;
	public CnvComboBox Receivers;
	public CnvButton SendButton;
	public UnityMenu UnityMenu;
	public CnvInputField UraniumInputField;
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
	public List<System.Int32> ___modifiedEcho00;
	public List<System.Int32> ___currentEcho10;
	public System.Single ___currentPayment10;
	public List<System.Int32> ___currentSelection10;
	public System.Single ___cost10;
	public System.String ___uraniumInput20;
	public System.String ___plutoniumInput20;
	public System.String ___ironInput20;
	public System.String ___oilInput20;
	public System.Single ___uraniumPayment20;
	public System.Single ___plutoniumPayment20;
	public System.Single ___ironPayment20;
	public System.Single ___oilPayment20;
	public System.Int32 ___playerOil31;
	public System.Int32 ___playerIron41;
	public System.Int32 ___playerPlutonium51;
	public System.Int32 ___playerUranium61;
	public System.Single ___currentPayment71;
	public System.Single ___currentCost70;
	public System.String ___receiver74;
	public System.String ___messageName81;
	public List<Player> ___receiver85;
	public List<System.Int32> ___images82;
	public Player ___receiver86;
	public Message ___newMessage80;
	public System.Single count_down7;
	public Player ___currentPlayer115;
	public List<Player> ___players121;
	public Player ___p120;
	public System.Int32 counter312;
	public List<Message> ___playerMessages131;
	public List<Message> ___filteredMessages130;
	public void Update(float dt, World world) {
frame = World.frame;		this.Rule14(dt, world);

		ClearButton.Update(dt, world);
		CloseButton.Update(dt, world);
		CostIcon.Update(dt, world);
		DeleteButton.Update(dt, world);
		for(int x0 = 0; x0 < ImageSelection.Count; x0++) { 
			ImageSelection[x0].Update(dt, world);
		}
		IronInputField.Update(dt, world);
		for(int x0 = 0; x0 < KeyboardButtons.Count; x0++) { 
			KeyboardButtons[x0].Update(dt, world);
		}
		MessageFrame.Update(dt, world);
		for(int x0 = 0; x0 < MessagePreview.Count; x0++) { 
			MessagePreview[x0].Update(dt, world);
		}
		OilInputField.Update(dt, world);
		for(int x0 = 0; x0 < OutgoingMessages.Count; x0++) { 
			OutgoingMessages[x0].Update(dt, world);
		}
		PlutoniumInputField.Update(dt, world);
		Receivers.Update(dt, world);
		SendButton.Update(dt, world);
		UraniumInputField.Update(dt, world);
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
		this.Rule11(dt, world);
		this.Rule12(dt, world);
		this.Rule13(dt, world);
		this.Rule15(dt, world);
		this.Rule16(dt, world);
	}

	public void Rule14(float dt, World world) 
	{
	CurrentSelection = (

(ImageSelection).Select(__ContextSymbol99 => new { ___image142 = __ContextSymbol99 })
.Where(__ContextSymbol100 => __ContextSymbol100.___image142.Selected)
.Select(__ContextSymbol101 => __ContextSymbol101.___image142.ImageIndex)
.ToList<System.Int32>()).ToList<System.Int32>();
	}
	




	int s0=-1;
	public void Rule0(float dt, World world){ 
	switch (s0)
	{

	case -1:
	if(!(DeleteButton.Clicked))
	{

	s0 = -1;
return;	}else
	{

	goto case 2;	}
	case 2:
	if(((MessageEcho.Count) > (0)))
	{

	goto case 0;	}else
	{

	goto case 1;	}
	case 0:
	___modifiedEcho00 = (

(Enumerable.Range(1,(1) + (((MessageEcho.Count) - (1)) - (1))).ToList<System.Int32>()).Select(__ContextSymbol102 => new { ___i05 = __ContextSymbol102 })
.Select(__ContextSymbol103 => MessageEcho[__ContextSymbol103.___i05])
.ToList<System.Int32>()).ToList<System.Int32>();
	MessageEcho = ___modifiedEcho00;
	DeleteButton.Clicked = false;
	s0 = -1;
return;
	case 1:
	MessageEcho = MessageEcho;
	DeleteButton.Clicked = false;
	s0 = -1;
return;	
	default: return;}}
	

	int s1=-1;
	public void Rule1(float dt, World world){ 
	switch (s1)
	{

	case -1:
	___currentEcho10 = MessageEcho;
	___currentPayment10 = Payment;
	___currentSelection10 = CurrentSelection;
	if(((((MessageEcho.Count) == (0))) || (((___currentSelection10.Count) == (0)))))
	{

	___cost10 = 0f;	}else
	{

	___cost10 = UnityEngine.Mathf.Round((world.GameConstants.MessageMaximumCost) / (GameUtils.Factorial(___currentSelection10.Count)));	}
	CostIcon.Text = ((((GameUtils.FloatToString(Payment)) + ("/"))) + (GameUtils.FloatToString(___cost10)));
	Cost = ___cost10;
	s1 = 0;
return;
	case 0:
	if(!(((((!(((___currentEcho10) == (MessageEcho)))) || (!(((___currentSelection10) == (CurrentSelection)))))) || (!(((___currentPayment10) == (Payment)))))))
	{

	s1 = 0;
return;	}else
	{

	s1 = -1;
return;	}	
	default: return;}}
	

	int s2=-1;
	public void Rule2(float dt, World world){ 
	switch (s2)
	{

	case -1:
	___uraniumInput20 = UraniumInputField.Text;
	___plutoniumInput20 = PlutoniumInputField.Text;
	___ironInput20 = IronInputField.Text;
	___oilInput20 = OilInputField.Text;
	___uraniumPayment20 = GameUtils.StringToFloat(___uraniumInput20);
	___plutoniumPayment20 = GameUtils.StringToFloat(___plutoniumInput20);
	___ironPayment20 = GameUtils.StringToFloat(___ironInput20);
	___oilPayment20 = GameUtils.StringToFloat(___oilInput20);
	Payment = ((((((___uraniumPayment20) + (___plutoniumPayment20))) + (___ironPayment20))) + (___oilPayment20));
	s2 = 0;
return;
	case 0:
	if(!(((((((!(((___uraniumInput20) == (UraniumInputField.Text)))) || (!(((___plutoniumInput20) == (PlutoniumInputField.Text)))))) || (!(((___ironInput20) == (IronInputField.Text)))))) || (!(((___oilInput20) == (OilInputField.Text)))))))
	{

	s2 = 0;
return;	}else
	{

	s2 = -1;
return;	}	
	default: return;}}
	

	int s3=-1;
	public void Rule3(float dt, World world){ 
	switch (s3)
	{

	case -1:
	___playerOil31 = world.CurrentPlayer.Resources.Oil;
	OilInputField.MaxValue = ___playerOil31;
	OilInputField.MinValue = 0;
	s3 = 0;
return;
	case 0:
	if(!(!(((___playerOil31) == (world.CurrentPlayer.Resources.Oil)))))
	{

	s3 = 0;
return;	}else
	{

	s3 = -1;
return;	}	
	default: return;}}
	

	int s4=-1;
	public void Rule4(float dt, World world){ 
	switch (s4)
	{

	case -1:
	___playerIron41 = world.CurrentPlayer.Resources.Iron;
	IronInputField.MaxValue = ___playerIron41;
	IronInputField.MinValue = 0;
	s4 = 0;
return;
	case 0:
	if(!(!(((___playerIron41) == (world.CurrentPlayer.Resources.Iron)))))
	{

	s4 = 0;
return;	}else
	{

	s4 = -1;
return;	}	
	default: return;}}
	

	int s5=-1;
	public void Rule5(float dt, World world){ 
	switch (s5)
	{

	case -1:
	___playerPlutonium51 = world.CurrentPlayer.Resources.Plutonium;
	PlutoniumInputField.MaxValue = ___playerPlutonium51;
	PlutoniumInputField.MinValue = 0;
	s5 = 0;
return;
	case 0:
	if(!(!(((___playerPlutonium51) == (world.CurrentPlayer.Resources.Plutonium)))))
	{

	s5 = 0;
return;	}else
	{

	s5 = -1;
return;	}	
	default: return;}}
	

	int s6=-1;
	public void Rule6(float dt, World world){ 
	switch (s6)
	{

	case -1:
	___playerUranium61 = world.CurrentPlayer.Resources.Uranium;
	UraniumInputField.MaxValue = ___playerUranium61;
	UraniumInputField.MinValue = 0;
	s6 = 0;
return;
	case 0:
	if(!(!(((___playerUranium61) == (world.CurrentPlayer.Resources.Uranium)))))
	{

	s6 = 0;
return;	}else
	{

	s6 = -1;
return;	}	
	default: return;}}
	

	int s7=-1;
	public void Rule7(float dt, World world){ 
	switch (s7)
	{

	case -1:
	___currentPayment71 = Payment;
	___currentCost70 = Cost;
	___receiver74 = Receivers.SelectionName;
	SendButton.Disabled = ((((((Cost) > (Payment))) || (((MessageEcho.Count) == (0))))) || (((___receiver74) == (world.CurrentPlayer.Name))));
	s7 = 0;
return;
	case 0:
	if(!(((((((!(((___currentPayment71) == (Payment)))) || (!(((___currentCost70) == (Cost)))))) || (!(((___receiver74) == (Receivers.SelectionName)))))) || (((world.CurrentPlayer.MiningDrones.Count) > (0))))))
	{

	s7 = 0;
return;	}else
	{

	s7 = -1;
return;	}	
	default: return;}}
	

	int s8=-1;
	public void Rule8(float dt, World world){ 
	switch (s8)
	{

	case -1:
	if(!(SendButton.Clicked))
	{

	s8 = -1;
return;	}else
	{

	goto case 7;	}
	case 7:
	___messageName81 = ((((((((world.CurrentPlayer.Name) + (" => "))) + (Receivers.SelectionName))) + (" : "))) + (GameUtils.Date()));
	___receiver85 = (

(world.Players).Select(__ContextSymbol104 => new { ___player83 = __ContextSymbol104 })
.Where(__ContextSymbol105 => ((__ContextSymbol105.___player83.Name) == (Receivers.SelectionName)))
.Select(__ContextSymbol106 => __ContextSymbol106.___player83)
.ToList<Player>()).ToList<Player>();
	___images82 = (

(ImageSelection).Select(__ContextSymbol107 => new { ___image81 = __ContextSymbol107 })
.Where(__ContextSymbol108 => __ContextSymbol108.___image81.Selected)
.Select(__ContextSymbol109 => __ContextSymbol109.___image81.ImageIndex)
.ToList<System.Int32>()).ToList<System.Int32>();
	___receiver86 = ___receiver85.Head();
	___newMessage80 = new Message(___receiver86,world.CurrentPlayer,MessageEcho,___images82,___messageName81);
	count_down7 = dt;
	goto case 2;
	case 2:
	if(((count_down7) > (0f)))
	{

	count_down7 = ((count_down7) - (dt));
	s8 = 2;
return;	}else
	{

	goto case 0;	}
	case 0:
	SendButton.Clicked = false;
	OutgoingMessages = new Cons<Message>(___newMessage80, (OutgoingMessages)).ToList<Message>();
	s8 = -1;
return;	
	default: return;}}
	

	int s9=-1;
	public void Rule9(float dt, World world){ 
	switch (s9)
	{

	case -1:
	if(!(ClearButton.Clicked))
	{

	s9 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	MessageEcho = (

Enumerable.Empty<System.Int32>()).ToList<System.Int32>();
	ClearButton.Clicked = false;
	s9 = -1;
return;	
	default: return;}}
	

	int s10=-1;
	public void Rule10(float dt, World world){ 
	switch (s10)
	{

	case -1:
	if(!(CloseButton.Clicked))
	{

	s10 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	Active = false;
	MessageEcho = (

Enumerable.Empty<System.Int32>()).ToList<System.Int32>();
	CloseButton.Clicked = false;
	s10 = -1;
return;	
	default: return;}}
	

	int s11=-1;
	public void Rule11(float dt, World world){ 
	switch (s11)
	{

	case -1:
	___currentPlayer115 = world.CurrentPlayer;
	goto case 1;
	case 1:
	if(!(!(((world.CurrentPlayer) == (world.CurrentPlayer)))))
	{

	s11 = 1;
return;	}else
	{

	goto case 0;	}
	case 0:
	Active = false;
	MessageEcho = (

Enumerable.Empty<System.Int32>()).ToList<System.Int32>();
	s11 = -1;
return;	
	default: return;}}
	

	int s12=-1;
	public void Rule12(float dt, World world){ 
	switch (s12)
	{

	case -1:
	___players121 = world.Players;
	
	counter312 = -1;
	if((((___players121).Count) == (0)))
	{

	goto case 1;	}else
	{

	___p120 = (___players121)[0];
	goto case 3;	}
	case 3:
	counter312 = ((counter312) + (1));
	if((((((___players121).Count) == (counter312))) || (((counter312) > ((___players121).Count)))))
	{

	goto case 1;	}else
	{

	___p120 = (___players121)[counter312];
	goto case 4;	}
	case 4:
	ComboBox.AddButton(Receivers.ComboBox,___p120.Name);
	s12 = 3;
return;
	case 1:
	Receivers = Receivers;
	s12 = 0;
return;
	case 0:
	if(!(((world.Players.Count) > (___players121.Count))))
	{

	s12 = 0;
return;	}else
	{

	s12 = -1;
return;	}	
	default: return;}}
	

	int s13=-1;
	public void Rule13(float dt, World world){ 
	switch (s13)
	{

	case -1:
	if(!(world.Log.WritingLog))
	{

	s13 = -1;
return;	}else
	{

	goto case 2;	}
	case 2:
	___playerMessages131 = (

(world.Players).Select(__ContextSymbol113 => new { ___player134 = __ContextSymbol113 })
.SelectMany(__ContextSymbol114=> (__ContextSymbol114.___player134.ReceivedMessages).Select(__ContextSymbol115 => new { ___message133 = __ContextSymbol115,
                                                      prev = __ContextSymbol114 })
.Select(__ContextSymbol116 => __ContextSymbol116.___message133)
.ToList<Message>())).ToList<Message>();
	___filteredMessages130 = (

(OutgoingMessages).Select(__ContextSymbol117 => new { ___m1130 = __ContextSymbol117 })
.Select(__ContextSymbol118 => new {___receivedMessages130 = (

(___playerMessages131).Select(__ContextSymbol119 => new { ___m2130 = __ContextSymbol119,prev = __ContextSymbol118 })
.Where(__ContextSymbol120 => ((__ContextSymbol120.prev.___m1130) == (__ContextSymbol120.___m2130)))
.Select(__ContextSymbol121 => __ContextSymbol121.prev.___m1130)
.ToList<Message>()).ToList<Message>(), prev = __ContextSymbol118 })
.Where(__ContextSymbol122 => ((__ContextSymbol122.___receivedMessages130.Count) == (0)))
.Select(__ContextSymbol123 => __ContextSymbol123.prev.___m1130)
.ToList<Message>()).ToList<Message>();
	OutgoingMessages = ___filteredMessages130;
	s13 = -1;
return;	
	default: return;}}
	

	int s15=-1;
	public void Rule15(float dt, World world){ 
	switch (s15)
	{

	case -1:
	if(!(world.GUI.QuickBar.MessageButton.Clicked))
	{

	s15 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	Active = true;
	s15 = -1;
return;	
	default: return;}}
	

	int s16=-1;
	public void Rule16(float dt, World world){ 
	switch (s16)
	{

	case -1:
	Active = false;
	s16 = 0;
return;
	case 0:
	if(!(false))
	{

	s16 = 0;
return;	}else
	{

	s16 = -1;
return;	}	
	default: return;}}
	





}
public class MainMenu{
public int frame;
public bool JustEntered = true;
	public int ID;
public MainMenu()
	{JustEntered = false;
 frame = World.frame;
		UnityMenu = UnityMenu.GetMenu("UICanvas/MainMenuCanvas");
		GameMenu = new CnvButton("UICanvas/MainMenuCanvas/GameMenuButton");
		Exit = new CnvButton("UICanvas/MainMenuCanvas/QuitButton");
		Back = new CnvButton("UICanvas/MainMenuCanvas/ReturnGameButton");
		
}
		public System.Boolean Active{  get { return UnityMenu.Active; }
  set{UnityMenu.Active = value; }
 }
	public CnvButton Back;
	public CnvButton Exit;
	public CnvButton GameMenu;
	public System.Boolean OnMenu{  get { return UnityMenu.OnMenu; }
  set{UnityMenu.OnMenu = value; }
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
	public System.Single count_down8;
	public System.Single count_down9;
	public void Update(float dt, World world) {
frame = World.frame;

		Back.Update(dt, world);
		Exit.Update(dt, world);
		GameMenu.Update(dt, world);
		this.Rule0(dt, world);
		this.Rule1(dt, world);
		this.Rule2(dt, world);
		this.Rule3(dt, world);
		this.Rule4(dt, world);
	}





	int s0=-1;
	public void Rule0(float dt, World world){ 
	switch (s0)
	{

	case -1:
	if(!(UnityEngine.Input.GetKeyDown(KeyCode.Escape)))
	{

	s0 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	Active = !(Active);
	s0 = -1;
return;	
	default: return;}}
	

	int s1=-1;
	public void Rule1(float dt, World world){ 
	switch (s1)
	{

	case -1:
	if(!(Back.Clicked))
	{

	s1 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	Back.Clicked = false;
	Active = false;
	OnMenu = false;
	s1 = -1;
return;	
	default: return;}}
	

	int s2=-1;
	public void Rule2(float dt, World world){ 
	switch (s2)
	{

	case -1:
	if(!(GameMenu.Clicked))
	{

	s2 = -1;
return;	}else
	{

	goto case 3;	}
	case 3:
	count_down8 = dt;
	goto case 4;
	case 4:
	if(((count_down8) > (0f)))
	{

	count_down8 = ((count_down8) - (dt));
	s2 = 4;
return;	}else
	{

	goto case 2;	}
	case 2:
	if(!(!(world.Log.WritingLog)))
	{

	s2 = 2;
return;	}else
	{

	goto case 1;	}
	case 1:
	GameMenu.Clicked = false;
	s2 = 0;
return;
	case 0:
	UnityEngine.Application.LoadLevel("menu");
	s2 = -1;
return;	
	default: return;}}
	

	int s3=-1;
	public void Rule3(float dt, World world){ 
	switch (s3)
	{

	case -1:
	if(!(Exit.Clicked))
	{

	s3 = -1;
return;	}else
	{

	goto case 3;	}
	case 3:
	count_down9 = dt;
	goto case 4;
	case 4:
	if(((count_down9) > (0f)))
	{

	count_down9 = ((count_down9) - (dt));
	s3 = 4;
return;	}else
	{

	goto case 2;	}
	case 2:
	if(!(!(world.Log.WritingLog)))
	{

	s3 = 2;
return;	}else
	{

	goto case 1;	}
	case 1:
	Exit.Clicked = false;
	s3 = 0;
return;
	case 0:
	UnityEngine.Application.Quit();
	s3 = -1;
return;	
	default: return;}}
	

	int s4=-1;
	public void Rule4(float dt, World world){ 
	switch (s4)
	{

	case -1:
	Active = false;
	s4 = 0;
return;
	case 0:
	if(!(false))
	{

	s4 = 0;
return;	}else
	{

	s4 = -1;
return;	}	
	default: return;}}
	





}
public class ResourceBar{
public int frame;
public bool JustEntered = true;
	public int ID;
public ResourceBar()
	{JustEntered = false;
 frame = World.frame;
		UraniumIcon = new CnvIcon("UICanvas/PlayerResourcesCanvas/UraniumIcon",(new Nothing<IconInstantiation>()));
		PlutoniumIcon = new CnvIcon("UICanvas/PlayerResourcesCanvas/PlutoniumIcon",(new Nothing<IconInstantiation>()));
		OilIcon = new CnvIcon("UICanvas/PlayerResourcesCanvas/OilIcon",(new Nothing<IconInstantiation>()));
		IronIcon = new CnvIcon("UICanvas/PlayerResourcesCanvas/IronIcon",(new Nothing<IconInstantiation>()));
		
}
		public CnvIcon IronIcon;
	public CnvIcon OilIcon;
	public CnvIcon PlutoniumIcon;
	public CnvIcon UraniumIcon;
	public void Update(float dt, World world) {
frame = World.frame;		this.Rule0(dt, world);

		IronIcon.Update(dt, world);
		OilIcon.Update(dt, world);
		PlutoniumIcon.Update(dt, world);
		UraniumIcon.Update(dt, world);


	}

	public void Rule0(float dt, World world) 
	{
	System.String ___uranium00;
	___uranium00 = GameUtils.IntToString(world.CurrentPlayer.Resources.Uranium);
	System.String ___plutonium00;
	___plutonium00 = GameUtils.IntToString(world.CurrentPlayer.Resources.Plutonium);
	System.String ___oil00;
	___oil00 = GameUtils.IntToString(world.CurrentPlayer.Resources.Oil);
	System.String ___iron00;
	___iron00 = GameUtils.IntToString(world.CurrentPlayer.Resources.Iron);
		UraniumIcon.Text = ___uranium00;
	PlutoniumIcon.Text = ___plutonium00;
	OilIcon.Text = ___oil00;
	IronIcon.Text = ___iron00;
	}
	










}
public class PlanetMenu{
public int frame;
public bool JustEntered = true;
	public int ID;
public PlanetMenu()
	{JustEntered = false;
 frame = World.frame;
		UraniumIcon = new CnvIcon("UICanvas/PlanetMenuCanvas/UraniumIcon",(new Nothing<IconInstantiation>()));
		UnityMenu = UnityMenu.GetMenu("UICanvas/PlanetMenuCanvas");
		SendMessage = new CnvButton("UICanvas/PlanetMenuCanvas/MessageButton");
		ScanForResources = new CnvButton("UICanvas/PlanetMenuCanvas/ResourcesButton");
		PlutoniumIcon = new CnvIcon("UICanvas/PlanetMenuCanvas/PlutoniumIcon",(new Nothing<IconInstantiation>()));
		OilIcon = new CnvIcon("UICanvas/PlanetMenuCanvas/OilIcon",(new Nothing<IconInstantiation>()));
		Mine = new CnvButton("UICanvas/PlanetMenuCanvas/MineButton");
		IronIcon = new CnvIcon("UICanvas/PlanetMenuCanvas/IronIcon",(new Nothing<IconInstantiation>()));
		CancelOrders = new CnvButton("UICanvas/PlanetMenuCanvas/OrderButton");
		
}
		public System.Boolean Active{  get { return UnityMenu.Active; }
  set{UnityMenu.Active = value; }
 }
	public CnvButton CancelOrders;
	public CnvIcon IronIcon;
	public CnvButton Mine;
	public CnvIcon OilIcon;
	public System.Boolean OnMenu{  get { return UnityMenu.OnMenu; }
  set{UnityMenu.OnMenu = value; }
 }
	public CnvIcon PlutoniumIcon;
	public CnvButton ScanForResources;
	public CnvButton SendMessage;
	public UnityMenu UnityMenu;
	public CnvIcon UraniumIcon;
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
	public Planet ___p02;
	public List<Player> ___currentScanner00;
	public System.Boolean ___scannedByCurrentPlayer00;
	public System.String ___uranium01;
	public System.String ___plutonium01;
	public System.String ___iron01;
	public System.String ___oil01;
	public List<Player> ___currentScanner31;
	public System.Single count_down10;
	public List<Player> ___currentScanner52;
	public System.Single count_down11;
	public List<Drone> ___busyDrones72;
	public List<Drone> ___targetingDrones70;
	public System.Single count_down12;
	public void Update(float dt, World world) {
frame = World.frame;		this.Rule1(dt, world);
		this.Rule9(dt, world);
		CancelOrders.Update(dt, world);
		IronIcon.Update(dt, world);
		Mine.Update(dt, world);
		OilIcon.Update(dt, world);
		PlutoniumIcon.Update(dt, world);
		ScanForResources.Update(dt, world);
		SendMessage.Update(dt, world);
		UraniumIcon.Update(dt, world);
		this.Rule0(dt, world);
		this.Rule2(dt, world);
		this.Rule3(dt, world);
		this.Rule4(dt, world);
		this.Rule5(dt, world);
		this.Rule6(dt, world);
		this.Rule7(dt, world);
		this.Rule8(dt, world);
	}

	public void Rule1(float dt, World world) 
	{
	SendMessage.Disabled = ((world.SelectedPlanet.IsNone) || (((world.SelectedPlanet.IsSome) && (((world.SelectedPlanet.Value.Owner.IsNone) || (((world.SelectedPlanet.Value.Owner.Value) == (world.CurrentPlayer))))))));
	}
	

	public void Rule9(float dt, World world) 
	{
	Active = world.SelectedPlanet.IsSome;
	}
	



	int s0=-1;
	public void Rule0(float dt, World world){ 
	switch (s0)
	{

	case -1:
	if(!(world.SelectedPlanet.IsSome))
	{

	s0 = -1;
return;	}else
	{

	goto case 11;	}
	case 11:
	___p02 = world.SelectedPlanet.Value;
	___currentScanner00 = (

(___p02.ScannedBy).Select(__ContextSymbol124 => new { ___player05 = __ContextSymbol124 })
.Where(__ContextSymbol125 => ((__ContextSymbol125.___player05) == (world.CurrentPlayer)))
.Select(__ContextSymbol126 => __ContextSymbol126.___player05)
.ToList<Player>()).ToList<Player>();
	___scannedByCurrentPlayer00 = ((___currentScanner00.Count) > (0));
	if(___scannedByCurrentPlayer00)
	{

	goto case 0;	}else
	{

	goto case 1;	}
	case 0:
	___uranium01 = GameUtils.IntToString(___p02.Resources.Uranium);
	___plutonium01 = GameUtils.IntToString(___p02.Resources.Plutonium);
	___iron01 = GameUtils.IntToString(___p02.Resources.Iron);
	___oil01 = GameUtils.IntToString(___p02.Resources.Oil);
	UraniumIcon.Text = ___uranium01;
	PlutoniumIcon.Text = ___plutonium01;
	IronIcon.Text = ___iron01;
	OilIcon.Text = ___oil01;
	s0 = -1;
return;
	case 1:
	UraniumIcon.Text = "?";
	PlutoniumIcon.Text = "?";
	IronIcon.Text = "?";
	OilIcon.Text = "?";
	s0 = -1;
return;	
	default: return;}}
	

	int s2=-1;
	public void Rule2(float dt, World world){ 
	switch (s2)
	{

	case -1:
	if(!(SendMessage.Clicked))
	{

	s2 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	world.GUI.MessageMenu.Active = true;
	SendMessage.Clicked = false;
	s2 = -1;
return;	
	default: return;}}
	

	int s3=-1;
	public void Rule3(float dt, World world){ 
	switch (s3)
	{

	case -1:
	if(!(world.SelectedPlanet.IsSome))
	{

	s3 = -1;
return;	}else
	{

	goto case 1;	}
	case 1:
	___currentScanner31 = (

(world.SelectedPlanet.Value.ScannedBy).Select(__ContextSymbol127 => new { ___player36 = __ContextSymbol127 })
.Where(__ContextSymbol128 => ((world.CurrentPlayer) == (__ContextSymbol128.___player36)))
.Select(__ContextSymbol129 => __ContextSymbol129.___player36)
.ToList<Player>()).ToList<Player>();
	ScanForResources.Disabled = ((___currentScanner31.Count) > (0));
	s3 = -1;
return;	
	default: return;}}
	

	int s4=-1;
	public void Rule4(float dt, World world){ 
	switch (s4)
	{

	case -1:
	if(!(ScanForResources.Clicked))
	{

	s4 = -1;
return;	}else
	{

	goto case 1;	}
	case 1:
	count_down10 = dt;
	goto case 2;
	case 2:
	if(((count_down10) > (0f)))
	{

	count_down10 = ((count_down10) - (dt));
	s4 = 2;
return;	}else
	{

	goto case 0;	}
	case 0:
	ScanForResources.Clicked = false;
	s4 = -1;
return;	
	default: return;}}
	

	int s5=-1;
	public void Rule5(float dt, World world){ 
	switch (s5)
	{

	case -1:
	if(!(world.SelectedPlanet.IsSome))
	{

	s5 = -1;
return;	}else
	{

	goto case 1;	}
	case 1:
	___currentScanner52 = (

(world.SelectedPlanet.Value.ScannedBy).Select(__ContextSymbol130 => new { ___player57 = __ContextSymbol130 })
.Where(__ContextSymbol131 => ((world.CurrentPlayer) == (__ContextSymbol131.___player57)))
.Select(__ContextSymbol132 => __ContextSymbol132.___player57)
.ToList<Player>()).ToList<Player>();
	Mine.Disabled = ((((___currentScanner52.Count) == (0))) || (((((((((world.SelectedPlanet.Value.Resources.Uranium) == (0))) && (((world.SelectedPlanet.Value.Resources.Plutonium) == (0))))) && (((world.SelectedPlanet.Value.Resources.Oil) == (0))))) && (((world.SelectedPlanet.Value.Resources.Iron) == (0))))));
	s5 = -1;
return;	
	default: return;}}
	

	int s6=-1;
	public void Rule6(float dt, World world){ 
	switch (s6)
	{

	case -1:
	if(!(Mine.Clicked))
	{

	s6 = -1;
return;	}else
	{

	goto case 1;	}
	case 1:
	count_down11 = dt;
	goto case 2;
	case 2:
	if(((count_down11) > (0f)))
	{

	count_down11 = ((count_down11) - (dt));
	s6 = 2;
return;	}else
	{

	goto case 0;	}
	case 0:
	Mine.Clicked = false;
	s6 = -1;
return;	
	default: return;}}
	

	int s7=-1;
	public void Rule7(float dt, World world){ 
	switch (s7)
	{

	case -1:
	if(!(world.SelectedPlanet.IsSome))
	{

	s7 = -1;
return;	}else
	{

	goto case 2;	}
	case 2:
	___busyDrones72 = (

(world.CurrentPlayer.Drones).Select(__ContextSymbol133 => new { ___drone72 = __ContextSymbol133 })
.Where(__ContextSymbol134 => __ContextSymbol134.___drone72.Busy)
.Select(__ContextSymbol135 => __ContextSymbol135.___drone72)
.ToList<Drone>()).ToList<Drone>();
	___targetingDrones70 = (

(world.CurrentPlayer.Drones).Select(__ContextSymbol136 => new { ___drone73 = __ContextSymbol136 })
.Where(__ContextSymbol137 => ((((__ContextSymbol137.___drone73.ScanAction.IsSome) && (((__ContextSymbol137.___drone73.ScanAction.Value.Action.Target) == (world.SelectedPlanet.Value))))) || (((__ContextSymbol137.___drone73.GatherAction.IsSome) && (((__ContextSymbol137.___drone73.GatherAction.Value.Action.Target) == (world.SelectedPlanet.Value)))))))
.Select(__ContextSymbol138 => __ContextSymbol138.___drone73)
.ToList<Drone>()).ToList<Drone>();
	CancelOrders.Disabled = ((((___busyDrones72.Count) > (0))) || (((___targetingDrones70.Count) == (0))));
	s7 = -1;
return;	
	default: return;}}
	

	int s8=-1;
	public void Rule8(float dt, World world){ 
	switch (s8)
	{

	case -1:
	if(!(CancelOrders.Clicked))
	{

	s8 = -1;
return;	}else
	{

	goto case 1;	}
	case 1:
	count_down12 = dt;
	goto case 2;
	case 2:
	if(((count_down12) > (0f)))
	{

	count_down12 = ((count_down12) - (dt));
	s8 = 2;
return;	}else
	{

	goto case 0;	}
	case 0:
	CancelOrders.Clicked = false;
	s8 = -1;
return;	
	default: return;}}
	





}
public class CnvButton{
public int frame;
public bool JustEntered = true;
private System.String n;
	public int ID;
public CnvButton(System.String n)
	{JustEntered = false;
 frame = World.frame;
		UnityButton = UnityButton.GetButton(n);
		
}
		public System.Boolean Active{  get { return UnityButton.Active; }
  set{UnityButton.Active = value; }
 }
	public System.Boolean Clicked{  get { return UnityButton.Clicked; }
  set{UnityButton.Clicked = value; }
 }
	public System.Boolean Disabled{  get { return UnityButton.Disabled; }
  set{UnityButton.Disabled = value; }
 }
	public System.String ImageName{  get { return UnityButton.ImageName; }
  set{UnityButton.ImageName = value; }
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
	public void Update(float dt, World world) {
frame = World.frame;



	}











}
public class CnvSelectionBox{
public int frame;
public bool JustEntered = true;
private System.String n;
	public int ID;
public CnvSelectionBox(System.String n)
	{JustEntered = false;
 frame = World.frame;
		UnitySelectionBox = UnitySelectionBox.GetBox(n);
		
}
		public System.Boolean Active{  get { return UnitySelectionBox.Active; }
  set{UnitySelectionBox.Active = value; }
 }
	public System.Boolean Clicked{  get { return UnitySelectionBox.Clicked; }
  set{UnitySelectionBox.Clicked = value; }
 }
	public System.Boolean Disabled{  get { return UnitySelectionBox.Disabled; }
  set{UnitySelectionBox.Disabled = value; }
 }
	public System.String ImageName{  get { return UnitySelectionBox.ImageName; }
  set{UnitySelectionBox.ImageName = value; }
 }
	public UnitySelectionBox UnitySelectionBox;
	public UnityEngine.Animation animation{  get { return UnitySelectionBox.animation; }
 }
	public UnityEngine.AudioSource audio{  get { return UnitySelectionBox.audio; }
 }
	public UnityEngine.Camera camera{  get { return UnitySelectionBox.camera; }
 }
	public UnityEngine.Collider collider{  get { return UnitySelectionBox.collider; }
 }
	public UnityEngine.Collider2D collider2D{  get { return UnitySelectionBox.collider2D; }
 }
	public UnityEngine.ConstantForce constantForce{  get { return UnitySelectionBox.constantForce; }
 }
	public System.Boolean enabled{  get { return UnitySelectionBox.enabled; }
  set{UnitySelectionBox.enabled = value; }
 }
	public UnityEngine.GameObject gameObject{  get { return UnitySelectionBox.gameObject; }
 }
	public UnityEngine.GUIElement guiElement{  get { return UnitySelectionBox.guiElement; }
 }
	public UnityEngine.GUIText guiText{  get { return UnitySelectionBox.guiText; }
 }
	public UnityEngine.GUITexture guiTexture{  get { return UnitySelectionBox.guiTexture; }
 }
	public UnityEngine.HideFlags hideFlags{  get { return UnitySelectionBox.hideFlags; }
  set{UnitySelectionBox.hideFlags = value; }
 }
	public UnityEngine.HingeJoint hingeJoint{  get { return UnitySelectionBox.hingeJoint; }
 }
	public UnityEngine.Light light{  get { return UnitySelectionBox.light; }
 }
	public System.String name{  get { return UnitySelectionBox.name; }
  set{UnitySelectionBox.name = value; }
 }
	public UnityEngine.ParticleEmitter particleEmitter{  get { return UnitySelectionBox.particleEmitter; }
 }
	public UnityEngine.ParticleSystem particleSystem{  get { return UnitySelectionBox.particleSystem; }
 }
	public UnityEngine.Renderer renderer{  get { return UnitySelectionBox.renderer; }
 }
	public UnityEngine.Rigidbody rigidbody{  get { return UnitySelectionBox.rigidbody; }
 }
	public UnityEngine.Rigidbody2D rigidbody2D{  get { return UnitySelectionBox.rigidbody2D; }
 }
	public System.String tag{  get { return UnitySelectionBox.tag; }
  set{UnitySelectionBox.tag = value; }
 }
	public UnityEngine.Transform transform{  get { return UnitySelectionBox.transform; }
 }
	public System.Boolean useGUILayout{  get { return UnitySelectionBox.useGUILayout; }
  set{UnitySelectionBox.useGUILayout = value; }
 }
	public void Update(float dt, World world) {
frame = World.frame;



	}











}
public class CnvText{
public int frame;
public bool JustEntered = true;
private System.String n;
	public int ID;
public CnvText(System.String n)
	{JustEntered = false;
 frame = World.frame;
		UnityText = UnityText.GetText(n);
		
}
		public System.Single Alpha{  get { return UnityText.Alpha; }
  set{UnityText.Alpha = value; }
 }
	public System.String Text{  get { return UnityText.Text; }
  set{UnityText.Text = value; }
 }
	public UnityText UnityText;
	public UnityEngine.Animation animation{  get { return UnityText.animation; }
 }
	public UnityEngine.AudioSource audio{  get { return UnityText.audio; }
 }
	public UnityEngine.Camera camera{  get { return UnityText.camera; }
 }
	public UnityEngine.Collider collider{  get { return UnityText.collider; }
 }
	public UnityEngine.Collider2D collider2D{  get { return UnityText.collider2D; }
 }
	public UnityEngine.ConstantForce constantForce{  get { return UnityText.constantForce; }
 }
	public System.Boolean enabled{  get { return UnityText.enabled; }
  set{UnityText.enabled = value; }
 }
	public UnityEngine.GameObject gameObject{  get { return UnityText.gameObject; }
 }
	public UnityEngine.GUIElement guiElement{  get { return UnityText.guiElement; }
 }
	public UnityEngine.GUIText guiText{  get { return UnityText.guiText; }
 }
	public UnityEngine.GUITexture guiTexture{  get { return UnityText.guiTexture; }
 }
	public UnityEngine.HideFlags hideFlags{  get { return UnityText.hideFlags; }
  set{UnityText.hideFlags = value; }
 }
	public UnityEngine.HingeJoint hingeJoint{  get { return UnityText.hingeJoint; }
 }
	public UnityEngine.Light light{  get { return UnityText.light; }
 }
	public System.String name{  get { return UnityText.name; }
  set{UnityText.name = value; }
 }
	public UnityEngine.ParticleEmitter particleEmitter{  get { return UnityText.particleEmitter; }
 }
	public UnityEngine.ParticleSystem particleSystem{  get { return UnityText.particleSystem; }
 }
	public UnityEngine.Renderer renderer{  get { return UnityText.renderer; }
 }
	public UnityEngine.Rigidbody rigidbody{  get { return UnityText.rigidbody; }
 }
	public UnityEngine.Rigidbody2D rigidbody2D{  get { return UnityText.rigidbody2D; }
 }
	public System.String tag{  get { return UnityText.tag; }
  set{UnityText.tag = value; }
 }
	public UnityEngine.Transform transform{  get { return UnityText.transform; }
 }
	public System.Boolean useGUILayout{  get { return UnityText.useGUILayout; }
  set{UnityText.useGUILayout = value; }
 }
	public void Update(float dt, World world) {
frame = World.frame;



	}











}
public class IconInstantiation{
public int frame;
public bool JustEntered = true;
private UnityEngine.Vector3 position;
private System.String imageName;
private System.String parentPath;
	public int ID;
public IconInstantiation(UnityEngine.Vector3 position, System.String imageName, System.String parentPath)
	{JustEntered = false;
 frame = World.frame;
		Position = position;
		ParentPath = parentPath;
		ImageName = imageName;
		
}
		public System.String ImageName;
	public System.String ParentPath;
	public UnityEngine.Vector3 Position;
	public void Update(float dt, World world) {
frame = World.frame;



	}











}
public class CnvIcon{
public int frame;
public bool JustEntered = true;
private System.String n;
private Option<IconInstantiation> instantiate;
	public int ID;
public CnvIcon(System.String n, Option<IconInstantiation> instantiate)
	{JustEntered = false;
 frame = World.frame;
		UnityIcon ___icon01;
		if(instantiate.IsSome)
			{
			___icon01 = UnityIcon.Instantiate(n,instantiate.Value.ImageName,instantiate.Value.ParentPath,instantiate.Value.Position);
			}else
			{
			___icon01 = UnityIcon.GetIcon(n);
			}
		UnityIcon = ___icon01;
		
}
		public System.Boolean Active{  get { return UnityIcon.Active; }
  set{UnityIcon.Active = value; }
 }
	public UnityEngine.Color Color{  get { return UnityIcon.Color; }
  set{UnityIcon.Color = value; }
 }
	public System.Single Height{  get { return UnityIcon.Height; }
 }
	public System.String ImageName{  get { return UnityIcon.ImageName; }
  set{UnityIcon.ImageName = value; }
 }
	public UnityEngine.Vector2 Origin{  get { return UnityIcon.Origin; }
  set{UnityIcon.Origin = value; }
 }
	public System.String Text{  get { return UnityIcon.Text; }
  set{UnityIcon.Text = value; }
 }
	public UnityIcon UnityIcon;
	public System.Single Width{  get { return UnityIcon.Width; }
 }
	public UnityEngine.Animation animation{  get { return UnityIcon.animation; }
 }
	public UnityEngine.AudioSource audio{  get { return UnityIcon.audio; }
 }
	public UnityEngine.Camera camera{  get { return UnityIcon.camera; }
 }
	public UnityEngine.Collider collider{  get { return UnityIcon.collider; }
 }
	public UnityEngine.Collider2D collider2D{  get { return UnityIcon.collider2D; }
 }
	public UnityEngine.ConstantForce constantForce{  get { return UnityIcon.constantForce; }
 }
	public System.Boolean enabled{  get { return UnityIcon.enabled; }
  set{UnityIcon.enabled = value; }
 }
	public UnityEngine.GameObject gameObject{  get { return UnityIcon.gameObject; }
 }
	public UnityEngine.GUIElement guiElement{  get { return UnityIcon.guiElement; }
 }
	public UnityEngine.GUIText guiText{  get { return UnityIcon.guiText; }
 }
	public UnityEngine.GUITexture guiTexture{  get { return UnityIcon.guiTexture; }
 }
	public UnityEngine.HideFlags hideFlags{  get { return UnityIcon.hideFlags; }
  set{UnityIcon.hideFlags = value; }
 }
	public UnityEngine.HingeJoint hingeJoint{  get { return UnityIcon.hingeJoint; }
 }
	public UnityEngine.Light light{  get { return UnityIcon.light; }
 }
	public System.String name{  get { return UnityIcon.name; }
  set{UnityIcon.name = value; }
 }
	public UnityEngine.ParticleEmitter particleEmitter{  get { return UnityIcon.particleEmitter; }
 }
	public UnityEngine.ParticleSystem particleSystem{  get { return UnityIcon.particleSystem; }
 }
	public UnityEngine.Renderer renderer{  get { return UnityIcon.renderer; }
 }
	public UnityEngine.Rigidbody rigidbody{  get { return UnityIcon.rigidbody; }
 }
	public UnityEngine.Rigidbody2D rigidbody2D{  get { return UnityIcon.rigidbody2D; }
 }
	public System.String tag{  get { return UnityIcon.tag; }
  set{UnityIcon.tag = value; }
 }
	public UnityEngine.Transform transform{  get { return UnityIcon.transform; }
 }
	public System.Boolean useGUILayout{  get { return UnityIcon.useGUILayout; }
  set{UnityIcon.useGUILayout = value; }
 }
	public void Update(float dt, World world) {
frame = World.frame;



	}











}
public class CnvFrame{
public int frame;
public bool JustEntered = true;
private System.String n;
private Option<System.Single> offsetX;
private Option<System.Single> offsetY;
	public int ID;
public CnvFrame(System.String n, Option<System.Single> offsetX, Option<System.Single> offsetY)
	{JustEntered = false;
 frame = World.frame;
		System.Single ___offsetX04;
		if(offsetX.IsNone)
			{
			___offsetX04 = 0f;
			}else
			{
			___offsetX04 = offsetX.Value;
			}
		System.Single ___offsetY04;
		if(offsetY.IsNone)
			{
			___offsetY04 = 0f;
			}else
			{
			___offsetY04 = offsetY.Value;
			}
		UnityFrame = UnityFrame.GetFrame(n,___offsetX04,___offsetY04);
		
}
		public System.Single Height{  get { return UnityFrame.Height; }
 }
	public System.Single OffsetX{  get { return UnityFrame.OffsetX; }
  set{UnityFrame.OffsetX = value; }
 }
	public System.Single OffsetY{  get { return UnityFrame.OffsetY; }
  set{UnityFrame.OffsetY = value; }
 }
	public UnityEngine.Vector2 Origin{  get { return UnityFrame.Origin; }
 }
	public UnityFrame UnityFrame;
	public System.Single Width{  get { return UnityFrame.Width; }
 }
	public UnityEngine.Animation animation{  get { return UnityFrame.animation; }
 }
	public UnityEngine.AudioSource audio{  get { return UnityFrame.audio; }
 }
	public UnityEngine.Camera camera{  get { return UnityFrame.camera; }
 }
	public UnityEngine.Collider collider{  get { return UnityFrame.collider; }
 }
	public UnityEngine.Collider2D collider2D{  get { return UnityFrame.collider2D; }
 }
	public UnityEngine.ConstantForce constantForce{  get { return UnityFrame.constantForce; }
 }
	public System.Boolean enabled{  get { return UnityFrame.enabled; }
  set{UnityFrame.enabled = value; }
 }
	public UnityEngine.GameObject gameObject{  get { return UnityFrame.gameObject; }
 }
	public UnityEngine.GUIElement guiElement{  get { return UnityFrame.guiElement; }
 }
	public UnityEngine.GUIText guiText{  get { return UnityFrame.guiText; }
 }
	public UnityEngine.GUITexture guiTexture{  get { return UnityFrame.guiTexture; }
 }
	public UnityEngine.HideFlags hideFlags{  get { return UnityFrame.hideFlags; }
  set{UnityFrame.hideFlags = value; }
 }
	public UnityEngine.HingeJoint hingeJoint{  get { return UnityFrame.hingeJoint; }
 }
	public UnityEngine.Light light{  get { return UnityFrame.light; }
 }
	public System.String name{  get { return UnityFrame.name; }
  set{UnityFrame.name = value; }
 }
	public UnityEngine.ParticleEmitter particleEmitter{  get { return UnityFrame.particleEmitter; }
 }
	public UnityEngine.ParticleSystem particleSystem{  get { return UnityFrame.particleSystem; }
 }
	public UnityEngine.Renderer renderer{  get { return UnityFrame.renderer; }
 }
	public UnityEngine.Rigidbody rigidbody{  get { return UnityFrame.rigidbody; }
 }
	public UnityEngine.Rigidbody2D rigidbody2D{  get { return UnityFrame.rigidbody2D; }
 }
	public System.String tag{  get { return UnityFrame.tag; }
  set{UnityFrame.tag = value; }
 }
	public UnityEngine.Transform transform{  get { return UnityFrame.transform; }
 }
	public System.Boolean useGUILayout{  get { return UnityFrame.useGUILayout; }
  set{UnityFrame.useGUILayout = value; }
 }
	public void Update(float dt, World world) {
frame = World.frame;



	}











}
public class CnvToggle{
public int frame;
public bool JustEntered = true;
private System.String n;
	public int ID;
public CnvToggle(System.String n)
	{JustEntered = false;
 frame = World.frame;
		UnityToggle = UnityToggle.GetToggle(n);
		
}
		public System.Boolean IsOn{  get { return UnityToggle.IsOn; }
  set{UnityToggle.IsOn = value; }
 }
	public UnityToggle UnityToggle;
	public UnityEngine.Animation animation{  get { return UnityToggle.animation; }
 }
	public UnityEngine.AudioSource audio{  get { return UnityToggle.audio; }
 }
	public UnityEngine.Camera camera{  get { return UnityToggle.camera; }
 }
	public UnityEngine.Collider collider{  get { return UnityToggle.collider; }
 }
	public UnityEngine.Collider2D collider2D{  get { return UnityToggle.collider2D; }
 }
	public UnityEngine.ConstantForce constantForce{  get { return UnityToggle.constantForce; }
 }
	public System.Boolean enabled{  get { return UnityToggle.enabled; }
  set{UnityToggle.enabled = value; }
 }
	public UnityEngine.GameObject gameObject{  get { return UnityToggle.gameObject; }
 }
	public UnityEngine.GUIElement guiElement{  get { return UnityToggle.guiElement; }
 }
	public UnityEngine.GUIText guiText{  get { return UnityToggle.guiText; }
 }
	public UnityEngine.GUITexture guiTexture{  get { return UnityToggle.guiTexture; }
 }
	public UnityEngine.HideFlags hideFlags{  get { return UnityToggle.hideFlags; }
  set{UnityToggle.hideFlags = value; }
 }
	public UnityEngine.HingeJoint hingeJoint{  get { return UnityToggle.hingeJoint; }
 }
	public UnityEngine.Light light{  get { return UnityToggle.light; }
 }
	public System.String name{  get { return UnityToggle.name; }
  set{UnityToggle.name = value; }
 }
	public UnityEngine.ParticleEmitter particleEmitter{  get { return UnityToggle.particleEmitter; }
 }
	public UnityEngine.ParticleSystem particleSystem{  get { return UnityToggle.particleSystem; }
 }
	public UnityEngine.Renderer renderer{  get { return UnityToggle.renderer; }
 }
	public UnityEngine.Rigidbody rigidbody{  get { return UnityToggle.rigidbody; }
 }
	public UnityEngine.Rigidbody2D rigidbody2D{  get { return UnityToggle.rigidbody2D; }
 }
	public System.String tag{  get { return UnityToggle.tag; }
  set{UnityToggle.tag = value; }
 }
	public UnityEngine.Transform transform{  get { return UnityToggle.transform; }
 }
	public System.Boolean useGUILayout{  get { return UnityToggle.useGUILayout; }
  set{UnityToggle.useGUILayout = value; }
 }
	public void Update(float dt, World world) {
frame = World.frame;



	}











}
public class CnvPanel{
public int frame;
public bool JustEntered = true;
private System.String n;
	public int ID;
public CnvPanel(System.String n)
	{JustEntered = false;
 frame = World.frame;
		UnityPanel = UnityPanel.GetPanel(n);
		
}
		public System.Single Alpha{  get { return UnityPanel.Alpha; }
  set{UnityPanel.Alpha = value; }
 }
	public UnityPanel UnityPanel;
	public UnityEngine.Animation animation{  get { return UnityPanel.animation; }
 }
	public UnityEngine.AudioSource audio{  get { return UnityPanel.audio; }
 }
	public UnityEngine.Camera camera{  get { return UnityPanel.camera; }
 }
	public UnityEngine.Collider collider{  get { return UnityPanel.collider; }
 }
	public UnityEngine.Collider2D collider2D{  get { return UnityPanel.collider2D; }
 }
	public UnityEngine.ConstantForce constantForce{  get { return UnityPanel.constantForce; }
 }
	public System.Boolean enabled{  get { return UnityPanel.enabled; }
  set{UnityPanel.enabled = value; }
 }
	public UnityEngine.GameObject gameObject{  get { return UnityPanel.gameObject; }
 }
	public UnityEngine.GUIElement guiElement{  get { return UnityPanel.guiElement; }
 }
	public UnityEngine.GUIText guiText{  get { return UnityPanel.guiText; }
 }
	public UnityEngine.GUITexture guiTexture{  get { return UnityPanel.guiTexture; }
 }
	public UnityEngine.HideFlags hideFlags{  get { return UnityPanel.hideFlags; }
  set{UnityPanel.hideFlags = value; }
 }
	public UnityEngine.HingeJoint hingeJoint{  get { return UnityPanel.hingeJoint; }
 }
	public UnityEngine.Light light{  get { return UnityPanel.light; }
 }
	public System.String name{  get { return UnityPanel.name; }
  set{UnityPanel.name = value; }
 }
	public UnityEngine.ParticleEmitter particleEmitter{  get { return UnityPanel.particleEmitter; }
 }
	public UnityEngine.ParticleSystem particleSystem{  get { return UnityPanel.particleSystem; }
 }
	public UnityEngine.Renderer renderer{  get { return UnityPanel.renderer; }
 }
	public UnityEngine.Rigidbody rigidbody{  get { return UnityPanel.rigidbody; }
 }
	public UnityEngine.Rigidbody2D rigidbody2D{  get { return UnityPanel.rigidbody2D; }
 }
	public System.String tag{  get { return UnityPanel.tag; }
  set{UnityPanel.tag = value; }
 }
	public UnityEngine.Transform transform{  get { return UnityPanel.transform; }
 }
	public System.Boolean useGUILayout{  get { return UnityPanel.useGUILayout; }
  set{UnityPanel.useGUILayout = value; }
 }
	public void Update(float dt, World world) {
frame = World.frame;



	}











}
public class CnvInputField{
public int frame;
public bool JustEntered = true;
private System.String n;
	public int ID;
public CnvInputField(System.String n)
	{JustEntered = false;
 frame = World.frame;
		UnityInputField = UnityInputField.Find(n);
		
}
		public System.Int32 MaxValue{  get { return UnityInputField.MaxValue; }
  set{UnityInputField.MaxValue = value; }
 }
	public System.Int32 MinValue{  get { return UnityInputField.MinValue; }
  set{UnityInputField.MinValue = value; }
 }
	public System.String Text{  get { return UnityInputField.Text; }
  set{UnityInputField.Text = value; }
 }
	public UnityInputField UnityInputField;
	public UnityEngine.Animation animation{  get { return UnityInputField.animation; }
 }
	public UnityEngine.AudioSource audio{  get { return UnityInputField.audio; }
 }
	public UnityEngine.Camera camera{  get { return UnityInputField.camera; }
 }
	public UnityEngine.Collider collider{  get { return UnityInputField.collider; }
 }
	public UnityEngine.Collider2D collider2D{  get { return UnityInputField.collider2D; }
 }
	public UnityEngine.ConstantForce constantForce{  get { return UnityInputField.constantForce; }
 }
	public System.Boolean enabled{  get { return UnityInputField.enabled; }
  set{UnityInputField.enabled = value; }
 }
	public UnityEngine.GameObject gameObject{  get { return UnityInputField.gameObject; }
 }
	public UnityEngine.GUIElement guiElement{  get { return UnityInputField.guiElement; }
 }
	public UnityEngine.GUIText guiText{  get { return UnityInputField.guiText; }
 }
	public UnityEngine.GUITexture guiTexture{  get { return UnityInputField.guiTexture; }
 }
	public UnityEngine.HideFlags hideFlags{  get { return UnityInputField.hideFlags; }
  set{UnityInputField.hideFlags = value; }
 }
	public UnityEngine.HingeJoint hingeJoint{  get { return UnityInputField.hingeJoint; }
 }
	public UnityEngine.Light light{  get { return UnityInputField.light; }
 }
	public System.String name{  get { return UnityInputField.name; }
  set{UnityInputField.name = value; }
 }
	public UnityEngine.ParticleEmitter particleEmitter{  get { return UnityInputField.particleEmitter; }
 }
	public UnityEngine.ParticleSystem particleSystem{  get { return UnityInputField.particleSystem; }
 }
	public UnityEngine.Renderer renderer{  get { return UnityInputField.renderer; }
 }
	public UnityEngine.Rigidbody rigidbody{  get { return UnityInputField.rigidbody; }
 }
	public UnityEngine.Rigidbody2D rigidbody2D{  get { return UnityInputField.rigidbody2D; }
 }
	public System.String tag{  get { return UnityInputField.tag; }
  set{UnityInputField.tag = value; }
 }
	public UnityEngine.Transform transform{  get { return UnityInputField.transform; }
 }
	public System.Boolean useGUILayout{  get { return UnityInputField.useGUILayout; }
  set{UnityInputField.useGUILayout = value; }
 }
	public void Update(float dt, World world) {
frame = World.frame;



	}











}
public class CnvComboBox{
public int frame;
public bool JustEntered = true;
private System.String n;
	public int ID;
public CnvComboBox(System.String n)
	{JustEntered = false;
 frame = World.frame;
		ComboBox = ComboBox.Find(n);
		
}
		public System.Single ButtonDistance{  get { return ComboBox.ButtonDistance; }
 }
	public ComboBox ComboBox;
	public UnityEngine.GameObject RootCanvas{  get { return ComboBox.RootCanvas; }
 }
	public System.String SelectionName{  get { return ComboBox.SelectionName; }
 }
	public UnityEngine.Animation animation{  get { return ComboBox.animation; }
 }
	public UnityEngine.AudioSource audio{  get { return ComboBox.audio; }
 }
	public UnityEngine.Camera camera{  get { return ComboBox.camera; }
 }
	public UnityEngine.Collider collider{  get { return ComboBox.collider; }
 }
	public UnityEngine.Collider2D collider2D{  get { return ComboBox.collider2D; }
 }
	public UnityEngine.ConstantForce constantForce{  get { return ComboBox.constantForce; }
 }
	public System.Boolean enabled{  get { return ComboBox.enabled; }
  set{ComboBox.enabled = value; }
 }
	public UnityEngine.GameObject gameObject{  get { return ComboBox.gameObject; }
 }
	public UnityEngine.GUIElement guiElement{  get { return ComboBox.guiElement; }
 }
	public UnityEngine.GUIText guiText{  get { return ComboBox.guiText; }
 }
	public UnityEngine.GUITexture guiTexture{  get { return ComboBox.guiTexture; }
 }
	public UnityEngine.HideFlags hideFlags{  get { return ComboBox.hideFlags; }
  set{ComboBox.hideFlags = value; }
 }
	public UnityEngine.HingeJoint hingeJoint{  get { return ComboBox.hingeJoint; }
 }
	public UnityEngine.Light light{  get { return ComboBox.light; }
 }
	public System.String name{  get { return ComboBox.name; }
  set{ComboBox.name = value; }
 }
	public UnityEngine.ParticleEmitter particleEmitter{  get { return ComboBox.particleEmitter; }
 }
	public UnityEngine.ParticleSystem particleSystem{  get { return ComboBox.particleSystem; }
 }
	public UnityEngine.Renderer renderer{  get { return ComboBox.renderer; }
 }
	public UnityEngine.Rigidbody rigidbody{  get { return ComboBox.rigidbody; }
 }
	public UnityEngine.Rigidbody2D rigidbody2D{  get { return ComboBox.rigidbody2D; }
 }
	public System.String tag{  get { return ComboBox.tag; }
  set{ComboBox.tag = value; }
 }
	public UnityEngine.Transform transform{  get { return ComboBox.transform; }
 }
	public System.Boolean useGUILayout{  get { return ComboBox.useGUILayout; }
  set{ComboBox.useGUILayout = value; }
 }
	public void Update(float dt, World world) {
frame = World.frame;



	}











}
public class CnvComboList{
public int frame;
public bool JustEntered = true;
private System.String n;
	public int ID;
public CnvComboList(System.String n)
	{JustEntered = false;
 frame = World.frame;
		ComboList = ComboList.Find(n);
		
}
		public System.Single ButtonDistance{  get { return ComboList.ButtonDistance; }
 }
	public System.Collections.Generic.List<UnityEngine.GameObject> Buttons{  get { return ComboList.Buttons; }
 }
	public ComboList ComboList;
	public System.Int32 CurrentSelectionIndex{  get { return ComboList.CurrentSelectionIndex; }
 }
	public UnityEngine.GameObject RootCanvas{  get { return ComboList.RootCanvas; }
 }
	public System.String SelectionName{  get { return ComboList.SelectionName; }
 }
	public UnityEngine.Animation animation{  get { return ComboList.animation; }
 }
	public UnityEngine.AudioSource audio{  get { return ComboList.audio; }
 }
	public UnityEngine.Camera camera{  get { return ComboList.camera; }
 }
	public UnityEngine.Collider collider{  get { return ComboList.collider; }
 }
	public UnityEngine.Collider2D collider2D{  get { return ComboList.collider2D; }
 }
	public UnityEngine.ConstantForce constantForce{  get { return ComboList.constantForce; }
 }
	public System.Boolean enabled{  get { return ComboList.enabled; }
  set{ComboList.enabled = value; }
 }
	public UnityEngine.GameObject gameObject{  get { return ComboList.gameObject; }
 }
	public UnityEngine.GUIElement guiElement{  get { return ComboList.guiElement; }
 }
	public UnityEngine.GUIText guiText{  get { return ComboList.guiText; }
 }
	public UnityEngine.GUITexture guiTexture{  get { return ComboList.guiTexture; }
 }
	public UnityEngine.HideFlags hideFlags{  get { return ComboList.hideFlags; }
  set{ComboList.hideFlags = value; }
 }
	public UnityEngine.HingeJoint hingeJoint{  get { return ComboList.hingeJoint; }
 }
	public UnityEngine.Light light{  get { return ComboList.light; }
 }
	public System.String name{  get { return ComboList.name; }
  set{ComboList.name = value; }
 }
	public UnityEngine.ParticleEmitter particleEmitter{  get { return ComboList.particleEmitter; }
 }
	public UnityEngine.ParticleSystem particleSystem{  get { return ComboList.particleSystem; }
 }
	public UnityEngine.Renderer renderer{  get { return ComboList.renderer; }
 }
	public UnityEngine.Rigidbody rigidbody{  get { return ComboList.rigidbody; }
 }
	public UnityEngine.Rigidbody2D rigidbody2D{  get { return ComboList.rigidbody2D; }
 }
	public System.String tag{  get { return ComboList.tag; }
  set{ComboList.tag = value; }
 }
	public UnityEngine.Transform transform{  get { return ComboList.transform; }
 }
	public System.Boolean useGUILayout{  get { return ComboList.useGUILayout; }
  set{ComboList.useGUILayout = value; }
 }
	public void Update(float dt, World world) {
frame = World.frame;



	}











}
public class Player{
public int frame;
public bool JustEntered = true;
private System.Int32 index;
private GameConstants constants;
private List<System.Int32> symbolSet;
	public int ID;
public Player(System.Int32 index, GameConstants constants, List<System.Int32> symbolSet)
	{JustEntered = false;
 frame = World.frame;
		System.Collections.Generic.List<System.Int32> ___randomizedResources00;
		___randomizedResources00 = GameUtils.RandomizeResources(constants.MaximumStartingResources,4);
		System.Int32 ___uranium02;
		___uranium02 = ___randomizedResources00[0];
		System.Int32 ___plutonium02;
		___plutonium02 = ___randomizedResources00[1];
		System.Int32 ___oil02;
		___oil02 = ___randomizedResources00[2];
		System.Int32 ___iron02;
		___iron02 = ___randomizedResources00[3];
		List<System.Int32> ___symbols00;
		if(((symbolSet.Count) == (0)))
			{
			___symbols00 = (

(GameUtils.randomDistinctIntList(10,0,constants.SymbolCount)).Select(__ContextSymbol145 => new { ___x01 = __ContextSymbol145 })
.Select(__ContextSymbol146 => __ContextSymbol146.___x01)
.ToList<System.Int32>()).ToList<System.Int32>();
			}else
			{
			___symbols00 = symbolSet;
			}
		Symbols = ___symbols00;
		SpentActions = 0;
		Resources = new GameResources(___uranium02,___plutonium02,___oil02,___iron02);
		ReceivedTrades = (

Enumerable.Empty<Trade>()).ToList<Trade>();
		ReceivedMessages = (

Enumerable.Empty<Message>()).ToList<Message>();
		Objectives = (

Enumerable.Empty<Objective>()).ToList<Objective>();
		Name = ("Player") + (GameUtils.IntToString((index) + (1)));
		MiningDrones = (

Enumerable.Empty<Drone>()).ToList<Drone>();
		Index = index;
		IdlingDrones = (

Enumerable.Empty<Drone>()).ToList<Drone>();
		Drones = (

Enumerable.Empty<Drone>()).ToList<Drone>();
		Color = GameUtils.AssignColor(index);
		
}
		public UnityEngine.Color Color;
	public List<Drone> Drones;
	public List<Drone> IdlingDrones;
	public System.Int32 Index;
	public List<Drone> MiningDrones;
	public System.String Name;
	public List<Objective> Objectives;
	public List<Message> ReceivedMessages;
	public List<Trade> ReceivedTrades;
	public GameResources Resources;
	public System.Int32 SpentActions;
	public System.Collections.Generic.List<System.Int32> Symbols;
	public System.Int32 ___receivedUranium10;
	public System.Int32 ___receivedPlutonium10;
	public System.Int32 ___receivedOil10;
	public System.Int32 ___receivedIron10;
	public System.Single count_down13;
	public Player ___currentPlayer26;
	public List<Trade> ___newTrades20;
	public List<Trade> ___updatedTrades20;
	public Player ___currentPlayer37;
	public List<Message> ___newMessages30;
	public List<Message> ___updatedMessages30;
	public System.Int32 ___totalUranium60;
	public System.Int32 ___totalPlutonium60;
	public System.Int32 ___totalOil60;
	public System.Int32 ___totalIron60;
	public System.Int32 ___payedUranium70;
	public System.Int32 ___payedPlutonium70;
	public System.Int32 ___payedOil70;
	public System.Int32 ___payedIron70;
	public System.Int32 ___payedUranium81;
	public System.Int32 ___payedPlutonium81;
	public System.Int32 ___payedOil81;
	public System.Int32 ___payedIron81;
	public List<Planet> ___homePlanetList90;
	public Planet ___homePlanet90;
	public Drone ___startingDrone190;
	public System.Collections.Generic.List<System.Int32> ___randomizedTaskResources100;
	public System.Int32 ___objectiveUranium100;
	public System.Int32 ___objectivePlutonium100;
	public System.Int32 ___objectiveOil100;
	public System.Int32 ___objectiveIron100;
	public Objective ___miningObjective100;
	public void Update(float dt, World world) {
frame = World.frame;		this.Rule4(dt, world);
		this.Rule5(dt, world);
		for(int x0 = 0; x0 < Drones.Count; x0++) { 
			Drones[x0].Update(dt, world);
		}
		for(int x0 = 0; x0 < Objectives.Count; x0++) { 
			Objectives[x0].Update(dt, world);
		}
		for(int x0 = 0; x0 < ReceivedMessages.Count; x0++) { 
			ReceivedMessages[x0].Update(dt, world);
		}
		Resources.Update(dt, world);
		this.Rule0(dt, world);
		this.Rule1(dt, world);
		this.Rule2(dt, world);
		this.Rule3(dt, world);
		this.Rule6(dt, world);
		this.Rule7(dt, world);
		this.Rule8(dt, world);
		this.Rule9(dt, world);
		this.Rule10(dt, world);
	}

	public void Rule4(float dt, World world) 
	{
	IdlingDrones = (

(Drones).Select(__ContextSymbol153 => new { ___drone44 = __ContextSymbol153 })
.Where(__ContextSymbol154 => ((((__ContextSymbol154.___drone44.ScanAction.IsNone) && (__ContextSymbol154.___drone44.GatherAction.IsNone))) && (!(__ContextSymbol154.___drone44.ActionSpent))))
.Select(__ContextSymbol155 => __ContextSymbol155.___drone44)
.ToList<Drone>()).ToList<Drone>();
	}
	

	public void Rule5(float dt, World world) 
	{
	MiningDrones = (

(Drones).Select(__ContextSymbol156 => new { ___drone55 = __ContextSymbol156 })
.Where(__ContextSymbol157 => ((__ContextSymbol157.___drone55.GatherAction.IsSome) && (__ContextSymbol157.___drone55.GatherAction.Value.MinedResources.IsSome)))
.Select(__ContextSymbol158 => __ContextSymbol158.___drone55)
.ToList<Drone>()).ToList<Drone>();
	}
	



	int s0=-1;
	public void Rule0(float dt, World world){ 
	switch (s0)
	{

	case -1:
	if(!(((world.CurrentPlayer) == (this))))
	{

	s0 = -1;
return;	}else
	{

	goto case 1;	}
	case 1:
	SpentActions = 0;
	s0 = 0;
return;
	case 0:
	if(!(!(((world.CurrentPlayer) == (this)))))
	{

	s0 = 0;
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
	if(!(((ReceivedTrades.Count) > (0))))
	{

	s1 = -1;
return;	}else
	{

	goto case 13;	}
	case 13:
	UnityEngine.Debug.Log((this.Name) + (" received new trades. Updating resources..."));
	UnityEngine.Debug.Log(ReceivedTrades.Count);
	___receivedUranium10 = (

(ReceivedTrades).Select(__ContextSymbol159 => new { ___trade11 = __ContextSymbol159 })
.Select(__ContextSymbol160 => __ContextSymbol160.___trade11.Resources.Uranium)
.Aggregate(default(System.Int32), (acc, __x) => acc + __x));
	___receivedPlutonium10 = (

(ReceivedTrades).Select(__ContextSymbol162 => new { ___trade12 = __ContextSymbol162 })
.Select(__ContextSymbol163 => __ContextSymbol163.___trade12.Resources.Plutonium)
.Aggregate(default(System.Int32), (acc, __x) => acc + __x));
	___receivedOil10 = (

(ReceivedTrades).Select(__ContextSymbol165 => new { ___trade13 = __ContextSymbol165 })
.Select(__ContextSymbol166 => __ContextSymbol166.___trade13.Resources.Oil)
.Aggregate(default(System.Int32), (acc, __x) => acc + __x));
	___receivedIron10 = (

(ReceivedTrades).Select(__ContextSymbol168 => new { ___trade14 = __ContextSymbol168 })
.Select(__ContextSymbol169 => __ContextSymbol169.___trade14.Resources.Iron)
.Aggregate(default(System.Int32), (acc, __x) => acc + __x));
	UnityEngine.Debug.Log(___receivedUranium10);
	UnityEngine.Debug.Log(___receivedPlutonium10);
	UnityEngine.Debug.Log(___receivedOil10);
	UnityEngine.Debug.Log(___receivedIron10);
	ReceivedTrades = ReceivedTrades;
	Resources = new GameResources((Resources.Uranium) + (___receivedUranium10),(Resources.Plutonium) + (___receivedPlutonium10),(Resources.Oil) + (___receivedOil10),(Resources.Iron) + (___receivedIron10));
	s1 = 1;
return;
	case 1:
	count_down13 = dt;
	goto case 2;
	case 2:
	if(((count_down13) > (0f)))
	{

	count_down13 = ((count_down13) - (dt));
	s1 = 2;
return;	}else
	{

	goto case 0;	}
	case 0:
	ReceivedTrades = (

Enumerable.Empty<Trade>()).ToList<Trade>();
	Resources = Resources;
	s1 = -1;
return;	
	default: return;}}
	

	int s2=-1;
	public void Rule2(float dt, World world){ 
	switch (s2)
	{

	case -1:
	if(!(((world.CurrentPlayer) == (this))))
	{

	s2 = -1;
return;	}else
	{

	goto case 4;	}
	case 4:
	___currentPlayer26 = world.CurrentPlayer;
	___newTrades20 = (

(world.GUI.LogMenu.Trades).Select(__ContextSymbol172 => new { ___trade25 = __ContextSymbol172 })
.Where(__ContextSymbol173 => ((__ContextSymbol173.___trade25.Receiver) == (this)))
.Select(__ContextSymbol174 => __ContextSymbol174.___trade25)
.ToList<Trade>()).ToList<Trade>();
	___updatedTrades20 = (___newTrades20).Concat(ReceivedTrades).ToList<Trade>();
	ReceivedTrades = (___newTrades20).Concat(ReceivedTrades).ToList<Trade>();
	s2 = 0;
return;
	case 0:
	if(!(!(((___currentPlayer26) == (world.CurrentPlayer)))))
	{

	s2 = 0;
return;	}else
	{

	s2 = -1;
return;	}	
	default: return;}}
	

	int s3=-1;
	public void Rule3(float dt, World world){ 
	switch (s3)
	{

	case -1:
	if(!(((world.CurrentPlayer) == (this))))
	{

	s3 = -1;
return;	}else
	{

	goto case 4;	}
	case 4:
	___currentPlayer37 = world.CurrentPlayer;
	___newMessages30 = (

(world.GUI.MessageMenu.OutgoingMessages).Select(__ContextSymbol175 => new { ___message34 = __ContextSymbol175 })
.Where(__ContextSymbol176 => ((__ContextSymbol176.___message34.Receiver) == (this)))
.Select(__ContextSymbol177 => __ContextSymbol177.___message34)
.ToList<Message>()).ToList<Message>();
	___updatedMessages30 = (___newMessages30).Concat(ReceivedMessages).ToList<Message>();
	ReceivedMessages = (___newMessages30).Concat(ReceivedMessages).ToList<Message>();
	s3 = 0;
return;
	case 0:
	if(!(!(((___currentPlayer37) == (world.CurrentPlayer)))))
	{

	s3 = 0;
return;	}else
	{

	s3 = -1;
return;	}	
	default: return;}}
	

	int s6=-1;
	public void Rule6(float dt, World world){ 
	switch (s6)
	{

	case -1:
	if(!(((MiningDrones.Count) > (0))))
	{

	s6 = -1;
return;	}else
	{

	goto case 5;	}
	case 5:
	___totalUranium60 = (

(MiningDrones).Select(__ContextSymbol178 => new { ___drone66 = __ContextSymbol178 })
.Select(__ContextSymbol179 => new {___gather60 = __ContextSymbol179.___drone66.GatherAction.Value, prev = __ContextSymbol179 })
.Select(__ContextSymbol180 => new {___resources60 = __ContextSymbol180.___gather60.MinedResources.Value, prev = __ContextSymbol180 })
.Select(__ContextSymbol181 => __ContextSymbol181.prev.prev.___drone66.GatherAction.Value.MinedResources.Value.Uranium)
.Aggregate(default(System.Int32), (acc, __x) => acc + __x));
	___totalPlutonium60 = (

(MiningDrones).Select(__ContextSymbol183 => new { ___drone67 = __ContextSymbol183 })
.Select(__ContextSymbol184 => __ContextSymbol184.___drone67.GatherAction.Value.MinedResources.Value.Plutonium)
.Aggregate(default(System.Int32), (acc, __x) => acc + __x));
	___totalOil60 = (

(MiningDrones).Select(__ContextSymbol186 => new { ___drone68 = __ContextSymbol186 })
.Select(__ContextSymbol187 => __ContextSymbol187.___drone68.GatherAction.Value.MinedResources.Value.Oil)
.Aggregate(default(System.Int32), (acc, __x) => acc + __x));
	___totalIron60 = (

(MiningDrones).Select(__ContextSymbol189 => new { ___drone69 = __ContextSymbol189 })
.Select(__ContextSymbol190 => __ContextSymbol190.___drone69.GatherAction.Value.MinedResources.Value.Iron)
.Aggregate(default(System.Int32), (acc, __x) => acc + __x));
	Resources = new GameResources((Resources.Uranium) + (___totalUranium60),(Resources.Plutonium) + (___totalPlutonium60),(Resources.Oil) + (___totalOil60),(Resources.Iron) + (___totalIron60));
	s6 = 0;
return;
	case 0:
	if(!(((MiningDrones.Count) == (0))))
	{

	s6 = 0;
return;	}else
	{

	s6 = -1;
return;	}	
	default: return;}}
	

	int s7=-1;
	public void Rule7(float dt, World world){ 
	switch (s7)
	{

	case -1:
	if(!(((world.GUI.LogMenu.SendButton.Clicked) && (((world.CurrentPlayer) == (this))))))
	{

	s7 = -1;
return;	}else
	{

	goto case 4;	}
	case 4:
	___payedUranium70 = GameUtils.StringToInt(world.GUI.LogMenu.UraniumInputField.Text);
	___payedPlutonium70 = GameUtils.StringToInt(world.GUI.LogMenu.PlutoniumInputField.Text);
	___payedOil70 = GameUtils.StringToInt(world.GUI.LogMenu.OilInputField.Text);
	___payedIron70 = GameUtils.StringToInt(world.GUI.LogMenu.IronInputField.Text);
	Resources = new GameResources((Resources.Uranium) - (___payedUranium70),(Resources.Plutonium) - (___payedPlutonium70),(Resources.Oil) - (___payedOil70),(Resources.Iron) - (___payedIron70));
	s7 = -1;
return;	
	default: return;}}
	

	int s8=-1;
	public void Rule8(float dt, World world){ 
	switch (s8)
	{

	case -1:
	if(!(((world.GUI.MessageMenu.SendButton.Clicked) && (((world.CurrentPlayer) == (this))))))
	{

	s8 = -1;
return;	}else
	{

	goto case 4;	}
	case 4:
	___payedUranium81 = GameUtils.StringToInt(world.GUI.MessageMenu.UraniumInputField.Text);
	___payedPlutonium81 = GameUtils.StringToInt(world.GUI.MessageMenu.PlutoniumInputField.Text);
	___payedOil81 = GameUtils.StringToInt(world.GUI.MessageMenu.OilInputField.Text);
	___payedIron81 = GameUtils.StringToInt(world.GUI.MessageMenu.IronInputField.Text);
	Resources = new GameResources((Resources.Uranium) - (___payedUranium81),(Resources.Plutonium) - (___payedPlutonium81),(Resources.Oil) - (___payedOil81),(Resources.Iron) - (___payedIron81));
	s8 = -1;
return;	
	default: return;}}
	

	int s9=-1;
	public void Rule9(float dt, World world){ 
	switch (s9)
	{

	case -1:
	___homePlanetList90 = (

(world.StarSystems).Select(__ContextSymbol192 => new { ___system90 = __ContextSymbol192 })
.SelectMany(__ContextSymbol193=> (__ContextSymbol193.___system90.Planets).Select(__ContextSymbol194 => new { ___planet90 = __ContextSymbol194,
                                                      prev = __ContextSymbol193 })
.Where(__ContextSymbol195 => ((__ContextSymbol195.___planet90.Owner.IsSome) && (((__ContextSymbol195.___planet90.Owner.Value) == (this)))))
.Select(__ContextSymbol196 => __ContextSymbol196.___planet90)
.ToList<Planet>())).ToList<Planet>();
	if(((___homePlanetList90.Count) > (0)))
	{

	goto case 6;	}else
	{

	goto case 7;	}
	case 6:
	___homePlanet90 = ___homePlanetList90.Head();
	___startingDrone190 = new Drone(this,new UnityEngine.Vector3((___homePlanet90.Position.x) + (5f),___homePlanet90.Position.y,___homePlanet90.Position.z));
	Drones = (

(new Cons<Drone>(___startingDrone190,(new Empty<Drone>()).ToList<Drone>())).ToList<Drone>()).ToList<Drone>();
	s9 = 9;
return;
	case 9:
	if(!(false))
	{

	s9 = 9;
return;	}else
	{

	s9 = -1;
return;	}
	case 7:
	Drones = Drones;
	s9 = -1;
return;	
	default: return;}}
	

	int s10=-1;
	public void Rule10(float dt, World world){ 
	switch (s10)
	{

	case -1:
	___randomizedTaskResources100 = GameUtils.RandomizeResources(world.GameConstants.TotalObjectiveResources,4);
	___objectiveUranium100 = (___randomizedTaskResources100)[0];
	___objectivePlutonium100 = (___randomizedTaskResources100)[1];
	___objectiveOil100 = (___randomizedTaskResources100)[2];
	___objectiveIron100 = (___randomizedTaskResources100)[3];
	___miningObjective100 = new Objective((new Just<GameResources>(new GameResources(___objectiveUranium100,___objectivePlutonium100,___objectiveOil100,___objectiveIron100))),this);
	Objectives = (

(new Cons<Objective>(___miningObjective100,(new Empty<Objective>()).ToList<Objective>())).ToList<Objective>()).ToList<Objective>();
	s10 = 0;
return;
	case 0:
	if(!(false))
	{

	s10 = 0;
return;	}else
	{

	s10 = -1;
return;	}	
	default: return;}}
	





}
public class Trade{
public int frame;
public bool JustEntered = true;
private Player receiver;
private GameResources resources;
	public int ID;
public Trade(Player receiver, GameResources resources)
	{JustEntered = false;
 frame = World.frame;
		Resources = resources;
		Receiver = receiver;
		
}
		public Player Receiver;
	public GameResources Resources;
	public void Update(float dt, World world) {
frame = World.frame;

		Resources.Update(dt, world);


	}











}
public class Message{
public int frame;
public bool JustEntered = true;
private Player receiver;
private Player sender;
private List<System.Int32> content;
private List<System.Int32> images;
private System.String name;
	public int ID;
public Message(Player receiver, Player sender, List<System.Int32> content, List<System.Int32> images, System.String name)
	{JustEntered = false;
 frame = World.frame;
		Sender = sender;
		Receiver = receiver;
		Read = false;
		Name = name;
		MessageContent = content;
		Images = images;
		
}
		public List<System.Int32> Images;
	public List<System.Int32> MessageContent;
	public System.String Name;
	public System.Boolean Read;
	public Player Receiver;
	public Player Sender;
	public void Update(float dt, World world) {
frame = World.frame;

		this.Rule0(dt, world);

	}





	int s0=-1;
	public void Rule0(float dt, World world){ 
	switch (s0)
	{

	case -1:
	if(!(((world.GUI.LogMenu.SelectedMessage.IsSome) && (((world.GUI.LogMenu.SelectedMessage.Value) == (this))))))
	{

	s0 = -1;
return;	}else
	{

	goto case 1;	}
	case 1:
	Read = true;
	s0 = 0;
return;
	case 0:
	if(!(false))
	{

	s0 = 0;
return;	}else
	{

	s0 = -1;
return;	}	
	default: return;}}
	






}
public class Objective{
public int frame;
public bool JustEntered = true;
private Option<GameResources> miningTask;
private Player owner;
	public int ID;
public Objective(Option<GameResources> miningTask, Player owner)
	{JustEntered = false;
 frame = World.frame;
		Text = "";
		Owner = owner;
		MiningTask = miningTask;
		Completed = false;
		
}
		public System.Boolean Completed;
	public Option<GameResources> MiningTask;
	public Player Owner;
	public System.String Text;
	public void Update(float dt, World world) {
frame = World.frame;

if(MiningTask.IsSome){ 		MiningTask.Value.Update(dt, world);
 } 
		this.Rule0(dt, world);
		this.Rule1(dt, world);
	}





	int s0=-1;
	public void Rule0(float dt, World world){ 
	switch (s0)
	{

	case -1:
	if(!(MiningTask.IsSome))
	{

	s0 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	Text = (((((((((((((((("- Collect at least ") + (GameUtils.IntToString(this.MiningTask.Value.Uranium)))) + (" Uranium, "))) + (GameUtils.IntToString(this.MiningTask.Value.Plutonium)))) + (" Plutonium, "))) + (GameUtils.IntToString(this.MiningTask.Value.Iron)))) + (" Iron, "))) + (GameUtils.IntToString(this.MiningTask.Value.Oil)))) + (" Oil"));
	s0 = -1;
return;	
	default: return;}}
	

	int s1=-1;
	public void Rule1(float dt, World world){ 
	switch (s1)
	{

	case -1:
	if(!(MiningTask.IsSome))
	{

	s1 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	Completed = ((((((((((Owner.Resources.Uranium) > (this.MiningTask.Value.Uranium))) || (((Owner.Resources.Uranium) == (this.MiningTask.Value.Uranium))))) && (((((Owner.Resources.Plutonium) > (this.MiningTask.Value.Plutonium))) || (((Owner.Resources.Plutonium) == (this.MiningTask.Value.Plutonium))))))) && (((((Owner.Resources.Oil) > (this.MiningTask.Value.Oil))) || (((Owner.Resources.Oil) == (this.MiningTask.Value.Oil))))))) && (((((Owner.Resources.Iron) > (this.MiningTask.Value.Iron))) || (((Owner.Resources.Iron) == (this.MiningTask.Value.Iron))))));
	s1 = -1;
return;	
	default: return;}}
	





}
public class GameResources{
public int frame;
public bool JustEntered = true;
private System.Int32 u;
private System.Int32 p;
private System.Int32 o;
private System.Int32 i;
	public int ID;
public GameResources(System.Int32 u, System.Int32 p, System.Int32 o, System.Int32 i)
	{JustEntered = false;
 frame = World.frame;
		Uranium = u;
		Plutonium = p;
		Oil = o;
		Iron = i;
		
}
		public System.Int32 Iron;
	public System.Int32 Oil;
	public System.Int32 Plutonium;
	public System.Int32 Uranium;
	public void Update(float dt, World world) {
frame = World.frame;



	}











}
public class Effect{
public int frame;
public bool JustEntered = true;
private System.String material;
private UnityEngine.Vector3 position;
private UnityEngine.Vector3 scale;
private UnityEngine.Vector3 velocity;
private UnityEngine.Vector3 scaleVariation;
private System.Boolean fading;
private System.Single lifetime;
	public int ID;
public Effect(System.String material, UnityEngine.Vector3 position, UnityEngine.Vector3 scale, UnityEngine.Vector3 velocity, UnityEngine.Vector3 scaleVariation, System.Boolean fading, System.Single lifetime)
	{JustEntered = false;
 frame = World.frame;
		UnityEngine.Quaternion ___startingRotation00;
		___startingRotation00 = UnityEngine.Quaternion.LookRotation(velocity);
		Velocity = velocity;
		UnityEffect = UnityEffect.Instantiate(material,position,___startingRotation00,scale);
		ScaleVariation = scaleVariation;
		Lifetime = lifetime;
		Fading = fading;
		Deleted = false;
		
}
		public System.Single Alpha{  get { return UnityEffect.Alpha; }
  set{UnityEffect.Alpha = value; }
 }
	public System.Boolean Deleted;
	public System.Boolean Destroyed{  get { return UnityEffect.Destroyed; }
  set{UnityEffect.Destroyed = value; }
 }
	public UnityEngine.Vector3 Down{  get { return UnityEffect.Down; }
 }
	public System.Boolean Fading;
	public UnityEngine.Vector3 Forward{  get { return UnityEffect.Forward; }
 }
	public System.Single Lifetime;
	public System.String MaterialName{  get { return UnityEffect.MaterialName; }
  set{UnityEffect.MaterialName = value; }
 }
	public UnityEngine.Vector3 Position{  get { return UnityEffect.Position; }
  set{UnityEffect.Position = value; }
 }
	public UnityEngine.Quaternion Rotation{  get { return UnityEffect.Rotation; }
  set{UnityEffect.Rotation = value; }
 }
	public UnityEngine.Vector3 Scale{  get { return UnityEffect.Scale; }
  set{UnityEffect.Scale = value; }
 }
	public UnityEngine.Vector3 ScaleVariation;
	public UnityEffect UnityEffect;
	public UnityEngine.Vector3 Velocity;
	public UnityEngine.Animation animation{  get { return UnityEffect.animation; }
 }
	public UnityEngine.AudioSource audio{  get { return UnityEffect.audio; }
 }
	public UnityEngine.Camera camera{  get { return UnityEffect.camera; }
 }
	public UnityEngine.Collider collider{  get { return UnityEffect.collider; }
 }
	public UnityEngine.Collider2D collider2D{  get { return UnityEffect.collider2D; }
 }
	public UnityEngine.ConstantForce constantForce{  get { return UnityEffect.constantForce; }
 }
	public System.Boolean enabled{  get { return UnityEffect.enabled; }
  set{UnityEffect.enabled = value; }
 }
	public UnityEngine.GameObject gameObject{  get { return UnityEffect.gameObject; }
 }
	public UnityEngine.GUIElement guiElement{  get { return UnityEffect.guiElement; }
 }
	public UnityEngine.GUIText guiText{  get { return UnityEffect.guiText; }
 }
	public UnityEngine.GUITexture guiTexture{  get { return UnityEffect.guiTexture; }
 }
	public UnityEngine.HideFlags hideFlags{  get { return UnityEffect.hideFlags; }
  set{UnityEffect.hideFlags = value; }
 }
	public UnityEngine.HingeJoint hingeJoint{  get { return UnityEffect.hingeJoint; }
 }
	public UnityEngine.Light light{  get { return UnityEffect.light; }
 }
	public System.String name{  get { return UnityEffect.name; }
  set{UnityEffect.name = value; }
 }
	public UnityEngine.ParticleEmitter particleEmitter{  get { return UnityEffect.particleEmitter; }
 }
	public UnityEngine.ParticleSystem particleSystem{  get { return UnityEffect.particleSystem; }
 }
	public UnityEngine.Renderer renderer{  get { return UnityEffect.renderer; }
 }
	public UnityEngine.Rigidbody rigidbody{  get { return UnityEffect.rigidbody; }
 }
	public UnityEngine.Rigidbody2D rigidbody2D{  get { return UnityEffect.rigidbody2D; }
 }
	public System.String tag{  get { return UnityEffect.tag; }
  set{UnityEffect.tag = value; }
 }
	public UnityEngine.Transform transform{  get { return UnityEffect.transform; }
 }
	public System.Boolean useGUILayout{  get { return UnityEffect.useGUILayout; }
  set{UnityEffect.useGUILayout = value; }
 }
	public System.Single count_down14;
	public System.Single count_down15;
	public UnityEngine.Quaternion ___rotation40;
	public void Update(float dt, World world) {
frame = World.frame;

		this.Rule0(dt, world);
		this.Rule1(dt, world);
		this.Rule2(dt, world);
		this.Rule3(dt, world);
		this.Rule4(dt, world);
	}





	int s0=-1;
	public void Rule0(float dt, World world){ 
	switch (s0)
	{

	case -1:
	if(!(Destroyed))
	{

	s0 = -1;
return;	}else
	{

	goto case 1;	}
	case 1:
	count_down14 = dt;
	goto case 2;
	case 2:
	if(((count_down14) > (0f)))
	{

	count_down14 = ((count_down14) - (dt));
	s0 = 2;
return;	}else
	{

	goto case 0;	}
	case 0:
	Deleted = true;
	s0 = -1;
return;	
	default: return;}}
	

	int s1=-1;
	public void Rule1(float dt, World world){ 
	switch (s1)
	{

	case -1:
	count_down15 = Lifetime;
	goto case 2;
	case 2:
	if(((count_down15) > (0f)))
	{

	count_down15 = ((count_down15) - (dt));
	s1 = 2;
return;	}else
	{

	goto case 0;	}
	case 0:
	Destroyed = true;
	s1 = -1;
return;	
	default: return;}}
	

	int s2=-1;
	public void Rule2(float dt, World world){ 
	switch (s2)
	{

	case -1:
	if(!(((!(Destroyed)) && (Fading))))
	{

	s2 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	Alpha = ((Alpha) - (((((1f) / (Lifetime))) * (dt))));
	s2 = -1;
return;	
	default: return;}}
	

	int s3=-1;
	public void Rule3(float dt, World world){ 
	switch (s3)
	{

	case -1:
	if(!(!(Destroyed)))
	{

	s3 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	Scale = ((Scale) + (((ScaleVariation) * (dt))));
	s3 = -1;
return;	
	default: return;}}
	

	int s4=-1;
	public void Rule4(float dt, World world){ 
	switch (s4)
	{

	case -1:
	if(!(!(Destroyed)))
	{

	s4 = -1;
return;	}else
	{

	goto case 1;	}
	case 1:
	___rotation40 = UnityEngine.Quaternion.LookRotation(Velocity);
	Position = ((Position) + (((Velocity) * (dt))));
	Rotation = ___rotation40;
	s4 = -1;
return;	
	default: return;}}
	





}
public class BaseAction{
public int frame;
public bool JustEntered = true;
private System.Int32 duration;
private Planet target;
private System.Single distance;
private Drone caller;
	public int ID;
public BaseAction(System.Int32 duration, Planet target, System.Single distance, Drone caller)
	{JustEntered = false;
 frame = World.frame;
		Target = target;
		InteractionDistance = distance;
		InProgress = false;
		Duration = duration;
		Caller = caller;
		
}
		public Drone Caller;
	public System.Int32 Duration;
	public System.Boolean InProgress;
	public System.Single InteractionDistance;
	public Planet Target;
	public void Update(float dt, World world) {
frame = World.frame;



	}











}
public class Gather{
public int frame;
public bool JustEntered = true;
private Planet target;
private Drone caller;
private System.Int32 miningRate;
	public int ID;
public Gather(Planet target, Drone caller, System.Int32 miningRate)
	{JustEntered = false;
 frame = World.frame;
		MiningRate = miningRate;
		MinedResources = (new Nothing<GameResources>());
		Effects = (

Enumerable.Empty<Effect>()).ToList<Effect>();
		Action = new BaseAction(3,target,5f,caller);
		
}
		public BaseAction Action;
	public List<Effect> Effects;
	public Option<GameResources> MinedResources;
	public System.Int32 MiningRate;
	public Drone ___caller00;
	public Planet ___target00;
	public GameResources ___resources01;
	public System.Single ___interactionDistance00;
	public System.Int32 ___i09;
	public System.Int32 counter200;
	public UnityEngine.Vector3 ___forward00;
	public List<Effect> ___newEffects00;
	public System.Single count_down16;
	public System.Int32 ___collectedUranium00;
	public System.Int32 ___collectedPlutonium00;
	public System.Int32 ___collectedOil00;
	public System.Int32 ___collectedIron00;
	public Planet ___target01;
	public Drone ___caller01;
	public Player ___owner01;
	public GameResources ___targetResources00;
	public GameResources ___playerResources00;
	public List<Trade> ___playerTrades01;
	public void Update(float dt, World world) {
frame = World.frame;		this.Rule1(dt, world);

		Action.Update(dt, world);
		for(int x0 = 0; x0 < Effects.Count; x0++) { 
			Effects[x0].Update(dt, world);
		}
if(MinedResources.IsSome){ 		MinedResources.Value.Update(dt, world);
 } 
		this.Rule0(dt, world);

	}

	public void Rule1(float dt, World world) 
	{
	Effects = (

(Effects).Select(__ContextSymbol201 => new { ___effect10 = __ContextSymbol201 })
.Where(__ContextSymbol202 => !(__ContextSymbol202.___effect10.Deleted))
.Select(__ContextSymbol203 => __ContextSymbol203.___effect10)
.ToList<Effect>()).ToList<Effect>();
	}
	




	int s0=-1;
	public void Rule0(float dt, World world){ 
	switch (s0)
	{

	case -1:
	___caller00 = Action.Caller;
	goto case 32;
	case 32:
	if(!(((((((Action.Duration) > (0))) && (((world.CurrentPlayer) == (___caller00.Owner))))) && (!(___caller00.ActionSpent)))))
	{

	s0 = 32;
return;	}else
	{

	goto case 2;	}
	case 2:
	if(((((((!(((this.Action.Target.Resources.Uranium) > (0)))) && (!(((this.Action.Target.Resources.Plutonium) > (0)))))) && (!(((this.Action.Target.Resources.Oil) > (0)))))) && (!(((this.Action.Target.Resources.Iron) > (0))))))
	{

	goto case 0;	}else
	{

	goto case 1;	}
	case 0:
	Action.Duration = 0;
	Action.InProgress = false;
	Effects = (

Enumerable.Empty<Effect>()).ToList<Effect>();
	MinedResources = (new Nothing<GameResources>());
	s0 = -1;
return;
	case 1:
	___target00 = Action.Target;
	___resources01 = ___target00.Resources;
	___interactionDistance00 = Action.InteractionDistance;
	goto case 28;
	case 28:
	if(!(!(((UnityEngine.Vector3.Distance(___caller00.Position,___target00.Position)) > (___interactionDistance00)))))
	{

	s0 = 28;
return;	}else
	{

	goto case 27;	}
	case 27:
	Action.Duration = Action.Duration;
	Action.InProgress = true;
	Effects = Effects;
	MinedResources = MinedResources;
	s0 = 19;
return;
	case 19:
	
	counter200 = -1;
	if((((Enumerable.Range(1,((1) + (((20) - (1))))).ToList<System.Int32>()).Count) == (0)))
	{

	goto case 18;	}else
	{

	___i09 = (Enumerable.Range(1,((1) + (((20) - (1))))).ToList<System.Int32>())[0];
	goto case 20;	}
	case 20:
	counter200 = ((counter200) + (1));
	if((((((Enumerable.Range(1,((1) + (((20) - (1))))).ToList<System.Int32>()).Count) == (counter200))) || (((counter200) > ((Enumerable.Range(1,((1) + (((20) - (1))))).ToList<System.Int32>()).Count)))))
	{

	goto case 18;	}else
	{

	___i09 = (Enumerable.Range(1,((1) + (((20) - (1))))).ToList<System.Int32>())[counter200];
	goto case 21;	}
	case 21:
	___forward00 = ___caller00.Forward;
	___newEffects00 = (

(Enumerable.Range(1,(1) + ((3) - (1))).ToList<System.Int32>()).Select(__ContextSymbol205 => new { ___j00 = __ContextSymbol205 })
.Select(__ContextSymbol206 => new {___randomVelocity00 = (GameUtils.RotateVector(UnityEngine.Random.Range(-25f,25f),___forward00,Vector3.up)) * (9f), prev = __ContextSymbol206 })
.Select(__ContextSymbol207 => new Effect("UnitTextures/Material/particle",(___caller00.Position) + ((___forward00) * (1.5f)),new UnityEngine.Vector3(0.025f,0.025f,0.025f),__ContextSymbol207.___randomVelocity00,new UnityEngine.Vector3(0f,0f,0f),true,0.25f))
.ToList<Effect>()).ToList<Effect>();
	Action.Duration = Action.Duration;
	Action.InProgress = Action.InProgress;
	Effects = (___newEffects00).Concat(Effects).ToList<Effect>();
	MinedResources = MinedResources;
	s0 = 22;
return;
	case 22:
	count_down16 = 0.1f;
	goto case 23;
	case 23:
	if(((count_down16) > (0f)))
	{

	count_down16 = ((count_down16) - (dt));
	s0 = 23;
return;	}else
	{

	s0 = 20;
return;	}
	case 18:
	if(!(((Effects.Count) == (0))))
	{

	s0 = 18;
return;	}else
	{

	goto case 17;	}
	case 17:
	___collectedUranium00 = UnityEngine.Mathf.Min(MiningRate,___resources01.Uranium);
	___collectedPlutonium00 = UnityEngine.Mathf.Min(MiningRate,___resources01.Plutonium);
	___collectedOil00 = UnityEngine.Mathf.Min(MiningRate,___resources01.Oil);
	___collectedIron00 = UnityEngine.Mathf.Min(MiningRate,___resources01.Iron);
	Action.Duration = Action.Duration;
	Action.InProgress = false;
	Effects = Effects;
	MinedResources = (new Just<GameResources>(new GameResources(___collectedUranium00,___collectedPlutonium00,___collectedOil00,___collectedIron00)));
	s0 = 12;
return;
	case 12:
	___target01 = Action.Target;
	___caller01 = Action.Caller;
	___owner01 = ___caller01.Owner;
	___targetResources00 = ___target01.Resources;
	___playerResources00 = ___owner01.Resources;
	___playerTrades01 = ___owner01.ReceivedTrades;
	goto case 6;
	case 6:
	if(!(((!(((___targetResources00) == (___target01.Resources)))) && (!(((___playerResources00) == (___owner01.Resources)))))))
	{

	s0 = 6;
return;	}else
	{

	goto case 5;	}
	case 5:
	UnityEngine.Debug.Log("Resetting mined resources...");
	Action.Duration = ((Action.Duration) - (1));
	Action.InProgress = Action.InProgress;
	Effects = Effects;
	MinedResources = (new Nothing<GameResources>());
	s0 = -1;
return;	
	default: return;}}
	






}
public class Scan{
public int frame;
public bool JustEntered = true;
private Planet target;
private Drone caller;
	public int ID;
public Scan(Planet target, Drone caller)
	{JustEntered = false;
 frame = World.frame;
		Effects = (

Enumerable.Empty<Effect>()).ToList<Effect>();
		Action = new BaseAction(1,target,5f,caller);
		
}
		public BaseAction Action;
	public List<Effect> Effects;
	public Drone ___caller02;
	public Planet ___target02;
	public System.Single ___interactionDistance01;
	public System.Int32 ___i010;
	public System.Int32 counter30;
	public UnityEngine.Vector3 ___forward01;
	public Effect ___newEffect00;
	public System.Single count_down17;
	public void Update(float dt, World world) {
frame = World.frame;		this.Rule1(dt, world);

		Action.Update(dt, world);
		for(int x0 = 0; x0 < Effects.Count; x0++) { 
			Effects[x0].Update(dt, world);
		}
		this.Rule0(dt, world);

	}

	public void Rule1(float dt, World world) 
	{
	Effects = (

(Effects).Select(__ContextSymbol210 => new { ___effect11 = __ContextSymbol210 })
.Where(__ContextSymbol211 => !(__ContextSymbol211.___effect11.Deleted))
.Select(__ContextSymbol212 => __ContextSymbol212.___effect11)
.ToList<Effect>()).ToList<Effect>();
	}
	




	int s0=-1;
	public void Rule0(float dt, World world){ 
	switch (s0)
	{

	case -1:
	___caller02 = Action.Caller;
	goto case 14;
	case 14:
	if(!(((((((Action.Duration) > (0))) && (((world.CurrentPlayer) == (___caller02.Owner))))) && (!(___caller02.ActionSpent)))))
	{

	s0 = 14;
return;	}else
	{

	goto case 13;	}
	case 13:
	___target02 = Action.Target;
	___interactionDistance01 = Action.InteractionDistance;
	goto case 11;
	case 11:
	if(!(!(((UnityEngine.Vector3.Distance(___caller02.Position,___target02.Position)) > (___interactionDistance01)))))
	{

	s0 = 11;
return;	}else
	{

	goto case 10;	}
	case 10:
	Action.Duration = Action.Duration;
	Action.InProgress = true;
	Effects = Effects;
	s0 = 2;
return;
	case 2:
	
	counter30 = -1;
	if((((Enumerable.Range(1,((1) + (((6) - (1))))).ToList<System.Int32>()).Count) == (0)))
	{

	goto case 1;	}else
	{

	___i010 = (Enumerable.Range(1,((1) + (((6) - (1))))).ToList<System.Int32>())[0];
	goto case 3;	}
	case 3:
	counter30 = ((counter30) + (1));
	if((((((Enumerable.Range(1,((1) + (((6) - (1))))).ToList<System.Int32>()).Count) == (counter30))) || (((counter30) > ((Enumerable.Range(1,((1) + (((6) - (1))))).ToList<System.Int32>()).Count)))))
	{

	goto case 1;	}else
	{

	___i010 = (Enumerable.Range(1,((1) + (((6) - (1))))).ToList<System.Int32>())[counter30];
	goto case 4;	}
	case 4:
	___forward01 = ___caller02.Forward;
	___newEffect00 = new Effect("UnitTextures/Material/radio_wave",(___caller02.Position) + ((___forward01) * (1.5f)),new UnityEngine.Vector3(0.25f,0.25f,0.25f),(___caller02.Forward) * (3f),new UnityEngine.Vector3(0.25f,0f,0.25f),true,2f);
	Action.Duration = Action.Duration;
	Action.InProgress = Action.InProgress;
	Effects = new Cons<Effect>(___newEffect00, (Effects)).ToList<Effect>();
	s0 = 5;
return;
	case 5:
	count_down17 = 0.25f;
	goto case 6;
	case 6:
	if(((count_down17) > (0f)))
	{

	count_down17 = ((count_down17) - (dt));
	s0 = 6;
return;	}else
	{

	s0 = 3;
return;	}
	case 1:
	if(!(((Effects.Count) == (0))))
	{

	s0 = 1;
return;	}else
	{

	goto case 0;	}
	case 0:
	Action.Duration = ((Action.Duration) - (1));
	Action.InProgress = false;
	Effects = Effects;
	s0 = -1;
return;	
	default: return;}}
	






}
public class Drone{
public int frame;
public bool JustEntered = true;
private Player owner;
private UnityEngine.Vector3 pos;
	public int ID;
public Drone(Player owner, UnityEngine.Vector3 pos)
	{JustEntered = false;
 frame = World.frame;
		UnityDrone = UnityDrone.Instantiate(pos);
		Speed = 7.5f;
		ScanAction = (new Nothing<Scan>());
		RotationVelocity = new UnityEngine.Vector3(0f,4f,0f);
		Owner = owner;
		MoveDistance = 15f;
		Id = UnityEngine.Random.Range(1,1000000);
		GatherAction = (new Nothing<Gather>());
		Busy = false;
		ActionSpent = false;
		
}
		public System.Boolean ActionSpent;
	public System.Boolean Busy;
	public UnityEngine.Vector3 Forward{  get { return UnityDrone.Forward; }
 }
	public Option<Gather> GatherAction;
	public System.Int32 Id;
	public System.Single MoveDistance;
	public Player Owner;
	public UnityEngine.Color OwnerColor{  get { return UnityDrone.OwnerColor; }
  set{UnityDrone.OwnerColor = value; }
 }
	public UnityEngine.Vector3 Position{  get { return UnityDrone.Position; }
  set{UnityDrone.Position = value; }
 }
	public UnityEngine.Quaternion Rotation{  get { return UnityDrone.Rotation; }
  set{UnityDrone.Rotation = value; }
 }
	public UnityEngine.Vector3 RotationVelocity;
	public Option<Scan> ScanAction;
	public System.Single Speed;
	public UnityDrone UnityDrone;
	public System.Boolean Visible{  get { return UnityDrone.Visible; }
  set{UnityDrone.Visible = value; }
 }
	public UnityEngine.Animation animation{  get { return UnityDrone.animation; }
 }
	public UnityEngine.AudioSource audio{  get { return UnityDrone.audio; }
 }
	public UnityEngine.Camera camera{  get { return UnityDrone.camera; }
 }
	public UnityEngine.Collider collider{  get { return UnityDrone.collider; }
 }
	public UnityEngine.Collider2D collider2D{  get { return UnityDrone.collider2D; }
 }
	public UnityEngine.ConstantForce constantForce{  get { return UnityDrone.constantForce; }
 }
	public System.Boolean enabled{  get { return UnityDrone.enabled; }
  set{UnityDrone.enabled = value; }
 }
	public UnityEngine.GameObject gameObject{  get { return UnityDrone.gameObject; }
 }
	public UnityEngine.GUIElement guiElement{  get { return UnityDrone.guiElement; }
 }
	public UnityEngine.GUIText guiText{  get { return UnityDrone.guiText; }
 }
	public UnityEngine.GUITexture guiTexture{  get { return UnityDrone.guiTexture; }
 }
	public UnityEngine.HideFlags hideFlags{  get { return UnityDrone.hideFlags; }
  set{UnityDrone.hideFlags = value; }
 }
	public UnityEngine.HingeJoint hingeJoint{  get { return UnityDrone.hingeJoint; }
 }
	public UnityEngine.Light light{  get { return UnityDrone.light; }
 }
	public System.String name{  get { return UnityDrone.name; }
  set{UnityDrone.name = value; }
 }
	public UnityEngine.ParticleEmitter particleEmitter{  get { return UnityDrone.particleEmitter; }
 }
	public UnityEngine.ParticleSystem particleSystem{  get { return UnityDrone.particleSystem; }
 }
	public UnityEngine.Renderer renderer{  get { return UnityDrone.renderer; }
 }
	public UnityEngine.Rigidbody rigidbody{  get { return UnityDrone.rigidbody; }
 }
	public UnityEngine.Rigidbody2D rigidbody2D{  get { return UnityDrone.rigidbody2D; }
 }
	public System.String tag{  get { return UnityDrone.tag; }
  set{UnityDrone.tag = value; }
 }
	public UnityEngine.Transform transform{  get { return UnityDrone.transform; }
 }
	public System.Boolean useGUILayout{  get { return UnityDrone.useGUILayout; }
  set{UnityDrone.useGUILayout = value; }
 }
	public UnityEngine.Vector3 ___lookDirection10;
	public UnityEngine.Quaternion ___rotation11;
	public UnityEngine.Vector3 ___lookDirection21;
	public UnityEngine.Quaternion ___rotation22;
	public UnityEngine.Vector3 ___startPosition30;
	public BaseAction ___currentAction30;
	public UnityEngine.Vector3 ___direction30;
	public UnityEngine.Vector3 ___velocity30;
	public UnityEngine.Vector3 ___updatedPos30;
	public UnityEngine.Quaternion ___updatedRot30;
	public System.Boolean ___outOfRange30;
	public UnityEngine.Vector3 ___startPosition41;
	public BaseAction ___currentAction41;
	public UnityEngine.Vector3 ___direction41;
	public UnityEngine.Vector3 ___velocity41;
	public UnityEngine.Vector3 ___updatedPos41;
	public UnityEngine.Quaternion ___updatedRot41;
	public System.Boolean ___outOfRange41;
	public System.Single count_down18;
	public System.Single count_down19;
	public Player ___currentPlayer118;
	public Player ___currentPlayer129;
	public void Update(float dt, World world) {
frame = World.frame;

if(GatherAction.IsSome){ 		GatherAction.Value.Update(dt, world);
 } 
if(ScanAction.IsSome){ 		ScanAction.Value.Update(dt, world);
 } 
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
		this.Rule11(dt, world);
		this.Rule12(dt, world);
	}





	int s0=-1;
	public void Rule0(float dt, World world){ 
	switch (s0)
	{

	case -1:
	OwnerColor = Owner.Color;
	s0 = 0;
return;
	case 0:
	if(!(false))
	{

	s0 = 0;
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
	if(!(((((GatherAction.IsSome) && (!(((UnityEngine.Vector3.Distance(Position,GatherAction.Value.Action.Target.Position)) > (GatherAction.Value.Action.InteractionDistance)))))) && (!(ActionSpent)))))
	{

	s1 = -1;
return;	}else
	{

	goto case 4;	}
	case 4:
	___lookDirection10 = ((GatherAction.Value.Action.Target.Position) - (Position));
	___rotation11 = UnityEngine.Quaternion.LookRotation(___lookDirection10);
	Rotation = ___rotation11;
	ActionSpent = true;
	Busy = true;
	s1 = 1;
return;
	case 1:
	if(!(((GatherAction.IsNone) || (!(GatherAction.Value.Action.InProgress)))))
	{

	s1 = 1;
return;	}else
	{

	goto case 0;	}
	case 0:
	Rotation = Rotation;
	ActionSpent = true;
	Busy = false;
	s1 = -1;
return;	
	default: return;}}
	

	int s2=-1;
	public void Rule2(float dt, World world){ 
	switch (s2)
	{

	case -1:
	if(!(((((ScanAction.IsSome) && (!(((UnityEngine.Vector3.Distance(Position,ScanAction.Value.Action.Target.Position)) > (ScanAction.Value.Action.InteractionDistance)))))) && (!(ActionSpent)))))
	{

	s2 = -1;
return;	}else
	{

	goto case 4;	}
	case 4:
	___lookDirection21 = ((ScanAction.Value.Action.Target.Position) - (Position));
	___rotation22 = UnityEngine.Quaternion.LookRotation(___lookDirection21);
	Rotation = ___rotation22;
	ActionSpent = true;
	Busy = true;
	s2 = 1;
return;
	case 1:
	if(!(((ScanAction.IsNone) || (!(ScanAction.Value.Action.InProgress)))))
	{

	s2 = 1;
return;	}else
	{

	goto case 0;	}
	case 0:
	Rotation = Rotation;
	ActionSpent = true;
	Busy = false;
	s2 = -1;
return;	
	default: return;}}
	

	int s3=-1;
	public void Rule3(float dt, World world){ 
	switch (s3)
	{

	case -1:
	if(!(((((GatherAction.IsSome) && (((UnityEngine.Vector3.Distance(Position,GatherAction.Value.Action.Target.Position)) > (GatherAction.Value.Action.InteractionDistance))))) && (!(ActionSpent)))))
	{

	s3 = -1;
return;	}else
	{

	goto case 10;	}
	case 10:
	___startPosition30 = Position;
	___currentAction30 = GatherAction.Value.Action;
	goto case 2;
	case 2:
	if(!(((((UnityEngine.Vector3.Distance(Position,___currentAction30.Target.Position)) > (___currentAction30.InteractionDistance))) && (((MoveDistance) > (UnityEngine.Vector3.Distance(___startPosition30,Position)))))))
	{

	goto case 1;	}else
	{

	goto case 3;	}
	case 3:
	___direction30 = UnityEngine.Vector3.Normalize((___currentAction30.Target.Position) - (Position));
	___velocity30 = ((___direction30) * (Speed));
	___updatedPos30 = ((Position) + (((___velocity30) * (dt))));
	___updatedRot30 = UnityEngine.Quaternion.LookRotation(___velocity30);
	Position = ___updatedPos30;
	Rotation = ___updatedRot30;
	ActionSpent = true;
	Busy = true;
	s3 = 2;
return;
	case 1:
	___outOfRange30 = ((UnityEngine.Vector3.Distance(Position,___currentAction30.Target.Position)) > (___currentAction30.InteractionDistance));
	Position = Position;
	Rotation = Rotation;
	ActionSpent = ___outOfRange30;
	Busy = !(___outOfRange30);
	s3 = -1;
return;	
	default: return;}}
	

	int s4=-1;
	public void Rule4(float dt, World world){ 
	switch (s4)
	{

	case -1:
	if(!(((((ScanAction.IsSome) && (((UnityEngine.Vector3.Distance(Position,ScanAction.Value.Action.Target.Position)) > (ScanAction.Value.Action.InteractionDistance))))) && (!(ActionSpent)))))
	{

	s4 = -1;
return;	}else
	{

	goto case 10;	}
	case 10:
	___startPosition41 = Position;
	___currentAction41 = ScanAction.Value.Action;
	goto case 2;
	case 2:
	if(!(((((UnityEngine.Vector3.Distance(Position,___currentAction41.Target.Position)) > (___currentAction41.InteractionDistance))) && (((MoveDistance) > (UnityEngine.Vector3.Distance(___startPosition41,Position)))))))
	{

	goto case 1;	}else
	{

	goto case 3;	}
	case 3:
	___direction41 = UnityEngine.Vector3.Normalize((___currentAction41.Target.Position) - (Position));
	___velocity41 = ((___direction41) * (Speed));
	___updatedPos41 = ((Position) + (((___velocity41) * (dt))));
	___updatedRot41 = UnityEngine.Quaternion.LookRotation(___velocity41);
	Position = ___updatedPos41;
	Rotation = ___updatedRot41;
	ActionSpent = true;
	Busy = true;
	s4 = 2;
return;
	case 1:
	___outOfRange41 = ((UnityEngine.Vector3.Distance(Position,___currentAction41.Target.Position)) > (___currentAction41.InteractionDistance));
	Position = Position;
	Rotation = Rotation;
	ActionSpent = ___outOfRange41;
	Busy = !(___outOfRange41);
	s4 = -1;
return;	
	default: return;}}
	

	int s5=-1;
	public void Rule5(float dt, World world){ 
	switch (s5)
	{

	case -1:
	if(!(((((((ScanAction.IsNone) && (GatherAction.IsNone))) && (world.GUI.PlanetMenu.Mine.Clicked))) && (((Owner) == (world.CurrentPlayer))))))
	{

	s5 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	GatherAction = (new Just<Gather>(new Gather(world.SelectedPlanet.Value,this,world.GameConstants.MiningRate)));
	s5 = -1;
return;	
	default: return;}}
	

	int s6=-1;
	public void Rule6(float dt, World world){ 
	switch (s6)
	{

	case -1:
	if(!(((((((((ScanAction.IsNone) && (GatherAction.IsNone))) && (!(ActionSpent)))) && (world.GUI.PlanetMenu.ScanForResources.Clicked))) && (((Owner) == (world.CurrentPlayer))))))
	{

	s6 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	ScanAction = (new Just<Scan>(new Scan(world.SelectedPlanet.Value,this)));
	s6 = -1;
return;	
	default: return;}}
	

	int s7=-1;
	public void Rule7(float dt, World world){ 
	switch (s7)
	{

	case -1:
	if(!(((((((GatherAction.IsSome) && (world.GUI.PlanetMenu.CancelOrders.Clicked))) && (((Owner) == (world.CurrentPlayer))))) && (((this.GatherAction.Value.Action.Target) == (world.SelectedPlanet.Value))))))
	{

	s7 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	GatherAction = (new Nothing<Gather>());
	s7 = -1;
return;	
	default: return;}}
	

	int s8=-1;
	public void Rule8(float dt, World world){ 
	switch (s8)
	{

	case -1:
	if(!(((((((ScanAction.IsSome) && (world.GUI.PlanetMenu.CancelOrders.Clicked))) && (((Owner) == (world.CurrentPlayer))))) && (((this.ScanAction.Value.Action.Target) == (world.SelectedPlanet.Value))))))
	{

	s8 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	ScanAction = (new Nothing<Scan>());
	s8 = -1;
return;	
	default: return;}}
	

	int s9=-1;
	public void Rule9(float dt, World world){ 
	switch (s9)
	{

	case -1:
	if(!(((GatherAction.IsSome) && (((GatherAction.Value.Action.Duration) == (0))))))
	{

	s9 = -1;
return;	}else
	{

	goto case 1;	}
	case 1:
	count_down18 = 0.1f;
	goto case 2;
	case 2:
	if(((count_down18) > (0f)))
	{

	count_down18 = ((count_down18) - (dt));
	s9 = 2;
return;	}else
	{

	goto case 0;	}
	case 0:
	GatherAction = (new Nothing<Gather>());
	s9 = -1;
return;	
	default: return;}}
	

	int s10=-1;
	public void Rule10(float dt, World world){ 
	switch (s10)
	{

	case -1:
	if(!(((ScanAction.IsSome) && (((ScanAction.Value.Action.Duration) == (0))))))
	{

	s10 = -1;
return;	}else
	{

	goto case 1;	}
	case 1:
	count_down19 = 0.1f;
	goto case 2;
	case 2:
	if(((count_down19) > (0f)))
	{

	count_down19 = ((count_down19) - (dt));
	s10 = 2;
return;	}else
	{

	goto case 0;	}
	case 0:
	ScanAction = (new Nothing<Scan>());
	s10 = -1;
return;	
	default: return;}}
	

	int s11=-1;
	public void Rule11(float dt, World world){ 
	switch (s11)
	{

	case -1:
	___currentPlayer118 = world.CurrentPlayer;
	Visible = ((Owner) == (___currentPlayer118));
	s11 = 0;
return;
	case 0:
	if(!(!(((___currentPlayer118) == (world.CurrentPlayer)))))
	{

	s11 = 0;
return;	}else
	{

	s11 = -1;
return;	}	
	default: return;}}
	

	int s12=-1;
	public void Rule12(float dt, World world){ 
	switch (s12)
	{

	case -1:
	if(!(((world.CurrentPlayer) == (Owner))))
	{

	s12 = -1;
return;	}else
	{

	goto case 2;	}
	case 2:
	___currentPlayer129 = world.CurrentPlayer;
	ActionSpent = false;
	s12 = 0;
return;
	case 0:
	if(!(!(((___currentPlayer129) == (world.CurrentPlayer)))))
	{

	s12 = 0;
return;	}else
	{

	s12 = -1;
return;	}	
	default: return;}}
	





}
public class Planet{
public int frame;
public bool JustEntered = true;
private UnityEngine.Vector3 pos;
private UnityEngine.Vector3 center;
private Option<Player> owner;
	public int ID;
public Planet(UnityEngine.Vector3 pos, UnityEngine.Vector3 center, Option<Player> owner)
	{JustEntered = false;
 frame = World.frame;
		System.Int32 ___plutonium03;
		___plutonium03 = GameUtils.SpawnResource();
		System.Int32 ___uranium03;
		___uranium03 = GameUtils.SpawnResource();
		System.Int32 ___iron03;
		___iron03 = GameUtils.SpawnResource();
		System.Int32 ___oil03;
		___oil03 = GameUtils.SpawnResource();
		UnityPlanet = UnityPlanet.Instantiate(pos);
		ScannedBy = (

Enumerable.Empty<Player>()).ToList<Player>();
		RotationVelocity = new UnityEngine.Vector3(0f,UnityEngine.Random.Range(3f,7f),0f);
		RevolutionVelocity = 0f;
		Resources = new GameResources(___uranium03,___plutonium03,___oil03,___iron03);
		Owner = owner;
		MiningDrones = (

Enumerable.Empty<Drone>()).ToList<Drone>();
		Center = center;
		
}
		public UnityEngine.Vector3 Center;
	public System.Boolean ClickedOver{  get { return UnityPlanet.ClickedOver; }
 }
	public List<Drone> MiningDrones;
	public Option<Player> Owner;
	public UnityEngine.Color OwnerColor{  get { return UnityPlanet.OwnerColor; }
  set{UnityPlanet.OwnerColor = value; }
 }
	public UnityEngine.Vector3 Position{  get { return UnityPlanet.Position; }
  set{UnityPlanet.Position = value; }
 }
	public GameResources Resources;
	public System.Single RevolutionVelocity;
	public UnityEngine.Quaternion Rotation{  get { return UnityPlanet.Rotation; }
  set{UnityPlanet.Rotation = value; }
 }
	public UnityEngine.Vector3 RotationVelocity;
	public List<Player> ScannedBy;
	public System.Boolean Selected{  get { return UnityPlanet.Selected; }
  set{UnityPlanet.Selected = value; }
 }
	public UnityEngine.Color TargetingPlayerColor{  get { return UnityPlanet.TargetingPlayerColor; }
  set{UnityPlanet.TargetingPlayerColor = value; }
 }
	public UnityPlanet UnityPlanet;
	public UnityEngine.Animation animation{  get { return UnityPlanet.animation; }
 }
	public UnityEngine.AudioSource audio{  get { return UnityPlanet.audio; }
 }
	public UnityEngine.Camera camera{  get { return UnityPlanet.camera; }
 }
	public UnityEngine.Collider collider{  get { return UnityPlanet.collider; }
 }
	public UnityEngine.Collider2D collider2D{  get { return UnityPlanet.collider2D; }
 }
	public UnityEngine.ConstantForce constantForce{  get { return UnityPlanet.constantForce; }
 }
	public System.Boolean enabled{  get { return UnityPlanet.enabled; }
  set{UnityPlanet.enabled = value; }
 }
	public UnityEngine.GameObject gameObject{  get { return UnityPlanet.gameObject; }
 }
	public UnityEngine.GUIElement guiElement{  get { return UnityPlanet.guiElement; }
 }
	public UnityEngine.GUIText guiText{  get { return UnityPlanet.guiText; }
 }
	public UnityEngine.GUITexture guiTexture{  get { return UnityPlanet.guiTexture; }
 }
	public UnityEngine.HideFlags hideFlags{  get { return UnityPlanet.hideFlags; }
  set{UnityPlanet.hideFlags = value; }
 }
	public UnityEngine.HingeJoint hingeJoint{  get { return UnityPlanet.hingeJoint; }
 }
	public UnityEngine.Light light{  get { return UnityPlanet.light; }
 }
	public System.String name{  get { return UnityPlanet.name; }
  set{UnityPlanet.name = value; }
 }
	public UnityEngine.ParticleEmitter particleEmitter{  get { return UnityPlanet.particleEmitter; }
 }
	public UnityEngine.ParticleSystem particleSystem{  get { return UnityPlanet.particleSystem; }
 }
	public UnityEngine.Renderer renderer{  get { return UnityPlanet.renderer; }
 }
	public UnityEngine.Rigidbody rigidbody{  get { return UnityPlanet.rigidbody; }
 }
	public UnityEngine.Rigidbody2D rigidbody2D{  get { return UnityPlanet.rigidbody2D; }
 }
	public System.String tag{  get { return UnityPlanet.tag; }
  set{UnityPlanet.tag = value; }
 }
	public UnityEngine.Transform transform{  get { return UnityPlanet.transform; }
 }
	public System.Boolean useGUILayout{  get { return UnityPlanet.useGUILayout; }
  set{UnityPlanet.useGUILayout = value; }
 }
	public System.Int32 ___totalUranium01;
	public System.Int32 ___totalPlutonium01;
	public System.Int32 ___totalOil01;
	public System.Int32 ___totalIron01;
	public Player ___currentOwner50;
	public void Update(float dt, World world) {
frame = World.frame;		this.Rule1(dt, world);
		this.Rule2(dt, world);
		this.Rule6(dt, world);
		this.Rule7(dt, world);
		Resources.Update(dt, world);
		this.Rule0(dt, world);
		this.Rule3(dt, world);
		this.Rule4(dt, world);
		this.Rule5(dt, world);
		this.Rule8(dt, world);
	}

	public void Rule1(float dt, World world) 
	{
	MiningDrones = (

(world.CurrentPlayer.MiningDrones).Select(__ContextSymbol217 => new { ___drone114 = __ContextSymbol217 })
.Where(__ContextSymbol218 => ((__ContextSymbol218.___drone114.GatherAction.IsSome) && (!(((UnityEngine.Vector3.Distance(__ContextSymbol218.___drone114.Position,this.Position)) > (__ContextSymbol218.___drone114.GatherAction.Value.Action.InteractionDistance))))))
.Select(__ContextSymbol219 => __ContextSymbol219.___drone114)
.ToList<Drone>()).ToList<Drone>();
	}
	

	public void Rule2(float dt, World world) 
	{
	List<Drone> ___scanningDrones20;
	___scanningDrones20 = (

(world.CurrentPlayer.Drones).Select(__ContextSymbol220 => new { ___drone215 = __ContextSymbol220 })
.Where(__ContextSymbol221 => ((((((__ContextSymbol221.___drone215.ScanAction.IsSome) && (((__ContextSymbol221.___drone215.ScanAction.Value.Action.Target) == (this))))) && (!(((UnityEngine.Vector3.Distance(Position,__ContextSymbol221.___drone215.Position)) > (__ContextSymbol221.___drone215.ScanAction.Value.Action.InteractionDistance)))))) && (((__ContextSymbol221.___drone215.ScanAction.Value.Action.Duration) == (0)))))
.Select(__ContextSymbol222 => __ContextSymbol222.___drone215)
.ToList<Drone>()).ToList<Drone>();
	List<Player> ___newScanners20;
	if(((___scanningDrones20.Count) > (0)))
		{
		___newScanners20 = new Cons<Player>(___scanningDrones20.Head().Owner, (ScannedBy)).ToList();
		}else
		{
		___newScanners20 = ScannedBy;
		}
	ScannedBy = ___newScanners20;
	}
	

	public void Rule6(float dt, World world) 
	{
	List<Drone> ___targetingDrones61;
	___targetingDrones61 = (

(world.CurrentPlayer.Drones).Select(__ContextSymbol223 => new { ___drone616 = __ContextSymbol223 })
.Where(__ContextSymbol224 => ((((__ContextSymbol224.___drone616.ScanAction.IsSome) && (((__ContextSymbol224.___drone616.ScanAction.Value.Action.Target) == (this))))) || (((__ContextSymbol224.___drone616.GatherAction.IsSome) && (((__ContextSymbol224.___drone616.GatherAction.Value.Action.Target) == (this)))))))
.Select(__ContextSymbol225 => __ContextSymbol225.___drone616)
.ToList<Drone>()).ToList<Drone>();
	if(((___targetingDrones61.Count) == (0)))
		{
		UnityPlanet.TargetingPlayerColor = new UnityEngine.Color(0,0,0,0);
		}else
		{
		UnityPlanet.TargetingPlayerColor = world.CurrentPlayer.Color;
		}
	}
	

	public void Rule7(float dt, World world) 
	{
	if(Owner.IsNone)
		{
		UnityPlanet.OwnerColor = new UnityEngine.Color(0,0,0,0);
		}else
		{
		UnityPlanet.OwnerColor = Owner.Value.Color;
		}
	}
	



	int s0=-1;
	public void Rule0(float dt, World world){ 
	switch (s0)
	{

	case -1:
	if(!(((MiningDrones.Count) > (0))))
	{

	s0 = -1;
return;	}else
	{

	goto case 5;	}
	case 5:
	___totalUranium01 = (

(MiningDrones).Select(__ContextSymbol226 => new { ___drone010 = __ContextSymbol226 })
.Select(__ContextSymbol227 => __ContextSymbol227.___drone010.GatherAction.Value.MinedResources.Value.Uranium)
.Aggregate(default(System.Int32), (acc, __x) => acc + __x));
	___totalPlutonium01 = (

(MiningDrones).Select(__ContextSymbol229 => new { ___drone011 = __ContextSymbol229 })
.Select(__ContextSymbol230 => __ContextSymbol230.___drone011.GatherAction.Value.MinedResources.Value.Plutonium)
.Aggregate(default(System.Int32), (acc, __x) => acc + __x));
	___totalOil01 = (

(MiningDrones).Select(__ContextSymbol232 => new { ___drone012 = __ContextSymbol232 })
.Select(__ContextSymbol233 => __ContextSymbol233.___drone012.GatherAction.Value.MinedResources.Value.Oil)
.Aggregate(default(System.Int32), (acc, __x) => acc + __x));
	___totalIron01 = (

(MiningDrones).Select(__ContextSymbol235 => new { ___drone013 = __ContextSymbol235 })
.Select(__ContextSymbol236 => __ContextSymbol236.___drone013.GatherAction.Value.MinedResources.Value.Iron)
.Aggregate(default(System.Int32), (acc, __x) => acc + __x));
	Resources = new GameResources((Resources.Uranium) - (___totalUranium01),(Resources.Plutonium) - (___totalPlutonium01),(Resources.Oil) - (___totalOil01),(Resources.Iron) - (___totalIron01));
	s0 = 0;
return;
	case 0:
	if(!(((MiningDrones.Count) == (0))))
	{

	s0 = 0;
return;	}else
	{

	s0 = -1;
return;	}	
	default: return;}}
	

	int s3=-1;
	public void Rule3(float dt, World world){ 
	switch (s3)
	{

	case -1:
	if(!(!(world.GamePaused)))
	{

	s3 = -1;
return;	}else
	{

	goto case 1;	}
	case 1:
	UnityPlanet.Revolute(UnityPlanet,Center,(RevolutionVelocity) * (dt));
	RevolutionVelocity = RevolutionVelocity;
	s3 = -1;
return;	
	default: return;}}
	

	int s4=-1;
	public void Rule4(float dt, World world){ 
	switch (s4)
	{

	case -1:
	if(!(!(world.GamePaused)))
	{

	s4 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	Rotation = ((UnityEngine.Quaternion.Euler((RotationVelocity) * (dt))) * (Rotation));
	s4 = -1;
return;	
	default: return;}}
	

	int s5=-1;
	public void Rule5(float dt, World world){ 
	switch (s5)
	{

	case -1:
	if(!(Owner.IsSome))
	{

	s5 = -1;
return;	}else
	{

	goto case 2;	}
	case 2:
	___currentOwner50 = Owner.Value;
	ScannedBy = new Cons<Player>(___currentOwner50, (ScannedBy)).ToList<Player>();
	s5 = 0;
return;
	case 0:
	if(!(((Owner.IsNone) || (!(((___currentOwner50) == (Owner.Value)))))))
	{

	s5 = 0;
return;	}else
	{

	s5 = -1;
return;	}	
	default: return;}}
	

	int s8=-1;
	public void Rule8(float dt, World world){ 
	switch (s8)
	{

	case -1:
	if(!(((((UnityEngine.Input.GetMouseButtonDown(0)) && (!(world.GUI.OnGUI)))) && (!(world.GamePaused)))))
	{

	s8 = -1;
return;	}else
	{

	goto case 2;	}
	case 2:
	if(UnityPlanet.ClickedOver)
	{

	goto case 0;	}else
	{

	goto case 1;	}
	case 0:
	Selected = true;
	world.SelectedPlanet = (new Just<Planet>(this));
	s8 = -1;
return;
	case 1:
	if(((world.SelectedPlanet.IsSome) && (!(((world.SelectedPlanet.Value) == (this))))))
	{

	goto case 4;	}else
	{

	goto case 5;	}
	case 4:
	Selected = false;
	world.SelectedPlanet = world.SelectedPlanet;
	s8 = -1;
return;
	case 5:
	Selected = false;
	world.SelectedPlanet = (new Nothing<Planet>());
	s8 = -1;
return;	
	default: return;}}
	





}
public class Star{
public int frame;
public bool JustEntered = true;
private UnityEngine.Vector3 pos;
	public int ID;
public Star(UnityEngine.Vector3 pos)
	{JustEntered = false;
 frame = World.frame;
		UnityStar = UnityStar.Instantiate(pos);
		
}
		public UnityEngine.Vector3 Position{  get { return UnityStar.Position; }
  set{UnityStar.Position = value; }
 }
	public UnityStar UnityStar;
	public UnityEngine.Animation animation{  get { return UnityStar.animation; }
 }
	public UnityEngine.AudioSource audio{  get { return UnityStar.audio; }
 }
	public UnityEngine.Camera camera{  get { return UnityStar.camera; }
 }
	public UnityEngine.Collider collider{  get { return UnityStar.collider; }
 }
	public UnityEngine.Collider2D collider2D{  get { return UnityStar.collider2D; }
 }
	public UnityEngine.ConstantForce constantForce{  get { return UnityStar.constantForce; }
 }
	public System.Boolean enabled{  get { return UnityStar.enabled; }
  set{UnityStar.enabled = value; }
 }
	public UnityEngine.GameObject gameObject{  get { return UnityStar.gameObject; }
 }
	public UnityEngine.GUIElement guiElement{  get { return UnityStar.guiElement; }
 }
	public UnityEngine.GUIText guiText{  get { return UnityStar.guiText; }
 }
	public UnityEngine.GUITexture guiTexture{  get { return UnityStar.guiTexture; }
 }
	public UnityEngine.HideFlags hideFlags{  get { return UnityStar.hideFlags; }
  set{UnityStar.hideFlags = value; }
 }
	public UnityEngine.HingeJoint hingeJoint{  get { return UnityStar.hingeJoint; }
 }
	public UnityEngine.Light light{  get { return UnityStar.light; }
 }
	public System.String name{  get { return UnityStar.name; }
  set{UnityStar.name = value; }
 }
	public UnityEngine.ParticleEmitter particleEmitter{  get { return UnityStar.particleEmitter; }
 }
	public UnityEngine.ParticleSystem particleSystem{  get { return UnityStar.particleSystem; }
 }
	public UnityEngine.Renderer renderer{  get { return UnityStar.renderer; }
 }
	public UnityEngine.Rigidbody rigidbody{  get { return UnityStar.rigidbody; }
 }
	public UnityEngine.Rigidbody2D rigidbody2D{  get { return UnityStar.rigidbody2D; }
 }
	public System.String tag{  get { return UnityStar.tag; }
  set{UnityStar.tag = value; }
 }
	public UnityEngine.Transform transform{  get { return UnityStar.transform; }
 }
	public System.Boolean useGUILayout{  get { return UnityStar.useGUILayout; }
  set{UnityStar.useGUILayout = value; }
 }
	public void Update(float dt, World world) {
frame = World.frame;



	}











}
public class StarSystem{
public int frame;
public bool JustEntered = true;
private UnityEngine.Vector3 starPosition;
private GameConstants constants;
private Option<Player> owner;
	public int ID;
public StarSystem(UnityEngine.Vector3 starPosition, GameConstants constants, Option<Player> owner)
	{JustEntered = false;
 frame = World.frame;
		Star ___star00;
		___star00 = new Star(starPosition);
		System.Int32 ___maxPlanets00;
		___maxPlanets00 = UnityEngine.Random.Range(constants.MinPlanetsPerSystem,constants.MaxPlanetsPerSystem);
		System.Int32 ___ownerIndex00;
		if(owner.IsSome)
			{
			___ownerIndex00 = UnityEngine.Random.Range(0,___maxPlanets00);
			}else
			{
			___ownerIndex00 = -1;
			}
		List<UnityEngine.Vector3> ___randomPositions01;
		___randomPositions01 = (

(Enumerable.Range(1,(1) + ((___maxPlanets00) - (1))).ToList<System.Int32>()).Select(__ContextSymbol238 => new { ___i011 = __ContextSymbol238 })
.Select(__ContextSymbol239 => new {___minr01 = (constants.MinDistanceFromStar) + (((__ContextSymbol239.___i011) - (1)) * (constants.MinDistanceBetweenPlanets)), prev = __ContextSymbol239 })
.Select(__ContextSymbol240 => new {___posx02 = UnityEngine.Random.Range(__ContextSymbol240.___minr01,(__ContextSymbol240.___minr01) + (2f)), prev = __ContextSymbol240 })
.Select(__ContextSymbol241 => new {___posz02 = UnityEngine.Random.Range(__ContextSymbol241.prev.___minr01,(__ContextSymbol241.prev.___minr01) + (2f)), prev = __ContextSymbol241 })
.Select(__ContextSymbol242 => new {___posx03 = Utils.IfThenElse<System.Single>((()=> ((UnityEngine.Random.Range(0f,1f)) > (0.5f))), (()=>	__ContextSymbol242.prev.___posx02
),(()=>	(__ContextSymbol242.prev.___posx02) * (-1f)
)), prev = __ContextSymbol242 })
.Select(__ContextSymbol243 => new {___posz03 = Utils.IfThenElse<System.Single>((()=> ((UnityEngine.Random.Range(0f,1f)) > (0.5f))), (()=>	__ContextSymbol243.prev.___posz02
),(()=>	(__ContextSymbol243.prev.___posz02) * (-1f)
)), prev = __ContextSymbol243 })
.Select(__ContextSymbol244 => new {___posx04 = (starPosition.x) + (__ContextSymbol244.prev.___posx03), prev = __ContextSymbol244 })
.Select(__ContextSymbol245 => new {___posz04 = (starPosition.z) + (__ContextSymbol245.prev.___posz03), prev = __ContextSymbol245 })
.Select(__ContextSymbol246 => new UnityEngine.Vector3(__ContextSymbol246.prev.___posx04,0f,__ContextSymbol246.___posz04))
.ToList<UnityEngine.Vector3>()).ToList<UnityEngine.Vector3>();
		List<Planet> ___generatedPlanets00;
		___generatedPlanets00 = (

(Enumerable.Range(1,(1) + ((___randomPositions01.Count) - (1))).ToList<System.Int32>()).Select(__ContextSymbol247 => new { ___i012 = __ContextSymbol247 })
.Select(__ContextSymbol248 => new {___startingOwner00 = Utils.IfThenElse<Option<Player>>((()=> ((((___ownerIndex00) > (-1))) && (((___ownerIndex00) == ((__ContextSymbol248.___i012) - (1)))))), (()=>	owner
),(()=>	(new Nothing<Player>())
)), prev = __ContextSymbol248 })
.Select(__ContextSymbol249 => new Planet(___randomPositions01[(__ContextSymbol249.prev.___i012) - (1)],starPosition,__ContextSymbol249.___startingOwner00))
.ToList<Planet>()).ToList<Planet>();
		Star = ___star00;
		Planets = ___generatedPlanets00;
		
}
		public List<Planet> Planets;
	public Star Star;
	public void Update(float dt, World world) {
frame = World.frame;

		for(int x0 = 0; x0 < Planets.Count; x0++) { 
			Planets[x0].Update(dt, world);
		}
		Star.Update(dt, world);


	}











}
public class GameCamera{
public int frame;
public bool JustEntered = true;
	public int ID;
public GameCamera()
	{JustEntered = false;
 frame = World.frame;
		UnityCamera = UnityCamera.CreateMainCamera();
		Sensitivity = 0.25f;
		ScreenWidth = Screen.width;
		ScreenHeight = Screen.height;
		MouseSensitivity = 12f;
		MousePosition = Vector3.zero;
		BoundaryY = (ScreenHeight) / (25);
		BoundaryX = (ScreenWidth) / (25);
		
}
		public System.Int32 BoundaryX;
	public System.Int32 BoundaryY;
	public UnityEngine.Vector3 CameraPosition{  get { return UnityCamera.CameraPosition; }
  set{UnityCamera.CameraPosition = value; }
 }
	public System.Single CameraSize{  get { return UnityCamera.CameraSize; }
  set{UnityCamera.CameraSize = value; }
 }
	public UnityEngine.Vector3 MousePosition;
	public System.Single MouseSensitivity;
	public System.Int32 ScreenHeight;
	public System.Int32 ScreenWidth;
	public System.Single Sensitivity;
	public UnityCamera UnityCamera;
	public UnityEngine.Animation animation{  get { return UnityCamera.animation; }
 }
	public UnityEngine.AudioSource audio{  get { return UnityCamera.audio; }
 }
	public UnityEngine.Camera camera{  get { return UnityCamera.camera; }
 }
	public UnityEngine.Collider collider{  get { return UnityCamera.collider; }
 }
	public UnityEngine.Collider2D collider2D{  get { return UnityCamera.collider2D; }
 }
	public UnityEngine.ConstantForce constantForce{  get { return UnityCamera.constantForce; }
 }
	public System.Boolean enabled{  get { return UnityCamera.enabled; }
  set{UnityCamera.enabled = value; }
 }
	public UnityEngine.GameObject gameObject{  get { return UnityCamera.gameObject; }
 }
	public UnityEngine.GUIElement guiElement{  get { return UnityCamera.guiElement; }
 }
	public UnityEngine.GUIText guiText{  get { return UnityCamera.guiText; }
 }
	public UnityEngine.GUITexture guiTexture{  get { return UnityCamera.guiTexture; }
 }
	public UnityEngine.HideFlags hideFlags{  get { return UnityCamera.hideFlags; }
  set{UnityCamera.hideFlags = value; }
 }
	public UnityEngine.HingeJoint hingeJoint{  get { return UnityCamera.hingeJoint; }
 }
	public UnityEngine.Light light{  get { return UnityCamera.light; }
 }
	public System.String name{  get { return UnityCamera.name; }
  set{UnityCamera.name = value; }
 }
	public UnityEngine.ParticleEmitter particleEmitter{  get { return UnityCamera.particleEmitter; }
 }
	public UnityEngine.ParticleSystem particleSystem{  get { return UnityCamera.particleSystem; }
 }
	public UnityEngine.Renderer renderer{  get { return UnityCamera.renderer; }
 }
	public UnityEngine.Rigidbody rigidbody{  get { return UnityCamera.rigidbody; }
 }
	public UnityEngine.Rigidbody2D rigidbody2D{  get { return UnityCamera.rigidbody2D; }
 }
	public System.String tag{  get { return UnityCamera.tag; }
  set{UnityCamera.tag = value; }
 }
	public UnityEngine.Transform transform{  get { return UnityCamera.transform; }
 }
	public System.Boolean useGUILayout{  get { return UnityCamera.useGUILayout; }
  set{UnityCamera.useGUILayout = value; }
 }
	public System.Single ___delta00;
	public System.Single ___delta01;
	public System.Single ___adjustment10;
	public System.Single ___adjustment11;
	public System.Single ___adjustment12;
	public System.Single ___adjustment13;
	public System.Single ___adjustment14;
	public System.Single ___adjustment15;
	public System.Single ___adjustment16;
	public System.Single ___adjustment17;
	public Drone ___drone417;
	public System.Int32 counter64;
	public System.Single ___sensitivity00;
	public System.Single ___maxHeight00;
	public System.Single ___minHeight00;
	public List<UnityEngine.Vector3> ___starPositionsList10;
	public System.Single ___minBoundX10;
	public System.Single ___maxBoundX10;
	public System.Single ___minBoundZ10;
	public System.Single ___maxBoundZ10;
	public Player ___currentPlayer210;
	public Player ___currentPlayer311;
	public List<Planet> ___playerPlanets30;
	public Planet ___homePlanet31;
	public Player ___currentPlayer412;
	public System.Single ___size50;
	public void Update(float dt, World world) {
frame = World.frame;		this.Rule6(dt, world);
		this.Rule7(dt, world);
		this.Rule8(dt, world);
		this.Rule0(dt, world);
		this.Rule1(dt, world);
		this.Rule2(dt, world);
		this.Rule3(dt, world);
		this.Rule4(dt, world);
		this.Rule5(dt, world);
	}

	public void Rule6(float dt, World world) 
	{
	ScreenHeight = Screen.height;
	}
	

	public void Rule7(float dt, World world) 
	{
	ScreenWidth = Screen.width;
	}
	

	public void Rule8(float dt, World world) 
	{
	MousePosition = Input.mousePosition;
	}
	

	int s000=-1;
	public void parallelMethod000(float dt, World world){ 
	switch (s000)
	{

	case -1:
	if(!(((((UnityEngine.Input.GetKey(KeyCode.DownArrow)) && (!(((CameraSize) > (___maxHeight00)))))) && (!(world.GamePaused)))))
	{

	s000 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	CameraSize = System.Math.Min(___maxHeight00,(CameraSize) + (___sensitivity00));
	s000 = -1;
return;	
	default: return;}}
	

	int s001=-1;
	public void parallelMethod001(float dt, World world){ 
	switch (s001)
	{

	case -1:
	if(!(((((UnityEngine.Input.GetKey(KeyCode.UpArrow)) && (((((CameraSize) > (___minHeight00))) || (((CameraSize) == (___minHeight00))))))) && (!(world.GamePaused)))))
	{

	s001 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	CameraSize = System.Math.Max(___minHeight00,(CameraSize) - (___sensitivity00));
	s001 = -1;
return;	
	default: return;}}
	

	int s002=-1;
	public void parallelMethod002(float dt, World world){ 
	switch (s002)
	{

	case -1:
	if(!(((((((0f) > (UnityEngine.Input.GetAxis("Mouse ScrollWheel")))) && (!(((CameraSize) > (___maxHeight00)))))) && (!(world.GamePaused)))))
	{

	s002 = -1;
return;	}else
	{

	goto case 1;	}
	case 1:
	___delta00 = UnityEngine.Input.GetAxis("Mouse ScrollWheel");
	CameraSize = System.Math.Min(___maxHeight00,(CameraSize) - ((___delta00) * (MouseSensitivity)));
	s002 = -1;
return;	
	default: return;}}
	

	int s003=-1;
	public void parallelMethod003(float dt, World world){ 
	switch (s003)
	{

	case -1:
	if(!(((((((UnityEngine.Input.GetAxis("Mouse ScrollWheel")) > (0f))) && (((((CameraSize) > (___minHeight00))) || (((CameraSize) == (___minHeight00))))))) && (!(world.GamePaused)))))
	{

	s003 = -1;
return;	}else
	{

	goto case 1;	}
	case 1:
	___delta01 = UnityEngine.Input.GetAxis("Mouse ScrollWheel");
	CameraSize = System.Math.Max(___minHeight00,(CameraSize) - ((___delta01) * (MouseSensitivity)));
	s003 = -1;
return;	
	default: return;}}
	

	int s110=-1;
	public void parallelMethod110(float dt, World world){ 
	switch (s110)
	{

	case -1:
	if(!(((((UnityEngine.Input.GetKey(KeyCode.A)) && (!(world.GamePaused)))) && (((CameraPosition.x) > (___minBoundX10))))))
	{

	s110 = -1;
return;	}else
	{

	goto case 1;	}
	case 1:
	___adjustment10 = ((Sensitivity) * (-1f));
	CameraPosition = ((CameraPosition) + (new UnityEngine.Vector3(___adjustment10,0f,0f)));
	s110 = -1;
return;	
	default: return;}}
	

	int s111=-1;
	public void parallelMethod111(float dt, World world){ 
	switch (s111)
	{

	case -1:
	if(!(((((UnityEngine.Input.GetKey(KeyCode.D)) && (!(world.GamePaused)))) && (((___maxBoundX10) > (CameraPosition.x))))))
	{

	s111 = -1;
return;	}else
	{

	goto case 1;	}
	case 1:
	___adjustment11 = Sensitivity;
	CameraPosition = ((CameraPosition) + (new UnityEngine.Vector3(___adjustment11,0f,0f)));
	s111 = -1;
return;	
	default: return;}}
	

	int s112=-1;
	public void parallelMethod112(float dt, World world){ 
	switch (s112)
	{

	case -1:
	if(!(((((UnityEngine.Input.GetKey(KeyCode.S)) && (!(world.GamePaused)))) && (((CameraPosition.z) > (___minBoundZ10))))))
	{

	s112 = -1;
return;	}else
	{

	goto case 1;	}
	case 1:
	___adjustment12 = ((Sensitivity) * (-1f));
	CameraPosition = ((CameraPosition) + (new UnityEngine.Vector3(0f,0f,___adjustment12)));
	s112 = -1;
return;	
	default: return;}}
	

	int s113=-1;
	public void parallelMethod113(float dt, World world){ 
	switch (s113)
	{

	case -1:
	if(!(((((UnityEngine.Input.GetKey(KeyCode.W)) && (!(world.GamePaused)))) && (((___maxBoundZ10) > (CameraPosition.z))))))
	{

	s113 = -1;
return;	}else
	{

	goto case 1;	}
	case 1:
	___adjustment13 = Sensitivity;
	CameraPosition = ((CameraPosition) + (new UnityEngine.Vector3(0f,0f,___adjustment13)));
	s113 = -1;
return;	
	default: return;}}
	

	int s114=-1;
	public void parallelMethod114(float dt, World world){ 
	switch (s114)
	{

	case -1:
	if(!(((((((((BoundaryX) > (MousePosition.x))) && (!(world.GamePaused)))) && (((CameraPosition.x) > (___minBoundX10))))) && (!(world.GUI.OnGUI)))))
	{

	s114 = -1;
return;	}else
	{

	goto case 1;	}
	case 1:
	___adjustment14 = ((Sensitivity) * (-1f));
	CameraPosition = ((CameraPosition) + (new UnityEngine.Vector3(___adjustment14,0f,0f)));
	s114 = -1;
return;	
	default: return;}}
	

	int s115=-1;
	public void parallelMethod115(float dt, World world){ 
	switch (s115)
	{

	case -1:
	if(!(((((((((MousePosition.x) > (((ScreenWidth) - (BoundaryX))))) && (!(world.GamePaused)))) && (((___maxBoundX10) > (CameraPosition.x))))) && (!(world.GUI.OnGUI)))))
	{

	s115 = -1;
return;	}else
	{

	goto case 1;	}
	case 1:
	___adjustment15 = Sensitivity;
	CameraPosition = ((CameraPosition) + (new UnityEngine.Vector3(___adjustment15,0f,0f)));
	s115 = -1;
return;	
	default: return;}}
	

	int s116=-1;
	public void parallelMethod116(float dt, World world){ 
	switch (s116)
	{

	case -1:
	if(!(((((((((BoundaryY) > (MousePosition.y))) && (!(world.GamePaused)))) && (((CameraPosition.z) > (___minBoundZ10))))) && (!(world.GUI.OnGUI)))))
	{

	s116 = -1;
return;	}else
	{

	goto case 1;	}
	case 1:
	___adjustment16 = ((Sensitivity) * (-1f));
	CameraPosition = ((CameraPosition) + (new UnityEngine.Vector3(0f,0f,___adjustment16)));
	s116 = -1;
return;	
	default: return;}}
	

	int s117=-1;
	public void parallelMethod117(float dt, World world){ 
	switch (s117)
	{

	case -1:
	if(!(((((((((MousePosition.y) > (((ScreenHeight) - (BoundaryY))))) && (!(world.GamePaused)))) && (((___maxBoundZ10) > (CameraPosition.z))))) && (!(world.GUI.OnGUI)))))
	{

	s117 = -1;
return;	}else
	{

	goto case 1;	}
	case 1:
	___adjustment17 = Sensitivity;
	CameraPosition = ((CameraPosition) + (new UnityEngine.Vector3(0f,0f,___adjustment17)));
	s117 = -1;
return;	
	default: return;}}
	

	int s0=-1;
	public void Rule0(float dt, World world){ 
	switch (s0)
	{

	case -1:
	___sensitivity00 = 0.5f;
	___maxHeight00 = 100f;
	___minHeight00 = 5f;
	goto case 0;
	case 0:
	this.parallelMethod000(dt,world);
	this.parallelMethod001(dt,world);
	this.parallelMethod002(dt,world);
	this.parallelMethod003(dt,world);
	s0 = 0;
return;	
	default: return;}}
	

	int s1=-1;
	public void Rule1(float dt, World world){ 
	switch (s1)
	{

	case -1:
	___starPositionsList10 = (

(world.StarSystems).Select(__ContextSymbol250 => new { ___s10 = __ContextSymbol250 })
.Select(__ContextSymbol251 => __ContextSymbol251.___s10.Star.Position)
.ToList<UnityEngine.Vector3>()).ToList<UnityEngine.Vector3>();
	___minBoundX10 = (

(___starPositionsList10).Select(__ContextSymbol252 => new { ___p13 = __ContextSymbol252 })
.Select(__ContextSymbol253 => __ContextSymbol253.___p13.x)
.Min());
	___maxBoundX10 = (

(___starPositionsList10).Select(__ContextSymbol255 => new { ___p14 = __ContextSymbol255 })
.Select(__ContextSymbol256 => __ContextSymbol256.___p14.x)
.Max());
	___minBoundZ10 = (

(___starPositionsList10).Select(__ContextSymbol258 => new { ___p15 = __ContextSymbol258 })
.Select(__ContextSymbol259 => __ContextSymbol259.___p15.z)
.Min());
	___maxBoundZ10 = (

(___starPositionsList10).Select(__ContextSymbol261 => new { ___p16 = __ContextSymbol261 })
.Select(__ContextSymbol262 => __ContextSymbol262.___p16.z)
.Max());
	goto case 0;
	case 0:
	this.parallelMethod110(dt,world);
	this.parallelMethod111(dt,world);
	this.parallelMethod112(dt,world);
	this.parallelMethod113(dt,world);
	this.parallelMethod114(dt,world);
	this.parallelMethod115(dt,world);
	this.parallelMethod116(dt,world);
	this.parallelMethod117(dt,world);
	s1 = 0;
return;	
	default: return;}}
	

	int s2=-1;
	public void Rule2(float dt, World world){ 
	switch (s2)
	{

	case -1:
	___currentPlayer210 = world.CurrentPlayer;
	goto case 2;
	case 2:
	if(!(((___currentPlayer210.Drones.Count) > (0))))
	{

	s2 = 2;
return;	}else
	{

	goto case 1;	}
	case 1:
	CameraPosition = new UnityEngine.Vector3(___currentPlayer210.Drones.Head().Position.x,CameraPosition.y,___currentPlayer210.Drones.Head().Position.z);
	s2 = 0;
return;
	case 0:
	if(!(!(((___currentPlayer210) == (world.CurrentPlayer)))))
	{

	s2 = 0;
return;	}else
	{

	s2 = -1;
return;	}	
	default: return;}}
	

	int s3=-1;
	public void Rule3(float dt, World world){ 
	switch (s3)
	{

	case -1:
	___currentPlayer311 = world.CurrentPlayer;
	goto case 3;
	case 3:
	if(!(world.GUI.QuickBar.HomeButton.Clicked))
	{

	s3 = 3;
return;	}else
	{

	goto case 2;	}
	case 2:
	___playerPlanets30 = (

(world.StarSystems).Select(__ContextSymbol264 => new { ___system31 = __ContextSymbol264 })
.SelectMany(__ContextSymbol265=> (__ContextSymbol265.___system31.Planets).Select(__ContextSymbol266 => new { ___planet31 = __ContextSymbol266,
                                                      prev = __ContextSymbol265 })
.Where(__ContextSymbol267 => ((__ContextSymbol267.___planet31.Owner.IsSome) && (((__ContextSymbol267.___planet31.Owner.Value) == (world.CurrentPlayer)))))
.Select(__ContextSymbol268 => __ContextSymbol268.___planet31)
.ToList<Planet>())).ToList<Planet>();
	___homePlanet31 = ___playerPlanets30.Head();
	CameraPosition = new UnityEngine.Vector3(___homePlanet31.Position.x,CameraPosition.y,___homePlanet31.Position.z);
	s3 = -1;
return;	
	default: return;}}
	

	int s4=-1;
	public void Rule4(float dt, World world){ 
	switch (s4)
	{

	case -1:
	___currentPlayer412 = world.CurrentPlayer;
	goto case 0;
	case 0:
	this.concurrentSelectMethod40(dt,world);
	if(__m_done40)
	{

	__m_current_state40 = -1;
	__m_done40 = false;
	s4 = -1;
return;	}else
	{

	s4 = 0;
return;	}	
	default: return;}}
	

	int s5=-1;
	public void Rule5(float dt, World world){ 
	switch (s5)
	{

	case -1:
	___size50 = CameraSize;
	Sensitivity = ((___size50) * (0.1f));
	s5 = 0;
return;
	case 0:
	if(!(!(((___size50) == (CameraSize)))))
	{

	s5 = 0;
return;	}else
	{

	s5 = -1;
return;	}	
	default: return;}}
	

	int __m_current_state40, s40=-1;
	bool __m_done40 = false;
	public void concurrentSelectMethod40(float dt, World world){
	{

	if(!(((___currentPlayer412) == (world.CurrentPlayer))))
	{

	if(!(((__m_current_state40) == (0))))
	{

	__m_current_state40 = -1;
	__m_done40 = false;
	s40 = 0;
	__m_current_state40 = 0;	}	}else
	{

	if(true)
	{

	if(!(((__m_current_state40) == (1))))
	{

	__m_current_state40 = -1;
	__m_done40 = false;
	s40 = 1;
	__m_current_state40 = 1;	}	}else
	{

	if(((!(!(((___currentPlayer412) == (world.CurrentPlayer))))) && (!(true))))
	{

	s40 = 3;	}	}	}	}
	switch (s40)
	{

	case 3:
	s40 = 0;
return;
	goto case 1;
	case 0:
	CameraPosition = CameraPosition;
	s40 = 2;
return;
	case 1:
	
	counter64 = -1;
	if((((world.CurrentPlayer.Drones).Count) == (0)))
	{

	s40 = 2;
return;	}else
	{

	___drone417 = (world.CurrentPlayer.Drones)[0];
	goto case 6;	}
	case 6:
	counter64 = ((counter64) + (1));
	if((((((world.CurrentPlayer.Drones).Count) == (counter64))) || (((counter64) > ((world.CurrentPlayer.Drones).Count)))))
	{

	s40 = 2;
return;	}else
	{

	___drone417 = (world.CurrentPlayer.Drones)[counter64];
	goto case 7;	}
	case 7:
	if(!(world.GUI.QuickBar.DroneButton.Clicked))
	{

	s40 = 7;
return;	}else
	{

	goto case 8;	}
	case 8:
	CameraPosition = new UnityEngine.Vector3(___drone417.Position.x,CameraPosition.y,___drone417.Position.z);
	s40 = 6;
return;
	case 2:
	__m_done40 = true;
	goto case 3;	}}
	




}
public class Minimap{
public int frame;
public bool JustEntered = true;
	public int ID;
public Minimap()
	{JustEntered = false;
 frame = World.frame;
		UnityMinimap = UnityMinimap.CreateMinimap();
		
}
		public UnityEngine.Vector3 Position{  get { return UnityMinimap.Position; }
  set{UnityMinimap.Position = value; }
 }
	public UnityMinimap UnityMinimap;
	public UnityEngine.Animation animation{  get { return UnityMinimap.animation; }
 }
	public UnityEngine.AudioSource audio{  get { return UnityMinimap.audio; }
 }
	public UnityEngine.Camera camera{  get { return UnityMinimap.camera; }
 }
	public UnityEngine.Collider collider{  get { return UnityMinimap.collider; }
 }
	public UnityEngine.Collider2D collider2D{  get { return UnityMinimap.collider2D; }
 }
	public UnityEngine.ConstantForce constantForce{  get { return UnityMinimap.constantForce; }
 }
	public System.Boolean enabled{  get { return UnityMinimap.enabled; }
  set{UnityMinimap.enabled = value; }
 }
	public UnityEngine.GameObject gameObject{  get { return UnityMinimap.gameObject; }
 }
	public UnityEngine.GUIElement guiElement{  get { return UnityMinimap.guiElement; }
 }
	public UnityEngine.GUIText guiText{  get { return UnityMinimap.guiText; }
 }
	public UnityEngine.GUITexture guiTexture{  get { return UnityMinimap.guiTexture; }
 }
	public UnityEngine.HideFlags hideFlags{  get { return UnityMinimap.hideFlags; }
  set{UnityMinimap.hideFlags = value; }
 }
	public UnityEngine.HingeJoint hingeJoint{  get { return UnityMinimap.hingeJoint; }
 }
	public UnityEngine.Light light{  get { return UnityMinimap.light; }
 }
	public System.String name{  get { return UnityMinimap.name; }
  set{UnityMinimap.name = value; }
 }
	public UnityEngine.ParticleEmitter particleEmitter{  get { return UnityMinimap.particleEmitter; }
 }
	public UnityEngine.ParticleSystem particleSystem{  get { return UnityMinimap.particleSystem; }
 }
	public UnityEngine.Renderer renderer{  get { return UnityMinimap.renderer; }
 }
	public UnityEngine.Rigidbody rigidbody{  get { return UnityMinimap.rigidbody; }
 }
	public UnityEngine.Rigidbody2D rigidbody2D{  get { return UnityMinimap.rigidbody2D; }
 }
	public System.String tag{  get { return UnityMinimap.tag; }
  set{UnityMinimap.tag = value; }
 }
	public UnityEngine.Transform transform{  get { return UnityMinimap.transform; }
 }
	public System.Boolean useGUILayout{  get { return UnityMinimap.useGUILayout; }
  set{UnityMinimap.useGUILayout = value; }
 }
	public UnityEngine.Vector3 ___cameraPosition02;
	public void Update(float dt, World world) {
frame = World.frame;

		this.Rule0(dt, world);

	}





	int s0=-1;
	public void Rule0(float dt, World world){ 
	switch (s0)
	{

	case -1:
	___cameraPosition02 = world.MainCamera.CameraPosition;
	Position = ___cameraPosition02;
	s0 = 0;
return;
	case 0:
	if(!(!(((___cameraPosition02) == (world.MainCamera.CameraPosition)))))
	{

	s0 = 0;
return;	}else
	{

	s0 = -1;
return;	}	
	default: return;}}
	






}
}