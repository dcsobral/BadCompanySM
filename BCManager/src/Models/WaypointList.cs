using System;
using System.Collections.Generic;

namespace BCM.Models
{
  [Serializable]
  public class WaypointList
  {
    private List<Waypoint> waypoints = new List<Waypoint>();
    private string markerpos;

    public WaypointList()
    {
    }

    public WaypointList(PlayerInfo _pInfo)
    {
      Load(_pInfo);
    }

    public void Load(PlayerInfo _pInfo)
    {
      markerpos = (_pInfo.PDF.markerPosition != Vector3i.zero ? GameUtils.WorldPosToStr(_pInfo.PDF.markerPosition.ToVector3(), " ") : "None");
      foreach (Waypoint wp in _pInfo.PDF.waypoints.List)
      {
        waypoints.Add(wp);
      }
    }

    public string Display()
    {
      string output = "MarkerPosition:" + markerpos + "\n";

      bool first = true;
      output += "Waypoints(saved)={\n";
      foreach (Waypoint wp in waypoints)
      {
        if (!first) { output += ",\n"; } else { first = false; }
        output += wp.name + ":" + GameUtils.WorldPosToStr(wp.pos.ToVector3(), " ");
      }
      output += "\n}\n";

      return output;
    }
  }
}
