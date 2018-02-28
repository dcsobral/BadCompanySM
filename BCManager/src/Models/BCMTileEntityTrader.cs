using System.Collections.Generic;
using System.Linq;

namespace BCM.Models
{
  public class BCMTileEntityTrader : BCMTileEntity
  {
    public int Money;
    public ulong ResetTime;
    public int TraderId;
    public bool IsOpen;
    public bool PlayerOwned;
    public List<BCMItemStack> Inventory;
    public List<List<BCMItemStack>> TierGroups;

    public BCMTileEntityTrader(Vector3i pos, TileEntityTrader te) : base(pos, te)
    {
      Money = te.TraderData.AvailableMoney;
      ResetTime = te.TraderData.NextResetTime;
      TraderId = te.TraderData.TraderID;
      IsOpen = te.TraderData.TraderInfo.IsOpen;
      PlayerOwned = te.TraderData.TraderInfo.PlayerOwned;
      Inventory = new List<BCMItemStack>();
      foreach (var itemStack in te.TraderData.PrimaryInventory)
      {
        Inventory.Add(new BCMItemStack(itemStack));
      }

      TierGroups = new List<List<BCMItemStack>>();
      foreach (var tierGroup in te.TraderData.TierItemGroups)
      {
        TierGroups.Add(tierGroup.Select(itemStack => new BCMItemStack(itemStack)).ToList());
      }
    }
  }
}