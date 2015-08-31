using UnityEngine;
using System.Collections;
using System.Threading;
using System.Linq;
using System;

[RequireComponent(typeof(Renderer))]
public class DisplayDepth : MonoBehaviour {
	
	public DepthWrapper dw;

  Thread oThread;
  
  public float[] dv;

  void Start () {

    
    oThread = new Thread(new ThreadStart(getDepth));
    oThread.Start();
    oThread.IsBackground = true;

	}
	
	void Update () {
    if (Input.GetKeyDown(KeyCode.Space))
      endthread();
		//if (dw.pollDepth())
		//{
			//tex.SetPixels32(convertDepthToColor(dw.depthImgSmoothed));
      //Debug.Log("normalized: "+dw.depthImgNormalizedSmoothed[720]);
      //Debug.Log("wtf : "+dw.OriginalImage[44000]);
      //tex.SetPixels32(convertPlayersToCutout(dw.segmentations));
			//tex.Apply(false);

		//}
	}

  public float getValue(int index)
  {
    try
    {
      return dv[index];
    }
    catch (Exception e)
    {
      Debug.Log(e.Message);
      return 0.0f;
    }
  }
	
    public void getDepth()
  {
    while (true)
    {
      try
      {
        dw.pollDepth();
        dv = dw.depthImgSmoothed;
        Debug.Log(dv[720]);
        Thread.Sleep(500);
      }
      catch (Exception e)
      {
        Debug.Log(e.Message);
      }

    }
   }

  

	private Color32[] convertDepthToColor(float[] depthBuf)
	{
		Color32[] img = new Color32[depthBuf.Length];
		for (int pix = 0; pix < depthBuf.Length; pix++)
		{
			img[pix].r = (byte)(depthBuf[pix] * 255 );
			img[pix].g = (byte)(depthBuf[pix] * 255);
			img[pix].b = (byte)(depthBuf[pix] * 255);
		}
		return img;
	}

  public void endthread()
  {
    try
    {
      if (oThread.IsAlive)
      {
        oThread.Abort();
        Debug.Log("trying to abort");
      }
    }
    catch (Exception e)
    {
      Debug.Log(e.Message);
    }
  }


}



 


