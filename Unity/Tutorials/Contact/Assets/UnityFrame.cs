using UnityEngine;
using System.Collections;

public class UnityFrame : MonoBehaviour
{
	private float offsetX;
	private float offsetY;
	
	
	public float OffsetX
	{
		get { return this.offsetX; }
		set { this.offsetX = value; }
	}
	
	public float OffsetY
	{
		get { return this.offsetY; }
		set { this.offsetY = value; }
	}
	
	public Vector2 Origin
	{
		get 
		{
			RectTransform rectTransform = this.gameObject.GetComponent<RectTransform>();
			Vector2 pos = new Vector2(rectTransform.anchoredPosition.x,rectTransform.anchoredPosition.y);
			//Debug.Log (pos);
			return pos;
		}
	}
	
	public float Width
	{
		get { return this.gameObject.GetComponent<RectTransform>().rect.width; }
	}
	
	public float Height
	{
		get { return this.gameObject.GetComponent<RectTransform>().rect.height; }
	}

	public static UnityFrame GetFrame(string name, float offsetX, float offsetY)
	{
		UnityFrame frame = GameObject.Find(name).GetComponent<UnityFrame>();
		frame.offsetX = offsetX;
		frame.OffsetY = offsetY;
		return frame;
	}
}                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                   