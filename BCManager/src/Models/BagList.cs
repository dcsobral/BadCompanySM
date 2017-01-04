using System;

namespace BCM.Models
{
  [Serializable]
  public class BagList
  {
    private ItemStack[] bag = null;

    public BagList()
    {
    }

    public BagList(PlayerInfo _pInfo)
    {
      Load(_pInfo);
    }

    public void Load(PlayerInfo _pInfo)
    {
      // todo: try to find the bag info on the live player for immediate updates on connected clients.
      // Updates are instantly triggered when looting, but not when an item is moved to equipment so there is a delay of up to 30 seconds
      bag = _pInfo.PDF.bag;
    }

    public string Display()
    {
      //BAG
      string output = "Backpack={\n";
      bool first = true;
      int idx = 1;
      // todo: refine error checking
      try
      {
        foreach (ItemStack i in bag)
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
        output += "Backpack Item Error\n";
      }

      return output;
    }
  }
}