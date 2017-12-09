using System;
using System.Collections.Generic;
using System.Reflection;
using BCM.Models;

namespace BCM.Commands
{
  public class BCSleeper : BCCommandAbstract
  {
    private const string _volumesFunction = "LW";

    public override void Process()
    {
      if (Params.Count == 0)
      {
        SendOutput(GetHelp());

        return;
      }

      var world = GameManager.Instance.World;
      if (world == null) return;

      switch (Params[0])
      {
        case "chunk":
          {
            if (!GetChunkKey(world, out var chunkKey)) return;
            ListVolumes(world, chunkKey);
            return;
          }

        case "clear":
          {
            if (!GetChunkKey(world, out var chunkKey)) return;
            ClearChunkVolume(world, chunkKey);
            return;
          }

        case "list":
          {
            GetWorldVolumes(world);
            return;
          }

        case "volume":
          {
            GetWorldVolume(world);

            return;
          }

        default:
          SendOutput(GetHelp());

          return;
      }
    }

    private static void GetWorldVolumes(World world)
    {
      var sleeperVolumes = typeof(World).GetField(_volumesFunction, BindingFlags.NonPublic | BindingFlags.Instance);
      if (sleeperVolumes != null)
      {
        if (!(sleeperVolumes.GetValue(world) is List<SleeperVolume> volumes)) return;

        var i = 0;
        var volumeList = new List<BCMSleeperVolume>();
        foreach (var vol in volumes)
        {
          volumeList.Add(new BCMSleeperVolume(i, vol, world));
          i++;
        }
        SendJson(new
        {
          Total = volumeList.Count,
          Volumes = volumeList
        });
      }
      else
      {
        SendOutput("Couldn't get sleeper volumes from world");
      }
    }

    private void GetWorldVolume(World world)
    {
      if (Params.Count != 2)
      {
        SendOutput(GetHelp());
        return;
      }
      var sleeperVolumes = typeof(World).GetField(_volumesFunction, BindingFlags.NonPublic | BindingFlags.Instance);
      if (sleeperVolumes != null)
      {
        if (!(sleeperVolumes.GetValue(world) is List<SleeperVolume> volumes)) return;

        if (int.TryParse(Params[1], out var i))
        {
          SendJson(new BCMSleeperVolumeDetialed(i, volumes[i], world));
        }
        else
        {
          SendOutput("Unable to find given index");
        }
      }
      else
      {
        SendOutput("Couldn't get sleeper volumes from world");
      }
    }

    private static void ClearChunkVolume(World world, long chunkKey)
    {
      var iChunk = world.GetChunkSync(chunkKey);
      if (!(iChunk is Chunk chunk))
      {
        SendOutput("Unable to retrieve chunk");

        return;
      }

      chunk.GetSleeperVolumes().Clear();
      SendOutput($"Sleeper volumes cleared from {chunk.X},{chunk.Z}");
    }

    private static void ListVolumes(World world, long chunkKey)
    {
      var iChunk = world.GetChunkSync(chunkKey);
      if (!(iChunk is Chunk chunk))
      {
        SendOutput("Unable to retrieve chunk");

        return;
      }

      var sleepers = chunk.GetSleeperVolumes();
      SendJson(new { msg = $"Sleeper volume array indexes for chunk {chunk.X},{chunk.Z}", data = sleepers });
    }

    private static bool GetChunkKey(World world, out long chunkKey)
    {
      chunkKey = long.MinValue;

      switch (Params.Count)
      {
        case 3:
          {
            if (!int.TryParse(Params[1], out var x) || !int.TryParse(Params[2], out var z))
            {
              SendOutput("Unable to parse x z for numbers");

              return false;
            }

            chunkKey = WorldChunkCache.MakeChunkKey(x, z);

            return true;
          }
        default:
          {
            if (SenderInfo.RemoteClientInfo == null)
            {
              SendOutput("Unable to get location from player position");

              return false;
            }

            var entityId = SenderInfo.RemoteClientInfo.entityId;
            if (!world.Players.dict.ContainsKey(entityId))
            {
              SendOutput("Unable to get location from player position");

              return false;
            }

            var ep = world.Players.dict[entityId];
            chunkKey = WorldChunkCache.MakeChunkKey((int)Math.Floor(ep.position.x) >> 4, (int)Math.Floor(ep.position.z) >> 4);

            return true;
          }
      }
    }
  }
}
