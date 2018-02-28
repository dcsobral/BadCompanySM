using System;
using System.Collections.Generic;

namespace BCM.Commands
{
  public class BCWorldBlocksRPC : BCCommandAbstract
  {
    private bool ProcessParams(out Vector3i position, out string blockname1)
    {
      position = new Vector3i();
      blockname1 = null;

      if (Params.Count == 4 || Params.Count == 5)
      {
        if (!int.TryParse(Params[1], out position.x) || !int.TryParse(Params[2], out position.y) ||
            !int.TryParse(Params[3], out position.z))
        {
          SendOutput("Unable to parse coordinates");

          return false;
        }
        blockname1 = Params.Count == 5 ? Params[4] : "*";
      }
      else
      {
        SendOutput(GetHelp());

        return false;
      }

      return true;
    }

    public override void Process()
    {
      var world = GameManager.Instance.World;
      if (world == null)
      {
        SendOutput("World not loaded");

        return;
      }

      if (!ProcessParams(out var position, out var blockname1)) return;

      var blockValue1 = int.TryParse(blockname1, out var blockId) ? Block.GetBlockValue(blockId) : Block.GetBlockValue(blockname1);

      if (blockValue1.type == 0 && !(blockname1 == "*" || blockname1 == "air" || blockname1 == "0"))
      {
        SendOutput($"Unable to find block by name or id '{blockname1}'");

        return;
      }

      if (world.GetChunkSync(World.toChunkXZ(position.x), World.toChunkXZ(position.z)) == null)
      {
        SendOutput("Chunk not loaded at given location");

        return;
      }

      switch (Params[0])
      {
        case "scan":
          ScanBlock(world, position);
          break;
        case "fill":
          FillBlock(world, position, blockValue1);
          break;
        case "repair":
          RepairBlock(world, position);
          break;
        case "damage":
          DamageBlock(world, position);
          break;
        case "upgrade":
          UpgradeBlock(world, position);
          break;
        case "downgrade":
          DowngradeBlock(world, position);
          break;
        case "paint":
          SetPaint(world, position);
          break;
        case "paintface":
          SetPaintFace(world, position);
          break;
        case "paintstrip":
          RemovePaint(world, position);
          break;
        case "density":
          SetDensity(world, position);
          break;
        case "rotate":
          SetRotation(world, position);
          break;
        default:
          SendOutput(GetHelp());
          break;
      }
    }

    private static void SetBlock(World world, Vector3i pos, BlockValue targetbv)
    {
      sbyte density = 0;
      if (Options.ContainsKey("d") && sbyte.TryParse(Options["d"], out density))
      {
        SendOutput($"Using density {density}");
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

      var blockPos = new Vector3i(pos.x, pos.y, pos.z);
      var bvCurrent = world.GetBlock(blockPos);

      if (Options.ContainsKey("delmulti")) return;

      //REMOVE PARENT OF MULTIDIM
      if (bvCurrent.Block.isMultiBlock && bvCurrent.ischild)
      {
        var parentPos = bvCurrent.Block.multiBlockPos.GetParentPos(blockPos, bvCurrent);
        var parent = world.GetBlock(parentPos);
        if (parent.ischild || parent.type != bvCurrent.type) return;

        world.ChunkClusters[0].SetBlock(parentPos, BlockValue.Air, false, false);
        //world.SetBlockRPC(parentPos, BlockValue.Air);
      }
      if (Options.ContainsKey("delmulti")) return;

      //REMOVE LCB's
      if (bvCurrent.Block.IndexName == "lpblock")
      {
        GameManager.Instance.persistentPlayers.RemoveLandProtectionBlock(new Vector3i(blockPos.x, blockPos.y, blockPos.z));
      }

      var chunkSync = world.GetChunkFromWorldPos(blockPos.x, blockPos.y, blockPos.z) as Chunk;

      if (targetbv.Equals(BlockValue.Air))
      {
        density = MarchingCubes.DensityAir;

        if (world.GetTerrainHeight(blockPos.x, blockPos.z) > blockPos.y)
        {
          chunkSync?.SetTerrainHeight(blockPos.x & 15, blockPos.z & 15, (byte)blockPos.y);
        }
      }
      else if (targetbv.Block.shape.IsTerrain())
      {
        density = MarchingCubes.DensityTerrain;

        if (world.GetTerrainHeight(blockPos.x, blockPos.z) < blockPos.y)
        {
          chunkSync?.SetTerrainHeight(blockPos.x & 15, blockPos.z & 15, (byte)blockPos.y);
        }
      }
      else
      {
        //SET TEXTURE
        world.ChunkClusters[0].SetTextureFull(blockPos, textureFull);
      }

      //SET BLOCK
      //world.ChunkClusters[0].SetBlock(p5, true, bvNew, true, density, false, false);
      world.SetBlockRPC(blockPos, targetbv, density);
    }

    private static void ScanBlock(World world, Vector3i pos)
    {
      var stats = new Dictionary<string, string>();

      var blockValue = world.GetBlock(pos.x, pos.y, pos.z);
      var name = ItemClass.list[blockValue.type]?.Name;
      if (string.IsNullOrEmpty(name))
      {
        name = "air";
      }

      stats.Add("Name", name);
      stats.Add("Type", $"{blockValue.type}");
      stats.Add("Density", $"{world.GetDensity(0, pos.x, pos.y, pos.z)}");
      stats.Add("Rotation", $"{blockValue.rotation}");
      stats.Add("Damage", $"{blockValue.damage}");
      stats.Add("Paint", $"{BCPrefab.GetTexStr(world.GetTexture(pos.x, pos.y, pos.z))}");
      stats.Add("Meta", $"{blockValue.meta}");
      stats.Add("Meta2", $"{blockValue.meta2}");
      stats.Add("Meta3", $"{blockValue.meta3}");
      if (blockValue.ischild)
      {
        stats.Add("IsChild", $"{blockValue.ischild}");
        stats.Add("ParentXYZ", $"{blockValue.parentx}, {blockValue.parenty}, {blockValue.parentz}");
      }

      SendJson(stats);
    }

    private static void FillBlock(World world, Vector3i pos, BlockValue targetbv)
    {
      SendOutput(Options.ContainsKey("delmulti")
        ? $"Removed multidim blocks @ {pos}"
        : $"Inserting block '{Block.list[targetbv.type].GetBlockName()}' @ {pos}");

      SetBlock(world, pos, targetbv);
    }

    private static void SetDensity(World world, Vector3i pos)
    {
      sbyte density = 1;
      if (Options.ContainsKey("d"))
      {
        if (sbyte.TryParse(Options["d"], out density))
        {
          SendOutput($"Using density {density}");
        }
      }

      var blockValue = world.GetBlock(pos);
      if (blockValue.Equals(BlockValue.Air) || blockValue.ischild) return;

      world.SetBlockRPC(pos, blockValue, density);

      SendOutput($"Setting density on block '{density}' @ {pos}");
    }

    private static void SetRotation(World world, Vector3i pos)
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

      var blockValue = world.GetBlock(pos);
      if (blockValue.Equals(BlockValue.Air) || blockValue.ischild || !blockValue.Block.shape.IsRotatable) return;

      blockValue.rotation = rotation;
      world.SetBlockRPC(pos, blockValue);

      SendOutput($"Setting rotation on block @ {pos}");
    }

    private static void DowngradeBlock(World world, Vector3i pos)
    {
      var blockValue = world.GetBlock(pos);
      var downgradeBlockValue = blockValue.Block.DowngradeBlock;
      if (downgradeBlockValue.Equals(BlockValue.Air) || blockValue.ischild) return;

      downgradeBlockValue.rotation = blockValue.rotation;
      world.SetBlockRPC(pos, downgradeBlockValue);

      SendOutput($"Downgrading block @ {pos}");
    }

    private static void UpgradeBlock(World world, Vector3i pos)
    {
      var blockValue = world.GetBlock(pos);
      var upgradeBlockValue = blockValue.Block.UpgradeBlock;
      if (upgradeBlockValue.Equals(BlockValue.Air) || blockValue.ischild) return;

      upgradeBlockValue.rotation = blockValue.rotation;
      world.SetBlockRPC(pos, upgradeBlockValue);

      SendOutput($"Upgrading block @ {pos}");
    }

    private static void DamageBlock(World world, Vector3i pos)
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

      var blockValue = world.GetBlock(pos);
      if (blockValue.Equals(BlockValue.Air)) return;

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

      world.SetBlockRPC(pos, blockValue);

      SendOutput($"Damaging block @ {pos}");
    }

    private static void RepairBlock(World world, Vector3i pos)
    {
      var blockValue = world.GetBlock(pos);
      if (blockValue.Equals(BlockValue.Air)) return;

      blockValue.damage = 0;
      world.SetBlockRPC(pos, blockValue);

      SendOutput($"Repairing block @ {pos}");
    }

    //GameManager.Instance.SetBlockTextureServer(_blockPos, BlockFace.None, 0, _entityIdThatDamaged);
    private static void SetPaintFace(World world, Vector3i pos)
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

      var blockValue = world.GetBlock(pos);
      if (blockValue.Equals(BlockValue.Air)) return;

      world.ChunkClusters[0].SetBlockFaceTexture(pos, (BlockFace)setFace, texture);

      SendOutput($"Painting block on face '{((BlockFace)setFace).ToString()}' with texture '{BlockTextureData.GetDataByTextureID(texture)?.Name}' @ {pos}");
    }

    private static void SetPaint(World world, Vector3i pos)
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

      var blockValue = world.GetBlock(pos);
      if (blockValue.Block.shape.IsTerrain() || blockValue.Equals(BlockValue.Air)) return;

      world.ChunkClusters[0].SetTextureFull(pos, textureFull);

      SendOutput($"Painting block with texture '{BlockTextureData.GetDataByTextureID(texture)?.Name}' @ {pos}");
    }

    private static void RemovePaint(World world, Vector3i pos)
    {
      var blockValue = world.GetBlock(pos);
      if (blockValue.Block.shape.IsTerrain() || blockValue.Equals(BlockValue.Air)) return;

      world.ChunkClusters[0].SetTextureFull(pos, 0L);

      SendOutput($"Paint removed from block @ {pos}");
    }
  }
}
