using System;
using UnityEngine;

namespace BCM.Models
{
  public class BCMVector4
  {
    public int x;
    public int y;
    public int z;
    public int w;
    public BCMVector4()
    {
      x = 0;
      y = 0;
      z = 0;
      w = 0;
    }
    public BCMVector4(int x, int y, int z, int w)
    {
      this.x = x;
      this.y = y;
      this.z = z;
      this.w = w;
    }

    public override string ToString()
    {
      return $"{x} {z} {y} {w}";
    }

    public Vector3 ToV3()
    {
      return new Vector3(8 + x * 16 + (z * 16 - x * 16) / 2, 0, 8 + y * 16 + (w * 16 - y * 16) / 2);
    }

    public int GetRadius()
    {
      return Math.Max((z - x) / 2, (w - y) / 2);
    }
  }
}
