using System.Collections.Generic;

namespace BCM.Models
{
  public class BCMSpawnerClass
  {
    public string Day;
    public string Name;
    public string Group;
    public string TimeOfDay;
    public double SpawnDelay;
    public int TotalAlive;
    public double WaveDelay;
    public int WaveMin;
    public int WaveMax;
    public int Waves;
    public bool Attack;
    public bool OnGround;
    public bool IgnoreTrigger;
    public bool Territorial;
    public int Range;
    public bool ResetToday;
    public int DaysToRespawn;
    public string StartSound;
    public string StartText;

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
