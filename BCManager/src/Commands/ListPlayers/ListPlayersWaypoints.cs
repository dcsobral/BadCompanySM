using BCM.Models;

namespace BCM.Commands
{
  public class ListPlayersWaypoints : ListPlayers
  {
    public override void displayPlayer(PlayerInfo _pInfo)
    {
      string output = "";
      output += new ClientInfoList(_pInfo, _options).DisplayShort(_sep);
      output += new WaypointList(_pInfo, _options).Display(_sep);

      SendOutput(output);
    }
  }
}
