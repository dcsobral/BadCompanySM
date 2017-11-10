using System.Collections.Generic;
using System.Linq;

namespace BCM.Models
{
  public class BCMSlot
  {
    public string Name;
    //public string UISlot;
    public string EqSlot;
    public string EqLayer;
    public bool AltHair;
    public List<string> Textures;
    public List<string> Colors;
    public Dictionary<string, string> Masks;

    public BCMSlot(UMASlot slot)
    {
      Name = slot.Name;
      //UISlot = slot.UISlot.ToString();
      EqSlot = slot.EquipmentSlot.ToString();
      EqLayer = slot.EquipmentLayer.ToString();
      AltHair = slot.ShowAltHair;
      Textures = slot.Textures;
      Colors = slot.Colors;
      Masks = slot.Masks.ToDictionary(s => s.EquipmentSlot.ToString(), s => s.EquipmentLayer.ToString());
    }
  }
}
