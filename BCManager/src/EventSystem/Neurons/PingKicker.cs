using System.Collections.Generic;

namespace BCM.Neurons
{
  public class PingKicker : NeuronAbstract
  {
    public List<string> WhiteList { get; }

    private readonly Dictionary<string, int> _pingCache;
    private readonly int _limitThreshhold;
    private readonly int _beatsBeforeKick;

    public PingKicker()
    {
      //todo: commands to override these settings, or pull them in from config
      //todo: command to add playes to whitelist

      _pingCache = new Dictionary<string, int>();
      WhiteList = new List<string>();
      _limitThreshhold = 200;
      _beatsBeforeKick = 30;
    }

    public override void Fire(int b)
    {
      if (!Steam.Network.IsServer)
      {
        Log.Out($"{Config.ModPrefix}Can\'t kick players for high ping. Not a network server");

        return;
      }

      //todo: add checks for settings in persistent data and override defaults with it
      var clients = ConnectionManager.Instance.GetClients();

      foreach (var client in clients)
      {
        if (client.ownerId == null || !WhiteList.Contains(client.ownerId)) continue;

        if (!client.loginDone || client.ping <= _limitThreshhold) continue;

        if (!_pingCache.ContainsKey(client.ownerId))
        {
          _pingCache[client.ownerId] = _beatsBeforeKick;
        }
        else
        {
          _pingCache[client.ownerId] -= 1;
        }
      }

      //todo: check player pings vs limit and whilelists, kick any that fail test
      foreach (var c in _pingCache)
      {
        if (c.Value < 0)
        {
          //do kick

        }
      }
    }
  }
}
