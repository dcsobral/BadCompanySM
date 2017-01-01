using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BCM.Models
{
  [Serializable]
  public class EquipmentList
  {
    private ItemValue[] equipment = null;

    public EquipmentList()
    {
    }

    public EquipmentList(PlayerDataFile _pdf, EntityPlayer _pl)
    {
      Load(_pdf, _pl);
    }

    public void Load(PlayerDataFile _pdf, EntityPlayer _pl)
    {
      if (_pl != null)
      {
        equipment = _pl.equipment.GetItems();
      }
      else
      {
        equipment = _pdf.equipment.GetItems();
      }

    }
    //ItemValue
    public string Display()
    {
      //WORN ITEMS
      string output = "Equipment={\n";
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
          if (!first) { output += ",\n"; } else { first = false; }
          output += ic.EquipSlot + ":" + ic.Name + "(" + xt + ")";
        }
      }
      output += "\n}\n";

      return output;
    }
  }
}