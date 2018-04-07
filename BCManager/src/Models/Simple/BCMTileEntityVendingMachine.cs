using System.Collections.Generic;
using JetBrains.Annotations;

namespace BCM.Models
{
  public class BCMTileEntityVendingMachine : BCMTileEntityTrader
  {
    [UsedImplicitly] public string Owner;
    [UsedImplicitly] public List<string> Users;
    [UsedImplicitly] public bool HasPassword;
    [UsedImplicitly] public bool IsLocked;

    public BCMTileEntityVendingMachine(Vector3i pos, [NotNull] TileEntityVendingMachine te) : base(pos, te)
    {
      Owner = te.GetOwner();
      Users = te.GetUsers();
      HasPassword = te.HasPassword();
      IsLocked = te.IsLocked();
    }
  }
}