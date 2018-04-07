using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace BCM.Models
{
  public class BCMTileEntityTrader : BCMTileEntity
  {
    [UsedImplicitly] public int Money;
    [UsedImplicitly] public ulong ResetTime;
    [UsedImplicitly] public int TraderId;
    [UsedImplicitly] public bool IsOpen;
    [UsedImplicitly] public bool PlayerOwned;
    [NotNull] [UsedImplicitly] public List<BCMItemStack> Inventory = new List<BCMItemStack>();
    [NotNull] [UsedImplicitly] public List<List<BCMItemStack>> TierGroups = new List<List<BCMItemStack>>();

    public BCMTileEntityTrader(Vector3i pos, TileEntityTrader te) : base(pos, te)
    {
      Money = te.TraderData.AvailableMoney;
      ResetTime = te.TraderData.NextResetTime;
      TraderId = te.TraderData.TraderID;
      IsOpen = te.TraderData.TraderInfo.IsOpen;
      PlayerOwned = te.TraderData.TraderInfo.PlayerOwned;

      foreach (var itemStack in te.TraderData.PrimaryInventory)
      {
        Inventory.Add(new BCMItemStack(itemStack));
      }

      foreach (var tierGroup in te.TraderData.TierItemGroups)
      {
        TierGroups.Add(tierGroup.Select(itemStack => new BCMItemStack(itemStack)).ToList());
      }
    }
  }
}