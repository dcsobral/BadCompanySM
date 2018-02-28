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
      var prefab = new Prefab();
      if (command.Position == null)
      {
        command.Position = new BCMVector3(command.ChunkBounds.x * 16, 0, command.ChunkBounds.y * 16);
        command.Size = new BCMVector3((command.ChunkBounds.z - command.ChunkBounds.x) * 16 + 15, 255, (command.ChunkBounds.w - command.ChunkBounds.y) * 16 + 15);
      }
      prefab.CopyFromWorld(world, command.Position.ToV3Int(), command.Position.ToV3Int() + command.Size.ToV3Int());

      prefab.filename = command.Pars[0];
      prefab.bCopyAirBlocks = true;
      prefab.addAllChildBlocks();

      //todo: process meta, lock doors etc?
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
