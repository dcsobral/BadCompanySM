using System.Collections.Generic;
using JetBrains.Annotations;

namespace BCM.Models
{
  public class BCMTileEntitySecureDoor : BCMTileEntity
  {
    [UsedImplicitly] public string Owner;
    [UsedImplicitly] public List<string> Users;
    [UsedImplicitly] public bool HasPassword;
    [UsedImplicitly] public bool IsLocked;

    public BCMTileEntitySecureDoor(Vector3i pos, [NotNull] TileEntitySecureDoor te) : base(pos, te)
    {
      Owner = te.GetOwner();
      Users = te.GetUsers();
      HasPassword = te.HasPassword();
      IsLocked = te.IsLocked();
    }
  }
}