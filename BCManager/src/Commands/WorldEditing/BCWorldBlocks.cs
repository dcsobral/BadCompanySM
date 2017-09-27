using System;
using System.Collections.Generic;
using System.IO;

namespace BCM.Commands
{
  public class BCWorldBlocks : BCCommandAbstract
  {
    private const string UndoDir = "Data/Prefabs/BCMUndoCache";

    private class PrefabCache
    {
      public string Filename;
      public Vector3i Pos;
    }
    private readonly Dictionary<int, List<PrefabCache>> _undoCache = new Dictionary<int, List<PrefabCache>>();

    private void CreateUndo(EntityPlayer sender, Vector3i p0, Vector3i size)
    {
      var steamId = "_server";
      if (SenderInfo.RemoteClientInfo != null)
      {
        steamId = SenderInfo.RemoteClientInfo.ownerId;
      }

      var areaCache = new Prefab();
      var userId = 0; // id will be 0 for web console issued commands
      areaCache.CopyFromWorld(GameManager.Instance.World, p0, new Vector3i(p0.x + size.x, p0.y + size.y, p0.z + size.z));
      areaCache.bCopyAirBlocks = true;

      if (sender != null)
      {
        userId = sender.entityId;
      }
      Directory.CreateDirectory(Utils.GetGameDir(UndoDir));
      var filename = $"{steamId}.{p0.x}.{p0.y}.{p0.z}.{DateTime.UtcNow.ToFileTime()}";
      areaCache.Save(Utils.GetGameDir(UndoDir), filename);

      if (_undoCache.ContainsKey(userId))
      {
        _undoCache[userId].Add(new PrefabCache { Filename = filename, Pos = p0 });
      }
      else
      {
        _undoCache.Add(userId, new List<PrefabCache> { new PrefabCache { Filename = filename, Pos = p0 } });
      }
    }

    private void UndoInsert(EntityPlayer sender)
    {
      var userId = 0;
      if (sender != null)
      {
        userId = sender.entityId;
      }
      if (!_undoCache.ContainsKey(userId)) return;

      if (_undoCache[userId].Count <= 0) return;

      var pCache = _undoCache[userId][_undoCache[userId].Count - 1];
      if (pCache != null)
      {
        var p = new Prefab();
        p.Load(Utils.GetGameDir(UndoDir), pCache.Filename);
        BCImport.InsertPrefab(p, pCache.Pos.x, pCache.Pos.y, pCache.Pos.z, pCache.Pos);

        var cacheFile = Utils.GetGameDir($"{UndoDir}{pCache.Filename}");
        if (Utils.FileExists($"{cacheFile}.tts"))
        {
          Utils.FileDelete($"{cacheFile}.tts");
        }
        if (Utils.FileExists($"{cacheFile}.xml"))
        {
          Utils.FileDelete($"{cacheFile}.xml");
        }
      }
      _undoCache[userId].RemoveAt(_undoCache[userId].Count - 1);
    }

    public override void Process()
    {
      //todo: damage
      //todo: use Chunk.RecalcHeightAt(int _x, int _yMaxStart, int _z)

      //todo: allow /y=-1 too be used to offset the insert when using player locs to allow sub areas to be accessed without underground clipping
      //      /y=terrain to set the bottom y co-ord to the lowest non terrain block -1

      //todo: apply a custom placeholder mapping to a block value in the area

      var world = GameManager.Instance.World;
      if (world == null)
      {
        SendOutput("World not loaded");

        return;
      }

      var p1 = new Vector3i(int.MinValue, 0, int.MinValue);
      var p2 = new Vector3i(int.MinValue, 0, int.MinValue);
      string blockname;
      string blockname2 = null;

      //get loc and player current pos
      EntityPlayer sender = null;
      string steamId = null;
      if (SenderInfo.RemoteClientInfo != null)
      {
        steamId = SenderInfo.RemoteClientInfo.ownerId;
        sender = world.Entities.dict[SenderInfo.RemoteClientInfo.entityId] as EntityPlayer;
        if (sender != null)
        {
          p2 = new Vector3i((int)Math.Floor(sender.serverPos.x / 32f), (int)Math.Floor(sender.serverPos.y / 32f), (int)Math.Floor(sender.serverPos.z / 32f));
        }
        else
        {
          SendOutput("Error: unable to get player location");

          return;
        }
      }

      if (Options.ContainsKey("undo"))
      {
        UndoInsert(sender);

        return;
      }

      switch (Params.Count)
      {
        case 1:
        case 2:
          if (steamId != null)
          {
            p1 = BCLocation.GetPos(steamId);
            if (p1.x == int.MinValue)
            {
              SendOutput("No location stored. Use bc-loc to store a location.");

              return;
            }

            blockname = Params[0];
            if (Params.Count == 2)
            {
              blockname2 = Params[1];
            }
          }
          else
          {
            SendOutput("Error: unable to get player location");

            return;
          }
          break;
        case 7:
        case 8:
          //parse params
          if (!int.TryParse(Params[0], out p1.x) || !int.TryParse(Params[1], out p1.y) || !int.TryParse(Params[2], out p1.z) || !int.TryParse(Params[3], out p2.x) || !int.TryParse(Params[4], out p2.y) || !int.TryParse(Params[5], out p2.z))
          {
            SendOutput("Error: unable to parse coordinates");

            return;
          }
          blockname = Params[6];
          if (Params.Count == 8)
          {
            blockname2 = Params[7];
          }
          break;
        default:
          //todo: make a helper method for below output SendHelp(Enum.InvalidParamCount)
          SendOutput("Error: Incorrect command format.");
          SendOutput(GetHelp());

          return;
      }

      var size = new Vector3i(Math.Abs(p1.x - p2.x) + 1, Math.Abs(p1.y - p2.y) + 1, Math.Abs(p1.z - p2.z) + 1);

      var p3 = new Vector3i(
        p1.x < p2.x ? p1.x : p2.x,
        p1.y < p2.y ? p1.y : p2.y,
        p1.z < p2.z ? p1.z : p2.z
      );

      //**************** GET BLOCKVALUE
      var bvNew = int.TryParse(blockname, out var blockId) ? Block.GetBlockValue(blockId) : Block.GetBlockValue(blockname);

      var modifiedChunks = GetAffectedChunks(p3, size);

      //CREATE UNDO
      //create backup of area blocks will insert to
      if (!Options.ContainsKey("noundo"))
      {
        CreateUndo(sender, p3, size);
      }

      if (Options.ContainsKey("swap"))
      {
        SwapBlocks(p3, size, bvNew, blockname2, modifiedChunks);
      }
      else if (Options.ContainsKey("smooth"))
      {
        //todo
      }
      else if (Options.ContainsKey("repair"))
      {
        //todo
      }
      else if (Options.ContainsKey("upgrade"))
      {
        //todo
      }
      else if (Options.ContainsKey("nopaint"))
      {
        //todo
      }
      else if (Options.ContainsKey("lit"))
      {
        //todo - _blockValue.meta = (byte)(((int)_blockValue.meta & -3) | ((!isOn) ? 0 : 2));
      }
      else if (Options.ContainsKey("scan"))
      {
        ScanBlocks(size, p3, bvNew, blockname);
      }
      else
      {
        FillBlocks(p3, size, bvNew, blockname, modifiedChunks);
      }
    }

    private static Dictionary<long, Chunk> GetAffectedChunks(Vector3i p3, Vector3i size)
    {
      //GET AFFECTED CHUNKS
      //todo: get an array of chunk keys instead, trigger loading here, if not done by end of process, then defer changes to sub thread
      var modifiedChunks = new Dictionary<long, Chunk>();
      for (var cx = -1; cx <= size.x + 16; cx = cx + 16)
      {
        for (var cz = -1; cz <= size.z + 16; cz = cz + 16)
        {
          var chunk = GameManager.Instance.World.GetChunkFromWorldPos(p3.x + cx, p3.y, p3.z + cz) as Chunk;
          if (chunk == null)
          {
            SendOutput($"Unable to load chunk for insert @ {p3.x + cx},{p3.z + cz}");
          }
          else
          {
            if (modifiedChunks.ContainsKey(chunk.Key)) continue;

            modifiedChunks.Add(chunk.Key, chunk);
          }
        }
      }

      return modifiedChunks;
    }

    private static void SwapBlocks(Vector3i p3, Vector3i size, BlockValue newbv, string blockname, Dictionary<long, Chunk> modifiedChunks)
    {
      var targetbv = int.TryParse(blockname, out int blockId) ? Block.GetBlockValue(blockId) : Block.GetBlockValue(blockname);

      const int clrIdx = 0;
      var counter = 0;

      var block1 = Block.list[targetbv.type];
      if (block1 == null)
      {
        SendOutput("Unable to find target block by id or name");

        return;
      }

      var block2 = Block.list[newbv.type];
      if (block2 == null)
      {
        SendOutput("Unable to find replacement block by id or name");

        return;
      }

      var world = GameManager.Instance.World;
      for (var i = 0; i < size.x; i++)
      {
        for (var j = 0; j < size.y; j++)
        {
          for (var k = 0; k < size.z; k++)
          {
            //todo:
            sbyte density = 1;
            var textureFull = 0L;

            if (newbv.Equals(BlockValue.Air))
            {
              density = MarchingCubes.DensityAir;
            }
            else if (newbv.Block.shape.IsTerrain())
            {
              density = MarchingCubes.DensityTerrain;
            }

            var p5 = new Vector3i(i + p3.x, j + p3.y, k + p3.z);
            if (world.GetBlock(p5).Block.GetBlockName() != block1.GetBlockName()) continue;

            world.ChunkClusters[clrIdx].SetBlock(p5, true, newbv, false, density, false, false);
            world.ChunkClusters[clrIdx].SetTextureFull(p5, textureFull);

            counter++;
          }
        }
      }

      SendOutput($@"Replaced {counter} '{block1.GetBlockName()}' blocks with '{block2.GetBlockName()}' @ {p3} to {p3 + size}");
      SendOutput("Use bc-wblock /undo to revert the changes");

      //RELOAD CHUNKS
      BCChunks.ReloadForClients(modifiedChunks);
    }

    private static void FillBlocks(Vector3i p3, Vector3i size, BlockValue bv, string search, Dictionary<long, Chunk> modifiedChunks)
    {
      const int clrIdx = 0;

      if (Block.list[bv.type] == null)
      {
        SendOutput("Unable to find block by id or name");

        return;
      }

      SetBlocks(clrIdx, p3, size, bv, search == "*");

      if (Options.ContainsKey("delmulti"))
      {
        SendOutput($@"Removed multidim blocks @ {p3} to {p3 + size}");
      }
      else
      {
        SendOutput($@"Inserting block '{Block.list[bv.type].GetBlockName()}' @ {p3} to {p3 + size}");
        SendOutput("Use bc-wblock /undo to revert the changes");
      }

      //RELOAD CHUNKS
      BCChunks.ReloadForClients(modifiedChunks);
    }

    private static void SetBlocks(int clrIdx, Vector3i p0, Vector3i size, BlockValue bvNew, bool searchAll)
    {
      var world = GameManager.Instance.World;
      var chunkCluster = world.ChunkClusters[clrIdx];

      sbyte density = 1;
      if (Options.ContainsKey("d"))
      {
        if (sbyte.TryParse(Options["d"], out density))
        {
          SendOutput($"Using density {density}");
        }
      }

      var textureFull = 0L;
      if (Options.ContainsKey("t"))
      {
        sbyte.TryParse(Options["t"], out sbyte texture);

        var num = 0L;
        for (var face = 0; face < 6; face++)
        {
          var num2 = face * 8;
          num &= ~(255L << num2);
          num |= (long)(texture & 255) << num2;
        }
        textureFull = num;
      }

      for (var j = 0; j < size.y; j++)
      {
        for (var i = 0; i < size.x; i++)
        {
          for (var k = 0; k < size.z; k++)
          {
            var p5 = new Vector3i(p0.x + i, p0.y + j, p0.z + k);
            var bvCurrent = world.GetBlock(p5);

            if (Options.ContainsKey("delmulti") && (!searchAll || bvNew.type != bvCurrent.type)) continue;

            //REMOVE PARENT OF MULTIDIM
            if (bvCurrent.Block.isMultiBlock && bvCurrent.ischild)
            {
              var parentPos = bvCurrent.Block.multiBlockPos.GetParentPos(p5, bvCurrent);
              var parent = chunkCluster.GetBlock(parentPos);
              if (parent.ischild || parent.type != bvCurrent.type) continue;

              chunkCluster.SetBlock(parentPos, BlockValue.Air, false, false);
            }
            if (Options.ContainsKey("delmulti")) continue;

            //REMOVE LCB's
            if (bvCurrent.Block.IndexName == "lpblock")
            {
              GameManager.Instance.persistentPlayers.RemoveLandProtectionBlock(new Vector3i(p5.x, p5.y, p5.z));
            }

            //todo: move to a chunk request and then process all blocks on that chunk
            var chunkSync = world.GetChunkFromWorldPos(p5.x, p5.y, p5.z) as Chunk;

            if (bvNew.Equals(BlockValue.Air))
            {
              density = MarchingCubes.DensityAir;

              if (world.GetTerrainHeight(p5.x, p5.z) > p5.y)
              {
                chunkSync?.SetTerrainHeight(p5.x & 15, p5.z & 15, (byte)p5.y);
              }
            }
            else if (bvNew.Block.shape.IsTerrain())
            {
              density = MarchingCubes.DensityTerrain;

              if (world.GetTerrainHeight(p5.x, p5.z) < p5.y)
              {
                chunkSync?.SetTerrainHeight(p5.x & 15, p5.z & 15, (byte)p5.y);
              }
            }
            else
            {
              //SET TEXTURE
              world.ChunkClusters[clrIdx].SetTextureFull(p5, textureFull);
            }

            //SET BLOCK
            world.ChunkClusters[clrIdx].SetBlock(p5, true, bvNew, true, density, false, false);
          }
        }
      }


    }

    private static void ScanBlocks(Vector3i size, Vector3i p3, BlockValue bv, string search)
    {
      var block1 = Block.list[bv.type];
      if (block1 == null && search != "*")
      {
        SendOutput("Unable to find block by id or name");

        return;
      }

      var stats = new SortedDictionary<string, int>();
      const int clrIdx = 0;
      for (var j = 0; j < size.y; j++)
      {
        for (var i = 0; i < size.x; i++)
        {
          for (var k = 0; k < size.z; k++)
          {
            var p5 = new Vector3i(i + p3.x, j + p3.y, k + p3.z);
            var blockValue = GameManager.Instance.World.GetBlock(clrIdx, p5);
            //var d = GameManager.Instance.World.GetDensity(_clrIdx, p5);
            //var t = GameManager.Instance.World.GetTexture(i + p3.x, j + p3.y, k + p3.z);
            var name = ItemClass.list[blockValue.type]?.Name;
            if (string.IsNullOrEmpty(name))
            {
              name = "air";
            }

            if (search == "*")
            {
              SetStats(name, blockValue, stats);
            }
            else
            {
              if (name != bv.Block.GetBlockName()) continue;

              SetStats(name, blockValue, stats);
            }
          }
        }
      }

      SendJson(stats);
    }

    private static void SetStats(string name, BlockValue bv, IDictionary<string, int> stats)
    {
      if (stats.ContainsKey($"{bv.type:D4}:{name}"))
      {
        stats[$"{bv.type:D4}:{name}"] += 1;
      }
      else
      {
        stats.Add($"{bv.type:D4}:{name}", 1);
      }
    }
  }
}
