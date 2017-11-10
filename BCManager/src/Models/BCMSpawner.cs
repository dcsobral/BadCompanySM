using System;
using System.Collections.Generic;

namespace BCM.Models
{
  [Serializable]
  public class BCMSpawner : BCMAbstract
  {
    #region Filters
    public static class StrFilters
    {
      public const string Key = "key";
      public const string Dynamic = "dynamic";
      public const string WrapDays = "wrapdays";
      public const string ClampDays = "clampdays";
      public const string Spawns = "spawns";
    }

    private static readonly Dictionary<int, string> _filterMap = new Dictionary<int, string>
    {
      { 0,  StrFilters.Key },
      { 1,  StrFilters.Dynamic },
      { 2,  StrFilters.WrapDays },
      { 3,  StrFilters.ClampDays },
      { 4,  StrFilters.Spawns }
    };
    public static Dictionary<int, string> FilterMap => _filterMap;
    #endregion

    #region Properties
    public string Key;
    public bool Dynamic;
    public bool WrapDays;
    public bool ClampDays;
    public List<BCMSpawnerClass> Spawns = new List<BCMSpawnerClass>();
    #endregion;

    public BCMSpawner(object obj, string typeStr, Dictionary<string, string> options, List<string> filters) : base(obj, typeStr, options, filters)
    {
    }

    public override void GetData(object obj)
    {
      if (obj == null) return;

      var spawner = (KeyValuePair<string, EntitySpawnerClassForDay>)obj;
      if (IsOption("filter"))
      {
        foreach (var f in StrFilter)
        {
          switch (f)
          {
            case StrFilters.Key:
              GetKey(spawner.Key);
              break;
            case StrFilters.Dynamic:
              GetDynamic(spawner);
              break;
            case StrFilters.WrapDays:
              GetWrapDays(spawner);
              break;
            case StrFilters.ClampDays:
              GetClampDays(spawner);
              break;
            case StrFilters.Spawns:
              GetSpawns(spawner.Value);
              break;
            default:
              Log.Out($"{Config.ModPrefix} Unknown filter {f}");
              break;
          }
        }
      }
      else
      {
        GetKey(spawner.Key);
        GetDynamic(spawner);
        GetWrapDays(spawner);
        GetClampDays(spawner);
        GetSpawns(spawner.Value);
      }
    }

    private void GetSpawns(EntitySpawnerClassForDay spawnerClassForDay)
    {
      var k = 0;
      for (var i = 0; i < spawnerClassForDay.Count(); i++)
      {
        var day = "";
        //todo: fix this... need to compress before passing the object to BCMSpawner?
        if (
          i == 0 && spawnerClassForDay.Count() == 1
          ||
          i + 1 < spawnerClassForDay.Count() && !spawnerClassForDay.Day(i).Equals(spawnerClassForDay.Day(i + 1))
          ||
          i == spawnerClassForDay.Count() - 1)
        {
          if (k == 0 && spawnerClassForDay.Count() > 1)
          {
            k = 1;
          }
          day = k == i ? $"{k}" : (k == 0 ? "*" : $"{k}-{i}");
          k = i + 1;
        }

        var entitySpawnerClass = spawnerClassForDay.Day(i);
        if (entitySpawnerClass != null)
        {
          Spawns.Add(new BCMSpawnerClass(new KeyValuePair<string, EntitySpawnerClass>(day, entitySpawnerClass)));
        }
      }

      Bin.Add("Spawns", Spawns);
    }

    private void GetClampDays(KeyValuePair<string, EntitySpawnerClassForDay> spawner) => Bin.Add("ClampDays", ClampDays = spawner.Value.bClampDays);

    private void GetWrapDays(KeyValuePair<string, EntitySpawnerClassForDay> spawner) => Bin.Add("WrapDays", WrapDays = spawner.Value.bWrapDays);

    private void GetDynamic(KeyValuePair<string, EntitySpawnerClassForDay> spawner) => Bin.Add("Dynamic", Dynamic = spawner.Value.bDynamicSpawner);

    private void GetKey(string key) => Bin.Add("Key", Key = key);
  }
}
