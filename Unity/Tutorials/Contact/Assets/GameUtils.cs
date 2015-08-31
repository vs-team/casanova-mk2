using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GameUtils
{
  public static string IntToString(int n)
  {
    return n.ToString();
  }
	
	public static string BoolToString(bool b)
	{
		return b.ToString();
	}
	
	public static string FloatToString(float f)
	{
		return f.ToString();
	}
	
	public static int StringToInt(string s)
	{
		return System.Convert.ToInt32(s);
	}
	
	public static float StringToFloat(string s)
	{
		return System.Convert.ToSingle(s);
	}
	
	public static void printMsg(string m)
	{
		Debug.Log(m);
	}
	
	public static void printInt(int n)
	{
		Debug.Log(n);
	}
	
	public static int FloatToInt(float f)
	{
		return (int)f;
	}
	
	public static float IntToFloat(int n)
	{
		return (float)n;
	}

  public static Vector3 RotateVector(float angle,Vector3 v,Vector3 rotationAxis)
  {
    return Quaternion.AngleAxis(angle, rotationAxis) * v;
  }
	
	public static string IntListToString(IEnumerable<int> l)
	{
		string s = "[";
    for (int i = 0; i < l.Count(); i++)
    {
      s += l.ElementAt(i).ToString() + (i < l.Count() - 1 ? " " : "");
    }
    s += "]";
		return s;
	}
	
	public static string Vector3ListToString(List<Vector3> l)
	{
		string s = "";
		foreach (Vector3 x in l) 
			s += "(" + x.x + "," + x.y + ")" + "\t";
		return s;
	}
	
	public static void AddStringToList(List<string> l, string s)
	{
		l.Add(s);
	}
	
	public static void printPlanet(UnityPlanet p)
	{
		Debug.Log(p);
	}
	
	public static void printColor(Color c)
	{
		Debug.Log (c);
	}
	
	public static void printColors(List<Color> colors)
	{
		Debug.Log(colors);
	}
	
  public static void printMessage(string name, string senderName, string receiverName, IEnumerable<int> content, IEnumerable<int> images, bool read)
  {
    List<int> varContent = content.ToList();
    List<int> varImages = images.ToList();
    Debug.Log(name + "\n" + senderName + "\n" + receiverName + "\n" + (IntListToString(varContent)) + "\n" + (IntListToString(varImages)) + "\n" + read.ToString());
  }

  public static void Shuffle<T>(List<T> list)
  {
    int n = list.Count;
    while (n > 1)
    {
      n--;
      int k = Random.Range(0,n + 1);
      T value = list[k];
      list[k] = list[n];
      list[n] = value;
    }
  }

  public static List<int> RandomizeResources(int maxAmount, int numResources)
  {
    int currentAmount = 0;
    List<int> l = new List<int>(numResources);
    for (int i = 0; i < numResources - 1; i++)
    {
      int r = Random.Range(0, maxAmount - currentAmount + 1);
      l.Add(r);
      currentAmount += r;
    }
    l.Add(maxAmount - currentAmount);
    Shuffle(l);
    return l;
  }

  public static int SpawnResource()
  {
    float r = Random.Range(0.0f,1.0f);
    int resourceLevel;
    if (r < 0.2f) resourceLevel = 0;
    else if (r >= 0.2f && r < 0.4f) resourceLevel = 1;
    else if (r >= 0.4f && r < 0.9f) resourceLevel = 2;
    else if (r >= 0.9f && r < 0.975f) resourceLevel = 3;
    else resourceLevel = 4;

    switch (resourceLevel)
    {
      case 0: return 0;
      case 1: return Random.Range(15, 48);
      case 2: return Random.Range(30, 75);
      case 3: return Random.Range(100, 200);
      case 4: return Random.Range(300,500);
      default: return 0;
    }
  }

  public static float Factorial(int n)
  {
    if (n == 0) return 1.0f;
    else return n * (Factorial(n - 1));
  }


	//create a list of distinc random integer values in [rangeMin,rangeMax]
	public static List<int> randomDistinctIntList(int numVal, int rangeMin, int rangeMax)
	{
		if (rangeMax - rangeMin < numVal)
			throw new System.ArgumentException("The range is too small to generate distinct values");
		List<int> randomVals = new List<int>(numVal);
		List<int> valuePool = new List<int>(rangeMax - rangeMin + 1);
		for (int i = rangeMin; i < rangeMax; i++)
			valuePool.Add(i);
		for (int i = 0; i < numVal; i++)
		{
			int elementIndex = Random.Range(0, valuePool.Count);
			randomVals.Add(valuePool[elementIndex]);
			valuePool.Remove(valuePool[elementIndex]);
		}
		return randomVals;
	}
	
	public static List<UnityEngine.Color> generateColorList()
	{
		Color[] colors = { Color.red,Color.blue,Color.green,Color.yellow };
		return new List<UnityEngine.Color>(colors);
	}
	
	public static List<UnityEngine.Color> randomDistinctColors(int numColors)
	{
		List<UnityEngine.Color> colors = generateColorList();
		if (numColors > colors.Count)
			throw new System.ArgumentException("The color set is too small");
		List<UnityEngine.Color> randomColors = new List<UnityEngine.Color>(numColors);
		for (int i = 0; i < numColors; i++)
		{
			Color c = colors[Random.Range(0,colors.Count)];
			randomColors.Add(c);
			colors.Remove(c);
		}
		return randomColors;
	}
	
	public static Color AssignColor(int playerIndex)
	{
		switch (playerIndex)
		{
			case 0: return Color.red;
			case 1: return Color.blue;
			case 2: return Color.cyan;
			case 3: return new Color(72,0,255);
			case 4: return new Color(255,106,0);
			case 5: return new Color(127,151,0);
			case 6: return Color.white;
			case 7: return Color.yellow;
			default: return Color.grey;
		}
	}
	

	public static bool Contains(List<int> l,int x)
	{
		return l.Contains(x);
	}
	
	public static int FindIndex(List<int> l,int x)
	{
		return l.FindIndex(y => x == y);
	}
	
	public static string Date()
	{
    string date =
        System.DateTime.Now.Hour.ToString() + ":" +
        System.DateTime.Now.Minute.ToString() + ":" +
        System.DateTime.Now.Second.ToString() + ":" +
        System.DateTime.Now.Millisecond.ToString();
    return date;
	}

  public static void SetSeed(int seed)
  {
    Random.seed = seed;
  }

  public static string ConcatenateStrings(IEnumerable<string> l)
  {
    string concatenation = "";
    foreach (var s in l)
    {
      concatenation = concatenation + s + "\n";
    }
    return concatenation;
  }
}                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                      