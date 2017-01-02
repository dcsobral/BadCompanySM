using BCM.PersistentData;
using System;
using System.Reflection;

namespace BCM
{
  public class API : ModApiAbstract
  {
    public API()
    {
      Config.LoadCommands();
      Config.ConfigureSystem();
      Heartbeat.Start();
    }

    public override void GameAwake()
    {
      StateManager.Awake();
    }

    public override void GameShutdown()
    {
      StateManager.Shutdown();
    }

    public override void SavePlayerData(ClientInfo _cInfo, PlayerDataFile _playerDataFile)
    {
      DataManager.SavePlayerData(_cInfo, _playerDataFile);
    }

    public override void PlayerLogin(ClientInfo _cInfo, string _compatibilityVersion)
    {
      //_cInfo.SendPackage(new NetPackageConsoleCmdClient("dm", true));
    }

    public override void PlayerSpawning(ClientInfo _cInfo, int _chunkViewDim, PlayerProfile _playerProfile)
    {
      try
      {
        PersistentContainer.Instance.Players[_cInfo.playerId, true].SetOnline(_cInfo);
        PersistentContainer.Instance.Save();
      }
      catch (Exception e)
      {
        Log.Out("" + Config.ModPrefix + " Error in " + GetType().Name + "." + MethodBase.GetCurrentMethod().Name + ": " + e);
      }
    }

    public override void PlayerDisconnected(ClientInfo _cInfo, bool _bShutdown)
    {
      try
      {
        Player p = PersistentContainer.Instance.Players[_cInfo.playerId, true];
        if (p != null)
        {
          p.SetOffline();
        }
        else
        {
          //Log.Out("" + Config.ModPrefix + " Disconnected player not found in client list...");
        }
        PersistentContainer.Instance.Save();
      }
      catch (Exception e)
      {
        Log.Out("" + Config.ModPrefix + " Error in " + GetType().Name + "." + MethodBase.GetCurrentMethod().Name + ": " + e);
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

    //public override void GameUpdate() {
    //}

  }
}
