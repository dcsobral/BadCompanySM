using System.Collections.Generic;
using JetBrains.Annotations;

namespace BCM.Models
{
  public class BCMBuffEntity
  {
    [UsedImplicitly] public int EntityId;
    [UsedImplicitly] public string Name;
    [NotNull] [UsedImplicitly] public List<BCMBuffInfo> Buffs = new List<BCMBuffInfo>();

    public BCMBuffEntity( int entityId, string name)
    {
      EntityId = entityId;
      Name = name;
    }
  }
}