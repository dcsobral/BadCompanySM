using UnityEngine;

namespace BCM
{
  public static class Convert
  {
    public static string PosToStr(Vector3i v, string postype)
    {
      string position;
      switch (postype)
      {
        case "worldpos":
          position = GameUtils.WorldPosToStr(v.ToVector3(), " ");
          break;
        case "csvpos":
          position = $"{v.x}, {v.y}, {v.z}";
          break;
        default:
          position = $"{v.x} {v.y} {v.z}";
          break;
      }
      return position;
    }
    public static string PosToStr(Vector3 v, string postype)
    {
      string position;
      switch (postype)
      {
        case "worldpos":
          position = GameUtils.WorldPosToStr(v, " ");
          break;
        case "csvpos":
          position = $"{v.x:F0},{v.y:F0},{v.z:F0}";
          break;
        default:
          position = $"{v.x:F0} {v.y:F0} {v.z:F0}";
          break;
      }
      return position;
    }
    public static string PosToStr(Vector2i v, string postype)
    {
      return string.Format(postype == "csvpos" ? "{0}, {1}" : "{0} {1}", v.x, v.y);
    }
  }
}
