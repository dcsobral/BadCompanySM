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
          var data = new BCMPlayer(new GetPlayer().BySteamId(_steamId), _options);
          SendJson(data.Data());
        }
      }
      else
      {
        //todo: /players=id1,id2,id3 instead of single or all
        // All players
        //todo: fix below function, shouldnt hit player files if /nopdf or /fastpos+/online are set

        List<string> players = GetEntity.GetStoredPlayers(_options);
        Dictionary<string, Dictionary<string, object>> data = new Dictionary<string, Dictionary<string, object>>();
        foreach (string _steamId in players)
        {
          var player = new BCMPlayer(new GetPlayer().BySteamId(_steamId), _options);
          data.Add(_steamId, player.Data());
        }
        SendJson(data);
      }
    }
  }
}
