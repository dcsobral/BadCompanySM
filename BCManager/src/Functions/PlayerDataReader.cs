using System;
using System.Collections.Generic;
using System.IO;

namespace BCM
{
  public class PlayerDataReader
  {
    public bool bLoaded;
    public EntityCreationData ecd = new EntityCreationData();
    public ItemStack[] inventory = new ItemStack[0];
    public ItemStack[] bag = new ItemStack[0];
    public Equipment equipment = new Equipment();
    public Equipment favoriteEquipment = new Equipment();
    public int selectedInventorySlot;
    public LiveStats food = new LiveStats(0, 0);
    public LiveStats drink = new LiveStats(0, 0);
    public List<Vector3i> spawnPoints = new List<Vector3i>();
    public long selectedSpawnPointKey;
    public HashSet<string> alreadyCraftedList = new HashSet<string>();
    public List<string> unlockedRecipeList = new List<string>();
    public List<string> favoriteRecipeList = new List<string>();
    public SpawnPosition lastSpawnPosition = SpawnPosition.Undef;
    public Vector3i droppedBackpackPosition = Vector3i.zero;
    public int playerKills;
    public int zombieKills;
    public int deaths;
    public int score;
    public int id;
    public Vector3i markerPosition = new Vector3i();
    public uint experience;
    public int level;
    public int skillPoints;
    public bool bCrouchedLocked;
    public CraftingData craftingData = new CraftingData();
    public int deathUpdateTime;
    public bool bDead;
    public float distanceWalked = 0f;
    public uint totalItemsCrafted = 0u;
    public float longestLife = 0f;
    public float currentLife = 0f;
    public WaypointCollection waypoints = new WaypointCollection();
    public QuestJournal questJournal = new QuestJournal();
    public bool IsModdedSave;
    public PlayerJournal playerJournal = new PlayerJournal();
    public Vector3i rentedVMPosition = Vector3i.zero;
    public ulong rentalEndTime = 0uL;
    public List<int> trackedFriendEntityIds = new List<int>();
    public List<Skill> skills;

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
      ecd = new EntityCreationData();
      ecd.read(_br, false);
      food = new LiveStats(Constants.cMaxPlayerFood, Constants.cFoodOversaturate);
      food.Read(_br);
      drink = new LiveStats(Constants.cMaxPlayerDrink, Constants.cDrinkOversaturate);
      drink.Read(_br);
      inventory = GameUtils.ReadItemStack(_br);
      selectedInventorySlot = _br.ReadByte();
      bag = GameUtils.ReadItemStack(_br);
      if (bag.Length > 32)
      {
        ItemStack[] destinationArray = ItemStack.CreateArray(32);
        Array.Copy(bag, destinationArray, 32);
        bag = destinationArray;
      }
      alreadyCraftedList = new HashSet<string>();
      int num = _br.ReadUInt16();
      for (int i = 0; i < num; i++)
      {
        alreadyCraftedList.Add(_br.ReadString());
      }
      byte b = _br.ReadByte();
      for (int j = 0; j < b; j++)
      {
        spawnPoints.Add(new Vector3i(_br.ReadInt32(), _br.ReadInt32(), _br.ReadInt32()));
      }
      selectedSpawnPointKey = _br.ReadInt64();
      _br.ReadBoolean();
      _br.ReadInt16();
      if (_version > 1u)
      {
        bLoaded = _br.ReadBoolean();
      }
      if (_version > 2u)
      {
        lastSpawnPosition = new SpawnPosition(new Vector3i(_br.ReadInt32(), _br.ReadInt32(), _br.ReadInt32()), _br.ReadSingle());
      }
      else if (_version > 1u)
      {
        lastSpawnPosition = new SpawnPosition(new Vector3i(_br.ReadInt32(), _br.ReadInt32(), _br.ReadInt32()), 0f);
      }
      if (_version > 3u)
      {
        id = _br.ReadInt32();
      }
      if (_version > 4u)
      {
        droppedBackpackPosition = new Vector3i(_br.ReadInt32(), _br.ReadInt32(), _br.ReadInt32());
      }
      if (_version > 5u)
      {
        playerKills = _br.ReadInt32();
        zombieKills = _br.ReadInt32();
        deaths = _br.ReadInt32();
        score = _br.ReadInt32();
        equipment = Equipment.Read(_br);
      }
      if (_version > 6u)
      {
        unlockedRecipeList = new List<string>();
        num = _br.ReadUInt16();
        for (int k = 0; k < num; k++)
        {
          unlockedRecipeList.Add(_br.ReadString());
        }
      }
      if (_version > 7u)
      {
        _br.ReadUInt16();
        markerPosition = NetworkUtils.ReadVector3i(_br);
      }
      if (_version > 8u)
      {
        favoriteEquipment = Equipment.Read(_br);
      }
      if (_version > 10u)
      {
        experience = _br.ReadUInt32();
      }
      if (_version > 22u)
      {
        level = _br.ReadInt32();
      }
      if (_version > 11u)
      {
        bCrouchedLocked = _br.ReadBoolean();
      }
      craftingData.Read(_br, _version);

      if (_version > 14u)
      {
        if (_version < 18u)
        {
          Skills pdfskills = new Skills();
          pdfskills.Read(_br, _version);
        }
      }
      if (_version > 16u)
      {
        favoriteRecipeList = new List<string>();
        num = _br.ReadUInt16();
        for (int l = 0; l < num; l++)
        {
          favoriteRecipeList.Add(_br.ReadString());
        }
      }
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
            EP.Skills.Read(new BinaryReader(pdfskills), 34u);
          }
          skills = EP.Skills.GetAllSkills();
          //end custom skill loader
        }
      }
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
    }
  }
}
