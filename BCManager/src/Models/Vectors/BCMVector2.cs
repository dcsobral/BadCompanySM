using System;
using UnityEngine;

namespace BCM.Models
{
  public class BCMVector2
  {
    public readonly int x;
    public readonly int y;

    public BCMVector2(Vector2 v)
    {
      x = (int)Math.Floor(v.x);
      y = (int)Math.Floor(v.y);
    }
    public BCMVector2(Vector2i v)
    {
      x = v.x;
      y = v.y;
    }

    public override string ToString() => $"{x} {y}";
  }
}
