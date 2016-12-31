using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

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
            Log.Out("(" + Config.ModPrefix + ") Created new player entry for ID: " + steamId);
            Player p = new Player(steamId);
            players.Add(steamId, p);
            return p;
          }
          return null;
        }
      }
    }

    public List<string> SteamIDs
    {
      get { return new List<string>(players.Keys); }
    }

    public int Count
    {
      get { return players.Count; }
    }

    //		public Player GetPlayerByNameOrId (string _nameOrId, bool _ignoreColorCodes)
    //		{
    //			string sid = GetSteamID (_nameOrId, _ignoreColorCodes);
    //			if (sid != null)
    //				return this [sid];
    //			else
    //				return null;
    //		}

    public string GetSteamID(string _nameOrId, bool _ignoreColorCodes)
    {
      if (_nameOrId == null || _nameOrId.Length == 0)
      {
        return null;
      }

      long tempLong;
      if (_nameOrId.Length == 17 && long.TryParse(_nameOrId, out tempLong))
      {
        return _nameOrId;
      }
      else
      {
        int entityId = -1;
        if (int.TryParse(_nameOrId, out entityId))
        {
          foreach (KeyValuePair<string, Player> kvp in players)
          {
            if (kvp.Value.IsOnline && kvp.Value.EntityID == entityId)
            {
              return kvp.Key;
            }
          }
        }

        _nameOrId = _nameOrId.ToLower();
        foreach (KeyValuePair<string, Player> kvp in players)
        {
          string name = kvp.Value.Name.ToLower();
          if (_ignoreColorCodes)
          {
            name = Regex.Replace(name, "\\[[0-9a-fA-F]{6}\\]", "");
          }
          if (kvp.Value.IsOnline && name.Equals(_nameOrId))
          {
            return kvp.Key;
          }
        }
      }
      return null;
    }
  }
}
