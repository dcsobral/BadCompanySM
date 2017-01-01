using System.Collections.Generic;

namespace BCM
{
  public class BlockItemList
  {
    private static BlockItemList instance;

    public static BlockItemList Instance
    {
      get
      {
        if (instance == null)
        {
          instance = new BlockItemList();
        }
        return instance;
      }
    }

    private BlockItemList()
    {
    }

    private SortedDictionary<string, ItemValue> blockitems = new SortedDictionary<string, ItemValue>();

    public List<string> BlockItemNames
    {
      get { return new List<string>(blockitems.Keys); }
    }

    public ItemValue GetItemValue(string blockitemName)
    {
      if (blockitems.ContainsKey(blockitemName))
      {
        return blockitems[blockitemName].Clone();
      }
      else
      {
        blockitemName = blockitemName.ToLower();
        foreach (KeyValuePair<string, ItemValue> kvp in blockitems)
        {
          if (kvp.Key.ToLower().Equals(blockitemName))
          {
            return kvp.Value.Clone();
          }
        }
        return null;
      }
    }

    public void Init()
    {
      NGuiInvGridCreativeMenu cm = new NGuiInvGridCreativeMenu();
      foreach (ItemStack invF in cm.GetAllItems())
      {
        ItemClass ib = ItemClass.list[invF.itemValue.type];
        string name = ib.GetItemName();
        if (name != null && name.Length > 0)
        {
          if (!blockitems.ContainsKey(name))
          {
            blockitems.Add(name, invF.itemValue);
          }
          else
          {
            //Log.Out ("" + Config.ModPrefix + " Item \"" + name + "\" already in list!");
          }
        }
      }
      foreach (ItemStack invF in cm.GetAllBlocks())
      {
        ItemClass ib = ItemClass.list[invF.itemValue.type];
        string name = ib.GetItemName();
        if (name != null && name.Length > 0)
        {
          if (!blockitems.ContainsKey(name))
          {
            blockitems.Add(name, invF.itemValue);
          }
          else
          {
            //Log.Out ("" + Config.ModPrefix + " Item \"" + name + "\" already in list!");
          }
        }
      }
    }

  }
}
