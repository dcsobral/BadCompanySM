using System;
using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;

namespace BCM
{
  public class PlayerDataReader
  {
    #region Properties
    [UsedImplicitly] public HashSet<string> alreadyCraftedList = new HashSet<string>();
    [UsedImplicitly] public ItemStack[] bag = new ItemStack[0];
    [UsedImplicitly] public bool bCrouchedLocked;
    [UsedImplicitly] public bool bDead;
    [UsedImplicitly] public bool bLoaded;
    [UsedImplicitly] public CraftingData craftingData = new CraftingData();
    [UsedImplicitly] public float currentLife;
    [UsedImplicitly] public int deaths;
    [UsedImplicitly] public int deathUpdateTime;
    [UsedImplicitly] public float distanceWalked;
    [UsedImplicitly] public LiveStats drink = new LiveStats(0, 0);
    [UsedImplicitly] public Vector3i droppedBackpackPosition = Vector3i.zero;
    [UsedImplicitly] public EntityCreationData ecd = new EntityCreationData();
    [UsedImplicitly] public Equipment equipment = new Equipment();
    [UsedImplicitly] public uint experience;
    [UsedImplicitly] public Equipment favoriteEquipment = new Equipment();
    [UsedImplicitly] public List<string> favoriteRecipeList = new List<string>();
    [UsedImplicitly] public LiveStats food = new LiveStats(0, 0);
    [UsedImplicitly] public int id;
    [UsedImplicitly] public ItemStack[] inventory = new ItemStack[0];
    [UsedImplicitly] public SpawnPosition lastSpawnPosition = SpawnPosition.Undef;
    [UsedImplicitly] public int level;
    [UsedImplicitly] public float longestLife;
    [UsedImplicitly] public Vector3i markerPosition;
    [UsedImplicitly] public PlayerJournal playerJournal = new PlayerJournal();
    [UsedImplicitly] public int playerKills;
    [UsedImplicitly] public QuestJournal questJournal = new QuestJournal();
    [UsedImplicitly] public ulong rentalEndTime;
    [UsedImplicitly] public Vector3i rentedVMPosition = Vector3i.zero;
    [UsedImplicitly] public int score;
    [UsedImplicitly] public int selectedInventorySlot;
    [UsedImplicitly] public long selectedSpawnPointKey;
    [UsedImplicitly] public int skillPoints;
    [UsedImplicitly] public List<Vector3i> spawnPoints = new List<Vector3i>();
    [UsedImplicitly] public uint totalItemsCrafted;
    [UsedImplicitly] public List<int> trackedFriendEntityIds = new List<int>();
    [UsedImplicitly] public List<string> unlockedRecipeList = new List<string>();
    [UsedImplicitly] public WaypointCollection waypoints = new WaypointCollection();
    [UsedImplicitly] public int zombieKills;
    [UsedImplicitly] public ulong gameStageLifetimeTicks;

    [UsedImplicitly] public bool IsModdedSave;
    [UsedImplicitly] public List<Skill> skills;
    //public PlayerStealth stealth;
    #endregion

    public PlayerDataReader()
    {
    }

    public PlayerDataReader(string steamid)
    {
      Init(steamid);
    }

    public void Init(string steamid)
    {
      try
      {
        var file = $"{GameUtils.GetPlayerDataDir()}/{steamid}.ttp";
        if (!Utils.FileExists(file)) return;

        var binaryReader = new BinaryReader(new FileStream(file, FileMode.Open));
        if (binaryReader.ReadChar() != 't' ||
          binaryReader.ReadChar() != 't' ||
          binaryReader.ReadChar() != 'p' ||
          binaryReader.ReadChar() != '\0')
          return;

        Parse(binaryReader);
        binaryReader.Close();

        bLoaded = true;
      }
      catch (Exception e)
      {
        Log.Error($"{Config.ModPrefix} Error loading data file. {e}");
      }
    }

    public void Parse(BinaryReader binaryReader)
    {
      var version = (uint)binaryReader.ReadByte();

      Read(binaryReader, version);
    }

    public void Read(BinaryReader _br, uint _version)
    {
      //ECD
      ecd = new EntityCreationData();
      ecd.read(_br, false);

      //FOOD/DRINK
      food = new LiveStats(Constants.cMaxPlayerFood, Constants.cFoodOversaturate);
      food.Read(_br);
      drink = new LiveStats(Constants.cMaxPlayerDrink, Constants.cDrinkOversaturate);
      drink.Read(_br);

      //INVENTORY
      inventory = GameUtils.ReadItemStack(_br);
      selectedInventorySlot = _br.ReadByte();

      //BAG
      bag = GameUtils.ReadItemStack(_br);
      //REMOVED - To allow for bigger backpack mods
      //if (bag.Length > 32)
      //{
      //  var destinationArray = ItemStack.CreateArray(32);
      //  Array.Copy(bag, destinationArray, 32);
      //  bag = destinationArray;
      //}

      //CRAFTED
      alreadyCraftedList = new HashSet<string>();
      int num = _br.ReadUInt16();
      for (var i = 0; i < num; i++)
      {
        alreadyCraftedList.Add(_br.ReadString());
      }

      //SPAWNS
      var b = _br.ReadByte();
      for (var j = 0; j < b; j++)
      {
        spawnPoints.Add(new Vector3i(_br.ReadInt32(), _br.ReadInt32(), _br.ReadInt32()));
      }
      selectedSpawnPointKey = _br.ReadInt64();

      //LOADED
      _br.ReadBoolean();
      _br.ReadInt16();
      if (_version > 1u)
      {
        bLoaded = _br.ReadBoolean();
      }

      //LASTSPAWN
      if (_version > 2u)
      {
        lastSpawnPosition = new SpawnPosition(new Vector3i(_br.ReadInt32(), _br.ReadInt32(), _br.ReadInt32()), _br.ReadSingle());
      }
      else if (_version > 1u)
      {
        lastSpawnPosition = new SpawnPosition(new Vector3i(_br.ReadInt32(), _br.ReadInt32(), _br.ReadInt32()), 0f);
      }

      //ID
      if (_version > 3u)
      {
        id = _br.ReadInt32();
      }

      //BACKPACK
      if (_version > 4u)
      {
        droppedBackpackPosition = new Vector3i(_br.ReadInt32(), _br.ReadInt32(), _br.ReadInt32());
      }

      //STATS/EQUIPMENT
      if (_version > 5u)
      {
        playerKills = _br.ReadInt32();
        zombieKills = _br.ReadInt32();
        deaths = _br.ReadInt32();
        score = _br.ReadInt32();
        equipment = Equipment.Read(_br);
      }

      //RECIPES
      if (_version > 6u)
      {
        unlockedRecipeList = new List<string>();
        num = _br.ReadUInt16();
        for (var k = 0; k < num; k++)
        {
          unlockedRecipeList.Add(_br.ReadString());
        }
      }

      //MARKER
      if (_version > 7u)
      {
        _br.ReadUInt16();
        markerPosition = NetworkUtils.ReadVector3i(_br);
      }

      //FAVS
      if (_version > 8u)
      {
        favoriteEquipment = Equipment.Read(_br);
      }

      //EXP
      if (_version > 10u)
      {
        experience = _br.ReadUInt32();
      }

      //LEVEL
      if (_version > 22u)
      {
        level = _br.ReadInt32();
      }

      //CROUCHED
      if (_version > 11u)
      {
        bCrouchedLocked = _br.ReadBoolean();
      }

      //CRAFTINGDATA
      craftingData.Read(_br, _version);

      //SKILLS - part1
      if (_version > 14u)
      {
        if (_version < 18u)
        {
          var pdfskills = new Skills();
          pdfskills.Read(_br, _version);
        }
      }

      //FAVRECIPES
      if (_version > 16u)
      {
        favoriteRecipeList = new List<string>();
        num = _br.ReadUInt16();
        for (var l = 0; l < num; l++)
        {
          favoriteRecipeList.Add(_br.ReadString());
        }
      }

      //SKILLS - part2
      if (_version > 17u)
      {
        var num2 = (int)_br.ReadUInt32();
        if (num2 > 0)
        {
          //custom skill loader
          var pdfskills = new MemoryStream(_br.ReadBytes(num2));
          
          var skillsReader = new Skills();
          skillsReader.Read(new BinaryReader(pdfskills), _version);
          skills = skillsReader.GetAllSkills();
          //end custom skill loader
        }
      }

      //STATS
      if (_version > 18u)
      {
        totalItemsCrafted = _br.ReadUInt32();
        distanceWalked = _br.ReadSingle();
        longestLife = _br.ReadSingle();
      }

      if (_version > 35u)
      {
        gameStageLifetimeTicks = _br.ReadUInt64();
      }
      else
      {
        gameStageLifetimeTicks = 0uL;
      }

      if (_version > 19u)
      {
        waypoints = new WaypointCollection();
        waypoints.Read(_br);
      }
      if (_version > 23u)
      {
        skillPoints = _br.ReadInt32();
      }
      if (_version > 24u)
      {
        questJournal = new QuestJournal();
        questJournal.Read(_br);
      }
      if (_version > 25u)
      {
        deathUpdateTime = _br.ReadInt32();
      }
      if (_version > 26u)
      {
        currentLife = _br.ReadSingle();
      }
      if (_version > 29u)
      {
        bDead = _br.ReadBoolean();
      }
      if (_version > 30u)
      {
        _br.ReadByte();
        IsModdedSave = _br.ReadBoolean();
      }
      if (_version > 31u)
      {
        playerJournal = new PlayerJournal();
        playerJournal.Read(_br);
      }
      if (_version > 32u)
      {
        rentedVMPosition = NetworkUtils.ReadVector3i(_br);
        rentalEndTime = _br.ReadUInt64();
      }
      if (_version > 33u)
      {
        trackedFriendEntityIds.Clear();
        int num3 = _br.ReadUInt16();
        for (var m = 0; m < num3; m++)
        {
          trackedFriendEntityIds.Add(_br.ReadInt32());
        }
      }
      if (_version > 34u)
      {
        var num4 = _br.ReadInt32();
        if (num4 > 0)
        {
          //var pdfstealth = new MemoryStream(_br.ReadBytes(num4));
          //todo: custom loader
        }
      }
    }
  }
}
