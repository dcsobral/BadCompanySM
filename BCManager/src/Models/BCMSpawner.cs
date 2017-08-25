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

    private static Dictionary<int, string> _filterMap = new Dictionary<int, string>
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

    public class BCMSpawnerClass
    {
      public string Day;
      public string Name;
      public string Group;
      public string TimeOfDay;
      public double SpawnDelay;
      public int TotalAlive;
      public double WaveDelay;
      public int WaveMin;
      public int WaveMax;
      public int Waves;
      public bool Attack;
      public bool OnGround;
      public bool IgnoreTrigger;
      public bool Territorial;
      public int Range;
      public bool ResetToday;
      public int DaysToRespawn;
      public string StartSound;
      public string StartText;

      public BCMSpawnerClass(KeyValuePair<string, EntitySpawnerClass> kvp)
      {
        Day = kvp.Key;
        Name = kvp.Value.name;
        Group = kvp.Value.entityGroupName;
        TimeOfDay = kvp.Value.spawnAtTimeOfDay.ToString();
        SpawnDelay = kvp.Value.delayBetweenSpawns;
        TotalAlive = kvp.Value.totalAlive;
        WaveDelay = kvp.Value.delayToNextWave;
        WaveMin = kvp.Value.totalPerWaveMin;
        WaveMax = kvp.Value.totalPerWaveMax;
        Waves = kvp.Value.numberOfWaves;
        Attack = kvp.Value.bAttackPlayerImmediately;
        OnGround = kvp.Value.bSpawnOnGround;
        IgnoreTrigger = kvp.Value.bIgnoreTrigger;
        Territorial = kvp.Value.bTerritorial;
        Range = kvp.Value.territorialRange;
        ResetToday = kvp.Value.bPropResetToday;
        DaysToRespawn = kvp.Value.daysToRespawnIfPlayerLeft;
        StartSound = kvp.Value.startSound;
        StartText = kvp.Value.startText;
      }
    }
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

        //foreach (string key in EntitySpawnerClass.list.Keys)
        //{
        //  Dictionary<string, string> details = new Dictionary<string, string>();

        //  var entitySpawnerClasses = EntitySpawnerClass.list[key];
        //  details.Add("key", key);
        //  details.Add("bClampDays", entitySpawnerClasses.bClampDays.ToString());
        //  details.Add("bDynamicSpawner", entitySpawnerClasses.bDynamicSpawner.ToString());
        //  details.Add("bWrapDays", entitySpawnerClasses.bWrapDays.ToString());
        //  details.Add("Count", entitySpawnerClasses.Count().ToString());

        //  //DAYS
        //  List<string> days = new List<string>();
        //  var k = 0;
        //  for (var j = 0; j < entitySpawnerClasses.Count(); j++)
        //  {
        //    Dictionary<string, string> day = new Dictionary<string, string>();
        //    var d = entitySpawnerClasses.Day(j);
        //    if (
        //      (j == 0
        //       &&
        //       entitySpawnerClasses.Count() == 1)
        //      ||
        //      (j + 1 < entitySpawnerClasses.Count()
        //       &&
        //       !entitySpawnerClasses.Day(j).Equals(entitySpawnerClasses.Day(j + 1)))
        //      ||
        //      j == entitySpawnerClasses.Count() - 1)
        //    {
        //      if (k == 0 && entitySpawnerClasses.Count() > 1)
        //      {
        //        k = 1;
        //      }
        //      day.Add("day", k.ToString() + "-" + j.ToString());
        //      k = j + 1;

        //      day.Add("name", (d.name != null ? d.name : ""));
        //      day.Add("bAttackPlayerImmediately", d.bAttackPlayerImmediately.ToString());
        //      day.Add("bIgnoreTrigger", d.bIgnoreTrigger.ToString());//all false
        //      day.Add("bPropResetToday", d.bPropResetToday.ToString());
        //      day.Add("bSpawnOnGround", d.bSpawnOnGround.ToString());//all true
        //      day.Add("bTerritorial", d.bTerritorial.ToString());//all False
        //      day.Add("daysToRespawnIfPlayerLeft", d.daysToRespawnIfPlayerLeft.ToString());
        //      day.Add("delayBetweenSpawns", d.delayBetweenSpawns.ToString());
        //      day.Add("delayToNextWave", d.delayToNextWave.ToString());//all 1
        //      day.Add("entityGroupName", (d.entityGroupName != null ? d.entityGroupName : ""));
        //      day.Add("numberOfWaves", d.numberOfWaves.ToString());//all 0
        //      day.Add("spawnAtTimeOfDay", d.spawnAtTimeOfDay.ToString());//all Any
        //      day.Add("startSound", (d.startSound != null ? d.startSound : ""));//all null
        //      day.Add("startText", (d.startText != null ? d.startText : ""));//all null
        //      day.Add("territorialRange", d.territorialRange.ToString());
        //      day.Add("totalAlive", d.totalAlive.ToString());
        //      day.Add("totalPerWaveMax", d.totalPerWaveMax.ToString());
        //      day.Add("totalPerWaveMin", d.totalPerWaveMin.ToString());

        //      days.Add(BCUtils.toJson(day));
        //    }
        //  }
        //  details.Add("Days", BCUtils.toJson(days));

        //  data.Add(i.ToString(), BCUtils.toJson(details));
        //  i++;
        //}

      }

    }

    private void GetClampDays(KeyValuePair<string, EntitySpawnerClassForDay> spawner)
    {
      ClampDays = spawner.Value.bClampDays;
      Bin.Add("ClampDays", ClampDays);
    }

    private void GetWrapDays(KeyValuePair<string, EntitySpawnerClassForDay> spawner)
    {
      WrapDays = spawner.Value.bWrapDays;
      Bin.Add("WrapDays", WrapDays);
    }

    private void GetDynamic(KeyValuePair<string, EntitySpawnerClassForDay> spawner)
    {
      Dynamic = spawner.Value.bDynamicSpawner;
      Bin.Add("Dynamic", Dynamic);
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

    private void GetKey(string key) => Bin.Add("Key", Key = key);
  }
}
