using System;
using System.Collections.Generic;

namespace BCM.Models
{
  [Serializable]
  public class WaypointList : AbstractList
  {
    private List<Waypoint> waypoints = new List<Waypoint>();
    private string markerpos;

    public WaypointList(PlayerInfo _pInfo, Dictionary<string, string> _options) : base(_pInfo, _options)
    {
    }

    public override void Load(PlayerInfo _pInfo)
    {
      string postype = GetPosType();
      markerpos = (_pInfo.PDF.markerPosition != Vector3i.zero ? Convert.PosToStr(_pInfo.PDF.markerPosition, postype) : "None");
      foreach (Waypoint wp in _pInfo.PDF.waypoints.List)
      {
        waypoints.Add(wp);
      }
    }

    public override string Display(string sep = " ")
    {
      string postype = GetPosType();
      string output = "MarkerPosition:" + markerpos + sep;

      bool first = true;
      output += "Waypoints:{";
      foreach (Waypoint wp in waypoints)
      {
        if (!first) { output += sep; } else { first = false; }
        output += wp.name + ":" + Convert.PosToStr(wp.pos, postype);
      }
      output += "}";

      return output;
    }
    public Dictionary<string, string> GetWaypoints ()
    {
      Dictionary<string, string> _waypoints = new Dictionary<string, string>();
      foreach (Waypoint wp in waypoints)
      {
        _waypoints.Add(wp.name, Convert.PosToStr(wp.pos, GetPosType()));
      }
      return _waypoints;
    }
    public Dictionary<string, string> GetMarkerpos()
    {
      Dictionary<string, string> mp = new Dictionary<string, string>();
      mp.Add("MarkerPosition", markerpos);
      return mp;
    }
  }
}
