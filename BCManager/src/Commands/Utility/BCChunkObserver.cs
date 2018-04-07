using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace BCM.Commands
{
  [UsedImplicitly]
  public class BCChunkObserver : BCCommandAbstract
  {
    protected override void Process()
    {
      if (!BCUtils.CheckWorld(out var world)) return;

      if (Params.Count == 0)
      {
        SendOutput(GetHelp());

        return;
      }

      switch (Params[0])
      {
        case "list":
          GetList(world.m_ChunkManager.m_ObservedEntities);
          break;

        case "spawn":
          var id = SpawnChunkObserver(world);
          SendOutput(id == -1 ? "Spawn CO failed" : $"Spawned CO: {id}");
          break;

        case "remove":
          DoRemove(world.m_ChunkManager.m_ObservedEntities, world);
          break;

        case "move":
          DoMove(world.m_ChunkManager.m_ObservedEntities);
          break;

        case "reload":
          DoReload();
          break;

        default:
          SendOutput($"Unknown param {Params[0]}");
          SendOutput(GetHelp());
          break;
      }
    }

    private int SpawnChunkObserver(World world)
    {
      if (Params.Count > 3 && int.TryParse(Params[1], out var x) && int.TryParse(Params[2], out var z) && int.TryParse(Params[3], out var viewDim))
        return world.m_ChunkManager.AddChunkObserver(new Vector3(x, 0, z), false, viewDim, -1)?.id ?? -1;

      SendOutput("Unable to parse x or z for spawn");
      SendOutput(GetHelp());

      return -1;
    }

    private void DoMove(IEnumerable<ChunkManager.ChunkObserver> observedEntities)
    {
      if (Params.Count <= 3 || !int.TryParse(Params[1], out var id))
      {
        SendOutput("Incorrect params count for move");
        SendOutput(GetHelp());

        return;
      }

      if (!int.TryParse(Params[2], out var x) || !int.TryParse(Params[3], out var z))
      {
        SendOutput("Unable to parse x or z for move");
        SendOutput(GetHelp());

        return;
      }

      foreach (var oe in observedEntities)
      {
        if (oe.id != id) continue;

        if (IsPlayer(oe)) return;

        oe.SetPosition(new Vector3(x, 0, z));
        SendOutput($"Moved chunk observer {oe.id} to {x} {z}");
        return;
      }

      SendOutput($"Unable to find observer with id: {id}");
    }

    private static bool IsPlayer(ChunkManager.ChunkObserver oe) => GameManager.Instance.World.Players.dict.ContainsKey(oe.entityIdToSendChunksTo);

    private void DoRemove(IEnumerable<ChunkManager.ChunkObserver> observedEntities, World world)
    {
      if (Params.Count <= 1 || !int.TryParse(Params[1], out var id))
      {
        SendOutput("Incorrect params count for remove");
        SendOutput(GetHelp());

        return;
      }

      foreach (var observer in observedEntities)
      {
        if (observer.id != id) continue;

        if (IsPlayer(observer)) return;

        world.m_ChunkManager.RemoveChunkObserver(observer);
        SendOutput($"Removed chunk observer {observer.id}");
        return;
      }

      SendOutput($"Unable to find observer with id: {id}");
    }

    private void DoReload()
    {
      if (Params.Count != 5 && Params.Count != 3)
      {
        SendOutput("Incorrect params count for reload");
        SendOutput(GetHelp());

        return;
      }

      int cx;
      int cy;
      int cz;
      int cw;

      if (Params.Count == 3)
      {
        if (!int.TryParse(Params[1], out cx) || !int.TryParse(Params[2], out cy))
        {
          SendOutput("Unable to parse chunk co-ords from params");
          SendOutput(GetHelp());

          return;
        }
        cz = cx;
        cw = cy;
      }
      else
      {
        if (!int.TryParse(Params[1], out cx) || !int.TryParse(Params[2], out cy) ||
            !int.TryParse(Params[3], out cz) || !int.TryParse(Params[4], out cw))
        {
          SendOutput("Unable to parse chunk co-ords from params");
          SendOutput(GetHelp());

          return;
        }
      }

      var x1 = Math.Min(cx, cz);
      var x2 = Math.Max(cx, cz);
      var z1 = Math.Min(cy, cw);
      var z2 = Math.Max(cy, cw);

      var chunks = new Dictionary<long, Chunk>();
      for (var x = x1; x <= x2; x++)
      {
        for (var z = z1; z <= z2; z++)
        {
          var chunkKey = WorldChunkCache.MakeChunkKey(x, z);
          chunks[chunkKey] = null;
        }
      }
      var count = BCChunks.ReloadForClients(chunks);
      SendOutput($"Chunks reloaded for {count} clients in area: {x1},{z1} to {x2},{z2}");
    }

    private static void GetList(ICollection<ChunkManager.ChunkObserver> observedEntities)
    {
      SendOutput($"Count:{observedEntities.Count}");
      foreach (var observer in observedEntities)
      {
        SendOutput($"id:{observer.id}, pos:{Math.Round(observer.position.x, 0)} {Math.Round(observer.position.z, 0)}, dim:{observer.viewDim}, chunks:" 
          + (observer.entityIdToSendChunksTo > 0
          ? $"{observer.chunksLoaded.Count}/{observer.chunksLoaded.Count + observer.chunksToLoad.list.Count}"
          : $"{observer.chunksToLoad.list.Count}") 
          + $", entity:{observer.entityIdToSendChunksTo}");
      }
    }
  }
}
