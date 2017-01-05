using System;
using UnityEngine;

namespace BCM
{
  public static class Convert
  {
    public static string PosToStr(Vector3i v, string postype)
    {
      string position = string.Empty;
      if (postype == "worldpos")
      {
        position = GameUtils.WorldPosToStr(v.ToVector3(), " ");
      }
      else if (postype == "csvpos")
      {
        position = string.Format("{0}, {1}, {2}", v.x, v.y, v.z);
      }
      else
      {
        position = string.Format("{0} {1} {2}", v.x, v.y, v.z);
      }
      return position;
    }
    public static string PosToStr(Vector3 v, string postype)
    {
      string position = string.Empty;
      if (postype == "worldpos")
      {
        position = GameUtils.WorldPosToStr(v, " ");
      }
      else if (postype == "csvpos")
      {
        position = string.Format("{0}, {1}, {2}", v.x, v.y, v.z);
      }
      else
      {
        position = string.Format("{0} {1} {2}", v.x, v.y, v.z);
      }
      return position;
    }
  }
}
