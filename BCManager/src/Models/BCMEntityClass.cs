using System;
using System.Collections.Generic;

namespace BCM.Models
{
  [Serializable]
  public class BCMEntityClass : BCMAbstract
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

    public BCMEntityClass(object obj, string typeStr, Dictionary<string, string> options, List<string> filters) : base(obj, typeStr, options, filters)
    {
    }

    public override void GetData(object obj)
    {
      var entityClass = obj as EntityClass;
      if (entityClass == null) return;

      if (IsOption("filter"))
      {
        foreach (var f in StrFilter)
        {
          switch (f)
          {
            case StrFilters.Id:
              GetId(entityClass);
              break;
            case StrFilters.Name:
              GetName(entityClass);
              break;
            default:
              Log.Out($"{Config.ModPrefix} Unknown filter {f}");
              break;
          }
        }
      }
      else
      {
        GetId(entityClass);
        GetName(entityClass);
      }

    }

    private void GetName(EntityClass entityClass) => Bin.Add("Name", Name = entityClass.entityClassName);

    private void GetId(EntityClass entityClass) => Bin.Add("Id", Id = entityClass.GetHashCode());
  }
}
