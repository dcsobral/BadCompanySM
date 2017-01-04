using BCM.Models;

namespace BCM.Commands
{
  public class ListPlayersQuests : ListPlayers
  {
    public override void displayPlayer(PlayerInfo _pInfo)
    {
      string output = "\n";
      output += new ClientInfoList(_pInfo).DisplayShort();
      output += new QuestList(_pInfo).Display();

      SendOutput(output);
    }
  }
}
