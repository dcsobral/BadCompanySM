using System.Collections.Generic;
using System.Linq;

namespace BCM.Models
{
  public class BCMBiomeLayer
  {
    public BCMBiomeBlockDecoration Block;
    public int Depth;
    public int FillTo;
    public List<List<BCMBiomeBlockDecoration>> Resources = new List<List<BCMBiomeBlockDecoration>>();
    //public List<List<double>> SumResProbs = new List<List<double>>();
    //public List<double> MaxResProb = new List<double>();

    public BCMBiomeLayer(BiomeLayer layer)
    {
      Block = new BCMBiomeBlockDecoration(layer.m_Block);
      Depth = layer.m_Depth;
      FillTo = layer.m_FillUpTo;
      foreach (var p in layer.m_Resources)
      {
        Resources.Add(p.Select(deco => new BCMBiomeBlockDecoration(deco)).ToList());
      }
    }
  }
}
