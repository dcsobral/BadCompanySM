using System.Collections.Generic;
using System.Threading;

namespace BCM
{
  public static class BCChunks
  {
    public static bool ReloadForClients(Dictionary<long, Chunk> chunks)
    {
      //RESET CHUNK STABILITY
      StabilityInitializer _si = new StabilityInitializer(GameManager.Instance.World);
      foreach (Chunk _chunk in chunks.Values)
      {
        _chunk.ResetStability();
      }
      foreach (Chunk _chunk in chunks.Values)
      {
        _si.DistributeStability(_chunk);
        _chunk.NeedsRegeneration = true;
      }

      // REFRESH CLIENTS CHUNKS
      List<ClientInfo> clients = ConnectionManager.Instance.GetClients();
      //List<Entity> clients = GameManager.Instance.World.Entities.list;
      //clients = clients.Where(p => p.GetType() == typeof(EntityPlayer));
      Dictionary<ClientInfo, List<long>> reloadforclients = new Dictionary<ClientInfo, List<long>>();
      foreach (ClientInfo client in clients)
      {
        if (GameManager.Instance.World.Entities.dict.ContainsKey(client.entityId))
        {
          EntityPlayer EP = GameManager.Instance.World.Entities.dict[client.entityId] as EntityPlayer;

          if (EP != null) //EP.bIsChunkObserver
          {
            HashSet<long> chunksLoaded = EP.ChunkObserver.chunksLoaded;
            foreach (long _chunk in chunksLoaded)
            {
              if (chunks.ContainsKey(_chunk))
              {
                client.SendPackage(new NetPackageChunkRemove(_chunk));
                if (reloadforclients.ContainsKey(client))
                {
                  reloadforclients[client].Add(_chunk);
                }
                else
                {
                  reloadforclients.Add(client, new List<long> { _chunk });
                }
              }
            }
            Log.Out(Config.ModPrefix + " Reloading " + reloadforclients[client].Count.ToString() + "/" + EP.ChunkObserver.chunksLoaded.Count.ToString() + " chunks for " + client.playerName);
          }

        }
      }

      // delay to allow remove chunk packets to reach clients
      Thread.Sleep(50);
      foreach (ClientInfo client in reloadforclients.Keys)
      {
        if (reloadforclients[client] != null)
        {
          foreach (long _chunkKey in reloadforclients[client])
          {
            var chunkCache = GameManager.Instance.World.ChunkClusters[0];
            if (chunkCache != null)
            {
              var chunkKeys = chunkCache.GetChunkKeysCopySync();
              EntityPlayer EP = GameManager.Instance.World.Entities.dict[client.entityId] as EntityPlayer;
              if (chunkKeys.Contains(_chunkKey) && EP != null && EP.ChunkObserver.chunksLoaded.Contains(_chunkKey))
              {
                var c = chunkCache.GetChunkSync(_chunkKey);
                if (c != null)
                {
                  client.SendPackage(new NetPackageChunk(c));
                }
              }
            }
          }
        }
      }

      return true;
    }
  }
}
