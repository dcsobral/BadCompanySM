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
          displayPlayer(new GetPlayer().BySteamId(_steamId));
        }
      }
      else
      {
        // All players
        List<string> players = GetEntity.GetStoredPlayers();
        foreach (string _steamId in players)
        {
          displayPlayer(new GetPlayer().BySteamId(_steamId));
        }
        SendOutput("\n\nTotal of " + players.Count + " player data files\n");
      }
    }

    public virtual void displayPlayer(PlayerInfo _pInfo)
    {
      string output = "\n";
      output += new ClientInfoList(_pInfo).Display();
      output += new StatsList(_pInfo).Display();

      SendOutput(output);
    }
  }
}
