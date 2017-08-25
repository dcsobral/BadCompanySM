using System;
using System.Runtime.Serialization;

namespace BCM.PersistentData
{
  [Serializable]
  public class Player
  {
    private readonly string steamId;
    private int entityId;
    private string name;
    private string ip;
    private long totalPlayTime;

    [OptionalField]
    private DateTime lastOnline;
    [OptionalField]
    private int gamestage;

    [NonSerialized]
    private ClientInfo clientInfo;
    [NonSerialized]
    private PlayerDataReader playerData;

    public string SteamID => steamId;

    public int EntityID => entityId;

    public string Name => name ?? string.Empty;

    public int Gamestage => gamestage;

    public string IP => ip ?? string.Empty;

    public bool IsOnline => clientInfo != null;

    public ClientInfo ClientInfo => clientInfo;

    public PlayerDataReader PlayerData => playerData ?? (playerData = new PlayerDataReader(steamId));

    public EntityPlayer Entity => !IsOnline ? null : GameManager.Instance.World.Players.dict[clientInfo.entityId];

    public long TotalPlayTime => IsOnline ? totalPlayTime + (long) (DateTime.UtcNow - lastOnline).TotalSeconds : totalPlayTime;

    public DateTime LastOnline => IsOnline ? DateTime.UtcNow : lastOnline;

    public bool LandProtectionActive => GameManager.Instance.World.IsLandProtectionValidForPlayer(GameManager.Instance.GetPersistentPlayerList().GetPlayerData(SteamID));

    public float LandProtectionMultiplier => GameManager.Instance.World.GetLandProtectionHardnessModifierForPlayer(GameManager.Instance.GetPersistentPlayerList().GetPlayerData(SteamID));

    public void SetOffline()
    {
      if (clientInfo == null) return;

      if (Entity != null)
      {
        gamestage = Entity.gameStage;
      }
      totalPlayTime += (long)(DateTime.UtcNow - lastOnline).TotalSeconds;
      lastOnline = DateTime.UtcNow;

      clientInfo = null;
      playerData = null;
    }

    public void SetOnline(ClientInfo ci)
    {
      clientInfo = ci;
      entityId = ci.entityId;
      name = ci.playerName;
      ip = ci.ip;
      lastOnline = DateTime.UtcNow;
      PlayerData.Init(steamId);
    }

    public void Update(PlayerDataFile pdf)
    {
      PlayerData.bLoaded = pdf.bLoaded;
      PlayerData.ecd = pdf.ecd;
      PlayerData.inventory = pdf.inventory;
      PlayerData.bag = pdf.bag;
      PlayerData.equipment = pdf.equipment;
      PlayerData.favoriteEquipment = pdf.favoriteEquipment;
      PlayerData.selectedInventorySlot = pdf.selectedInventorySlot;
      PlayerData.food = pdf.food;
      PlayerData.drink = pdf.drink;
      PlayerData.spawnPoints = pdf.spawnPoints;
      PlayerData.selectedSpawnPointKey = pdf.selectedSpawnPointKey;
      PlayerData.alreadyCraftedList = pdf.alreadyCraftedList;
      PlayerData.unlockedRecipeList = pdf.unlockedRecipeList;
      PlayerData.favoriteRecipeList = pdf.favoriteRecipeList;
      PlayerData.lastSpawnPosition = pdf.lastSpawnPosition;
      PlayerData.droppedBackpackPosition = pdf.droppedBackpackPosition;
      PlayerData.playerKills = pdf.playerKills;
      PlayerData.zombieKills = pdf.zombieKills;
      PlayerData.deaths = pdf.deaths;
      PlayerData.score = pdf.score;
      PlayerData.id = pdf.id;
      PlayerData.markerPosition = pdf.markerPosition;
      PlayerData.experience = pdf.experience;
      PlayerData.level = pdf.level;
      PlayerData.skillPoints = pdf.skillPoints;
      PlayerData.bCrouchedLocked = pdf.bCrouchedLocked;
      PlayerData.craftingData = pdf.craftingData;
      PlayerData.deathUpdateTime = pdf.deathUpdateTime;
      PlayerData.bDead = pdf.bDead;
      PlayerData.distanceWalked = pdf.distanceWalked;
      PlayerData.totalItemsCrafted = pdf.totalItemsCrafted;
      PlayerData.longestLife = pdf.longestLife;
      PlayerData.currentLife = pdf.currentLife;
      PlayerData.waypoints = pdf.waypoints;
      PlayerData.questJournal = pdf.questJournal;
      //PlayerData.IsModdedSave = _pdf.IsModdedSave;
      PlayerData.playerJournal = pdf.playerJournal;
      PlayerData.rentedVMPosition = pdf.rentedVMPosition;
      PlayerData.rentalEndTime = pdf.rentalEndTime;
      PlayerData.trackedFriendEntityIds = pdf.trackedFriendEntityIds;
      //PlayerData.skills = _pdf.skills;
    }

    public Player(string _steamId) => steamId = _steamId;
  }
}
