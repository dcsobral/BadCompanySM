namespace BCM.Models
{
  public class BCMTileEntity
  {
    public string Type;
    public BCMVector3 Pos;

    public BCMTileEntity(Vector3i pos, TileEntity te)
    {
      Type = te.GetTileEntityType().ToString();
      Pos = new BCMVector3(pos);
    }
  }
}
