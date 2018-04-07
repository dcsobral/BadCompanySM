using System.Collections.Generic;
using JetBrains.Annotations;

namespace BCM.Commands
{
  [UsedImplicitly]
  public class BCHordeSpawners : BCCommandAbstract
  {
    //todo: option to nuke spawn queue
    //todo: option to disable all active hordespawners

    protected override void Process()
    {
      var data = new Dictionary<string, object>();
      lock (EntitySpawner.SpawnQueue)
      {
        data.Add("QueueCount", EntitySpawner.SpawnQueue.Count);
        data.Add("Queue", EntitySpawner.SpawnQueue);
      }

      lock (EntitySpawner.HordeSpawners)
      {
        data.Add("HordeSpawners", EntitySpawner.HordeSpawners);
      }

      SendJson(data);
    }
  }
}
