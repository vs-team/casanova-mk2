using UnityEngine;
using System;
using System.Collections;
using System.Linq;

/// <summary>
/// Level of indirection for the depth image,
/// provides:
/// -a frames of depth image (no player information),
/// -an array representing which players are detected,
/// -a segmentation image for each player,
/// -bounds for the segmentation of each player.
/// </summary>
public class DepthWrapper : MonoBehaviour
{

  public DeviceOrEmulator devOrEmu;
  private Kinect.KinectInterface kinect;

  private struct frameData
  {
    public float[] depthImg;
    public float[] depthImgNormalized;
    public float[] rollingVariance;
    public float[] OriginalImage;
    public float[] rollingVarianceNormalized;
    public float[] depthImgNormalizedSmooth;
    public float[] rollingVarianceNormalizedLastFrame;
    public bool[] players;
    public bool[,] segmentation;
    public int[,] bounds;
  }

  public int storedFrames = 1;

  private bool updatedSeqmentation = false;
  private bool newSeqmentation = false;
  //public float[] rollingVariance;
  private Queue frameQueue;

  /// <summary>
  /// Depth image for the latest frame
  /// </summary>
  [HideInInspector]
  public float[] depthImgSmoothed;
  public float[] depthImgNormalizedSmoothed;
  public float[] depthImgNormalizedBlurred;
  public float[] OriginalImage;

  [HideInInspector]
  public float[] rollingVariance;
  public float[] rollingVarianceNormalized;
  public float[] rollingVarianceNormalizedLastFrame;

  /// <summary>
  /// players[i] true iff i has been detected in the frame
  /// </summary>
  [HideInInspector]
  public bool[] players;
  /// <summary>
  /// Array of segmentation images [player, pixel]
  /// </summary>
  [HideInInspector]
  public bool[,] segmentations;
  /// <summary>
  /// Array of bounding boxes for each player (left, right, top, bottom)
  /// </summary>
  [HideInInspector]
  //right,left,up,down : but the image is fliped horizontally.
  public int[,] bounds;

  // Use this for initialization
  void Start()
  {
    kinect = devOrEmu.getKinect();
    //allocate space to store the data of storedFrames frames.
    frameQueue = new Queue(storedFrames);
    //rollingVariance = new float[320 * 240];
    for (int ii = 0; ii < storedFrames; ii++)
    {
      frameData frame = new frameData();
      frame.depthImg = new float[320 * 240];
      frame.depthImgNormalized = new float[320 * 240];
      frame.rollingVariance = new float[320 * 240];
      frame.OriginalImage = new float[320 * 240];
      frame.rollingVarianceNormalized = new float[320 * 240];
      frame.rollingVarianceNormalizedLastFrame = new float[320 * 240];
      frame.depthImgNormalizedSmooth = new float[320 * 240];
      frame.segmentation = new bool[Kinect.Constants.NuiSkeletonCount, 320 * 240];
      frame.bounds = new int[Kinect.Constants.NuiSkeletonCount, 4];
      frameQueue.Enqueue(frame);

    }
    pollDepth();
  }

  // Update is called once per frame
  void Update()
  {
    //pollDepth();
    //processDepth();
  }

  void LateUpdate()
  {
    updatedSeqmentation = false;
    newSeqmentation = false;
  }
  /// <summary>
  /// First call per frame checks if there is a new depth image and updates,
  /// returns true if there is new data
  /// Subsequent calls do nothing have the same return as the first call.
  /// </summary>
  /// <returns>
  /// A <see cref="System.Boolean"/>
  /// </returns>
  public bool pollDepth()
  {
    if (!updatedSeqmentation)
    {
      updatedSeqmentation = true;
      if (kinect.pollDepth())
      {
        newSeqmentation = true;
        frameData frame = (frameData)frameQueue.Dequeue();
        rollingVariance = frame.rollingVariance;
        depthImgSmoothed = frame.depthImg;
        depthImgNormalizedSmoothed = frame.depthImgNormalized;
        OriginalImage = frame.OriginalImage;
        rollingVarianceNormalizedLastFrame = frame.rollingVarianceNormalizedLastFrame;
        rollingVarianceNormalized = frame.rollingVarianceNormalized;
        depthImgNormalizedBlurred = frame.depthImgNormalizedSmooth;
        segmentations = frame.segmentation;
        frameQueue.Enqueue(frame);
        processDepth();
      }
    }
    return newSeqmentation;
  }

  private void processDepth()
  {
    
    for (int ii = 0; ii < 320 * 240; ii++)
    {
      depthImgSmoothed[ii] = kinect.getDepth()[ii];
      OriginalImage[ii] = kinect.getDepth()[ii];
    }


    depthImgNormalizedSmoothed = normalize(depthImgSmoothed);
    for (int b = 0; b < 320 * 240; b++)
    {
      rollingVariance[b] = 0.75f * rollingVariance[b] + 0.25f * calcVariance(depthImgSmoothed[b], b);
    }



    //rollingVariance = calcVariance(depthImgNormalizedSmoothed);
    //rollingVariance = normalize(rollingVariance);
    rollingVarianceNormalized = normalize(rollingVariance);



    for (int i = 0; i < 320 * 240; i++)
    {
      depthImgSmoothed[i] = lerp(depthImgSmoothed[i], kinect.getDepth()[i], rollingVarianceNormalized[i]);//(rollingVarianceNormalized[i] * depthImgNormalizedBlurred[i]) + ((1 - rollingVarianceNormalized[i]) * depthImgSmoothed[i]);
    }
    depthImgSmoothed = normalize(depthImgSmoothed);
    getBlurred(depthImgSmoothed, depthImgSmoothed);
    getBlurred(depthImgSmoothed, depthImgSmoothed);
    getBlurred(depthImgSmoothed, depthImgSmoothed);
    depthImgNormalizedSmoothed = normalize(depthImgSmoothed);
    //////getBlurred(depthImgSmoothed, depthImgSmoothed);
    //////getBlurred(depthImgSmoothed, depthImgSmoothed);
    //////getBlurred(depthImgSmoothed, depthImgSmoothed);
    //////getBlurred(depthImgSmoothed, depthImgSmoothed);


  }

  public void getBlurred(float[] normalDepth, float[] outputSmoothedDepth)
  {
    float[,] gaussianWeights = new float[,] { {0.00000067f,0.00002292f,0.00019117f,0.00038771f,0.00019117f,0.00002292f,0.00000067f}, 
                                                  {0.00002292f,0.00078634f,0.00655965f,0.01330373f,0.00655965f,0.00078633f,0.00002292f}, 
                                                  {0.00019117f,0.00655965f,0.05472157f,0.11098164f,0.05472157f,0.00655965f,0.00019117f},
                                                  {0.00038771f,0.01330373f,0.11098164f,0.22508352f,0.11098164f,0.01330373f,0.00038771f}, 
                                                  {0.00019117f,0.00655965f,0.05472157f,0.11098164f,0.05472157f,0.00655965f,0.00019117f},
                                                  {0.00002292f,0.00078633f,0.00655965f,0.01330373f,0.00655965f,0.00078633f,0.00002292f},
                                                  {0.00000067f,0.00002292f,0.00019117f,0.00038771f,0.00019117f,0.00002292f,0.00000067f}};
    var kernelWidth = gaussianWeights.GetLength(0);
    var kernelHeight = gaussianWeights.GetLength(1);

    for (int i = kernelWidth / 2; i < 320 - kernelWidth / 2; i++)
    {
      for (int j = kernelHeight / 2; j < 240 - kernelHeight / 2; j++)
      {
        var ij = i + j * 320;

        var pixel = 0.0f;
        for (int di = -kernelWidth / 2; di <= kernelWidth / 2; di++)
        {
          for (int dj = -kernelHeight / 2; dj <= kernelHeight / 2; dj++)
          {
            var ij2 = (i + di) + (j + dj) * 320;
            pixel += gaussianWeights[di + kernelWidth / 2, dj + kernelHeight / 2] * normalDepth[ij2];
          }
        }

        outputSmoothedDepth[ij] = pixel;
      }
    }
  }

  public float[] calcVariance(float[] oldData)
  {
    ////if (oldData.Length != newData.Length)
    ////{
    ////    Debug.Log("calcVariance: Array lengths do not match");
    ////    return null;
    ////}
    float[] rollingVariance = new float[oldData.Length];
    for (int i = 0; i < oldData.Length; i++)
    {
      rollingVariance[i] = (kinect.getDepth()[i] * kinect.getDepth()[i]) - (oldData[i] * oldData[i]);
    }
    return rollingVariance;

  }

  public float calcVariance(float oldData, int index)
  {

    return ((kinect.getDepth()[index] * kinect.getDepth()[index]) + (oldData * oldData));


  }

  public float[] normalize(float[] target)
  {
    float maxValue = target.Max();
    //Debug.Log("Max: "+maxValue);
    float minValue = target.Min();
    //Debug.Log("Min: "+minValue);
    if (maxValue == 0)
      throw new Exception("hier zit je probleem dwoes");
    float[] normalized = new float[target.Length];
    for (int i = 0; i < target.Length; i++)
    {
      normalized[i] = (target[i] - minValue) / (maxValue - minValue);
    }
    return normalized;
  }

  public float[] lerpArray(float[] oldValue, float[] newValue, float factor)
  {
    float[] lerped = new float[oldValue.Length];
    for (int i = 0; i < oldValue.Length; i++)
    {
      lerped[i] = (factor * oldValue[i]) * ((1.0f - factor) * newValue[i]);
    }
    return lerped;
  }

  public float lerp(float oldValue, float newValue, float factor)
  {
    float lerped;
    lerped = (factor * oldValue) * ((1.0f - factor) * newValue);
    return lerped;
  }
}
