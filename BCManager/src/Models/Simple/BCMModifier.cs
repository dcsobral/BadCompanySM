using System;
using JetBrains.Annotations;

namespace BCM.Models
{
  public class BCMModifier
  {
    [UsedImplicitly] public int MinLevel;
    [UsedImplicitly] public int MaxLevel;
    [UsedImplicitly] public double MinValue;
    [UsedImplicitly] public double MaxValue;

    public BCMModifier([NotNull] Skill.IModifier modifier)
    {
      MinLevel = modifier.MinLevel;
      MaxLevel = modifier.MaxLevel;
      MinValue = Math.Round(modifier.MinValue, 6);
      MaxValue = Math.Round(modifier.MaxValue, 6);
    }
  }
}
