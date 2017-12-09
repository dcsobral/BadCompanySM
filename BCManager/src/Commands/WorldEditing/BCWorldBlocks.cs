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
      //todo: use Chunk.RecalcHeightAt(int _x, int _yMaxStart, int _z)
      //todo: allow /y=-1 too be used to offset the insert when using player locs to allow sub areas to be accessed without underground clipping
      //      /y=terrain to set the bottom y co-ord to the lowest non terrain block -1
      //todo: apply a custom placeholder mapping to a block value in the area

      //todo: refactor to use CmdArea and tasks

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
        case 2:
        case 3:
          if (steamId != null)
          {
            p1 = BCLocation.GetPos(steamId);
            if (p1.x == int.MinValue)
            {
              SendOutput("No location stored. Use bc-loc to store a location.");

              return;
            }

            blockname = Params[1];
            if (Params.Count == 3)
            {
              blockname2 = Params[2];
            }
          }
          else
          {
            SendOutput("Error: unable to get player location");

            return;
          }
          break;
        case 8:
        case 9:
          //parse params
          if (!int.TryParse(Params[1], out p1.x) || !int.TryParse(Params[2], out p1.y) || !int.TryParse(Params[3], out p1.z) || !int.TryParse(Params[4], out p2.x) || !int.TryParse(Params[5], out p2.y) || !int.TryParse(Params[6], out p2.z))
          {
            SendOutput("Error: unable to parse coordinates");

            return;
          }
          blockname = Params[7];
          if (Params.Count == 9)
          {
            blockname2 = Params[8];
          }
          break;
        default:
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

      switch (Params[0])
      {
        case "scan":
          ScanBlocks(p3, size, bvNew, blockname);
          break;
        case "fill":
          FillBlocks(p3, size, bvNew, blockname, modifiedChunks);
          break;
        case "swap":
          SwapBlocks(p3, size, bvNew, blockname2, modifiedChunks);
          break;
        case "repair":
          RepairBlocks(p3, size, modifiedChunks);
          break;
        case "damage":
          DamageBlocks(p3, size, modifiedChunks);
          break;
        case "upgrade":
          UpgradeBlocks(p3, size, modifiedChunks);
          break;
        case "downgrade":
          DowngradeBlocks(p3, size, modifiedChunks);
          break;
        case "paint":
          SetPaint(p3, size, modifiedChunks);
          break;
        case "paintface":
          SetPaintFace(p3, size, modifiedChunks);
          break;
        case "paintstrip":
          RemovePaint(p3, size, modifiedChunks);
          break;
        case "density":
          SetDensity(p3, size, modifiedChunks);
          break;
        case "rotate":
          SetRotation(p3, size, modifiedChunks);
          break;
        default:
          SendOutput(GetHelp());
          break;
      }
    }

    private static void SetDensity(Vector3i p3, Vector3i size, Dictionary<long, Chunk> modifiedChunks)
    {
      sbyte density = 1;
      if (Options.ContainsKey("d"))
      {
        if (sbyte.TryParse(Options["d"], out density))
        {
          SendOutput($"Using density {density}");
        }
      }

      const int clrIdx = 0;
      var counter = 0;
      for (var j = 0; j < size.y; j++)
      {
        for (var i = 0; i < size.x; i++)
        {
          for (var k = 0; k < size.z; k++)
          {
            var p5 = new Vector3i(i + p3.x, j + p3.y, k + p3.z);
            var blockValue = GameManager.Instance.World.GetBlock(clrIdx, p5);
            if (blockValue.Equals(BlockValue.Air) || blockValue.ischild) continue;

            GameManager.Instance.World.ChunkClusters[clrIdx].SetBlock(p5, false, blockValue, true, density, false, false);
            counter++;
          }
        }
      }

      SendOutput($"Setting density on {counter} blocks '{density}' @ {p3} to {p3 + size}");
      SendOutput("Use bc-wblock /undo to revert the changes");
      Reload(modifiedChunks);
    }

    private static void SetRotation(Vector3i p3, Vector3i size, Dictionary<long, Chunk> modifiedChunks)
    {
      byte rotation = 0;
      if (Options.ContainsKey("rot"))
      {
        if (!byte.TryParse(Options["rot"], out rotation))
        {
          SendOutput($"Unable to parse rotation '{Options["rot"]}'");

          return;
        }
      }

      const int clrIdx = 0;
      var counter = 0;
      for (var j = 0; j < size.y; j++)
      {
        for (var i = 0; i < size.x; i++)
        {
          for (var k = 0; k < size.z; k++)
          {
            var p5 = new Vector3i(i + p3.x, j + p3.y, k + p3.z);
            var blockValue = GameManager.Instance.World.GetBlock(clrIdx, p5);
            if (blockValue.Equals(BlockValue.Air) || blockValue.ischild || !blockValue.Block.shape.IsRotatable) continue;

            blockValue.rotation = rotation;
            GameManager.Instance.World.ChunkClusters[clrIdx].SetBlock(p5, blockValue, false, false);
            counter++;
          }
        }
      }

      SendOutput($"Setting rotation on '{counter}' blocks @ {p3} to {p3 + size}");
      SendOutput("Use bc-wblock /undo to revert the changes");
      Reload(modifiedChunks);
    }

    private static void DowngradeBlocks(Vector3i p3, Vector3i size, Dictionary<long, Chunk> modifiedChunks)
    {
      const int clrIdx = 0;
      var counter = 0;
      for (var j = 0; j < size.y; j++)
      {
        for (var i = 0; i < size.x; i++)
        {
          for (var k = 0; k < size.z; k++)
          {
            var p5 = new Vector3i(i + p3.x, j + p3.y, k + p3.z);
            var blockValue = GameManager.Instance.World.GetBlock(clrIdx, p5);
            var downgradeBlockValue = blockValue.Block.DowngradeBlock;
            if (downgradeBlockValue.Equals(BlockValue.Air) || blockValue.ischild) continue;

            downgradeBlockValue.rotation = blockValue.rotation;
            GameManager.Instance.World.ChunkClusters[clrIdx].SetBlock(p5, downgradeBlockValue, false, false);
            counter++;
          }
        }
      }

      SendOutput($"Downgrading {counter} blocks @ {p3} to {p3 + size}");
      SendOutput("Use bc-wblock /undo to revert the changes");
      Reload(modifiedChunks);
    }

    private static void UpgradeBlocks(Vector3i p3, Vector3i size, Dictionary<long, Chunk> modifiedChunks)
    {
      const int clrIdx = 0;
      var counter = 0;
      for (var j = 0; j < size.y; j++)
      {
        for (var i = 0; i < size.x; i++)
        {
          for (var k = 0; k < size.z; k++)
          {
            var p5 = new Vector3i(i + p3.x, j + p3.y, k + p3.z);
            var blockValue = GameManager.Instance.World.GetBlock(clrIdx, p5);
            var upgradeBlockValue = blockValue.Block.UpgradeBlock;
            if (upgradeBlockValue.Equals(BlockValue.Air) || blockValue.ischild) continue;

            upgradeBlockValue.rotation = blockValue.rotation;
            GameManager.Instance.World.ChunkClusters[clrIdx].SetBlock(p5, upgradeBlockValue, false, false);
            counter++;
          }
        }
      }

      SendOutput($"Upgrading {counter} blocks @ {p3} to {p3 + size}");
      SendOutput("Use bc-wblock /undo to revert the changes");
      Reload(modifiedChunks);
    }

    private static void DamageBlocks(Vector3i p3, Vector3i size, Dictionary<long, Chunk> modifiedChunks)
    {
      var damageMin = 0;
      var damageMax = 0;
      if (Options.ContainsKey("d"))
      {
        if (Options["d"].IndexOf(",", StringComparison.InvariantCulture) > -1)
        {
          var dRange = Options["d"].Split(',');
          if (dRange.Length != 2)
          {
            SendOutput("Unable to parse damage values");

            return;
          }

          if (!int.TryParse(dRange[0], out damageMin))
          {
            SendOutput("Unable to parse damage min value");

            return;
          }

          if (!int.TryParse(dRange[1], out damageMax))
          {
            SendOutput("Unable to parse damage max value");

            return;
          }
        }
        else
        {
          if (!int.TryParse(Options["d"], out damageMin))
          {
            SendOutput("Unable to parse damage value");

            return;
          }
        }
      }

      const int clrIdx = 0;
      var counter = 0;
      for (var j = 0; j < size.y; j++)
      {
        for (var i = 0; i < size.x; i++)
        {
          for (var k = 0; k < size.z; k++)
          {
            var p5 = new Vector3i(i + p3.x, j + p3.y, k + p3.z);
            var blockValue = GameManager.Instance.World.GetBlock(clrIdx, p5);
            if (blockValue.Equals(BlockValue.Air)) continue;

            var max = blockValue.Block.blockMaterial.MaxDamage;
            var damage = (damageMax != 0 ? UnityEngine.Random.Range(damageMin, damageMax) : damageMin) + blockValue.damage;
            if (Options.ContainsKey("nobreak"))
            {
              blockValue.damage = Math.Min(damage, max - 1);
            }
            else if (Options.ContainsKey("overkill"))
            {
              //needs to downgrade if overflow damage, then apply remaining damage until all used or downgraded to air
              var d = damage;
              while (d > 0 || blockValue.type != 0)
              {
                var downgrade = blockValue.Block.DowngradeBlock;
                downgrade.rotation = blockValue.rotation;
                blockValue = downgrade;
                blockValue.damage = d;
                d = d - max;
              }
              blockValue.damage = damageMin;
            }
            else
            {
              //needs to downgrade if damage > max, no overflow damage
              if (damage >= max)
              {
                var downgrade = blockValue.Block.DowngradeBlock;
                downgrade.rotation = blockValue.rotation;
                blockValue = downgrade;
              }
              else
              {
                blockValue.damage = damage;
              }
            }

            GameManager.Instance.World.ChunkClusters[clrIdx].SetBlock(p5, blockValue, false, false);
            counter++;
          }
        }
      }

      SendOutput($"Damaging {counter} blocks @ {p3} to {p3 + size}");
      SendOutput("Use bc-wblock /undo to revert the changes");
      Reload(modifiedChunks);
    }

    private static void RepairBlocks(Vector3i p3, Vector3i size, Dictionary<long, Chunk> modifiedChunks)
    {
      const int clrIdx = 0;
      var counter = 0;
      for (var j = 0; j < size.y; j++)
      {
        for (var i = 0; i < size.x; i++)
        {
          for (var k = 0; k < size.z; k++)
          {
            var p5 = new Vector3i(i + p3.x, j + p3.y, k + p3.z);
            var blockValue = GameManager.Instance.World.GetBlock(clrIdx, p5);
            if (blockValue.Equals(BlockValue.Air)) continue;

            blockValue.damage = 0;
            GameManager.Instance.World.ChunkClusters[clrIdx].SetBlock(p5, blockValue, false, false);
            counter++;
          }
        }
      }

      SendOutput($"Repairing {counter} blocks @ {p3} to {p3 + size}");
      SendOutput("Use bc-wblock /undo to revert the changes");
      Reload(modifiedChunks);
    }

    private static void SetPaintFace(Vector3i p3, Vector3i size, Dictionary<long, Chunk> modifiedChunks)
    {
      byte texture = 0;
      if (Options.ContainsKey("t"))
      {
        if (!byte.TryParse(Options["t"], out texture))
        {
          SendOutput("Unable to parse texture value");

          return;
        }
        if (BlockTextureData.list[texture] == null)
        {
          SendOutput($"Unknown texture index {texture}");

          return;
        }
      }
      uint setFace = 0;
      if (Options.ContainsKey("face"))
      {
        if (!uint.TryParse(Options["face"], out setFace))
        {
          SendOutput("Unable to parse face value");

          return;
        }
      }
      if (setFace > 5)
      {
        SendOutput("Face must be between 0 and 5");

        return;
      }

      const int clrIdx = 0;
      var counter = 0;
      for (var j = 0; j < size.y; j++)
      {
        for (var i = 0; i < size.x; i++)
        {
          for (var k = 0; k < size.z; k++)
          {
            var p5 = new Vector3i(i + p3.x, j + p3.y, k + p3.z);
            var blockValue = GameManager.Instance.World.GetBlock(clrIdx, p5);
            if (blockValue.Equals(BlockValue.Air)) continue;

            GameManager.Instance.World.ChunkClusters[clrIdx].SetBlockFaceTexture(p5, (BlockFace)setFace, texture);
            counter++;
          }
        }
      }

      SendOutput($"Painting {counter} blocks on face '{((BlockFace)setFace).ToString()}' with texture '{BlockTextureData.GetDataByTextureID(texture)?.Name}' @ {p3} to {p3 + size}");
      SendOutput("Use bc-wblock /undo to revert the changes");
      Reload(modifiedChunks);
    }

    private static void SetPaint(Vector3i p3, Vector3i size, Dictionary<long, Chunk> modifiedChunks)
    {
      var texture = 0;
      if (Options.ContainsKey("t"))
      {
        if (!int.TryParse(Options["t"], out texture))
        {
          SendOutput("Unable to parse texture value");

          return;
        }
        if (BlockTextureData.list[texture] == null)
        {
          SendOutput($"Unknown texture index {texture}");

          return;
        }
      }

      var num = 0L;
      for (var face = 0; face < 6; face++)
      {
        var num2 = face * 8;
        num &= ~(255L << num2);
        num |= (long)(texture & 255) << num2;
      }
      var textureFull = num;

      const int clrIdx = 0;
      var counter = 0;
      for (var j = 0; j < size.y; j++)
      {
        for (var i = 0; i < size.x; i++)
        {
          for (var k = 0; k < size.z; k++)
          {
            var p5 = new Vector3i(i + p3.x, j + p3.y, k + p3.z);
            var blockValue = GameManager.Instance.World.GetBlock(clrIdx, p5);
            if (blockValue.Block.shape.IsTerrain() || blockValue.Equals(BlockValue.Air)) continue;

            GameManager.Instance.World.ChunkClusters[clrIdx].SetTextureFull(p5, textureFull);
            counter++;
          }
        }
      }

      SendOutput($"Painting {counter} blocks with texture '{BlockTextureData.GetDataByTextureID(texture)?.Name}' @ {p3} to {p3 + size}");
      SendOutput("Use bc-wblock /undo to revert the changes");
      Reload(modifiedChunks);
    }

    private static void RemovePaint(Vector3i p3, Vector3i size, Dictionary<long, Chunk> modifiedChunks)
    {
      const int clrIdx = 0;
      var counter = 0;
      for (var j = 0; j < size.y; j++)
      {
        for (var i = 0; i < size.x; i++)
        {
          for (var k = 0; k < size.z; k++)
          {
            var p5 = new Vector3i(i + p3.x, j + p3.y, k + p3.z);
            var blockValue = GameManager.Instance.World.GetBlock(clrIdx, p5);
            if (blockValue.Block.shape.IsTerrain() || blockValue.Equals(BlockValue.Air)) continue;

            GameManager.Instance.World.ChunkClusters[clrIdx].SetTextureFull(p5, 0L);
            counter++;
          }
        }
      }

      SendOutput($"Paint removed from {counter} blocks @ {p3} to {p3 + size}");
      SendOutput("Use bc-wblock /undo to revert the changes");
      Reload(modifiedChunks);
    }

    private static void SwapBlocks(Vector3i p3, Vector3i size, BlockValue newbv, string blockname, Dictionary<long, Chunk> modifiedChunks)
    {
      var targetbv = int.TryParse(blockname, out var blockId) ? Block.GetBlockValue(blockId) : Block.GetBlockValue(blockname);

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

      const int clrIdx = 0;
      var counter = 0;
      var world = GameManager.Instance.World;
      for (var i = 0; i < size.x; i++)
      {
        for (var j = 0; j < size.y; j++)
        {
          for (var k = 0; k < size.z; k++)
          {
            sbyte density = 1;
            if (Options.ContainsKey("d"))
            {
              if (sbyte.TryParse(Options["d"], out density))
              {
                SendOutput($"Using density {density}");
              }
            }
            else
            {
              if (newbv.Equals(BlockValue.Air))
              {
                density = MarchingCubes.DensityAir;
              }
              else if (newbv.Block.shape.IsTerrain())
              {
                density = MarchingCubes.DensityTerrain;
              }
            }

            var textureFull = 0L;
            if (Options.ContainsKey("t"))
            {
              if (!byte.TryParse(Options["t"], out var texture))
              {
                SendOutput("Unable to parse texture index");

                return;
              }

              if (BlockTextureData.list[texture] == null)
              {
                SendOutput($"Unknown texture index {texture}");

                return;
              }

              var num = 0L;
              for (var face = 0; face < 6; face++)
              {
                var num2 = face * 8;
                num &= ~(255L << num2);
                num |= (long)(texture & 255) << num2;
              }
              textureFull = num;
            }

            var p5 = new Vector3i(i + p3.x, j + p3.y, k + p3.z);
            if (world.GetBlock(p5).Block.GetBlockName() != block1.GetBlockName()) continue;

            world.ChunkClusters[clrIdx].SetBlock(p5, true, newbv, false, density, false, false);
            world.ChunkClusters[clrIdx].SetTextureFull(p5, textureFull);
            counter++;
          }
        }
      }

      SendOutput($"Replaced {counter} '{block1.GetBlockName()}' blocks with '{block2.GetBlockName()}' @ {p3} to {p3 + size}");
      SendOutput("Use bc-wblock /undo to revert the changes");
      Reload(modifiedChunks);
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
        SendOutput($"Removed multidim blocks @ {p3} to {p3 + size}");
      }
      else
      {
        SendOutput($"Inserting block '{Block.list[bv.type].GetBlockName()}' @ {p3} to {p3 + size}");
        SendOutput("Use bc-wblock /undo to revert the changes");
      }

      Reload(modifiedChunks);
    }

    //private static void LightBlocks(Vector3i p3, Vector3i size, bool isOn, Dictionary<long, Chunk> modifiedChunks)
    //{
    //  const int clrIdx = 0;
    //  for (var j = 0; j < size.y; j++)
    //  {
    //    for (var i = 0; i < size.x; i++)
    //    {
    //      for (var k = 0; k < size.z; k++)
    //      {
    //        var p5 = new Vector3i(i + p3.x, j + p3.y, k + p3.z);
    //        var blockValue = GameManager.Instance.World.GetBlock(clrIdx, p5);
    //        if (blockValue.Block is BlockLight)
    //        {
    //          blockValue.meta = (byte)((blockValue.meta & -3) | (!isOn ? 0 : 2));
    //        }
    //      }
    //    }
    //  }

    //  Reload(modifiedChunks);
    //}

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
        if (!byte.TryParse(Options["t"], out var texture))
        {
          SendOutput("Unable to parse texture index");

          return;
        }

        if (BlockTextureData.list[texture] == null)
        {
          SendOutput($"Unknown texture index {texture}");

          return;
        }

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

    private static void ScanBlocks(Vector3i p3, Vector3i size, BlockValue bv, string search)
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

    private static void Reload(Dictionary<long, Chunk> modifiedChunks)
    {
      if (!(Options.ContainsKey("noreload") || Options.ContainsKey("nr")))
      {
        BCChunks.ReloadForClients(modifiedChunks);
      }
    }

    private static Dictionary<long, Chunk> GetAffectedChunks(Vector3i p3, Vector3i size)
    {
      //GET AFFECTED CHUNKS
      var modifiedChunks = new Dictionary<long, Chunk>();
      for (var cx = -1; cx <= size.x + 16; cx = cx + 16)
      {
        for (var cz = -1; cz <= size.z + 16; cz = cz + 16)
        {
          if (!(GameManager.Instance.World.GetChunkFromWorldPos(p3.x + cx, p3.y, p3.z + cz) is Chunk chunk))
          {
            //todo: output chunk co-ords instead of world pos? Only needed in force sync mode
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
  }
}
