using BCM.Models;

namespace BCM.Commands
{
  public class ListPlayersCraftingQueue : ListPlayers
  {
    public override void displayPlayer(PlayerInfo _pInfo)
    {
      string output = "\n";
      output += new ClientInfoList(_pInfo).DisplayShort();
      output += new CraftingQueue(_pInfo).Display();

      SendOutput(output);
    }
  }
}
