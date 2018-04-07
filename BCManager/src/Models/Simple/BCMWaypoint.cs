using JetBrains.Annotations;
using UnityEngine;

namespace BCM.Models
{
  public class BCMWaypoint
  {
    [UsedImplicitly] public string Name;
    [UsedImplicitly] public string Icon;
    [UsedImplicitly] public Vector3 Pos;

    public BCMWaypoint(Waypoint waypoint)
    {
      Name = waypoint.name;
      Pos = waypoint.pos.ToVector3();
      Icon = waypoint.icon;
    }
  }
}
