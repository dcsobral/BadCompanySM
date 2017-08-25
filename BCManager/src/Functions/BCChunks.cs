using System;
using System.Collections.Generic;
using System.Threading;

namespace BCM
{
  public static class BCChunks
  {
    public static bool ReloadForClients(Dictionary<long, Chunk> chunks)
    {
      var world = GameManager.Instance.World;
      if (world == null) return false;

      //RESET CHUNK STABILITY
      ResetStability(world, chunks);

      // REFRESH CLIENTS CHUNKS
      var reloadforclients = new Dictionary<ClientInfo, List<long>>();
      foreach (var client in ConnectionManager.Instance.GetClients())
      {
        try
        {
          if (!world.Entities.dict.ContainsKey(client.entityId)) continue;

          var entityPlayer = world.Entities.dict[client.entityId] as EntityPlayer;
          if (entityPlayer == null) continue;

          var chunksLoaded = entityPlayer.ChunkObserver.chunksLoaded;
          if (chunksLoaded == null) continue;

          foreach (var chunkKey in chunksLoaded)
          {
            if (!chunks.ContainsKey(chunkKey)) continue;

            if (reloadforclients.ContainsKey(client))
            {
              reloadforclients[client].Add(chunkKey);
            }
            else
            {
              reloadforclients.Add(client, new List<long> { chunkKey });
            }

            client.SendPackage(new NetPackageChunkRemove(chunkKey));
          }

          if (reloadforclients.ContainsKey(client))
          {
            Log.Out($"{Config.ModPrefix} Reloading {reloadforclients[client].Count}/{entityPlayer.ChunkObserver.chunksLoaded.Count} chunks for {client.playerName}");
          }
        }
        catch (Exception e)
        {
          Log.Out($"{Config.ModPrefix} Error removing chunks for {client.playerName}:\n{e}");
        }
      }

      // delay to allow remove chunk packets to reach clients
      Thread.Sleep(50);

      foreach (var client in reloadforclients.Keys)
      {
        try
        {
          if (reloadforclients[client] == null) continue;

          var chunkCache = world.ChunkClusters[0];
          if (chunkCache == null) continue;

          var entityPlayer = world.Entities.dict[client.entityId] as EntityPlayer;
          if (entityPlayer == null) continue;

          var chunkKeys = chunkCache.GetChunkKeysCopySync();
          foreach (var chunkKey in reloadforclients[client])
          {
            if (!chunkKeys.Contains(chunkKey) || !entityPlayer.ChunkObserver.chunksLoaded.Contains(chunkKey)) continue;

            var chunk = chunkCache.GetChunkSync(chunkKey);
            if (chunk == null) continue;

            try
            {
              client.SendPackage(new NetPackageChunk(chunk));
            }
            catch (Exception e)
            {
              Log.Out($"{Config.ModPrefix} Error reloading chunk {chunkKey} for {client.playerName}:\n{e}");
            }
          }
        }
        catch (Exception e)
        {
          Log.Out($"{Config.ModPrefix} Error resending chunks for {client.playerName}:\n{e}");
        }

      }

      return true;
    }

    public static void ResetStability(World world, Dictionary<long, Chunk> chunks)
    {
      var si = new StabilityInitializer(world);
      foreach (var chunk in chunks.Values)
      {
        chunk.ResetStability();
      }

      foreach (var chunk in chunks.Values)
      {
        si.DistributeStability(chunk);
        chunk.NeedsRegeneration = true;
        chunk.NeedsLightCalculation = true;
      }
    }
  }
}
