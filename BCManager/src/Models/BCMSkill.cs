using System;
using System.Collections.Generic;

namespace BCM.Models
{
  [Serializable]
  public class BCMSkill : BCMAbstract
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
    public int Id;
    public string Name;
    #endregion;

    public BCMSkill(object obj, string typeStr, Dictionary<string, string> options, List<string> filters) : base(obj, typeStr, options, filters)
    {
    }

    public override void GetData(object obj)
    {
      var skill = obj as Skill;
      if (skill == null) return;

      if (IsOption("filter"))
      {
        foreach (var f in StrFilter)
        {
          switch (f)
          {
            case StrFilters.Id:
              GetId(skill);
              break;
            case StrFilters.Name:
              GetName(skill);
              break;
            default:
              Log.Out($"{Config.ModPrefix} Unknown filter {f}");
              break;
          }
        }
      }
      else
      {
        GetId(skill);
        GetName(skill);
      }

    }

    private void GetName(Skill skill) => Bin.Add("Name", Name = skill.Name);

    private void GetId(Skill skill) => Bin.Add("Id", Id = skill.Id);
  }
}
