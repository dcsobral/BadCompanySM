using System.Xml;
using JetBrains.Annotations;

namespace BCM.Models
{
  public class BCMMeshData : BCMMeshDataShort
  {
    [UsedImplicitly] public double X;
    [UsedImplicitly] public double Y;
    [UsedImplicitly] public double W;
    [UsedImplicitly] public double H;
    [UsedImplicitly] public int Blockw;
    [UsedImplicitly] public int Blockh;
    [UsedImplicitly] public bool Globaluv;

    public BCMMeshData([NotNull] XmlElement uv) : base(uv)
    {
      if (uv.HasAttribute("x")) double.TryParse(uv.GetAttribute("x"), out X);
      if (uv.HasAttribute("y")) double.TryParse(uv.GetAttribute("y"), out Y);
      if (uv.HasAttribute("w")) double.TryParse(uv.GetAttribute("w"), out W);
      if (uv.HasAttribute("h")) double.TryParse(uv.GetAttribute("h"), out H);
      if (uv.HasAttribute("blockw")) int.TryParse(uv.GetAttribute("blockw"), out Blockw);
      if (uv.HasAttribute("blockh")) int.TryParse(uv.GetAttribute("blockh"), out Blockh);

      if (uv.HasAttribute("globaluv")) bool.TryParse(uv.GetAttribute("globaluv"), out Globaluv);
    }
  }
}