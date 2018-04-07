using RWG2.Rules;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace BCM.Models
{
  public class BCMBiomeSpawnRule
  {
    [UsedImplicitly] public string Name;
    [NotNull] [UsedImplicitly] public readonly List<BCMVector2d> BiomeList = new List<BCMVector2d>();
    [NotNull] [UsedImplicitly] public readonly List<BCMVector2> DistList = new List<BCMVector2>();
    [NotNull] [UsedImplicitly] public readonly List<BCMVector2> TerrainList = new List<BCMVector2>();

    public BCMBiomeSpawnRule([NotNull] BiomeSpawnRule spawnRule)
    {
      Name = spawnRule.Name;
      if (spawnRule.BiomeGenRanges != null)
      {
        foreach (var b in spawnRule.BiomeGenRanges)
        {
          BiomeList.Add(new BCMVector2d(b));
        }
      }
      if (spawnRule.DistanceFromCenterRanges != null)
      {
        foreach (var d in spawnRule.DistanceFromCenterRanges)
        {
          DistList.Add(new BCMVector2(d));
        }
      }
      if (spawnRule.TerrainGenRanges != null)
      {
        foreach (var t in spawnRule.TerrainGenRanges)
        {
          TerrainList.Add(new BCMVector2(t));
        }
      }
    }
  }
}
