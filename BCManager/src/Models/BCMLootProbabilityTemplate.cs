using System;
using System.Collections.Generic;

namespace BCM.Models
{
  [Serializable]
  public class BCMLootProbabilityTemplate : BCMAbstract
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
    public List<BCMLootEntry> Templates = new List<BCMLootEntry>();
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
    #endregion;

    public BCMLootProbabilityTemplate(object obj, string typeStr, Dictionary<string, string> options, List<string> filters) : base(obj, typeStr, options, filters)
    {
    }

    public override void GetData(object obj)
    {
      var loot = obj as LootContainer.LootProbabilityTemplate;
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

    private void GetTemplates(LootContainer.LootProbabilityTemplate loot)
    {
      foreach (var lootTemplate in loot.templates)
      {
        Templates.Add(new BCMLootEntry(lootTemplate));
      }
      Bin.Add("Templates", Templates);
    }

    private void GetName(LootContainer.LootProbabilityTemplate loot)
    {
      Name = loot.name;
      Bin.Add("Name", Name);
    }
  }
}
