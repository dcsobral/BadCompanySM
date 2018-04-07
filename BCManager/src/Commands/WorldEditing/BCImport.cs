using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Random = System.Random;

namespace BCM.Commands
{
  //todo: add feedback when undo is processed
  //todo: if undo is out of range or only partially loaded, rollback and infom to get closer
  //todo: add a redo feature
  //todo: set owner if option set
  //todo: remove loot container contents before inserting new prefab, optional flag
  //todo: create chunk observer and spawn prefab once chunks are loaded?
  //todo: use loc position for imports with /loc option

  // todo: create a copy of the chunks in the bounded dimensions of the prefab size for an undo
  //       should work better than a prefab copy undo as it will preserve block ownership and state?
  // optional todo: option to carve terrain where prefab will spawn, maybe reapply decorations?
  //                insert lot into cell data and regenerate chunk area from world/seed defaults

  // optional todo: allow for partial names for prefab, provide list if more then one result, allow for partial + # from list to specify
  // optional todo: refresh nearest prefab
  // optional todo: store last x commands for each player, with linked undo data add a repeat option so that prefab inserts at a players location can be repeated at the same location even if player moves

  [UsedImplicitly]
  public class BCImport : BCCommandAbstract
  {
    protected override void Process()
    {
      if (!BCUtils.CheckWorld(out var world)) return;

      if (Options.ContainsKey("undo"))
      {
        SendOutput("Please use the bc-undo command to undo changes");

        return;
      }

      EntityPlayer sender = null;
      if (SenderInfo.RemoteClientInfo != null)
      {
        sender = world.Entities.dict[SenderInfo.RemoteClientInfo.entityId] as EntityPlayer;
      }

      if (Params.Count == 0) return;

      var prefab = new Prefab();
      if (!prefab.Load(Params[0])) return;

      if (!GetRxyz(prefab, sender, out var r, out var x, out var y, out var z)) return;

      var pos = Options.ContainsKey("nooffset") ? new Vector3i(x, y, z) : new Vector3i(x, y + prefab.yOffset, z);

      if (Options.ContainsKey("air"))
      {
        prefab.bCopyAirBlocks = true;
      }
      if (Options.ContainsKey("noair"))
      {
        prefab.bCopyAirBlocks = false;
      }

      BlockTranslations(world, prefab, pos);

      //CREATE UNDO
      //create backup of area prefab will insert to
      if (!Options.ContainsKey("noundo"))
      {
        BCUtils.CreateUndo(sender, pos, prefab.size);
      }

      // SPAWN PREFAB
      Log.Out($"{Config.ModPrefix}Spawning prefab {prefab.filename} @ {pos}, size={prefab.size}, rot={r}");
      SendOutput($"Spawning prefab {prefab.filename} @ {pos}, size={prefab.size}, rot={r}");
      if (Options.ContainsKey("sblock") || Options.ContainsKey("editmode"))
      {
        SendOutput("* with Sleeper Blocks option set");
      }
      else
      {
        SendOutput("* with Sleeper Spawning option set");
      }
      if (Options.ContainsKey("tfill") || Options.ContainsKey("editmode"))
      {
        SendOutput("* with Terrain Filler option set");
      }
      if (Options.ContainsKey("notrans") || Options.ContainsKey("editmode"))
      {
        SendOutput("* with No Placeholder Translations option set");
      }
      SendOutput("use bc-undo to revert the changes");

      InsertPrefab(world, prefab, pos);
    }

    public static void InsertPrefab(World world, Prefab prefab, Vector3i pos)
    {
      if (prefab == null)
      {
        SendOutput("No Prefab loaded.");

        return;
      }

      //GET AFFECTED CHUNKS
      var modifiedChunks = new Dictionary<long, Chunk>();
      for (var cx = -1; cx <= prefab.size.x + 16; cx = cx + 16)
      {
        for (var cz = -1; cz <= prefab.size.z + 16; cz = cz + 16)
        {
          if (world.GetChunkFromWorldPos(pos.x + cx, 0, pos.z + cz) is Chunk chunk)
          {
            if (modifiedChunks.ContainsKey(chunk.Key)) continue;

            modifiedChunks.Add(chunk.Key, chunk);
          }
          else
          {
            SendOutput($"Unable to load chunk for prefab @ {pos.x + cx},{pos.z + cz}");
          }
        }
      }

      //INSERT PREFAB
      CopyIntoLocal(world, prefab, pos);

      //RELOAD CHUNKS
      if (!(Options.ContainsKey("noreload") || Options.ContainsKey("nr")))
      {
        BCChunks.ReloadForClients(modifiedChunks);
      }
    }

    private bool GetRxyz(Prefab prefab, Entity entity, out int r, out int x, out int y, out int z)
    {
      r = 0;
      x = 0;
      y = 0;
      z = 0;

      if (entity == null && Params.Count < 4)
      {
        SendOutput("Command requires <name> <x> <y> <z> params if not sent by an online player");
      }
      else if (entity != null)
      {
        var loc = new Vector3i((int)Math.Floor(entity.serverPos.x / 32f), (int)Math.Floor(entity.serverPos.y / 32f), (int)Math.Floor(entity.serverPos.z / 32f));

        x = loc.x - prefab.size.x / 2;
        y = loc.y;
        z = loc.z - prefab.size.z / 2;
        if (Options.ContainsKey("csw") || Options.ContainsKey("ne"))
        {
          x = loc.x;
          z = loc.z;
        }
        else if (Options.ContainsKey("cse") || Options.ContainsKey("nw"))
        {
          x = loc.x - prefab.size.x;
          z = loc.z;
        }
        else if (Options.ContainsKey("cnw") || Options.ContainsKey("se"))
        {
          x = loc.x;
          z = loc.z - prefab.size.z;
        }
        else if (Options.ContainsKey("cne") || Options.ContainsKey("sw"))
        {
          x = loc.x - prefab.size.x;
          z = loc.z - prefab.size.z;
        }
      }

      if (Params.Count > 1)
      {
        switch (Params.Count)
        {
          case 2:
            if (!int.TryParse(Params[1], out r))
            {
              SendOutput("<rot> param could not be parsed as a number.");

              return false;
            }

            break;

          case 4:
            // specific spawnpoint
            if (!int.TryParse(Params[1], out x) || !int.TryParse(Params[2], out y) || !int.TryParse(Params[3], out z))
            {
              SendOutput("One of <x> <y> <z> params could not be parsed as a number.");

              return false;
            }
            r = 0;

            break;

          case 5:
            // specific spawnpoint
            if (!int.TryParse(Params[1], out x) || !int.TryParse(Params[2], out y) || !int.TryParse(Params[3], out z) || !int.TryParse(Params[4], out r))
            {
              SendOutput("One of <x> <y> <z> <rot> params could not be parsed as a number.");

              return false;
            }

            break;

          default:
            if (Params.Count != 6)
            {
              SendOutput("Error: Incorrect command format.");
              SendOutput(GetHelp());

              return false;
            }

            break;
        }
        // spin the prefab
        prefab.RotateY(false, r % 4);
      }

      //bounds
      if (y < 0)
      {
        SendOutput($"Y position is too low by {y * -1} blocks");

        return false;
      }
      if (y + prefab.size.y > 255)
      {
        SendOutput($"Y position is too high by {y + prefab.size.y - 255} blocks");

        return false;
      }

      return true;
    }

    private static void BlockTranslations(World world, Prefab prefab, Vector3i pos)
    {
      // ENTITIES
      var entities = new List<int>();
      prefab.CopyEntitiesIntoWorld(world, pos, entities, !(Options.ContainsKey("noent") || Options.ContainsKey("editmode")));

      //BLOCK TRANSLATIONS
      if (Options.ContainsKey("notrans") || Options.ContainsKey("editmode")) return;

      var map = LootContainer.lootPlaceholderMap;
      for (var px = 0; px < prefab.size.x; px++)
      {
        for (var py = 0; py < prefab.size.y; py++)
        {
          for (var pz = 0; pz < prefab.size.z; pz++)
          {
            var bv = prefab.GetBlock(px, py, pz);
            // LOOT PLACEHOLDERS
            if (bv.type == 0) continue;

            var bvr = new BlockValue(map.Replace(bv, new Random(Guid.NewGuid().GetHashCode())).rawData);
            if (bv.type == bvr.type) continue;

            prefab.SetBlock(px, py, pz, bvr);
          }
        }
      }
    }

    private static void AddSleeperSpawns(Prefab prefab, int idx, Vector3i dest, SleeperVolume volume, Vector3i volStart, Vector3i volMax)
    {
      var startX = Mathf.Max(volStart.x, 0);
      var startY = Mathf.Max(volStart.y, 0);
      var startZ = Mathf.Max(volStart.z, 0);
      var sizeX = Mathf.Min(prefab.size.x, volMax.x);
      var sizeY = Mathf.Min(prefab.size.y, volMax.y);
      var sizeZ = Mathf.Min(prefab.size.z, volMax.z);
      for (var x = startX; x < sizeX; ++x)
      {
        var destX = x + dest.x;
        for (var z = startZ; z < sizeZ; ++z)
        {
          var destZ = z + dest.z;
          for (var y = startY; y < sizeY; ++y)
          {
            var blockValue = prefab.GetBlock(x, y, z);
            var block = Block.list[blockValue.type];
            if (!block.IsSleeperBlock) continue;

            var flag = false;
            var point = new Vector3i(x, y, z);
            for (var index = 0; index < prefab.SleeperVolumesStart.Count; ++index)
            {
              if (index == idx || !prefab.SleeperVolumeUsed[index] || !prefab.SleeperIsLootVolume[index] ||
                !prefab.CheckSleeperVolumesContainPoint(index, point)) continue;

              flag = true;
              break;
            }
            if (!flag)
              volume.AddSpawnPoint(destX, y + dest.y, destZ, (BlockSleeper)block, blockValue);
          }
        }
      }
    }

    private static void ProcessSleepers(Prefab prefab, World world, Vector3i dest)
    {
      for (var index = 0; index < prefab.SleeperVolumesStart.Count; ++index)
      {
        if (!prefab.SleeperVolumeUsed[index]) continue;

        var volStart = prefab.SleeperVolumesStart[index];
        var volSize = prefab.SleeperVolumesSize[index];
        var volMax = volStart + volSize;
        var volStartDest = volStart + dest;
        var volMaxDest = volMax + dest;
        var chunkXz1 = World.toChunkXZ(volStartDest.x);
        var chunkXz2 = World.toChunkXZ(volMaxDest.x);
        var chunkXz3 = World.toChunkXZ(volStartDest.z);
        var chunkXz4 = World.toChunkXZ(volMaxDest.z);
        var volume = SleeperVolume.Create(prefab.SleeperVolumesGroup[index], volStartDest, volMaxDest, dest, prefab.SleeperVolumeGameStageAdjust[index]);
        var volKey = world.AddSleeperVolume(volume);
        AddSleeperSpawns(prefab, index, dest, volume, volStart, volMax);
        for (var chunkX = chunkXz1; chunkX <= chunkXz2; ++chunkX)
        {
          for (var chunkZ = chunkXz3; chunkZ <= chunkXz4; ++chunkZ)
          {
            ((Chunk)world.GetChunkSync(chunkX, 0, chunkZ))?.GetSleeperVolumes().Add(volKey);
          }
        }
      }
    }

    private static void CopyIntoLocal(World world, Prefab prefab, Vector3i dest)
    {
      if (!Options.ContainsKey("sblocks"))
      {
        ProcessSleepers(prefab, world, dest);
      }

      var chunkSync = world.ChunkCache.GetChunkSync(World.toChunkXZ(dest.x), World.toChunkXZ(dest.z));

      if (!Options.ContainsKey("refresh"))
      {
        for (var x = 0; x < prefab.size.x; ++x)
        {
          for (var z = 0; z < prefab.size.z; ++z)
          {
            var chunkX = World.toChunkXZ(x + dest.x);
            var chunkZ = World.toChunkXZ(z + dest.z);
            var blockX = World.toBlockXZ(x + dest.x);
            var blockZ = World.toBlockXZ(z + dest.z);
            if (chunkSync == null || chunkSync.X != chunkX || chunkSync.Z != chunkZ)
            {
              chunkSync = world.ChunkCache.GetChunkSync(chunkX, chunkZ);
            }

            if (chunkSync == null) continue;

            for (var y = 0; y < prefab.size.y; ++y)
            {
              var blockY = World.toBlockY(y + dest.y);
              var chunkBlock = chunkSync.GetBlock(blockX, blockY, blockZ);

              //REMOVE PARENT OF MULTIDIM
              if (!chunkBlock.Block.isMultiBlock || !chunkBlock.ischild) continue;

              var parentPos = chunkBlock.Block.multiBlockPos.GetParentPos(new Vector3i(dest.x + x, dest.y + y, dest.z + z), chunkBlock);
              var parent = world.ChunkClusters[0].GetBlock(parentPos);
              if (parent.ischild || parent.type != chunkBlock.type) continue;

              world.ChunkClusters[0].SetBlock(parentPos, BlockValue.Air, false, false);
            }
          }
        }
      }

      for (var x = 0; x < prefab.size.x; ++x)
      {
        for (var z = 0; z < prefab.size.z; ++z)
        {
          var chunkX = World.toChunkXZ(x + dest.x);
          var chunkZ = World.toChunkXZ(z + dest.z);
          var blockX = World.toBlockXZ(x + dest.x);
          var blockZ = World.toBlockXZ(z + dest.z);
          if (chunkSync == null || chunkSync.X != chunkX || chunkSync.Z != chunkZ)
          {
            chunkSync = world.ChunkCache.GetChunkSync(chunkX, chunkZ);
          }

          if (chunkSync == null) continue;

          var terrainHeight = (int)chunkSync.GetTerrainHeight(blockX, blockZ);

          for (var y = 0; y < prefab.size.y; ++y)
          {
            var prefabBlock = prefab.GetBlock(x, y, z);

            //SLEEPER BLOCKS
            if (!(Options.ContainsKey("sblocks") || Options.ContainsKey("editmode")) && Block.list[prefabBlock.type].IsSleeperBlock)
            {
              prefabBlock = BlockValue.Air;
            }

            //COPY AIR
            if (!prefab.bCopyAirBlocks && prefabBlock.type == 0) continue;

            var blockY = World.toBlockY(y + dest.y);
            var chunkBlock = chunkSync.GetBlock(blockX, blockY, blockZ);

            //REMOVE LCB's
            if (chunkBlock.Block.IndexName == "lpblock")
            {
              GameManager.Instance.persistentPlayers.RemoveLandProtectionBlock(new Vector3i(x, y, z));
            }

            //REMOVE LOOT - Make optional for reimporting same prefab over existing loot blocks
            if (Options.ContainsKey("clearloot"))
            {

            }

            //TERRAIN FILLER
            if (!(Options.ContainsKey("tfill") || Options.ContainsKey("editmode")) && Constants.cTerrainFillerBlockValue.type != 0 && prefabBlock.type == Constants.cTerrainFillerBlockValue.type)
            {
              if (chunkBlock.type == 0 || Block.list[chunkBlock.type] == null || !Block.list[chunkBlock.type].shape.IsTerrain()) continue;

              prefabBlock = chunkBlock;
            }

            //DENSITY
            //todo: validate density?
            var density = prefab.GetDensity(x, y, z);
            if (density == 0)
            {
              density = !Block.list[prefabBlock.type].shape.IsTerrain() ? MarchingCubes.DensityAir : MarchingCubes.DensityTerrain;
            }
            chunkSync.SetDensity(blockX, blockY, blockZ, density);

            if (chunkBlock.ischild) continue;

            //DECORATIONS
            if (prefab.bAllowTopSoilDecorations)
            {
              chunkSync.SetDecoAllowedAt(blockX, blockZ, EnumDecoAllowed.NoBigOnlySmall);
            }
            else
            {
              if (blockY >= terrainHeight + 1)
              {
                chunkSync.SetDecoAllowedAt(blockX, blockZ, EnumDecoAllowed.NoBigNoSmall);
              }
              else if (blockY == terrainHeight)
              {
                chunkSync.SetDecoAllowedAt(blockX, blockZ, EnumDecoAllowed.Nothing);
              }
            }

            //TRADER SPAWNS

            //SET OWNER

            //***** SET BLOCK *****
            chunkSync.SetTextureFull(blockX, blockY, blockZ, prefab.GetTexture(x, y, z));
            chunkSync.SetBlock(world, blockX, blockY, blockZ, prefabBlock);

            //SET HEIGHT
            if (Block.list[prefabBlock.type].shape.IsTerrain() && chunkSync.GetTerrainHeight(blockX, blockZ) < blockY)
            {
              chunkSync.SetTerrainHeight(blockX, blockZ, (byte)blockY);
            }
          }
          //todo: should this be here? is it needed?
          chunkSync.SetDecoAllowedAt(blockX, blockZ, EnumDecoAllowed.NoBigOnlySmall);
        }
      }
    }
  }
}
