using BCM.Models;

namespace BCM.Commands
{
  public class BCPlayersGamestage : BCPlayers
  {
    public override void Process()
    {
      if (Options.ContainsKey("filter"))
      {
        SendOutput("Error: Can't set filters on this alias command");
        SendOutput(GetHelp());

        return;
      }

      Options.Add("filter", BCMPlayer.StrFilters.Gamestage);
      var cmd = new BCPlayers();
      cmd.Process(Options, Params);
    }
  }
}
