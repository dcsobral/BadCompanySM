using BCM.Models;

namespace BCM.Commands
{
  public class ListPlayersPosition : ListPlayers
  {
    public override void displayPlayer(PlayerInfo _pInfo)
    {
      string output = "";
      output += new ClientInfoList(_pInfo, _options).DisplayShortWithPos();
      
      SendOutput(output);
    }
  }
}
