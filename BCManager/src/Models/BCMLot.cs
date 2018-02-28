namespace BCM.Models
{
  public class BCMLot
  {
    public string Prefab;
    public string Township;
    public string Type;
    public int InstanceId;
    public BCMVector3 Position;
    public int Rotation;
    public string LotType;

    public BCMLot(BCMLot lot)
    {
      Prefab = lot.Prefab;
      Township = lot.Township;
      Type = lot.Type;
      InstanceId = lot.InstanceId;
      Position = lot.Position;
      Rotation = lot.Rotation;
      LotType = lot.LotType;
    }

    public BCMLot(RWG2.HubCell.Lot lot, BCMLotType type)
    {
      Prefab = lot.PrefabName;
      Township = lot.Township.ToString();
      Type = lot.Type.ToString();
      InstanceId = lot.PrefabInstance.id;
      Position = new BCMVector3(lot.PrefabSpawnPos);
      Rotation = lot.RoadDirection;
      LotType = type.ToString();
    }
  }
}