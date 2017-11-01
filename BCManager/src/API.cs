using BCM.PersistentData;
using System;
using System.Reflection;

namespace BCM
{
  public class API : ModApiAbstract
  {
    private static readonly EntitySpawner EntitySpawner = new EntitySpawner();
    public static bool IsAlive;

    public API()
    {
      Config.Init();
      //if (Config.LogCache)
      //{
      //  LogCache.Instance.GetType();
      //}
      Heartbeat.Start();
    }

    public override void GameUpdate()
    {
      if (IsAlive)
      {
        EntitySpawner.ProcessSpawnQueue();
      }
    }

    public override void GameAwake()
    {
      StateManager.Awake();
      IsAlive = true;
    }

    public override void GameShutdown()
    {
      StateManager.Shutdown();
    }

    public override void SavePlayerData(ClientInfo cInfo, PlayerDataFile playerDataFile)
    {
      DataManager.SavePlayerData(cInfo, playerDataFile);
    }

    public override void PlayerLogin(ClientInfo cInfo, string compatibilityVersion)
    {

    }

    public override void PlayerSpawning(ClientInfo cInfo, int chunkViewDim, PlayerProfile playerProfile)
    {
      try
      {
        PersistentContainer.Instance.Players[cInfo.playerId, true]?.SetOnline(cInfo);
        PersistentContainer.Instance.Save();
      }
      catch (Exception e)
      {
        Log.Out($"{Config.ModPrefix} Error in {GetType().Name}.{MethodBase.GetCurrentMethod().Name}: {e}");
      }
    }

    public override void PlayerDisconnected(ClientInfo cInfo, bool bShutdown)
    {
      try
      {
        PersistentContainer.Instance.Players[cInfo.playerId, true]?.SetOffline(cInfo);
        PersistentContainer.Instance.Save();
      }
      catch (Exception e)
      {
        Log.Out($"{Config.ModPrefix} Error in {GetType().Name}.{MethodBase.GetCurrentMethod().Name}: {e}");
      }
    }

    //public override bool ChatMessage(ClientInfo _cInfo, EnumGameMessages _type, string _msg, string _mainName, bool _localizeMain, string _secondaryName, bool _localizeSecondary)
    //{
    //  return ChatHookExample.Hook(_cInfo, _type, _msg, _mainName);
    //}

    //public override void CalcChunkColorsDone(Chunk _chunk) {
    //}

    //public override void GameStartDone() {
    //}

    public override void PlayerSpawnedInWorld(ClientInfo cInfo, RespawnType respawnReason, Vector3i pos)
    {
      //_cInfo.SendPackage(new NetPackageConsoleCmdClient("dm", true));
      //Log.Out(Config.ModPrefix + " Player Spawned: " + _cInfo.entityId + " @" + _pos.x.ToString() + " " + _pos.y.ToString() + " " + _pos.z.ToString());
    }

  }
}
