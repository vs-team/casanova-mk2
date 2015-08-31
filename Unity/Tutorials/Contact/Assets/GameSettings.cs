using UnityEngine;
using System.Collections;

public struct Settings
{
  public int StartingResources;
  public int TurnDuration;
  public int PlayerCount;
  public int WinTotalResources;
  public int MaxMessageCost;
  public int MiningRate;
  public bool SameResources;
  public bool SameWinCondition;
}

public class GameSettings
{
  public static int StartingResources { get; set; }
  public static int TurnDuration { get; set; }
  public static int PlayerCount { get; set; }
  public static int WinTotalResources { get; set; }
  public static int MaxMessageCost { get; set; }
  public static int MiningRate { get; set; }
  public static int MapSize   { get; set; }
  public static int MapSeed { get; set; }
  public static bool SameResources { get; set; }
  public static bool DifferentObjective { get; set; }

  public static void SetParameters(int startingResources, int turnDuration, int playerCount,
                                   int winTotalResources, int maxMessageCost, int miningRate,
                                   int mapSize, int mapSeed)
  {
    GameSettings.StartingResources = startingResources;
    GameSettings.TurnDuration = turnDuration;
    GameSettings.PlayerCount = playerCount;
    GameSettings.WinTotalResources = winTotalResources;
    GameSettings.MaxMessageCost = maxMessageCost;
    GameSettings.MiningRate = miningRate;
    GameSettings.MapSize = mapSize;
    GameSettings.MapSeed = mapSeed;
  }

  public static void Print()
  {
    string message =
      "StartingResources = " + StartingResources.ToString() + "\n" +
      "TurnDuration = " + TurnDuration.ToString() + "\n" +
      "PlayerCount = " + PlayerCount.ToString() + "\n" +
      "WinTotalResources = " + WinTotalResources.ToString() + "\n" +
      "MaxMessageCost = " + MaxMessageCost.ToString() + "\n" +
      "MiningRate = " + MiningRate.ToString() + "\n" +
      "MapSize = " + MapSize.ToString() + "\n" +
      "MapSeed = " + MapSeed.ToString() + "\n";
    Debug.Log(message);
  }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           