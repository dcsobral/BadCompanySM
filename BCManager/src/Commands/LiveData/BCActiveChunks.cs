using System.Collections.Generic;

namespace BCM.Commands
{
  public class BCActiveChunks : BCCommandAbstract
  {
    public class BCMChunkInfo
    {
      public int X;
      public int Z;
      public bool IsDisplayed;
      public int Mem;

      public BCMChunkInfo(Chunk chunk)
      {
        X = chunk.X;
        Z = chunk.Z;
        IsDisplayed = chunk.IsDisplayed;
        Mem = chunk.GetUsedMem();
      }
    }

    public override void Process()
    {
      var data = new Dictionary<string, object>();
      var chunkClusters = GameManager.Instance.World.ChunkClusters[0];

      var syncRoot = chunkClusters.GetSyncRoot();
      lock (syncRoot)
      {
        data.Add("Count", chunkClusters.Count());

        using (var enumerator = chunkClusters.GetChunkArray().GetEnumerator())
        {
          var chunks = new List<object>();
          while (enumerator.MoveNext())
          {
            if (enumerator.Current != null)
            {
              chunks.Add(new BCMChunkInfo(enumerator.Current));
            }
          }
          data.Add("Chunks", chunks);
        }
      }

      SendJson(data);
    }
  }
}
