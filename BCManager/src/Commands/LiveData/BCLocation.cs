using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace BCM.Commands
{
  [UsedImplicitly]
  public class BCLocation : BCCommandAbstract
  {
    private static readonly Dictionary<string, Vector3i> Cache = new Dictionary<string, Vector3i>();

    protected override void Process()
    {
      if (!BCUtils.CheckWorld(out var world)) return;

      var pos = new Vector3i(int.MinValue, 0, int.MinValue);
      EntityPlayer sender = null;
      string steamId = null;
      if (SenderInfo.RemoteClientInfo != null)
      {
        steamId = SenderInfo.RemoteClientInfo.ownerId;
        sender = world.Entities.dict[SenderInfo.RemoteClientInfo.entityId] as EntityPlayer;
        if (sender != null && Params.Count == 0)
        {
          pos = new Vector3i((int)Math.Floor(sender.serverPos.x / 32f), (int)Math.Floor(sender.serverPos.y / 32f), (int)Math.Floor(sender.serverPos.z / 32f));
        }
      }

      switch (Params.Count)
      {
        case 0:
          if (sender == null || steamId == null)
          {
            SendOutput("Error getting location of command sender.");
          }
          else
          {
            SetPos(steamId, pos);

            SendOutput("Location: " + pos.x + " " + pos.y + " " + pos.z);
            SendOutput("ChunkXZ: " + (pos.x >> 4) + "," + (pos.z >> 4) + " + " + "ChunkBlockXZ: " + (pos.x & 15) + "," + (pos.z & 15));
            SendOutput("RegionXZ: " + (pos.x >> 9) + "," + (pos.z >> 9) + " + " + "RegionBlockXZ: " + (pos.x & 511) + "," + (pos.z & 511));
            SendOutput("Distance To Ground Height: " + (pos.y - world.GetHeight(pos.x, pos.z) - 1));
            SendOutput("Distance To Terrain Height: " + (pos.y - world.GetTerrainHeight(pos.x, pos.z) - 1));
          }
          break;

        case 3 when int.TryParse(Params[0], out pos.x) && int.TryParse(Params[1], out pos.y) && int.TryParse(Params[2], out pos.z) && sender != null && steamId != null:
          SetPos(steamId, pos);

          SendOutput("Location set to: " + pos.x + " " + pos.y + " " + pos.z);
          SendOutput("ChunkXZ: " + (pos.x >> 4) + "," + (pos.z >> 4) + " + " + "ChunkBlockXZ: " + (pos.x & 15) + "," + (pos.z & 15));
          SendOutput("RegionXZ: " + (pos.x >> 9) + "," + (pos.z >> 9) + " + " + "RegionBlockXZ: " + (pos.x & 511) + "," + (pos.z & 511));
          SendOutput("Distance To Ground Height: " + (pos.y - world.GetHeight(pos.x, pos.z) - 1));
          SendOutput("Distance To Terrain Height: " + (pos.y - world.GetTerrainHeight(pos.x, pos.z) - 1));
          break;

        default:
          SendOutput(GetHelp());
          break;
      }
    }

    public static Vector3i GetPos(string steamId)
    {
      return steamId == null || !Cache.ContainsKey(steamId) ? new Vector3i(int.MinValue, 0, int.MinValue) : Cache[steamId];
    }

    private static void SetPos(string steamId, Vector3i pos)
    {
      Cache[steamId] = pos;
    }
  }
}
