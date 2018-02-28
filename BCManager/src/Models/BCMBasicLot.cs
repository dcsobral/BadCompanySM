namespace BCM.Models
{
  public class BCMBasicLot
  {
    public string Prefab;
    public string Position;

    public BCMBasicLot(BCMLot lot)
    {
      Prefab = lot.Prefab;
      Position = $"{lot.Position.x} {lot.Position.y} {lot.Position.z}";
    }
  }
}