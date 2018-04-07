using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace BCM.Models
{
  public class BCMBiomeLayer
  {
    [UsedImplicitly] public BCMBiomeBlockDecoration Block;
    [UsedImplicitly] public int Depth;
    [UsedImplicitly] public int FillTo;
    [UsedImplicitly] public List<List<BCMBiomeBlockDecoration>> Resources = new List<List<BCMBiomeBlockDecoration>>();

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
