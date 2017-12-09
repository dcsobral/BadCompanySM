using System.Collections.Generic;

namespace BCM.Commands
{
  public class BCGetSpawn : BCCommandAbstract
  {
    public override void Process()
    {

      if (Params.Count != 2)
      {
        SendOutput(GetHelp());

        return;
      }

      if (!int.TryParse(Params[0], out var x))
      {
        SendOutput("x was not a number");

        return;
      }
      if (!int.TryParse(Params[1], out var z))
      {
        SendOutput("z was not a number");

        return;
      }

      if (GameManager.Instance.World == null)
      {
        SendOutput("The world isn't loaded");

        return;
      }

      var world = GameManager.Instance.World;

      var x2 = World.toChunkXZ(x);
      var z2 = World.toChunkXZ(z);

      var i = 0;
      Chunk chunk = null;
      world.ChunkCache.ChunkProvider.RequestChunk(x2, z2);
      var chunkKey = WorldChunkCache.MakeChunkKey(x2, z2);
      while (i < 100)
      {
        chunk = world.GetChunkSync(chunkKey) as Chunk;
        if (chunk != null)
        {
          break;
        }
        System.Threading.Thread.Sleep(10);
        i++;
      }
      //todo: allow for /timeout to increase limit?

      if (chunk == null)
      {
        SendOutput("Unable to get chunk");

        return;
      }

      var heights = new List<string>();

      if (Options.ContainsKey("ch"))
      {
        for (var x3 = 0; x3 < 16; x3++)
        {
          for (var z3 = 0; z3 < 16; z3++)
          {
            heights.Add(chunk.GetHeight(x3, z3).ToString());
          }
        }
        SendOutput("ChunkHeights:" + string.Join(",", heights.ToArray()));
      }
      else
      {
        var cx = x & 15;
        var cz = z & 15;
        if (Options.ContainsKey("ph"))
        {
          SendOutput("PointHeight:" + chunk.GetHeight(cx, cz));
        }
        else
        {
          if (chunk.FindSpawnPointAtXZ(cx, cz, out var y, 15, 0, 3, 251, true))
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
