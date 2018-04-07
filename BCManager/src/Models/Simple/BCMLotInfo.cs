using RWG2.Rules;
using JetBrains.Annotations;

namespace BCM.Models
{
  public class BCMLotInfo
  {
    [UsedImplicitly] public string Name;
    [UsedImplicitly] public BCMVector2 Pos;
    [UsedImplicitly] public BCMVector2 Size;
    [UsedImplicitly] public int Rot;
    [UsedImplicitly] public string Prefab;
    [UsedImplicitly] public string Zoning;
    [UsedImplicitly] public string Cond;
    [UsedImplicitly] public string Age;

    public BCMLotInfo(HubLayout.LotInfo lotInfo)
    {
      Name = lotInfo.Name;
      Pos = new BCMVector2(lotInfo.Position);
      Size = new BCMVector2(lotInfo.Size);
      Rot = lotInfo.RotationFromNorth;
      Prefab = lotInfo.PrefabName;
      Zoning = lotInfo.Zoning.ToString();
      Cond = lotInfo.Condition;
      Age = lotInfo.Age;
    }
  }
}
