using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;

namespace BCM.Models
{
  public class BCMSleeperVolumeDetialed : BCMSleeperVolume
  {
    //todo: find field by params and types
    private const string SpawnsFieldName = "VH";

    [UsedImplicitly] public List<BCMSpawnPoint> SpawnPoints;

    public BCMSleeperVolumeDetialed(int index, SleeperVolume volume, World world) : base(index, volume, world)
    {
      var spawns = typeof(SleeperVolume).GetField(SpawnsFieldName, BindingFlags.NonPublic | BindingFlags.Instance);
      if (spawns == null) return;

      if (!(spawns.GetValue(volume) is List<SleeperVolume.SpawnPoint> spawnPoints)) return;
      SpawnPoints = spawnPoints.Select(sp => new BCMSpawnPoint(sp)).ToList();

      //todo: list active spawns and counters
    }
  }
}
