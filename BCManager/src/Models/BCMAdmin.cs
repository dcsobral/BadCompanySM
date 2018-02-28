namespace BCM.Models
{
  public class BCMAdmin
  {
    public string SteamId;
    public int PermissionLevel;
    public string PlayerName;

    public BCMAdmin(AdminToolsClientInfo atci, ClientInfo ci)
    {
      SteamId = atci.SteamID;
      PermissionLevel = atci.PermissionLevel;
      PlayerName = ci != null ? ci.playerName : "";
    }
  }
}