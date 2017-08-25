using System;
using System.Collections.Generic;

namespace BCM.Models
{
  [Serializable]
  public class BCMQuest : BCMAbstract
  {
    #region Filters
    public static class StrFilters
    {
      public const string Id = "id";
      public const string Name = "name";
    }

    private static Dictionary<int, string> _filterMap = new Dictionary<int, string>
    {
      { 0,  StrFilters.Id },
      { 1,  StrFilters.Name }
    };
    public static Dictionary<int, string> FilterMap
    {
      get => _filterMap;
      set => _filterMap = value;
    }
    #endregion

    #region Properties
    public string Id;
    public string Name;
    #endregion;

    public BCMQuest(object obj, string typeStr, Dictionary<string, string> options, List<string> filters) : base(obj, typeStr, options, filters)
    {
    }

    public override void GetData(object obj)
    {
      var questClass = obj as QuestClass;
      if (questClass == null) return;

      if (IsOption("filter"))
      {
        foreach (var f in StrFilter)
        {
          switch (f)
          {
            case StrFilters.Id:
              GetId(questClass);
              break;
            case StrFilters.Name:
              GetName(questClass);
              break;
            default:
              Log.Out($"{Config.ModPrefix} Unknown filter {f}");
              break;
          }
        }
      }
      else
      {
        GetId(questClass);
        GetName(questClass);
      }

    }

    private void GetName(QuestClass questClass) => Bin.Add("Name", Name = questClass.Name);

    private void GetId(QuestClass questClass) => Bin.Add("Id", Id = questClass.ID);
  }
}
