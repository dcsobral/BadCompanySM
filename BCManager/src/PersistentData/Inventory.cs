using System;
using System.Collections.Generic;

namespace BCM.PersistentData
{
  [Serializable]
  public class Inventory
  {
    public List<InvItem> bag;
    public List<InvItem> belt;
    public InvItem[] equipment;

    public Inventory()
    {
      bag = new List<InvItem>();
      belt = new List<InvItem>();
      equipment = null;
    }

    public void Update(PlayerDataFile pdf)
    {
      lock (this)
      {
        //Log.Out ("(" + Config.ModPrefix + ") Updating player inventory - player id: " + pdf.id);
        ProcessInv(bag, pdf.bag, pdf.id);
        ProcessInv(belt, pdf.inventory, pdf.id);
        ProcessEqu(pdf.equipment, pdf.id);
      }
    }

    private void ProcessInv(List<InvItem> target, ItemStack[] sourceFields, int id)
    {
      target.Clear();
      for (int i = 0; i < sourceFields.Length; i++)
      {
        InvItem item = CreateInvItem(sourceFields[i].itemValue, sourceFields[i].count, id);
        if (item != null && sourceFields[i].itemValue.Parts != null)
        {
          ProcessParts(sourceFields[i].itemValue.Parts, item, id);
        }
        target.Add(item);
      }
    }

    private void ProcessEqu(Equipment sourceEquipment, int _playerId)
    {
      equipment = new InvItem[sourceEquipment.GetSlotCount()];
      for (int i = 0; i < sourceEquipment.GetSlotCount(); i++)
      {
        equipment[i] = CreateInvItem(sourceEquipment.GetSlotItem(i), 1, _playerId);
      }
    }

    private void ProcessParts(ItemValue[] _parts, InvItem _item, int _playerId)
    {
      InvItem[] itemParts = new InvItem[_parts.Length];
      for (int i = 0; i < _parts.Length; i++)
      {
        InvItem partItem = CreateInvItem(_parts[i], 1, _playerId);
        if (partItem != null && _parts[i].Parts != null)
        {
          ProcessParts(_parts[i].Parts, partItem, _playerId);
        }
        itemParts[i] = partItem;
      }
      _item.parts = itemParts;
    }

    private InvItem CreateInvItem(ItemValue _itemValue, int _count, int _playerId)
    {
      if (_count > 0 && _itemValue != null && !_itemValue.Equals(ItemValue.None))
      {
        ItemClass itemClass = ItemClass.list[_itemValue.type];
        int maxAllowed = itemClass.Stacknumber.Value;
        string name = itemClass.GetItemName();

        if (_count > maxAllowed)
        {
          Log.Out("(" + Config.ModPrefix + ") Player with ID " + _playerId + " has stack for \"" + name + "\" greater than allowed (" + _count + " > " + maxAllowed + ")");
        }

        InvItem item = null;
        if (_itemValue.HasQuality)
        {
          item = new InvItem(name, _count, _itemValue.Quality, _itemValue.MaxUseTimes, _itemValue.UseTimes);
        }
        else
        {
          item = new InvItem(name, _count, -1, _itemValue.MaxUseTimes, _itemValue.UseTimes);
        }

        item.icon = itemClass.GetIconName();

        item.iconcolor = BCUtils.ColorToHex(itemClass.GetIconTint());

        return item;
      }
      else
      {
        return null;
      }
    }
  }
}
