using System.Collections.Generic;
using UnityEngine;

namespace BCM.Commands
{
  public class ListActiveChunks : BCCommandAbstract
  {
    public virtual Dictionary<string, string> jsonObject()
    {
      var data = new Dictionary<string, string>();

      var num = 1;
      object syncRoot = GameManager.Instance.World.ChunkClusters[0].GetSyncRoot();
      lock (syncRoot)
      {
        //output
        data.Add("count", GameManager.Instance.World.ChunkClusters[0].Count().ToString());

        using (List<Chunk>.Enumerator enumerator = GameManager.Instance.World.ChunkClusters[0].GetChunkArray().GetEnumerator())
        {
          var chunks = new Dictionary<string, string>();
          while (enumerator.MoveNext())
          {
            Chunk current = enumerator.Current;
            var chunk = new Dictionary<string, string>();

            //outputs
            //chunk.Add("index", num++.ToString());
            chunk.Add("x", current.X.ToString());
            chunk.Add("z", current.Z.ToString());
            chunk.Add("displayed", current.IsDisplayed.ToString());
            //chunk.Add("meshlayercount", current.MeshLayerCount.ToString());

            chunk.Add("usedMem", current.GetUsedMem().ToString());
            chunk.Add("DominantBiome", current.DominantBiome.ToString());
            //chunk.Add("mem", (current.GetUsedMem() / 1024).ToString());
            //chunk.Add("chunkmem", (current.GetUsedMem() / 1048576).ToString() + "MB");
            chunks.Add(num++.ToString(),BCUtils.toJson(chunk));
          }
          data.Add("chunks", BCUtils.toJson(chunks));
        }
      }

      return data;
    }
    public override void Process()
    {
      string output = "";
      if (_options.ContainsKey("json"))
      {
        if (_options.ContainsKey("tag"))
        {
          if (_options["tag"] == null)
          {
            _options["tag"] = "bc-cc";
          }

          SendOutput("{\"tag\":\"" + _options["tag"] + "\",\"data\":" + BCUtils.toJson(jsonObject()) + "}");
        }
        else
        {
          SendOutput(BCUtils.toJson(jsonObject()));
        }
      }
      else
      {
        output = "Not Implemented, use /json";
        SendOutput(output);
      }
    }
  }
}
