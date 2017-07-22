using System.Collections.Generic;
using UnityEngine;

namespace BCM.Commands
{
  public class BCHordeSpawners : BCCommandAbstract
  {
    public override void Process()
    {
      lock (EntitySpawner.spawnQueue)
      {
        SendOutput("Spawn Queue:" + EntitySpawner.spawnQueue.Count);
        SendJson(EntitySpawner.spawnQueue);
      }

      lock (EntitySpawner.hordeSpawners)
      {
        SendOutput("Horde Spawners");
        SendJson(EntitySpawner.hordeSpawners);
        //foreach (KeyValuePair<int, HordeSpawner> hs in EntitySpawner.hordeSpawners)
        //{
        //  SendOutput("Spawner For " + hs.Key + ":" + hs.Value.spawns.Count);
        //}
      }

      //todo: option to nuke spawn queue
      //todo: option to disable all active hordespawners
    }
  }
}
