using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BCM.Neurons
{
  public class DeadIsDead : NeuronAbstract
  {
    private static readonly char _s = Path.DirectorySeparatorChar;
    private readonly string _dir = $"{GameUtils.GetPlayerDataDir()}{_s}";

    public bool GlobalMode;
    public List<string> DiDModePlayers = new List<string>();

    public DeadIsDead(Synapse s) : base (s)
    {
      //todo: load config xml, load saved config
    }

    public override void Awake()
    {
      if (synapse.IsEnabled)
      {
        Log.Out($"{Config.ModPrefix} Dead Is Dead Initialised");
      }
    }

    public bool ToggleGlobal(bool? mode = null)
    {
      return GlobalMode = mode ?? !GlobalMode;
    }

    public void EnableMode(string steamId)
    {
      if (steamId.Length != 17 || DiDModePlayers.Contains(steamId)) return;

      DiDModePlayers.Add(steamId);
    }
    public bool DisableMode(string steamId)
    {
      return DiDModePlayers.Remove(steamId);
    }

    public string BackupAndDelete(string steamId, bool kickdelete = true)
    {
      if (kickdelete)
      {
        var cInfo = ConnectionManager.Instance.GetClients().FirstOrDefault(c => c.playerId == steamId);
        if (cInfo != null)
        {
          GameUtils.KickPlayerForClientInfo(cInfo, new GameUtils.KickPlayerData(GameUtils.EKickReason.ManualKick, 0, default(DateTime), "You Died! (with Dead is Dead), rejoin to start again..."));
        }
      }

      //does this need a delay for slow servers to let kicked player info save first? Check last saved stat?
      var ttpFile = $"{_dir}{steamId}.ttp";
      if (!File.Exists(ttpFile)) return "PlayerFileNotFound";

      var backupDir = $"{_dir}BCMBackups{_s}{steamId}{_s}";
      Directory.CreateDirectory(backupDir);

      var timestamp = $"{DateTime.UtcNow:yyyy-MM-dd_HH_mm_ss.fffZ}";
      var mapFile = _dir + steamId + ".map";
      var bakFile = _dir + steamId + ".ttp.bak";

      if (kickdelete)
      {
        File.Move(ttpFile, $"{backupDir}{timestamp}.ttp");

        if (File.Exists(mapFile))
        {
          File.Move(mapFile, $"{backupDir}{timestamp}.map");
        }

        if (File.Exists(bakFile))
        {
          File.Move(bakFile, $"{backupDir}{timestamp}.ttp.bak");
        }

        //todo: remove lcbs from persistent data
        //todo: remove ownership of chests (would require loading chunks)?
      }
      else
      {
        File.Copy(ttpFile, $"{backupDir}{timestamp}.ttp");

        if (File.Exists(mapFile))
        {
          File.Copy(mapFile, $"{backupDir}{timestamp}.map");
        }

        if (File.Exists(bakFile))
        {
          File.Copy(bakFile, $"{backupDir}{timestamp}.ttp.bak");
        }
      }

      return timestamp;
    }

    public override void Fire(int b)
    {
    }
  }
}

