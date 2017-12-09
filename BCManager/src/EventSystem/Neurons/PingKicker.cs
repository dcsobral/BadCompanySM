using System;
using System.Collections.Generic;
using BCM.PersistentData;
using UnityEngine;

namespace BCM.Neurons
{
  public class PingKicker : NeuronAbstract
  {
    private readonly Dictionary<string, int> _pingCache;
    private int _limitThreshhold;
    private int _beatsBeforeKick;

    public PingKicker(Synapse s) : base(s)
    {
      _pingCache = new Dictionary<string, int>();

      var neuronConfig = PersistentContainer.Instance.EventsConfig["pingkicker", true];
      if (neuronConfig == null) return;

      _limitThreshhold = 250;
      _beatsBeforeKick = 30;

      foreach (var kvp in JsonUtility.FromJson<Dictionary<string, int>>(synapse.Options))
      {
        switch (kvp.Key)
        {
          case "threshhold":
            _limitThreshhold = kvp.Value;
            break;
          case "count":
            _beatsBeforeKick = kvp.Value;
            break;
          default:
            //unknown property
            Log.Out($"{Config.ModPrefix} Unknown property {kvp.Key}");
            break;
        }
      }

      if (!neuronConfig.Settings.ContainsKey("Whitelist"))
      {
        neuronConfig.SetItem("Whitelist", new List<string>());
      }

      if (neuronConfig.Settings.ContainsKey("Threshhold"))
      {
        int.TryParse(neuronConfig.Settings["Threshhold"].ToString(), out _limitThreshhold);
      }

      if (neuronConfig.Settings.ContainsKey("BeatsBeforeKick"))
      {
        int.TryParse(neuronConfig.Settings["BeatsBeforeKick"].ToString(), out _beatsBeforeKick);
      }

      PersistentContainer.Instance.Save("events");
    }

    public Dictionary<string, int> GetWatchlist()
    {
      return _pingCache;
      //return _pingCache.Where(p => p.Value < _beatsBeforeKick).ToDictionary(p => p.Key, p=> p.Value);
    }

    public void ClearPlayer(string steamId)
    {
      _pingCache.Remove(steamId);
    }

    public void ClearCache()
    {
      _pingCache.Clear();
    }

    public void ClearWhitelist()
    {
      var neuronConfig = PersistentContainer.Instance.EventsConfig["pingkicker", false];
      if (neuronConfig != null) neuronConfig.Settings["Whitelist"] = new List<string>();
      PersistentContainer.Instance.Save("events");
    }

    public void SetLimit(int limit)
    {
      var neuronConfig = PersistentContainer.Instance.EventsConfig["pingkicker", false];
      if (neuronConfig == null) return;

      neuronConfig.Settings["Threshhold"] = limit;
      _limitThreshhold = limit;
      PersistentContainer.Instance.Save("events");
    }

    public void SetBeats(int beats)
    {
      var neuronConfig = PersistentContainer.Instance.EventsConfig["pingkicker", false];
      if (neuronConfig == null) return;

      neuronConfig.Settings["BeatsBeforeKick"] = beats;
      _beatsBeforeKick = beats;
      PersistentContainer.Instance.Save("events");
    }

    public void WhitelistPlayer(string steamId, bool addPlayer = true)
    {
      var neuronConfig = PersistentContainer.Instance.EventsConfig["pingkicker", false];
      if (neuronConfig == null) return;

      if (!(neuronConfig.Settings["Whitelist"] is List<string> whitelist)) return;

      if (!whitelist.Contains(steamId) && addPlayer)
      {
        whitelist.Add(steamId);
      }
      if (whitelist.Contains(steamId) && !addPlayer)
      {
        whitelist.Remove(steamId);
      }
      PersistentContainer.Instance.Save("events");
    }

    public override void Fire(int b)
    {
      var neuronConfig = PersistentContainer.Instance.EventsConfig["pingkicker", false];
      if (neuronConfig == null) return;

      if (!(neuronConfig.Settings["Whitelist"] is List<string> whitelist)) return;

      if (ConnectionManager.Instance.ClientCount() == 0) return;

      var clients = ConnectionManager.Instance.GetClients();

      if (clients == null) return;

      foreach (var client in clients)
      {
        if (client.playerId == null || whitelist.Contains(client.playerId) || !client.loginDone) continue;

        if (client.ping <= _limitThreshhold)
        {
          //Reset
          _pingCache[client.playerId] = _beatsBeforeKick;

          continue;
        }

        if (!_pingCache.ContainsKey(client.playerId))
        {
          //New
          _pingCache[client.playerId] = _beatsBeforeKick;
        }
        else
        {
          //Decrease Counter
          _pingCache[client.playerId] -= 1;
        }
      }

      foreach (var c in _pingCache)
      {
        if (c.Value > 0) continue;

        //do kick
        var _ci = ConnectionManager.Instance.GetClientInfoForPlayerId(c.Key);
        if (_ci == null) continue;

        GameUtils.KickPlayerForClientInfo(_ci, new GameUtils.KickPlayerData(GameUtils.EKickReason.ManualKick, 0, default(DateTime), "You have been kicked for persistent high ping"));
        _pingCache.Remove(c.Key);
      }
    }
  }
}
