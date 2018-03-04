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
        case "strippaint":
        case "paintstrip":
          RemovePaint(world, position);
          break;
        case "density":
          SetDensity(world, position);
          break;
        case "rotate":
          SetRotation(world, position);
          break;
        case "meta1":
          SetMeta(1, world, position);
          break;
        case "meta2":
          SetMeta(2, world, position);
          break;
        case "meta3":
          SetMeta(3, world, position);
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

      byte texture = 0;
      if (Options.ContainsKey("t"))
      {
        if (!byte.TryParse(Options["t"], out texture))
        {
          SendOutput("Unable to parse texture index");

          return;
        }

        if (BlockTextureData.list[texture] == null)
        {
          SendOutput($"Unknown texture index {texture}");

          return;
        }

        SendOutput($"Using texture '{BlockTextureData.GetDataByTextureID(texture)?.Name}'");
      }

      if (Options.ContainsKey("delmulti")) return;

      var bvCurrent = world.GetBlock(pos);

      //REMOVE PARENT OF MULTIDIM
      if (bvCurrent.Block.isMultiBlock && bvCurrent.ischild)
      {
        var parentPos = bvCurrent.Block.multiBlockPos.GetParentPos(pos, bvCurrent);
        var parent = world.GetBlock(parentPos);
        if (parent.ischild || parent.type != bvCurrent.type) return;

        world.ChunkClusters[0].SetBlock(parentPos, BlockValue.Air, false, false);
      }
      if (Options.ContainsKey("delmulti")) return;

      //REMOVE LCB's
      if (bvCurrent.Block.IndexName == "lpblock")
      {
        GameManager.Instance.persistentPlayers.RemoveLandProtectionBlock(pos);
      }

      var chunkSync = world.GetChunkFromWorldPos(pos.x, pos.y, pos.z) as Chunk;

      if (targetbv.Equals(BlockValue.Air))
      {
        density = MarchingCubes.DensityAir;

        if (world.GetTerrainHeight(pos.x, pos.z) > pos.y)
        {
          chunkSync?.SetTerrainHeight(pos.x & 15, pos.z & 15, (byte)pos.y);
        }
      }
      else if (targetbv.Block.shape.IsTerrain())
      {
        density = MarchingCubes.DensityTerrain;

        if (world.GetTerrainHeight(pos.x, pos.z) < pos.y)
        {
          chunkSync?.SetTerrainHeight(pos.x & 15, pos.z & 15, (byte)pos.y);
        }
      }
      else
      {
        //SET TEXTURE
        GameManager.Instance.SetBlockTextureServer(pos, BlockFace.None, texture, -1);
      }

      //SET BLOCK
      world.SetBlockRPC(pos, targetbv, density);
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
      if (blockValue.Equals(BlockValue.Air))
      {
        SendOutput($"Target block is air @ {pos}");

        return;
      }
      if (blockValue.ischild)
      {
        SendOutput($"Target block is a child block @ {pos} - Parent@ {blockValue.parentx},{blockValue.parenty},{blockValue.parentz}");

        return;
      }

      var d = world.GetDensity(0, pos);
      if (d == density)
      {
        SendOutput($"No change in density @ {pos}");

        return;
      }

      world.SetBlockRPC(pos, blockValue, density);
      SendOutput($"Changing density on block from '{d}' to '{density}' @ {pos}");
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
      if (blockValue.Equals(BlockValue.Air))
      {
        SendOutput($"Target block is air @ {pos}");

        return;
      }
      if (blockValue.ischild)
      {
        SendOutput($"Target child block can't be rotated @ {pos} - Parent@ {blockValue.parentx},{blockValue.parenty},{blockValue.parentz}");

        return;
      }
      if (!blockValue.Block.shape.IsRotatable)
      {
        SendOutput($"Target block can't be rotated @ {pos}");

        return;
      }

      var r = blockValue.rotation;
      blockValue.rotation = rotation;
      world.SetBlockRPC(pos, blockValue);

      SendOutput($"Changing rotation on block from '{r}' to '{rotation}' @ {pos}");
    }

    private static void DowngradeBlock(World world, Vector3i pos)
    {
      var blockValue = world.GetBlock(pos);
      var downgradeBlockValue = blockValue.Block.DowngradeBlock;
      if (downgradeBlockValue.Equals(BlockValue.Air))
      {
        SendOutput($"Target block has no downgrade @ {pos}");

        return;
      }
      if (blockValue.ischild)
      {
        SendOutput($"Can't downgrade a child block @ {pos} - Parent@ {blockValue.parentx},{blockValue.parenty},{blockValue.parentz}");

        return;
      }


      downgradeBlockValue.rotation = blockValue.rotation;
      world.SetBlockRPC(pos, downgradeBlockValue);

      SendOutput($"Downgrading block from '{blockValue.Block.GetBlockName()}' to '{downgradeBlockValue.Block.GetBlockName()}' @ {pos}");
    }

    private static void UpgradeBlock(World world, Vector3i pos)
    {
      var blockValue = world.GetBlock(pos);
      var upgradeBlockValue = blockValue.Block.UpgradeBlock;
      if (upgradeBlockValue.Equals(BlockValue.Air))
      {
        SendOutput($"Target block has no upgrade @ {pos}");

        return;
      }
      if (blockValue.ischild)
      {
        SendOutput($"Can't upgrade a child block @ {pos} - Parent@ {blockValue.parentx},{blockValue.parenty},{blockValue.parentz}");

        return;
      }

      upgradeBlockValue.rotation = blockValue.rotation;
      world.SetBlockRPC(pos, upgradeBlockValue);

      SendOutput($"Upgrading block from '{blockValue.Block.GetBlockName()}' to '{upgradeBlockValue.Block.GetBlockName()}' @ {pos}");
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
      var impact = damageMax != 0 ? UnityEngine.Random.Range(damageMin, damageMax) : damageMin;
      var damage = impact + blockValue.damage;
      if (Options.ContainsKey("nobreak"))
      {
        blockValue.damage = Math.Min(damage, max - 1);
      }
      else if (Options.ContainsKey("overkill"))
      {
        if (damage >= max)
        {
          var downgradeBlock = blockValue.Block.DowngradeBlock;
          while (damage >= max)
          {
            downgradeBlock = blockValue.Block.DowngradeBlock;
            damage -= max;
            max = downgradeBlock.Block.blockMaterial.MaxDamage;
            downgradeBlock.rotation = blockValue.rotation;
            blockValue = downgradeBlock;
          }
          blockValue.damage = damage;

          SendOutput($"Damaging block for {-impact} caused downgrade to '{downgradeBlock.Block.GetBlockName()}' @ {pos}");
        }
        else
        {
          blockValue.damage = damage;
        }
      }
      else
      {
        //needs to downgrade if damage > max, no overflow damage
        if (damage >= max)
        {
          var downgrade = blockValue.Block.DowngradeBlock;
          SendOutput($"Damaging block for {-impact} caused downgrade to '{downgrade.Block.GetBlockName()}' @ {pos}");
          downgrade.rotation = blockValue.rotation;
          blockValue = downgrade;
        }
        else
        {
          blockValue.damage = damage;
        }
      }

      world.SetBlockRPC(pos, blockValue);
      if (!blockValue.Equals(BlockValue.Air))
      {
        SendOutput($"Damaging block for '{-impact}' leaving {blockValue.Block.blockMaterial.MaxDamage - blockValue.damage}/{blockValue.Block.blockMaterial.MaxDamage} @ {pos}");
      }
    }

    private static void RepairBlock(World world, Vector3i pos)
    {
      var blockValue = world.GetBlock(pos);
      if (blockValue.Equals(BlockValue.Air))
      {
        SendOutput($"Target block is air @ {pos}");

        return;
      }

      var d = blockValue.damage;
      if (d > 0)
      {
        SendOutput($"Target block not damaged @ {pos}");

        return;
      }

      blockValue.damage = 0;
      world.SetBlockRPC(pos, blockValue);

      SendOutput($"Repairing block for '{d}' damage @ {pos}");
    }

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
      if (blockValue.Equals(BlockValue.Air))
      {
        SendOutput($"Target block is air @ {pos}");

        return;
      }
      if (blockValue.Block.shape.IsTerrain())
      {
        SendOutput($"Target block is terrain @ {pos}");

        return;
      }

      GameManager.Instance.SetBlockTextureServer(pos, (BlockFace)setFace, texture, -1);
      SendOutput($"Painting block on face '{((BlockFace)setFace).ToString()}' with texture '{BlockTextureData.GetDataByTextureID(texture)?.Name}' @ {pos}");
    }

    private static void SetPaint(World world, Vector3i pos)
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

      var blockValue = world.GetBlock(pos);
      if (blockValue.Equals(BlockValue.Air))
      {
        SendOutput($"Target block is air @ {pos}");

        return;
      }
      if (blockValue.Block.shape.IsTerrain())
      {
        SendOutput($"Target block is terrain @ {pos}");

        return;
      }

      GameManager.Instance.SetBlockTextureServer(pos, BlockFace.None, texture, -1);
      SendOutput($"Painting block with texture '{BlockTextureData.GetDataByTextureID(texture)?.Name}' @ {pos}");
    }

    private static void RemovePaint(World world, Vector3i pos)
    {
      var blockValue = world.GetBlock(pos);
      if (blockValue.Equals(BlockValue.Air))
      {
        SendOutput($"Target block is air @ {pos}");

        return;
      }
      if (blockValue.Block.shape.IsTerrain())
      {
        SendOutput($"Target block is terrain @ {pos}");

        return;
      }

      GameManager.Instance.SetBlockTextureServer(pos, BlockFace.None, 0, -1);
      SendOutput($"Paint removed from block @ {pos}");
    }

    private static void SetMeta(int metaIdx, World world, Vector3i pos)
    {
      byte meta = 0;
      if (Options.ContainsKey("meta"))
      {
        if (!byte.TryParse(Options["meta"], out meta))
        {
          SendOutput($"Unable to parse meta '{Options["meta"]}'");

          return;
        }
      }

      var blockValue = world.GetBlock(pos);
      if (blockValue.Equals(BlockValue.Air))
      {
        SendOutput($"Target block is air @ {pos}");

        return;
      }
      if (blockValue.ischild)
      {
        SendOutput($"Target child block can't be set @ {pos} - Parent@ {blockValue.parentx},{blockValue.parenty},{blockValue.parentz}");

        return;
      }

      var m = blockValue.meta;
      switch (metaIdx)
      {
        case 1:
          blockValue.meta = meta;
          break;
        case 2:
          blockValue.meta2 = meta;
          break;
        case 3:
          blockValue.meta3 = meta;
          break;
        default:
          return;
      }
      world.SetBlockRPC(pos, blockValue);

      SendOutput($"Changing meta{metaIdx} on block from '{m}' to '{meta}' @ {pos}");
    }
  }
}
