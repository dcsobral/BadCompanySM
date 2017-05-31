using System;
using System.Collections.Generic;
using System.IO;

namespace BCM
{
  public class PlayerDataReader
  {
    public HashSet<string> alreadyCraftedList = new HashSet<string>();
    public ItemStack[] bag = new ItemStack[0];
    public bool bCrouchedLocked;
    public bool bDead;
    public bool bLoaded;
    public CraftingData craftingData = new CraftingData();
    public float currentLife = 0f;
    public int deaths;
    public int deathUpdateTime;
    public float distanceWalked = 0f;
    public LiveStats drink = new LiveStats(0, 0);
    public Vector3i droppedBackpackPosition = Vector3i.zero;
    public EntityCreationData ecd = new EntityCreationData();
    public Equipment equipment = new Equipment();
    public uint experience;
    public Equipment favoriteEquipment = new Equipment();
    public List<string> favoriteRecipeList = new List<string>();
    public LiveStats food = new LiveStats(0, 0);
    public int id;
    public ItemStack[] inventory = new ItemStack[0];
    public SpawnPosition lastSpawnPosition = SpawnPosition.Undef;
    public int level;
    public float longestLife = 0f;
    public Vector3i markerPosition = new Vector3i();
    public PlayerJournal playerJournal = new PlayerJournal();
    public int playerKills;
    public QuestJournal questJournal = new QuestJournal();
    public ulong rentalEndTime = 0uL;
    public Vector3i rentedVMPosition = Vector3i.zero;
    public int score;
    public int selectedInventorySlot;
    public long selectedSpawnPointKey;
    public int skillPoints;
    public List<Vector3i> spawnPoints = new List<Vector3i>();
    public uint totalItemsCrafted = 0u;
    public List<int> trackedFriendEntityIds = new List<int>();
    public List<string> unlockedRecipeList = new List<string>();
    public WaypointCollection waypoints = new WaypointCollection();
    public int zombieKills;

    public bool IsModdedSave;
    public List<Skill> skills;
    //public PlayerStealth stealth;

    public void GetData(string _steamid)
    {
      string _dir = GameUtils.GetPlayerDataDir();
      try
      {
        string file = _dir + "/" + _steamid + ".ttp";
        if (Utils.FileExists(file))
        {
          BinaryReader binaryReader = new BinaryReader(new FileStream(file, FileMode.Open));
          if (binaryReader.ReadChar() == 't')
          {
            if (binaryReader.ReadChar() == 't')
            {
              if (binaryReader.ReadChar() == 'p')
              {
                if (binaryReader.ReadChar() == '\0')
                {
                  uint version = (uint)binaryReader.ReadByte();
                  Read(binaryReader, version);
                  binaryReader.Close();
                  bLoaded = true;
                }
              }
            }
          }
        }
      }
      catch (Exception e)
      {
        Log.Error(Config.ModPrefix + " Error loading data file. " + e);
      }
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
      if (bag.Length > 32)
      {
        ItemStack[] destinationArray = ItemStack.CreateArray(32);
        Array.Copy(bag, destinationArray, 32);
        bag = destinationArray;
      }

      //CRAFTED
      alreadyCraftedList = new HashSet<string>();
      int num = _br.ReadUInt16();
      for (int i = 0; i < num; i++)
      {
        alreadyCraftedList.Add(_br.ReadString());
      }

      //SPAWNS
      byte b = _br.ReadByte();
      for (int j = 0; j < b; j++)
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
        for (int k = 0; k < num; k++)
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
          Skills pdfskills = new Skills();
          pdfskills.Read(_br, _version);
        }
      }

      //FAVRECIPES
      if (_version > 16u)
      {
        favoriteRecipeList = new List<string>();
        num = _br.ReadUInt16();
        for (int l = 0; l < num; l++)
        {
          favoriteRecipeList.Add(_br.ReadString());
        }
      }

      //SKILLS - part2
      if (_version > 17u)
      {
        int num2 = (int)_br.ReadUInt32();
        if (num2 > 0)
        {
          //custom skill loader
          MemoryStream pdfskills = new MemoryStream(0);
          pdfskills = new MemoryStream(_br.ReadBytes(num2));
          EntityPlayer EP = new EntityPlayer();
          EP.Skills = new Skills();
          if (pdfskills.Length > 0L)
          {
            EP.Skills.Read(new BinaryReader(pdfskills), _version);
          }
          skills = EP.Skills.GetAllSkills();
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
        for (int m = 0; m < num3; m++)
        {
          trackedFriendEntityIds.Add(_br.ReadInt32());
        }
      }
      if (_version > 34u)
      {
        int num4 = _br.ReadInt32();
        if (num4 > 0)
        {
          MemoryStream pdfstealth = new MemoryStream(0);
          pdfstealth = new MemoryStream(_br.ReadBytes(num4));
          //todo: custom loader
        }
      }
    }
  }
}
