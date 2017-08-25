using System;
using System.Collections.Generic;
using System.Linq;

namespace BCM.Models
{
  [Serializable]
  public class BCMBiomeSpawn : BCMAbstract
  {
    #region Filters
    public static class StrFilters
    {
      public const string Name = "name";
      public const string Spawns = "spawns";
    }

    private static Dictionary<int, string> _filterMap = new Dictionary<int, string>
    {
      { 0,  StrFilters.Name },
      { 1,  StrFilters.Spawns }
    };
    public static Dictionary<int, string> FilterMap
    {
      get => _filterMap;
      set => _filterMap = value;
    }
    #endregion

    #region Properties
    public string Name;
    public class BCMSpawnGroup
    {
      public string Group;
      public string Time;
      public int Max;
      public int Delay;
      public double Dead;
      public BCMSpawnGroup(BiomeSpawnEntityGroupData group)
      {
        Group = group.entityGroupRefName;
        Time = group.daytime.ToString();
        Max = group.maxCount;
        Delay = group.respawnDelayInWorldTime / 24000;
        Dead = group.spawnDeadChance;
      }
    }
    public List<BCMSpawnGroup> Spawns;
    #endregion;

    public BCMBiomeSpawn(object obj, string typeStr, Dictionary<string, string> options, List<string> filters) : base(obj, typeStr, options, filters)
    {
    }

    public override void GetData(object obj)
    {
      if (obj == null) return;

      var biomespawn = (KeyValuePair<string, BiomeSpawnEntityGroupList>)obj;
      if (IsOption("filter"))
      {
        foreach (var f in StrFilter)
        {
          switch (f)
          {
            case StrFilters.Name:
              GetName(biomespawn.Key);
              break;
            case StrFilters.Spawns:
              GetSpawns(biomespawn);
              break;
            default:
              Log.Out($"{Config.ModPrefix} Unknown filter {f}");
              break;
          }
        }
      }
      else
      {
        GetName(biomespawn.Key);
        GetSpawns(biomespawn);
      }
    }

    private void GetSpawns(KeyValuePair<string, BiomeSpawnEntityGroupList> biomespawn) => Bin.Add("Spawns", Spawns = biomespawn.Value.list.Select(group => new BCMSpawnGroup(group)).ToList());

    private void GetName(string name) => Bin.Add("Name", Name = name);
  }
}
