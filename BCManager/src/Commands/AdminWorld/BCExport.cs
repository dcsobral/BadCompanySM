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
      if (_params.Count != 7)
      {
        SendOutput("Param error, use bc-export <x1> <y1> <z1> <x2> <y2> <z2> <filename>");
      }
      else
      {
        int x1, x2, y1, y2, z1, z2;
        if (!int.TryParse(_params[0], out x1) || !int.TryParse(_params[1], out y1) || !int.TryParse(_params[2], out z1) || !int.TryParse(_params[3], out x2) || !int.TryParse(_params[4], out y2) || !int.TryParse(_params[5], out z2))
        {
          SendOutput("Error parsing coordinates");
        }
        else
        {
          Prefab _prefab = new Prefab();
          _prefab.CopyFromWorld(GameManager.Instance.World, new Vector3i(x1, y1, z1), new Vector3i(x2, y2, z2));
          //_prefab.CopyFromWorldWithEntities();

          _prefab.filename = _params[6];
          _prefab.bCopyAirBlocks = true;
          //todo: parse additional config from options

          string _dir = "Data/Prefabs";
          if (_options.ContainsKey("backup"))
          {
            _dir = "Data/Prefabs/Backup";
          }
          if (_prefab.Save(_dir, _params[6]))
          {
            //todo: create blocks info file and mesh
            SendOutput("Prefab " + _params[6] + " exported.");
          }
          else
          {
            SendOutput("Prefab " + _params[6] + " failed to save.");
          }
        }
      }
    }
  }
}
