using System.Collections.Generic;

namespace BCM.Models
{
  public class BCMMesh : BCMMeshShort
  {
    public string AtlasClass;
    public string MeshType;
    public bool Shadows;
    public string BlendMode;
    public string Tag;
    public string Collider;
    public string SecShader;
    public string ShaderName;
    public string ShaderDistant;
    public string MetaName;
    public string MetaText;
    public new List<BCMMeshData> MetaData;
  }
}