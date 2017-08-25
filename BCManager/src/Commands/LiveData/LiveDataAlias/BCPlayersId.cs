using BCM.Models;

namespace BCM.Commands
{
  public class BCPlayersId : BCPlayers
  {
    public override void Process()
    {
      if (Options.ContainsKey("filter"))
      {
        SendOutput("Error: Can't set filters on this alias command");
        SendOutput(GetHelp());

        return;
      }

      string filters = BCMPlayer.StrFilters.EntityId;

      if (Options.ContainsKey("n"))
      {
        filters += "," + BCMPlayer.StrFilters.Name;
      }

      Options.Add("filter", filters);
      var cmd = new BCPlayers();
      cmd.Process(Options, Params);
    }
  }
}
