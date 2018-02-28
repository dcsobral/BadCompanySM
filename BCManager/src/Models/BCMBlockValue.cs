using System;

namespace BCM.Models
{
  [Serializable]
  public class BCMBlockValue
  {
    public int type;
    public byte rotation;
    public byte meta;
    public byte meta2;
    public byte meta3;

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