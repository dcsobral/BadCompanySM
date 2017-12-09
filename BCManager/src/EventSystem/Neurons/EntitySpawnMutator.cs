using System;
using System.IO;
using System.Xml;

namespace BCM.Neurons
{
  public class EntitySpawnMutator : NeuronAbstract
  {
    private bool _hasDelegate;
    private readonly bool hasEnemy;
    private readonly bool hasNpc;
    private readonly bool hasItem;
    //private readonly bool hasFallingBlock;

    private readonly float lifetime = 60f;
    private readonly bool logItems;

    public EntitySpawnMutator(Synapse s) : base (s)
    {
      if (!string.IsNullOrEmpty(synapse.Cfg))
      {
        var xmlDoc = new XmlDocument();
        try
        {
          xmlDoc.Load(File.Exists($"{Config.EventsPath}{synapse.Cfg}.xml")
            ? $"{Config.EventsPath}{synapse.Cfg}.xml"
            : $"{Config.DefaultEventsPath}{synapse.Cfg}.xml");

          var nodeLifetime = xmlDoc.SelectNodes("/SpawnMutator/Lifetime");
          if (nodeLifetime != null && nodeLifetime.Count != 0 && nodeLifetime.Item(0) is XmlElement n &&
              n.HasAttribute("value") && !float.TryParse(n.GetAttribute("value"), out lifetime))
            Log.Out($"{Config.ModPrefix} Unable to parse value for lifetime in Spawn Mutator config");

          var nodeLogItems = xmlDoc.SelectNodes("/SpawnMutator/LogDroppedItems");
          if (nodeLogItems != null && nodeLogItems.Count != 0 && nodeLogItems.Item(0) is XmlElement l &&
              l.HasAttribute("value") && !bool.TryParse(l.GetAttribute("value"), out logItems))
            Log.Out($"{Config.ModPrefix} Unable to parse value for logitems in Spawn Mutator config");
        }
        catch (Exception e)
        {
          Log.Error($"{Config.ModPrefix} Error configuring Spawn Mutator\n{e}");
        }
      }

      foreach (var o in synapse.Options.Split(','))
      {
        switch (o.Trim())
        {
          case "EntityEnemy":
            hasEnemy = true;
            break;
          case "EntityNPC":
            hasNpc = true;
            break;
          case "EntityItem":
            hasItem = true;
            break;
          //case "EntityFallingBlock":
          //  hasFallingBlock = true;
          //  break;
          default:
            Log.Out($"{Config.ModPrefix} Unknown Spawn Mutator entity class {o}");
            break;
        }
      }
    }

    public override void Fire(int b)
    {
    }

    public override void Awake()
    {
      if (GameManager.Instance.World == null) return;
      if (_hasDelegate || !API.IsAwake) return;

      GameManager.Instance.World.EntityLoadedDelegates += OnEntityLoaded;

      //GameManager.Instance.World.ChunkCache.OnBlockChangedDelegates += OnBlockChanged;

      Log.Out($"{Config.ModPrefix} EntitySpawnMutator Initialised");

      _hasDelegate = true;
    }

    //private void OnBlockChanged(Vector3i _blockpos, BlockValue _blockvalueold, BlockValue _blockvaluenew)
    //{
    //  Log.Out($"Block Type: {_blockvalueold.type} changed to {_blockvaluenew.type} @ {_blockpos}");
    //}

    private void OnEntityLoaded(Entity entity)
    {
      if (hasEnemy && entity is EntityEnemy enemy)
      {
        ProcessEntityEnemy(enemy);

        return;
      }
      if (hasNpc && entity is EntityNPC npc)
      {
        ProcessEntityNpc(npc);

        return;
      }
      if (hasItem && entity.GetType() == typeof(EntityItem) && entity is EntityItem item)
      {
        ProcessEntityItem(item);

        return;
      }
      //if (hasFallingBlock && entity is EntityFallingBlock block)
      //{
      //  ProcessEntityFallingBlock(block);

      //  return;
      //}

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

    //private static void ProcessEntityFallingBlock(EntityFallingBlock block)
    //{

    //}

    private void ProcessEntityItem(EntityItem item)
    {
      if (logItems)
      {
        Log.Out($"{Config.ModPrefix} Item Dropped:[{item.entityId}]{item.itemStack.count}x {item.itemStack.itemValue.ItemClass.Name} @{(int)item.position.x} {(int)item.position.y} {(int)item.position.z}");
      }
      item.lifetime = lifetime;
    }

    private static string GetClassName(Entity entity)
    {
      if (!EntityClass.list.ContainsKey(entity.entityClass)) return string.Empty;

      var ec = EntityClass.list[entity.entityClass];
      return ec != null ? ec.entityClassName : string.Empty;
    }

    private static void ProcessEntityEnemy(EntityEnemy entity)
    {
      Log.Out($"{Config.ModPrefix} Spawn Detected:{GetClassName(entity)}[{entity.entityId}]({entity.GetType()})@{(int)entity.position.x} {(int)entity.position.y} {(int)entity.position.z}");
    }

    private static void ProcessEntityNpc(EntityNPC entity)
    {
      Log.Out($"{Config.ModPrefix} Spawn Detected:{GetClassName(entity)}[{entity.entityId}]({entity.GetType()})@{(int)entity.position.x} {(int)entity.position.y} {(int)entity.position.z}");
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
