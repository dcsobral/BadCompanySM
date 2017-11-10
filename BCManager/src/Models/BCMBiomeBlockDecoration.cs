using System;

namespace BCM.Models
{
  public class BCMBiomeBlockDecoration
  {
    public int Type;
    public double Prob;
    //public double CProb;
    public string Gen;
    public int RotMax;

    public BCMBiomeBlockDecoration(BiomeBlockDecoration block)
    {
      Type = block.m_BlockValue.type;
      Prob = Math.Round(block.m_Prob, 6);
      //CProb = Math.Round(block.m_dClusterProb, 6);
      Gen = block.m_resourceGeneration.ToString();
      RotMax = block.randomRotateMax;
    }
  }
}
