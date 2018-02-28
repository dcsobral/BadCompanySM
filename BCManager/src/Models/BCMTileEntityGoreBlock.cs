using System.Collections.Generic;

namespace BCM.Models
{
  public class BCMTileEntityGoreBlock : BCMTileEntity
  {
    public int LootList;
    public bool Touched;
    public ulong TimeTouched;
    public BCMVector2 Size;
    public double OpenTime;
    public List<BCMItemStack> Items;

    public BCMTileEntityGoreBlock(Vector3i pos, TileEntityGoreBlock te) : base(pos, te)
    {
      LootList = te.lootListIndex;
      Touched = te.bWasTouched;
      TimeTouched = te.worldTimeTouched;
      Size = new BCMVector2(te.GetContainerSize());
      OpenTime = te.GetOpenTime();

      Items = new List<BCMItemStack>();
      foreach (var itemStack in te.GetItems())
      {
        if (itemStack.itemValue.type == 0) continue;

        Items.Add(new BCMItemStack(itemStack));
      }
    }
  }
}