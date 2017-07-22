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
    string _function = "SpawnManager";
    string _entitiesCollection = "Entities";
    string _playersCollection = "Players";

    public SpawnManager()
    {
    }

    private void SpawnForEntity (int targetEntityId, Dictionary<string,string> _spawnerSettings)
    {
      Entity targetEntity;
      if (GameManager.Instance.World.Entities.dict.ContainsKey(targetEntityId))
      {
        targetEntity = GameManager.Instance.World.Entities.dict[targetEntityId];

        if (targetEntity == null)
        {
          Log.Out("Target entity was not found in world");

          return;
        }

        //if player is dead turn off spawner if set with end_on_death flag (default == true)
        if (targetEntity.IsDead())
        {
          if (!_spawnerSettings.ContainsKey("end_on_death") || _spawnerSettings["end_on_death"] == "true")
          {
            _spawnerSettings["enabled"] = "false";

            return;
          }
        }

        Spawn spawn = new Spawn();
        spawn.entityId = 0;
        spawn.spawnerId = targetEntity.entityId;
        spawn.targetId = targetEntity.entityId;
        spawn.pos.x = targetEntity.position.x;
        spawn.pos.y = targetEntity.position.y;
        spawn.pos.z = targetEntity.position.z;

        // todo: delay between spawns based on command settings

        string _groupname = null;
        _spawnerSettings.TryGetValue("group", out _groupname);

        if (_groupname == null)
        {
          _groupname = "ZombiesAll";
        }

        if (EntityGroups.list.ContainsKey(_groupname))
        {
          spawn.entityClassId = EntityGroups.GetRandomFromGroup(_groupname);
        }

        if (EntityClass.list.ContainsKey(spawn.entityClassId))
        {
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

          spawn.isObserver = true;
          if (_spawnerSettings.ContainsKey("observe"))
          {
            bool.TryParse(_spawnerSettings["observe"], out spawn.isObserver);
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
        Log.Out(Config.ModPrefix + " Player not found: " + targetEntityId);
      }
    }
    public override bool Fire(int b)
    {
      if (API.IsAlive && PersistentContainer.IsLoaded)
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
          // ENTITY TARGETED SPAWNING
          foreach (int _entityId in GameManager.Instance.World.Entities.dict.Keys)
          {
            var _spawnerSettings = _settings.GetFunction(_entitiesCollection, _entityId.ToString(), _function);
            if (_spawnerSettings != null && _spawnerSettings.ContainsKey("enabled") && _spawnerSettings["enabled"] == "true")
            {
              SpawnForEntity(_entityId, _spawnerSettings);
            }
          }

          // PLAYER TARGETED SPAWNING
          var _clients = ConnectionManager.Instance.GetClients();
          foreach (ClientInfo _client in _clients)
          {
            var _spawnerSettings = _settings.GetFunction(_playersCollection, _client.playerId, _function);
            if (_spawnerSettings != null && _spawnerSettings.ContainsKey("enabled") && _spawnerSettings["enabled"] == "true")
            {
              SpawnForEntity(_client.entityId, _spawnerSettings);
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
