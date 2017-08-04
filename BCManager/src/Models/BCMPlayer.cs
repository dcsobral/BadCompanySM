using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BCM.Models
{
  [Serializable]
  public class BCMPlayer// : AbstractList
  {
    private Dictionary<string, string> options = new Dictionary<string, string>();
    private Dictionary<string, object> _bin = new Dictionary<string, object>();
    public enum Filters
    {
      SteamId,
      Name,
      EntityId,
      IP,
      Ping,
      SessionPlayTime,
      TotalPlayTime,
      LastOnline,
      Underground,
      Position,
      Rotation,
      Health,
      Stamina,
      Wellness,
      Food,
      Drink,
      CoreTemp,
      SpeedModifier,
      LastZombieAttacked,
      IsDead,
      OnGround,
      IsStuck,
      IsSafeZoneActive,
      Level,
      LevelProgress,
      ExpToNextLevel,
      ExpForNextLevel,
      Gamestage,
      Score,
      KilledPlayers,
      KilledZombies,
      Deaths,
      DistanceWalked,
      ItemsCrafted,
      CurrentLife,
      LongestLife,
      Archetype,
      DroppedPack,
      RentedVendor,
      RentedVendorExpire,
      Remote,
      Bag,
      Belt,
      SelectedSlot,
      Equipment,
      Buffs,
      SkillPoints,
      Skills,
      CraftingQueue,
      FavouriteRecipes,
      UnlockedRecipes,
      Quests,
      Spawnpoints,
      Waypoints,
      Marker
    }
    public static class StrFilters
    {
      public static string SteamId = "steamid";
      public static string Name = "name";
      public static string EntityId = "entityid";
      public static string IP = "ip";
      public static string Ping = "ping";
      public static string SessionPlayTime = "session";
      public static string TotalPlayTime = "playtime";
      public static string LastOnline = "online";
      public static string Underground = "underground";
      public static string Position = "position";
      public static string Rotation = "rotation";
      public static string Health = "health";
      public static string Stamina = "stamina";
      public static string Wellness = "wellness";
      public static string Food = "food";
      public static string Drink = "drink";
      public static string CoreTemp = "coretemp";
      public static string SpeedModifier = "speed";
      public static string LastZombieAttacked = "lastattack";
      public static string IsDead = "isdead";
      public static string OnGround = "onground";
      public static string IsStuck = "isstuck";
      public static string IsSafeZoneActive = "issafe";
      public static string Level = "level";
      public static string LevelProgress = "progress";
      public static string ExpToNextLevel = "tonext";
      public static string ExpForNextLevel = "fornext";
      public static string Gamestage = "gamestage";
      public static string Score = "score";
      public static string KilledPlayers = "pkill";
      public static string KilledZombies = "zkill";
      public static string Deaths = "deaths";
      public static string DistanceWalked = "walked";
      public static string ItemsCrafted = "crafted";
      public static string CurrentLife = "current";
      public static string LongestLife = "longest";
      public static string Archetype = "archetype";
      public static string DroppedPack = "pack";
      public static string RentedVendor = "vendor";
      public static string RentedVendorExpire = "vendorexpire";
      public static string Remote = "remote";
      public static string Bag = "bag";
      public static string Belt = "belt";
      public static string SelectedSlot = "selslot";
      public static string Equipment = "equip";
      public static string Buffs = "buffs";
      public static string SkillPoints = "skillpts";
      public static string Skills = "skills";
      public static string CraftingQueue = "crafting";
      public static string FavouriteRecipes = "favs";
      public static string UnlockedRecipes = "unlocks";
      public static string Quests = "quests";
      public static string Spawnpoints = "spawns";
      public static string Waypoints = "waypoints";
      public static string Marker = "marker";
    }

    private List<string> _filter = new List<string>();

    #region Properties
    //CLIENTINFO
    public string SteamId;
    public string Name;
    public int EntityId;
    public string IP;
    public string Ping;
    public double SessionPlayTime;
    public double TotalPlayTime;
    public string LastOnline;
    public int Underground;
    public BCMVector3i Position;// todo: add options for doubles to 2dp
    public BCMVector3i Rotation;

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
    public string DroppedPack; //vector3i
    public string RentedVendor; //vector3i
    public ulong RentedVendorExpire;
    public bool? Remote;

    //BAG
    public class BCMParts
    {
      public int Type;
      public int Quality;
      public int UseTimes;
      public int MaxUse;
    }
    public class BCMAttachment
    {
      public int Type;
      public int Quality;
      public int UseTimes;
      public int MaxUse;
    }
    public class BCMItemStack
    {
      public int Type;
      public int Quality;
      public int UseTimes;
      public int MaxUse;
      public int AmmoIndex;
      public int Count;

      public List<BCMAttachment> Attachments;
      public List<BCMParts> Parts;
    }
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
    public class BCMVector3i
    {
      public int x;
      public int y;
      public int z;
      public BCMVector3i()
      {
        this.x = 0;
        this.y = 0;
        this.z = 0;
      }
      public BCMVector3i(Vector3 v)
      {
        this.x = Mathf.RoundToInt(v.x);
        this.y = Mathf.RoundToInt(v.y);
        this.z = Mathf.RoundToInt(v.z);
      }
      public BCMVector3i(Vector3i v)
      {
        this.x = v.x;
        this.y = v.y;
        this.z = v.z;
      }
    }
    public class BCMWaypoint
    {
      public string Name;
      public string Icon;
      public BCMVector3i Pos;
    }
    public List<BCMVector3i> Spawnpoints;
    public List<BCMWaypoint> Waypoints;
    public BCMVector3i Marker;// todo: add options for format (vector3i, vector3, string3i, string3) - doubles to 2dp
    #endregion;


    private bool useInt = false;
    private List<string> strFilter = new List<string>();
    private List<int> intFilter = new List<int>();


    //public BCMPlayer() : base()
    //{
    //}

    public BCMPlayer(PlayerInfo _pInfo, Dictionary<string, string> _options)// : base(_pInfo, _options)
    {
      options = _options;
      Load(_pInfo);
    }

    public Dictionary<string, object> Data ()
    {
      return _bin;
    }

    public void Load(PlayerInfo _pInfo)
    {
      if (isOption("filter"))
      {
        strFilter = OptionValue("filter").Split(',').ToList();
        if (strFilter.Count > 0)
        {
          intFilter = (from o in strFilter.Where((o) => { int d; return int.TryParse(o, out d); }) select int.Parse(o)).ToList();
          if (intFilter.Count == strFilter.Count)
          {
            useInt = true;
          }
        }
      }

      GetClientInfo(_pInfo);
      GetStats(_pInfo);

      //todo: sub filters, e.g. /bag=1,3,4,5

      if ((useInt && intFilter.Contains((int)Filters.Bag)) || (!useInt && strFilter.Contains(StrFilters.Bag)) || isOption("bag bg full"))
      {
        GetBag(_pInfo);
        _bin.Add("Bag", Bag);
      }
      if ((useInt && intFilter.Contains((int)Filters.Belt)) || (!useInt && strFilter.Contains(StrFilters.Belt)) || isOption("belt bt full"))
      {
        GetBelt(_pInfo);
        _bin.Add("Belt", Belt);
      }
      if ((useInt && intFilter.Contains((int)Filters.Equipment)) || (!useInt && strFilter.Contains(StrFilters.Equipment)) || isOption("equipment eq full"))
      {
        GetEquipment(_pInfo);
        _bin.Add("Equipment", Equipment);
      }
      if ((useInt && intFilter.Contains((int)Filters.Buffs)) || (!useInt && strFilter.Contains(StrFilters.Buffs)) || isOption("buffs bu full"))
      {
        GetBuffs(_pInfo);
        _bin.Add("Buffs", Buffs);
      }

      if (
        ((useInt && intFilter.Contains((int)Filters.SkillPoints)) || (!useInt && strFilter.Contains(StrFilters.SkillPoints)) || isOption("skillpoints pt full"))
        ||
        ((useInt && intFilter.Contains((int)Filters.Skills)) || (!useInt && strFilter.Contains(StrFilters.Skills)) || isOption("skills sk full"))
        )
      {
        GetSkills(_pInfo);
        if ((useInt && intFilter.Contains((int)Filters.SkillPoints)) || (!useInt && strFilter.Contains(StrFilters.SkillPoints)) || isOption("skillpoints pt full"))
        {
          _bin.Add("SkillPoints", SkillPoints);
        }
        if ((useInt && intFilter.Contains((int)Filters.Skills)) || (!useInt && strFilter.Contains(StrFilters.Skills)) || isOption("skills sk full"))
        {
          _bin.Add("Skills", Skills);
        }
      }

      if ((useInt && intFilter.Contains((int)Filters.CraftingQueue)) || (!useInt && strFilter.Contains(StrFilters.CraftingQueue)) || isOption("crafting cq full"))
      {
        GetCraftingQueue(_pInfo);
        _bin.Add("CraftingQueue", CraftingQueue);
      }
      if ((useInt && intFilter.Contains((int)Filters.FavouriteRecipes)) || (!useInt && strFilter.Contains(StrFilters.FavouriteRecipes)) || isOption("favrecipes fr full"))
      {
        GetFavouriteRecipes(_pInfo);
        _bin.Add("FavouriteRecipes", FavouriteRecipes);
      }
      if ((useInt && intFilter.Contains((int)Filters.UnlockedRecipes)) || (!useInt && strFilter.Contains(StrFilters.UnlockedRecipes)) || isOption("unlockedrecipes ur full"))
      {
        GetUnlockedRecipes(_pInfo);
        _bin.Add("UnlockedRecipes", UnlockedRecipes);
      }
      if ((useInt && intFilter.Contains((int)Filters.Quests)) || (!useInt && strFilter.Contains(StrFilters.Quests)) || isOption("quests qu full"))
      {
        GetQuests(_pInfo);
        _bin.Add("Quests", Quests);
      }
      if ((useInt && intFilter.Contains((int)Filters.Spawnpoints)) || (!useInt && strFilter.Contains(StrFilters.Spawnpoints)) || isOption("spawns sp full"))
      {
        GetSpawnpoints(_pInfo);
        _bin.Add("Spawnpoints", Spawnpoints);
      }
      if ((useInt && intFilter.Contains((int)Filters.Waypoints)) || (!useInt && strFilter.Contains(StrFilters.Waypoints)) || isOption("waypoints wp full"))
      {
        GetWaypoints(_pInfo);
        _bin.Add("Waypoints", Waypoints);
        _bin.Add("Marker", Marker);
      }
    }

    private void GetWaypoints(PlayerInfo _pInfo)
    {
      Marker = new BCMVector3i(_pInfo.PDF.markerPosition);

      Waypoints = new List<BCMWaypoint>();

      foreach (Waypoint waypoint in _pInfo.PDF.waypoints.List)
      {
        var _waypoint = new BCMWaypoint();
        _waypoint.Name = waypoint.name;
        //todo:BCMVector3i
        _waypoint.Pos = new BCMVector3i(waypoint.pos);
        _waypoint.Icon = waypoint.icon;

        Waypoints.Add(_waypoint);
      }
    }

    private void GetSpawnpoints(PlayerInfo _pInfo)
    {
      Spawnpoints = new List<BCMVector3i>();

      foreach (Vector3i spawn in _pInfo.PDF.spawnPoints)
      {
        var _spawn = new BCMVector3i(spawn);

        Spawnpoints.Add(_spawn);
      }
    }

    private void GetQuests(PlayerInfo _pInfo)
    {
      Quests = new List<BCMQuest>();

      foreach (Quest quest in _pInfo.PDF.questJournal.Clone().quests)
      {
        var _quest = new BCMQuest();
        if (QuestClass.s_Quests.ContainsKey(quest.ID))
        {
          var qc = QuestClass.s_Quests[quest.ID];
          _quest.Name = qc.Name;
          _quest.Id = qc.ID;
          _quest.CurrentState = quest.CurrentState.ToString();

        }
        else
        {
          _quest.Name = null;
        }

        Quests.Add(_quest);
      }
    }

    private void GetUnlockedRecipes(PlayerInfo _pInfo)
    {
      UnlockedRecipes = new List<string>();

      foreach (string name in _pInfo.PDF.unlockedRecipeList)
      {
        UnlockedRecipes.Add(name);
      }
    }

    private void GetFavouriteRecipes(PlayerInfo _pInfo)
    {
      FavouriteRecipes = new List<string>();

      foreach (string name in _pInfo.PDF.favoriteRecipeList)
      {
        FavouriteRecipes.Add(name);
      }
    }

    private void GetCraftingQueue(PlayerInfo _pInfo)
    {
      CraftingQueue = new List<BCMCraftingQueue>();

      foreach (RecipeQueueItem rqi in _pInfo.PDF.craftingData.RecipeQueueItems)
      {
        var queueItem = new BCMCraftingQueue();

        if (rqi.Recipe != null)
        {
          queueItem.Type = rqi.Recipe.itemValueType;
          queueItem.Name = rqi.Recipe.GetName();
          queueItem.Count = rqi.Multiplier;
          if (rqi.IsCrafting)
          {
            queueItem.TotalTime = Math.Round((rqi.Recipe.craftingTime * (rqi.Multiplier - 1) + rqi.CraftingTimeLeft), 1);
            queueItem.CraftTime = Math.Round(rqi.CraftingTimeLeft, 1);
          } else
          {
            queueItem.TotalTime = Math.Round((rqi.Recipe.craftingTime * rqi.Multiplier), 1);
            queueItem.CraftTime = Math.Round(rqi.Recipe.craftingTime, 1);
          }


          queueItem.Ingredients = new List<BCMIngredient>();

          foreach (ItemStack ingredient in rqi.Recipe.GetIngredientsSummedUp())
          {
            var _ingredient = new BCMIngredient();
            _ingredient.Type = ingredient.itemValue.type;
            _ingredient.Count = ingredient.count;

            queueItem.Ingredients.Add(_ingredient);
          }
        }
        CraftingQueue.Add(queueItem);
      }
    }

    private void GetSkills(PlayerInfo _pInfo)
    {
      SkillPoints = _pInfo.PDF.skillPoints;
      Skills = new List<BCMSkill>();

      var allSkills = new List<Skill>();
      if (_pInfo.EP != null)
      {
        allSkills = _pInfo.EP.Skills.GetAllSkills();
      }
      else
      {
        allSkills = _pInfo.PDF.skills;
      }

      foreach (Skill skill in allSkills)
      {
        var _skill = new BCMSkill();

        int l;
        try
        {
          l = skill.Level;
        }
        catch
        {
          l = 0;
        }
        _skill.Name = skill.Name;
        _skill.Level = l;
        _skill.Percent = Math.Round(skill.PercentThisLevel * 100, 1);

        Skills.Add(_skill);
      }
    }

    private void GetBuffs(PlayerInfo _pInfo)
    {
      Buffs = new List<BCMBuff>();
      Dictionary<string, MultiBuff> multiBuffs = new Dictionary<string, MultiBuff>();
      if (_pInfo.EP != null)
      {
        foreach (MultiBuff multiBuff in _pInfo.EP.Stats.Buffs)
        {
          if (multiBuff != null && multiBuff.MultiBuffClass.Id != null)
          {
            if (!multiBuffs.ContainsKey(multiBuff.MultiBuffClass.Id))
            {
              multiBuffs.Add(multiBuff.MultiBuffClass.Id, multiBuff);
            }
          }
        }
      }
      foreach (MultiBuff multiBuff in _pInfo.PDF.ecd.stats.Buffs)
      {
        if (multiBuff != null && multiBuff.MultiBuffClass.Id != null)
        {
          if (!multiBuffs.ContainsKey(multiBuff.MultiBuffClass.Id))
          {
            multiBuffs.Add(multiBuff.MultiBuffClass.Id, multiBuff);
          }
        }
      }

      foreach (KeyValuePair<string, MultiBuff> multiBuff in multiBuffs)
      {
        var buff = new BCMBuff();
        buff.Id = multiBuff.Value.MultiBuffClass.Id;
        buff.Name = multiBuff.Value.Name;
        buff.Expired = multiBuff.Value.Expired;
        buff.IsOverriden = multiBuff.Value.IsOverriden;
        buff.InstigatorId = multiBuff.Value.InstigatorId;
        buff.Duration = multiBuff.Value.MultiBuffClass.FDuration;
        buff.TimeFraction = multiBuff.Value.Timer.TimeFraction;

        Buffs.Add(buff);
      }
    }

    private void GetEquipment(PlayerInfo _pInfo)
    {
      Equipment = new Dictionary<string, BCMItemValue>();

      ItemValue[] equipment;
      if (_pInfo.EP != null)
      {
        equipment = _pInfo.EP.equipment.GetItems();
      }
      else
      {
        equipment = _pInfo.PDF.equipment.GetItems();
      }

      int k = 1;
      foreach (ItemValue item in equipment)
      {
        BCMItemValue slot = null;
        if (item.type != 0)
        {
          slot = new BCMItemValue();
          if (item.ItemClass != null)
          {
            slot.UISlot = item.ItemClass.UmaSlotData.UISlot.ToString();// item.ItemClass.EquipSlot.ToString();          
          }
          slot.Type = item.type;
          slot.Quality = item.Quality;
          slot.UseTimes = item.UseTimes;
          slot.MaxUse = item.MaxUseTimes;
          slot.AmmoIndex = item.SelectedAmmoTypeIndex;

          if (item.Attachments != null && item.Attachments.Length > 0)
          {
            slot.Attachments = new List<BCMAttachment>();
            foreach (ItemValue attachment in item.Attachments)
            {
              if (attachment != null && attachment.type != 0)
              {
                var _attachment = new BCMAttachment();
                _attachment.Type = attachment.type;
                _attachment.Quality = attachment.Quality;
                _attachment.MaxUse = attachment.MaxUseTimes;
                _attachment.UseTimes = attachment.UseTimes;

                slot.Attachments.Add(_attachment);
              }
            }
          }

          if (item.Parts != null && item.Parts.Length > 0)
          {
            slot.Parts = new List<BCMParts>();
            foreach (ItemValue part in item.Parts)
            {
              if (part != null && part.type != 0)
              {
                var _part = new BCMParts();
                _part.Type = part.type;
                _part.Quality = part.Quality;
                _part.MaxUse = part.MaxUseTimes;
                _part.UseTimes = part.UseTimes;
                slot.Parts.Add(_part);
              }
            }
          }
        }
        Equipment.Add(k.ToString(), slot);
        k++;
      }
    }

    private void GetBelt(PlayerInfo _pInfo)
    {
      Belt = new Dictionary<string, BCMItemStack>();

      ItemStack[] inv;
      if (_pInfo.EP != null)
      {
        inv = _pInfo.EP.inventory.GetSlots();
        SelectedSlot = _pInfo.EP.inventory.holdingItemIdx;
      }
      else
      {
        inv = _pInfo.PDF.inventory;
        SelectedSlot = _pInfo.PDF.selectedInventorySlot;
      }

      int j = 0;
      foreach (ItemStack item in inv)
      {
        BCMItemStack slot = null;
        if (item.itemValue.type != 0)
        {
          slot = new BCMItemStack();
          slot.Type = item.itemValue.type;
          slot.Quality = item.itemValue.Quality;
          slot.UseTimes = item.itemValue.UseTimes;
          slot.MaxUse = item.itemValue.MaxUseTimes;
          slot.AmmoIndex = item.itemValue.SelectedAmmoTypeIndex;

          if (item.itemValue.Attachments != null && item.itemValue.Attachments.Length > 0)
          {
            slot.Attachments = new List<BCMAttachment>();
            foreach (ItemValue attachment in item.itemValue.Attachments)
            {
              if (attachment != null && attachment.type != 0)
              {
                var _attachment = new BCMAttachment();
                _attachment.Type = attachment.type;
                _attachment.Quality = attachment.Quality;
                _attachment.MaxUse = attachment.MaxUseTimes;
                _attachment.UseTimes = attachment.UseTimes;

                slot.Attachments.Add(_attachment);
              }
            }
          }

          if (item.itemValue.Parts != null && item.itemValue.Parts.Length > 0)
          {
            slot.Parts = new List<BCMParts>();
            foreach (ItemValue part in item.itemValue.Parts)
            {
              if (part != null && part.type != 0)
              {
                var _part = new BCMParts();
                _part.Type = part.type;
                _part.Quality = part.Quality;
                _part.MaxUse = part.MaxUseTimes;
                _part.UseTimes = part.UseTimes;
                slot.Parts.Add(_part);
              }
            }
          }

          slot.Count = item.count;
        }
        Belt.Add(j.ToString(), slot);
        j++;
      }
    }

    private void GetBag(PlayerInfo _pInfo)
    {
      // Updates are instantly triggered when looting, but not when an item is moved to equipment so there is a delay of up to 30 seconds
      Bag = new Dictionary<string, BCMItemStack>();
      int i = 1;
      foreach (ItemStack item in _pInfo.PDF.bag)
      {
        BCMItemStack slot = null;
        if (item.itemValue.type != 0)
        {
          slot = new BCMItemStack();
          slot.Type = item.itemValue.type;
          slot.Quality = item.itemValue.Quality;
          slot.UseTimes = item.itemValue.UseTimes;
          slot.MaxUse = item.itemValue.MaxUseTimes;
          slot.AmmoIndex = item.itemValue.SelectedAmmoTypeIndex;

          if (item.itemValue.Attachments != null && item.itemValue.Attachments.Length > 0)
          {
            slot.Attachments = new List<BCMAttachment>();
            foreach (ItemValue attachment in item.itemValue.Attachments)
            {
              if (attachment != null && attachment.type != 0)
              {
                var _attachment = new BCMAttachment();
                _attachment.Type = attachment.type;
                _attachment.Quality = attachment.Quality;
                _attachment.MaxUse = attachment.MaxUseTimes;
                _attachment.UseTimes = attachment.UseTimes;

                slot.Attachments.Add(_attachment);
              }
            }
          }

          if (item.itemValue.Parts != null && item.itemValue.Parts.Length > 0)
          {
            slot.Parts = new List<BCMParts>();
            foreach (ItemValue part in item.itemValue.Parts)
            {
              if (part != null && part.type != 0)
              {
                var _part = new BCMParts();
                _part.Type = part.type;
                _part.Quality = part.Quality;
                _part.MaxUse = part.MaxUseTimes;
                _part.UseTimes = part.UseTimes;
                slot.Parts.Add(_part);
              }
            }
          }

          slot.Count = item.count;
        }
        Bag.Add(i.ToString(), slot);
        i++;
      }
    }

    private void GetStats(PlayerInfo _pInfo)
    {
      if (isOption("filter"))
      {
        //WELLNESS
        if ((useInt && intFilter.Contains((int)Filters.Wellness)) || (!useInt && strFilter.Contains(StrFilters.Wellness)))
        {
          Wellness = (int)(_pInfo.EP != null ? ((_pInfo.EP.Stats.Wellness.Value == 100 && _pInfo.EP.Stats.Wellness.Value != _pInfo.PDF.ecd.stats.Wellness.Value) ? _pInfo.PDF.ecd.stats.Wellness.Value : _pInfo.EP.Stats.Wellness.Value) : _pInfo.PDF.ecd.stats.Wellness.Value);
          _bin.Add("Wellness", Wellness);
        }

        //HEALTH
        if ((useInt && intFilter.Contains((int)Filters.Health)) || (!useInt && strFilter.Contains(StrFilters.Health)))
        {
          Health = (int)(_pInfo.EP != null ? ((_pInfo.EP.Health == 100 && _pInfo.EP.Health != _pInfo.PDF.ecd.stats.Health.Value) ? _pInfo.PDF.ecd.stats.Health.Value : _pInfo.EP.Health) : _pInfo.PDF.ecd.stats.Health.Value);
          _bin.Add("Health", Health);
        }

        //STAMINA
        if ((useInt && intFilter.Contains((int)Filters.Stamina)) || (!useInt && strFilter.Contains(StrFilters.Stamina)))
        {
          Stamina = (int)(_pInfo.EP != null ? ((_pInfo.EP.Stamina == 100 && _pInfo.EP.Stamina != _pInfo.PDF.ecd.stats.Stamina.Value) ? _pInfo.PDF.ecd.stats.Stamina.Value : _pInfo.EP.Stamina) : _pInfo.PDF.ecd.stats.Stamina.Value);
        _bin.Add("Stamina", Stamina);
        }

        //FOOD
        if ((useInt && intFilter.Contains((int)Filters.Food)) || (!useInt && strFilter.Contains(StrFilters.Food)))
        {
          Food = (int)(_pInfo.PDF.food.GetLifeLevelFraction() * 100);
        _bin.Add("Food", Food);
        }

        //DRINK
        if ((useInt && intFilter.Contains((int)Filters.Drink)) || (!useInt && strFilter.Contains(StrFilters.Drink)))
        {
          Drink = (int)(_pInfo.PDF.drink.GetLifeLevelFraction() * 100);
        _bin.Add("Drink", Drink);
        }

        //CORETEMP
        if ((useInt && intFilter.Contains((int)Filters.CoreTemp)) || (!useInt && strFilter.Contains(StrFilters.CoreTemp)))
        {
          CoreTemp = (int)_pInfo.PDF.ecd.stats.CoreTemp.Value;
        _bin.Add("CoreTemp", CoreTemp);
        }

        //SPEEDMODIFIER
        if ((useInt && intFilter.Contains((int)Filters.SpeedModifier)) || (!useInt && strFilter.Contains(StrFilters.SpeedModifier)))
        {
          SpeedModifier = Math.Round((_pInfo.EP != null ? ((_pInfo.EP.Stats.SpeedModifier.Value == 1 && _pInfo.EP.Stats.SpeedModifier.Value != _pInfo.PDF.ecd.stats.SpeedModifier.Value) ? _pInfo.PDF.ecd.stats.SpeedModifier.Value : _pInfo.EP.Stats.SpeedModifier.Value) : _pInfo.PDF.ecd.stats.SpeedModifier.Value), 1);
        _bin.Add("SpeedModifier", SpeedModifier);
        }

        //ARCHETYPE
        if ((useInt && intFilter.Contains((int)Filters.Archetype)) || (!useInt && strFilter.Contains(StrFilters.Archetype)))
        {
          Archetype = _pInfo.PDF.ecd.playerProfile.Archetype;
        _bin.Add("Archetype", Archetype);
        }

        //DISTANCEWALKED
        if ((useInt && intFilter.Contains((int)Filters.DistanceWalked)) || (!useInt && strFilter.Contains(StrFilters.DistanceWalked)))
        {
          DistanceWalked = Math.Round(_pInfo.PDF.distanceWalked, 1);
        _bin.Add("DistanceWalked", DistanceWalked);
        }

        //DROPPEDPACK
        if ((useInt && intFilter.Contains((int)Filters.DroppedPack)) || (!useInt && strFilter.Contains(StrFilters.DroppedPack)))
        {
          //todo:BCMVector3i
          DroppedPack = (_pInfo.PDF.droppedBackpackPosition != Vector3i.zero ? Convert.PosToStr(_pInfo.PDF.droppedBackpackPosition, GetPosType()) : "None");
        _bin.Add("DroppedPack", DroppedPack);
        }

        //LEVEL
        if ((useInt && intFilter.Contains((int)Filters.Level)) || (!useInt && strFilter.Contains(StrFilters.Level)))
        {
          Level = (_pInfo.EP != null ? _pInfo.EP.GetLevel() : _pInfo.PDF.level);
        _bin.Add("Level", Level);
        }

        //LEVELPROGRESS
        if ((useInt && intFilter.Contains((int)Filters.LevelProgress)) || (!useInt && strFilter.Contains(StrFilters.LevelProgress)))
        {
          LevelProgress = Math.Round((_pInfo.EP != null ? (_pInfo.EP.GetLevelProgressPercentage() * 100) : ((1 - _pInfo.PDF.experience / Math.Min((Progression.BaseExpToLevel * Mathf.Pow(Progression.ExpMultiplier, _pInfo.PDF.level + 1)), int.MaxValue)) * 100)), 2);
        _bin.Add("LevelProgress", LevelProgress);
        }

        //EXPTONEXTLEVEL
        if ((useInt && intFilter.Contains((int)Filters.ExpToNextLevel)) || (!useInt && strFilter.Contains(StrFilters.ExpToNextLevel)))
        {
          ExpToNextLevel = (_pInfo.EP != null ? _pInfo.EP.ExpToNextLevel : (int)_pInfo.PDF.experience);
        _bin.Add("ExpToNextLevel", ExpToNextLevel);
        }

        //EXPFORNEXTLEVEL
        if ((useInt && intFilter.Contains((int)Filters.ExpForNextLevel)) || (!useInt && strFilter.Contains(StrFilters.ExpForNextLevel)))
        {
          ExpForNextLevel = (_pInfo.EP != null ? _pInfo.EP.GetExpForNextLevel() : (int)Math.Min((Progression.BaseExpToLevel * Mathf.Pow(Progression.ExpMultiplier, _pInfo.PDF.level + 1)), int.MaxValue));
        _bin.Add("ExpForNextLevel", ExpForNextLevel);
        }

        //GAMESTAGE
        if ((useInt && intFilter.Contains((int)Filters.Gamestage)) || (!useInt && strFilter.Contains(StrFilters.Gamestage)))
        {
          Gamestage = (_pInfo.EP != null ? _pInfo.EP.gameStage : (_pInfo.PCP != null ? (int?)_pInfo.PCP.Gamestage : null));
          _bin.Add("Gamestage", Gamestage);
        }

        //SCORE
        if ((useInt && intFilter.Contains((int)Filters.Score)) || (!useInt && strFilter.Contains(StrFilters.Score)))
        {
          Score = (_pInfo.EP != null ? _pInfo.EP.Score : _pInfo.PDF.score);
        _bin.Add("Score", Score);
        }

        //KILLEDPLAYERS
        if ((useInt && intFilter.Contains((int)Filters.KilledPlayers)) || (!useInt && strFilter.Contains(StrFilters.KilledPlayers)))
        {
          KilledPlayers = (_pInfo.EP != null ? _pInfo.EP.KilledPlayers : _pInfo.PDF.playerKills);
        _bin.Add("KilledPlayers", KilledPlayers);
        }

        //KILLEDZOMBIES
        if ((useInt && intFilter.Contains((int)Filters.KilledZombies)) || (!useInt && strFilter.Contains(StrFilters.KilledZombies)))
        {
          KilledZombies = (_pInfo.EP != null ? _pInfo.EP.KilledZombies : _pInfo.PDF.zombieKills);
        _bin.Add("KilledZombies", KilledZombies);
        }

        //DEATHS
        if ((useInt && intFilter.Contains((int)Filters.Deaths)) || (!useInt && strFilter.Contains(StrFilters.Deaths)))
        {
          Deaths = (_pInfo.EP != null ? _pInfo.EP.Died : _pInfo.PDF.deaths);
        _bin.Add("Deaths", Deaths);
        }

        //CURRENTLIFE
        if ((useInt && intFilter.Contains((int)Filters.CurrentLife)) || (!useInt && strFilter.Contains(StrFilters.CurrentLife)))
        {
          CurrentLife = Math.Round((_pInfo.EP != null ? _pInfo.EP.currentLife : _pInfo.PDF.currentLife), 2);
        _bin.Add("CurrentLife", CurrentLife);
        }

        //LONGESTLIFE
        if ((useInt && intFilter.Contains((int)Filters.LongestLife)) || (!useInt && strFilter.Contains(StrFilters.LongestLife)))
        {
          LongestLife = Math.Round((_pInfo.EP != null ? _pInfo.EP.longestLife : _pInfo.PDF.longestLife), 2);
        _bin.Add("LongestLife", LongestLife);
        }

        //ITEMSCRAFTED
        if ((useInt && intFilter.Contains((int)Filters.ItemsCrafted)) || (!useInt && strFilter.Contains(StrFilters.ItemsCrafted)))
        {
          ItemsCrafted = (_pInfo.EP != null ? _pInfo.EP.totalItemsCrafted : _pInfo.PDF.totalItemsCrafted);
          _bin.Add("ItemsCrafted", ItemsCrafted);
        }

        //ISDEAD
        if ((useInt && intFilter.Contains((int)Filters.IsDead)) || (!useInt && strFilter.Contains(StrFilters.IsDead)))
        {
          IsDead = (_pInfo.EP != null ? _pInfo.EP.IsDead() : _pInfo.PDF.bDead);
          _bin.Add("IsDead", IsDead);
        }

        if (_pInfo.EP != null)
        {
          //ONGROUND
          if ((useInt && intFilter.Contains((int)Filters.OnGround)) || (!useInt && strFilter.Contains(StrFilters.OnGround)))
          {
            OnGround = _pInfo.EP.onGround;
            _bin.Add("OnGround", OnGround);
          }

          //ISSTUCK
          if ((useInt && intFilter.Contains((int)Filters.IsStuck)) || (!useInt && strFilter.Contains(StrFilters.IsStuck)))
          {
            IsStuck = _pInfo.EP.IsStuck;
            _bin.Add("IsStuck", IsStuck);
          }

          //ISSAFEZONEACTIVE
          if ((useInt && intFilter.Contains((int)Filters.IsSafeZoneActive)) || (!useInt && strFilter.Contains(StrFilters.IsSafeZoneActive)))
          {
            IsSafeZoneActive = _pInfo.EP.IsSafeZoneActive();
            _bin.Add("IsSafeZoneActive", IsSafeZoneActive);
          }

          //REMOTE
          if ((useInt && intFilter.Contains((int)Filters.Remote)) || (!useInt && strFilter.Contains(StrFilters.Remote)))
          {
            Remote = _pInfo.EP.isEntityRemote;
            _bin.Add("Remote", Remote);
          }

          //LASTZOMBIEATTACKED
          if ((useInt && intFilter.Contains((int)Filters.LastZombieAttacked)) || (!useInt && strFilter.Contains(StrFilters.LastZombieAttacked)))
          {
            LastZombieAttacked = Math.Round(((GameManager.Instance.World.worldTime - _pInfo.EP.LastZombieAttackTime) / 600f), 2);
            _bin.Add("LastZombieAttacked", LastZombieAttacked);
          }
        }

        //RENTEDVENDOR
        if ((useInt && intFilter.Contains((int)Filters.RentedVendor)) || (!useInt && strFilter.Contains(StrFilters.RentedVendor)))
        {
          //todo:BCMVector3i
          RentedVendor = (_pInfo.EP != null ? Convert.PosToStr(_pInfo.EP.RentedVMPosition, GetPosType()) : Convert.PosToStr(_pInfo.PDF.rentedVMPosition, GetPosType()));
          _bin.Add("RentedVendor", RentedVendor);
        }

        //RENTEDVENDOREXPIRE
        if ((useInt && intFilter.Contains((int)Filters.RentedVendorExpire)) || (!useInt && strFilter.Contains(StrFilters.RentedVendorExpire)))
        {
          RentedVendorExpire = (_pInfo.EP != null ? _pInfo.EP.RentalEndTime : _pInfo.PDF.rentalEndTime);
          _bin.Add("RentedVendorExpire", RentedVendorExpire);
        }
      }
      else
      {
        if (isOption("full") || !isOption("bag bg belt bt equipment eq buffs bu skillpoints pt skills sk crafting cq favrecipes fr unlockedrecipes ur quests qu spawns sp waypoints wp"))
        {
          Wellness = (int)(_pInfo.EP != null ? ((_pInfo.EP.Stats.Wellness.Value == 100 && _pInfo.EP.Stats.Wellness.Value != _pInfo.PDF.ecd.stats.Wellness.Value) ? _pInfo.PDF.ecd.stats.Wellness.Value : _pInfo.EP.Stats.Wellness.Value) : _pInfo.PDF.ecd.stats.Wellness.Value);
          Health = (int)(_pInfo.EP != null ? ((_pInfo.EP.Health == 100 && _pInfo.EP.Health != _pInfo.PDF.ecd.stats.Health.Value) ? _pInfo.PDF.ecd.stats.Health.Value : _pInfo.EP.Health) : _pInfo.PDF.ecd.stats.Health.Value);
          Stamina = (int)(_pInfo.EP != null ? ((_pInfo.EP.Stamina == 100 && _pInfo.EP.Stamina != _pInfo.PDF.ecd.stats.Stamina.Value) ? _pInfo.PDF.ecd.stats.Stamina.Value : _pInfo.EP.Stamina) : _pInfo.PDF.ecd.stats.Stamina.Value);
          Food = (int)(_pInfo.PDF.food.GetLifeLevelFraction() * 100);
          Drink = (int)(_pInfo.PDF.drink.GetLifeLevelFraction() * 100);
          CoreTemp = (int)_pInfo.PDF.ecd.stats.CoreTemp.Value;
          SpeedModifier = Math.Round((_pInfo.EP != null ? ((_pInfo.EP.Stats.SpeedModifier.Value == 1 && _pInfo.EP.Stats.SpeedModifier.Value != _pInfo.PDF.ecd.stats.SpeedModifier.Value) ? _pInfo.PDF.ecd.stats.SpeedModifier.Value : _pInfo.EP.Stats.SpeedModifier.Value) : _pInfo.PDF.ecd.stats.SpeedModifier.Value), 1);
          Archetype = _pInfo.PDF.ecd.playerProfile.Archetype;
          DistanceWalked = Math.Round(_pInfo.PDF.distanceWalked, 1);
          //todo:BCMVector3i
          DroppedPack = (_pInfo.PDF.droppedBackpackPosition != Vector3i.zero ? Convert.PosToStr(_pInfo.PDF.droppedBackpackPosition, GetPosType()) : "None");
          Level = (_pInfo.EP != null ? _pInfo.EP.GetLevel() : _pInfo.PDF.level);
          LevelProgress = Math.Round((_pInfo.EP != null ? (_pInfo.EP.GetLevelProgressPercentage() * 100) : ((1 - _pInfo.PDF.experience / Math.Min((Progression.BaseExpToLevel * Mathf.Pow(Progression.ExpMultiplier, _pInfo.PDF.level + 1)), int.MaxValue)) * 100)), 2);
          ExpToNextLevel = (_pInfo.EP != null ? _pInfo.EP.ExpToNextLevel : (int)_pInfo.PDF.experience);
          ExpForNextLevel = (_pInfo.EP != null ? _pInfo.EP.GetExpForNextLevel() : (int)Math.Min((Progression.BaseExpToLevel * Mathf.Pow(Progression.ExpMultiplier, _pInfo.PDF.level + 1)), int.MaxValue));
          Gamestage = (_pInfo.EP != null ? _pInfo.EP.gameStage : (_pInfo.PCP != null ? (int?)_pInfo.PCP.Gamestage : null));
          Score = (_pInfo.EP != null ? _pInfo.EP.Score : _pInfo.PDF.score);
          KilledPlayers = (_pInfo.EP != null ? _pInfo.EP.KilledPlayers : _pInfo.PDF.playerKills);
          KilledZombies = (_pInfo.EP != null ? _pInfo.EP.KilledZombies : _pInfo.PDF.zombieKills);
          Deaths = (_pInfo.EP != null ? _pInfo.EP.Died : _pInfo.PDF.deaths);
          CurrentLife = Math.Round((_pInfo.EP != null ? _pInfo.EP.currentLife : _pInfo.PDF.currentLife), 2);
          LongestLife = Math.Round((_pInfo.EP != null ? _pInfo.EP.longestLife : _pInfo.PDF.longestLife), 2);
          ItemsCrafted = (_pInfo.EP != null ? _pInfo.EP.totalItemsCrafted : _pInfo.PDF.totalItemsCrafted);
          IsDead = (_pInfo.EP != null ? _pInfo.EP.IsDead() : _pInfo.PDF.bDead);
          if (_pInfo.EP != null)
          {
            OnGround = _pInfo.EP.onGround;
            IsStuck = _pInfo.EP.IsStuck;
            IsSafeZoneActive = _pInfo.EP.IsSafeZoneActive();
            Remote = _pInfo.EP.isEntityRemote;
            LastZombieAttacked = Math.Round(((GameManager.Instance.World.worldTime - _pInfo.EP.LastZombieAttackTime) / 600f), 2);
          }
          //todo:BCMVector3i
          RentedVendor = (_pInfo.EP != null ? Convert.PosToStr(_pInfo.EP.RentedVMPosition, GetPosType()) : Convert.PosToStr(_pInfo.PDF.rentedVMPosition, GetPosType()));
          RentedVendorExpire = (_pInfo.EP != null ? _pInfo.EP.RentalEndTime : _pInfo.PDF.rentalEndTime);

          _bin.Add("Wellness", Wellness);
          _bin.Add("Health", Health);
          _bin.Add("Stamina", Stamina);
          if (isOption("full")) { _bin.Add("Food", Food); }
          if (isOption("full")) { _bin.Add("Drink", Drink); }
          if (isOption("full")) { _bin.Add("CoreTemp", CoreTemp); }
          if (isOption("full")) { _bin.Add("SpeedModifier", SpeedModifier); }
          if (isOption("full")) { _bin.Add("Archetype", Archetype); }
          if (isOption("full")) { _bin.Add("DistanceWalked", DistanceWalked); }
          if (isOption("full")) { _bin.Add("DroppedPack", DroppedPack); }
          _bin.Add("Level", Level);
          _bin.Add("LevelProgress", LevelProgress);
          if (isOption("full")) { _bin.Add("ExpToNextLevel", ExpToNextLevel); }
          if (isOption("full")) { _bin.Add("ExpForNextLevel", ExpForNextLevel); }
          _bin.Add("Gamestage", Gamestage);
          _bin.Add("Score", Score);
          _bin.Add("KilledPlayers", KilledPlayers);
          _bin.Add("KilledZombies", KilledZombies);
          _bin.Add("Deaths", Deaths);
          if (isOption("full")) { _bin.Add("CurrentLife", CurrentLife); }
          if (isOption("full")) { _bin.Add("LongestLife", LongestLife); }
          if (isOption("full")) { _bin.Add("ItemsCrafted", ItemsCrafted); }
          if (isOption("full")) { _bin.Add("IsDead", IsDead); }
          if (isOption("full")) { _bin.Add("OnGround", OnGround); }
          if (isOption("full")) { _bin.Add("IsStuck", IsStuck); }
          if (isOption("full")) { _bin.Add("IsSafeZoneActive", IsSafeZoneActive); }
          if (isOption("full")) { _bin.Add("Remote", Remote); }
          if (isOption("full")) { _bin.Add("LastZombieAttacked", LastZombieAttacked); }
          if (isOption("full")) { _bin.Add("RentedVendor", RentedVendor); }
          if (isOption("full")) { _bin.Add("RentedVendorExpire", RentedVendorExpire); }
        }
      }
    }

    private void GetClientInfo(PlayerInfo _pInfo)
    {
      if (isOption("filter"))
      {
        //STEAMID
        if ((useInt && intFilter.Contains((int)Filters.SteamId)) || (!useInt && strFilter.Contains(StrFilters.SteamId)))
        {
          SteamId = _pInfo._steamId;
          _bin.Add("SteamId", SteamId);
        }

        //NAME
        if ((useInt && intFilter.Contains((int)Filters.Name)) || (!useInt && strFilter.Contains(StrFilters.Name)))
        {
          Name = (_pInfo.CI != null ? _pInfo.CI.playerName : _pInfo.PCP != null ? _pInfo.PCP.Name : string.Empty);
          _bin.Add("Name", Name);
        }

        //ENTITYID
        if ((useInt && intFilter.Contains((int)Filters.EntityId)) || (!useInt && strFilter.Contains(StrFilters.EntityId)))
        {
          if (_pInfo.EP != null)
          {
            EntityId = _pInfo.EP.entityId;
          }
          else if (_pInfo.PDF != null)
          {
            EntityId = _pInfo.PDF.id;
          }
          _bin.Add("EntityId", EntityId);
        }

        //IP
        if ((useInt && intFilter.Contains((int)Filters.IP)) || (!useInt && strFilter.Contains(StrFilters.IP)))
        {
          IP = (_pInfo.CI != null ? _pInfo.CI.ip.ToString() : _pInfo.PCP != null ? _pInfo.PCP.IP.ToString() : string.Empty);
          _bin.Add("IP", IP);
        }

        //PING
        if ((useInt && intFilter.Contains((int)Filters.Ping)) || (!useInt && strFilter.Contains(StrFilters.Ping)))
        {
          Ping = (_pInfo.CI != null ? _pInfo.CI.ping.ToString() : "Offline");
          _bin.Add("Ping", Ping);
        }

        //TOTALPLAYTIME
        if ((useInt && intFilter.Contains((int)Filters.TotalPlayTime)) || (!useInt && strFilter.Contains(StrFilters.TotalPlayTime)))
        {
          long totalPlayTime = (_pInfo.PCP != null ? _pInfo.PCP.TotalPlayTime : 0);
          TotalPlayTime = Math.Round(totalPlayTime / 60f, 2);
          _bin.Add("TotalPlayTime", TotalPlayTime);
        }

        if (_pInfo.EP == null)
        {
          //LASTONLINE
          if ((useInt && intFilter.Contains((int)Filters.LastOnline)) || (!useInt && strFilter.Contains(StrFilters.LastOnline)))
          {
            LastOnline = (_pInfo.PCP != null ? _pInfo.PCP.LastOnline.ToString("yyyy-MM-dd HH:mm") : "");
            _bin.Add("LastOnline", LastOnline);
          }
        }
        else if (_pInfo.EP != null)
        {
          //SESSIONPLAYTIME
          if ((useInt && intFilter.Contains((int)Filters.SessionPlayTime)) || (!useInt && strFilter.Contains(StrFilters.SessionPlayTime)))
          {
            SessionPlayTime = Math.Round((Time.timeSinceLevelLoad - _pInfo.EP.CreationTimeSinceLevelLoad) / 60, 2);
            _bin.Add("SessionPlayTime", SessionPlayTime);
          }
        }

        //UNDERGROUND
        if ((useInt && intFilter.Contains((int)Filters.Underground)) || (!useInt && strFilter.Contains(StrFilters.Underground)))
        {
          Underground = (int)(_pInfo.EP != null ? _pInfo.EP.position.y - (int)Math.Floor(_pInfo.EP.serverPos.y / 32f) : 0);
          _bin.Add("Underground", Underground);
        }

        //POSITION
        if ((useInt && intFilter.Contains((int)Filters.Position)) || (!useInt && strFilter.Contains(StrFilters.Position)))
        {
          var p = (_pInfo.EP != null ? _pInfo.EP.position : (_pInfo.PDF != null ? _pInfo.PDF.ecd.pos : Vector3.zero));
          Position = new BCMVector3i(p);
          _bin.Add("Position", GetVectorObj(Position));
        }

        //ROTATION
        if ((useInt && intFilter.Contains((int)Filters.Rotation)) || (!useInt && strFilter.Contains(StrFilters.Rotation)))
        {
          var r = (_pInfo.EP != null ? _pInfo.EP.rotation : (_pInfo.PDF != null ? _pInfo.PDF.ecd.rot : Vector3.zero));
          Rotation = new BCMVector3i(r);
          _bin.Add("Rotation", GetVectorObj(Rotation));
        }
      }
      else
      {
        if (isOption("full") || !isOption("bag bg belt bt equipment eq buffs bu skillpoints pt skills sk crafting cq favrecipes fr unlockedrecipes ur quests qu spawns sp waypoints wp"))
        {
          SteamId = _pInfo._steamId;
          Name = (_pInfo.CI != null ? _pInfo.CI.playerName : _pInfo.PCP != null ? _pInfo.PCP.Name : string.Empty);
          EntityId = _pInfo.PDF.id;
          IP = (_pInfo.CI != null ? _pInfo.CI.ip.ToString() : _pInfo.PCP != null ? _pInfo.PCP.IP.ToString() : string.Empty);
          //todo: add last ping to persistent data?
          Ping = (_pInfo.CI != null ? _pInfo.CI.ping.ToString() : "Offline");

          long totalPlayTime = (_pInfo.PCP != null ? _pInfo.PCP.TotalPlayTime : 0);
          TotalPlayTime = Math.Round(totalPlayTime / 60f, 2);
          if (_pInfo.EP == null)
          {
            LastOnline = (_pInfo.PCP != null ? _pInfo.PCP.LastOnline.ToString("yyyy-MM-dd HH:mm") : "");
          }
          else if (_pInfo.EP != null)
          {
            SessionPlayTime = Math.Round((Time.timeSinceLevelLoad - _pInfo.EP.CreationTimeSinceLevelLoad) / 60, 2);
          }
          Underground = (int)(_pInfo.EP != null ? _pInfo.EP.position.y - (int)Math.Floor(_pInfo.EP.serverPos.y / 32f) : 0);

          var p = (_pInfo.EP != null ? _pInfo.EP.position : (_pInfo.PDF != null ? _pInfo.PDF.ecd.pos : Vector3.zero));
          var r = (_pInfo.EP != null ? _pInfo.EP.rotation : (_pInfo.PDF != null ? _pInfo.PDF.ecd.rot : Vector3.zero));
          Position = new BCMVector3i(p);
          Rotation = new BCMVector3i(r);

          _bin.Add("SteamId", SteamId);
          _bin.Add("Name", Name);
          _bin.Add("EntityId", EntityId);
          if (isOption("full"))
          { 
            _bin.Add("IP", IP);
            _bin.Add("Ping", Ping);
            _bin.Add("TotalPlayTime", TotalPlayTime);
            _bin.Add("LastOnline", LastOnline);
            _bin.Add("SessionPlayTime", SessionPlayTime);
          }
          _bin.Add("Underground", Underground);
          _bin.Add("Position", GetVectorObj(Position));
          if (isOption("full"))
          {
            _bin.Add("Rotation", GetVectorObj(Rotation));
          }
        }
      }
    }

    private object GetVectorObj(BCMVector3i p)
    {
      if (options.ContainsKey("strpos"))
      {
        return p.x.ToString() + " " + p.y.ToString() + " " + p.z.ToString();
      }
      else if (options.ContainsKey("worldpos"))
      {
        return GameUtils.WorldPosToStr(new Vector3(p.x, p.y, p.z), " ");
      }
      else if (options.ContainsKey("csvpos"))
      {
        return new int[3] { p.x, p.y, p.z };
        //return "[" + p.x.ToString() + "," + p.y.ToString() + "," + p.z.ToString() + "]";
      }
      else
      {
        //vectors
        return p;
      }
    }

    public bool isOption(string key, bool isOr = true)
    {
      var _o = !isOr;
      var keys = key.Split(' ');
      if (keys.Length > 1)
      {
        foreach (var k in keys)
        {
          if (isOr)
          {
            _o |= options.ContainsKey(k);
          }
          else
          {
            _o &= options.ContainsKey(k);
          }
        }
        return _o;
      }
      else
      {
        return options.ContainsKey(key);
      }
    }

    public string OptionValue(string key)
    {
      if (options.ContainsKey(key))
      {
        return options[key];
      }
      return "";
    }

    public string GetPosType()
    {
      var postype = "strpos";
      if (options.ContainsKey("worldpos"))
      {
        postype = "worldpos";
      }
      if (options.ContainsKey("csvpos"))
      {
        postype = "csvpos";
      }
      if (options.ContainsKey("vectors"))
      {
        postype = "vectors";
      }

      return postype;
    }

  }
}
