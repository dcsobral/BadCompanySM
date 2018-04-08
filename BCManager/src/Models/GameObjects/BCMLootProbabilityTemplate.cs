using System;
using System.Collections.Generic;
using JetBrains.Annotations;

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

    private static readonly Dictionary<int, string> _filterMap = new Dictionary<int, string>
    {
      { 0,  StrFilters.Name },
      { 1, StrFilters.Templates }
    };
    public static Dictionary<int, string> FilterMap => _filterMap;
    #endregion

    #region Properties
    [UsedImplicitly] public string Name;
    [NotNull] [UsedImplicitly] public List<BCMLootEntry> Templates = new List<BCMLootEntry>();
    #endregion;

    public BCMLootProbabilityTemplate(object obj, string typeStr, Dictionary<string, string> options, List<string> filters) : base(obj, typeStr, options, filters)
    {
    }

    protected override void GetData(object obj)
    {
      if (!(obj is LootContainer.LootProbabilityTemplate loot)) return;

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

    private void GetName(LootContainer.LootProbabilityTemplate loot) => Bin.Add("Name", Name = loot.name);
  }
}
