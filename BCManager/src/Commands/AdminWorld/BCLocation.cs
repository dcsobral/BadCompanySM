using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using UnityEngine;

namespace BCM.Commands
{
  public class BCLocation : BCCommandAbstract
  {
    private static Dictionary<string, Vector3i> _cache = new Dictionary<string, Vector3i>();

    public override void Process()
    {
      EntityPlayer sender = null;
      string steamId = null;
      Vector3i pos = new Vector3i(int.MinValue, 0, int.MinValue);
      if (_senderInfo.RemoteClientInfo != null)
      {
        steamId = _senderInfo.RemoteClientInfo.ownerId.ToString();
        sender = GameManager.Instance.World.Entities.dict[_senderInfo.RemoteClientInfo.entityId] as EntityPlayer;
        if (sender != null)
        {
          pos = new Vector3i((int)Math.Floor(sender.serverPos.x / 32f), (int)Math.Floor(sender.serverPos.y / 32f), (int)Math.Floor(sender.serverPos.z / 32f));
        }
      }

      _cache[steamId] = pos;

      if (sender != null)
      {
        SendOutput("Current Location: " + pos.x + " " + pos.y + " " + pos.z);
        SendOutput("Distance Below Ground: " + ((int)sender.position.y - pos.y));
      }
      else
      {
        SendOutput("Error getting location of command sender.");
      }
    }

    public static Vector3i GetPos(string steamId)
    {
      return _cache.ContainsKey(steamId) ? _cache[steamId] : new Vector3i(int.MinValue, 0, int.MinValue);
    }

    public static void SetPos(string steamId, Vector3i pos)
    {
      _cache[steamId] = pos;
    }
  }
}
