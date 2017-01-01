using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BCM.Models
{
  [Serializable]
  public class WaypointList
  {
    private List<Waypoint> waypoints = new List<Waypoint>();

    public WaypointList()
    {
    }

    public WaypointList(PlayerDataFile _pdf)
    {
      Load(_pdf);
    }

    public void Load(PlayerDataFile _pdf)
    {
      foreach (Waypoint wp in _pdf.waypoints.List)
      {
        waypoints.Add(wp);
      }
    }

    public string Display()
    {
      bool first = true;
      string output = "Waypoints(saved)={\n";
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
