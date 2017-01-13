using BCM.PersistentData;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BCM.Neurons
{
  public class SpawnManager : NeuronAbstract
  {
    BCMSettings _settings;
    Dictionary<string, int> EntityClassList = new Dictionary<string, int>();
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
          if (EntityClassList.Count == 0)
          {
            if (EntityClass.list.Count > 0)
            {
              foreach (int entityClassID in EntityClass.list.Keys)
              {
                if (EntityClass.list[entityClassID].bAllowUserInstantiate)
                {
                  EntityClassList.Add(EntityClass.list[entityClassID].entityClassName, entityClassID);
                }
              }
            }
            else
            {
              Log.Out(Config.ModPrefix + " No EntityClasses found.");
            }
          }
          // PLAYER TARGETED SPAWNING
          string smn = _neuronname + ".players";
          Dictionary<string, string> players = _settings.Get(smn);
          if (players != null)
          {
            if (players.Count > 0)
            {
              foreach (string entityid in players.Keys)
              {
                int ent = -1;
                if (int.TryParse(entityid, out ent))
                {
                  string enabled = _settings.GetValue(smn, entityid);
                  bool spawnon = false;
                  bool.TryParse(enabled, out spawnon);
                  if (spawnon == true)
                  {
                    if (GameManager.Instance.World.Players.dict.ContainsKey(ent))
                    {
                      EntityPlayer ep;
                      if (GameManager.Instance.World.Players.dict.ContainsKey(ent))
                      {
                        ep = GameManager.Instance.World.Players.dict[ent];
                      }
                      else
                      {
                        Log.Out(Config.ModPrefix + " Player not found: " + ent);
                        continue;
                      }
                      BCMSpawner spawner = new BCMSpawner();
                      string classname = "zombie01";
                      if (EntityClassList.ContainsKey(classname))
                      {
                        spawner.entityClassID = EntityClassList[classname];
                        spawner.pos = ep.position;
                        EntitySpawner.spawnQueue.Enqueue(spawner);
                      }
                      else
                      {
                        Log.Out(Config.ModPrefix + "Entity Class not found: " + classname);
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
