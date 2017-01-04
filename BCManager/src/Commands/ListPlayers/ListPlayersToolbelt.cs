using BCM.Models;

namespace BCM.Commands
{
  public class ListPlayersToolbelt : ListPlayers
  {
    public override void displayPlayer(PlayerInfo _pInfo)
    {
      string output = "\n";
      output += new ClientInfoList(_pInfo).DisplayShort();
      output += new ToolbeltList(_pInfo).Display();

      SendOutput(output);
    }
  }
}
