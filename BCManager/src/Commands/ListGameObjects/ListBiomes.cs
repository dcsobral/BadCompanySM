using System.Collections.Generic;
using UnityEngine;

namespace BCM.Commands
{
  public class ListBiomes : BCCommandAbstract
  {
    //public byte m_Id;
    //public string m_sBiomeName;
    //public uint m_uiColor;
    //public int m_RadiationLevel;
    //public string m_SpectrumName;
    //public static Dictionary<string, byte> nameToId = null;
    //public List<BiomeLayer> m_Layers;
    //public List<BiomeBlockDecoration> m_DecoBlocks;
    //public List<BiomePrefabDecoration> m_DecoPrefabs;
    //public BiomeDefinition.Probabilities weatherProbabilities = new BiomeDefinition.Probabilities();
    //public TGMAbstract m_Terrain;
    //public int TotalLayerDepth;
    //public List<BiomeDefinition> subbiomes = new List<BiomeDefinition>();
    //public float freq = 0.03f;
    //public float prob;
    //public int yLT;
    //public int yGT;
    //public Dictionary<int, int> Replacements = new Dictionary<int, int>();
    private Color GetColor(uint par0001)
    {
      byte b = (byte)(par0001 >> 24);
      byte r = (byte)(par0001 >> 16);
      byte g = (byte)(par0001 >> 8);
      byte b2 = (byte)par0001;
      return new Color32(r, g, b2, 255);
    }

    public override void Process()
    {
      string output = "";
      Dictionary<uint, BiomeDefinition> wb = GameManager.Instance.World.Biomes.GetBiomeMap();
      foreach (BiomeDefinition bd in wb.Values)
      {
        Color c = GetColor(bd.m_uiColor);
        int ri = (int)(c.r * 255f);
        int gi = (int)(c.g * 255f);
        int bi = (int)(c.b * 255f);
        int ai = (int)(c.a * 255f);
        if (_options.ContainsKey("hex"))
        {
          string r = ri.ToString("X").PadLeft(2,'0');
          string g = gi.ToString("X").PadLeft(2, '0');
          string b = bi.ToString("X").PadLeft(2, '0');
          output += bd.m_sBiomeName + ":" + r + g + b + _sep;
        } else
        {
          output += bd.m_sBiomeName + ":" + ri + "," + gi + "," + bi + "," + ai + _sep;
        }
      }
      SendOutput(output);
    }
  }
}
