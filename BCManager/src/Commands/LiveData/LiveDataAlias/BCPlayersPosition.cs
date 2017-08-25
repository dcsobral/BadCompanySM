using BCM.Models;

namespace BCM.Commands
{
  public class BCPlayersPosition : BCPlayers
  {
    public override void Process()
    {
      if (Options.ContainsKey("filter"))
      {
        SendOutput("Error: Can't set filters on this alias command");
        SendOutput(GetHelp());

        return;
      }

      string filters = BCMPlayer.StrFilters.Position;

      if (Options.ContainsKey("r"))
      {
        filters += "," + BCMPlayer.StrFilters.Rotation;
      }
      if (Options.ContainsKey("u"))
      {
        filters += "," + BCMPlayer.StrFilters.Underground;
      }
      if (Options.ContainsKey("g"))
      {
        filters += "," + BCMPlayer.StrFilters.OnGround;
      }

      Options.Add("filter", filters);
      var cmd = new BCPlayers();
      cmd.Process(Options, Params);
    }
  }
}
