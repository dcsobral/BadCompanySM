using System.Collections.Generic;
using BCM.Models;
using System;
using System.Reflection;

namespace BCM.Commands
{
  public class ListPlayers : BCCommandAbstract
  {
    private void Filters()
    {
      var data = new Dictionary<string, string>();
      var obj = typeof(BCMPlayer.StrFilters);
      foreach (var field in obj.GetFields())
      {
        data.Add(field.Name, field.GetValue(obj).ToString());
      }

      SendJson(data);
    }
    private void Index()
    {
      var data = new Dictionary<string, int>();
      foreach (var value in Enum.GetValues(typeof(BCMPlayer.Filters)))
      {
        data.Add(value.ToString(), (int)value);
      }

      SendJson(data);
    }

    public override void Process()
    {
      if (GameManager.Instance.World == null)
      {
        SendOutput("The world isn't loaded");

        return;
      }

      if (_options.ContainsKey("filters"))
      {
        Filters();
        return;
      }
      if (_options.ContainsKey("index"))
      {
        Index();
        return;
      }

      if (_params.Count > 1)
      {
        SendOutput("Wrong number of arguments");
        SendOutput(Config.GetHelp(GetType().Name));

        return;
      }

      if (_params.Count == 1)
      {
        // specific player
        string _steamId = "";
        if (GetEntity.GetBySearch(_params[0], out _steamId, "CON"))
        {
          var player = new BCMPlayer(new GetPlayer().BySteamId(_steamId), _options);

          if (_options.ContainsKey("nokeys"))
          {
            List<List<object>> keyless = new List<List<object>>();
            List<object> obj = new List<object>();
            foreach (var d in player.Data())
            {
              obj.Add(d.Value);
            }
            keyless.Add(obj);
            SendJson(keyless);
          }
          else
          {
            SendJson(player.Data());
          }
        }
      }
      else
      {
        //todo: /players=id1,id2,id3 instead of single or all
        // All players
        //todo: fix below function, shouldnt hit player files if /nopdf or /fastpos+/online are set

        List<string> players = GetEntity.GetStoredPlayers(_options);
        Dictionary<string, Dictionary<string, object>> data = new Dictionary<string, Dictionary<string, object>>();
        List<List<object>> keyless = new List<List<object>>();
        foreach (string _steamId in players)
        {
          var player = new BCMPlayer(new GetPlayer().BySteamId(_steamId), _options);
          if (_options.ContainsKey("nokeys"))
          {
            var obj = new List<object>();
            foreach (var d in player.Data())
            {
              obj.Add(d.Value);
            }
            keyless.Add(obj);
          }
          else
          {
            data.Add(_steamId, player.Data());
          }
        }

        if (_options.ContainsKey("nokeys"))
        {
          SendJson(keyless);
        }
        else
        {
          SendJson(data);
        }
      }
    }
  }
}
