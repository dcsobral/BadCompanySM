using JetBrains.Annotations;

namespace BCM.Models
{
  public class BCMPermission
  {
    [UsedImplicitly] public string Command;
    [UsedImplicitly] public int PermissionLevel;

    public BCMPermission(AdminToolsCommandPermissions atcp)
    {
      Command = atcp.Command;
      PermissionLevel = atcp.PermissionLevel;
    }
  }
}