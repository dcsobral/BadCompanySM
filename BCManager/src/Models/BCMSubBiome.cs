using System;
using System.Collections.Generic;

namespace BCM.Models
{
  public class BCMSubBiome
  {
    public byte Id;
    public string Name;
    //public uint Color;
    //public int Rad;
    //public string Spec;
    public double Freq;
    public int Depth;
    public double Prob;
    public List<BCMBiomeLayer> Layers = new List<BCMBiomeLayer>();
    public List<BCMBiomeBlockDecoration> DecoBlocks = new List<BCMBiomeBlockDecoration>();
    public List<BCMBiomePrefabDecoration> DecoPrefabs = new List<BCMBiomePrefabDecoration>();

    public BCMSubBiome(BiomeDefinition sub)
    {
      Id = sub.m_Id;
      Name = sub.m_sBiomeName;
      //Color = sub.m_uiColor;
      //Rad = sub.m_RadiationLevel;
      //Spec = sub.m_SpectrumName;
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
