using System;
using System.Collections.Generic;

namespace BCM.PersistentData
{
  [Serializable]
  public class Players
  {
    private Dictionary<string, Player> players = new Dictionary<string, Player>();

    public Player this[string steamId, bool create]
    {
      get
      {
        if (string.IsNullOrEmpty(steamId))
        {
          return null;
        }
        else if (players.ContainsKey(steamId))
        {
          return players[steamId];
        }
        else
        {
          if (create && steamId != null && steamId.Length == 17)
          {
            Log.Out("" + Config.ModPrefix + " Created new player entry for ID: " + steamId);
            Player p = new Player(steamId);
            players.Add(steamId, p);
            return p;
          }
          return null;
        }
      }
    }

    public int Count
    {
      get { return players.Count; }
    }
  }
}
