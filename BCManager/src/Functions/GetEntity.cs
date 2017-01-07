using System.Collections.Generic;
using System.IO;

namespace BCM
{
  public static class GetEntity
  {
    public static bool GetBySearch(string _param, out string _steamId, string _el = "")
    {
      // todo: allow for finding by entityid for offline players (playerdatafile.id)

      // finds an entity based on entityid, steamid, or partial name match
      _steamId = _param;
      ClientInfo _cInfo = null;
      // get steam id if found in online entities, or is a valid id
      int _count = ConsoleHelper.ParseParamPartialNameOrId(_param, out _steamId, out _cInfo, false);
      if (_cInfo == null && _count > 0)
      {
        // not found in online entities, check if is in stored data
        if (!InStored(_steamId))
        {
          // not found in stored players
          _count = 0;
        }
      }

      if (_count == 1)
      {
        return true;
      }
      else
      {
        if (_count > 1)
        {
          Error(_el, "" + Config.ModPrefix + " Multiple matches found.");
        }
        else
        {
          Error(_el, "" + Config.ModPrefix + " Entity not found.");
        }
        return false;
      }

    }

    public static bool InStored(string _steamId)
    {
      List<string> players = GetStoredPlayers(null);
      return players.Contains(_steamId);
    }

    /* returns a list of steam ids found in Player Data Dir */
    public static List<string> GetStoredPlayers(Dictionary<string, string> _options)
    {
      string playerDataDir = GameUtils.GetPlayerDataDir();
      string[] files = Directory.GetFiles(playerDataDir);
      List<string> players = new List<string>();

      List<ClientInfo> clients = new List<ClientInfo>();
      if (_options != null)
      {
        if (_options.ContainsKey("online") || _options.ContainsKey("offline"))
        {
          if (ConnectionManager.Instance.ClientCount() > 0)
          {
            clients = ConnectionManager.Instance.GetClients();
          }
        }
      }

      for (int i = files.Length - 1; i >= 0; i--)
      {
        string file = files[i];
        string ext = Path.GetExtension(file);
        if (ext == ".ttp")
        {
          // todo:  GetFilenameFromPath
          int start = playerDataDir.Length + 1;
          int len = file.Length - start - 4;
          if (start + len <= file.Length)
          {
            string id = file.Substring(start, len);

            if (_options != null)
            {
              if (_options.ContainsKey("online"))
              {
                if (clients.Find(x => x.playerId == id) != null)
                {
                  players.Add(id);
                  continue;
                }
              } else if (_options.ContainsKey("offline"))
              {
                if (clients.Find(x => x.playerId == id) == null)
                {
                  players.Add(id);
                  continue;
                }
              } else
              {
                players.Add(id);
              }
            }
            else
            {
              players.Add(id);
            }
          }
        }
      }
      return players;
    }

    /* sends error message to console, log, or both */
    private static void Error(string el, string err)
    {
      if (el != "")
      {
        if (el == "CON")
        {
          SdtdConsole.Instance.Output(err);
        }
        if (el == "LOG")
        {
          Log.Out(err);
        }
        if (el == "ALL")
        {
          SdtdConsole.Instance.Output(err);
          Log.Out(err);
        }
      }
    }
  }
}
