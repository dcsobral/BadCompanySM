using System;
using System.Collections.Generic;

namespace BCM.Models
{
  [Serializable]
  public class BagList : AbstractList
  {
    private ItemStack[] bag = null;

    public BagList(PlayerInfo _pInfo, Dictionary<string, string> _options) : base(_pInfo, _options)
    {
    }

    public override void Load(PlayerInfo _pInfo)
    {
      // Updates are instantly triggered when looting, but not when an item is moved to equipment so there is a delay of up to 30 seconds
      bag = _pInfo.PDF.bag;
    }

    public override string Display(string sep = " ")
    {
      string output = "Backpack:{";
      bool first = true;
      int idx = 1;
      foreach (ItemStack i in bag)
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
          output += idx + ":" + ic.Name + "(" + it + ")*" + i.count;
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