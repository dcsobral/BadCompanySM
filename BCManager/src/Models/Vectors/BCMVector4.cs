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
    public BCMVector4(BCMVector3 v, int _w)
    {
      x = v.x;
      y = v.y;
      z = v.z;
      w = _w;
    }
    public BCMVector4(Vector3 v, int _w)
    {
      x = (int)Math.Floor(v.x);
      y = (int)Math.Floor(v.y);
      z = (int)Math.Floor(v.z);
      w = _w;
    }
    public BCMVector4(int x, int y, int z, int w)
    {
      this.x = x;
      this.y = y;
      this.z = z;
      this.w = w;
    }

    public override string ToString() => $"{x} {z} {y} {w}";

    public Vector3 ToV3() => new Vector3(8 + x * 16 + (z * 16 - x * 16) / 2, 0, 8 + y * 16 + (w * 16 - y * 16) / 2);

    public int GetRadius() => Math.Max((z - x) / 2, (w - y) / 2);
  }
}
