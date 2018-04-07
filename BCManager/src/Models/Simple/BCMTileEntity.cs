using JetBrains.Annotations;

namespace BCM.Models
{
  public class BCMTileEntity
  {
    [UsedImplicitly] public string Type;
    [UsedImplicitly] public BCMVector3 Pos;

    public BCMTileEntity(Vector3i pos, [NotNull] TileEntity te)
    {
      Type = te.GetTileEntityType().ToString();
      Pos = new BCMVector3(pos);
    }
  }
}
