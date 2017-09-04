using System;
using System.Runtime.Serialization;

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

    //CLIENT INFO DATA
    [NonSerialized] private ClientInfo _clientInfo;
    public bool IsOnline => _clientInfo != null;
    public string Name => _name ?? string.Empty;
    public string Ip => _ip ?? string.Empty;
    public long TotalPlayTime => IsOnline ? _totalPlayTime + (long)(DateTime.UtcNow - _lastOnline).TotalSeconds : _totalPlayTime;
    public DateTime LastOnline => IsOnline ? DateTime.UtcNow : _lastOnline;

    //ENTITY PLAYER DATA
    public int Gamestage => _gamestage;

    //PLAYER DATA FILE DATA - Used for non saved cache of player file data for commands
    [NonSerialized] private PlayerDataReader _playerData;
    public PlayerDataReader DataCache => _playerData ?? (_playerData = new PlayerDataReader());

    //CONSTRUCTOR
    public Player(string steamId) => _steamId = steamId;

    //SET OFFLINE
    public void SetOffline(EntityPlayer ep)
    {
      if (_clientInfo == null) return;

      _totalPlayTime += (long)(DateTime.UtcNow - _lastOnline).TotalSeconds;
      _lastOnline = DateTime.UtcNow;
       _gamestage = ep != null ? ep.gameStage : -1;
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
