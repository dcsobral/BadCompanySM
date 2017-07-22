using System;
using System.Collections.Generic;

namespace BCM.Models.Legacy
{
  [Serializable]
  public class EquipmentList : AbstractList
  {
    private ItemValue[] equipment = null;

    public EquipmentList(PlayerInfo _pInfo, Dictionary<string, string> _options) : base(_pInfo, _options)
    {
    }

    public override void Load(PlayerInfo _pInfo)
    {
      if (_pInfo.EP != null)
      {
        equipment = _pInfo.EP.equipment.GetItems();
      }
      else
      {
        equipment = _pInfo.PDF.equipment.GetItems();
      }
    }

    public override string Display(string sep = " ")
    {
      string output = "Equipment:{";
      bool first = true;
      foreach (ItemValue iv in equipment)
      {
        if (iv.type != 0)
        {
          ItemClass ic = ItemClass.list[iv.type];
          int xt = iv.type;
          if (xt > 4096)
          {
            xt = xt - 4096;
          }
          if (!first) { output += sep; } else { first = false; }
          output += ic.EquipSlot + ":" + ic.Name + "(" + xt + ")";
        }
      }
      output += "}";

      return output;
    }

    public Dictionary<string, string> GetEquipment()
    {
      Dictionary<string, string> equipdict = new Dictionary<string, string>();

      int slot = 0;
      if (equipment != null)
      {
        foreach (ItemValue item in equipment)
        {
          if (item != null && item.type != 0)
          {
            BCMItem bi = new BCMItem(item);
            bi.Parse(item);
            equipdict.Add(slot.ToString(), bi.GetJson());
          }
          slot++;
        }
      }

      return equipdict;
    }
  }
}
