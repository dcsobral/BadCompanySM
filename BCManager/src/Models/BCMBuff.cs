using System;
using System.Collections.Generic;

namespace BCM.Models
{
  [Serializable]
  public class BCMBuff : BCMAbstract
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

    public BCMBuff(object obj, string typeStr, Dictionary<string, string> options, List<string> filters) : base(obj, typeStr, options, filters)
    {
    }

    public override void GetData(object obj)
    {
      var buff = obj as MultiBuffClass;
      if (buff == null) return;

      if (IsOption("filter"))
      {
        foreach (var f in StrFilter)
        {
          switch (f)
          {
            case StrFilters.Id:
              GetId(buff);
              break;
            case StrFilters.Name:
              GetName(buff);
              break;
            default:
              Log.Out($"{Config.ModPrefix} Unknown filter {f}");
              break;
          }
        }
      }
      else
      {
        GetId(buff);
        GetName(buff);
      }

    }

    private void GetName(MultiBuffClass buff) => Bin.Add("Name", Name = buff.Name);
    private void GetId(MultiBuffClass buff) => Bin.Add("Id", Id = buff.Id);
  }
}
