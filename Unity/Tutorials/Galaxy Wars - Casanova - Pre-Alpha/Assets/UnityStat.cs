using UnityEngine;
using System.Collections;
using Casanova.Prelude;
using System.Collections.Generic;

public class UnityStat : MonoBehaviour
{

  private TextMesh AttackStat;
  private TextMesh DefenseStat;
  private TextMesh ProductivityStat;
  private TextMesh ResearchStat;
  private TextMesh NextShipIn;
  private TextMesh Owner;
  private TextMesh ShipCount;

  public static UnityStat FindStat(UnityPlanet p)
  {
    //var stat = p.GetComponentInChildren<UnityStat>() as UnityStat;
    var stat = p.transform.FindChild("Stat").GetComponent<UnityStat>() as UnityStat;
    stat.Enabled = true;
    stat.AttackStat = stat.transform.FindChild("Attack").GetComponent<TextMesh>();
    stat.DefenseStat = stat.transform.FindChild("Defense").GetComponent<TextMesh>();
    stat.ProductivityStat = stat.transform.FindChild("Productivity").GetComponent<TextMesh>();
    stat.ResearchStat = stat.transform.FindChild("Research").GetComponent<TextMesh>();
    stat.NextShipIn = stat.transform.FindChild("NextShipIn").GetComponent<TextMesh>();
    stat.Owner = stat.transform.FindChild("OwnerText").GetComponent<TextMesh>(); 
    stat.ShipCount = stat.transform.FindChild("ShipCountText").GetComponent<TextMesh>(); 
    return stat;
  }

  public bool Enabled
  {
    get { return this.active;}
    set { this.active = value;}
  }

  public string AttackText
  {
    get { return AttackStat.text; }
    set { AttackStat.text = "    : " + value; }
  }

  public string DefenseText
  {
    get { return DefenseStat.text; }
    set { DefenseStat.text = "D : " + value; }
  }

  public string ProductivityText
  {
    get { return ProductivityStat.text; }
    set { ProductivityStat.text = "P : " + value; }
  }
  public string ResearchText
  {
    get { return ResearchStat.text; }
    set { ResearchStat.text = "R : " + value; }
  }
  public string NextShipInText
  {
    get { return NextShipIn.text; }
    set { NextShipIn.text = "NS : " + value; }
  }
  public string OwnerText
  {
    get { return Owner.text; }
    set { Owner.text = value; }
  }
  public string ShipCountText
  {
    get { return ShipCount.text; }
    set { ShipCount.text = value; }
  }
  public Vector3 Rotation
  {
    get { return this.gameObject.transform.rotation.eulerAngles; }
    set { this.gameObject.transform.rotation = Quaternion.Euler(value); }
  }
  public Color setColor
  {
    get { return this.transform.guiText.color; }
    set { this.transform.guiText.color = value; }
  }
}                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         