using JetBrains.Annotations;

namespace BCM.Models
{
  public class BCMBan
  {
    [UsedImplicitly] public string SteamId;
    [UsedImplicitly] public string BannedUntil;
    [UsedImplicitly] public string BanReason;

    public BCMBan(AdminToolsClientInfo atci)
    {
      SteamId = atci.SteamID;
      BannedUntil = atci.BannedUntil.ToCultureInvariantString();
      BanReason = atci.BanReason;
    }
  }
}