namespace BCM
{
  public static class BCUtils
  {
    public static string ColorToHex(UnityEngine.Color _color)
    {
      return string.Format("{0:X02}{1:X02}{2:X02}", (int)(_color.r * 255), (int)(_color.g * 255), (int)(_color.b * 255));
    }
  }
}
