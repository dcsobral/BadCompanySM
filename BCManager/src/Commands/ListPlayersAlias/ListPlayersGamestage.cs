using BCM.Models;

namespace BCM.Commands
{
  public class ListPlayersGamestage : ListPlayers
  {
    public override void Process()
    {
      if (_options.ContainsKey("filter"))
      {
        SendOutput("Error: Can't set filters on this alias command");
        SendOutput(GetHelp());

        return;
      }

      _options.Add("filter", BCMPlayer.StrFilters.Gamestage);
      var listPlayersCmd = new ListPlayers();
      listPlayersCmd.Process(_options, _params);
    }
  }
}
