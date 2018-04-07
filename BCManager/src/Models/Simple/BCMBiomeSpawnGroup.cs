using JetBrains.Annotations;

namespace BCM.Models
{
  public class BCMBiomeSpawnGroup
  {
    [UsedImplicitly] public string Group;
    [UsedImplicitly] public string Time;
    [UsedImplicitly] public int Max;
    [UsedImplicitly] public int Delay;
    [UsedImplicitly] public double Dead;

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
