using System.Collections.Generic;

namespace BCM.Models
{
  public class BCMItemStack
  {
    public int Type;
    public int Quality;
    public int UseTimes;
    public int MaxUse;
    public int AmmoIndex;
    public int Count;
    public List<BCMAttachment> Attachments;
    public List<BCMParts> Parts;

    public BCMItemStack(ItemStack item)
    {
      if (item.itemValue.type == 0) return;

      Type = item.itemValue.type;
      Quality = item.itemValue.Quality;
      UseTimes = item.itemValue.UseTimes;
      MaxUse = item.itemValue.MaxUseTimes;
      AmmoIndex = item.itemValue.SelectedAmmoTypeIndex;
      Count = item.count;

      if (item.itemValue.Attachments != null && item.itemValue.Attachments.Length > 0)
      {
        Attachments = new List<BCMAttachment>();
        foreach (var attachment in item.itemValue.Attachments)
        {
          if (attachment == null || attachment.type == 0) continue;

          Attachments.Add(new BCMAttachment
          {
            Type = attachment.type,
            Quality = attachment.Quality,
            MaxUse = attachment.MaxUseTimes,
            UseTimes = attachment.UseTimes
          });
        }
      }

      if (item.itemValue.Parts != null && item.itemValue.Parts.Length > 0)
      {
        Parts = new List<BCMParts>();
        foreach (var part in item.itemValue.Parts)
        {
          if (part == null || part.type == 0) continue;

          Parts.Add(new BCMParts
          {
            Type = part.type,
            Quality = part.Quality,
            MaxUse = part.MaxUseTimes,
            UseTimes = part.UseTimes
          });
        }
      }
    }
  }
}