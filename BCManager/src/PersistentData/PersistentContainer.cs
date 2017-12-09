using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using JetBrains.Annotations;

namespace BCM.PersistentData
{
  [Serializable]
  public class PersistentContainer
  {
    private static PersistentContainer _instance;
    private Players _players;
    [OptionalField]
    private BCMSettings _settings;
    [OptionalField]
    private BCMPlayerLogs _playerlogs;
    [OptionalField]
    private BCMEventsConfig _eventsConfig;

    private static readonly char _s = Path.DirectorySeparatorChar;
    private static readonly string _playersfile = $"{GameUtils.GetSaveGameDir()}{_s}BCMData{_s}Players.bin";
    private static readonly string _settingsfile = $"{GameUtils.GetSaveGameDir()}{_s}BCMData{_s}Settings.bin";
    private static readonly string _playerlogsfile = $"{GameUtils.GetSaveGameDir()}{_s}BCMData{_s}PlayerLogs{_s}log_{DateTime.UtcNow:yyyy-MM-dd_HH_mm_ss}.bin";
    private static readonly string _eventsconfigsfile = $"{GameUtils.GetSaveGameDir()}{_s}BCMData{_s}EventsConfig.bin";
    private static readonly BinaryFormatter bFormatter = new BinaryFormatter();

    private PersistentContainer()
    {
      Directory.CreateDirectory($"{GameUtils.GetSaveGameDir()}{_s}BCMData{_s}");
      Directory.CreateDirectory($"{GameUtils.GetSaveGameDir()}{_s}BCMData{_s}PlayerLogs");
    }

    [NotNull]
    public Players Players => _players ?? (_players = new Players());

    [NotNull]
    public BCMSettings Settings => _settings ?? (_settings = new BCMSettings());

    [NotNull]
    public BCMPlayerLogs PlayerLogs => _playerlogs ?? (_playerlogs = new BCMPlayerLogs());

    [NotNull]
    public BCMEventsConfig EventsConfig => _eventsConfig ?? (_eventsConfig = new BCMEventsConfig());

    [NotNull]
    public static PersistentContainer Instance => _instance ?? (_instance = new PersistentContainer());

    public static bool IsLoaded => _instance != null;

    public void Save(string bin)
    {
      try
      {
        if ((bin == null || bin == "players") && Players.Count > 0)
        {
          Stream streamPlayers = File.Open(_playersfile, FileMode.Create);
          bFormatter.Serialize(streamPlayers, _players);
          streamPlayers.Close();
          Log.Out($"{Config.ModPrefix} Players Saved");
        }

        if ((bin == null || bin == "settings") && _settings != null)
        {
          Stream streamSettings = File.Open(_settingsfile, FileMode.Create);
          bFormatter.Serialize(streamSettings, _settings);
          streamSettings.Close();
          //Log.Out($"{Config.ModPrefix} Settings Saved");
        }

        if ((bin == null || bin == "logs") && _playerlogs != null)
        {
          Stream streamPlayerLogs = File.Open(_playerlogsfile, FileMode.Create);
          bFormatter.Serialize(streamPlayerLogs, _playerlogs);
          streamPlayerLogs.Close();
          //Log.Out($"{Config.ModPrefix} Player Logs Saved");
        }

        if ((bin == null || bin == "events") && _eventsConfig != null)
        {
          Stream streamEventsConfig = File.Open(_eventsconfigsfile, FileMode.Create);
          bFormatter.Serialize(streamEventsConfig, _eventsConfig);
          streamEventsConfig.Close();
          //Log.Out($"{Config.ModPrefix} Events Config Saved");
        }
      }
      catch (Exception e)
      {
        Log.Error($"{Config.ModPrefix} Error in PersistentContainer.{MethodBase.GetCurrentMethod().Name}: {e}");
      }
    }

    public static bool Load()
    {
      try
      {
        var obj = new PersistentContainer();

        //PLAYERS
        Stream streamPlayers = File.Open(_playersfile, FileMode.OpenOrCreate);
        if (streamPlayers.Length > 0)
        {
          obj._players = (Players)bFormatter.Deserialize(streamPlayers);
        }
        streamPlayers.Close();

        //SETTINGS
        Stream streamSettings = File.Open(_settingsfile, FileMode.OpenOrCreate);
        if (streamSettings.Length > 0)
        {
          obj._settings = (BCMSettings)bFormatter.Deserialize(streamSettings);
        }
        streamSettings.Close();

        //PLAYER LOGS
        Stream streamPlayerLogs = File.Open(_playerlogsfile, FileMode.Create);
        streamPlayerLogs.Close();

        //EVENTS CONFIG
        Stream streamEventsConfig = File.Open(_eventsconfigsfile, FileMode.OpenOrCreate);
        if (streamEventsConfig.Length > 0)
        {
          obj._eventsConfig = (BCMEventsConfig)bFormatter.Deserialize(streamEventsConfig);
        }
        streamEventsConfig.Close();

        _instance = obj;
        return true;
      }
      catch (Exception e)
      {
        Log.Error($"{Config.ModPrefix} Error in PersistentContainer.{MethodBase.GetCurrentMethod().Name}: {e}");
      }

      return false;
    }
  }
}
