using System.Collections.Generic;
using UnityEngine;

namespace BCM.Commands
{
  public class ListBiomeSpawning : BCCommandAbstract
  {
    public override void Process()
    {
      string output = "";
      DictionarySave<string, BiomeSpawnEntityGroupList> bsc = BiomeSpawningClass.list;
      foreach (string biomename in bsc.Keys)
      {
        BiomeSpawnEntityGroupList bsegl = bsc[biomename];
        string groupdata = "";
        foreach (BiomeSpawnEntityGroupData bsegd in bsegl.list)
        {
          groupdata += "[" + "group=" + bsegd.entityGroupRefName + ",time=" + bsegd.daytime + ",max=" + bsegd.maxCount + ",delay=" + (bsegd.respawnDelayInWorldTime/1000/60).ToString() + ",dead=" + bsegd.spawnDeadChance + "]" + _sep;
        }
        output += biomename + ":(" + bsegl.list.Count + ")" + groupdata + _sep;
      }
      SendOutput(output);
    }
  }
}
