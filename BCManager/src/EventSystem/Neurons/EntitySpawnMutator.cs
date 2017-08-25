using System.Collections.Generic;

namespace BCM.Neurons
{
  public class EntitySpawnMutator : NeuronAbstract
  {
    private bool _hasDelegate;
    private readonly List<string> _options = new List<string>();

    public EntitySpawnMutator(Synapse s)
    {
      foreach (var o in s.Options.Split(','))
      {
        _options.Add(o);
      }
    }
    public override void Fire(int b)
    {
      if (GameManager.Instance.World == null) return;
      if (_hasDelegate || !API.IsAlive) return;

      GameManager.Instance.World.EntityLoadedDelegates += OnEntityLoaded;
      Log.Out("EntitySpawnMutator Initialised");

      _hasDelegate = true;
    }

    private void OnEntityLoaded(Entity entity)
    {
      if (_options.Contains("EntityEnemy"))
      {
        var enemy = entity as EntityEnemy;
        if (enemy != null)
        {
          ProcessEntityEnemy(enemy);
        }
      }
      else
      {
        if (_options.Contains("EntityNPC"))
        {
          var npc = entity as EntityNPC;
          if (npc != null)
          {
            ProcessEntityNpc(npc);
          }
        }
      }

      //todo: use threaded task to get chunk and report ground pos
      //if (_options.Contains("EntitySupplyCrate"))
      //{
      //  var crate = entity as EntitySupplyCrate;
      //  if (crate != null)
      //  {
      //    ProcessEntitySupplyCrate(crate);
      //  }
      //}

      //todo: apply settings to entity based on config
    }

    private static string GetClassName(Entity entity)
    {
      if (!EntityClass.list.ContainsKey(entity.entityClass)) return string.Empty;

      var ec = EntityClass.list[entity.entityClass];
      return ec != null ? ec.entityClassName : string.Empty;
    }

    private static void ProcessEntityEnemy(EntityEnemy entity)
    {
      Log.Out($"{Config.ModPrefix} Spawn Detected:{GetClassName(entity)}[{entity.entityId}]({entity.GetType()})@{(int) entity.position.x} {(int) entity.position.y} {(int) entity.position.z}");
    }

    private static void ProcessEntityNpc(EntityNPC entity)
    {
      Log.Out($"{Config.ModPrefix} Spawn Detected:{GetClassName(entity)}[{entity.entityId}]({entity.GetType()})@{(int) entity.position.x} {(int) entity.position.y} {(int) entity.position.z}");
    }

    //private void ProcessEntitySupplyCrate(EntitySupplyCrate entity)
    //{
    //  var p = new Vector3i();
    //  p.RoundToInt(entity.position);
    //  int y;
    //  var x2 = World.toBlockXZ(p.x);
    //  var z2 = World.toBlockXZ(p.z);

    //  //object syncRoot = GameManager.Instance.World.ChunkClusters[0].GetSyncRoot();
    //  //lock (syncRoot)
    //  //{
    //  //}
    //  //List<Chunk> chunkArray = GameManager.Instance.World.ChunkClusters[0].GetChunkArray();

    //  var world = GameManager.Instance.World;
    //  var chunkKey = WorldChunkCache.MakeChunkKey(x2, z2);
    //  world.ChunkCache.ChunkProvider.RequestChunk(x2, z2);

    //  //todo: needs to be processed in subthread

    //  var chunk = world.GetChunkSync(chunkKey) as Chunk;
    //  if (chunk == null) return;

    //  if (chunk.FindSpawnPointAtXZ(x2, z2, out y, 15, 0, 3, 251, true))
    //  {
    //    Log.Out($"{Config.ModPrefix} AirDropGroundPos:[{(int) entity.position.x} {y} {(int) entity.position.z}]");
    //  }
    //  else
    //  {
    //    Log.Out(Config.ModPrefix + " AirDropGroundPos: No valid spawn point found");
    //  }
    //}
  }
}
