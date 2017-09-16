using System;
using System.Collections.Generic;

namespace BCM.Commands
{
  public class BCLocation : BCCommandAbstract
  {
    private static Dictionary<string, Vector3i> _cache = new Dictionary<string, Vector3i>();

    public override void Process()
    {
      var pos = new Vector3i(int.MinValue, 0, int.MinValue);
      EntityPlayer sender = null;
      string steamId = null;
      if (SenderInfo.RemoteClientInfo != null)
      {
        steamId = SenderInfo.RemoteClientInfo.ownerId;
        sender = GameManager.Instance.World.Entities.dict[SenderInfo.RemoteClientInfo.entityId] as EntityPlayer;
        if (sender != null && Params.Count == 0)
        {
          pos = new Vector3i((int)Math.Floor(sender.serverPos.x / 32f), (int)Math.Floor(sender.serverPos.y / 32f), (int)Math.Floor(sender.serverPos.z / 32f));
        }
      }

      if (Params.Count == 0)
      {
        if (sender == null || steamId == null)
        {
          SendOutput("Error getting location of command sender.");
        }
        else
        {
          SetPos(steamId, pos);

          SendOutput("Current Location: " + pos.x + " " + pos.y + " " + pos.z);
          if (Options.ContainsKey("h"))
          {
            int height = GameManager.Instance.World.GetHeight(pos.x, pos.z);
            SendOutput("Distance To Ground Height: " + (pos.y - height - 1));
          }
          if (Options.ContainsKey("c"))
          {
            SendOutput("ChunkXZ: " + (pos.x >> 4) + "," + (pos.z >> 4) + " + " + "ChunkBlockXZ: " + (pos.x & 15) + "," +
                       (pos.z & 15));
          }
          if (Options.ContainsKey("r"))
          {
            SendOutput("RegionXZ: " + (pos.x >> 9) + "," + (pos.z >> 9));
          }
        }
      }
      else if (Params.Count == 3 && int.TryParse(Params[0], out pos.x) && int.TryParse(Params[1], out pos.y) && int.TryParse(Params[2], out pos.z) && sender != null && steamId != null)
      {
        SetPos(steamId, pos);
      }
    }

    public static Vector3i GetPos(string steamId)
    {
      return steamId == null || !_cache.ContainsKey(steamId) ? new Vector3i(int.MinValue, 0, int.MinValue) : _cache[steamId];
    }

    public static void SetPos(string steamId, Vector3i pos)
    {
      _cache[steamId] = pos;
    }
  }
}
