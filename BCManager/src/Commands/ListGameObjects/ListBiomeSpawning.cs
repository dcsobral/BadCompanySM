using System.Collections.Generic;
using UnityEngine;

namespace BCM.Commands
{
  public class ListBiomeSpawning : BCCommandAbstract
  {
    public virtual Dictionary<string, Dictionary<string, string>> jsonObject()
    {
      Dictionary<string, Dictionary<string, string>> data = new Dictionary<string, Dictionary<string, string>>();

      DictionarySave<string, BiomeSpawnEntityGroupList> biomeSpawningClass = BiomeSpawningClass.list;
      foreach (string biomeName in biomeSpawningClass.Keys)
      {
        BiomeSpawnEntityGroupList biomeSpawnEntityGroupList = biomeSpawningClass[biomeName];
        Dictionary<string, string> groupData = new Dictionary<string, string>();
        int i = 0;
        foreach (BiomeSpawnEntityGroupData biomeSpawnEntityGroupData in biomeSpawnEntityGroupList.list)
        {
          groupData.Add(i.ToString(), "{ \"group\":\"" + biomeSpawnEntityGroupData.entityGroupRefName + "\", \"time\":\"" + biomeSpawnEntityGroupData.daytime + "\", \"max\":\"" + biomeSpawnEntityGroupData.maxCount + "\", \"delay\":\"" + (biomeSpawnEntityGroupData.respawnDelayInWorldTime / 1000 / 60).ToString() + "\", \"dead\":\"" + biomeSpawnEntityGroupData.spawnDeadChance + "\" }");
          i++;
        }
        data.Add(biomeName, groupData);
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
        DictionarySave<string, BiomeSpawnEntityGroupList> bsc = BiomeSpawningClass.list;
        foreach (string biomename in bsc.Keys)
        {
          BiomeSpawnEntityGroupList bsegl = bsc[biomename];
          string groupdata = "";
          foreach (BiomeSpawnEntityGroupData bsegd in bsegl.list)
          {
            groupdata += "[" + "group=" + bsegd.entityGroupRefName + ",time=" + bsegd.daytime + ",max=" + bsegd.maxCount + ",delay=" + (bsegd.respawnDelayInWorldTime / 1000 / 60).ToString() + ",dead=" + bsegd.spawnDeadChance + "]" + _sep;
          }
          output += biomename + ":(" + bsegl.list.Count + ")" + groupdata + _sep;
        }
        SendOutput(output);
      }
    }
  }
}
