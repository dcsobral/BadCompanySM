using BCM.Models;
using System.Collections.Generic;

namespace BCM.Commands
{
  public class ListPlayersWaypoints : ListPlayers
  {
    public override Dictionary<string, string> jsonPlayer(PlayerInfo _pInfo)
    {
      return null;
    }
    public override string displayPlayer(PlayerInfo _pInfo)
    {
      string output = "";
      output += new ClientInfoList(_pInfo, _options).DisplayShort(_sep);
      output += _sep;
      output += new WaypointList(_pInfo, _options).Display(_sep);

      return output;
    }
  }
}
