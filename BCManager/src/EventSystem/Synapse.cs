using BCM.Neurons;
using System;
using System.Collections.Generic;
using System.Linq;
using BCM.Models;
using BCM.PersistentData;

namespace BCM
{
  public class Synapse
  {
    public string Name;
    public bool IsEnabled;
    public int Beats = -1;
    public string Options;
    public string Cfg;
    private int _lastfired;
    private readonly List<NeuronAbstract> _neurons = new List<NeuronAbstract>();

    public void WireNeurons()
    {
      // todo: change to use the config files to define which neurons should be wired for a synapse
      //todo: add a check for init before world awake, if set to false, then delay wiring until world has loaded
      switch (Name)
      {
        case "spawnmanager":
          _neurons.Add(new SpawnManager(this));
          break;
        case "spawnmutator":
          _neurons.Add(new EntitySpawnMutator(this));
          break;

        case "logcache":
          _neurons.Add(new LogCache(this));
          break;

        case "tracker":
          _neurons.Add(new PositionTracker(this));
          break;
        case "pingkicker":
          _neurons.Add(new PingKicker(this));
          break;

        case "deadisdead":
          _neurons.Add(new DeadIsDead(this));
          break;
        case "deathwatch":
          _neurons.Add(new DeathWatch(this));
          break;

        case "bagmonitor":
          _neurons.Add(new BagMonitor(this));
          break;
        case "toolbeltmonitor":
          _neurons.Add(new ToolbeltMonitor(this));
          break;
        case "equipmentmonitor":
          _neurons.Add(new EquipmentMonitor(this));
          break;
        case "buffmonitor":
          _neurons.Add(new BuffMonitor(this));
          break;
        case "questmonitor":
          _neurons.Add(new QuestMonitor(this));
          break;

        case "mapexplorer":
          _neurons.Add(new MapExplorer(this));
          break;
        case "saveworld":
          _neurons.Add(new SaveWorld(this));
          break;
        //case "trashcollector":
        //  _neurons.Add(new TrashCollector());
        //  break;

        case "broadcastapi":
          _neurons.Add(new BroadcastAPI(this));
          break;

        case "motd":
          _neurons.Add(new Motd(this));
          break;

        default:
          Log.Out($"{Config.ModPrefix} Unknown Synapse {Name}");
          break;
      }
    }

    public List<NeuronAbstract> GetNeurons()
    {
      return _neurons;
    }

    public void FireNeurons(int b)
    {
      if (!IsEnabled || Beats == -1 || b < _lastfired + Beats) return;

      _lastfired = b;
      foreach (var n in _neurons)
      {
        try
        {
          n.Fire(b);
        }
        catch (Exception e)
        {
          Log.Out($"{Config.ModPrefix} Brain Damage detected trying to fire Neuron {n.GetType()}:\n{e}");
        }
      }
    }

    public static void PlayerTeleported(ClientInfo cInfo, Vector3i pos)
    {
      //check for allowed to tp, or command issued to allow tp
    }

    public static void ReturnPlayer(ClientInfo cInfo, Vector3i pos)
    {
      //returning player
    }

    public static void NewPlayer(ClientInfo cInfo, Vector3i pos)
    {
      //new player
      //todo: add a teleport to defined spawn location (randomly if more than one), can be 2points and use top ground pos for y
      //todo: create a task and run through a scripted sequence such as display chat messages before a 5 sec delayed teleport, etc
    }

    public static void DeadIsDead(ClientInfo cInfo)
    {
      var synapse = Brain.GetSynapse("deadisdead");
      if (synapse == null || !synapse.IsEnabled) return;

      var neuron = synapse.GetNeurons().OfType<DeadIsDead>().FirstOrDefault();
      if (neuron == null)
      {
        Log.Out($"{Config.ModPrefix} Unable to load neuron for dead is dead mode");

        return;
      }
      if (neuron.GlobalMode || neuron.DiDModePlayers.Contains(cInfo.playerId))
      {
        Log.Out($"{Config.ModPrefix} Player kicked and archived for DiD mode: {cInfo.playerId}/{neuron.BackupAndDelete(cInfo.playerId)} - {cInfo.playerName}");
      }
    }

    public static void PlayerTracker(ClientInfo cInfo, RespawnType respawnReason, bool create = true)
    {
      var synapse = Brain.GetSynapse("tracker");
      if (synapse == null || !synapse.IsEnabled) return;

      var world = GameManager.Instance.World;
      if (world == null) return;

      var playerlog = PersistentContainer.Instance.PlayerLogs[cInfo.playerId, create];
      if (playerlog == null) return;

      var ts = $"{DateTime.UtcNow:yyyy-MM-dd_HH_mm_ss.fffZ}";
      if (world.Players.dict.ContainsKey(cInfo.entityId))
      {
        var ep = world.Players.dict[cInfo.entityId];
        if (!playerlog.LogDataCache.ContainsKey(ts) && ep != null)
        {
          playerlog.LogDataCache.Add(ts,
            new LogData(new BCMVector4(ep.position, (int)Math.Floor(ep.rotation.y)),
              $"{(respawnReason == RespawnType.Unknown ? "DISCONNECT" : $"SPAWN: {respawnReason}")}"));
        }
      }
      PersistentContainer.Instance.Save("logs");
    }
  }
}
