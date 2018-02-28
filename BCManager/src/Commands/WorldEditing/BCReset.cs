using System.Collections.Generic;
using System.Reflection;

namespace BCM.Commands
{
  public class BCReset : BCCommandAbstract
  {
    private readonly string _decorateFunction = "MH";

    public override void Process()
    {
      var world = GameManager.Instance.World;
      if (world == null) return;

      //todo add a 1 param option for 'bc-reset here' to reset chunk player is in
      //Resets the chunk to the original state it was created in
      if (Params.Count != 2)
      {
        SendOutput("Incorrect Params");
        SendOutput(GetHelp());

        return;
      }

      if (!int.TryParse(Params[0], out var cx) || !int.TryParse(Params[1], out var cz))
      {
        SendOutput("Unable to parse cx or cz as numbers");

        return;
      }
      var chunkKey = WorldChunkCache.MakeChunkKey(cx, cz);

      //todo: make unloaded chunk option
      //todo: deferred load
      var chunkCache = world.ChunkCache;
      if (!(chunkCache.ChunkProvider is ChunkProviderGenerateWorld chunkProvider))
      {
        SendOutput("Unable to load chunk provider");

        return;
      }
      //todo: deferred load
      var chunkSync = chunkCache.GetChunkSync(chunkKey);
      if (chunkSync == null)
      {
        SendOutput("Chunk not loaded");

        return;
      }

      //create reset chunk
      var chunk = MemoryPools.PoolChunks.AllocSync(true);
      if (chunk == null)
      {
        SendOutput("Couldn't allocate chunk from memory pool");

        return;
      }

      chunk.X = cx;
      chunk.Z = cz;

      if (!(chunkProvider.GetTerrainGenerator() is TerrainGeneratorWithBiomeResource terrainGenerator))
      {
        SendOutput("Couldn't load terrain generator");

        return;
      }

      var random = Utils.RandomFromSeedOnPos(cx, cz, world.Seed);
      terrainGenerator.GenerateTerrain(world, chunk, random);
      chunk.NeedsDecoration = true;
      chunk.NeedsLightCalculation = true;
      DoPrefabDecorations(world, chunkProvider, chunk, random);
      DoEntityDecoration(world, chunkProvider, chunk, random);
      DoSpawnerDecorations(world, chunkProvider, chunk, random);

      //UPDATE CHUNK
      var syncRoot = chunkCache.GetSyncRoot();
      lock (syncRoot)
      {
        //todo: instead of remove and add, generate the chunk then use regular setblockrpc calls to update chunk blocks
        //remove old chunk
        if (chunkCache.ContainsChunkSync(chunkKey))
        {
          chunkCache.RemoveChunkSync(chunkKey);
        }

        if (chunkCache.ContainsChunkSync(chunk.Key))
        {
          SendOutput("Reset chunk still exists in chunk cache");

          return;
        }

        if (!chunkCache.AddChunkSync(chunk))
        {
          MemoryPools.PoolChunks.FreeSync(chunk);
          SendOutput("Unable to add new chunk to cache");

          return;
        }
      }
      var decorateWithNeigbours = typeof(ChunkProviderGenerateWorld).GetMethod(_decorateFunction, BindingFlags.NonPublic | BindingFlags.Instance);
      if (decorateWithNeigbours == null)
      {
        SendOutput("Couldn't access method for DecorateWithNeigbours");

        return;
      }
      decorateWithNeigbours.Invoke(chunkProvider, new object[] { chunk });

      chunk.InProgressRegeneration = false;
      chunk.NeedsCopying = true;
      chunk.isModified = true;

      SendOutput($"Chunk reset @ {cx},{cz}");
      Log.Out($"{Config.ModPrefix} Chunk reset @ {cx},{cz}");

      //RELOAD CHUNKS
      if (!(Options.ContainsKey("noreload") || Options.ContainsKey("nr")))
      {
        BCChunks.ReloadForClients(new Dictionary<long, Chunk> { { chunkKey, chunk } });
      }
    }

    private static void DoSpawnerDecorations(World world, ChunkProviderGenerateWorld chunkProvider, Chunk chunk, System.Random random)
    {
      if (chunkProvider.GetDynamicEntitySpawnerDecorator() != null)
      {
        chunkProvider.GetDynamicEntitySpawnerDecorator().DecorateChunk(world, chunk, random);
      }
      else
      {
        SendOutput("Couldn't load entity spawner decorator");
      }
    }

    private static void DoEntityDecoration(World world, ChunkProviderGenerateWorld chunkProvider, Chunk chunk, System.Random random)
    {
      if (chunkProvider.GetDynamicEntityDecorator() != null)
      {
        chunkProvider.GetDynamicEntityDecorator().DecorateChunk(world, chunk, random);
      }
      else
      {
        SendOutput("Couldn't load entity decorator");
      }
    }

    private static void DoPrefabDecorations(World world, ChunkProviderGenerateWorld chunkProvider, Chunk chunk, System.Random random)
    {
      if (chunkProvider.GetDynamicPrefabDecorator() != null)
      {
        chunkProvider.GetDynamicPrefabDecorator().DecorateChunk(world, chunk, random);
      }
      else
      {
        SendOutput("Couldn't load prefab decorator");
      }
    }
  }
}
