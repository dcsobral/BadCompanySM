using System.Collections.Generic;

namespace BCM.Models
{
  public class BCMTileEntitySecureDoor : BCMTileEntity
  {
    public string Owner;
    public List<string> Users;
    public bool HasPassword;
    public bool IsLocked;

    public BCMTileEntitySecureDoor(Vector3i pos, TileEntitySecureDoor te) : base(pos, te)
    {
      Owner = te.GetOwner();
      Users = te.GetUsers();
      HasPassword = te.HasPassword();
      IsLocked = te.IsLocked();
    }
  }
}