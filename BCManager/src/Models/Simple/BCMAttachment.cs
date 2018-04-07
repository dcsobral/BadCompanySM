using JetBrains.Annotations;

namespace BCM.Models
{
  public class BCMAttachment
  {
    [UsedImplicitly] public int Type;
    [UsedImplicitly] public int Quality;
    [UsedImplicitly] public int UseTimes;
    [UsedImplicitly] public int MaxUse;

    public BCMAttachment([NotNull] ItemValue attachment)
    {
      Type = attachment.type;
      Quality = attachment.Quality;
      UseTimes = attachment.UseTimes;
      MaxUse = attachment.MaxUseTimes;
    }
  }
}
