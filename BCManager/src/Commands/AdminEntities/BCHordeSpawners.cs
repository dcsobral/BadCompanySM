using System.Collections.Generic;

namespace BCM.Commands
{
  public class BCHordeSpawners : BCCommandAbstract
  {
    public override void Process()
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

      //todo: option to nuke spawn queue
      //todo: option to disable all active hordespawners
    }
  }
}
