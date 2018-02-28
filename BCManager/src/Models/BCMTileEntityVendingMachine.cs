using System.Collections.Generic;

namespace BCM.Models
{
  public class BCMTileEntityVendingMachine : BCMTileEntityTrader
  {
    public string Owner;
    public List<string> Users;
    public bool HasPassword;
    public bool IsLocked;

    public BCMTileEntityVendingMachine(Vector3i pos, TileEntityVendingMachine te) : base(pos, te)
    {
      Owner = te.GetOwner();
      Users = te.GetUsers();
      HasPassword = te.HasPassword();
      IsLocked = te.IsLocked();
    }
  }
}