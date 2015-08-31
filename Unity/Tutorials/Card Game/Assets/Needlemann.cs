using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Needlemann
{
  public class Needlemann
  {
    public enum Result { c, w, m }
    
    public Result checkForMatch(string Input, string Word)
    {
      int WordSequenceCount = Word.Length + 1;
      int InputSequenceCount = Input.Length + 1;
      

      int[,] ScoreMatrix = new int[InputSequenceCount, WordSequenceCount];

     
      for (int i = 0; i < InputSequenceCount; i++)
      {
        ScoreMatrix[i, 0] = 0;
      }

      for (int j = 0; j < WordSequenceCount; j++)
      {
        ScoreMatrix[0, j] = 0;
      }    

      for (int i = 1; i < InputSequenceCount; i++)
      {
        for (int j = 1; j < WordSequenceCount; j++)
        {
          int ScoreDiagonal = 0;
          if (Word.Substring(j - 1, 1) == Input.Substring(i - 1, 1))
            ScoreDiagonal = ScoreMatrix[i - 1, j - 1] + 0;
          else
            ScoreDiagonal = ScoreMatrix[i - 1, j - 1] + -2;

          int ScoreLeft = ScoreMatrix[i, j - 1] - 1;
          int ScoreUp = ScoreMatrix[i - 1, j] - 1;

          int maxScore = Math.Max(Math.Max(ScoreDiagonal, ScoreLeft), ScoreUp);

          ScoreMatrix[i, j] = maxScore;
        }
      }
      if (ScoreMatrix[InputSequenceCount - 1, WordSequenceCount - 1] == 0)
        return Result.c;
      else if (Math.Abs(ScoreMatrix[InputSequenceCount - 1, WordSequenceCount - 1]) < Word.Length / 2)
        return Result.m;
      else
        return Result.w;
    }
  }
}
                                                                                                                                                                                                                                                                                                                                                                                                  