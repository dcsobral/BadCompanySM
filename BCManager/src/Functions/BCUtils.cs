using System.Collections.Generic;
using System.Text;

namespace BCM
{
  public static class BCUtils
  {
    public static string ColorToHex(UnityEngine.Color _color)
    {
      return string.Format("{0:X02}{1:X02}{2:X02}", (int)(_color.r * 255), (int)(_color.g * 255), (int)(_color.b * 255));
    }
    public static string GenerateJson(Dictionary<string, Dictionary<string, string>> data)
    {
      StringBuilder strb = new StringBuilder();
      strb.Append("{");
      bool first = true;
      foreach (string key in data.Keys)
      {
        if (!first) { strb.Append(","); } else { first = false; }
        strb.Append(string.Format("\"{0}\"", key));
        strb.Append(":");
        strb.Append(GenerateJson(data[key]));
      }
      strb.Append("}");
      return strb.ToString();
    }

    public static string GenerateJson(Dictionary<string, string> data)
    {
      StringBuilder strb = new StringBuilder();
      strb.Append("{");
      bool first = true;
      foreach (string key in data.Keys)
      {
        if (!first) { strb.Append(","); } else { first = false; }
        strb.Append(string.Format("\"{0}\"", key));
        strb.Append(":");
        strb.Append(string.Format("\"{0}\"", data[key]));
      }
      strb.Append("}");

      return strb.ToString();
    }

  }
}
