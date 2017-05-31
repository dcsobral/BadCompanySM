using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace BCM.PersistentData
{
  [Serializable]
  public class PersistentContainer
  {
    private static PersistentContainer instance;
    private Players players;
    [OptionalField]
    private BCMSettings settings;

    private PersistentContainer()
    {
    }

    public Players Players
    {
      get
      {
        if (players == null)
          players = new Players();
        return players;
      }
    }

    public BCMSettings Settings
    {
      get
      {
        if (settings == null)
        {
          settings = new BCMSettings();
        }
        return settings;
      }
    }

    public static PersistentContainer Instance
    {
      get
      {
        if (instance == null)
        {
          instance = new PersistentContainer();
        }
        return instance;
      }
    }
    public static bool IsLoaded
    {
      get
      {
        return instance != null;
      }
    }

    public void Save()
    {
      try
      {

        Directory.CreateDirectory(GameUtils.GetSaveGameDir() + "/BCM/");
        BinaryFormatter bFormatter = new BinaryFormatter();

        if (Players.Count > 0)
        {
          // todo: create a keys database, move older players to a file that isnt loaded to reduce memory used, default /offline commands to recent players (claim expiry x 4?) and use /archives to get older data
          Stream streamPlayers = File.Open(GameUtils.GetSaveGameDir() + "/BCM/Players.bin", FileMode.Create);
          bFormatter.Serialize(streamPlayers, players);
          streamPlayers.Close();
          Log.Out(Config.ModPrefix + " Players Saved");
        }
        else
        {
          Log.Out(Config.ModPrefix + " No Players to Save");
        }

        if (settings != null)
        {
          Stream streamSettings = File.Open(GameUtils.GetSaveGameDir() + "/BCM/Settings.bin", FileMode.Create);
          bFormatter.Serialize(streamSettings, settings);
          streamSettings.Close();
          Log.Out(Config.ModPrefix + " Settings Saved");
        }
        else
        {
          Log.Out(Config.ModPrefix + " No Mod Settings to Save");
        }
      }
      catch (Exception e)
      {
        Log.Error("" + Config.ModPrefix + " Error in PersistentContainer." + MethodBase.GetCurrentMethod().Name + ": " + e);
      }
    }

    public static bool Load()
    {
      try
      {
        PersistentContainer obj = new PersistentContainer();
        BinaryFormatter bFormatter = new BinaryFormatter();

        Directory.CreateDirectory(GameUtils.GetSaveGameDir() + "/BCM/");

        // todo: add files for players stats, position, buff, inventory histories 
        Stream streamPlayers = File.Open(GameUtils.GetSaveGameDir() + "/BCM/Players.bin", FileMode.OpenOrCreate);
        if (streamPlayers.Length > 0)
        {
          obj.players = (Players)bFormatter.Deserialize(streamPlayers);
        }
        else
        {
          Log.Out(Config.ModPrefix + " No Players to Load");
        }
        streamPlayers.Close();

        Stream streamSettings = File.Open(GameUtils.GetSaveGameDir() + "/BCM/Settings.bin", FileMode.OpenOrCreate);
        if (streamSettings.Length > 0)
        {
          obj.settings = (BCMSettings)bFormatter.Deserialize(streamSettings);
        }
        else
        {
          Log.Out(Config.ModPrefix + " No Mod Settings to Load");
        }
        streamSettings.Close();

        instance = obj;
        return true;
      }
      catch (Exception e)
      {
        Log.Error("" + Config.ModPrefix + " Error in PersistentContainer." + MethodBase.GetCurrentMethod().Name + ": " + e);
      }
      return false;
    }

  }
}
