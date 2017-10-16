using System;
using UnityEngine;

namespace BCM.Models
{
  [Serializable]
  public class BCMVector3
  {
    public int x;
    public int y;
    public int z;
    public BCMVector3()
    {
      x = 0;
      y = 0;
      z = 0;
    }
    public BCMVector3(Vector3 v)
    {
      //todo: use floor instead?
      x = Mathf.RoundToInt(v.x);
      y = Mathf.RoundToInt(v.y);
      z = Mathf.RoundToInt(v.z);
    }
    public BCMVector3(Vector3i v)
    {
      x = v.x;
      y = v.y;
      z = v.z;
    }

    public BCMVector3(int x, int y, int z)
    {
      this.x = x;
      this.y = y;
      this.z = z;
    }

    public override string ToString()
    {
      return $"{x} {y} {z}";
    }


    public Vector3 ToV3()
    {
      return new Vector3(x + 0.5f, y + 0.5f, z + 0.5f);
    }
  }
}
