using System;
using JetBrains.Annotations;

namespace BCM.Models
{
  public class BCMLootBuffAction
  {
    [UsedImplicitly] public string BuffId;
    [UsedImplicitly] public double Chance;

    public BCMLootBuffAction([NotNull] MultiBuffClassAction buffAction)
    {
      BuffId = buffAction.Class.Id;
      Chance = Math.Round(buffAction.Chance, 6);
    }

    public BCMLootBuffAction(string buffId, double chance)
    {
      BuffId = buffId;
      Chance = chance;
    }
  }
}
