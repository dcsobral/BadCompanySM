using JetBrains.Annotations;

namespace BCM.Models
{
  public class BCMBasicLot
  {
    [UsedImplicitly] public string Prefab;
    [UsedImplicitly] public string Position;

    public BCMBasicLot(BCMLot lot)
    {
      Prefab = lot.Prefab;
      Position = $"{lot.Position.x} {lot.Position.y} {lot.Position.z}";
    }
  }
}