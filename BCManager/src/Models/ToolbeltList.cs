using System;
using System.Collections.Generic;

namespace BCM.Models
{
  [Serializable]
  public class ToolbeltList : AbstractList
  {
    private ItemStack[] inventory = null;
    private int selecteditemSlot = 0;
    private ItemStack selecteditem = new ItemStack();

    public ToolbeltList(PlayerInfo _pInfo, Dictionary<string, string> _options) : base(_pInfo, _options)
    {
    }

    public override void Load(PlayerInfo _pInfo)
    {
      if (_pInfo.EP != null)
      {
        inventory = _pInfo.EP.inventory.GetSlots();
        int idx = 1;
        foreach (ItemStack i in inventory)
        {
          ItemStack xi = i;
          if (i.itemValue.type == 0)
          {
            // get items from _pdf until they have been held at least once to force an update, could result in showing an item from saved data when no item held
            // todo: fix function, doesnt seem to be pulling data correctly
            xi = _pInfo.PDF.inventory[idx];
          }
          inventory[idx] = xi;
        }
        selecteditemSlot = _pInfo.EP.inventory.holdingItemIdx + 1;
        if (selecteditemSlot > 0)
        {
          selecteditem = inventory[_pInfo.EP.inventory.holdingItemIdx];
        }
      }
      else
      {
        inventory = _pInfo.PDF.inventory;
        selecteditemSlot = _pInfo.PDF.selectedInventorySlot + 1;
        if (selecteditemSlot > 0)
        {
          selecteditem = inventory[_pInfo.PDF.selectedInventorySlot];
        }
      }

    }

    public override string Display(string sep = " ")
    {
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
      output += sep;

      output += "Toolbelt:{";
      bool first = true;
      int idx = 1;
      foreach (ItemStack i in inventory)
      {
        if (!first) { output += sep; } else { first = false; }
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
      output += "}" + sep;

      return output;
    }
  }
}