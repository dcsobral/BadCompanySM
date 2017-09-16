using RWG2.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BCM.Models
{
  [Serializable]
  public class BCMRWG : BCMAbstract
  {
    #region Filters
    public static class StrFilters
    {
      public const string Rulesets = "rules";
      public const string CellRules = "cells";
      public const string HubRules = "hubs";
      public const string WildernessRules = "wilderness";
      public const string PrefabSpawnRules = "prefabs";
      public const string BiomeSpawnRules = "biomes";
      public const string HubLayouts = "layouts";
    }

    private static Dictionary<int, string> _filterMap = new Dictionary<int, string>
    {
      { 0,  StrFilters.Rulesets },
      { 1,  StrFilters.CellRules },
      { 2,  StrFilters.HubRules },
      { 3,  StrFilters.WildernessRules },
      { 4,  StrFilters.PrefabSpawnRules },
      { 5,  StrFilters.BiomeSpawnRules },
      { 6,  StrFilters.HubLayouts }
    };
    public static Dictionary<int, string> FilterMap => _filterMap;

    #endregion

    #region Properties

    public class BCMVector2
    {
      public int x;
      public int y;
      public BCMVector2()
      {
        x = 0;
        y = 0;
      }
      public BCMVector2(int x, int y)
      {
        this.x = x;
        this.y = y;
      }
      public BCMVector2(Vector2 v)
      {
        x = Mathf.RoundToInt(v.x);
        y = Mathf.RoundToInt(v.y);
      }
      public BCMVector2(Vector2i v)
      {
        x = v.x;
        y = v.y;
      }
    }

    public class BCMVector2D
    {
      public double x;
      public double y;
      public BCMVector2D(Vector2 v)
      {
        x = Math.Round(v.x, 3);
        y = Math.Round(v.y, 3);
      }
    }

    public class BCMFilterData
    {
      public double Prob;
      public BCMVector2 Grid;

      public BCMFilterData(FilterData filterData)
      {
        Prob = Math.Round(filterData.Probability, 3);
        if (!filterData.HasGridPosition) return;

        Grid = new BCMVector2(filterData.GridPosition);
      }
    }

    public class BCMRuleset
    {
      public string Name;
      public string TerrainGen;
      public string BiomeGen;
      public int CellCache;
      public int CellSize;
      public double CellOffset;
      public int GenDist;
      public Dictionary<string, BCMFilterData> CellRules = new Dictionary<string, BCMFilterData>();

      public BCMRuleset(Ruleset ruleset)
      {
        Name = ruleset.Name;
        TerrainGen = ruleset.TerrainGenerator;
        BiomeGen = ruleset.BiomeGenerator;
        CellCache = ruleset.CellCacheSize;
        CellSize = ruleset.CellSize;
        CellOffset = ruleset.CellOffset;
        GenDist = ruleset.GenerationDistanceFromCenter;
        foreach (var filter in ruleset.CellRules)
        {
          CellRules.Add(filter.Key, new BCMFilterData(filter.Value));
        }
      }
    }

    public class BCMCellRule
    {
      public string Name;
      public BCMVector2 CavesMinMax;
      public int PathType;
      public int PathRadius;
      public Dictionary<string, double> HubRules;
      public Dictionary<string, double> WildernessRules;

      public BCMCellRule(CellRule cellRule)
      {
        Name = cellRule.Name;
        CavesMinMax = new BCMVector2(cellRule.CavesMinMax);
        PathType = cellRule.PathMaterial.type;
        PathRadius = cellRule.PathRadius;
        HubRules = cellRule.HubRules.ToDictionary(f => f.Key, f => Math.Round(f.Value, 2));
        WildernessRules = cellRule.WildernessRules.ToDictionary(f => f.Key, f => Math.Round(f.Value, 2));
      }
    }

    public class BCMStreetGenerationData
    {
      public int Level;
      public int Mult;
      public string Axiom;
      public Dictionary<string, string> Rules;
      public string[] Alts;

      public BCMStreetGenerationData(StreetGenerationData streetGenData)
      {
        Level = streetGenData.Level;
        Mult = streetGenData.LengthMultiplier;
        Axiom = streetGenData.Axiom;
        Rules = streetGenData.Rules;
        Alts = streetGenData.AltCommands;
      }
    }

    public class BCMHubRule
    {
      public double DtZone;
      public string Name;
      public string HubType;
      public BCMVector2 Width;
      public BCMVector2 Height;
      public int PathType;
      public int PathRadius;
      public BCMStreetGenerationData Streets;
      public string Layout;
      public Dictionary<string, object> PrefabRules;

      public BCMHubRule(HubRule hubRule)
      {
        DtZone = Math.Round(hubRule.DowntownZoningPerc, 3);
        Name = hubRule.Name;
        HubType = hubRule.HubType.ToString();
        Width = new BCMVector2(hubRule.WidthMinMax);
        Height = new BCMVector2(hubRule.HeightMinMax);
        PathType = hubRule.PathMaterial.type;
        PathRadius = hubRule.PathRadius;
        Streets = new BCMStreetGenerationData(hubRule.StreetGenData);
        Layout = hubRule.HubLayoutName;
        PrefabRules = hubRule.PrefabSpawnRules.ToDictionary(r => r.Key, r => (object)new { Prob = Math.Round(r.Value.Probability, 3) });
      }
    }
    public class BCMWildernessRule
    {
      public string Name;
      public BCMVector2 MinMax;
      public int PathType;
      public int PathRadius;
      public Dictionary<string, object> PrefabRules;

      public BCMWildernessRule(WildernessRule wildernessRule)
      {
        Name = wildernessRule.Name;
        MinMax = new BCMVector2(wildernessRule.SpawnMinMax);
        PathType = wildernessRule.PathMaterial.type;
        PathRadius = wildernessRule.PathRadius;
        PrefabRules = wildernessRule.PrefabSpawnRules.ToDictionary(r => r.Key, r => (object)new { Prob = Math.Round(r.Value.Probability, 3) });
      }
    }

    public class BCMPrefabInfo
    {
      public string Name;
      public double Prob;
      public bool List;
      //public bool IsGroup;
      //public string Biome;
      public int Min;
      public int Max;

      public BCMPrefabInfo(PrefabInfo info)
      {
        Name = info.Name;
        Prob = Math.Round(info.Prob, 3);
        List = info.FilteredList;
        //IsGroup = info.IsPrefabGroup;
        //Biome = info.Biome;
        Min = info.MinCount;
        Max = info.MaxCount;
      }
    }

    public class BCMPrefabSpawnRule
    {
      public string Name;
      public readonly List<BCMPrefabInfo> Prefabs = new List<BCMPrefabInfo>();

      public BCMPrefabSpawnRule(PrefabSpawnRule spawnRule)
      {
        Name = spawnRule.Name;
        if (spawnRule.prefabs == null) return;

        foreach (var prefab in spawnRule.prefabs)
        {
          Prefabs.Add(new BCMPrefabInfo(prefab));
        }
      }
    }
    public class BCMBiomeSpawnRule
    {
      public string Name;
      public double Prob;
      public readonly List<BCMVector2D> BiomeList = new List<BCMVector2D>();
      public readonly List<BCMVector2> DistList = new List<BCMVector2>();
      public readonly List<BCMVector2> TerrainList = new List<BCMVector2>();

      public BCMBiomeSpawnRule(BiomeSpawnRule spawnRule)
      {
        Name = spawnRule.Name;
        Prob = Math.Round(spawnRule.probability, 3);
        if (spawnRule.BiomeGenRanges != null)
        {
          foreach (var b in spawnRule.BiomeGenRanges)
          {
            BiomeList.Add(new BCMVector2D(b));
          }
        }
        if (spawnRule.DistanceFromCenterRanges != null)
        {
          foreach (var d in spawnRule.DistanceFromCenterRanges)
          {
            DistList.Add(new BCMVector2(d));
          }
        }
        if (spawnRule.TerrainGenRanges != null)
        {
          foreach (var t in spawnRule.TerrainGenRanges)
          {
            TerrainList.Add(new BCMVector2(t));
          }
        }
      }
    }
    public class BCMStreetInfo
    {
      public string Name;
      public BCMVector2 StartPoint;
      public BCMVector2 EndPoint;
      public int PathType;
      public int PathRadius;

      public BCMStreetInfo(HubLayout.StreetInfo streetInfo)
      {
        Name = streetInfo.Name;
        StartPoint = new BCMVector2(streetInfo.StartPoint);
        EndPoint = new BCMVector2(streetInfo.EndPoint);
        PathType = streetInfo.PathMaterial.type;
        PathRadius = streetInfo.PathRadius;
      }
    }

    public class BCMLotInfo
    {
      public string Name;
      public BCMVector2 Pos;
      public BCMVector2 Size;
      public int Rot;
      public string Prefab;
      public string Zoning;
      public string Cond;
      public string Age;

      public BCMLotInfo(HubLayout.LotInfo lotInfo)
      {
        Name = lotInfo.Name;
        Pos = new BCMVector2(lotInfo.Position); ;
        Size = new BCMVector2(lotInfo.Size); ;
        Rot = lotInfo.RotationFromNorth;
        Prefab = lotInfo.PrefabName;
        Zoning = lotInfo.Zoning.ToString();
        Cond = lotInfo.Condition;
        Age = lotInfo.Age;
      }
    }

    public class BCMHubLayout
    {
      public string Name;
      public string Type;
      public List<BCMStreetInfo> Streets = new List<BCMStreetInfo>();
      public List<BCMLotInfo> Lots = new List<BCMLotInfo>();

      public BCMHubLayout(HubLayout layout)
      {
        Name = layout.Name;
        Type = layout.TownshipType.ToString();
        foreach (var s in layout.Streets)
        {
          Streets.Add(new BCMStreetInfo(s));
        }
        foreach (var l in layout.Lots)
        {
          Lots.Add(new BCMLotInfo(l));
        }
      }
    }

    public List<BCMRuleset> Rulesets = new List<BCMRuleset>();
    public List<BCMCellRule> CellRules = new List<BCMCellRule>();
    public List<BCMHubRule> HubRules = new List<BCMHubRule>();
    public List<BCMWildernessRule> WildernessRules = new List<BCMWildernessRule>();
    public List<BCMPrefabSpawnRule> PrefabSpawnRules = new List<BCMPrefabSpawnRule>();
    public List<BCMBiomeSpawnRule> BiomeSpawnRules = new List<BCMBiomeSpawnRule>();
    public List<BCMHubLayout> HubLayouts = new List<BCMHubLayout>();
    #endregion;

    public BCMRWG(object obj, string typeStr, Dictionary<string, string> options, List<string> filters) : base(obj, typeStr, options, filters)
    {
    }

    public override void GetData(object obj)
    {
      var rwgSections = obj as Dictionary<string, object>;
      if (rwgSections == null) return;

      if (IsOption("filter"))
      {
        foreach (var f in StrFilter)
        {
          switch (f)
          {
            case StrFilters.Rulesets:
              GetRulesets(rwgSections["Rulesets"]);
              break;
            case StrFilters.CellRules:
              GetCellrules(rwgSections["CellRules"]);
              break;
            case StrFilters.HubRules:
              GetHubRules(rwgSections["HubRules"]);
              break;
            case StrFilters.WildernessRules:
              GetWildernessRules(rwgSections["WildernessRules"]);
              break;
            case StrFilters.PrefabSpawnRules:
              GetPrefabSpawnRules(rwgSections["PrefabSpawnRules"]);
              break;
            case StrFilters.BiomeSpawnRules:
              GetBiomeSpawnRules(rwgSections["BiomeSpawnRules"]);
              break;
            case StrFilters.HubLayouts:
              GetHubLayouts(rwgSections["HubLayouts"]);
              break;
            default:
              Log.Out($"{Config.ModPrefix} Unknown filter {f}");
              break;
          }
        }
      }
      else
      {
        GetRulesets(rwgSections["Rulesets"]);
        GetCellrules(rwgSections["CellRules"]);
        GetHubRules(rwgSections["HubRules"]);
        GetWildernessRules(rwgSections["WildernessRules"]);
        GetPrefabSpawnRules(rwgSections["PrefabSpawnRules"]);
        GetBiomeSpawnRules(rwgSections["BiomeSpawnRules"]);
        GetHubLayouts(rwgSections["HubLayouts"]);
      }
    }
    private void GetHubLayouts(object obj)
    {
      var rwgHubLayouts = obj as Dictionary<string, HubLayout>;
      if (rwgHubLayouts == null) return;
      foreach (var layout in rwgHubLayouts.Values)
      {
        HubLayouts.Add(new BCMHubLayout(layout));
      }
      Bin.Add("HubLayouts", HubLayouts);
    }
    private void GetBiomeSpawnRules(object obj)
    {
      var rwgBiomeSpawnRules = obj as List<BiomeSpawnRule>;
      if (rwgBiomeSpawnRules == null) return;
      foreach (var biomeSpawnRule in rwgBiomeSpawnRules)
      {
        BiomeSpawnRules.Add(new BCMBiomeSpawnRule(biomeSpawnRule));
      }
      Bin.Add("BiomeSpawnRules", BiomeSpawnRules);
    }
    private void GetPrefabSpawnRules(object obj)
    {
      var rwgPrefabSpawnRules = obj as Dictionary<string, PrefabSpawnRule>;
      if (rwgPrefabSpawnRules == null) return;
      foreach (var prefabSpawnRule in rwgPrefabSpawnRules.Values)
      {
        PrefabSpawnRules.Add(new BCMPrefabSpawnRule(prefabSpawnRule));
      }
      Bin.Add("PrefabSpawnRules", PrefabSpawnRules);
    }
    private void GetWildernessRules(object obj)
    {
      var rwgWildernessRules = obj as Dictionary<string, WildernessRule>;
      if (rwgWildernessRules == null) return;
      foreach (var wildernessRule in rwgWildernessRules.Values)
      {
        WildernessRules.Add(new BCMWildernessRule(wildernessRule));
      }
      Bin.Add("WildernessRules", WildernessRules);
    }
    private void GetHubRules(object obj)
    {
      var rwgHubRules = obj as Dictionary<string, HubRule>;
      if (rwgHubRules == null) return;
      foreach (var hubRule in rwgHubRules.Values)
      {
        HubRules.Add(new BCMHubRule(hubRule));
      }
      Bin.Add("HubRules", HubRules);
    }
    private void GetCellrules(object obj)
    {
      var rwgCellRules = obj as Dictionary<string, CellRule>;
      if (rwgCellRules == null) return;
      foreach (var cellRule in rwgCellRules.Values)
      {
        CellRules.Add(new BCMCellRule(cellRule));
      }
      Bin.Add("CellRules", CellRules);
    }

    private void GetRulesets(object obj)
    {
      var rwgRulesets = obj as Dictionary<string, Ruleset>;
      if (rwgRulesets == null) return;
      foreach (var ruleset in rwgRulesets.Values)
      {
        Rulesets.Add(new BCMRuleset(ruleset));
      }
      Bin.Add("Rulesets", Rulesets);
    }
  }
}
