using System.Collections.Generic;
using JetBrains.Annotations;

namespace BCM.Models
{
  public class BCMMesh : BCMMeshShort
  {
    [UsedImplicitly] public string AtlasClass;
    [UsedImplicitly] public string MeshType;
    [UsedImplicitly] public bool Shadows;
    [UsedImplicitly] public string BlendMode;
    [UsedImplicitly] public string Tag;
    [UsedImplicitly] public string Collider;
    [UsedImplicitly] public string SecShader;
    [UsedImplicitly] public string ShaderName;
    [UsedImplicitly] public string ShaderDistant;
    [UsedImplicitly] public string MetaName;
    [UsedImplicitly] public string MetaText;
    [NotNull] [UsedImplicitly] public new List<BCMMeshData> MetaData;

    public BCMMesh([NotNull] MeshDescription meshDesc, [NotNull] List<BCMMeshData> metaData)
    {
      Name = meshDesc.Name;
      AtlasClass = meshDesc.TextureAtlasClass;
      MeshType = meshDesc.meshType.ToString();
      Shadows = meshDesc.bCastShadows;
      BlendMode = meshDesc.BlendMode.ToString();
      Tag = meshDesc.Tag;
      Collider = meshDesc.ColliderLayerName;
      SecShader = meshDesc.SecondaryShader;
      ShaderName = meshDesc.ShaderName;
      ShaderDistant = meshDesc.ShaderNameDistant;
      MetaName = meshDesc.MetaData?.name;
      MetaText = meshDesc.MetaData?.text;
      MetaData = metaData;
    }
  }
}
