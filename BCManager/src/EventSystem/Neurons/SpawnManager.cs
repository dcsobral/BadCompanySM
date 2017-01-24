using BCM.PersistentData;
using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace BCM.Neurons
{
  public class SpawnManager : NeuronAbstract
  {
    BCMSettings _settings;
    //Dictionary<string, int> EntityClassList = new Dictionary<string, int>();
    string _neuronname = "SpawnManager";

    public SpawnManager()
    {
    }
    public override bool Fire(int b)
    {
      if (PersistentContainer.IsLoaded)
      {
        _settings = PersistentContainer.Instance.Settings;
        int onlineplayercount = 0;
        try
        {
          onlineplayercount = GameManager.Instance.World.Players.Count;
        }
        catch
        {
          return false;
        }
        if (onlineplayercount > 0)
        {
          //if (EntityClassList.Count == 0)
          //{
          //  if (EntityClass.list.Count > 0)
          //  {
          //    foreach (int entityClassID in EntityClass.list.Keys)
          //    {
          //      if (EntityClass.list[entityClassID].bAllowUserInstantiate)
          //      {
          //        EntityClassList.Add(EntityClass.list[entityClassID].entityClassName, entityClassID);
          //      }
          //    }
          //  }
          //  else
          //  {
          //    Log.Out(Config.ModPrefix + " No EntityClasses found.");
          //  }
          //}


          // PLAYER TARGETED SPAWNING
          string settingnode = _neuronname + ".players";
          //todo: flip the foreach to check through online players instead of configured settings
          Dictionary<string, string> players = _settings.Get(settingnode);
          if (players != null)
          {
            if (players.Count > 0)
            {
              foreach (string entityid in players.Keys)
              {
                int ent = -1;
                if (int.TryParse(entityid, out ent))
                {
                  string jsondata = _settings.GetValue(settingnode, entityid);
                  bool spawnon = false;
                  Dictionary<string, string> playerdata = JsonUtility.FromJson<Dictionary<string, string>>(jsondata);
                  if (playerdata.ContainsKey("enabled"))
                  {
                    bool.TryParse(playerdata["enabled"], out spawnon);
                    if (spawnon == true)
                    {
                      EntityPlayer ep;
                      if (GameManager.Instance.World.Players.dict.ContainsKey(ent))
                      {
                        ep = GameManager.Instance.World.Players.dict[ent];

                        //todo: if player is dead turn off spawner if set with endondeath flag

                        Spawn spawn = new Spawn();

                        // todo: type or group defined in the command settings
                        // todo: delay between spawns based on command settings

                        //string entitygroupname = "ZombieScoutsEasy";
                        string groupname = "ZombiesAll";
                        if (playerdata.ContainsKey("group"))
                        {
                          groupname = playerdata["group"];
                        }
                        if (EntityGroups.list.ContainsKey(groupname))
                        {
                          spawn.entityClassId = EntityGroups.GetRandomFromGroup(groupname);
                        }
                        if (EntityClass.list.ContainsKey(spawn.entityClassId))
                        {
                          spawn.entityId = 0;
                          //player hordes are assigned to entityid as spawner id
                          spawn.spawnerId = ep.entityId;
                          spawn.targetId = ep.entityId;
                          spawn.pos = ep.position;

                          spawn.minRange = 40;
                          if (playerdata.ContainsKey("minrange"))
                          {
                            int.TryParse(playerdata["minrange"], out spawn.minRange);
                          }

                          spawn.maxRange = 60;
                          if (playerdata.ContainsKey("maxrange"))
                          {
                            int.TryParse(playerdata["maxrange"], out spawn.maxRange);
                          }

                          spawn.bIsChunkObserver = true;
                          if (playerdata.ContainsKey("observe"))
                          {
                            bool.TryParse(playerdata["observe"], out spawn.bIsChunkObserver);
                          }

                          spawn.isFeral = false;
                          if (playerdata.ContainsKey("feral"))
                          {
                            bool.TryParse(playerdata["feral"], out spawn.isFeral);
                          }

                          spawn.nightRun = false;
                          if (playerdata.ContainsKey("nightrun"))
                          {
                            bool.TryParse(playerdata["nightrun"], out spawn.nightRun);
                          }

                          spawn.speedBase = 0;
                          if (playerdata.ContainsKey("speedbase"))
                          {
                            float.TryParse(playerdata["speedbase"], out spawn.speedBase);
                          }

                          float speedmin = 0;
                          if (playerdata.ContainsKey("speedmin"))
                          {
                            float.TryParse(playerdata["speedmin"], out speedmin);
                          }
                          float speedmax = 0;
                          if (playerdata.ContainsKey("speedmax"))
                          {
                            float.TryParse(playerdata["speedmax"], out speedmax);
                          }
                          if (speedmin == 0)
                          {
                            speedmin = 1;
                          }
                          if (speedmax < speedmin)
                          {
                            speedmax = speedmin;
                          }
                          spawn.speedMul = UnityEngine.Random.Range(speedmin, speedmax);

                          EntitySpawner.spawnQueue.Enqueue(spawn);
                        }
                        else
                        {
                          Log.Out(Config.ModPrefix + "Entity Class not found " + spawn.entityClassId);
                        }
                      }
                      else
                      {
                        Log.Out(Config.ModPrefix + " Player not found: " + ent);
                      }
                    }
                  }
                }
              }
            }
          }

          //AREA SPAWNERS
          // todo:


          return true;
        }
      }
      return false;
    }
  }
}
