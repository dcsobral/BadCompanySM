using JetBrains.Annotations;

namespace BCM.Models
{
  public class BCMPrefabSleeperVolume
  {
    [UsedImplicitly] public bool Used;
    [UsedImplicitly] public BCMVector3 Start;
    [UsedImplicitly] public BCMVector3 Size;
    [UsedImplicitly] public string Group;
    [UsedImplicitly] public string Adjust;
    [UsedImplicitly] public bool IsLoot;

    public BCMPrefabSleeperVolume(Prefab prefab, int x)
    {
      Used = prefab.SleeperVolumeUsed[x];
      Start = new BCMVector3(prefab.SleeperVolumesStart[x]);
      Size = new BCMVector3(prefab.SleeperVolumesSize[x]);
      Group = prefab.SleeperVolumesGroup[x];
      Adjust = prefab.SleeperVolumeGameStageAdjust[x];
      IsLoot = prefab.SleeperIsLootVolume[x];
    }
  }
}
