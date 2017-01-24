using System.Collections.Generic;
using BCM.Models;

namespace BCM.Commands
{
  public class ListPlayers : BCCommandAbstract
  {
    public override void Process()
    {
      if (_params.Count > 1)
      {
        SdtdConsole.Instance.Output("Wrong number of arguments");
        SdtdConsole.Instance.Output(Config.GetHelp(GetType().Name));
        return;
      }

      if (_params.Count == 1)
      {
        // specific player
        string _steamId = "";
        if (GetEntity.GetBySearch(_params[0], out _steamId, "CON"))
        {
          if (_options.ContainsKey("json"))
          {
            Dictionary<string, string> data = jsonPlayer(new GetPlayer().BySteamId(_steamId));
            SendOutput(BCUtils.GenerateJson(data));
          }
          else
          {
            SendOutput(displayPlayer(new GetPlayer().BySteamId(_steamId)));
          }
        }
      }
      else
      {
        // All players
        List<string> players = GetEntity.GetStoredPlayers(_options);
        Dictionary<string, Dictionary<string, string>> data = new Dictionary<string, Dictionary<string, string>>();
        foreach (string _steamId in players)
        {
          if (_options.ContainsKey("json"))
          {
            data.Add(_steamId, jsonPlayer(new GetPlayer().BySteamId(_steamId)));
          }
          else
          {
            SendOutput(displayPlayer(new GetPlayer().BySteamId(_steamId)));
          }
        }
        if (_options.ContainsKey("json"))
        {
          SendOutput(BCUtils.GenerateJson(data));
        } else if (players.Count > 0) {
          SendOutput("Total of " + players.Count + " player data files");
        }
      }
    }
    public virtual Dictionary<string, string> jsonPlayer(PlayerInfo _pInfo)
    {
      Dictionary<string, string> data = new Dictionary<string, string>();

      Dictionary<string, string> info = new ClientInfoList(_pInfo, _options).GetInfo();
      Dictionary<string, string> stats = new StatsList(_pInfo, _options).GetStats();
      foreach (string key in info.Keys)
      {
        data.Add(key, info[key]);
      }
      foreach (string key in stats.Keys)
      {
        if (!data.ContainsKey(key))
        {
          data.Add(key, stats[key]);
        }
      }
      return data;
    }

    public virtual string displayPlayer(PlayerInfo _pInfo)
    {
      string output = "";
      output += new ClientInfoList(_pInfo, _options).Display(_sep);
      output += _sep;
      output += new StatsList(_pInfo, _options).Display(_sep);

      return output;
    }
  }
}
