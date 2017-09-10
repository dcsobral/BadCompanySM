using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BCM
{
  public static class PlayerStore
  {
    public static bool GetId(string param, out string steamId, string el = "")
    {
      var count = ConsoleHelper.ParseParamPartialNameOrId(param, out steamId, out ClientInfo cInfo, false);

      if (cInfo == null && count > 0 && !IsStoredPlayer(steamId))
      {
        count = 0;
      }

      if (count == 1)
      {
        return true;
      }

      Error(el, count > 1 ? $"{Config.ModPrefix} Multiple matches found." : $"{Config.ModPrefix} Entity not found.");

      return false;
    }

    private static bool IsStoredPlayer(string steamId)
    {
      return File.Exists(GameUtils.GetPlayerDataDir() + Path.DirectorySeparatorChar + steamId + ".ttp");
    }

    /* Returns a list of steam ids found in Player Data Dir */
    public static IEnumerable<string> GetAll(Dictionary<string, string> options)
    {
      var playerDataDir = GameUtils.GetPlayerDataDir();
      var players = new List<string>();

      if (!Directory.Exists(playerDataDir)) return players;

      var files = Directory.GetFiles(playerDataDir);

      var clients = new List<ClientInfo>();
      if (options != null && (options.ContainsKey("online") || options.ContainsKey("offline")) && ConnectionManager.Instance.ClientCount() > 0)
      {
        clients = ConnectionManager.Instance.GetClients();
      }

      if (options == null)
      {
        for (var i = files.Length - 1; i >= 0; i--)
        {
          if (Path.GetExtension(files[i]) != ".ttp") continue;

          players.Add(Path.GetFileNameWithoutExtension(files[i]));
        }
      }
      else
      {
        if (options.ContainsKey("players"))
        {
          var ids = options["players"].Split(',').ToList();
          for (var i = files.Length - 1; i >= 0; i--)
          {
            if (Path.GetExtension(files[i]) != ".ttp") continue;

            var steamId = Path.GetFileNameWithoutExtension(files[i]);
            if (ids.Contains(steamId))
            {
              players.Add(steamId);
            }
          }
        }

        if (options.ContainsKey("online"))
        {
          for (var i = files.Length - 1; i >= 0; i--)
          {
            if (Path.GetExtension(files[i]) != ".ttp") continue;

            var steamId = Path.GetFileNameWithoutExtension(files[i]);
            if (clients.Find(x => x.playerId == steamId) != null)
            {
              players.Add(steamId);
            }
          }
        }
        else if (options.ContainsKey("offline"))
        {
          for (var i = files.Length - 1; i >= 0; i--)
          {
            if (Path.GetExtension(files[i]) != ".ttp") continue;

            var steamId = Path.GetFileNameWithoutExtension(files[i]);
            if (clients.Find(x => x.playerId == steamId) == null)
            {
              players.Add(steamId);
            }
          }
        }
        else
        {
          for (var i = files.Length - 1; i >= 0; i--)
          {
            if (Path.GetExtension(files[i]) != ".ttp") continue;

            var steamId = Path.GetFileNameWithoutExtension(files[i]);
            players.Add(steamId);
          }
        }
      }

      return players;
    }

    private static void Error(string el, string err)
    {
      switch (el)
      {
        case "CON":
          SdtdConsole.Instance.Output(err);
          break;
        case "LOG":
          Log.Out(err);
          break;
        case "ALL":
          SdtdConsole.Instance.Output(err);
          Log.Out(err);
          break;
        default:
          return;
      }
    }
  }
}
