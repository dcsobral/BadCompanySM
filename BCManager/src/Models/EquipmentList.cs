using System;

namespace BCM.Models
{
  [Serializable]
  public class EquipmentList
  {
    private ItemValue[] equipment = null;

    public EquipmentList()
    {
    }

    public EquipmentList(PlayerInfo _pInfo)
    {
      Load(_pInfo);
    }

    public void Load(PlayerInfo _pInfo)
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