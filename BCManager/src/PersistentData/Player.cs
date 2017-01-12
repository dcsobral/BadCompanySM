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
      playerData.GetData(steamId);
    }

    public void Update(PlayerDataFile _pdf)
    {
      playerData.bLoaded = _pdf.bLoaded;
      playerData.ecd = _pdf.ecd;
      playerData.inventory = _pdf.inventory;
      playerData.bag = _pdf.bag;
      playerData.equipment = _pdf.equipment;
      playerData.favoriteEquipment = _pdf.favoriteEquipment;
      playerData.selectedInventorySlot = _pdf.selectedInventorySlot;
      playerData.food = _pdf.food;
      playerData.drink = _pdf.drink;
      playerData.spawnPoints = _pdf.spawnPoints;
      playerData.selectedSpawnPointKey = _pdf.selectedSpawnPointKey;
      playerData.alreadyCraftedList = _pdf.alreadyCraftedList;
      playerData.unlockedRecipeList = _pdf.unlockedRecipeList;
      playerData.favoriteRecipeList = _pdf.favoriteRecipeList;
      playerData.lastSpawnPosition = _pdf.lastSpawnPosition;
      playerData.droppedBackpackPosition = _pdf.droppedBackpackPosition;
      playerData.playerKills = _pdf.playerKills;
      playerData.zombieKills = _pdf.zombieKills;
      playerData.deaths = _pdf.deaths;
      playerData.score = _pdf.score;
      playerData.id = _pdf.id;
      playerData.markerPosition = _pdf.markerPosition;
      playerData.experience = _pdf.experience;
      playerData.level = _pdf.level;
      playerData.skillPoints = _pdf.skillPoints;
      playerData.bCrouchedLocked = _pdf.bCrouchedLocked;
      playerData.craftingData = _pdf.craftingData;
      playerData.deathUpdateTime = _pdf.deathUpdateTime;
      playerData.bDead = _pdf.bDead;
      playerData.distanceWalked = _pdf.distanceWalked;
      playerData.totalItemsCrafted = _pdf.totalItemsCrafted;
      playerData.longestLife = _pdf.longestLife;
      playerData.currentLife = _pdf.currentLife;
      playerData.waypoints = _pdf.waypoints;
      playerData.questJournal = _pdf.questJournal;
      //playerData.IsModdedSave = _pdf.IsModdedSave;
      playerData.playerJournal = _pdf.playerJournal;
      playerData.rentedVMPosition = _pdf.rentedVMPosition;
      playerData.rentalEndTime = _pdf.rentalEndTime;
      playerData.trackedFriendEntityIds = _pdf.trackedFriendEntityIds;
      //playerData.skills = _pdf.skills;
    }

    public Player(string _steamId)
    {
      steamId = _steamId;
    }
  }
}
