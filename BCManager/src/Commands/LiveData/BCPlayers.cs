using System.Collections.Generic;
using BCM.Models;
using System.Linq;

namespace BCM.Commands
{
  public class BCPlayers : BCCommandAbstract
  {
    private static void Filters() => SendJson(typeof(BCMPlayer.StrFilters).GetFields()
      .ToDictionary(field => field.Name, field => $"{field.GetValue(typeof(BCMPlayer.StrFilters))}"));

    private static void Indexed() => SendJson(BCMPlayer.FilterMap
      .GroupBy(kvp => kvp.Value)
      .Select(group => group.First()).ToDictionary(kvp => kvp.Value, kvp => kvp.Key));

    public override void Process()
    {
      if (GameManager.Instance.World == null)
      {
        SendOutput("The world isn't loaded");

        return;
      }

      if (Options.ContainsKey("filters"))
      {
        Filters();

        return;
      }

      if (Options.ContainsKey("index"))
      {
        Indexed();

        return;
      }

      if (Params.Count > 1)
      {
        SendOutput("Wrong number of arguments");
        SendOutput(Config.GetHelp(GetType().Name));

        return;
      }

      if (Params.Count == 1)
      {
        //SPECIFIC PLAYER
        if (!PlayerStore.GetId(Params[0], out string steamId, "CON")) return;

        var player = new BCMPlayer(PlayerData.PlayerInfo(steamId), Options, GetFilters(BCMGameObject.GOTypes.Players));
        if (Options.ContainsKey("min"))
        {
          SendJson(new List<List<object>>
          {
            player.Data().Select(d => d.Value).ToList()
          });
        }
        else
        {
          SendJson(player.Data());
        }
      }
      else
      {
        //ALL PLAYERS
        var data = new List<object>();
        foreach (var player in PlayerStore.GetAll(Options).Select(s => new BCMPlayer(PlayerData.PlayerInfo(s), Options, GetFilters(BCMGameObject.GOTypes.Players))))
        {
          if (Options.ContainsKey("min"))
          {
            data.Add(player.Data().Select(d => d.Value).ToList());
          }
          else
          {
            data.Add(player.Data());
          }
        }

        SendJson(data);
      }
    }
  }
}
