using System;
using System.Collections.Generic;

namespace BCM.Models
{
  [Serializable]
  public class BCMBiome : BCMAbstract
  {
    #region Filters
    public static class StrFilters
    {
      public const string Name = "name";
    }

    private static Dictionary<int, string> _filterMap = new Dictionary<int, string>
    {
      { 0,  StrFilters.Name }
    };
    public static Dictionary<int, string> FilterMap
    {
      get => _filterMap;
      set => _filterMap = value;
    }
    #endregion

    #region Properties
    public string Name;
    #endregion;

    public BCMBiome(object obj, string typeStr, Dictionary<string, string> options, List<string> filters) : base(obj, typeStr, options, filters)
    {
    }

    public override void GetData(object obj)
    {
      var biomeDefinition = obj as BiomeDefinition;
      if (biomeDefinition == null) return;

      if (IsOption("filter"))
      {
        foreach (var f in StrFilter)
        {
          switch (f)
          {
            case StrFilters.Name:
              GetName(biomeDefinition);
              break;
            default:
              Log.Out($"{Config.ModPrefix} Unknown filter {f}");
              break;
          }
        }
      }
      else
      {
        GetName(biomeDefinition);
      }
    }

    private void GetName(BiomeDefinition biome)
    {
      Name = biome.m_sBiomeName;
      Bin.Add("Name", Name);
    }
  }
}
