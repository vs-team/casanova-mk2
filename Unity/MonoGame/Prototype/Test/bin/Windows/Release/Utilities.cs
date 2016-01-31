namespace Utilities
{

  public static class Random
  {
    private static System.Random generator = new System.Random();

    public static float RandFloat(float min, float max)
    {
      return min + (float)generator.NextDouble() * (max - min);
    }

    public static int RandInt(int min, int max)
    {
      return generator.Next(min, max);
    }
  }
}