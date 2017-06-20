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
            SendOutput(BCUtils.toJson(data));
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
            if (_options.ContainsKey("nopdf"))
            {
              data.Add(_steamId, jsonPlayer(_steamId));
            }
            else
            {
              data.Add(_steamId, jsonPlayer(new GetPlayer().BySteamId(_steamId)));
            }
          }
          else
          {
            SendOutput(displayPlayer(new GetPlayer().BySteamId(_steamId)));
          }
        }
        if (_options.ContainsKey("json"))
        {
          SendOutput(BCUtils.toJson(data));
        } else if (players.Count > 0) {
          SendOutput("Total of " + players.Count + " player data files");
        }
      }
    }
    public virtual Dictionary<string, string> jsonPlayer(string _pInfo)
    {
      Dictionary<string, string> data = new Dictionary<string, string>();

      return data;
    }
    public virtual Dictionary<string, string> jsonPlayer(PlayerInfo _pInfo)
    {
      Dictionary<string, string> data = new Dictionary<string, string>();

      if (_options.ContainsKey("details"))
      {
        data.Add("playerInfo", BCUtils.toJson(new ClientInfoList(_pInfo, _options).GetInfo()) ?? "");
        //data.Add("playerStats", BCUtils.toJson(new StatsList(_pInfo, _options).GetStats()) ?? "");
        //data.Add("playersGamestage", BCUtils.toJson(new ListPlayersGamestage().jsonPlayer(_pInfo)) ?? "");

        if (_options.ContainsKey("1"))
        {
          data.Add("playerBag", BCUtils.toJson(new ListPlayersBag().jsonPlayer(_pInfo)) ?? "");
        }
        if (_options.ContainsKey("2"))
        {
          data.Add("playerBuffs", BCUtils.toJson(new ListPlayersBuffs().jsonPlayer(_pInfo)) ?? "");
        }
        if (_options.ContainsKey("3"))
        {
          data.Add("playerCraftingQueue", BCUtils.toJson(new ListPlayersCraftingQueue().jsonPlayer(_pInfo)) ?? "");
        }
        if (_options.ContainsKey("4"))
        {
          data.Add("playersEquipment", BCUtils.toJson(new ListPlayersEquipment().jsonPlayer(_pInfo)) ?? "");
        }
        if (_options.ContainsKey("5"))
        {
          data.Add("playersFavRecipes", BCUtils.toJson(new ListPlayersFavRecipes().jsonPlayer(_pInfo)) ?? "");
        }
        if (_options.ContainsKey("6"))
        {
          data.Add("playersQuests", BCUtils.toJson(new ListPlayersQuests().jsonPlayer(_pInfo)) ?? "");
        }
        if (_options.ContainsKey("7"))
        {
          data.Add("playersRecipes", BCUtils.toJson(new ListPlayersRecipes().jsonPlayer(_pInfo)) ?? "");
        }
        if (_options.ContainsKey("8"))
        {
          data.Add("playersSkills", BCUtils.toJson(new ListPlayersSkills().jsonPlayer(_pInfo)) ?? "");
        }
        if (_options.ContainsKey("9"))
        {
          //data.Add("playersSpawns", BCUtils.toJson(new ListPlayersSpawns().jsonPlayer(_pInfo)) ?? "");
        }
        if (_options.ContainsKey("a"))
        {
          data.Add("playersToolbelt", BCUtils.toJson(new ListPlayersToolbelt().jsonPlayer(_pInfo)) ?? "");
        }
          //data.Add("playersWaypoints", BCUtils.toJson(new ListPlayersWaypoints().jsonPlayer(_pInfo)) ?? "");


        }
        else
      {
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
