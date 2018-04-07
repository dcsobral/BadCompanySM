using System.Collections.Generic;
using JetBrains.Annotations;

namespace BCM.Models
{
  public class BCMItemStack
  {
    [UsedImplicitly] public int Type;
    [UsedImplicitly] public int Quality;
    [UsedImplicitly] public int UseTimes;
    [UsedImplicitly] public int MaxUse;
    [UsedImplicitly] public int AmmoIndex;
    [UsedImplicitly] public int Count;
    [UsedImplicitly] public int Meta;
    [CanBeNull] [UsedImplicitly] public List<BCMAttachment> Attachments;
    [NotNull] [UsedImplicitly] public List<BCMParts> Parts = new List<BCMParts>();

    public BCMItemStack(ItemStack item)
    {
      if (item == null || item.itemValue.type == 0) return;

      Type = item.itemValue.type;
      Quality = item.itemValue.Quality;
      UseTimes = item.itemValue.UseTimes;
      MaxUse = item.itemValue.MaxUseTimes;
      AmmoIndex = item.itemValue.SelectedAmmoTypeIndex;
      Count = item.count;
      Meta = item.itemValue.Meta;

      if (item.itemValue.Attachments != null && item.itemValue.Attachments.Length > 0)
      {
        Attachments = new List<BCMAttachment>();
        foreach (var attachment in item.itemValue.Attachments)
        {
          if (attachment == null || attachment.type == 0) continue;

          Attachments.Add(new BCMAttachment(attachment));
        }
      }

      if (item.itemValue.Parts == null || item.itemValue.Parts.Length <= 0) return;

      foreach (var part in item.itemValue.Parts)
      {
        if (part == null || part.type == 0) continue;

        Parts.Add(new BCMParts(part));
      }
    }
  }
}