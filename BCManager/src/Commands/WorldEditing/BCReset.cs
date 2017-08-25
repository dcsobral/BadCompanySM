namespace BCM.Commands
{
  public class BCReset : BCCommandAbstract
  {
    public override void Process()
    {
      //todo: reset region

      var loc = new Vector3i {y = 0};

      if (Params.Count == 2)
      {
        if (int.TryParse(Params[0], out loc.x) && int.TryParse(Params[1], out loc.z)) return;

        SendOutput("One of <x> <z> params could not be parsed as a number.");
      }
      else if (Options.ContainsKey("here"))
      {
        EntityPlayer entityPlayer = null;
        if (SenderInfo.RemoteClientInfo != null)
        {
          entityPlayer = GameManager.Instance.World.Entities.dict[SenderInfo.RemoteClientInfo.entityId] as EntityPlayer;
        }

        if (entityPlayer == null) return;

        loc.RoundToInt(entityPlayer.position);
      }
      else
      {
        SendOutput(GetHelp());

        return;
      }

      //var chunk = GameManager.Instance.World.GetChunkFromWorldPos(loc) as Chunk;
      //todo: call the regeneration methods from ChunkProviderGenerateWorld
      //chunk.Reset();
    }
  }
}
