using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using UnityEngine;

namespace BCM.Commands
{
  public class BCReset : BCCommandAbstract
  {
    public override void Process()
    {
      //todo: reset region

      Vector3i loc = new Vector3i();
      loc.y = 0;

      if (_params.Count == 2)
      {
        if (!int.TryParse(_params[0], out loc.x) || !int.TryParse(_params[1], out loc.z))
        {
          SendOutput("One of <x> <z> params could not be parsed as a number.");
          return;
        }
      }
      else if (_options.ContainsKey("here"))
      {
        EntityPlayer _e = null;
        if (_senderInfo.RemoteClientInfo != null)
        {
          _e = GameManager.Instance.World.Entities.dict[_senderInfo.RemoteClientInfo.entityId] as EntityPlayer;
        }

        if (_e == null)
        {
          return;
        }

        loc.RoundToInt(_e.position);
      }
      else
      {
        SendOutput(GetHelp());

        return;
      }

      Chunk _chunk = GameManager.Instance.World.GetChunkFromWorldPos(loc) as Chunk;
      //todo: find a way that doesnt break the server
      //_chunk.Reset();
    }
  }
}
