namespace BCM.Models
{
  public class BCMBiomeSpawnGroup
  {
    public string Group;
    public string Time;
    public int Max;
    public int Delay;
    public double Dead;
    public BCMBiomeSpawnGroup(BiomeSpawnEntityGroupData group)
    {
      Group = group.entityGroupRefName;
      Time = group.daytime.ToString();
      Max = group.maxCount;
      Delay = group.respawnDelayInWorldTime / 24000;
      Dead = group.spawnDeadChance;
    }
  }
}
