using BCM.Commands;
using System;
using System.Collections.Generic;

namespace BCM.Neurons
{
  public class PingKicker : NeuronAbstract
  {
    private Dictionary<string, int> pingCache = new Dictionary<string, int>();
    private List<string> whiteList = new List<string>();
    private int limitThreshhold = 200;
    private int beatsBeforeKick = 30;

    public PingKicker()
    {
    }

    public override bool Fire(int b)
    {
      if (!Steam.Network.IsServer)
      {
        Log.Out(Config.ModPrefix + "Can't kick players for high ping. Not a network server");
        return false;
      }

      //todo: add checks for settings in persistent data and override defaults with it
      var clients = ConnectionManager.Instance.GetClients();

      foreach (var client in clients)
      {
        if (client.ownerId != null && whiteList.Contains(client.ownerId))
        {
          if (client.loginDone && client.ping > limitThreshhold)
          {
            if (!pingCache.ContainsKey(client.ownerId))
            {
              pingCache[client.ownerId] = beatsBeforeKick;
            }
            else
            {
              pingCache[client.ownerId] -= 1;
            }

          }
        }
      }

      //todo: check player pings vs limit and whilelists, kick any that fail test
      foreach (var c in pingCache)
      {
        if (c.Value < 0)
        {
          //do kick
        }
      }

      return true;
    }
  }
}
