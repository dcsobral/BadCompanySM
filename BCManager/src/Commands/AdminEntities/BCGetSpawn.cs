using System;
using System.Collections.Generic;
using UnityEngine;

namespace BCM.Commands
{
  public class BCGetSpawn : BCCommandAbstract
  {
    public override void Process()
    {
      int x = 0;
      int z = 0;

      if (_params.Count != 2)
      {
        SendOutput(GetHelp());

        return;
      }

      if (!int.TryParse(_params[0], out x))
      {
        SendOutput("x was not a number");

        return;
      }
      if (!int.TryParse(_params[1], out z))
      {
        SendOutput("z was not a number");

        return;
      }

      if (GameManager.Instance.World == null)
      {
        SendOutput("The world isn't loaded");

        return;
      }

      var _world = GameManager.Instance.World;

      int x2 = World.toChunkXZ(x);
      int z2 = World.toChunkXZ(z);
      int y = 0;

      int i = 0;
      Chunk chunk = null;
      GameManager.Instance.World.ChunkCache.ChunkProvider.RequestChunk(x2, z2);
      long chunkKey = WorldChunkCache.MakeChunkKey(x2, z2);
      while (i < 100)
      {
        chunk = GameManager.Instance.World.GetChunkSync(chunkKey) as Chunk;
        if (chunk != null)
        {
          break;
        }
        System.Threading.Thread.Sleep(10);
        i++;
      }
      //todo: allow for /timeout to increase limit?
      //SendOutput("debug:" + i);

      if (chunk == null)
      {
        SendOutput("Unable to get chunk");

        return;
      }

      var h = new List<string>();

      if (_options.ContainsKey("ch"))
      {
        for (var _x = 0; _x < 16; _x++)
        {
          for (var _z = 0; _z < 16; _z++)
          {
            h.Add(chunk.GetHeight(_x, _z).ToString());
          }
        }
        SendOutput("ChunkHeights:" + string.Join(",", h.ToArray()));
      }
      else
      {
        int _cx = (x < 0 ? 15 - Math.Abs(x % 16) : Math.Abs(x % 16));
        int _cz = (z < 0 ? 15 - Math.Abs(z % 16) : Math.Abs(z % 16));
        if (_options.ContainsKey("ph"))
        {
          SendOutput("PointHeight:" + chunk.GetHeight(_cx, _cz).ToString());
        }
        else
        {
          if (chunk.FindSpawnPointAtXZ(_cx, _cz, out y, 15, 0, 3, 251, true))
          {
            SendOutput("SpawnPoint:" + x + " " + y + " " + z);
          }
          else
          {
            SendOutput("Couldn't find valid spawn point at " + x + " " + z);
          }
        }
      }

    }
  }
}
