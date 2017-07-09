using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using UnityEngine;

namespace BCM.Commands
{
  public class BCExport : BCCommandAbstract
  {
    public override void Process()
    {
      //todo: report location and size exported

      Vector3i p1 = new Vector3i(int.MinValue, 0, int.MinValue);
      Vector3i p2 = new Vector3i(int.MinValue, 0, int.MinValue);
      string filename = null;

      if (_params.Count == 1)
      {
        //get loc and player current pos
        EntityPlayer sender = null;
        string steamId = null;
        Vector3i currentPos = new Vector3i(int.MinValue, 0, int.MinValue);
        if (_senderInfo.RemoteClientInfo != null)
        {
          steamId = _senderInfo.RemoteClientInfo.ownerId.ToString();
          sender = GameManager.Instance.World.Entities.dict[_senderInfo.RemoteClientInfo.entityId] as EntityPlayer;
          if (sender != null)
          {
            currentPos = new Vector3i((int)Math.Floor(sender.serverPos.x / 32f), (int)Math.Floor(sender.serverPos.y / 32f), (int)Math.Floor(sender.serverPos.z / 32f));
          }
          else
          {
            SendOutput("Error: unable to get player location");

            return;
          }
        }
        else
        {
          SendOutput("Error: unable to get player location");

          return;
        }

        if (steamId != null)
        {
          p1 = BCLocation.GetPos(steamId);
          if (p1.x == int.MinValue)
          {
            SendOutput("No location stored. Use bc-loc to store a location.");

            return;
          }
          p2 = currentPos;

          filename = _params[0];
        }
        else
        {
          SendOutput("Error: unable to get player location");

          return;
        }
      }
      else if (_params.Count == 7)
      {
        //parse params
        if (!int.TryParse(_params[0], out p1.x) || !int.TryParse(_params[1], out p1.y) || !int.TryParse(_params[2], out p1.z) || !int.TryParse(_params[3], out p2.x) || !int.TryParse(_params[4], out p2.y) || !int.TryParse(_params[5], out p2.z))
        {
          SendOutput("Error: unable to parse coordinates");

          return;
        }
        filename = _params[6];
      }
      else
      {
        SendOutput("Error: Incorrect command format.");
        SendOutput(GetHelp());

        return;
      }


      if (filename != null)
      {
        Prefab _prefab = new Prefab();
        _prefab.CopyFromWorld(GameManager.Instance.World, p1, p2);
        //_prefab.CopyFromWorldWithEntities();

        _prefab.filename = filename;
        _prefab.bCopyAirBlocks = true;
        //todo: parse additional config from options

        string _dir = "Data/Prefabs";
        if (_options.ContainsKey("backup"))
        {
          _dir = "Data/Prefabs/Backup";
        }
        if (_prefab.Save(_dir, _prefab.filename))
        {
          //todo: create blocks info file and mesh
          SendOutput("Prefab " + _prefab.filename + " exported.");
        }
        else
        {
          SendOutput("Error: Prefab " + _prefab.filename + " failed to save.");
        }
      }
      else
      {
        SendOutput("Error: Prefab filename was null.");
      }

    }
  }
}
