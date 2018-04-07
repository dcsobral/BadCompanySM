using System;
using System.Reflection;
using JetBrains.Annotations;

namespace BCM.Models
{
  public class BCMSleeperVolume
  {
    //todo: find field by params and types
    private const string VolumeGroupFieldName = "QH";
    private const string TimerFieldName = "IH";

    [UsedImplicitly] public int Index;
    [UsedImplicitly] public string Group;
    [UsedImplicitly] public BCMVector3 Position;
    [UsedImplicitly] public BCMVector3 Extent;
    [UsedImplicitly] public BCMVector3 Size;
    [UsedImplicitly] public double RespawnTimer;

    public BCMSleeperVolume(int index, [NotNull] SleeperVolume volume, [NotNull] World world)
    {
      Index = index;
      var volumeGroup = typeof(SleeperVolume).GetField(VolumeGroupFieldName, BindingFlags.NonPublic | BindingFlags.Instance);
      if (volumeGroup != null)
      {
        if (!(volumeGroup.GetValue(volume) is string name)) return;
        Group = name;
      }

      Position = new BCMVector3(volume.mins);
      Extent = new BCMVector3(volume.maxs);
      Size = new BCMVector3(volume.maxs - volume.mins + Vector3i.one);

      var timer = typeof(SleeperVolume).GetField(TimerFieldName, BindingFlags.NonPublic | BindingFlags.Instance);
      if (timer == null) return;

      if (!(timer.GetValue(volume) is ulong timerValue)) return;

      if (timerValue > world.worldTime)
      {
        RespawnTimer = Math.Round((timerValue - world.worldTime) / 24000f, 2);
      }
    }
  }
}
