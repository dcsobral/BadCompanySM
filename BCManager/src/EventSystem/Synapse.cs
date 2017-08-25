using BCM.Neurons;
using System;
using System.Collections.Generic;

namespace BCM
{
  public class Synapse
  {
    public string Name;
    public bool IsEnabled;
    public int Beats;
    public string Options;
    private int _lastfired;
    private readonly List<NeuronAbstract> _neurons = new List<NeuronAbstract>();

    public void WireNeurons()
    {
      // todo: change to use the config files to define which neurons should be wired for a synapse
      //todo: add a check for init before world awake, if set to false, then delay wiring until world has loaded
      switch (Name)
      {
        case "spawnmanager":
          _neurons.Add(new SpawnManager());
          break;
        case "entityspawnmutator":
          _neurons.Add(new EntitySpawnMutator(this));
          break;

        case "bagmonitor":
          _neurons.Add(new BagMonitor());
          break;
        case "buffmonitor":
          _neurons.Add(new BuffMonitor());
          break;
        case "deadisdead":
          _neurons.Add(new DeadIsDead());
          break;
        case "deathwatch":
          _neurons.Add(new DeathWatch());
          break;
        case "equipmentmonitor":
          _neurons.Add(new EquipmentMonitor());
          break;
        case "positiontracker":
          _neurons.Add(new PositionTracker());
          break;
        case "questmonitor":
          _neurons.Add(new QuestMonitor());
          break;
        case "toolbeltmonitor":
          _neurons.Add(new ToolbeltMonitor());
          break;

        case "pingkicker":
          _neurons.Add(new PingKicker());
          break;

        case "mapexplorer":
          _neurons.Add(new MapExplorer());
          break;
        case "saveworld":
          _neurons.Add(new SaveWorld());
          break;
        case "trashcollector":
          _neurons.Add(new TrashCollector());
          break;

        case "broadcastapi":
          _neurons.Add(new BroadcastAPI());
          break;

        case "motd":
          _neurons.Add(new Motd());
          break;

        default:
          Log.Out(Config.ModPrefix + " Unknown Synapse " + Name);
          break;
      }
    }

    public void FireNeurons(int b)
    {
      if (!IsEnabled || b < _lastfired + Beats) return;

      _lastfired = b;
      foreach (var n in _neurons)
      {
        try
        {
          n.Fire(b);
        }
        catch (Exception e)
        {
          Log.Out(Config.ModPrefix + " WARNING: Brain Damage detected trying to fire Neuron: " + n.GetType() + e);
        }
      }
    }
  }
}
