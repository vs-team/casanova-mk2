using System;
using System.Collections.Generic;
using UnityEngine;


namespace Track
{

  public class XMLTests { public List<XMLTest> Tests = new List<XMLTest>(); }

  public class XMLTest
  {
    public List<XMLActual> ActualTracks = new List<XMLActual>();
    public List<XMLTutorial> TutorialTracks = new List<XMLTutorial>(); }

  public class XMLActual
  {
    public string path;
    public XMLActual(string path) { this.path = path; }
  }

  public class XMLTutorial 
    {
    public string path;
    public XMLTutorial(string path) { this.path = path; }
  }

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
      System.IO.File.WriteAllText(System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), now.Day + "" + now.Month + "" + now.Year + "_" + now.Hour + "-" + now.Minute + "-" + now.Second + ".csv"),
                                  ResultToStore);    
    }
    public static void Init(XMLTests tests)
    {
      ExperimentNumber = 0;
      ResultToStore = "";

      //var file1 = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), file);

      //if (!System.IO.File.Exists(file1))
      //  new Exception("The referenced file does not exist. " + file1);
      
            

      List<Item> experiment;
      List<Item> tuorial;
      

      foreach (var test in tests.Tests)
      {
        experiment = new List<Item>();
        tuorial = new List<Item>();

        foreach(var t in test.TutorialTracks)
            tuorial.Add(new Item(t.path));
        
        foreach(var a in test.ActualTracks)
            experiment.Add(new Item(a.path));

        ItemsToPlay.Add(experiment);
        TutorialItems.Add(tuorial);
        
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
    //private bool itemToPlay;

    //public bool ItemToPlay
    //{
    //  get { return itemToPlay; }
    //  set { itemToPlay = value; }
    //}
    //private bool tutorialItem;

    //public bool TutorialItem
    //{
    //  get { return tutorialItem; }
    //  set { tutorialItem = value; }
    //}
    public Item(string itemName)
    {
      this.itemName = itemName;
      isSame = itemName.Split('_')[1].ToLower() == "same";
      //to_play = itemToPlay;
      //tutorialItem = tutorial;
    }
  }


}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    