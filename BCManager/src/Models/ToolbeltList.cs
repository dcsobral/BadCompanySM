using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BCM.Models
{
  [Serializable]
  public class ToolbeltList
  {
    private ItemStack[] inventory = null;
    private int selecteditemSlot = 0;
    private ItemStack selecteditem = new ItemStack();

    public ToolbeltList()
    {
    }

    public ToolbeltList(PlayerDataFile _pdf, EntityPlayer _pl)
    {
      Load(_pdf, _pl);
    }

    public void Load(PlayerDataFile _pdf, EntityPlayer _pl)
    {
      if (_pl != null)
      {
        inventory = _pl.inventory.GetSlots();
        int idx = 1;
        foreach (ItemStack i in inventory)
        {
          ItemStack xi = i;
          if (i.itemValue.type == 0)
          {
            // get items from _pdf until they have been held at least once to force an update, could result in showing an item from saved data when no item held
            // todo: fix function, doesnt seem to be pulling data correctly
            xi = _pdf.inventory[idx];
          }
          inventory[idx] = xi;
        }
        selecteditemSlot = _pl.inventory.holdingItemIdx + 1;
        if (selecteditemSlot > 0)
        {
          selecteditem = inventory[_pl.inventory.holdingItemIdx];
        }
      }
      else
      {
        inventory = _pdf.inventory;
        selecteditemSlot = _pdf.selectedInventorySlot + 1;
        if (selecteditemSlot > 0)
        {
          selecteditem = inventory[_pdf.selectedInventorySlot];
        }
      }

    }

    public string Display()
    {
      //SELECTED ITEM
      string output = "SelectedItem:";
      output += selecteditemSlot.ToString();
      int xt = selecteditem.itemValue.type;
      if (xt != 0)
      {
        ItemClass ic = ItemClass.list[xt];
        if (xt > 4096)
        {
          xt = xt - 4096;
        }
        output += "[" + ic.Name + "(" + xt + ")]";
      }
      output += "\n";


      // todo: refine error checking
      try
      {
        //TOOLBELT
        output += "Toolbelt={\n";
        bool first = true;
        int idx = 1;
        foreach (ItemStack i in inventory)
        {
          if (!first) { output += ",\n"; } else { first = false; }
          int it = i.itemValue.type;
          if (it != 0)
          {
            ItemClass ic = ItemClass.list[it];
            if (it > 4096)
            {
              it = it - 4096;
            }
            output += idx + ":" + ic.Name + "(" + it + ")*" + i.count + "";
          }
          else
          {
            output += idx + ":";
          }
          idx++;
        }
        output += "\n}\n";
      }
      catch
      {
        output += "Toolbelt Item Error\n";
      }

      return output;
    }
  }
}