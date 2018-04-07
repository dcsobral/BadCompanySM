using JetBrains.Annotations;

namespace BCM.Models
{
  public class BCMTexture
  {
    [UsedImplicitly] public int Id;
    [UsedImplicitly] public string Name;
    [UsedImplicitly] public string Local;
    [UsedImplicitly] public ushort TextureId;

    public BCMTexture([NotNull] BlockTextureData texture)
    {
      Id = texture.ID;
      Name = texture.Name;
      Local = texture.LocalizedName;
      TextureId = texture.TextureID;
    }
  }
}
