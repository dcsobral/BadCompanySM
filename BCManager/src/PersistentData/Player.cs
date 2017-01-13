using System;
using System.Runtime.Serialization;
using UnityEngine;

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

    public string SteamID
    {
      get { return steamId; }
    }

    public int EntityID
    {
      get { return entityId; }
    }

    public string Name
    {
      get { return name == null ? string.Empty : name; }
    }

    public int Gamestage
    {
      get { return gamestage; }
    }

    public string IP
    {
      get { return ip == null ? string.Empty : ip; }
    }

    public bool IsOnline
    {
      get { return clientInfo != null; }
    }

    public ClientInfo ClientInfo
    {
      get { return clientInfo; }
    }

    public PlayerDataReader PlayerData
    {
      get
      {
        if (playerData == null)
          playerData = new PlayerDataReader();
        return playerData;
      }
    }

    public EntityPlayer Entity
    {
      get
      {
        if (IsOnline)
        {
          return GameManager.Instance.World.Players.dict[clientInfo.entityId];
        }
        else
        {
          return null;
        }
      }
    }

    public long TotalPlayTime
    {
      get
      {
        if (IsOnline)
        {
          return totalPlayTime + (long)(DateTime.Now - lastOnline).TotalSeconds;
        }
        else
        {
          return totalPlayTime;
        }
      }
    }

    public DateTime LastOnline
    {
      get
      {
        if (IsOnline)
          return DateTime.Now;
        else
          return lastOnline;
      }
    }

    public bool LandProtectionActive
    {
      get
      {
        return GameManager.Instance.World.IsLandProtectionValidForPlayer(GameManager.Instance.GetPersistentPlayerList().GetPlayerData(SteamID));
      }
    }

    public float LandProtectionMultiplier
    {
      get
      {
        return GameManager.Instance.World.GetLandProtectionHardnessModifierForPlayer(GameManager.Instance.GetPersistentPlayerList().GetPlayerData(SteamID));
      }
    }

    public void SetOffline()
    {
      if (clientInfo != null)
      {
        totalPlayTime += (long)(DateTime.Now - lastOnline).TotalSeconds;
        //(long)(Time.timeSinceLevelLoad - Entity.CreationTimeSinceLevelLoad);
        lastOnline = DateTime.Now;

        clientInfo = null;
        playerData = null;
      }
    }

    public void SetOnline(ClientInfo ci)
    {
      clientInfo = ci;
      entityId = ci.entityId;
      name = ci.playerName;
      ip = ci.ip;
      lastOnline = DateTime.Now;
      if (Entity != null)
      {
        gamestage = Entity.gameStage;
      }
      PlayerData.GetData(steamId);
    }

    public void Update(PlayerDataFile _pdf)
    {
      PlayerData.bLoaded = _pdf.bLoaded;
      PlayerData.ecd = _pdf.ecd;
      PlayerData.inventory = _pdf.inventory;
      PlayerData.bag = _pdf.bag;
      PlayerData.equipment = _pdf.equipment;
      PlayerData.favoriteEquipment = _pdf.favoriteEquipment;
      PlayerData.selectedInventorySlot = _pdf.selectedInventorySlot;
      PlayerData.food = _pdf.food;
      PlayerData.drink = _pdf.drink;
      PlayerData.spawnPoints = _pdf.spawnPoints;
      PlayerData.selectedSpawnPointKey = _pdf.selectedSpawnPointKey;
      PlayerData.alreadyCraftedList = _pdf.alreadyCraftedList;
      PlayerData.unlockedRecipeList = _pdf.unlockedRecipeList;
      PlayerData.favoriteRecipeList = _pdf.favoriteRecipeList;
      PlayerData.lastSpawnPosition = _pdf.lastSpawnPosition;
      PlayerData.droppedBackpackPosition = _pdf.droppedBackpackPosition;
      PlayerData.playerKills = _pdf.playerKills;
      PlayerData.zombieKills = _pdf.zombieKills;
      PlayerData.deaths = _pdf.deaths;
      PlayerData.score = _pdf.score;
      PlayerData.id = _pdf.id;
      PlayerData.markerPosition = _pdf.markerPosition;
      PlayerData.experience = _pdf.experience;
      PlayerData.level = _pdf.level;
      PlayerData.skillPoints = _pdf.skillPoints;
      PlayerData.bCrouchedLocked = _pdf.bCrouchedLocked;
      PlayerData.craftingData = _pdf.craftingData;
      PlayerData.deathUpdateTime = _pdf.deathUpdateTime;
      PlayerData.bDead = _pdf.bDead;
      PlayerData.distanceWalked = _pdf.distanceWalked;
      PlayerData.totalItemsCrafted = _pdf.totalItemsCrafted;
      PlayerData.longestLife = _pdf.longestLife;
      PlayerData.currentLife = _pdf.currentLife;
      PlayerData.waypoints = _pdf.waypoints;
      PlayerData.questJournal = _pdf.questJournal;
      //PlayerData.IsModdedSave = _pdf.IsModdedSave;
      PlayerData.playerJournal = _pdf.playerJournal;
      PlayerData.rentedVMPosition = _pdf.rentedVMPosition;
      PlayerData.rentalEndTime = _pdf.rentalEndTime;
      PlayerData.trackedFriendEntityIds = _pdf.trackedFriendEntityIds;
      //PlayerData.skills = _pdf.skills;
    }

    public Player(string _steamId)
    {
      steamId = _steamId;
    }
  }
}
