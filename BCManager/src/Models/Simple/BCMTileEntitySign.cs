using System.Collections.Generic;
using JetBrains.Annotations;

namespace BCM.Models
{
  public class BCMTileEntitySign : BCMTileEntity
  {
    [UsedImplicitly] public string Owner;
    [UsedImplicitly] public List<string> Users;
    [UsedImplicitly] public bool HasPassword;
    [UsedImplicitly] public bool IsLocked;
    [UsedImplicitly] public string Text;

    public BCMTileEntitySign(Vector3i pos, [NotNull] TileEntitySign te) : base(pos, te)
    {
      Owner = te.GetOwner();
      Users = te.GetUsers();
      HasPassword = te.HasPassword();
      IsLocked = te.IsLocked();
      Text = te.GetText();
    }
  }
}