using System;
using BCM.Models;

namespace BCM.Commands
{
  public class BCExport : BCCommandAbstract
  {
    public override void Process()
    {
      var world = GameManager.Instance.World;
      if (world == null)
      {
        SendOutput("World not initialized.");

        return;
      }

      var command = new BCMCmdArea(Params, Options, "Export");
      if (!BCUtils.ProcessParams(command, 14))
      {
        SendOutput(GetHelp());

        return;
      }

      if (!BCUtils.GetIds(world, command, out var entity))
      {
        SendOutput("Command requires a position when not run by a player.");

        return;
      }

      if (!command.HasChunkPos && !command.HasPos && !BCUtils.GetEntPos(command, entity))
      {
        SendOutput("Unable to get position.");

        return;
      }

      BCUtils.DoProcess(world, command, this);
    }

    public override void ProcessSwitch(World world, BCMCmdArea command, out ReloadMode reload)
    {
      reload = ReloadMode.None;
      ExportPrefab(command, world);
    }

    private static void ExportPrefab(BCMCmdArea command, World world)
    {
      if (command.Position == null)
      {
        command.Position = new BCMVector3(command.ChunkBounds.x * 16, 0, command.ChunkBounds.y * 16);
        command.Size = new BCMVector3((command.ChunkBounds.z - command.ChunkBounds.x) * 16 + 15, 255, (command.ChunkBounds.w - command.ChunkBounds.y) * 16 + 15);
      }

      //must use size constructor to initialize private arrays
      var prefab = new Prefab(command.Size.ToV3Int());
      //Get prefab blocks from the world
      var _y = 0;
      var y = command.Position.y;
      while (y <= command.MaxPos.y)
      {
        var _x = 0;
        var x = command.Position.x;
        while (x <= command.MaxPos.x)
        {
          var _z = 0;
          var z = command.Position.z;
          while (z <= command.MaxPos.z)
          {
            prefab.SetBlock(_x, _y, _z, world.GetBlock(x, y, z));
            prefab.SetDensity(_x, _y, _z, world.GetDensity(0, x, y, z));
            prefab.SetTexture(_x, _y, _z, world.GetTexture(x, y, z));

            var te = world.GetTileEntity(0, new Vector3i(x, y, z));
            if (te != null)
            {
              switch (te.GetTileEntityType())
              {
                case TileEntityType.VendingMachine:
                {
                  if (te is TileEntityVendingMachine vm && vm.IsLocked())
                  {
                    var bv = prefab.GetBlock(_x, _y, _z);
                    bv.meta |= 4;
                    prefab.SetBlock(_x, _y, _z, bv);
                  }
                  break;

                  }
                case TileEntityType.SecureLoot:
                {
                  if (te is TileEntitySecureLootContainer sl && sl.IsLocked())
                  {
                    var bv = prefab.GetBlock(_x, _y, _z);
                    bv.meta |= 4;
                    prefab.SetBlock(_x, _y, _z, bv);
                  }
                  break;
                }
                case TileEntityType.SecureDoor:
                {
                  if (te is TileEntitySecureDoor sd && sd.IsLocked())
                  {
                    var bv = prefab.GetBlock(_x, _y, _z);
                    bv.meta |= 4;
                    prefab.SetBlock(_x, _y, _z, bv);
                  }
                  break;
                }
                case TileEntityType.Sign:
                {
                  if (te is TileEntitySign sg && sg.IsLocked())
                  {
                    var bv = prefab.GetBlock(_x, _y, _z);
                    bv.meta |= 4;
                    prefab.SetBlock(_x, _y, _z, bv);
                  }
                  break;
                }
              }
            }

            ++z;
            ++_z;
          }
          ++x;
          ++_x;
        }
        ++y;
        ++_y;
      }

      prefab.filename = command.Pars[0];
      prefab.addAllChildBlocks();
      prefab.bSleeperVolumes = false;

      //todo: parse additional config from options

      var dir = "Data/Prefabs";
      if (Options.ContainsKey("backup"))
      {
        dir = "Data/Prefabs/Backup";
      }

      SendOutput(prefab.Save(dir, prefab.filename)
        ? $"Prefab {prefab.filename} exported @ {command.Position}, size={command.Size}"
        : $"Error: Prefab {prefab.filename} failed to save.");
    }
  }
}
