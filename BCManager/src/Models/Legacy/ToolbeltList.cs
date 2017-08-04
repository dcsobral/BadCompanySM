//using System;
//using System.Collections.Generic;

//namespace BCM.Models.Legacy
//{
//  [Serializable]
//  public class ToolbeltList : AbstractList
//  {
//    private ItemStack[] inventory = null;
//    private int selecteditemSlot = 0;

//    public ToolbeltList(PlayerInfo _pInfo, Dictionary<string, string> _options) : base(_pInfo, _options)
//    {
//    }

//    public override void Load(PlayerInfo _pInfo)
//    {
//      if (_pInfo.EP != null)
//      {
//        ItemStack[] inv = new ItemStack[_pInfo.PDF.inventory.Length];

//        int idx = 0;

//        foreach (ItemStack i in _pInfo.EP.inventory.GetSlots())
//        {
//          ItemStack xi = i;
//          if (i.itemValue == null || i.itemValue.type == 0)
//          {
//            // get items from _pdf until they have been held at least once to force an update, could result in showing an item from saved data when no item held
//            if (idx < _pInfo.PDF.inventory.Length)
//            {
//              xi = _pInfo.PDF.inventory[idx];
//              inv.SetValue(xi, idx);
//            }
//          }
//          if (i.itemValue != null && i.itemValue.type > 0)
//          {
//            inv.SetValue(i, idx);
//          }
//          idx++;
//        }
//        inventory = inv;
//        selecteditemSlot = _pInfo.EP.inventory.holdingItemIdx;
//      }
//      else
//      {
//        inventory = _pInfo.PDF.inventory;
//        selecteditemSlot = _pInfo.PDF.selectedInventorySlot;
//      }

//    }

//    public override string Display(string sep = " ")
//    {
//      string output = "SelectedSlot:";
//      output += (selecteditemSlot).ToString();

//      output += "Toolbelt:{";
//      bool first = true;
//      int idx = 1;
//      foreach (ItemStack i in inventory)
//      {
//        if (!first) { output += sep; } else { first = false; }
//        int it = i.itemValue.type;
//        if (it != 0)
//        {
//          ItemClass ic = ItemClass.list[it];
//          if (it > 4096)
//          {
//            it = it - 4096;
//          }
//          output += idx + ":" + ic.Name + "(" + it + ")*" + i.count + "";
//        }
//        else
//        {
//          output += idx + ":";
//        }
//        idx++;
//      }
//      output += "}";

//      return output;
//    }
//    public Dictionary<string, string> GetInventory()
//    {
//      Dictionary<string, string> inv = new Dictionary<string, string>();

//      int slot = 0;
//      if (inventory != null)
//      {
//        foreach (ItemStack item in inventory)
//        {
//          if (item.itemValue != null && item.itemValue.type != 0)
//          {
//            BCMItem bi = new BCMItem(item);
//            bi.Parse(item);
//            inv.Add(slot.ToString(), bi.GetJson());
//          }
//          slot++;
//        }
//      }

//      return inv;
//    }
//    public int GetSelecteditemSlot()
//    {
//      return selecteditemSlot;
//    }
//  }
//}
