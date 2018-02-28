namespace BCM.Models
{
  public class BCMWhitelist
  {
    public string SteamId;
    public string PlayerName;

    public BCMWhitelist(AdminToolsClientInfo atci, ClientInfo ci)
    {
      SteamId = atci.SteamID;
      PlayerName = ci != null ? ci.playerName : "";
    }
  }
}