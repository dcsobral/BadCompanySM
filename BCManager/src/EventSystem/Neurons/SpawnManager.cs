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

    private void spawnForPlayer (int playerEntityId, Dictionary<string,string> _spawnerSettings)
    {
      EntityPlayer ep;
      if (GameManager.Instance.World.Players.dict.ContainsKey(playerEntityId))
      {
        ep = GameManager.Instance.World.Players.dict[playerEntityId];

        //EntitySpawner.hordeSpawners
        //if (x[ep.entityId])
        //{
        //  //already 
        //  return;
        //}

        //if player is dead turn off spawner if set with end_on_death flag (default == true)
        if (ep.IsDead())
        {
          if (!_spawnerSettings.ContainsKey("end_on_death") || _spawnerSettings["end_on_death"] == "true")
          {
            _spawnerSettings["enabled"] = "false";
            return;
          }
        }

        Spawn spawn = new Spawn();

        // todo: type or group defined in the command settings
        // todo: delay between spawns based on command settings



        string _groupname = "ZombiesAll";
        _spawnerSettings.TryGetValue("group", out _groupname);

        if (EntityGroups.list.ContainsKey(_groupname))
        {
          spawn.entityClassId = EntityGroups.GetRandomFromGroup(_groupname);
        }


        if (EntityClass.list.ContainsKey(spawn.entityClassId))
        {
          spawn.entityId = 0;
          //player hordes are assigned to entityid as spawner id
          spawn.spawnerId = ep.entityId;
          spawn.targetId = ep.entityId;
          spawn.pos = ep.position;

          spawn.minRange = 40;
          if (_spawnerSettings.ContainsKey("minrange"))
          {
            int.TryParse(_spawnerSettings["minrange"], out spawn.minRange);
          }

          spawn.maxRange = 60;
          if (_spawnerSettings.ContainsKey("maxrange"))
          {
            int.TryParse(_spawnerSettings["maxrange"], out spawn.maxRange);
          }

          spawn.bIsChunkObserver = true;
          if (_spawnerSettings.ContainsKey("observe"))
          {
            bool.TryParse(_spawnerSettings["observe"], out spawn.bIsChunkObserver);
          }

          spawn.isFeral = false;
          if (_spawnerSettings.ContainsKey("feral"))
          {
            bool.TryParse(_spawnerSettings["feral"], out spawn.isFeral);
          }

          spawn.nightRun = false;
          if (_spawnerSettings.ContainsKey("nightrun"))
          {
            bool.TryParse(_spawnerSettings["nightrun"], out spawn.nightRun);
          }

          spawn.speedBase = 0;
          if (_spawnerSettings.ContainsKey("speedbase"))
          {
            float.TryParse(_spawnerSettings["speedbase"], out spawn.speedBase);
          }

          float speedmin = 0;
          if (_spawnerSettings.ContainsKey("speedmin"))
          {
            float.TryParse(_spawnerSettings["speedmin"], out speedmin);
          }
          float speedmax = 0;
          if (_spawnerSettings.ContainsKey("speedmax"))
          {
            float.TryParse(_spawnerSettings["speedmax"], out speedmax);
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
        Log.Out(Config.ModPrefix + " Player not found: " + playerEntityId);
      }
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

          //ALL ENTITES
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
          string _collectionName = "Players";
          string _functionName = "SpawnManager";

//          var _players = GameManager.Instance.World.GetPlayers();

          var _clients = ConnectionManager.Instance.GetClients();

          foreach (ClientInfo _client in _clients)
          {
            var _spawnerSettings = _settings.GetFunction(_collectionName, _client.playerId, _functionName);
            if (_spawnerSettings != null && _spawnerSettings.ContainsKey("enabled") && _spawnerSettings["enabled"] == "true")
            {

              //EntitySpawner.hordeSpawners[spawn.spawnerId]

              spawnForPlayer(_client.entityId, _spawnerSettings);
            }
          }


          //Dictionary<string, string> players = null;
          ////Dictionary<string, string> players = _settings.GetCollection(settingnode);
          //if (players != null)
          //{
          //  if (players.Count > 0)
          //  {

          //    foreach (string entityid in players.Keys)
          //    {
          //      int ent = -1;
          //      if (int.TryParse(entityid, out ent))
          //      {
          //        string jsondata = "";// _settings.GetValue(settingnode, entityid);
          //        bool spawnon = false;
          //        Dictionary<string, string> playerdata = JsonUtility.FromJson<Dictionary<string, string>>(jsondata);
          //        if (playerdata.ContainsKey("enabled"))
          //        {
          //          bool.TryParse(playerdata["enabled"], out spawnon);
          //          if (spawnon == true)
          //          {
          //            spawnForPlayer(ent, playerdata);
          //          }
          //        }
          //      }
          //    }

          //  }
          //}

          //AREA SPAWNERS
          // todo:


          return true;
        }
      }
      return false;
    }
  }
}
