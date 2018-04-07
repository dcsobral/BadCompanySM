using System.Collections.Generic;
using JetBrains.Annotations;

namespace BCM.Models
{
  public class BCMEffect
  {
    [UsedImplicitly] public string Name;
    [NotNull] [UsedImplicitly] public List<BCMModifier> Modifiers = new List<BCMModifier>();

    public BCMEffect([NotNull] Skill.Effect effect)
    {
      Name = effect.Name;
      foreach (var modifier in effect.Modifiers)
      {
        Modifiers.Add(new BCMModifier(modifier));
      }
    }
  }
}
