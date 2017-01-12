using BCM.Models;
using BCM.PersistentData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BCM
{
  public struct PlayerInfo
  {
    public string _steamId;
    public EntityPlayer EP;
    public PlayerDataReader PDF;
    //public PlayerDataFile PDF;
    public Player PCP;
    public ClientInfo CI;
    public PersistentPlayerData PPD;
  }

  public class GetPlayer
  {
    public PlayerInfo BySteamId(string _steamId)
    {
      PlayerInfo _pinfo = new PlayerInfo();
      _pinfo._steamId = _steamId;
      _pinfo.EP = null;
      //_pinfo.PDF = new PlayerDataFile();
      _pinfo.PDF = new PlayerDataReader();
      try
      {
        _pinfo.PDF.GetData(_steamId);
        //_pinfo.PDF.Load(GameUtils.GetPlayerDataDir(), _steamId);
      }
      catch
      {
        Log.Out(Config.ModPrefix + " Player Data not found for SteamId: " + _steamId);
      }
      _pinfo.PCP = PersistentContainer.Instance.Players[_steamId, false];
      _pinfo.CI = ConnectionManager.Instance.GetClientInfoForPlayerId(_steamId);
      if (_pinfo.CI != null)
      {
        _pinfo.EP = (EntityPlayer)GameManager.Instance.World.Entities.dict[_pinfo.CI.entityId];
      }
      _pinfo.PPD = GameManager.Instance.persistentPlayers.GetPlayerData(_steamId);
      return _pinfo;
    }
  }
}
