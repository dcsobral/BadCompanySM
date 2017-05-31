using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace BCM
{
  public static class BCUtils
  {
    public static string ColorToHex(UnityEngine.Color _color)
    {
      return string.Format("{0:X02}{1:X02}{2:X02}", (int)(_color.r * 255), (int)(_color.g * 255), (int)(_color.b * 255));
    }
    public static string toJson(Dictionary<string, Dictionary<string, string>> data)
    {
      StringBuilder strb = new StringBuilder();
      strb.Append("{");
      bool first = true;
      foreach (string key in data.Keys)
      {
        if (!first) { strb.Append(","); } else { first = false; }
        strb.Append(string.Format("\"{0}\"", key));
        strb.Append(":");
        strb.Append(string.Format("{0}", toJson(data[key])));
      }
      strb.Append("}");
      return strb.ToString();
    }

    public static string toJson(Dictionary<string, string> data)
    {
      StringBuilder strb = new StringBuilder();
      bool first = true;
      if (data.Keys == null && data.Keys.Count == 0)
      {
        strb.Append("null");
      }
      else
      {
        strb.Append("{");
        foreach (string key in data.Keys)
        {
          if (!first) { strb.Append(","); } else { first = false; }
          strb.Append(string.Format("\"{0}\"", key));
          strb.Append(":");
          if (data[key].Length > 0 && (data[key].Substring(0, 1) == "{" || data[key].Substring(0, 1) == "["))
          {
            strb.Append(string.Format("{0}", data[key]));
          }
          else
          {
            strb.Append(string.Format("\"{0}\"", data[key]));
          }
        }
        strb.Append("}");
      }
      return strb.ToString();
    }
    public static string toJson(List<string> data)
    {
      StringBuilder strb = new StringBuilder();
      bool first = true;

      strb.Append("[");
      foreach (string value in data)
      {
        if (!first) { strb.Append(","); } else { first = false; }
        strb.Append(string.Format("{0}", value));
      }
      strb.Append("]");

      return strb.ToString();
    }

    //public static void CopyTo(Stream src, Stream dest)
    //{
    //  byte[] bytes = new byte[4096];

    //  int cnt;

    //  while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0)
    //  {
    //    dest.Write(bytes, 0, cnt);
    //  }
    //}

    //public static byte[] Zip(string str)
    //{
    //  var bytes = Encoding.UTF8.GetBytes(str);

    //  using (var msi = new MemoryStream(bytes))
    //  using (var mso = new MemoryStream())
    //  {
    //    using (var gs = new GZipStream(mso, CompressionMode.Compress))
    //    {
    //      //msi.CopyTo(gs);
    //      CopyTo(msi, gs);
    //    }

    //    return mso.ToArray();
    //  }
    //}

    //public static string Unzip(byte[] bytes)
    //{
    //  using (var msi = new MemoryStream(bytes))
    //  using (var mso = new MemoryStream())
    //  {
    //    using (var gs = new GZipStream(msi, CompressionMode.Decompress))
    //    {
    //      //gs.CopyTo(mso);
    //      CopyTo(gs, mso);
    //    }

    //    return Encoding.UTF8.GetString(mso.ToArray());
    //  }
    //}

    //static void Main(string[] args)
    //{
    //  byte[] r1 = Zip("StringStringStringStringStringStringStringStringStringStringStringStringStringString");
    //  string r2 = Unzip(r1);
    //}
  }
}
