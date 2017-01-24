using BCM.Models;
using System.Collections.Generic;

namespace BCM.Commands
{
  public class ListPlayersWaypoints : ListPlayers
  {
    public override Dictionary<string, string> jsonPlayer(PlayerInfo _pInfo)
    {
      Dictionary<string, string> waypoints = new WaypointList(_pInfo, _options).GetWaypoints();
      Dictionary<string, string> marker = new WaypointList(_pInfo, _options).GetMarkerpos();
      if (!waypoints.ContainsKey("MarkerPosition") && marker.ContainsKey("MarkerPosition"))
      {
        waypoints.Add("MarkerPosition", marker["MarkerPosition"]);
      }
      return waypoints;
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
