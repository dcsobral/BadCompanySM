namespace BCM.Models
{
  public class BCMPermission
  {
    public string Command;
    public int PermissionLevel;

    public BCMPermission(AdminToolsCommandPermissions atcp)
    {
      Command = atcp.Command;
      PermissionLevel = atcp.PermissionLevel;
    }
  }
}