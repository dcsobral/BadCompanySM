using JetBrains.Annotations;

namespace BCM.Commands
{
  [UsedImplicitly]
  public class BCUndo : BCCommandAbstract
  {
    //todo: list subcommand to display previous actions and the undo id
    //todo: ability to undo a specific undo id
    //todo: ability to undo x previous commands in reverse order
    //todo: if chunks not loaded, fail and dont remove undo from history

    protected override void Process()
    {
      if (!BCUtils.CheckWorld(out var world)) return;

      EntityPlayer sender = null;
      if (SenderInfo.RemoteClientInfo != null)
      {
        sender = world.Entities.dict[SenderInfo.RemoteClientInfo.entityId] as EntityPlayer;
      }

      if (sender == null) return;

      SendOutput(BCUtils.UndoSetBlocks(sender) ? "Undoing previous world editing command" : "Undo failed, nothing to undo?");
    }
  }
}
