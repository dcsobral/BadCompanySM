using System;
using System.Collections.Generic;
using Enumerable = System.Linq.Enumerable;

namespace BCM.Models
{
  [Serializable]
  public class BCMBiome : BCMAbstract
  {
    #region Filters
    public static class StrFilters
    {
      public const string Id = "id";
      public const string Name = "name";
      public const string Color = "color";
      public const string Rad = "rad";
      public const string Spec = "spec";
      public const string Depth = "depth";
      public const string Replacements = "replacements";
      public const string Layers = "layers";
      public const string Blocks = "blocks";
      public const string Prefabs = "prefabs";
      public const string SubBiomes = "subbiomes";
    }

    private static Dictionary<int, string> _filterMap = new Dictionary<int, string>
    {
      { 0,  StrFilters.Id },
      { 1,  StrFilters.Name },
      { 2,  StrFilters.Color },
      { 3,  StrFilters.Rad },
      { 4,  StrFilters.Spec },
      { 5,  StrFilters.Depth },
      { 6,  StrFilters.Replacements },
      { 7,  StrFilters.Layers },
      { 8,  StrFilters.Blocks },
      { 9,  StrFilters.Prefabs },
      { 10,  StrFilters.SubBiomes }
    };
    public static Dictionary<int, string> FilterMap => _filterMap;

    #endregion

    #region Properties
    public byte Id;
    public string Name;
    public string Color;
    public int Rad;
    public string Spec;
    public int Depth;
    //public double Freq;
    //public double Prob;
    //public int yLT;
    //public int yGT;
    public Dictionary<string, string> Replacements = new Dictionary<string, string>();
    public List<BCMBiomeLayer> Layers = new List<BCMBiomeLayer>();
    public List<BCMBiomeBlockDecoration> DecoBlocks = new List<BCMBiomeBlockDecoration>();
    public List<BCMBiomePrefabDecoration> DecoPrefabs = new List<BCMBiomePrefabDecoration>();
    public List<BCMSubBiome> SubBiomes = new List<BCMSubBiome>();

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
          Resources.Add(Enumerable.ToList(Enumerable.Select(p, deco => new BCMBiomeBlockDecoration(deco))));
        }
      }
    }

    public class BCMBiomePrefabDecoration
    {
      public string Name;
      public double Prob;

      public BCMBiomePrefabDecoration(BiomePrefabDecoration prefab)
      {
        Name = prefab.m_sPrefabName;
        Prob = Math.Round(prefab.m_Prob, 6);
      }
    }

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

    //public WeatherPackage weatherPackage = new WeatherPackage();
    //public BiomeDefinition.Probabilities weatherProbabilities = new BiomeDefinition.Probabilities();
    //public TGMAbstract m_Terrain;

    #endregion;

    public BCMBiome(object obj, string typeStr, Dictionary<string, string> options, List<string> filters) : base(obj, typeStr, options, filters)
    {
    }

    public override void GetData(object obj)
    {
      var biome = obj as BiomeDefinition;
      if (biome == null) return;

      if (IsOption("filter"))
      {
        foreach (var f in StrFilter)
        {
          switch (f)
          {
            case StrFilters.Id:
              GetId(biome);
              break;
            case StrFilters.Name:
              GetName(biome);
              break;
            case StrFilters.Color:
              GetColor(biome);
              break;
            case StrFilters.Rad:
              GetRad(biome);
              break;
            case StrFilters.Spec:
              GetSpec(biome);
              break;
            case StrFilters.Depth:
              GetDepth(biome);
              break;
            case StrFilters.Replacements:
              GetReplacements(biome);
              break;
            case StrFilters.Layers:
              GetLayers(biome);
              break;
            case StrFilters.Blocks:
              GetDecoBlocks(biome);
              break;
            case StrFilters.Prefabs:
              GetDecoPrefabs(biome);
              break;
            case StrFilters.SubBiomes:
              GetSubBiomes(biome);
              break;
            default:
              Log.Out($"{Config.ModPrefix} Unknown filter {f}");
              break;
          }
        }
      }
      else
      {
        GetId(biome);
        GetName(biome);

        if (!IsOption("full")) return;

        GetColor(biome);
        GetRad(biome);
        GetSpec(biome);
        GetDepth(biome);
        GetLayers(biome);
        GetDecoBlocks(biome);
        GetDecoPrefabs(biome);
        GetSubBiomes(biome);
      }
    }

    private void GetSubBiomes(BiomeDefinition biome)
    {
      foreach (var sub in biome.subbiomes)
      {
        SubBiomes.Add(new BCMSubBiome(sub));
      }
      Bin.Add("SubBiomes", SubBiomes);
    }

    private void GetDecoPrefabs(BiomeDefinition biome)
    {
      foreach (var deco in biome.m_DecoPrefabs)
      {
        DecoPrefabs.Add(new BCMBiomePrefabDecoration(deco));
      }
      Bin.Add("DecoPrefabs", DecoPrefabs);
    }

    private void GetDecoBlocks(BiomeDefinition biome)
    {
      foreach (var deco in biome.m_DecoBlocks)
      {
        DecoBlocks.Add(new BCMBiomeBlockDecoration(deco));
      }
      Bin.Add("DecoBlocks", DecoBlocks);
    }

    private void GetLayers(BiomeDefinition biome)
    {
      foreach (var layer in biome.m_Layers)
      {
        Layers.Add(new BCMBiomeLayer(layer));
      }
      Bin.Add("Layers", Layers);
    }

    private void GetReplacements(BiomeDefinition biome)
    {
      foreach (var r in biome.Replacements)
      {

        if (ItemClass.list[r.Key] == null) continue;
        if (ItemClass.list[r.Key].Name == null) continue;
        if (ItemClass.list[r.Value] == null) continue;
        Replacements.Add(ItemClass.list[r.Key].Name, ItemClass.list[r.Value].Name);
      }
      Bin.Add("Replacements", Replacements);
    }

    private void GetDepth(BiomeDefinition biome) => Bin.Add("Depth", Depth = biome.TotalLayerDepth);

    private void GetSpec(BiomeDefinition biome) => Bin.Add("Spec", Spec = biome.m_SpectrumName);

    private void GetRad(BiomeDefinition biome) => Bin.Add("Rad", Rad = biome.m_RadiationLevel);

    private void GetColor(BiomeDefinition biome) => Bin.Add("Color", Color = BCUtils.UIntToHex(biome.m_uiColor));

    private void GetName(BiomeDefinition biome) => Bin.Add("Name", Name = biome.m_sBiomeName);

    private void GetId(BiomeDefinition biome) => Bin.Add("Id", Id = biome.m_Id);
  }
}
