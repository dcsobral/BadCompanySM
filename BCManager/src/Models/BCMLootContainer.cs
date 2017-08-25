using System;
using System.Collections.Generic;

namespace BCM.Models
{
  [Serializable]
  public class BCMLootContainer : BCMAbstract
  {
    #region Filters
    public static class StrFilters
    {
      public const string Id = "id";
    }

    private static Dictionary<int, string> _filterMap = new Dictionary<int, string>
    {
      { 0,  StrFilters.Id }
    };
    public static Dictionary<int, string> FilterMap
    {
      get => _filterMap;
      set => _filterMap = value;
    }
    #endregion

    #region Properties
    public int Id;
    #endregion;

    public BCMLootContainer(object obj, string typeStr, Dictionary<string, string> options, List<string> filters) : base(obj, typeStr, options, filters)
    {
    }

    public override void GetData(object obj)
    {
      var loot = obj as LootContainer;
      if (loot == null) return;

      if (IsOption("filter"))
      {
        foreach (var f in StrFilter)
        {
          switch (f)
          {
            case StrFilters.Id:
              GetId(loot);
              break;
            default:
              Log.Out($"{Config.ModPrefix} Unknown filter {f}");
              break;
          }
        }
      }
      else
      {
        GetId(loot);
      }

    }

    private void GetId(LootContainer loot)
    {
      Id = loot.Id;
      Bin.Add("Id", Id);
    }
  }
}
