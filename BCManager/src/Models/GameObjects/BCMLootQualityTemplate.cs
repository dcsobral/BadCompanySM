using System;
using System.Collections.Generic;
using JetBrains.Annotations;

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

    private static readonly Dictionary<int, string> _filterMap = new Dictionary<int, string>
    {
      { 0,  StrFilters.Name },
      { 1, StrFilters.Templates }
    };
    public static Dictionary<int, string> FilterMap => _filterMap;
    #endregion

    #region Properties
    [UsedImplicitly] public string Name;
    [NotNull] [UsedImplicitly] public List<BCMLootGroupTemplate> Templates = new List<BCMLootGroupTemplate>();
    #endregion;

    public BCMLootQualityTemplate(object obj, string typeStr, Dictionary<string, string> options, List<string> filters) : base(obj, typeStr, options, filters)
    {
    }

    protected override void GetData(object obj)
    {
      if (!(obj is LootContainer.LootQualityTemplate loot)) return;

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

    private void GetName(LootContainer.LootQualityTemplate loot) => Bin.Add("Name", Name = loot.name);
  }
}
