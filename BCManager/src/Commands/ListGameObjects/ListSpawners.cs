using System.Collections.Generic;

namespace BCM.Commands
{
  public class ListSpawners : BCCommandAbstract
  {
    public virtual Dictionary<string, string> jsonObject()
    {
      Dictionary<string, string> data = new Dictionary<string, string>();

      var i = 0;
      foreach (string key in EntitySpawnerClass.list.Keys)
      {
        Dictionary<string, string> details = new Dictionary<string, string>();

        var entitySpawnerClasses = EntitySpawnerClass.list[key];
        details.Add("key", key);
        details.Add("bClampDays", entitySpawnerClasses.bClampDays.ToString());
        details.Add("bDynamicSpawner", entitySpawnerClasses.bDynamicSpawner.ToString());
        details.Add("bWrapDays", entitySpawnerClasses.bWrapDays.ToString());
        details.Add("Count", entitySpawnerClasses.Count().ToString());

        //DAYS
        List<string> days = new List<string>();
        var k = 0;
        for (var j = 0; j < entitySpawnerClasses.Count(); j++)
        {
          Dictionary<string, string> day = new Dictionary<string, string>();
          var d = entitySpawnerClasses.Day(j);
          if (
            (j == 0
              &&
              entitySpawnerClasses.Count() == 1)
            ||
            (j + 1 < entitySpawnerClasses.Count()
              && 
              !entitySpawnerClasses.Day(j).Equals(entitySpawnerClasses.Day(j + 1)))
            || 
            j == entitySpawnerClasses.Count() - 1) 
          {
            if (k==0 && entitySpawnerClasses.Count() > 1)
            {
              k = 1;
            }
            day.Add("day", k.ToString() + "-" + j.ToString());
            k = j + 1;

            day.Add("name", (d.name != null ? d.name : ""));
            day.Add("bAttackPlayerImmediately", d.bAttackPlayerImmediately.ToString());
            day.Add("bIgnoreTrigger", d.bIgnoreTrigger.ToString());//all false
            day.Add("bPropResetToday", d.bPropResetToday.ToString());
            day.Add("bSpawnOnGround", d.bSpawnOnGround.ToString());//all true
            day.Add("bTerritorial", d.bTerritorial.ToString());//all False
            day.Add("daysToRespawnIfPlayerLeft", d.daysToRespawnIfPlayerLeft.ToString());
            day.Add("delayBetweenSpawns", d.delayBetweenSpawns.ToString());
            day.Add("delayToNextWave", d.delayToNextWave.ToString());//all 1
            day.Add("entityGroupName", (d.entityGroupName != null ? d.entityGroupName : ""));
            day.Add("numberOfWaves", d.numberOfWaves.ToString());//all 0
            day.Add("spawnAtTimeOfDay", d.spawnAtTimeOfDay.ToString());//all Any
            day.Add("startSound", (d.startSound != null ? d.startSound : ""));//all null
            day.Add("startText", (d.startText != null ? d.startText : ""));//all null
            day.Add("territorialRange", d.territorialRange.ToString());
            day.Add("totalAlive", d.totalAlive.ToString());
            day.Add("totalPerWaveMax", d.totalPerWaveMax.ToString());
            day.Add("totalPerWaveMin", d.totalPerWaveMin.ToString());

            days.Add(BCUtils.toJson(day));
          }
        }
        details.Add("Days", BCUtils.toJson(days));

        data.Add(i.ToString(), BCUtils.toJson(details));
        i++;
      }

      return data;
    }

    public override void Process()
    {
      string output = "";
      if (_options.ContainsKey("json"))
      {
        output = BCUtils.toJson(jsonObject());

        SendOutput(output);
      }
      else
      {
        //GameManager.Instance.World.GetDynamiceSpawnManager();
        DictionarySave<string, EntitySpawnerClassForDay> esc = EntitySpawnerClass.list;
        foreach (string name in esc.Keys)
        {
          EntitySpawnerClassForDay escfd = esc[name];
          output += name + ":(" + escfd.Count() + ") [clamp=" + escfd.bClampDays + ",dynamic=" + escfd.bDynamicSpawner + ",wrap=" + escfd.bWrapDays + "]" + _sep;
          for (int i = 1; i < escfd.Count(); i++)
          {
            EntitySpawnerClass escday = escfd.Day(i);
            // todo: show groups on days
          }

        }
        SendOutput(output);
      }
    }
  }
}
