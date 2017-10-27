using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BCM.Models
{
  public class BCMSleeperVolumeDetialed : BCMSleeperVolume
  {
    public List<BCMSpawnPoint> SpawnPoints;

    public BCMSleeperVolumeDetialed(int index, SleeperVolume volume, World world) : base(index, volume, world)
    {
      var spawns = typeof(SleeperVolume).GetField("VH", BindingFlags.NonPublic | BindingFlags.Instance);
      if (spawns != null)
      {
        if (!(spawns.GetValue(volume) is List<SleeperVolume.SpawnPoint> spawnPoints)) return;
        SpawnPoints = spawnPoints.Select(sp => new BCMSpawnPoint(sp)).ToList();
      }

      //todo: list active spawns and counters
    }
  }

  public class BCMSleeperVolume
  {
    public class BCMSpawnPoint
    {
      public BCMVector3 Pos;
      public BCMVector3 Look;
      public double Rot;
      public int Pose;
      public int BlockId;

      public BCMSpawnPoint(SleeperVolume.SpawnPoint spawn)
      {
        Pos = new BCMVector3(spawn.pos);
        Look = new BCMVector3(spawn.look);
        Rot = spawn.rot;
        Pose = spawn.pose;
        BlockId = spawn.blockID;
      }
    }

    public int Index;
    public string Group;
    public BCMVector3 Position;
    public BCMVector3 Extent;
    public BCMVector3 Size;
    public double RespawnTimer;

    public BCMSleeperVolume(int index, SleeperVolume volume, World world)
    {
      Index = index;
      var volumeGroup = typeof(SleeperVolume).GetField("QH", BindingFlags.NonPublic | BindingFlags.Instance);
      if (volumeGroup != null)
      {
        if (!(volumeGroup.GetValue(volume) is string name)) return;
        Group = name;
      }

      Position = new BCMVector3(volume.mins);
      Extent = new BCMVector3(volume.maxs);
      Size = new BCMVector3(volume.maxs - volume.mins + Vector3i.one);

      var timer = typeof(SleeperVolume).GetField("IH", BindingFlags.NonPublic | BindingFlags.Instance);
      if (timer != null)
      {
        if (!(timer.GetValue(volume) is ulong timerValue)) return;

        if (timerValue > world.worldTime)
        {
          RespawnTimer = Math.Round((timerValue - world.worldTime) / 24000f, 2);
        }
      }
    }
  }
}
