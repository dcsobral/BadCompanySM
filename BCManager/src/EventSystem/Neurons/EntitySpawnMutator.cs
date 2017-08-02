using System;
using System.Collections.Generic;
using UnityEngine;

namespace BCM.Neurons
{
  public class EntitySpawnMutator : NeuronAbstract
  {
    private bool hasDelegate = false;
    private Synapse synapse = null;
    private List<string> options = new List<string>();

    public EntitySpawnMutator(Synapse s)
    {
      synapse = s;
      foreach(var o in synapse.options.Split(','))
      {
        options.Add(o);
      }
    }
    public override bool Fire(int b)
    {
      if (!hasDelegate && API.IsAlive)
      {
        if (GameManager.Instance.World != null)
        {
          GameManager.Instance.World.EntityLoadedDelegates += new World.OnEntityLoadedDelegate(OnEntityLoaded);
          Log.Out("EntitySpawnMutator Initialised");

          hasDelegate = true;
        }
      }

      return true;
    }

    private void OnEntityLoaded(Entity _entity)
    {
      if (_entity is EntityFallingBlock || _entity is EntityItem || _entity is EntityLootContainer)
      {
        //todo
        return;
      }

      if (_entity is EntityEnemy || _entity is EntityNPC || _entity is EntitySupplyCrate)
      {

        if (_entity is EntityEnemy && options.Contains("EntityEnemy"))
        {
          ProcessEntityEnemy(_entity as EntityEnemy);
        } else if (_entity is EntityNPC && options.Contains("EntityNPC"))
        {
          ProcessEntityNPC(_entity as EntityNPC);
        }

        if (_entity is EntitySupplyCrate && options.Contains("EntitySupplyCrate"))
        {
          ProcessEntitySupplyCrate(_entity as EntitySupplyCrate);
        }
        
        //todo: apply settings to entity based on config
      }

    }

    private string GetClassName(Entity _entity)
    {
      string className = "";
      EntityClass ec = null;

      if (EntityClass.list.ContainsKey(_entity.entityClass))
      {
        ec = EntityClass.list[_entity.entityClass];
      }

      if (ec != null)
      {
        className = ec.entityClassName;
      }

      return className;
    }

    private void ProcessEntityEnemy(EntityEnemy _entity)
    {
      Log.Out(Config.ModPrefix + " Spawn Detected:" + GetClassName(_entity) + "[" + _entity.entityId.ToString() + "](" + _entity.GetType().ToString() + ")@" + ((int)_entity.position.x).ToString() + " " + ((int)_entity.position.y).ToString() + " " + ((int)_entity.position.z).ToString());
    }

    private void ProcessEntityNPC(EntityNPC _entity)
    {
      Log.Out(Config.ModPrefix + " Spawn Detected:" + GetClassName(_entity) + "[" + _entity.entityId.ToString() + "](" + _entity.GetType().ToString() + ")@" + ((int)_entity.position.x).ToString() + " " + ((int)_entity.position.y).ToString() + " " + ((int)_entity.position.z).ToString());
    }

    private void ProcessEntitySupplyCrate(EntitySupplyCrate _entity)
    {
      var p = new Vector3i();
      p.RoundToInt(_entity.position);
      int y = 0;
      int x2 = World.toBlockXZ(p.x);
      int z2 = World.toBlockXZ(p.z);

      GameManager.Instance.World.ChunkCache.ChunkProvider.RequestChunk(x2, z2);
      long chunkKey = WorldChunkCache.MakeChunkKey(x2, z2);
      var chunk = GameManager.Instance.World.GetChunkSync(chunkKey) as Chunk;

      if (chunk.FindSpawnPointAtXZ(x2, z2, out y, 15, 0, 3, 251, true))
      {
        Log.Out(Config.ModPrefix + " AirDropGroundPos:[" + ((int)_entity.position.x).ToString() + " " + y.ToString() + " " + ((int)_entity.position.z).ToString() + "]");
      }
      else
      {
        Log.Out(Config.ModPrefix + " AirDropGroundPos: No valiud spawn point found");
      }

    }
  }
}
