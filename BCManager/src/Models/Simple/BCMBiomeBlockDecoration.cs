using System;
using JetBrains.Annotations;

namespace BCM.Models
{
  public class BCMBiomeBlockDecoration
  {
    [UsedImplicitly] public int Type;
    [UsedImplicitly] public double Prob;
    [UsedImplicitly] public string Gen;
    [UsedImplicitly] public int RotMax;

    public BCMBiomeBlockDecoration(BiomeBlockDecoration block)
    {
      Type = block.m_BlockValue.type;
      Prob = Math.Round(block.m_Prob, 6);
      Gen = block.m_resourceGeneration.ToString();
      RotMax = block.randomRotateMax;
    }
  }
}
