using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace BCM.Models
{
  public class BCMSubBiome
  {
    [UsedImplicitly] public byte Id;
    [UsedImplicitly] public string Name;
    [UsedImplicitly] public double Freq;
    [UsedImplicitly] public int Depth;
    [UsedImplicitly] public double Prob;
    [NotNull] [UsedImplicitly] public List<BCMBiomeLayer> Layers = new List<BCMBiomeLayer>();
    [NotNull] [UsedImplicitly] public List<BCMBiomeBlockDecoration> DecoBlocks = new List<BCMBiomeBlockDecoration>();
    [NotNull] [UsedImplicitly] public List<BCMBiomePrefabDecoration> DecoPrefabs = new List<BCMBiomePrefabDecoration>();

    public BCMSubBiome([NotNull] BiomeDefinition sub)
    {
      Id = sub.m_Id;
      Name = sub.m_sBiomeName;
      Freq = Math.Round(sub.freq, 6);
      Depth = sub.TotalLayerDepth;
      Prob = Math.Round(sub.prob, 6);
      foreach (var layer in sub.m_Layers)
      {
        Layers.Add(new BCMBiomeLayer(layer));
      }
      foreach (var deco in sub.m_DecoBlocks)
      {
        DecoBlocks.Add(new BCMBiomeBlockDecoration(deco));
      }
      foreach (var deco in sub.m_DecoPrefabs)
      {
        DecoPrefabs.Add(new BCMBiomePrefabDecoration(deco));
      }
    }
  }
}
