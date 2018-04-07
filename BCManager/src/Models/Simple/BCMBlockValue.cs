using System;
using JetBrains.Annotations;

namespace BCM.Models
{
  [Serializable]
  public class BCMBlockValue
  {
    [UsedImplicitly] public int type;
    [UsedImplicitly] public byte rotation;
    [UsedImplicitly] public byte meta;
    [UsedImplicitly] public byte meta2;
    [UsedImplicitly] public byte meta3;

    public BCMBlockValue(uint rawData)
    {
      var bv = new BlockValue(rawData);
      type = bv.type;
      rotation = bv.rotation;
      meta = bv.meta;
      meta2 = bv.meta2;
      meta3 = bv.meta3;
    }
  }
}