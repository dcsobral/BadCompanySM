using BCM.Models;

namespace BCM.Commands
{
  public class ListPlayersBuffs : ListPlayers
  {
    public override void displayPlayer(PlayerInfo _pInfo)
    {
      string output = "\n";
      output += new ClientInfoList(_pInfo).DisplayShort();
      output += new BuffList(_pInfo).Display();

      SendOutput(output);
    }
  }
}
