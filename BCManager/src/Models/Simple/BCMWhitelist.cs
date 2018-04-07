using JetBrains.Annotations;

namespace BCM.Models
{
  public class BCMWhitelist
  {
    [UsedImplicitly] public string SteamId;
    [UsedImplicitly] public string PlayerName;

    public BCMWhitelist(AdminToolsClientInfo atci, [CanBeNull] ClientInfo ci)
    {
      SteamId = atci.SteamID;
      PlayerName = ci != null ? ci.playerName : "";
    }
  }
}