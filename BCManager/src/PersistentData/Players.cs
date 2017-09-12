using System;
using System.Collections.Generic;

namespace BCM.PersistentData
{
  [Serializable]
  public class Players
  {
    private readonly Dictionary<string, Player> _players = new Dictionary<string, Player>();

    public Player this[string steamId, bool create]
    {
      get
      {
        if (string.IsNullOrEmpty(steamId))
        {
          return null;
        }
        if (_players.ContainsKey(steamId))
        {
          return _players[steamId];
        }
        if (!create || steamId.Length != 17) return null;
        Log.Out($"{Config.ModPrefix} Created new player entry for ID: {steamId}");
        var p = new Player(steamId);
        _players.Add(steamId, p);
        return p;
      }
    }

    public int Count => _players.Count;
  }
}
