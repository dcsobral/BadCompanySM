using System.Collections.Generic;

namespace BCM.Models
{
  public class BCMTileEntitySecureLootContainer : BCMTileEntityLootContainer
  {
    public string Owner;
    public List<string> Users;
    public bool HasPassword;
    public bool IsLocked;

    public BCMTileEntitySecureLootContainer(Vector3i pos, TileEntitySecureLootContainer te) : base(pos, te)
    {
      Owner = te.GetOwner();
      Users = te.GetUsers();
      HasPassword = te.HasPassword();
      IsLocked = te.IsLocked();
    }
  }
}