namespace BCM.Commands
{
  public class BCHordeSpawners : BCCommandAbstract
  {
    public override void Process()
    {
      lock (EntitySpawner.SpawnQueue)
      {
        SendOutput("Spawn Queue:" + EntitySpawner.SpawnQueue.Count);
        SendJson(EntitySpawner.SpawnQueue);
      }

      lock (EntitySpawner.HordeSpawners)
      {
        SendOutput("Horde Spawners");
        SendJson(EntitySpawner.HordeSpawners);
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
