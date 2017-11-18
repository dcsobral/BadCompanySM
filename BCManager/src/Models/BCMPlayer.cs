using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BCM.Models
{
  [Serializable]
  public class BCMPlayer : BCMAbstract
  {
    #region Filters
    public static class StrFilters
    {
      public const string SteamId = "steamid";
      public const string Name = "name";
      public const string EntityId = "entityid";
      public const string Ip = "ip";
      public const string Ping = "ping";
      public const string SessionPlayTime = "session";
      public const string TotalPlayTime = "playtime";
      public const string LastOnline = "online";
      public const string Underground = "underground";
      public const string Position = "position";
      public const string Rotation = "rotation";
      public const string Health = "health";
      public const string Stamina = "stamina";
      public const string Wellness = "wellness";
      public const string Food = "food";
      public const string Drink = "drink";
      public const string CoreTemp = "coretemp";
      public const string SpeedModifier = "speed";
      public const string LastZombieAttacked = "lastattack";
      public const string IsDead = "isdead";
      public const string OnGround = "onground";
      public const string IsStuck = "isstuck";
      public const string IsSafeZoneActive = "issafe";
      public const string Level = "level";
      public const string LevelProgress = "progress";
      public const string ExpToNextLevel = "tonext";
      public const string ExpForNextLevel = "fornext";
      public const string Gamestage = "gamestage";
      public const string Score = "score";
      public const string KilledPlayers = "pkill";
      public const string KilledZombies = "zkill";
      public const string Deaths = "deaths";
      public const string DistanceWalked = "walked";
      public const string ItemsCrafted = "crafted";
      public const string CurrentLife = "current";
      public const string LongestLife = "longest";
      public const string Archetype = "archetype";
      public const string DroppedPack = "pack";
      public const string RentedVendor = "vendor";
      public const string RentedVendorExpire = "vendorexpire";
      public const string Remote = "remote";
      public const string Bag = "bag";
      public const string Belt = "belt";
      public const string SelectedSlot = "selslot";
      public const string Equipment = "equip";
      public const string Buffs = "buffs";
      public const string SkillPoints = "skillpts";
      public const string Skills = "skills";
      public const string CraftingQueue = "crafting";
      public const string FavouriteRecipes = "favs";
      public const string UnlockedRecipes = "unlocks";
      public const string Quests = "quests";
      public const string Spawnpoints = "spawns";
      public const string Waypoints = "waypoints";
      public const string Marker = "marker";
      public const string Friends = "friends";
      public const string LPBlocks = "lpblocks";
    }

    private static readonly Dictionary<int, string> _filterMap = new Dictionary<int, string>
    {
      { 0,  StrFilters.SteamId },
      { 1,  StrFilters.Name },
      { 2,  StrFilters.EntityId },
      { 3,  StrFilters.Ip },
      { 4,  StrFilters.Ping },
      { 5,  StrFilters.SessionPlayTime },
      { 6,  StrFilters.TotalPlayTime },
      { 7,  StrFilters.LastOnline },
      { 8,  StrFilters.Underground },
      { 9,  StrFilters.Position },
      { 10,  StrFilters.Rotation },
      { 11,  StrFilters.Health },
      { 12,  StrFilters.Stamina },
      { 13,  StrFilters.Wellness },
      { 14,  StrFilters.Food },
      { 15,  StrFilters.Drink },
      { 16,  StrFilters.CoreTemp },
      { 17,  StrFilters.SpeedModifier },
      { 18,  StrFilters.LastZombieAttacked },
      { 19,  StrFilters.IsDead },
      { 20,  StrFilters.OnGround },
      { 21,  StrFilters.IsStuck },
      { 22,  StrFilters.IsSafeZoneActive },
      { 23,  StrFilters.Level },
      { 24,  StrFilters.LevelProgress },
      { 25,  StrFilters.ExpToNextLevel },
      { 26,  StrFilters.ExpForNextLevel },
      { 27,  StrFilters.Gamestage },
      { 28,  StrFilters.Score },
      { 29,  StrFilters.KilledPlayers },
      { 30,  StrFilters.KilledZombies },
      { 31,  StrFilters.Deaths },
      { 32,  StrFilters.DistanceWalked },
      { 33,  StrFilters.ItemsCrafted },
      { 34,  StrFilters.CurrentLife },
      { 35,  StrFilters.LongestLife },
      { 36,  StrFilters.Archetype },
      { 37,  StrFilters.DroppedPack },
      { 38,  StrFilters.RentedVendor },
      { 39,  StrFilters.RentedVendorExpire },
      { 40,  StrFilters.Remote },
      { 41,  StrFilters.Bag },
      { 42,  StrFilters.Belt },
      { 43,  StrFilters.SelectedSlot },
      { 44,  StrFilters.Equipment },
      { 45,  StrFilters.Buffs },
      { 46,  StrFilters.SkillPoints },
      { 47,  StrFilters.Skills },
      { 48,  StrFilters.CraftingQueue },
      { 49,  StrFilters.FavouriteRecipes },
      { 50,  StrFilters.UnlockedRecipes },
      { 51,  StrFilters.Quests },
      { 52,  StrFilters.Spawnpoints },
      { 53,  StrFilters.Waypoints },
      { 54,  StrFilters.Marker },
      { 55,  StrFilters.Friends },
      { 56,  StrFilters.LPBlocks }
    };
    public static Dictionary<int, string> FilterMap => _filterMap;
    #endregion

    #region Properties
    //CLIENTINFO
    public string SteamId;
    public string Name;
    public int EntityId;
    public string Ip;
    public string Ping;
    public double SessionPlayTime;
    public double TotalPlayTime;
    public string LastOnline;
    public int Underground;
    public BCMVector3 Position;
    public BCMVector3 Rotation;

    //STATS
    public int Health;
    public int Stamina;
    public int Wellness;
    public int Food;
    public int Drink;
    public int CoreTemp;
    public double SpeedModifier;
    public double? LastZombieAttacked;

    public bool IsDead;
    public bool? OnGround;
    public bool? IsStuck;
    public bool? IsSafeZoneActive;

    public int Level;
    public double LevelProgress;
    public int ExpToNextLevel;
    public int ExpForNextLevel;
    public int? Gamestage;

    public int Score;
    public int KilledPlayers;
    public int KilledZombies;
    public int Deaths;

    public double DistanceWalked;
    public uint ItemsCrafted;
    public double CurrentLife;
    public double LongestLife;

    public string Archetype;
    public BCMVector3 DroppedPack;
    public BCMVector3 RentedVendor;
    public ulong RentedVendorExpire;
    public bool? Remote;

    //BAG
    public Dictionary<string, BCMItemStack> Bag;

    //BELT
    public Dictionary<string, BCMItemStack> Belt;
    public int SelectedSlot;

    //EQUIPMENT
    public class BCMItemValue
    {
      public int Type;
      public int Quality;
      public int UseTimes;
      public int MaxUse;
      public int AmmoIndex;
      public string UISlot;

      public List<BCMAttachment> Attachments;
      public List<BCMParts> Parts;
    }
    public Dictionary<string, BCMItemValue> Equipment;

    //BUFFS
    public class BCMBuff
    {
      public string Id;
      public string Name;
      public bool Expired;
      public bool IsOverriden;
      public int InstigatorId;
      public double Duration;
      public double TimeFraction;
    }
    public List<BCMBuff> Buffs;

    //SKILLS
    public class BCMSkill
    {
      public string Name;
      public int Level;
      public double Percent;
    }
    public int SkillPoints;
    public List<BCMSkill> Skills;

    //CRAFTINGQUEUE
    public class BCMIngredient
    {
      public int Type;
      public int Count;
    }
    public class BCMCraftingQueue
    {
      public int Type;
      public string Name;
      public int Count;
      public double TotalTime;
      public double CraftTime;
      public List<BCMIngredient> Ingredients;
    }
    public List<BCMCraftingQueue> CraftingQueue;

    //RECIPES
    public List<string> FavouriteRecipes;
    public List<string> UnlockedRecipes;

    //QUESTS
    public class BCMQuest
    {
      public string Name;
      public string Id;
      public string CurrentState;
    }
    public List<BCMQuest> Quests;

    //SPAWNPOINTS+WAYPOINTS+MARKER
    public List<BCMVector3> Spawnpoints;
    public List<BCMWaypoint> Waypoints;
    public BCMVector3 Marker;
    public List<string> Friends = new List<string>();
    public List<object> LPBlocks = new List<object>();
    #endregion;

    public BCMPlayer(object obj, Dictionary<string, string> options, List<string> filters) : base(obj, "Entity", options, filters)
    {
    }

    public override void GetData(object obj)
    {
      if (obj == null) return;

      var pInfo = (PlayerInfo)obj;

      //all players must have PDF or EP to be valid
      if (pInfo.PDF == null && (pInfo.EP == null || pInfo.PCP == null)) return;

      if (IsOption("filter"))
      {
        foreach (var f in StrFilter)
        {
          switch (f)
          {
            case StrFilters.SteamId:
              GetSteamId(pInfo);
              break;
            case StrFilters.Name:
              GetName(pInfo);
              break;
            case StrFilters.EntityId:
              GetEntityId(pInfo);
              break;
            case StrFilters.Ip:
              GetIp(pInfo);
              break;
            case StrFilters.Ping:
              GetPing(pInfo);
              break;
            case StrFilters.TotalPlayTime:
              GetTotalPlaytime(pInfo);
              break;
            case StrFilters.SessionPlayTime:
              GetSessionPlaytime(pInfo);
              break;
            case StrFilters.LastOnline:
              GetLastOnline(pInfo);
              break;
            case StrFilters.Underground:
              GetUnderground(pInfo);
              break;
            case StrFilters.Position:
              GetPosition(pInfo);
              break;
            case StrFilters.Rotation:
              GetRotation(pInfo);
              break;
            case StrFilters.Wellness:
              GetWellness(pInfo);
              break;
            case StrFilters.Health:
              GetHealth(pInfo);
              break;
            case StrFilters.Stamina:
              GetStamina(pInfo);
              break;
            case StrFilters.Food:
              GetFood(pInfo);
              break;
            case StrFilters.Drink:
              GetDrink(pInfo);
              break;
            case StrFilters.CoreTemp:
              GetCoreTemp(pInfo);
              break;
            case StrFilters.SpeedModifier:
              GetSpeedModifier(pInfo);
              break;
            case StrFilters.Archetype:
              GetArchetype(pInfo);
              break;
            case StrFilters.DistanceWalked:
              GetDistanceWalked(pInfo);
              break;
            case StrFilters.DroppedPack:
              GetDroppedPack(pInfo);
              break;
            case StrFilters.Level:
              GetLevel(pInfo);
              break;
            case StrFilters.LevelProgress:
              GetLevelProgress(pInfo);
              break;
            case StrFilters.ExpToNextLevel:
              GetExpToNextLevel(pInfo);
              break;
            case StrFilters.ExpForNextLevel:
              GetExpForNextLevel(pInfo);
              break;
            case StrFilters.Gamestage:
              GetGamestage(pInfo);
              break;
            case StrFilters.Score:
              GetScore(pInfo);
              break;
            case StrFilters.KilledPlayers:
              GetKilledPlayers(pInfo);
              break;
            case StrFilters.KilledZombies:
              GetKilledZombies(pInfo);
              break;
            case StrFilters.Deaths:
              GetDeaths(pInfo);
              break;
            case StrFilters.CurrentLife:
              GetCurrentLife(pInfo);
              break;
            case StrFilters.LongestLife:
              GetLongestLife(pInfo);
              break;
            case StrFilters.ItemsCrafted:
              GetItemsCrafted(pInfo);
              break;
            case StrFilters.IsDead:
              GetIsDead(pInfo);
              break;
            case StrFilters.OnGround:
              GetOnGround(pInfo);
              break;
            case StrFilters.IsStuck:
              GetIsStuck(pInfo);
              break;
            case StrFilters.IsSafeZoneActive:
              GetIsSafeZoneActive(pInfo);
              break;
            case StrFilters.Remote:
              GetRemote(pInfo);
              break;
            case StrFilters.LastZombieAttacked:
              GetLastZombieAttacked(pInfo);
              break;
            case StrFilters.RentedVendor:
              GetRentedVendor(pInfo);
              break;
            case StrFilters.RentedVendorExpire:
              GetRentedVendorExpire(pInfo);
              break;
            case StrFilters.Bag:
              GetBag(pInfo);
              break;
            case StrFilters.Belt:
              GetBelt(pInfo);
              break;
            case StrFilters.Equipment:
              GetEquipment(pInfo);
              break;
            case StrFilters.Buffs:
              GetBuffs(pInfo);
              break;
            case StrFilters.SkillPoints:
              GetSkillPoints(pInfo);
              break;
            case StrFilters.Skills:
              GetSkills(pInfo);
              break;
            case StrFilters.CraftingQueue:
              GetCraftingQueue(pInfo);
              break;
            case StrFilters.FavouriteRecipes:
              GetFavouriteRecipes(pInfo);
              break;
            case StrFilters.UnlockedRecipes:
              GetUnlockedRecipes(pInfo);
              break;
            case StrFilters.Quests:
              GetQuests(pInfo);
              break;
            case StrFilters.Spawnpoints:
              GetSpawnpoints(pInfo);
              break;
            case StrFilters.Waypoints:
              GetWaypoints(pInfo);
              break;
            case StrFilters.Marker:
              GetMarker(pInfo);
              break;
            case StrFilters.Friends:
              GetFriends(pInfo);
              break;
            case StrFilters.LPBlocks:
              GetLpBlocks(pInfo);
              break;
          }
        }
      }
      else
      {
        GetSteamId(pInfo);
        GetName(pInfo);
        GetEntityId(pInfo);
        GetIp(pInfo);
        GetPing(pInfo);
        GetTotalPlaytime(pInfo);
        GetSessionPlaytime(pInfo);
        GetLastOnline(pInfo);
        GetUnderground(pInfo);
        GetPosition(pInfo);
        GetRotation(pInfo);
        GetWellness(pInfo);
        GetHealth(pInfo);
        GetStamina(pInfo);
        GetFood(pInfo);
        GetDrink(pInfo);
        GetCoreTemp(pInfo);
        GetSpeedModifier(pInfo);
        GetArchetype(pInfo);
        GetDistanceWalked(pInfo);
        GetDroppedPack(pInfo);
        GetLevel(pInfo);
        GetLevelProgress(pInfo);
        GetExpToNextLevel(pInfo);
        GetExpForNextLevel(pInfo);
        GetGamestage(pInfo);
        GetScore(pInfo);
        GetKilledPlayers(pInfo);
        GetKilledZombies(pInfo);
        GetDeaths(pInfo);
        GetCurrentLife(pInfo);
        GetLongestLife(pInfo);
        GetItemsCrafted(pInfo);
        GetIsDead(pInfo);
        GetOnGround(pInfo);
        GetIsStuck(pInfo);
        GetIsSafeZoneActive(pInfo);
        GetRemote(pInfo);
        GetLastZombieAttacked(pInfo);
        GetRentedVendor(pInfo);
        GetRentedVendorExpire(pInfo);

        if (!Options.ContainsKey("full")) return;
        //("bag bg belt bt equipment eq buffs bu skillpoints pt skills sk crafting cq favrecipes fr unlockedrecipes ur quests qu spawns sp waypoints wp")

        GetBag(pInfo);
        GetBelt(pInfo);
        GetEquipment(pInfo);
        GetBuffs(pInfo);
        GetSkillPoints(pInfo);
        GetSkills(pInfo);
        GetCraftingQueue(pInfo);
        GetFavouriteRecipes(pInfo);
        GetUnlockedRecipes(pInfo);
        GetQuests(pInfo);
        GetSpawnpoints(pInfo);
        GetWaypoints(pInfo);
        GetMarker(pInfo);
        GetFriends(pInfo);
        GetLpBlocks(pInfo);
      }
    }

    private void GetLpBlocks(PlayerInfo pInfo) => Bin.Add("LPBlocks", LPBlocks = pInfo.PPD?.LPBlocks?.Select(lpb => BCUtils.GetVectorObj(new BCMVector3(lpb), Options)).ToList());

    private void GetFriends(PlayerInfo pInfo) => Bin.Add("Friends", Friends = pInfo.PPD?.ACL?.ToList());

    private void GetRotation(PlayerInfo pInfo) => Bin.Add("Rotation", BCUtils.GetVectorObj(Rotation = new BCMVector3(pInfo.EP != null ? pInfo.EP.rotation : pInfo.PDF.ecd.rot), Options));

    private void GetPosition(PlayerInfo pInfo) => Bin.Add("Position", BCUtils.GetVectorObj(Position = new BCMVector3(pInfo.EP != null ? pInfo.EP.position : pInfo.PDF.ecd.pos), Options));

    private void GetUnderground(PlayerInfo pInfo) => Bin.Add("Underground", Underground = pInfo.EP != null ? (int)pInfo.EP.position.y - GameManager.Instance.World.GetHeight((int)pInfo.EP.position.x, (int)pInfo.EP.position.z) - 1 : 0);

    private void GetLastOnline(PlayerInfo pInfo) => Bin.Add("LastOnline", pInfo.EP == null ? LastOnline = pInfo.PCP != null ? pInfo.PCP.LastOnline.ToUtcStr() : "" : null);

    private void GetSessionPlaytime(PlayerInfo pInfo) => Bin.Add("SessionPlayTime", pInfo.EP != null ? SessionPlayTime = Math.Round((Time.timeSinceLevelLoad - pInfo.EP.CreationTimeSinceLevelLoad) / 60, 2) : 0);

    private void GetTotalPlaytime(PlayerInfo pInfo) => Bin.Add("TotalPlayTime", TotalPlayTime = Math.Round((pInfo.PCP != null ? pInfo.PCP.TotalPlayTime : 0) / 60f, 2));

    private void GetPing(PlayerInfo pInfo) => Bin.Add("Ping", Ping = pInfo.CI != null ? pInfo.CI.ping.ToString() : "Offline");

    private void GetIp(PlayerInfo pInfo) => Bin.Add("IP", Ip = pInfo.CI != null ? pInfo.CI.ip : pInfo.PCP != null ? pInfo.PCP.Ip : string.Empty);

    private void GetEntityId(PlayerInfo pInfo) => Bin.Add("EntityId", EntityId = pInfo.EP != null ? pInfo.EP.entityId : pInfo.PDF.id);

    private void GetName(PlayerInfo pInfo) => Bin.Add("Name", Name = pInfo.CI != null ? pInfo.CI.playerName : pInfo.PCP != null ? pInfo.PCP.Name : string.Empty);

    private void GetSteamId(PlayerInfo pInfo) => Bin.Add("SteamId", SteamId = pInfo.SteamId);

    //todo:EP not updating
    private void GetRentedVendorExpire(PlayerInfo pInfo) => Bin.Add("RentedVendorExpire", RentedVendorExpire = pInfo.EP != null ? pInfo.EP.RentalEndTime : pInfo.PDF.rentalEndTime);

    //todo:EP not updating
    private void GetRentedVendor(PlayerInfo pInfo) => Bin.Add("RentedVendor", BCUtils.GetVectorObj(RentedVendor = new BCMVector3(pInfo.EP != null ? pInfo.EP.RentedVMPosition : pInfo.PDF.rentedVMPosition), Options));

    private void GetLastZombieAttacked(PlayerInfo pInfo) => Bin.Add("LastZombieAttacked", pInfo.EP != null
        ? LastZombieAttacked = Math.Round((GameManager.Instance.World.worldTime - pInfo.EP.LastZombieAttackTime) / 600f, 2) : null);

    private void GetRemote(PlayerInfo pInfo) => Bin.Add("Remote", pInfo.EP != null ? Remote = pInfo.EP.isEntityRemote : null);

    private void GetIsSafeZoneActive(PlayerInfo pInfo) => Bin.Add("IsSafeZoneActive", pInfo.EP != null ? IsSafeZoneActive = pInfo.EP.IsSafeZoneActive() : null);

    private void GetIsStuck(PlayerInfo pInfo) => Bin.Add("IsStuck", pInfo.EP != null ? IsStuck = pInfo.EP.IsStuck : null);

    private void GetOnGround(PlayerInfo pInfo) => Bin.Add("OnGround", pInfo.EP != null ? OnGround = pInfo.EP.onGround : null);

    private void GetIsDead(PlayerInfo pInfo) => Bin.Add("IsDead", IsDead = pInfo.EP != null ? pInfo.EP.IsDead() : pInfo.PDF.bDead);

    private void GetItemsCrafted(PlayerInfo pInfo) => Bin.Add("ItemsCrafted",
      ItemsCrafted = pInfo.EP != null ? pInfo.EP.totalItemsCrafted : pInfo.PDF.totalItemsCrafted);

    private void GetLongestLife(PlayerInfo pInfo) => Bin.Add("LongestLife",
      LongestLife = Math.Round(pInfo.EP != null ? pInfo.EP.longestLife : pInfo.PDF.longestLife, 2));

    private void GetCurrentLife(PlayerInfo pInfo) => Bin.Add("CurrentLife",
      CurrentLife = Math.Round(pInfo.EP != null ? pInfo.EP.currentLife : pInfo.PDF.currentLife, 2));

    private void GetDeaths(PlayerInfo pInfo) => Bin.Add("Deaths", Deaths = pInfo.EP != null ? pInfo.EP.Died : pInfo.PDF.deaths);

    private void GetKilledZombies(PlayerInfo pInfo) => Bin.Add("KilledZombies", KilledZombies = pInfo.EP != null ? pInfo.EP.KilledZombies : pInfo.PDF.zombieKills);

    private void GetKilledPlayers(PlayerInfo pInfo) => Bin.Add("KilledPlayers", KilledPlayers = pInfo.EP != null ? pInfo.EP.KilledPlayers : pInfo.PDF.playerKills);

    private void GetScore(PlayerInfo pInfo) => Bin.Add("Score", Score = pInfo.EP != null ? pInfo.EP.Score : pInfo.PDF.score);

    private void GetGamestage(PlayerInfo pInfo) => Bin.Add("Gamestage",
      Gamestage = pInfo.EP != null ? pInfo.EP.gameStage : (pInfo.PCP != null ? (int?)pInfo.PCP.Gamestage : null));

    private void GetExpForNextLevel(PlayerInfo pInfo) => Bin.Add("ExpForNextLevel",
      ExpForNextLevel = pInfo.EP != null ? pInfo.EP.GetExpForNextLevel()
        : (int)Math.Min(Progression.BaseExpToLevel * Mathf.Pow(Progression.ExpMultiplier, pInfo.PDF.level + 1), int.MaxValue));

    private void GetExpToNextLevel(PlayerInfo pInfo) => Bin.Add("ExpToNextLevel",
      ExpToNextLevel = pInfo.EP != null ? pInfo.EP.ExpToNextLevel : (int)pInfo.PDF.experience);

    private void GetLevelProgress(PlayerInfo pInfo) => Bin.Add("LevelProgress",
      LevelProgress = Math.Round(pInfo.EP != null ? pInfo.EP.GetLevelProgressPercentage() * 100
        : (1 - pInfo.PDF.experience / Math.Min(Progression.BaseExpToLevel * Mathf.Pow(Progression.ExpMultiplier, pInfo.PDF.level + 1), int.MaxValue)) * 100, 2));

    private void GetLevel(PlayerInfo pInfo) => Bin.Add("Level", Level = pInfo.EP != null ? pInfo.EP.GetLevel() : pInfo.PDF.level);

    //todo: check this is updating while online
    private void GetDroppedPack(PlayerInfo pInfo) => Bin.Add("DroppedPack",
      BCUtils.GetVectorObj(DroppedPack = new BCMVector3(pInfo.EP != null ? pInfo.EP.GetDroppedBackpackPosition()
        : pInfo.PDF.droppedBackpackPosition), Options));

    private void GetDistanceWalked(PlayerInfo pInfo) => Bin.Add("DistanceWalked",
      DistanceWalked = Math.Round(pInfo.EP != null ? pInfo.EP.distanceWalked
        : pInfo.PDF.distanceWalked, 1));

    private void GetArchetype(PlayerInfo pInfo) => Bin.Add("Archetype",
      Archetype = pInfo.EP != null ? pInfo.EP.playerProfile.Archetype
        : pInfo.PDF.ecd.playerProfile.Archetype);

    private void GetEquipment(PlayerInfo pInfo)
    {
      Equipment = new Dictionary<string, BCMItemValue>();

      var equipment = pInfo.EP != null ? pInfo.EP.equipment.GetItems() : pInfo.PDF.equipment.GetItems();

      var k = 1;
      var slotidx = new[] { 16, 9, 10, 27, 19, 3, 28, 29, 5, 6 };
      foreach (var i in slotidx)
      {
        var item = equipment[i];
        BCMItemValue slot = null;
        if (item.type != 0)
        {
          slot = new BCMItemValue();
          if (item.ItemClass != null)
          {
            slot.UISlot = item.ItemClass.UmaSlotData.UISlot.ToString();
          }
          slot.Type = item.type;
          slot.Quality = item.Quality;
          slot.UseTimes = item.UseTimes;
          slot.MaxUse = item.MaxUseTimes;
          slot.AmmoIndex = item.SelectedAmmoTypeIndex;

          if (item.Attachments != null && item.Attachments.Length > 0)
          {
            slot.Attachments = new List<BCMAttachment>();
            foreach (var attachment in item.Attachments)
            {
              if (attachment != null && attachment.type != 0)
              {
                slot.Attachments.Add(new BCMAttachment
                {
                  Type = attachment.type,
                  Quality = attachment.Quality,
                  MaxUse = attachment.MaxUseTimes,
                  UseTimes = attachment.UseTimes
                });
              }
            }
          }

          if (item.Parts != null && item.Parts.Length > 0)
          {
            slot.Parts = new List<BCMParts>();
            foreach (var part in item.Parts)
            {
              if (part != null && part.type != 0)
              {
                slot.Parts.Add(new BCMParts
                {
                  Type = part.type,
                  Quality = part.Quality,
                  MaxUse = part.MaxUseTimes,
                  UseTimes = part.UseTimes
                });
              }
            }
          }
        }
        Equipment.Add(k.ToString(), slot);
        k++;
      }
      Bin.Add("Equipment", Equipment);
    }

    private void GetBelt(PlayerInfo pInfo)
    {
      Belt = new Dictionary<string, BCMItemStack>();

      ItemStack[] inv;
      if (pInfo.EP != null)
      {
        //only recognises changes once the slot is seleted
        inv = pInfo.EP.inventory.GetSlots();
        SelectedSlot = pInfo.EP.inventory.holdingItemIdx;
      }
      else
      {
        inv = pInfo.PDF.inventory;
        SelectedSlot = pInfo.PDF.selectedInventorySlot;
      }

      var j = 0;
      foreach (var item in inv)
      {
        BCMItemStack slot = null;
        if (item.itemValue.type != 0)
        {
          slot = new BCMItemStack(item);
        }
        Belt.Add(j.ToString(), slot);
        j++;
      }
      Bin.Add("Belt", Belt);
    }

    private void GetSkills(PlayerInfo pInfo)
    {
      Skills = new List<BCMSkill>();

      foreach (var skill in pInfo.EP != null ? pInfo.EP.Skills.GetAllSkills() : pInfo.PDF.skills)
      {
        var s = new BCMSkill();

        int l;
        try
        {
          l = skill.Level;
        }
        catch
        {
          l = 0;
        }
        s.Name = skill.Name;
        s.Level = l;
        s.Percent = Math.Round(skill.PercentThisLevel * 100, 1);

        Skills.Add(s);
      }
      Bin.Add("Skills", Skills);
    }

    private void GetSpawnpoints(PlayerInfo pInfo)
    {
      Spawnpoints = new List<BCMVector3>();

      if (pInfo.EP != null)
      {
        if (pInfo.EP.SpawnPoints != null)
        {
          foreach (var spawn in pInfo.EP.SpawnPoints.GetCopy())
          {
            Spawnpoints.Add(new BCMVector3(spawn));
          }
        }
      }
      else
      {
        foreach (var spawn in pInfo.PDF.spawnPoints)
        {
          Spawnpoints.Add(new BCMVector3(spawn));
        }

      }

      Bin.Add("Spawnpoints", Spawnpoints);
    }

    //get the updated value from PCP, EP stats doesnt always update on joining
    private void GetStamina(PlayerInfo pInfo) => Bin.Add("Stamina",
      Stamina = (int)(pInfo.EP != null ? (pInfo.EP.Stamina - 100 < 1f && pInfo.EP.Stamina - pInfo.PCP.DataCache.ecd.stats.Stamina.Value > 1f
      ? pInfo.PCP.DataCache.ecd.stats.Stamina.Value
      : pInfo.EP.Stamina)
      : pInfo.PDF.ecd.stats.Stamina.Value));

    //get the updated value from PCP, EP stats doesnt always update on joining
    private void GetHealth(PlayerInfo pInfo) => Bin.Add("Health",
      Health = (int)(pInfo.EP != null
      ? (pInfo.EP.Health == 100 && pInfo.EP.Health != (int)pInfo.PCP.DataCache.ecd.stats.Health.Value
      ? pInfo.PCP.DataCache.ecd.stats.Health.Value
      : pInfo.EP.Health)
      : pInfo.PDF.ecd.stats.Health.Value));

    //get the updated value from PCP, EP stats doesnt always update on joining
    private void GetWellness(PlayerInfo pInfo) => Bin.Add("Wellness",
    Wellness = (int)(pInfo.EP != null
    ? (pInfo.EP.Stats.Wellness.Value - 100 < 1f && pInfo.EP.Stats.Wellness.Value - pInfo.PCP.DataCache.ecd.stats.Wellness.Value > 1
    ? pInfo.PCP.DataCache.ecd.stats.Wellness.Value
    : pInfo.EP.Stats.Wellness.Value)
    : pInfo.PDF.ecd.stats.Wellness.Value));

    //get the updated value from PCP, EP stats doesnt always update on joining
    private void GetSpeedModifier(PlayerInfo pInfo) => Bin.Add("SpeedModifier",
      SpeedModifier = Math.Round(pInfo.EP != null
        ? (pInfo.EP.Stats.SpeedModifier.Value - 1 < 0.01 && pInfo.EP.Stats.SpeedModifier.Value - pInfo.PCP.DataCache.ecd.stats.SpeedModifier.Value > 0.01
        ? pInfo.PCP.DataCache.ecd.stats.SpeedModifier.Value
        : pInfo.EP.Stats.SpeedModifier.Value)
        : pInfo.PDF.ecd.stats.SpeedModifier.Value,
        1));

    //get the updated value from PCP, values on server not updating
    private void GetCoreTemp(PlayerInfo pInfo) => Bin.Add("CoreTemp", CoreTemp = (int)(pInfo.EP != null ? pInfo.PCP.DataCache.ecd.stats.CoreTemp.Value : pInfo.PDF.ecd.stats.CoreTemp.Value));

    //get the updated value from PCP, values on server not updating
    private void GetDrink(PlayerInfo pInfo) => Bin.Add("Drink", Drink = (int)((pInfo.EP != null ? pInfo.PCP.DataCache.drink.GetLifeLevelFraction() : pInfo.PDF.drink.GetLifeLevelFraction()) * 100));

    //get the updated value from PCP, values on server not updating
    private void GetFood(PlayerInfo pInfo) => Bin.Add("Food", Food = (int)((pInfo.EP != null ? pInfo.PCP.DataCache.food.GetLifeLevelFraction() : pInfo.PDF.food.GetLifeLevelFraction()) * 100));

    //get the updated value from PCP, values on server not updating
    private void GetWaypoints(PlayerInfo pInfo)
    {
      Waypoints = new List<BCMWaypoint>();
      //todo: broken for EP
      foreach (var waypoint in pInfo.EP != null ? pInfo.PCP.DataCache.waypoints.List : pInfo.PDF.waypoints.List)
      {
        Waypoints.Add(new BCMWaypoint
        {
          Name = waypoint.name,
          Pos = new BCMVector3(waypoint.pos),
          Icon = waypoint.icon
        });
      }
      Bin.Add("Waypoints", Waypoints);
    }

    //use PCP for online players
    private void GetMarker(PlayerInfo pInfo)
    {
      if (pInfo.PDF != null)
      {
        Bin.Add("Marker", Marker = new BCMVector3(pInfo.EP != null ? pInfo.PCP.DataCache.markerPosition : pInfo.PDF.markerPosition));
      }
    }

    //use PCP for online players
    private void GetQuests(PlayerInfo pInfo)
    {
      Quests = new List<BCMQuest>();

      foreach (var quest in pInfo.EP != null ? pInfo.PCP.DataCache.questJournal.Clone().quests : pInfo.PDF.questJournal.Clone().quests)
      {
        var q = new BCMQuest();
        if (QuestClass.s_Quests.ContainsKey(quest.ID))
        {
          var qc = QuestClass.s_Quests[quest.ID];
          q.Name = qc.Name;
          q.Id = qc.ID;
          q.CurrentState = quest.CurrentState.ToString();

        }
        else
        {
          q.Name = null;
        }

        Quests.Add(q);
      }
      Bin.Add("Quests", Quests);
    }

    //use PCP for online players
    private void GetUnlockedRecipes(PlayerInfo pInfo)
    {
      UnlockedRecipes = new List<string>();

      foreach (var name in pInfo.EP != null ? pInfo.PCP.DataCache.unlockedRecipeList : pInfo.PDF.unlockedRecipeList)
      {
        UnlockedRecipes.Add(name);
      }
      Bin.Add("UnlockedRecipes", UnlockedRecipes);
    }

    //use PCP for online players
    private void GetFavouriteRecipes(PlayerInfo pInfo)
    {
      FavouriteRecipes = new List<string>();

      foreach (var name in pInfo.EP != null ? pInfo.PCP.DataCache.favoriteRecipeList : pInfo.PDF.favoriteRecipeList)
      {
        FavouriteRecipes.Add(name);
      }
      Bin.Add("FavouriteRecipes", FavouriteRecipes);
    }

    //use PCP for online players
    private void GetCraftingQueue(PlayerInfo pInfo)
    {
      CraftingQueue = new List<BCMCraftingQueue>();

      foreach (var rqi in pInfo.EP != null ? pInfo.PCP.DataCache.craftingData.RecipeQueueItems : pInfo.PDF.craftingData.RecipeQueueItems)
      {
        var queueItem = new BCMCraftingQueue();

        if (rqi.Recipe != null)
        {
          queueItem.Type = rqi.Recipe.itemValueType;
          queueItem.Name = rqi.Recipe.GetName();
          queueItem.Count = rqi.Multiplier;
          if (rqi.IsCrafting)
          {
            queueItem.TotalTime = Math.Round(rqi.Recipe.craftingTime * (rqi.Multiplier - 1) + rqi.CraftingTimeLeft, 1);
            queueItem.CraftTime = Math.Round(rqi.CraftingTimeLeft, 1);
          }
          else
          {
            queueItem.TotalTime = Math.Round(rqi.Recipe.craftingTime * rqi.Multiplier, 1);
            queueItem.CraftTime = Math.Round(rqi.Recipe.craftingTime, 1);
          }


          queueItem.Ingredients = new List<BCMIngredient>();

          foreach (var ingredient in rqi.Recipe.GetIngredientsSummedUp())
          {
            var i = new BCMIngredient
            {
              Type = ingredient.itemValue.type,
              Count = ingredient.count
            };

            queueItem.Ingredients.Add(i);
          }
        }
        CraftingQueue.Add(queueItem);
      }
      Bin.Add("CraftingQueue", CraftingQueue);
    }

    //use PCP for online players
    private void GetSkillPoints(PlayerInfo pInfo) => Bin.Add("SkillPoints", SkillPoints = pInfo.EP != null ? pInfo.PCP.DataCache.skillPoints : pInfo.PDF.skillPoints);

    //use PCP for online players
    private void GetBuffs(PlayerInfo pInfo)
    {
      Buffs = new List<BCMBuff>();
      var multiBuffs = new Dictionary<string, MultiBuff>();
      if (pInfo.EP != null)
      {
        foreach (var buff in pInfo.EP.Stats.Buffs)
        {
          var multiBuff = buff as MultiBuff;
          if (multiBuff?.MultiBuffClass.Id == null) continue;

          if (!multiBuffs.ContainsKey(multiBuff.MultiBuffClass.Id))
          {
            multiBuffs.Add(multiBuff.MultiBuffClass.Id, buff as MultiBuff);
          }
        }
      }
      foreach (var buff in pInfo.EP != null ? pInfo.PCP.DataCache.ecd.stats.Buffs : pInfo.PDF.ecd.stats.Buffs)
      {
        var multiBuff = buff as MultiBuff;
        if (multiBuff?.MultiBuffClass.Id == null) continue;

        if (!multiBuffs.ContainsKey(multiBuff.MultiBuffClass.Id))
        {
          multiBuffs.Add(multiBuff.MultiBuffClass.Id, buff as MultiBuff);
        }
      }

      foreach (var multiBuff in multiBuffs.Values)
      {
        Buffs.Add(new BCMBuff
        {
          Id = multiBuff.MultiBuffClass.Id,
          Name = multiBuff.Name,
          Expired = multiBuff.Expired,
          IsOverriden = multiBuff.IsOverriden,
          InstigatorId = multiBuff.InstigatorId,
          Duration = multiBuff.MultiBuffClass.FDuration,
          TimeFraction = multiBuff.Timer.TimeFraction
        });
      }
      Bin.Add("Buffs", Buffs);
    }

    //use PCP for online players
    private void GetBag(PlayerInfo pInfo)
    {
      // Updates are instantly triggered when looting, but not when an item is moved to equipment so there is a delay of up to 30 seconds
      Bag = new Dictionary<string, BCMItemStack>();

      var i = 1;
      foreach (var item in pInfo.EP != null ? pInfo.PCP.DataCache.bag : pInfo.PDF.bag)
      {
        BCMItemStack slot = null;
        if (item.itemValue.type != 0)
        {
          slot = new BCMItemStack(item);
        }
        Bag.Add(i.ToString(), slot);
        i++;
      }
      Bin.Add("Bag", Bag);
    }
  }
}
