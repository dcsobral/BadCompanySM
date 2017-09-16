using System;
using BCM.Models;
using System.Collections.Generic;
using UnityEngine;

namespace BCM.Commands
{
  public class BCChunkObserver : BCCommandAbstract
  {

    public override void Process()
    {
      if (Params.Count == 0)
      {
        SendOutput(GetHelp());

        return;
      }

      var world = GameManager.Instance.World;
      if (world == null)
      {
        SendOutput("World not loaded");

        return;
      }

      var observedEntities = world.m_ChunkManager.m_ObservedEntities;
      switch (Params[0])
      {
        case "list":
          GetList(observedEntities);
          break;

        case "spawn":
          var id = SpawnChunkObserver(observedEntities, world);
          SendOutput(id == -1 ? "Spawn CO failed" : $"Spawned CO: {id}");
          break;

        case "remove":
          DoRemove(observedEntities, world);
          break;

        case "move":
          DoMove(observedEntities, world);
          break;

        //case "radius":
        //  SetRadius(observedEntities, world);
        //  break;

        default:
          SendOutput($"Unknown param {Params[0]}");
          SendOutput(GetHelp());
          break;
      }
    }

    private int SpawnChunkObserver(ICollection<ChunkManager.ChunkObserver> observedEntities, World world)
    {
      if (Params.Count > 4 && int.TryParse(Params[1], out var x) && int.TryParse(Params[2], out var y) &&
          int.TryParse(Params[3], out var z) && int.TryParse(Params[4], out var viewDim))
        return world.m_ChunkManager.AddChunkObserver(new Vector3(x, y, z), false, viewDim, -1)?.id ?? -1;

      SendOutput("Unable to parse x y z for spawn");
      SendOutput(GetHelp());

      return -1;
    }

    //private void SetRadius(IEnumerable<ChunkManager.ChunkObserver> observedEntities, World world)
    //{
    //  if (Params.Count < 3)
    //  {
    //    SendOutput("Incorrect params count for setradius");
    //    SendOutput(GetHelp());

    //    return;
    //  }

    //  if (!int.TryParse(Params[1], out var id) || !int.TryParse(Params[2], out var r))
    //  {
    //    SendOutput("Unable to parse id or radius for setradius");
    //    SendOutput(GetHelp());

    //    return;
    //  }

    //  r = r < 0 ? 0 : r > 13 ? 13 : r;

    //  foreach (var oe in observedEntities)
    //  {
    //    if (oe.id != id) continue;

    //    if (IsPlayer(oe, world)) return;

    //    oe.viewDim = r;
    //    SendOutput($"Chunk observer {oe.id} view radius set to {r}");
    //    return;
    //  }

    //  SendOutput($"Unable to find observer with id: {id}");
    //}

    private void DoMove(IEnumerable<ChunkManager.ChunkObserver> observedEntities, World world)
    {
      if (Params.Count <= 4 || !int.TryParse(Params[1], out var id))
      {
        SendOutput("Incorrect params count for move");
        SendOutput(GetHelp());

        return;
      }

      if (!int.TryParse(Params[2], out var x) || !int.TryParse(Params[3], out var y) || !int.TryParse(Params[4], out var z))
      {
        SendOutput("Unable to parse x y z for move");
        SendOutput(GetHelp());

        return;
      }

      foreach (var oe in observedEntities)
      {
        if (oe.id != id) continue;

        if (IsPlayer(oe, world)) return;

        oe.SetPosition(new Vector3(x, y, z));
        SendOutput($"Moved chunk observer {oe.id} to {x} {y} {z}");
        return;
      }

      SendOutput($"Unable to find observer with id: {id}");
    }

    private static bool IsPlayer(ChunkManager.ChunkObserver oe, World world)
    {
      if (!world.Players.dict.ContainsKey(oe.entityIdToSendChunksTo)) return false;

      SendOutput("Can't change player observers");

      return true;
    }

    private void DoRemove(IEnumerable<ChunkManager.ChunkObserver> observedEntities, World world)
    {
      if (Params.Count <= 1 || !int.TryParse(Params[1], out var id))
      {
        SendOutput("Incorrect params count for remove");
        SendOutput(GetHelp());

        return;
      }

      foreach (var oe in observedEntities)
      {
        if (oe.id != id) continue;

        if (IsPlayer(oe, world)) return;

        world.m_ChunkManager.RemoveChunkObserver(oe);
        SendOutput($"Removed chunk observer {oe.id}");
        return;
      }

      SendOutput($"Unable to find observer with id: {id}");
    }

    private static void GetList(ICollection<ChunkManager.ChunkObserver> observedEntities)
    {
      SendOutput($"Count:{observedEntities.Count}");
      foreach (var oe in observedEntities)
      {
        SendOutput(oe.entityIdToSendChunksTo > 0
          ? $"id:{oe.id}, pos:{Math.Round(oe.position.x, 0)} {Math.Round(oe.position.y, 0)} {Math.Round(oe.position.z, 0)}, dim:{oe.viewDim}, chunks:{oe.chunksLoaded.Count}/{oe.chunksLoaded.Count + oe.chunksToLoad.list.Count}, entity:{oe.entityIdToSendChunksTo}"
          : $"id:{oe.id}, pos:{Math.Round(oe.position.x, 0)} {Math.Round(oe.position.y, 0)} {Math.Round(oe.position.z, 0)}, dim:{oe.viewDim}, chunks:{oe.chunksToLoad.list.Count}, entity:{oe.entityIdToSendChunksTo}");
      }
    }
  }
}
