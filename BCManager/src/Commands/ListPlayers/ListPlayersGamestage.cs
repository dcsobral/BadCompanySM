using BCM.Models;

namespace BCM.Commands
{
  public class ListPlayersGamestage : ListPlayers
  {
    public override void displayPlayer(PlayerInfo _pInfo)
    {
      string output = "";
      output += new StatsList(_pInfo, _options).DisplayGamestage(_pInfo.CI, _sep);

      SendOutput(output);
    }
  }
}
