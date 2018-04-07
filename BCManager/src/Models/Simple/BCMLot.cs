using JetBrains.Annotations;

namespace BCM.Models
{
  public class BCMLot
  {
    [UsedImplicitly] public string Prefab;
    [UsedImplicitly] public string Township;
    [UsedImplicitly] public string Type;
    [UsedImplicitly] public int InstanceId;
    [UsedImplicitly] public BCMVector3 Position;
    [UsedImplicitly] public int Rotation;
    [UsedImplicitly] public string LotType;

    public BCMLot([NotNull] BCMLot lot)
    {
      Prefab = lot.Prefab;
      Township = lot.Township;
      Type = lot.Type;
      InstanceId = lot.InstanceId;
      Position = lot.Position;
      Rotation = lot.Rotation;
      LotType = lot.LotType;
    }

    public BCMLot([NotNull] RWG2.HubCell.Lot lot, BCMLotType type)
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