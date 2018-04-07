using System.Collections.Generic;
using JetBrains.Annotations;

namespace BCM.Models
{
  public class BCMTileEntityLootContainer : BCMTileEntity
  {
    [UsedImplicitly] public int LootList;
    [UsedImplicitly] public bool Touched;
    [UsedImplicitly] public ulong TimeTouched;
    [UsedImplicitly] public BCMVector2 Size;
    [UsedImplicitly] public double OpenTime;
    [NotNull] [UsedImplicitly] public List<BCMItemStack> Items = new List<BCMItemStack>();

    public BCMTileEntityLootContainer(Vector3i pos, [NotNull] TileEntityLootContainer te) : base(pos, te)
    {
      LootList = te.lootListIndex;
      Touched = te.bWasTouched;
      TimeTouched = te.worldTimeTouched;
      Size = new BCMVector2(te.GetContainerSize());
      OpenTime = te.GetOpenTime();

      foreach (var itemStack in te.GetItems())
      {
        if (itemStack.itemValue.type == 0) continue;

        Items.Add(new BCMItemStack(itemStack));
      }
    }
  }
}