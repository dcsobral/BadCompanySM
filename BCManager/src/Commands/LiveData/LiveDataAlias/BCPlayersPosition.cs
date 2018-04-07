using BCM.Models;
using JetBrains.Annotations;

namespace BCM.Commands
{
  [UsedImplicitly]
  public class BCPlayersPosition : BCPlayers
  {
    protected override void Process()
    {
      if (Options.ContainsKey("filter"))
      {
        SendOutput("Error: Can't set filters on this alias command");
        SendOutput(GetHelp());

        return;
      }

      var filters = BCMPlayer.StrFilters.Position;

      if (Options.ContainsKey("n"))
      {
        filters += "," + BCMPlayer.StrFilters.Name;
      }

      if (Options.ContainsKey("e"))
      {
        filters += "," + BCMPlayer.StrFilters.EntityId;
      }

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
