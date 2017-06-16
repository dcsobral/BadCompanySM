using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using UMA;
using UnityEngine;
using RWG2.Rules;

namespace BCM.Commands
{
  public class ListRWG : BCCommandAbstract
  {
    public override void Process()
    {
      string output = "";
      Dictionary<string, string> data = new Dictionary<string, string>();

      data.Add("CurrentRuleset", (RWGRules.Instance.CurrentRuleset.Name != null ? RWGRules.Instance.CurrentRuleset.Name : ""));

      Dictionary<string, Ruleset> Rulesets = RWGRules.Instance.Rulesets;
      List<string> _rulesets = new List<string>();
      foreach (var ruleset in Rulesets)
      {
        Dictionary<string, string> _ruleset = new Dictionary<string, string>();
        _ruleset.Add("Name", (ruleset.Value.Name != null ? ruleset.Value.Name : ""));
        _ruleset.Add("BiomeGenerator", (ruleset.Value.BiomeGenerator != null ? ruleset.Value.BiomeGenerator : ""));
        _ruleset.Add("TerrainGenerator", (ruleset.Value.TerrainGenerator != null ? ruleset.Value.TerrainGenerator : ""));
        _ruleset.Add("CellSize", ruleset.Value.CellSize.ToString());
        _ruleset.Add("CellOffset", ruleset.Value.CellOffset.ToString());
        _ruleset.Add("GenerationDistanceFromCenter", ruleset.Value.GenerationDistanceFromCenter.ToString());
        _ruleset.Add("CellCacheSize", ruleset.Value.CellCacheSize.ToString());
        List<string> _ruleset_cellrules = new List<string>();
        foreach (var ruleset_cellrule in ruleset.Value.CellRules)
        {
          Dictionary<string, string> _ruleset_cellrule = new Dictionary<string, string>();
          _ruleset_cellrule.Add("Name", ruleset_cellrule.Key);
          if (ruleset_cellrule.Value.HasGridPosition)
          {
            _ruleset_cellrule.Add("GridPosition", ((int)ruleset_cellrule.Value.GridPosition.x).ToString() + ","+ ((int)ruleset_cellrule.Value.GridPosition.y).ToString());
          }
          _ruleset_cellrule.Add("Probability", ruleset_cellrule.Value.Probability.ToString());

          _ruleset_cellrules.Add(BCUtils.toJson(_ruleset_cellrule));
        }
        _ruleset.Add("Ruleset_Cells", BCUtils.toJson(_ruleset_cellrules));

        _rulesets.Add(BCUtils.toJson(_ruleset));
      }
      data.Add("Rulesets", BCUtils.toJson(_rulesets));

      //CELLRULES
      List<string> _cellrules = new List<string>();
      Dictionary<string, CellRule> CellRules = RWGRules.Instance.CellRules;
      foreach (var cellrule in CellRules)
      {
        Dictionary<string, string> _cellrule = new Dictionary<string, string>();
        _cellrule.Add("Name", (cellrule.Value.Name != null ? cellrule.Value.Name : ""));
        _cellrule.Add("CavesMinMax", (cellrule.Value.CavesMinMax != null ? cellrule.Value.CavesMinMax.x.ToString() + "-" + cellrule.Value.CavesMinMax.y.ToString() : ""));
        _cellrule.Add("PathMaterial", cellrule.Value.PathMaterial.type.ToString());
        _cellrule.Add("PathRadius", cellrule.Value.PathRadius.ToString());

        //cellrule_HubRules
        List<string> _cellrule_hubrules = new List<string>();
        foreach (var cellrule_hubrule in cellrule.Value.HubRules)
        {
          Dictionary<string, string> _cellrule_hubrule = new Dictionary<string, string>();
          _cellrule_hubrule.Add("Name", cellrule_hubrule.Key);
          _cellrule_hubrule.Add("Probability", cellrule_hubrule.Value.ToString());

          _cellrule_hubrules.Add(BCUtils.toJson(_cellrule_hubrule));
        }
        _cellrule.Add("Cellrule_Hubs", BCUtils.toJson(_cellrule_hubrules));

        //cellrule_WildernessRules
        List<string> _cellrule_wildernessrules = new List<string>();
        foreach (var cellrule_wildernessrule in cellrule.Value.WildernessRules)
        {
          Dictionary<string, string> _cellrule_wildernessrule = new Dictionary<string, string>();
          _cellrule_wildernessrule.Add("Name", cellrule_wildernessrule.Key);
          _cellrule_wildernessrule.Add("Probability", cellrule_wildernessrule.Value.ToString());

          _cellrule_wildernessrules.Add(BCUtils.toJson(_cellrule_wildernessrule));
        }
        _cellrule.Add("Cellrule_Wilderness", BCUtils.toJson(_cellrule_wildernessrules));

        _cellrules.Add(BCUtils.toJson(_cellrule));
      }
      data.Add("Cellrules", BCUtils.toJson(_cellrules));

      //HUBRULES
      List<string> _hubrules = new List<string>();
      Dictionary<string, HubRule> HubRules = RWGRules.Instance.HubRules;
      foreach (var hubrule in HubRules)
      {
        Dictionary<string, string> _hubrule = new Dictionary<string, string>();
        _hubrule.Add("Name", (hubrule.Value.Name != null ? hubrule.Value.Name : ""));

        _hubrule.Add("DowntownZoningPerc", hubrule.Value.DowntownZoningPerc.ToString());
        _hubrule.Add("HasHubLayout", hubrule.Value.HasHubLayout.ToString());
        _hubrule.Add("HeightMinMax", (hubrule.Value.HeightMinMax != null ? hubrule.Value.HeightMinMax.x.ToString() + "-" + hubrule.Value.HeightMinMax.y.ToString() : ""));
        _hubrule.Add("HubLayoutName", (hubrule.Value.HubLayoutName != null ? hubrule.Value.HubLayoutName : ""));
        _hubrule.Add("HubType", hubrule.Value.HubType.ToString());
        _hubrule.Add("PathMaterial", hubrule.Value.PathMaterial.type.ToString());
        _hubrule.Add("PathRadius", hubrule.Value.PathRadius.ToString());
        _hubrule.Add("WidthMinMax", (hubrule.Value.WidthMinMax != null ? hubrule.Value.WidthMinMax.x.ToString() + "-" + hubrule.Value.WidthMinMax.y.ToString() : ""));

        //hubrule.Value.PrefabSpawnRules
        List<string> _hubrule_prefabspawnrules = new List<string>();
        foreach (var hubrule_prefabspawnrule in hubrule.Value.PrefabSpawnRules)
        {
          Dictionary<string, string> _hubrule_prefabspawnrule = new Dictionary<string, string>();
          _hubrule_prefabspawnrule.Add("Name", hubrule_prefabspawnrule.Key);
          _hubrule_prefabspawnrule.Add("Probability", hubrule_prefabspawnrule.Value.Probability.ToString());
          _hubrule_prefabspawnrule.Add("GridPosition", hubrule_prefabspawnrule.Value.GridPosition.x.ToString() + "/" + hubrule_prefabspawnrule.Value.GridPosition.y.ToString());

          _hubrule_prefabspawnrules.Add(BCUtils.toJson(_hubrule_prefabspawnrule));
        }
        _hubrule.Add("Hubrule_Prefabspawnrule", BCUtils.toJson(_hubrule_prefabspawnrules));

        //hubrule.Value.StreetGenData
        Dictionary<string, string> _hubrule_streetgendata = new Dictionary<string, string>();
        _hubrule_streetgendata.Add("Axiom", (hubrule.Value.StreetGenData.Axiom != null ? hubrule.Value.StreetGenData.Axiom : ""));
        _hubrule_streetgendata.Add("Level", hubrule.Value.StreetGenData.Level.ToString());
        _hubrule_streetgendata.Add("LengthMultiplier", hubrule.Value.StreetGenData.LengthMultiplier.ToString());

        List<string> _streetgendata_altcommands = new List<string>();
        string[] altCommands = hubrule.Value.StreetGenData.AltCommands;
        if (altCommands != null)
        {
          foreach (var command in altCommands)
          {
            if (command != null)
            {
              _streetgendata_altcommands.Add(command);
            }
          }
        }
        _hubrule_streetgendata.Add("AltCommands", BCUtils.toJson(_streetgendata_altcommands));

        Dictionary<string, string> _streetgendata_rules = new Dictionary<string, string>();
        Dictionary<string, string> rules = hubrule.Value.StreetGenData.Rules;
        if (rules != null)
        {
          foreach (var rule in rules)
          {
            _streetgendata_rules.Add(rule.Key, rule.Value);
          }
        }
        _hubrule_streetgendata.Add("Rules", BCUtils.toJson(_streetgendata_rules));

        _hubrule.Add("Hubrule_StreetGenData", BCUtils.toJson(_hubrule_streetgendata));

        _hubrules.Add(BCUtils.toJson(_hubrule));
      }
      data.Add("Hubrules", BCUtils.toJson(_hubrules));

      //WILDERNESSRULES
      List<string> _wildernessrules = new List<string>();
      Dictionary<string, WildernessRule> WildernessRules = RWGRules.Instance.WildernessRules;
      foreach (var wildernessrule in WildernessRules)
      {
        Dictionary<string, string> _wildernessrule = new Dictionary<string, string>();
        _wildernessrule.Add("Name", (wildernessrule.Value.Name != null ? wildernessrule.Value.Name : ""));

        _wildernessrules.Add(BCUtils.toJson(_wildernessrule));
      }
      data.Add("Wildernessrules", BCUtils.toJson(_wildernessrules));

      //PREFABSPAWNRULES
      List<string> _prefabspawnrules = new List<string>();
      Dictionary<string, PrefabSpawnRule> PrefabSpawnRules = RWGRules.Instance.PrefabSpawnRules;
      foreach (var prefabspawnrule in PrefabSpawnRules)
      {
        Dictionary<string, string> _prefabspawnrule = new Dictionary<string, string>();
        _prefabspawnrule.Add("Name", (prefabspawnrule.Value.Name != null ? prefabspawnrule.Value.Name : ""));

        _prefabspawnrules.Add(BCUtils.toJson(_prefabspawnrule));
      }
      data.Add("Prefabspawnrules", BCUtils.toJson(_prefabspawnrules));

      //BIOMESPAWNRULES
      List<string> _biomespawnrules = new List<string>();
      List<BiomeSpawnRule> BiomeSpawnRules = RWGRules.Instance.BiomeSpawnRules;
      foreach (var biomespawnrule in BiomeSpawnRules)
      {
        Dictionary<string, string> _biomespawnrule = new Dictionary<string, string>();
        _biomespawnrule.Add("Name", (biomespawnrule.Name != null ? biomespawnrule.Name : ""));

        _biomespawnrules.Add(BCUtils.toJson(_biomespawnrule));
      }
      data.Add("Biomespawnrules", BCUtils.toJson(_biomespawnrules));

      ///HUBLAYOUTS
      List<string> _hublayouts = new List<string>();
      Dictionary<string, HubLayout> HubLayouts = RWGRules.Instance.HubLayouts;
      foreach (var hublayout in HubLayouts)
      {
        Dictionary<string, string> _hublayout = new Dictionary<string, string>();
        _hublayout.Add("Name", (hublayout.Value.Name != null ? hublayout.Value.Name : ""));

        _hublayouts.Add(BCUtils.toJson(_hublayout));
      }
      data.Add("Hublayouts", BCUtils.toJson(_hublayouts));

      //var TerrainGenerators = new Dictionary<string, TGMCustom>();
      //var BiomeGenerators = new Dictionary<string, TGMCustom>();

      ////PREFABS
      //List<string> _prefabs = new List<string>();
      ////Dictionary<string, Prefab> Prefabs = RWG2.RWGCore.Instance.PrefabDecorator.GetAllPrefabs();
      //Dictionary<string, PrefabInfo> Prefabs = RWGRules.Instance.Prefabs;
      //foreach (var prefab in Prefabs)
      //{
      //  Dictionary<string, string> _prefab = new Dictionary<string, string>();
      //  _prefab.Add("Name", (prefab.Value.Name != null ? prefab.Value.Name : ""));

      //  _prefabs.Add(BCUtils.toJson(_prefab));
      //}
      //data.Add("Prefabs", BCUtils.toJson(_prefabs));

      output = BCUtils.toJson(data);
      SendOutput(output);
    }
  }
}
