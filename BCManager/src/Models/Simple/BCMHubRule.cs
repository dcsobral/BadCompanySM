using RWG2.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace BCM.Models
{
  public class BCMHubRule
  {
    [UsedImplicitly] public double DtZone;
    [UsedImplicitly] public string Name;
    [UsedImplicitly] public string HubType;
    [UsedImplicitly] public BCMVector2 Width;
    [UsedImplicitly] public BCMVector2 Height;
    [UsedImplicitly] public int PathType;
    [UsedImplicitly] public int PathRadius;
    [UsedImplicitly] public BCMStreetGenerationData Streets;
    [UsedImplicitly] public string Layout;
    [NotNull] [UsedImplicitly] public Dictionary<string, object> PrefabRules;

    public BCMHubRule([NotNull] HubRule hubRule)
    {
      DtZone = Math.Round(hubRule.DowntownZoningPerc, 3);
      Name = hubRule.Name;
      HubType = hubRule.HubType.ToString();
      Width = new BCMVector2(hubRule.WidthMinMax);
      Height = new BCMVector2(hubRule.HeightMinMax);
      PathType = hubRule.PathMaterial.type;
      PathRadius = hubRule.PathRadius;
      Streets = new BCMStreetGenerationData(hubRule.StreetGenData);
      Layout = hubRule.HubLayoutName;
      PrefabRules = hubRule.PrefabSpawnRules.ToDictionary(r => r.Key, r => (object)new { Prob = Math.Round(r.Value.Probability, 3) });
    }
  }
}
