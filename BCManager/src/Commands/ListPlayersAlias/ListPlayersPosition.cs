using BCM.Models;

namespace BCM.Commands
{
  public class ListPlayersPosition : ListPlayers
  {
    public override void Process()
    {
      if (_options.ContainsKey("filter"))
      {
        SendOutput("Error: Can't set filters on this alias command");
        SendOutput(GetHelp());

        return;
      }

      string filters = BCMPlayer.StrFilters.Position;

      if (_options.ContainsKey("r"))
      {
        filters += "," + BCMPlayer.StrFilters.Rotation;
      }
      if (_options.ContainsKey("u"))
      {
        filters += "," + BCMPlayer.StrFilters.Underground;
      }
      if (_options.ContainsKey("g"))
      {
        filters += "," + BCMPlayer.StrFilters.OnGround;
      }

      _options.Add("filter", filters);
      var listPlayersCmd = new ListPlayers();
      listPlayersCmd.Process(_options, _params);
    }
  }
}
