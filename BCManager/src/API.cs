using BCM.PersistentData;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace BCM
{
  public struct BCMSpawner
  {
    public int entityClassID;
    public Vector3 pos;
  }
  public class API : ModApiAbstract
  {
    public static Queue<BCMSpawner> SpawnQueue = new Queue<BCMSpawner>();
    public ulong lasttick = 0;
    public bool IsAlive = false;

    public API()
    {
      Config.Init();
      Heartbeat.Start();
    }

    public override void GameUpdate()
    {
      if (IsAlive)
      {
        try
        {
          if (GameManager.Instance.World.worldTime > lasttick + 5L) //use to set tickrate
          {
            // todo: needs to reset if game time is changed, change to use datetime.Now
            lasttick = GameManager.Instance.World.worldTime;


            //*********** move to heartbeat *******************//
            if (SpawnQueue.Count > 0)
            {
              Log.Out(Config.ModPrefix + " GAMEUPDATE " + SpawnQueue.Count);
              lock (SpawnQueue)
              {
                Log.Out(Config.ModPrefix + " LOCKED ");

                for (int i = 0; SpawnQueue.Count > 0; i++)
                {
                  BCMSpawner spawner = SpawnQueue.Dequeue();
                  int x, y, z = 0;
                  float m = 32;
                  Vector3 max = new Vector3(m, m, m);
                  Vector3 pos = new Vector3(0, 0, 0);
                  if (GameManager.Instance.World.FindRandomSpawnPointNearPosition(spawner.pos, 15, out x, out y, out z, max, true, true))
                  {
                    pos = new Vector3(x, y, z);
                    Log.Out(Config.ModPrefix + " Spawn Point " + pos);
                  }
                  else
                  {
                    Log.Out(Config.ModPrefix + " Unable to find Spawn Point");
                  }

                  Entity _entity = EntityFactory.CreateEntity(spawner.entityClassID, pos);//ep.position

                  if (_entity != null)
                  {
                    string name = "";
                    if (EntityClass.list.ContainsKey(_entity.entityClass))
                    {
                      name = EntityClass.list[_entity.entityClass].entityClassName;
                    }
                    Log.Out(Config.ModPrefix + " Spawning " + _entity.entityType + ":" + name + " @" + _entity.position);

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
            //*********** move to heartbeat *******************//

          }
        }
        catch
        {
          Log.Out(Config.ModPrefix + " Error in API.Update");
        }
      }
    }

    public override void GameAwake()
    {
      StateManager.Awake();
      IsAlive = true;
    }

    public override void GameShutdown()
    {
      StateManager.Shutdown();
    }

    public override void SavePlayerData(ClientInfo _cInfo, PlayerDataFile _playerDataFile)
    {
      DataManager.SavePlayerData(_cInfo, _playerDataFile);
    }

    public override void PlayerLogin(ClientInfo _cInfo, string _compatibilityVersion)
    {
      //_cInfo.SendPackage(new NetPackageConsoleCmdClient("dm", true));
    }

    public override void PlayerSpawning(ClientInfo _cInfo, int _chunkViewDim, PlayerProfile _playerProfile)
    {
      try
      {
        PersistentContainer.Instance.Players[_cInfo.playerId, true].SetOnline(_cInfo);
        PersistentContainer.Instance.Save();
      }
      catch (Exception e)
      {
        Log.Out("" + Config.ModPrefix + " Error in " + GetType().Name + "." + MethodBase.GetCurrentMethod().Name + ": " + e);
      }
    }

    public override void PlayerDisconnected(ClientInfo _cInfo, bool _bShutdown)
    {
      try
      {
        Player p = PersistentContainer.Instance.Players[_cInfo.playerId, true];
        if (p != null)
        {
          p.SetOffline();
        }
        else
        {
          //Log.Out("" + Config.ModPrefix + " Disconnected player not found in client list...");
        }
        PersistentContainer.Instance.Save();
      }
      catch (Exception e)
      {
        Log.Out("" + Config.ModPrefix + " Error in " + GetType().Name + "." + MethodBase.GetCurrentMethod().Name + ": " + e);
      }
    }

    //public override bool ChatMessage(ClientInfo _cInfo, EnumGameMessages _type, string _msg, string _mainName, bool _localizeMain, string _secondaryName, bool _localizeSecondary)
    //{
    //  return ChatHookExample.Hook(_cInfo, _type, _msg, _mainName);
    //}

    //public override void CalcChunkColorsDone(Chunk _chunk) {
    //}

    //public override void GameStartDone() {
    //}


  }
}
