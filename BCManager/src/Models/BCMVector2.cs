using UnityEngine;

namespace BCM.Commands
{
  public class BCMVector2
  {
    public int x;
    public int y;
    public BCMVector2()
    {
      x = 0;
      y = 0;
    }
    public BCMVector2(int x, int y)
    {
      this.x = x;
      this.y = y;
    }
    public BCMVector2(Vector2 v)
    {
      x = Mathf.RoundToInt(v.x);
      y = Mathf.RoundToInt(v.y);
    }
    public BCMVector2(Vector2i v)
    {
      x = v.x;
      y = v.y;
    }

    public override string ToString()
    {
      return $"{x} {y}";
    }
  }
}
