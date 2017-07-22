using UnityEngine;

namespace BCM
{
  public class BCMVector3
  {
    public float x;
    public float y;
    public float z;
    
    public override string ToString()
    {
      return x.ToString("0f") + " " + y.ToString("0f") + " " + z.ToString("0f");
    }
  }
}
