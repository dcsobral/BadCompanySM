using JetBrains.Annotations;

namespace BCM.Models
{
  public class BCMIngredient
  {
    [UsedImplicitly] public int Type;
    [UsedImplicitly] public int Count;

    public BCMIngredient(int type, int count)
    {
      Type = type;
      Count = count;
    }
  }
}
