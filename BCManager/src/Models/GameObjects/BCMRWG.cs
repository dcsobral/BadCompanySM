using RWG2.Rules;
using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace BCM.Models
{
  [Serializable]
  public class BCMRWG : BCMAbstract
  {
    #region Filters
    private static class StrFilters
    {
      public const string RulesetName = "rulesetname";
      public const string Rulesets = "rules";
      public const string CellRules = "cells";
      public const string HubRules = "hubs";
      public const string WildernessRules = "wilderness";
      public const string PrefabSpawnRules = "prefabs";
      public const string BiomeSpawnRules = "biomes";
      public const string HubLayouts = "layouts";
    }

    private static readonly Dictionary<int, string> _filterMap = new Dictionary<int, string>
    {
      { 0,  StrFilters.RulesetName },
      { 1,  StrFilters.Rulesets },
      { 2,  StrFilters.CellRules },
      { 3,  StrFilters.HubRules },
      { 4,  StrFilters.WildernessRules },
      { 5,  StrFilters.PrefabSpawnRules },
      { 6,  StrFilters.BiomeSpawnRules },
      { 7,  StrFilters.HubLayouts }
    };
    public static Dictionary<int, string> FilterMap => _filterMap;
    #endregion

    #region Properties
    [UsedImplicitly] public string RulesetName;
    [NotNull] [UsedImplicitly] public List<BCMRuleset> Rulesets = new List<BCMRuleset>();
    [NotNull] [UsedImplicitly] public List<BCMCellRule> CellRules = new List<BCMCellRule>();
    [NotNull] [UsedImplicitly] public List<BCMHubRule> HubRules = new List<BCMHubRule>();
    [NotNull] [UsedImplicitly] public List<BCMWildernessRule> WildernessRules = new List<BCMWildernessRule>();
    [NotNull] [UsedImplicitly] public List<BCMPrefabSpawnRule> PrefabSpawnRules = new List<BCMPrefabSpawnRule>();
    [NotNull] [UsedImplicitly] public List<BCMBiomeSpawnRule> BiomeSpawnRules = new List<BCMBiomeSpawnRule>();
    [NotNull] [UsedImplicitly] public List<BCMHubLayout> HubLayouts = new List<BCMHubLayout>();
    #endregion;

    public BCMRWG(object obj, string typeStr, Dictionary<string, string> options, List<string> filters) : base(obj, typeStr, options, filters)
    {
    }

    protected override void GetData(object obj)
    {
      if (!(obj is Dictionary<string, object> rwgSections)) return;

      if (IsOption("filter"))
      {
        foreach (var f in StrFilter)
        {
          switch (f)
          {
            case StrFilters.RulesetName:
              GetRulesetName(rwgSections["RulesetName"]);
              break;
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
        GetRulesetName(rwgSections["RulesetName"]);
        GetRulesets(rwgSections["Rulesets"]);
        GetCellrules(rwgSections["CellRules"]);
        GetHubRules(rwgSections["HubRules"]);
        GetWildernessRules(rwgSections["WildernessRules"]);
        GetPrefabSpawnRules(rwgSections["PrefabSpawnRules"]);
        GetBiomeSpawnRules(rwgSections["BiomeSpawnRules"]);
        GetHubLayouts(rwgSections["HubLayouts"]);
      }
    }

    private void GetRulesetName(object obj)
    {
      if (!(obj is string rwgRulesetName)) return;

      Bin.Add("RulesetName", rwgRulesetName);
    }

    private void GetHubLayouts(object obj)
    {
      if (!(obj is Dictionary<string, HubLayout> rwgHubLayouts)) return;

      foreach (var layout in rwgHubLayouts.Values)
      {
        HubLayouts.Add(new BCMHubLayout(layout));
      }
      Bin.Add("HubLayouts", HubLayouts);
    }

    private void GetBiomeSpawnRules(object obj)
    {
      if (!(obj is List<BiomeSpawnRule> rwgBiomeSpawnRules)) return;

      foreach (var biomeSpawnRule in rwgBiomeSpawnRules)
      {
        BiomeSpawnRules.Add(new BCMBiomeSpawnRule(biomeSpawnRule));
      }
      Bin.Add("BiomeSpawnRules", BiomeSpawnRules);
    }

    private void GetPrefabSpawnRules(object obj)
    {
      if (!(obj is Dictionary<string, PrefabSpawnRule> rwgPrefabSpawnRules)) return;

      foreach (var prefabSpawnRule in rwgPrefabSpawnRules.Values)
      {
        PrefabSpawnRules.Add(new BCMPrefabSpawnRule(prefabSpawnRule));
      }
      Bin.Add("PrefabSpawnRules", PrefabSpawnRules);
    }

    private void GetWildernessRules(object obj)
    {
      if (!(obj is Dictionary<string, WildernessRule> rwgWildernessRules)) return;

      foreach (var wildernessRule in rwgWildernessRules.Values)
      {
        WildernessRules.Add(new BCMWildernessRule(wildernessRule));
      }
      Bin.Add("WildernessRules", WildernessRules);
    }

    private void GetHubRules(object obj)
    {
      if (!(obj is Dictionary<string, HubRule> rwgHubRules)) return;

      foreach (var hubRule in rwgHubRules.Values)
      {
        HubRules.Add(new BCMHubRule(hubRule));
      }
      Bin.Add("HubRules", HubRules);
    }

    private void GetCellrules(object obj)
    {
      if (!(obj is Dictionary<string, CellRule> rwgCellRules)) return;

      foreach (var cellRule in rwgCellRules.Values)
      {
        CellRules.Add(new BCMCellRule(cellRule));
      }
      Bin.Add("CellRules", CellRules);
    }

    private void GetRulesets(object obj)
    {
      if (!(obj is Dictionary<string, Ruleset> rwgRulesets)) return;

      foreach (var ruleset in rwgRulesets.Values)
      {
        Rulesets.Add(new BCMRuleset(ruleset));
      }
      Bin.Add("Rulesets", Rulesets);
    }
  }
}
