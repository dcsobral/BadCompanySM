using System.Collections.Generic;
using JetBrains.Annotations;

namespace BCM.Models
{
  public class BCMSpawnerClass
  {
    [UsedImplicitly] public string Day;
    [UsedImplicitly] public string Name;
    [UsedImplicitly] public string Group;
    [UsedImplicitly] public string TimeOfDay;
    [UsedImplicitly] public double SpawnDelay;
    [UsedImplicitly] public int TotalAlive;
    [UsedImplicitly] public double WaveDelay;
    [UsedImplicitly] public int WaveMin;
    [UsedImplicitly] public int WaveMax;
    [UsedImplicitly] public int Waves;
    [UsedImplicitly] public bool Attack;
    [UsedImplicitly] public bool OnGround;
    [UsedImplicitly] public bool IgnoreTrigger;
    [UsedImplicitly] public bool Territorial;
    [UsedImplicitly] public int Range;
    [UsedImplicitly] public bool ResetToday;
    [UsedImplicitly] public int DaysToRespawn;
    [UsedImplicitly] public string StartSound;
    [UsedImplicitly] public string StartText;

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
}
