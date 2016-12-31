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
    private Inventory inventory;
    [OptionalField]
    private int lastPositionX, lastPositionY, lastPositionZ;
    [OptionalField]
    private bool chatMuted;
    [OptionalField]
    private int maxChatLength;
    [OptionalField]
    private string chatColor;
    [OptionalField]
    private bool chatName;
    [OptionalField]
    private uint expToNextLevel;
    [OptionalField]
    private int level;

    [NonSerialized]
    private ClientInfo clientInfo;

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

    public string IP
    {
      get { return ip == null ? string.Empty : ip; }
    }

    public Inventory Inventory
    {
      get
      {
        if (inventory == null)
          inventory = new Inventory();
        return inventory;
      }
    }

    public bool IsOnline
    {
      get { return clientInfo != null; }
    }

    public ClientInfo ClientInfo
    {
      get { return clientInfo; }
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
          return totalPlayTime + (long)(Time.timeSinceLevelLoad - Entity.CreationTimeSinceLevelLoad);
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

    public Vector3i LastPosition
    {
      get
      {
        if (IsOnline)
          return new Vector3i(Entity.GetPosition());
        else
          return new Vector3i(lastPositionX, lastPositionY, lastPositionZ);
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

    public float Level
    {
      get
      {
        float expForNextLevel = (int)Math.Min((Progression.BaseExpToLevel * Mathf.Pow(Progression.ExpMultiplier, level + 1)), int.MaxValue);
        float fLevel = level + 1f - ((float)expToNextLevel / expForNextLevel);
        return fLevel;
      }
    }

    public bool IsChatMuted
    {
      get
      {
        return chatMuted;
      }
      set
      {
        chatMuted = value;
      }
    }

    public int MaxChatLength
    {
      get
      {
        if (maxChatLength == 0)
        {
          maxChatLength = 255;
        }
        return maxChatLength;
      }
      set
      {
        maxChatLength = value;
      }
    }

    public string ChatColor
    {
      get
      {
        if (chatColor == null || chatColor == "")
        {
          chatColor = "";
        }
        return chatColor;
      }

      set
      {
        chatColor = value;
      }
    }

    public bool ChatName
    {
      get
      {
        return chatName;
      }

      set
      {
        chatName = value;
      }
    }

    public void SetOffline()
    {
      if (clientInfo != null)
      {
        //Log.Out("(" + Config.ModPrefix + ") Player set to offline: " + steamId);
        lastOnline = DateTime.Now;
        try
        {
          Vector3i lastPos = new Vector3i(Entity.GetPosition());
          lastPositionX = lastPos.x;
          lastPositionY = lastPos.y;
          lastPositionZ = lastPos.z;
          totalPlayTime += (long)(Time.timeSinceLevelLoad - Entity.CreationTimeSinceLevelLoad);
        }
        catch (NullReferenceException)
        {
          //Log.Out("(" + Config.ModPrefix + ") Entity not available. Something seems to be wrong here...");
        }
        clientInfo = null;
      }
    }

    public void SetOnline(ClientInfo ci)
    {
      //Log.Out("(" + Config.ModPrefix + ") Player set to online: " + steamId);
      clientInfo = ci;
      entityId = ci.entityId;
      name = ci.playerName;
      ip = ci.ip;
      lastOnline = DateTime.Now;
    }

    public void Update(PlayerDataFile _pdf)
    {
      expToNextLevel = _pdf.experience;
      level = _pdf.level;
      inventory.Update(_pdf);



      //string output = "";

      ////output += "_cInfo:" + JsonUtility.ToJson(_cInfo) + "\n";
      //output += "_pdf:" + JsonUtility.ToJson(_pdf) + "\n";

      //output += "ecd.stats.Health.Value:" + _pdf.ecd.stats.Health.Value + "\n";
      //output += "ecd.stats.Wellness.Value:" + _pdf.ecd.stats.Wellness.Value + "\n";
      //output += "ecd.stats.CoreTemp.Value:" + _pdf.ecd.stats.CoreTemp.Value + "\n";

      ////EntityPlayer _player = null;
      ////_pdf.ToPlayer(_player);
      ////output += "Health:" + _player.Health + "\n";
      ////output += "Stats.Health:" + _player.Stats.Health + "\n";


      //output += "spawnPoints:" + "\n" + "\n";
      //foreach (Vector3i sp in _pdf.spawnPoints)
      //{
      //  output += "    sp:" + sp.ToString() + "\n";
      //}

      //output += "quests:" + "\n" + "\n";
      //foreach (Quest q in _pdf.questJournal.quests)
      //{
      //  output += "    q:" + q.ID + "(" + q.CurrentState + ")" + "\n";
      //}

      //output += "waypoints:" + "\n";
      //foreach (Waypoint wp in _pdf.waypoints.List)
      //{
      //  output += "    wp:" + wp.pos + "\n";
      //}

      //output += "inventory:" + "\n";
      //foreach (ItemStack inv in _pdf.inventory)
      //{
      //  output += "    inv:" + inv + "\n";
      //}

      //output += "favoriteRecipeList:" + "\n";
      //foreach (string fr in _pdf.favoriteRecipeList)
      //{
      //  output += "    fr:" + fr + "\n";
      //}

      //output += "unlockedRecipeList:" + "\n";
      //foreach (string ur in _pdf.unlockedRecipeList)
      //{
      //  output += "    ur:" + ur + "\n";
      //}

      //output += "RecipeQueueItems:" + "\n";
      //foreach (RecipeQueueItem rqi in _pdf.craftingData.RecipeQueueItems)
      //{
      //  if (rqi.IsCrafting)
      //  {
      //    output += "    " + rqi.Recipe.GetName() + ": (" + rqi.Multiplier + ")" + rqi.CraftingTimeLeft + "\n";
      //  }
      //}

      //output += "equipment:" + "\n" + "\n";
      //foreach (ItemValue its in _pdf.equipment.GetItems())
      //{
      //  output += "    its:" + its + "\n";
      //}

      //output += "buffs:" + "\n" + "\n";
      //foreach (MultiBuff b in _pdf.ecd.stats.Buffs)
      //{
      //  output += "    " + b.Name + ":" + (b.Timer.TimeFraction * 100).ToString("0.0") + "\n";
      //}


      //Log.Out(output);



    }

    public Player(string steamId)
    {
      this.steamId = steamId;
      this.inventory = new Inventory();
    }


  }
}
