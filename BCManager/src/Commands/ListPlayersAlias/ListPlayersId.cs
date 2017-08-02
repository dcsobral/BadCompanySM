using BCM.Models;

namespace BCM.Commands
{
  public class ListPlayersId : ListPlayers
  {
    public override void Process()
    {
      if (_options.ContainsKey("filter"))
      {
        SendOutput("Error: Can't set filters on this alias command");
        SendOutput(GetHelp());

        return;
      }

      string filters = BCMPlayer.StrFilters.EntityId;

      if (_options.ContainsKey("n"))
      {
        filters += "," + BCMPlayer.StrFilters.Name;
      }

      _options.Add("filter", filters);
      var listPlayersCmd = new ListPlayers();
      listPlayersCmd.Process(_options, _params);
    }
  }
}
