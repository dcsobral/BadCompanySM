using System.Linq;
using BCM.Neurons;
using BCM.PersistentData;

namespace BCM.Commands
{
  public class BCEvents : BCCommandAbstract
  {
    public override void Process()
    {
      if (GameManager.Instance.World == null)
      {
        SendOutput("The world isn't loaded");

        return;
      }

      if (Params.Count == 0)
      {
        SendOutput(GetHelp());

        return;
      }

      switch (Params[0])
      {
        case "persist":
          //todo: save current settings to system.xml file
          break;

        case "on":
          if (Heartbeat.IsAlive)
          {
            SendOutput("Heartbeat is already beating");

            return;
          }
          Heartbeat.IsAlive = true;
          Heartbeat.Start();
          SendOutput("Heatbeat started");
          break;

        case "off":
          Heartbeat.IsAlive = false;
          SendOutput("Heatbeat stopped");
          break;

        case "bpm":
          if (Params.Count < 2)
          {
            SendOutput($"Current Bpm is: {Heartbeat.Bpm}");

            return;
          }

          if (!int.TryParse(Params[1], out var bpm))
          {
            SendOutput($"Unable to parse bpm from: {Params[1]}");

            return;
          }

          if (bpm > 300)
          {
            SendOutput($"Woah, {bpm} is a bit high, are you trying to give the server a heartattack!? Max is 300");

            return;
          }

          if (bpm <= 0)
          {
            SendOutput("Check your pulse, you might be dead! Bpm must be greater than 0");

            return;
          }

          Heartbeat.Bpm = bpm;
          SendOutput($"Bpm now set to {bpm}");

          break;

        case "state":
        case "toggle":
        case "disable":
        case "enable":
          {
            if (Params.Count != 2)
            {
              SendOutput("Select an event neuron to view/alter the state, * for all");
              SendOutput(string.Join(",", Brain.Synapses.Select(s => s.Name).ToArray()));

              return;
            }

            if (Params[1] == "*")
            {
              foreach (var s in Brain.Synapses)
              {
                ProcessSynapseState(s);
              }

              return;
            }

            var synapse = Brain.GetSynapse(Params[1]);
            ProcessSynapseState(synapse);

            return;
          }

        case "deadisdead":
          ConfigDeadIsDead();
          return;

        case "pingkicker":
          ConfigPingKicker();
          return;

        case "tracker":
          ConfigTracker();
          return;

        case "spawnmutator":
          ConfigSpawnMutator();
          return;

        case "spawnmanager":
          ConfigSpawnManager();
          return;

        case "logcache":
          ConfigLogCache();
          return;

        default:
          SendOutput($"Unknown neuron name {Params[0]}");
          return;
      }
    }

    private void ProcessSynapseState(Synapse synapse)
    {
      if (synapse == null)
      {
        SendOutput($"Unknown synapse {Params[1]}");

        return;
      }

      switch (Params[0])
      {
        case "state":
          break;
        case "toggle":
          synapse.IsEnabled = !synapse.IsEnabled;
          break;
        case "disable":
          synapse.IsEnabled = false;
          break;
        case "enable":
          synapse.IsEnabled = true;
          break;
      }

      SendOutput($"Synapse {synapse.Name} is {(synapse.IsEnabled ? "Enabled" : "Disabled")}");
    }

    public void ConfigLogCache()
    {
      if (Params.Count > 1)
      {
        var neuron = Brain.GetSynapseNeurons("logcache")?.OfType<LogCache>().FirstOrDefault();
        if (neuron == null)
        {
          SendOutput("Unable to access logcache neuron");

          return;
        }

        switch (Params[1])
        {
          case "list":
            {
              if (Params.Count > 2)
              {
                switch (Params[2])
                {
                  case "chat":
                    SendJson(neuron.GetChatEntries());

                    return;
                  case "gmsg":
                    SendJson(neuron.GetGmsgEntries());

                    return;
                  case "bcm":
                    SendJson(neuron.GetBcmEntries());

                    return;
                  case "error":
                    SendJson(neuron.GetErrorEntries());

                    return;
                }
              }
              else
              {
                SendJson(neuron.GetAllEntries());
              }

              return;
            }

          case "config":
            {
              var neuronConfig = PersistentContainer.Instance.EventsConfig["logcache", false];
              if (neuronConfig != null)
              {
                SendJson(neuronConfig.Settings);
              }

              return;
            }

          default:
            SendOutput("Invalid sub command");

            return;
        }
      }

      SendOutput("LogCache sub commands:");
      SendOutput("list,config");
    }

    public void ConfigSpawnMutator()
    {
      SendOutput("Under Development");
    }

    public void ConfigSpawnManager()
    {
      SendOutput("Under Development");
    }

    public void ConfigDeadIsDead()
    {
      if (Params.Count > 1)
      {
        var neuron = Brain.GetSynapseNeurons("deadisdead")?.OfType<DeadIsDead>().FirstOrDefault();
        if (neuron == null)
        {
          SendOutput("Unable to access deadisdead neuron");

          return;
        }

        switch (Params[1])
        {
          case "on":
            {
              if (Params.Count != 3)
              {
                SendOutput("deadisdead on requires a steamId. Use 'me' as the id to target yourself.");

                return;
              }
              var steamId = Params[2];
              if (steamId == "me" && SenderInfo.RemoteClientInfo != null)
              {
                steamId = SenderInfo.RemoteClientInfo.playerId;
              }
              neuron.EnableMode(steamId);
              SendOutput($"Dead is Dead enabled on player {steamId}");

              return;
            }
          case "off":
            {
              if (Params.Count != 3)
              {
                SendOutput("deadisdead off requires a steamId. Use 'me' as the id to target yourself.");

                return;
              }
              var steamId = Params[2];
              if (steamId == "me" && SenderInfo.RemoteClientInfo != null)
              {
                steamId = SenderInfo.RemoteClientInfo.playerId;
              }
              neuron.DisableMode(steamId);
              SendOutput($"Dead is Dead disabled on player {steamId}");

              return;
            }
          case "global":
            {
              bool? mode = null;
              if (Params.Count == 3 && bool.TryParse(Params[2], out var m)) mode = m;
              neuron.ToggleGlobal(mode);
              SendOutput($"Global mode set to {neuron.GlobalMode}");

              return;
            }
          case "config":
            {
              SendJson(new { Global = neuron.GlobalMode, Players = neuron.DiDModePlayers });

              return;
            }
          case "restore":
            {
              SendOutput("Work in progress.");

              return;
            }
          case "delete":
            {
              if (Params.Count != 3)
              {
                SendOutput("deadisdead delete requires a steamId");

                return;
              }
              var steamId = Params[2];
              if (steamId == "me" && SenderInfo.RemoteClientInfo != null)
              {
                steamId = SenderInfo.RemoteClientInfo.playerId;
              }
              neuron.BackupAndDelete(steamId);

              return;
            }
          case "backup":
            {
              if (Params.Count != 3)
              {
                SendOutput("deadisdead delete requires a steamId");

                return;
              }
              var steamId = Params[2];
              if (steamId == "me" && SenderInfo.RemoteClientInfo != null)
              {
                steamId = SenderInfo.RemoteClientInfo.playerId;
              }
              neuron.BackupAndDelete(steamId, false);

              return;
            }

          default:
            SendOutput("Invalid sub command");

            return;
        }
      }

      SendOutput("DeadIsDead sub commands:");
      SendOutput("on,off,global,config,restore,delete,backup");
    }

    public void ConfigTracker()
    {
      if (Params.Count > 1)
      {
        var neuron = Brain.GetSynapseNeurons("tracker")?.OfType<PositionTracker>().FirstOrDefault();
        if (neuron == null)
        {
          SendOutput("Unable to access tracker neuron");

          return;
        }

        switch (Params[1])
        {
          case "view":
            {
              if (Params.Count > 2)
              {
                var steamId = Params[2];
                if (steamId.Length != 17)
                {
                  SendOutput("Invalid steam id");

                  return;
                }

                var player = PersistentContainer.Instance.PlayerLogs[steamId, false];
                if (player == null)
                {
                  SendOutput($"Unable to load player information with steam id {steamId}");

                  return;
                }

                SendJson(player);
              }
              else
              {
                var keys = PersistentContainer.Instance.PlayerLogs.AllKeys().ToList();
                if (!keys.Any())
                {
                  SendOutput("No log info found");

                  return;
                }

                SendJson(keys);
              }

              return;
            }

          default:
            SendOutput("Invalid sub command");

            return;
        }
      }

      SendOutput("Tracker sub commands:");
      SendOutput("view");
    }

    public void ConfigPingKicker()
    {
      if (Params.Count > 1)
      {
        var neuron = Brain.GetSynapseNeurons("pingkicker")?.OfType<PingKicker>().FirstOrDefault();
        if (neuron == null)
        {
          SendOutput("Unable to access ping kicker neuron");

          return;
        }

        switch (Params[1])
        {
          case "add":
            {
              if (Params.Count <= 2 || Params[2].Length != 17) return;

              var steamId = Params[2];
              neuron.WhitelistPlayer(steamId);
              neuron.ClearPlayer(steamId);
              SendOutput($"SteamId {steamId} added to whitelist");

              return;
            }

          case "remove":
            {
              if (Params.Count <= 2 || Params[2].Length != 17) return;

              var steamId = Params[2];
              neuron.WhitelistPlayer(steamId, false);
              SendOutput($"SteamId {steamId} removed from whitelist");

              return;
            }

          case "limit":
            {
              if (Params.Count <= 2 || !int.TryParse(Params[2], out var limit)) return;

              neuron.SetLimit(limit);
              SendOutput($"Ping limit set to {limit}");

              return;
            }

          case "beats":
            {
              if (Params.Count <= 2 || !int.TryParse(Params[2], out var beats)) return;

              neuron.SetBeats(beats);
              SendOutput($"Beats before kick set to {beats}");

              return;
            }

          case "watchlist":
            {
              SendJson(neuron.GetWatchlist());

              return;
            }

          case "whitelist":
            {
              var neuronConfig = PersistentContainer.Instance.EventsConfig["pingkicker", false];
              if (neuronConfig != null)
              {
                SendJson(neuronConfig.Settings["Whitelist"]);
              }

              return;
            }

          case "config":
            {
              var neuronConfig = PersistentContainer.Instance.EventsConfig["pingkicker", false];
              if (neuronConfig != null)
              {
                SendJson(neuronConfig.Settings);
              }

              return;
            }

          case "clearcache":
            {
              neuron.ClearCache();

              return;
            }

          case "clearwhitelist":
            {
              neuron.ClearWhitelist();

              return;
            }

          default:
            SendOutput("Invalid sub command");

            return;
        }
      }

      SendOutput("Ping Kicker sub commands:");
      SendOutput("add, remove, limit, beats, watchlist, whitelist, config, clearcache, clearwhitelist");
    }
  }
}
