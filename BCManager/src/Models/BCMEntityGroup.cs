using System;
using System.Collections.Generic;

namespace BCM.Models
{
  [Serializable]
  public class BCMEntityGroup : BCMAbstract
  {
    #region Filters
    public static class StrFilters
    {
      public const string Name = "name";
      public const string Entities = "entities";
    }

    private static Dictionary<int, string> _filterMap = new Dictionary<int, string>
    {
      { 0,  StrFilters.Name },
      { 1,  StrFilters.Entities }
    };
    public static Dictionary<int, string> FilterMap
    {
      get => _filterMap;
      set => _filterMap = value;
    }
    #endregion

    #region Properties
    public string Name;

    public class BCMSpawn
    {
      public int EntityClassId;
      public double Prob;
      public int ReqMin;
      public int ReqMax;

      public BCMSpawn(SEntityClassAndProb spawn)
      {
        EntityClassId = spawn.entityClassId;
        Prob = spawn.prob;
        ReqMin = spawn.reqMin;
        ReqMax = spawn.reqMax;
      }
    }

    private List<BCMSpawn> _entities;
    public List<BCMSpawn> Entities
    {
      get => _entities ?? (_entities = new List<BCMSpawn>());
      set => _entities = value;
    }
    #endregion;

    public BCMEntityGroup(object obj, string typeStr, Dictionary<string, string> options, List<string> filters) : base(obj, typeStr, options, filters)
    {
    }

    public override void GetData(object obj)
    {
      var entityGroups = (KeyValuePair<string, List<SEntityClassAndProb>>)obj;

      if (IsOption("filter"))
      {
        foreach (var f in StrFilter)
        {
          switch (f)
          {
            case StrFilters.Name:
              GetName(entityGroups);
              break;
            case StrFilters.Entities:
              GetEntities(entityGroups);
              break;
            default:
              Log.Out($"{Config.ModPrefix} Unknown filter {f}");
              break;
          }
        }
      }
      else
      {
        GetName(entityGroups);
        GetEntities(entityGroups);
      }
    }

    private void GetName(KeyValuePair<string, List<SEntityClassAndProb>> entityGroups) => Bin.Add("Name", Name = entityGroups.Key);

    private void GetEntities(KeyValuePair<string, List<SEntityClassAndProb>> entityGroups)
    {
      foreach (var sEntityClassAndProb in entityGroups.Value)
      {
        Entities.Add(new BCMSpawn(sEntityClassAndProb));
      }
      Bin.Add("Entities", Entities);
    }
  }
}
