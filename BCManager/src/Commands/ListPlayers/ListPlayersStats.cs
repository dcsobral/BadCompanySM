using BCM.Models;

namespace BCM.Commands
{
  public class ListPlayersStats : ListPlayers
  {
    public override void displayPlayer(PlayerInfo _pInfo)
    {
      string output = "\n";
      output += new ClientInfoList(_pInfo).DisplayShort();
      output += new StatsList(_pInfo).Display();

      SendOutput(output);
    }
  }
}
