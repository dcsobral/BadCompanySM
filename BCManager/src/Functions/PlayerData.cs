using BCM.PersistentData;

namespace BCM
{
  public struct PlayerInfo
  {
    public string SteamId;
    public EntityPlayer EP;
    public PlayerDataReader PDF;
    public Player PCP;
    public ClientInfo CI;
    public PersistentPlayerData PPD;
  }

  public static class PlayerData
  {
    public static PlayerInfo PlayerInfo(string steamId)
    {
      var world = GameManager.Instance.World;
      if (world == null) return new PlayerInfo();

      var playerInfo = new PlayerInfo
      {
        SteamId = steamId,
        PCP = PersistentContainer.Instance.Players[steamId, false],
        CI = ConnectionManager.Instance.GetClientInfoForPlayerId(steamId),
        PPD = GameManager.Instance.persistentPlayers.GetPlayerData(steamId),
        PDF = new PlayerDataReader(steamId),
        EP = null
      };


      if (playerInfo.CI != null && world.Entities.dict.ContainsKey(playerInfo.CI.entityId))
      {
        playerInfo.EP = world.Entities.dict[playerInfo.CI.entityId] as EntityPlayer;
      }

      return playerInfo;
    }
  }
}
