using RWG2.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace BCM.Models
{
  public class BCMCellRule
  {
    [UsedImplicitly] public string Name;
    [UsedImplicitly] public BCMVector2 CavesMinMax;
    [UsedImplicitly] public int PathType;
    [UsedImplicitly] public int PathRadius;
    [UsedImplicitly] public Dictionary<string, double> HubRules;
    [UsedImplicitly] public Dictionary<string, double> WildernessRules;

    public BCMCellRule([NotNull] CellRule cellRule)
    {
      Name = cellRule.Name;
      CavesMinMax = new BCMVector2(cellRule.CavesMinMax);
      PathType = cellRule.PathMaterial.type;
      PathRadius = cellRule.PathRadius;
      HubRules = cellRule.HubRules.ToDictionary(f => f.Key, f => Math.Round(f.Value, 2));
      WildernessRules = cellRule.WildernessRules.ToDictionary(f => f.Key, f => Math.Round(f.Value, 2));
    }
  }
}
