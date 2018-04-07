using RWG2.Rules;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace BCM.Models
{
  public class BCMRuleset
  {
    [UsedImplicitly] public string Name;
    [UsedImplicitly] public string TerrainGen;
    [UsedImplicitly] public string BiomeGen;
    [UsedImplicitly] public int CellCache;
    [UsedImplicitly] public int CellSize;
    [UsedImplicitly] public double CellOffset;
    [UsedImplicitly] public int GenDist;
    [NotNull] [UsedImplicitly] public Dictionary<string, BCMFilterData> CellRules = new Dictionary<string, BCMFilterData>();

    public BCMRuleset(Ruleset ruleset)
    {
      Name = ruleset.Name;
      TerrainGen = ruleset.TerrainGenerator;
      BiomeGen = ruleset.BiomeGenerator;
      CellCache = ruleset.CellCacheSize;
      CellSize = ruleset.CellSize;
      CellOffset = ruleset.CellOffset;
      GenDist = ruleset.GenerationDistanceFromCenter;
      foreach (var filter in ruleset.CellRules)
      {
        CellRules.Add(filter.Key, new BCMFilterData(filter.Value));
      }
    }
  }
}
