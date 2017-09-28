using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace BCM
{
  public class EntitySpawner
  {
    public static readonly Queue<Spawn> SpawnQueue = new Queue<Spawn>();
    public static readonly Dictionary<string, HordeSpawner> HordeSpawners = new Dictionary<string, HordeSpawner>();
    private long _lastTick = DateTime.UtcNow.Ticks;

    public void ProcessSpawnQueue()
    {
      // 10,000,000 ticks per second
      if (DateTime.UtcNow.Ticks <= _lastTick + 5000000L) return;

      _lastTick = DateTime.UtcNow.Ticks;
      if (SpawnQueue.Count <= 0) return;

      lock (SpawnQueue)
      {
        for (; SpawnQueue.Count > 0;)
        {
          //todo: max execution time limit so that too many queued spawns doesnt bog server
          try
          {
            //if obey maxspawns use below
            //if (GameStats.GetInt(EnumGameStats.EnemyCount) >= GamePrefs.GetInt(EnumGamePrefs.MaxSpawnedZombies)) return;

            var spawn = SpawnQueue.Dequeue();
            var world = GameManager.Instance.World;

            if (world.GetRandomSpawnPositionMinMaxToPosition(new Vector3(spawn.TargetPos.x, spawn.TargetPos.y, spawn.TargetPos.z), spawn.MinRange, spawn.MaxRange, false, out var pos, true))
            {
              //todo: change to use EntityCreationData method?
              var entity = EntityFactory.CreateEntity(spawn.EntityClassId, pos) as EntityEnemy;
              if (entity == null) continue;

              //_entity.lifetime

              //todo: log entityid for checking against spawn counts in wave etc
              //string name = "";
              //if (EntityClass.list.ContainsKey(_entity.entityClass))
              //{
              //  name = EntityClass.list[_entity.entityClass].entityClassName;
              //}
              lock (HordeSpawners)
              {
                //todo: increase counters and disable spawner if limits reached, alter settings if next wave etc

                spawn.EntityId = entity.entityId;
                if (HordeSpawners.ContainsKey(spawn.SpawnerId.ToString()))
                {
                  HordeSpawners[spawn.SpawnerId.ToString()]
                    .Spawns.Add(entity.entityId.ToString(), spawn);
                }
                else
                {
                  HordeSpawners.Add(spawn.SpawnerId.ToString(), new HordeSpawner
                  {
                    Spawns = new Dictionary<string, Spawn> { { entity.entityId.ToString(), spawn } }
                  });
                }
              }

              entity.bIsChunkObserver = spawn.IsObserver;
              //will make them move at night speed
              entity.isFeral = spawn.IsFeral;
              entity.speedApproach = entity.speedApproach * spawn.SpeedMul;
              if (spawn.SpeedBase > 0f)
              {
                entity.speedApproach = spawn.SpeedBase * spawn.SpeedMul;
              }
              if (spawn.NightRun)
              {
                entity.speedApproachNight = entity.speedApproachNight * spawn.SpeedMul;
              }
              else
              {
                entity.speedApproachNight = entity.speedApproach;
              }

              Log.Out($"{Config.ModPrefix} Spawning {entity.entityType}({entity.entityId}):{entity.EntityName} @{pos} targeting: {spawn.TargetPos}");
              world.Entities.Add(entity.entityId, entity);

              if (entity.IsEntityAttachedToChunk && !entity.addedToChunk)
              {
                //todo: need to generate chunk and delay spawn if chunk isnt loaded?
                var chunk = world.GetChunkFromWorldPos(entity.GetBlockPosition()) as Chunk;
                chunk?.AddEntityToChunk(entity);
              }
              world.audioManager?.EntityAddedToWorld(entity, world);
              world.entityDistributer.Add(entity);
              entity.Spawned = true;
              world.aiDirector.AddEntity(entity);
              entity.SetInvestigatePosition(new Vector3(spawn.TargetPos.x, spawn.TargetPos.y, spawn.TargetPos.z), 6000);
            }
            else
            {
              Log.Out(
                $"{Config.ModPrefix} Unable to find Spawn Point near {spawn.TargetPos}, min:{spawn.MinRange}, max:{spawn.MaxRange}");
            }

          }
          catch (Exception e)
          {
            Log.Out($"{Config.ModPrefix} Error in {GetType().Name}.{MethodBase.GetCurrentMethod().Name}: {e}");
          }
        }
      }
    }
  }
}
