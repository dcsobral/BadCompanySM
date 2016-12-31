using BCM.PersistentData;
using System;

namespace BCM
{
  class DataManager
  {
    public static void SavePlayerData(ClientInfo _cInfo, PlayerDataFile _playerDataFile)
    {
      try
      {
        PersistentContainer.Instance.Players[_cInfo.playerId, true].Update(_playerDataFile);
      }
      catch (Exception e)
      {
        Log.Out("(" + Config.ModPrefix + ") Error in DataManager.SavePlayerData: " + e);
      }
    }
  }
}
