using BCM.Models;

namespace BCM.Commands
{
  public class ListPlayersId : ListPlayers
  {
    public override void displayPlayer(PlayerInfo _pInfo)
    {
      string output = "";
      output += new ClientInfoList(_pInfo, _options).DisplayShort(_sep);

      SendOutput(output);
    }
  }
}
