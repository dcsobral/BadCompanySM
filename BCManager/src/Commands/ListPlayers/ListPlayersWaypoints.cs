using BCM.Models;

namespace BCM.Commands
{
  public class ListPlayersWaypoints : ListPlayers
  {
    public override void displayPlayer(PlayerInfo _pInfo)
    {
      string output = "\n";
      output += new ClientInfoList(_pInfo).DisplayShort();
      output += new WaypointList(_pInfo).Display();

      SendOutput(output);
    }
  }
}
