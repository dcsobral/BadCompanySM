using System;
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
      Dictionary<ClientInfo, List<long>> reloadforclients = new Dictionary<ClientInfo, List<long>>();
      foreach (ClientInfo client in clients)
      {
        try
        {
          if (GameManager.Instance.World.Entities.dict.ContainsKey(client.entityId))
          {

            EntityPlayer EP = null;
            try
            {
              EP = GameManager.Instance.World.Entities.dict[client.entityId] as EntityPlayer;
            }
            catch (Exception ex)
            {
              Log.Out(Config.ModPrefix + " Unable to get player entity for " + client.playerName + "\n" + ex);
            }

            if (EP != null)
            {
              //todo: need a lock here?
              HashSet<long> chunksLoaded = new HashSet<long>();
              try
              {
                chunksLoaded = EP.ChunkObserver.chunksLoaded;
              }
              catch (Exception ex)
              {
                Log.Out(Config.ModPrefix + " Error getting chunks loaded for " + client.playerName + "\n" + ex);
              }

              foreach (long _chunkKey in chunksLoaded)
              {
                if (chunks.ContainsKey(_chunkKey))
                {
                  try
                  {
                    client.SendPackage(new NetPackageChunkRemove(_chunkKey));
                    if (reloadforclients.ContainsKey(client))
                    {
                      reloadforclients[client].Add(_chunkKey);
                    }
                    else
                    {
                      reloadforclients.Add(client, new List<long> { _chunkKey });
                    }
                  }
                  catch (Exception ex)
                  {
                    Log.Out(Config.ModPrefix + " Error removing chunk " + _chunkKey + " for " + client.playerName + ":\n" + ex);
                  }
                }
              }
              if (reloadforclients.ContainsKey(client))
              {
                Log.Out(Config.ModPrefix + " Reloading " + reloadforclients[client].Count.ToString() + "/" + EP.ChunkObserver.chunksLoaded.Count.ToString() + " chunks for " + client.playerName);
              }
            }

          }

        }
        catch (Exception ex)
        {
          Log.Out(Config.ModPrefix + " Error removing chunks for " + client.playerName + ":\n" + ex);
        }
      }

      // delay to allow remove chunk packets to reach clients
      Thread.Sleep(50);
      foreach (ClientInfo client in reloadforclients.Keys)
      {
        try
        {

          if (reloadforclients[client] != null)
          {
            foreach (long _chunkKey in reloadforclients[client])
            {
              //move to outside loop?
              var chunkCache = GameManager.Instance.World.ChunkClusters[0];
              if (chunkCache != null)
              {
                var chunkKeys = chunkCache.GetChunkKeysCopySync();
                EntityPlayer EP = null;
                try
                {
                  EP = GameManager.Instance.World.Entities.dict[client.entityId] as EntityPlayer;
                }
                catch (Exception ex)
                {
                  Log.Out(Config.ModPrefix + " Unable to get player entity for " + client.playerName + "\n" + ex);
                }
                if (EP != null)
                {
                  if (chunkKeys.Contains(_chunkKey) && EP.ChunkObserver.chunksLoaded.Contains(_chunkKey))
                  {
                    var c = chunkCache.GetChunkSync(_chunkKey);
                    if (c != null)
                    {
                      try
                      {
                        client.SendPackage(new NetPackageChunk(c));
                      }
                      catch (Exception ex)
                      {
                        Log.Out(Config.ModPrefix + " Error reloading chunk " + _chunkKey + " for " + client.playerName + ":\n" + ex);
                      }
                    }
                  }
                }
              }
            }
          }
        }
        catch (Exception ex)
        {
          Log.Out(Config.ModPrefix + " Error resending chunks for " + client.playerName + ":\n" + ex);
        }

      }

      return true;
    }
  }
}
