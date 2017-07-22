using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace BCM
{
  public class EntitySpawner
  {
    public static Queue<Spawn> spawnQueue = new Queue<Spawn>();
    public static Dictionary<string, HordeSpawner> hordeSpawners = new Dictionary<string, HordeSpawner>();
    public long lasttick = DateTime.Now.Ticks;

    public EntitySpawner()
    {
    }

    public void ProcessSpawnQueue()
    {
      // 10,000,000 ticks per second
      if (DateTime.Now.Ticks > lasttick + 5000000L) //2 per second
      {
        lasttick = DateTime.Now.Ticks;
        if (spawnQueue.Count > 0)
        {
          lock (spawnQueue)
          {
            for (int i = 0; spawnQueue.Count > 0; i++)
            {
              //todo: max execution time limit so that too many queued spawns doesnt bog server
              try
              {
                //if obey maxspawns use below
                //if (GameStats.GetInt(EnumGameStats.EnemyCount) < GamePrefs.GetInt(EnumGamePrefs.MaxSpawnedZombies))
                Spawn spawn = spawnQueue.Dequeue();

                Vector3 pos = new Vector3(0, 0, 0);
                if (GameManager.Instance.World.GetRandomSpawnPositionMinMaxToPosition(new Vector3(spawn.pos.x, spawn.pos.y, spawn.pos.z), spawn.minRange, spawn.maxRange, false, out pos, true))
                {

                  //todo: change to use EntityCreationData method?
                  EntityEnemy _entity = EntityFactory.CreateEntity(spawn.entityClassId, pos) as EntityEnemy;
                  if (_entity != null)
                  {

                    //_entity.lifetime

                    //todo: log entityid for checking against spawn counts in wave etc
                    //string name = "";
                    //if (EntityClass.list.ContainsKey(_entity.entityClass))
                    //{
                    //  name = EntityClass.list[_entity.entityClass].entityClassName;
                    //}
                    lock (hordeSpawners)
                    {
                      //todo: increase counters and disable spawner if limits reached, alter settings if next wave etc

                      spawn.entityId = _entity.entityId;
                      if (hordeSpawners.ContainsKey(spawn.spawnerId.ToString()))
                      {
                        hordeSpawners[spawn.spawnerId.ToString()].spawns.Add(_entity.entityId.ToString(), spawn);
                      }
                      else
                      {
                        HordeSpawner hs = new HordeSpawner();
                        hs.spawns = new Dictionary<string, Spawn>();
                        hs.spawns.Add(_entity.entityId.ToString(), spawn);
                        hordeSpawners.Add(spawn.spawnerId.ToString(), hs);
                      }
                    }

                    _entity.bIsChunkObserver = spawn.isObserver;
                    //will make them move at night speed
                    _entity.isFeral = spawn.isFeral;
                    _entity.speedApproach = _entity.speedApproach * spawn.speedMul;
                    if (spawn.speedBase != 0)
                    {
                      _entity.speedApproach = spawn.speedBase * spawn.speedMul;
                    }
                    if (spawn.nightRun)
                    {
                      _entity.speedApproachNight = _entity.speedApproachNight * spawn.speedMul;
                    }
                    else
                    {
                      _entity.speedApproachNight = _entity.speedApproach;
                    }
                    Log.Out(Config.ModPrefix + " Spawning " + _entity.entityType + "(" + _entity.entityId + "):" + _entity.EntityName + " @" + pos + " targeting: " + spawn.pos);
                    GameManager.Instance.World.Entities.Add(_entity.entityId, _entity);

                    if (_entity.IsEntityAttachedToChunk)
                    {
                      if (!_entity.addedToChunk)
                      {
                        //todo: need to generate chunk and delay spawn if chunk isnt loaded?
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
                    _entity.Spawned = true;
                    GameManager.Instance.World.aiDirector.AddEntity(_entity);
                    _entity.SetInvestigatePosition(new Vector3(spawn.pos.x, spawn.pos.y, spawn.pos.z), 6000);
                  }
                }
                else
                {
                  Log.Out(Config.ModPrefix + " Unable to find Spawn Point near " + spawn.pos + ", min:" + spawn.minRange + ", max:" + spawn.maxRange);
                }

              }
              catch (Exception e)
              {
                Log.Out(Config.ModPrefix + " Error in " + GetType().Name + "." + MethodBase.GetCurrentMethod().Name + ": " + e);
              }
            }
          }
        }
      }
    }
  }
}
