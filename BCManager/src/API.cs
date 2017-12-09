using BCM.PersistentData;
using System;
using System.Reflection;

namespace BCM
{
  public class API : ModApiAbstract
  {
    private static readonly EntitySpawner EntitySpawner = new EntitySpawner();
    public static bool IsAwake;

    private static void OnWorldChanged(World _world)
    {
      if (_world == null) return;

      Log.Out($"{Config.ModPrefix} World State Changed: Loading Reactive Neurons");
      IsAwake = true;
      StateManager.WorldAlive();
    }

    public API()
    {
      StateManager.Init();
      Config.Init();
      Heartbeat.Start();
      GameManager.Instance.OnWorldChanged += OnWorldChanged;
    }

    public override void GameUpdate()
    {
      if (IsAwake) EntitySpawner.ProcessSpawnQueue();
    }

    public override void GameAwake()
    {
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
        //Player Data Cache
        PersistentContainer.Instance.Players[cInfo.playerId, true]?.SetOffline(cInfo);

        //Log disconnects explictly
        Synapse.PlayerTracker(cInfo, RespawnType.Unknown, false);
      }
      catch (Exception e)
      {
        Log.Out($"{Config.ModPrefix} Error in {GetType().Name}.{MethodBase.GetCurrentMethod().Name}: {e}");
      }
    }

    //public override bool ChatMessage(ClientInfo _cInfo, EnumGameMessages _type, string _msg, string _mainName, bool _localizeMain, string _secondaryName, bool _localizeSecondary) { }

    //public override void CalcChunkColorsDone(Chunk _chunk) { }

    //public override void GameStartDone() { }

    public override void PlayerSpawnedInWorld(ClientInfo cInfo, RespawnType respawnReason, Vector3i pos)
    {
      try
      {
        //Log spawns explictly
        Synapse.PlayerTracker(cInfo, respawnReason);

        //check for respawn events
        switch (respawnReason)
        {
          case RespawnType.Died:
            Synapse.DeadIsDead(cInfo);
            break;

          case RespawnType.Teleport:
            Synapse.PlayerTeleported(cInfo, pos);
            break;

          case RespawnType.EnterMultiplayer:
            Synapse.NewPlayer(cInfo, pos);
            break;

          case RespawnType.JoinMultiplayer:
            Synapse.ReturnPlayer(cInfo, pos);
            break;
        }
      }
      catch (Exception e)
      {
        Log.Out($"{Config.ModPrefix} Error in {GetType().Name}.{MethodBase.GetCurrentMethod().Name}: {e}");
      }
    }
  }
}
