using System;
using System.Collections.Generic;
using UnityEngine;


namespace Track
{

  //public static class MyExtensions
  //{
  //  public static List<T> Tail<T>(this List<T> list)
  //  {

  //    var tmp = new List<T>();
  //    for (int i = 1; i < list.Count; i++)
  //    {
  //      tmp.Add(list[i]);
  //    }
  //    // omits validation, etc.
  //    return tmp;
  //  }
  //  public static T Head<T>(this List<T> list)
  //  {
  //    return list[0];
  //  }
  //}

  public class TrackManager
  {

    private static List<List<Item>> itemsToPlay = new List<List<Item>>();

    public static List<List<Item>> ItemsToPlay
    {
      get { return itemsToPlay; }
      set { itemsToPlay = value; }
    }

    private static List<List<Item>> tutorialItems = new List<List<Item>>();

    public static List<List<Item>> TutorialItems
    {
      get { return tutorialItems; }
      set { tutorialItems = value; }
    }

    private static List<Item> currentExperiment;
    public static List<Item> CurrentExperiment
    {
      get { return currentExperiment; }
      set { currentExperiment = value; }
    }

    private static List<Item> justRunExperiment;
    public static List<Item> JustRunExperiment
    {
      get { return justRunExperiment; }
      set { justRunExperiment = value; }
    }

    private static List<Item> currentTutorial;
    public static List<Item> CurrentTutorial
    {
      get { return currentTutorial; }
      set { currentTutorial = value; }
    }

    private static int experimentNumber;

    public static int ExperimentNumber
    {
      get { return experimentNumber; }
      set { experimentNumber = value; }
    }

    private static int totExperiments;

    public static int TotExperiments
    {
      get { return totExperiments; }
      set { totExperiments = value; }
    }
    




    public static void MoveNextExperiment()
    {

      if (ItemsToPlay.Count > 0)
      {
        CurrentExperiment = ItemsToPlay[0];
        ItemsToPlay.RemoveAt(0);
      }
      if (TutorialItems.Count > 0)
      {

        CurrentTutorial = TutorialItems[0];
        TutorialItems.RemoveAt(0);
      }
      ExperimentNumber++;


      //Debug.Log("Moved next");
      //Debug.Log("Experiment to play: " + CurrentExperiment.Count);
      //Debug.Log("Experiment tutorial to play: " + CurrentTutorial.Count);

    }

    public static string ResultToStore;

    public static void SaveAll()
    {

      JustRunExperiment = CurrentExperiment;

      foreach (var item in JustRunExperiment)
      {
        ResultToStore += item.ItemName + "," + item.Answer + "," + item.ResponceTime + "\n";
      }
      ResultToStore += "\n";
    }
    public static void SaveResultsToFile()
    {
      var now = System.DateTime.Now;
      //System.IO.File.WriteAllText("pippo.cnv",//System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), now.Day + "" + now.Month + "" + now.Year + "_" + now.Hour + "-" + now.Minute + "-" + now.Second + ".csv"), 
                                  //ResultToStore);    
    }
    public static void Init(string text)
    {
      ExperimentNumber = 0;
      ResultToStore = "";
      //var file1 = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), file);

      //if (!System.IO.File.Exists(file1))
      //  new Exception("The referenced file does not exist. " + file1);
      





      

      var experiment = new List<Item>();
      var tuorial = new List<Item>();
      bool skip = false;
      foreach (var _line in text.Split('\n'))
      {
        var line = _line.Split('\r')[0].Split(',');
        if (line[0] != "" && !line[0].StartsWith("//") && !line[0].StartsWith("end"))
        {
          skip = false;
          //Debug.Log("LINE   " + line[0] + "   ---   " + line[1]);

          var file_name = line[0];
          var track_to_play = Int32.Parse(line[1]) > 0;
          var is_tutorial = Int32.Parse(line[2]) > 0;
          if (track_to_play)
            experiment.Add(new Item(file_name, track_to_play, is_tutorial));
          if (is_tutorial)
            tuorial.Add(new Item(file_name, track_to_play, is_tutorial));

          //Debug.Log(file_name + "_" + track_to_play + "_" + is_tutorial);
        }
        else
        {
          //Debug.Log("Else");
          if (!skip  && line[0] == "end" || line[0] == "" && experiment.Count > 0)
          {
            skip = true;
            ItemsToPlay.Add(experiment);
            experiment = new List<Item>();
            TutorialItems.Add(tuorial);
            tuorial = new List<Item>();
          }
        }
      }
      TotExperiments = ItemsToPlay.Count;
      Debug.Log(TotExperiments);

      CurrentExperiment = ItemsToPlay[0];
      CurrentTutorial = TutorialItems[0];

      ItemsToPlay.RemoveAt(0);
      TutorialItems.RemoveAt(0);
      ExperimentNumber++;
      //Debug.Log("Items to play: " + ItemsToPlay.Count);

      //Debug.Log("Experiment to play: " + CurrentExperiment.Count);
      
      //Debug.Log("Experiment tutorial to play: " + CurrentTutorial.Count);
        
      


    }
    
  }

  public class Item
  {

    private bool isSame;

    public bool IsSame
    {
      get { return isSame; }
      set { isSame = value; }
    }

    private bool answer;

    public bool Answer
    {
      get { return answer; }
      set {
        Debug.Log("isSame == " + isSame);
        Debug.Log("value == " + value);
        if (isSame == value == true || isSame == !value == false) answer = true;
        else answer = false; }
    }

    private double responceTime;

    public double ResponceTime
    {
      get { return responceTime; }
      set { responceTime = value; }
    }



    private string itemName;

    public string ItemName
    {
      get { return itemName; }
      set { itemName = value; }
    }
    private bool itemToPlay;

    public bool ItemToPlay
    {
      get { return itemToPlay; }
      set { itemToPlay = value; }
    }
    private bool tutorialItem;

    public bool TutorialItem
    {
      get { return tutorialItem; }
      set { tutorialItem = value; }
    }
    public Item(string itemName, bool to_play, bool tutorial)
    {
      this.itemName = itemName;
      isSame = itemName.Split('_')[1].ToLower() == "same";
      to_play = itemToPlay;
      tutorialItem = tutorial;
    }
  }


}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           