using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace BCM.Models
{
  public class BCMSlot
  {
    [UsedImplicitly] public string Name;
    [UsedImplicitly] public string EqSlot;
    [UsedImplicitly] public string EqLayer;
    [UsedImplicitly] public bool AltHair;
    [NotNull] [UsedImplicitly] public List<string> Textures;
    [NotNull] [UsedImplicitly] public List<string> Colors;
    [NotNull] [UsedImplicitly] public Dictionary<string, string> Masks;

    public BCMSlot([NotNull] UMASlot slot)
    {
      Name = slot.Name;
      EqSlot = slot.EquipmentSlot.ToString();
      EqLayer = slot.EquipmentLayer.ToString();
      AltHair = slot.ShowAltHair;
      Textures = slot.Textures;
      Colors = slot.Colors;
      Masks = slot.Masks.ToDictionary(s => s.EquipmentSlot.ToString(), s => s.EquipmentLayer.ToString());
    }
  }
}
