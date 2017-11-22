using BCM.Models;
using System;
using System.Runtime.Serialization;
using UnityEngine;

namespace BCM.PersistentData
{
  [Serializable]
  public class Player
  {
    private readonly string _steamId;

    //PRIVATE PROPERTIES
    private string _name;
    private string _ip;
    private long _totalPlayTime;
    private DateTime _lastOnline;
    private int _gamestage;
    [OptionalField]
    private BCMVector3 _lastPos;
    [OptionalField]
    private DateTime _lastUpdate;

    //CLIENT INFO DATA
    [NonSerialized] private ClientInfo _clientInfo;
    public bool IsOnline => _clientInfo != null;
    public string Name => _name ?? string.Empty;
    public string Ip => _ip ?? string.Empty;
    public long TotalPlayTime => IsOnline ? _totalPlayTime + (long)(DateTime.UtcNow - _lastOnline).TotalSeconds : _totalPlayTime;
    public DateTime LastOnline => IsOnline ? DateTime.UtcNow : _lastOnline;

    //ENTITY PLAYER DATA
    public int Gamestage => _gamestage;
    public BCMVector3 LastLogoutPos => _lastPos;
    public DateTime LastSaveUtc => _lastUpdate;

    //PLAYER DATA FILE DATA - Used for non saved cache of player file data for commands
    [NonSerialized] private PlayerDataReader _playerData;

    public PlayerDataReader DataCache => _playerData ?? (_playerData = new PlayerDataReader());

    //CONSTRUCTOR
    public Player(string steamId) => _steamId = steamId;

    //SET OFFLINE
    public void SetOffline(ClientInfo ci)
    {
      if (_clientInfo == null) return;

      _totalPlayTime += (long)(DateTime.UtcNow - _lastOnline).TotalSeconds;
      _lastOnline = DateTime.UtcNow;

      var players = GameManager.Instance.World.Players.dict;
      var ep = players.ContainsKey(ci.entityId) ? players[ci.entityId] : null;
      _gamestage = ep != null ? ep.gameStage : -1;
      _lastPos = new BCMVector3(ep != null ? ep.position : Vector3.zero);
      _clientInfo = null;
      _playerData = null;
    }

    //SET ONLINE
    public void SetOnline(ClientInfo ci)
    {
      _clientInfo = ci;
      _name = ci.playerName;
      _ip = ci.ip;
      _lastOnline = DateTime.UtcNow;
    }

    //UPDATE
    //Catches the properties from the player data file as it is saved and caches them for use in the player data commands
    public void Update(PlayerDataFile dataFile)
    {
      _lastUpdate = DateTime.UtcNow;

      DataCache.ecd = dataFile.ecd;
      DataCache.food = dataFile.food;
      DataCache.drink = dataFile.drink;
      DataCache.skillPoints = dataFile.skillPoints;
      DataCache.bag = dataFile.bag;
      DataCache.craftingData = dataFile.craftingData;
      DataCache.favoriteRecipeList = dataFile.favoriteRecipeList;
      DataCache.unlockedRecipeList = dataFile.unlockedRecipeList;
      DataCache.questJournal = dataFile.questJournal;
      DataCache.markerPosition = dataFile.markerPosition;
    }
  }
}
