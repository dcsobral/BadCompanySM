using RWG2.Rules;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace BCM.Models
{
  public class BCMHubLayout
  {
    [UsedImplicitly] public string Name;
    [UsedImplicitly] public string Type;
    [NotNull] [UsedImplicitly] public List<BCMStreetInfo> Streets = new List<BCMStreetInfo>();
    [NotNull] [UsedImplicitly] public List<BCMLotInfo> Lots = new List<BCMLotInfo>();

    public BCMHubLayout([NotNull] HubLayout layout)
    {
      Name = layout.Name;
      Type = layout.TownshipType.ToString();

      foreach (var s in layout.Streets)
      {
        Streets.Add(new BCMStreetInfo(s));
      }

      foreach (var l in layout.Lots)
      {
        Lots.Add(new BCMLotInfo(l));
      }
    }
  }
}
