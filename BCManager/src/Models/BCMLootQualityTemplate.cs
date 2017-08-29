using System;
using System.Collections.Generic;

namespace BCM.Models
{
  [Serializable]
  public class BCMLootQualityTemplate : BCMAbstract
  {
    #region Filters
    public static class StrFilters
    {
      public const string Name = "name";
      public const string Templates = "templates";
    }

    private static Dictionary<int, string> _filterMap = new Dictionary<int, string>
    {
      { 0,  StrFilters.Name },
      { 1, StrFilters.Templates }
    };
    public static Dictionary<int, string> FilterMap => _filterMap;
    #endregion

    #region Properties
    public string Name;
    public List<BCMLootGroupTemplate> Templates = new List<BCMLootGroupTemplate>();

    public class BCMLootEntry
    {
      public int Item;
      public string Group;
      public double Prob;
      public string Template;
      public int Min;
      public int Max;
      public int MinQual;
      public int MaxQual;
      public double MinLevel;
      public double MaxLevel;
      //public string parentGroup;

      public BCMLootEntry(LootContainer.LootEntry lootEntry)
      {
        if (lootEntry.item != null) Item = lootEntry.item.itemValue.type;
        if (lootEntry.group != null) Group = lootEntry.group.name;
        Prob = Math.Round(lootEntry.prob, 6);
        Template = lootEntry.lootProbTemplate;
        Min = lootEntry.minCount;
        Max = lootEntry.maxCount;
        MinQual = lootEntry.minQuality;
        MaxQual = lootEntry.maxQuality;
        MinLevel = Math.Round(lootEntry.minLevel, 6);
        MaxLevel = Math.Round(lootEntry.maxLevel, 6);
      }
    }

    public class BCMLootGroupTemplate
    {
      public string Name;
      public int MinCount;
      public int MaxCount;
      public string Template;
      public int MinQual;
      public int MaxQual;
      public double MinLevel;
      public double MaxLevel;
      public List<BCMLootEntry> Items = new List<BCMLootEntry>();

      public BCMLootGroupTemplate(LootContainer.LootGroup lootGroup)
      {
        Name = lootGroup.name;
        MinCount = lootGroup.minCount;
        MaxCount = lootGroup.maxCount;
        Template = lootGroup.lootQualityTemplate;
        MinQual = lootGroup.minQuality;
        MaxQual = lootGroup.maxQuality;
        MinLevel = Math.Round(lootGroup.minLevel, 6);
        MaxLevel = Math.Round(lootGroup.maxLevel, 6);
        foreach (var item in lootGroup.items)
        {
          Items.Add(new BCMLootEntry(item));
        }
      }
    }
    #endregion;

    public BCMLootQualityTemplate(object obj, string typeStr, Dictionary<string, string> options, List<string> filters) : base(obj, typeStr, options, filters)
    {
    }

    public override void GetData(object obj)
    {
      var loot = obj as LootContainer.LootQualityTemplate;
      if (loot == null) return;

      if (IsOption("filter"))
      {
        foreach (var f in StrFilter)
        {
          switch (f)
          {
            case StrFilters.Name:
              GetName(loot);
              break;
            case StrFilters.Templates:
              GetTemplates(loot);
              break;
            default:
              Log.Out($"{Config.ModPrefix} Unknown filter {f}");
              break;
          }
        }
      }
      else
      {
        GetName(loot);
        GetTemplates(loot);
      }

    }

    private void GetTemplates(LootContainer.LootQualityTemplate loot)
    {
      foreach (var lootGroup in loot.templates)
      {
        Templates.Add(new BCMLootGroupTemplate(lootGroup));
      }
      Bin.Add("Templates", Templates);
    }

    private void GetName(LootContainer.LootQualityTemplate loot)
    {
      Bin.Add("Name", Name = loot.name);
    }
  }
}
