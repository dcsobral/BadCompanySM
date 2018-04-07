using System;
using JetBrains.Annotations;
using UnityEngine;

namespace BCM.Models
{
  public class BCMVector2d
  {
    [UsedImplicitly] public double x;
    [UsedImplicitly] public double y;

    public BCMVector2d(Vector2 v)
    {
      x = Math.Round(v.x, 3);
      y = Math.Round(v.y, 3);
    }
  }
}
