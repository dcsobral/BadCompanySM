using BCM.Models;

namespace BCM.Commands
{
  public class ListPlayersClientInfo : ListPlayers
  {
    public override void displayPlayer(PlayerInfo _pInfo)
    {
      string output = "\n";
      output += new ClientInfoList(_pInfo).Display();

      SendOutput(output);
    }
  }
}
