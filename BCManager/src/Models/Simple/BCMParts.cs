using JetBrains.Annotations;

namespace BCM.Models
{
  public class BCMParts
  {
    [UsedImplicitly] public int Type;
    [UsedImplicitly] public int Quality;
    [UsedImplicitly] public int UseTimes;
    [UsedImplicitly] public int MaxUse;

    public BCMParts([NotNull] ItemValue parts)
    {
      Type = parts.type;
      Quality = parts.Quality;
      UseTimes = parts.UseTimes;
      MaxUse = parts.MaxUseTimes;
    }
  }
}
