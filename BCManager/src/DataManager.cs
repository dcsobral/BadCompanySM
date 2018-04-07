using BCM.PersistentData;
using System;
using System.Reflection;

namespace BCM
{
  internal static class DataManager
  {
    public static void SavePlayerData(ClientInfo cInfo, PlayerDataFile playerDataFile)
    {
      try
      {
        PersistentContainer.Instance.Players[cInfo.playerId, true]?.Update(playerDataFile);
      }
      catch (Exception e)
      {
        Log.Out($"{Config.ModPrefix} Error in DataManager.{MethodBase.GetCurrentMethod().Name}: {e}");
      }
    }
  }
}
