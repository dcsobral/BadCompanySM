using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace BCM
{
  public struct BCMSpawner
  {
    public int entityClassID;
    public Vector3 pos;
    //todo: add additional fields for better spawning control
  }
  public class EntitySpawner
  {
    public static Queue<BCMSpawner> spawnQueue = new Queue<BCMSpawner>();
    public ulong lasttick = 0;

    public EntitySpawner()
    {
    }

    public void CheckSpawnQueue()
    {
      try
      {
        if (GameManager.Instance.World.worldTime > lasttick + 5L) //use to set tickrate
        {
          // todo: needs to reset if game time is changed, change to use datetime.Now
          lasttick = GameManager.Instance.World.worldTime;
          if (spawnQueue.Count > 0)
          {
            lock (spawnQueue)
            {
              for (int i = 0; spawnQueue.Count > 0; i++)
              {
                BCMSpawner spawner = spawnQueue.Dequeue();
                int x, y, z = 0;
                //todo: allow for setting via code/command settings
                //todo: add a min spawn distance
                float m = 32;
                Vector3 max = new Vector3(m, m, m);
                Vector3 pos = new Vector3(0, 0, 0);
                if (GameManager.Instance.World.FindRandomSpawnPointNearPosition(spawner.pos, 15, out x, out y, out z, max, true, true))
                {
                  pos = new Vector3(x, y, z);
                }
                else
                {
                  Log.Out(Config.ModPrefix + " Unable to find Spawn Point");
                }

                //todo: change to use EntityCreationData method
                Entity _entity = EntityFactory.CreateEntity(spawner.entityClassID, pos);
                if (_entity != null)
                {
                  //todo: log entityid for checking against spawn counts in wave etc
                  string name = "";
                  if (EntityClass.list.ContainsKey(_entity.entityClass))
                  {
                    name = EntityClass.list[_entity.entityClass].entityClassName;
                  }

                  //todo: customise entity attributes
                  //      speed, archtype? feral, scout, health, 
                  Log.Out(Config.ModPrefix + " Spawning " + _entity.entityType + ":" + name + " @" + pos);

                  GameManager.Instance.World.Entities.Add(_entity.entityId, _entity);
                  if (_entity.IsEntityAttachedToChunk)
                  {
                    if (!_entity.addedToChunk)
                    {
                      Chunk chunk = (Chunk)GameManager.Instance.World.GetChunkFromWorldPos(_entity.GetBlockPosition());
                      if (chunk != null)
                      {
                        chunk.AddEntityToChunk(_entity);
                      }
                    }
                  }
                  if (GameManager.Instance.World.audioManager != null)
                  {
                    GameManager.Instance.World.audioManager.EntityAddedToWorld(_entity, GameManager.Instance.World);
                  }
                  GameManager.Instance.World.entityDistributer.Add(_entity);
                  ((EntityAlive)_entity).Spawned = true;
                  GameManager.Instance.World.aiDirector.AddEntity(_entity);
                }

              }
            }
          }
        }
      }
      catch (Exception e)
      {
        Log.Out(Config.ModPrefix + " Error in " + GetType().Name + "." + MethodBase.GetCurrentMethod().Name + ": " + e);
      }
    }
  }
}
