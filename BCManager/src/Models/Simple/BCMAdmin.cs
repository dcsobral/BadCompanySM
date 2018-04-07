using JetBrains.Annotations;

namespace BCM.Models
{
  public class BCMAdmin
  {
    [UsedImplicitly] public string SteamId;
    [UsedImplicitly] public int PermissionLevel;
    [UsedImplicitly] public string PlayerName;

    public BCMAdmin(AdminToolsClientInfo atci, ClientInfo ci)
    {
      SteamId = atci.SteamID;
      PermissionLevel = atci.PermissionLevel;
      PlayerName = ci != null ? ci.playerName : "";
    }
  }
}