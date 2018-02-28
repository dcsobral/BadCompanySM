using System.Xml;
using UnityEngine;

namespace BCM.Models
{
  public class BCMMeshDataShort
  {
    public int Id;
    public string Color;
    public string Material;
    public string Texture;

    public BCMMeshDataShort(XmlElement uv)
    {
      if (uv.HasAttribute("id")) int.TryParse(uv.GetAttribute("id"), out Id);
      if (uv.HasAttribute("color"))
      {
        var rgb = uv.GetAttribute("color").Split(',');
        if (rgb.Length == 3 && float.TryParse(rgb[0], out var r) && float.TryParse(rgb[1], out var g) && float.TryParse(rgb[2], out var b))
        {
          Color = BCUtils.ColorToHex(new Color(r, g, b));
        }
      }
      Material = uv.HasAttribute("material") ? uv.GetAttribute("material") : "";
      Texture = uv.HasAttribute("texture") ? uv.GetAttribute("texture").Substring(0, uv.GetAttribute("texture").Length - 4) : "";
    }
  }
}