using RWG2.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace BCM.Models
{
  public class BCMWildernessRule
  {
    [UsedImplicitly] public string Name;
    [UsedImplicitly] public BCMVector2 MinMax;
    [UsedImplicitly] public int PathType;
    [UsedImplicitly] public int PathRadius;
    [NotNull] [UsedImplicitly] public Dictionary<string, object> PrefabRules;

    public BCMWildernessRule([NotNull] WildernessRule wildernessRule)
    {
      Name = wildernessRule.Name;
      MinMax = new BCMVector2(wildernessRule.SpawnMinMax);
      PathType = wildernessRule.PathMaterial.type;
      PathRadius = wildernessRule.PathRadius;
      PrefabRules = wildernessRule.PrefabSpawnRules.ToDictionary(r => r.Key, r => (object)new { Prob = Math.Round(r.Value.Probability, 3) });
    }
  }
}
