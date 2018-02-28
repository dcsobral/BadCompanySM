namespace BCM.Models
{
  public class BCMBan
  {
    public string SteamId;
    public string BannedUntil;
    public string BanReason;

    public BCMBan(AdminToolsClientInfo atci)
    {
      SteamId = atci.SteamID;
      BannedUntil = atci.BannedUntil.ToCultureInvariantString();
      BanReason = atci.BanReason;
    }
  }
}