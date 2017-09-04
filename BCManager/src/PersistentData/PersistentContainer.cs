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
    private static PersistentContainer _instance;
    private Players _players;
    [OptionalField]
    private BCMSettings _settings;

    private PersistentContainer()
    {
    }

    public Players Players => _players ?? (_players = new Players());

    public BCMSettings Settings => _settings ?? (_settings = new BCMSettings());

    public static PersistentContainer Instance => _instance ?? (_instance = new PersistentContainer());

    public static bool IsLoaded => _instance != null;

    public void Save()
    {
      try
      {

        Directory.CreateDirectory(GameUtils.GetSaveGameDir() + "/BCMData/");
        var bFormatter = new BinaryFormatter();

        if (Players.Count > 0)
        {
          Stream streamPlayers = File.Open(GameUtils.GetSaveGameDir() + "/BCMData/Players.bin", FileMode.Create);
          bFormatter.Serialize(streamPlayers, _players);
          streamPlayers.Close();
          Log.Out(Config.ModPrefix + " Players Saved");
        }

        if (_settings != null)
        {
          Stream streamSettings = File.Open(GameUtils.GetSaveGameDir() + "/BCMData/Settings.bin", FileMode.Create);
          bFormatter.Serialize(streamSettings, _settings);
          streamSettings.Close();
          Log.Out(Config.ModPrefix + " Settings Saved");
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
        Directory.CreateDirectory(GameUtils.GetSaveGameDir() + "/BCMData/");

        var obj = new PersistentContainer();
        var bFormatter = new BinaryFormatter();

        // todo: add files for players stats, position, buff, inventory histories 
        Stream streamPlayers = File.Open(GameUtils.GetSaveGameDir() + "/BCMData/Players.bin", FileMode.OpenOrCreate);
        if (streamPlayers.Length > 0)
        {
          obj._players = (Players)bFormatter.Deserialize(streamPlayers);
        }
        streamPlayers.Close();

        Stream streamSettings = File.Open(GameUtils.GetSaveGameDir() + "/BCMData/Settings.bin", FileMode.OpenOrCreate);
        if (streamSettings.Length > 0)
        {
          obj._settings = (BCMSettings)bFormatter.Deserialize(streamSettings);
        }
        streamSettings.Close();

        _instance = obj;

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
