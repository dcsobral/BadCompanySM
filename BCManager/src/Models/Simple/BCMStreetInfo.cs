using RWG2.Rules;
using JetBrains.Annotations;

namespace BCM.Models
{
  public class BCMStreetInfo
  {
    [UsedImplicitly] public string Name;
    [UsedImplicitly] public BCMVector2 StartPoint;
    [UsedImplicitly] public BCMVector2 EndPoint;
    [UsedImplicitly] public int PathType;
    [UsedImplicitly] public int PathRadius;

    public BCMStreetInfo(HubLayout.StreetInfo streetInfo)
    {
      Name = streetInfo.Name;
      StartPoint = new BCMVector2(streetInfo.StartPoint);
      EndPoint = new BCMVector2(streetInfo.EndPoint);
      PathType = streetInfo.PathMaterial.type;
      PathRadius = streetInfo.PathRadius;
    }
  }
}
