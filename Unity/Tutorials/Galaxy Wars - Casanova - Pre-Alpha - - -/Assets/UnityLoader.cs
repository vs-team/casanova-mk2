using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using System.Xml;

public class UnityLoader : MonoBehaviour {
  public static void LoadMap()
  {
    GameObject.Instantiate(Resources.Load("Maps/Map1"));
  }
  void Start()
  {
    LoadMap();
    GameObject.Instantiate(Resources.Load("Prefabs/EntryP"));

/*
    XmlDocument doc = new XmlDocument();
    TextAsset textAsset = (TextAsset)Resources.Load("Maps/Map1");
    doc.LoadXml(textAsset.text);

    List<XMLStarSystem> all_xml_ss = new List<XMLStarSystem>();
    List<XMLLink> all_xml_link = new List<XMLLink>();
    List<UnityPlanet> tmpAllPlanets = new List<UnityPlanet>();

    foreach (XmlNode ss in doc.DocumentElement.SelectSingleNode("/Root")) // iterate through only the star systems
    {
      if (ss.Name == "StarSystem")
      {
        string ssName = ss.Attributes["name"].InnerText;
        XMLStarSystem xml_ss = new XMLStarSystem();
        var px = float.Parse(ss.Attributes["px"].InnerText);
        var pz = float.Parse(ss.Attributes["pz"].InnerText);
        xml_ss.Name  = ssName;
        List<XMLPlanet> xml_ps = new List<XMLPlanet>();
        foreach (XmlNode p_or_l in ss.ChildNodes) // iterate through all planets
        {
          if (p_or_l.Name == "Planet")
          {
            var p_px = float.Parse(p_or_l.Attributes["px"].InnerText);
            var p_pz = float.Parse(p_or_l.Attributes["pz"].InnerText);
            var p_pos = new Vector3(px + p_px, 0, pz + p_pz);
            XMLPlanet planet = new XMLPlanet();
            planet.Name = ssName + p_or_l.Attributes["name"].InnerText ;
            planet.startingCommander = int.Parse(p_or_l.Attributes["owner"].InnerText);
            planet.Position = p_pos;
            xml_ps.Add(planet);
          }
        }

        List<XMLLink> xml_ls = new List<XMLLink>();
        foreach (XmlNode p_or_l in ss.ChildNodes) // iterate through all planets
        {
          if (p_or_l.Name == "Link")
          {

            XMLLink link = new XMLLink();
            var from = ssName + p_or_l.Attributes["from"].InnerText;
            var to = ssName + p_or_l.Attributes["to"].InnerText;
            var XMLfrom = xml_ps.Find(p => p.Name == from);
            var XMLto = xml_ps.Find(p => p.Name == to);
            link.Source = XMLfrom;
            link.Target = XMLto;
            xml_ls.Add(link);
          }

        }
        xml_ss.Links = xml_ls;
        xml_ss.Planets = xml_ps;
        all_xml_ss.Add(xml_ss);
      }
    }
    foreach (XmlNode ss in doc.DocumentElement.SelectSingleNode("/Root")) // iterate through only the star systems
    {
      if (ss.Name == "Link")
      {
        XMLLink link = new XMLLink();
        var stringfromSS = ss.Attributes["fromSS"].InnerText;
        var fromSS = all_xml_ss.Find(_ss => _ss.Name == stringfromSS);

        var stringtoSS = ss.Attributes["toSS"].InnerText;
        var toSS = all_xml_ss.Find(_ss => _ss.Name == stringtoSS);

        XMLPlanet XMLfrom = fromSS.Planets.Find(p => p.Name == stringfromSS + ss.Attributes["fromP"].InnerText);
        XMLPlanet XMLto = toSS.Planets.Find(p => p.Name == stringtoSS + ss.Attributes["toP"].InnerText);


        link.Source = XMLfrom;
        link.Target = XMLto;
        all_xml_link.Add(link);
      }
     }

    foreach (XMLStarSystem ss in all_xml_ss)
    {
      var UnitySS = (GameObject) GameObject.Instantiate(Resources.Load("Prefabs/StarSystem"));


      List<UnityPlanet> tmpPlanets = new List<UnityPlanet>();
      foreach (var xmlplanet in ss.Planets)
      {
        GameObject planet_obj = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/Planet"),xmlplanet.Position, Quaternion.identity);
        UnityPlanet planet = planet_obj.GetComponent<UnityPlanet>();
        tmpAllPlanets.Add(planet);
        planet.startingCommander = xmlplanet.startingCommander;
        planet.Name = xmlplanet.Name;
        planet.transform.parent = UnitySS.transform;
        tmpPlanets.Add(planet);
      }

      foreach (var xmllink in ss.Links)
      {
        GameObject link_obj = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/Link"));
        UnityLink link = link_obj.GetComponent<UnityLink>();
        link.startPlanet = tmpPlanets.Find(p => p.name == xmllink.Source.Name);
        link.endPlanet = tmpPlanets.Find(p => p.name == xmllink.Target.Name);
        link.SSLink = xmllink.ssLink;
      }
    }
    foreach (var xmllink in all_xml_link)
      {
        GameObject link_obj = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/Link"));
        UnityLink link = link_obj.GetComponent<UnityLink>();
        link.startPlanet = tmpAllPlanets.Find(p => p.name == xmllink.Source.Name);
        link.endPlanet = tmpAllPlanets.Find(p => p.name == xmllink.Target.Name);
        link.SSLink = xmllink.ssLink;
      }
 */



  }
	
	// Update is called once per frame
	void Update () {
	
	}


  public class XMLStarSystem
  {
    public string Name;
    public Vector2 Position;
    public List<XMLLink> Links = new List<XMLLink>();
    public List<XMLPlanet> Planets = new List<XMLPlanet>();
  }
  public class XMLLink
  {
    public XMLPlanet Source, Target;
    public bool ssLink;
  }

  public class XMLPlanet
  {
    public Vector3 Position;
    public string Name;
    public int startingCommander;
  }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  