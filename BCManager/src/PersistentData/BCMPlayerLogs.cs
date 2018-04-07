using System;
using System.Collections.Generic;
using BCM.Models;
using JetBrains.Annotations;
using UnityEngine;

namespace BCM.PersistentData
{
  [Serializable]
  public class BCMPlayerLogs
  {
    private readonly Dictionary<string, PlayerLog> _playerlogs = new Dictionary<string, PlayerLog>();

    public int Count => _playerlogs.Count;

    [CanBeNull]
    public PlayerLog this[string steamId, bool create]
    {
      get
      {
        if (string.IsNullOrEmpty(steamId))
        {
          return null;
        }

        if (_playerlogs.ContainsKey(steamId))
        {
          return _playerlogs[steamId];
        }

        if (!create || steamId.Length != 17) return null;

        var p = new PlayerLog(steamId);
        _playerlogs.Add(steamId, p);
        return p;
      }
    }

    public IEnumerable<string> AllKeys()
    {
      return _playerlogs.Keys;
    }
  }

  [Serializable]
  public class LogData
  {
    public string d;
    public string r;

    public LogData(BCMVector4 posrot, string reason)
    {
      d = $"{posrot}";
      r = $"{reason}";
    }
  }

  [Serializable]
  public class PlayerLog
  {
    public string SteamId;
    public readonly Dictionary<string, LogData> LogDataCache = new Dictionary<string, LogData>();
    private BCMVector3 last;

    public PlayerLog(string steamId)
    {
      SteamId = steamId;
    }

    private void LogPosition(BCMVector3 p, int r)
    {
      if (p.Equals(last)) return;

      var ts = $"{DateTime.UtcNow:yyyy-MM-dd_HH_mm_ss.fffZ}";
      if (!LogDataCache.ContainsKey(ts))
      {
        LogDataCache.Add(ts, new LogData( new BCMVector4(p, r), "M"));
      }
      last = p;
    }

    public void LogPosition(Vector4 p, int r) => LogPosition(new BCMVector3(p), r);
  }
}
