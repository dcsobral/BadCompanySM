using System.Collections.Generic;
using JetBrains.Annotations;

namespace BCM.Models
{
  public class BCMMeshShort
  {
    [UsedImplicitly] public string Name;
    [NotNull] [UsedImplicitly] public List<BCMMeshDataShort> MetaData;

    public BCMMeshShort()
    {
      MetaData = new List<BCMMeshDataShort>();
    }

    public BCMMeshShort([NotNull] MeshDescription meshDesc, [NotNull] List<BCMMeshDataShort> metaData)
    {
      Name = meshDesc.Name;
      MetaData = metaData;
    }
  }
}
