using System.Collections.Generic;

namespace BCM.Models
{
  public class BCMTileEntitySign : BCMTileEntity
  {
    public string Owner;
    public List<string> Users;
    public bool HasPassword;
    public bool IsLocked;
    public string Text;

    public BCMTileEntitySign(Vector3i pos, TileEntitySign te) : base(pos, te)
    {
      Owner = te.GetOwner();
      Users = te.GetUsers();
      HasPassword = te.HasPassword();
      IsLocked = te.IsLocked();
      Text = te.GetText();
    }
  }
}