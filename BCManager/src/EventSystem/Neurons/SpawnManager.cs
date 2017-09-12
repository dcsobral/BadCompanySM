using BCM.PersistentData;
using System.Collections.Generic;
using static System.Boolean;
using static System.Int32;
using static System.Single;

namespace BCM.Neurons
{
  public class SpawnManager : NeuronAbstract
  {
    private BCMSettings _settings;
    private const string Function = "SpawnManager";
    private const string EntitiesCollection = "Entities";
    private const string PlayersCollection = "Players";

    public SpawnManager()
    {
    }

    private static void SpawnForEntity (int targetEntityId, IDictionary<string, string> settings)
    {
      // todo: delay between spawns based on command settings

      var entities = GameManager.Instance.World.Entities.dict;
      if (!entities.ContainsKey(targetEntityId))
      {
        Log.Out($"{Config.ModPrefix} Player not found: {targetEntityId}");

        return;
      }

      var targetEntity = entities[targetEntityId];
      if (targetEntity == null)
      {
        Log.Out($"{Config.ModPrefix}Target entity was not found in world");

        return;
      }

      //if player is dead turn off spawner if set with end_on_death flag (default == true)
      if (targetEntity.IsDead() && (!settings.ContainsKey("end_on_death") || settings["end_on_death"] == "true"))
      {
        settings["enabled"] = "false";

        return;
      }

      if (!settings.TryGetValue("group", out string groupName))
      {
        groupName = "ZombiesAll";
      }

      if (!EntityGroups.list.ContainsKey(groupName))
      {
        Log.Out($"{Config.ModPrefix}Entity group not found {groupName}");

        return;
      }

      var classId = EntityGroups.GetRandomFromGroup(groupName);
      if (!EntityClass.list.ContainsKey(classId))
      {
        Log.Out($"{Config.ModPrefix}Entity class not found {classId}");

        return;
      }

      EntitySpawner.SpawnQueue.Enqueue(GetSpawnForTarget(settings, targetEntity, classId));
    }

    private static Spawn GetSpawnForTarget(IDictionary<string, string> settings, Entity target, int classId)
    {
      var spawn = new Spawn
      {
        EntityId = 0,
        SpawnerId = target.entityId,
        TargetId = target.entityId,
        Pos = target.position,
        MinRange = 40,
        MaxRange = 60,
        IsObserver = true,
        IsFeral = false,
        NightRun = false,
        SpeedBase = 0f,
        EntityClassId = classId
      };

      if (settings.ContainsKey("minrange"))
      {
        TryParse(settings["minrange"], out spawn.MinRange);
      }

      if (settings.ContainsKey("maxrange"))
      {
        TryParse(settings["maxrange"], out spawn.MaxRange);
      }

      if (settings.ContainsKey("observe"))
      {
        TryParse(settings["observe"], out spawn.IsObserver);
      }

      if (settings.ContainsKey("feral"))
      {
        TryParse(settings["feral"], out spawn.IsFeral);
      }

      if (settings.ContainsKey("nightrun"))
      {
        TryParse(settings["nightrun"], out spawn.NightRun);
      }

      if (settings.ContainsKey("speedbase"))
      {
        TryParse(settings["speedbase"], out spawn.SpeedBase);
      }

      var speedmin = 0f;
      if (settings.ContainsKey("speedmin"))
      {
        TryParse(settings["speedmin"], out speedmin);
      }
      var speedmax = 0f;
      if (settings.ContainsKey("speedmax"))
      {
        TryParse(settings["speedmax"], out speedmax);
      }
      if (speedmin == 0f)
      {
        speedmin = 1f;
      }
      if (speedmax < speedmin)
      {
        speedmax = speedmin;
      }
      spawn.SpeedMul = UnityEngine.Random.Range(speedmin, speedmax);

      return spawn;
    }

    public override void Fire(int b)
    {
      if (!API.IsAlive || !PersistentContainer.IsLoaded) return;

      _settings = PersistentContainer.Instance.Settings;
      var world = GameManager.Instance.World;

      var onlineplayercount = world?.Players.Count;
      if (!(onlineplayercount > 0)) return;

      // ENTITY TARGETED SPAWNING
      foreach (var entityId in world.Entities.dict.Keys)
      {
        var settings = _settings.GetFunction(EntitiesCollection, entityId.ToString(), Function);
        if (settings?.ContainsKey("enabled") != true || settings["enabled"] != "true") continue;

        SpawnForEntity(entityId, settings);
      }

      // PLAYER TARGETED SPAWNING
      var clients = ConnectionManager.Instance.GetClients();
      foreach (var client in clients)
      {
        var settings = _settings.GetFunction(PlayersCollection, client.playerId, Function);
        if (settings?.ContainsKey("enabled") != true || settings["enabled"] != "true") continue;

        SpawnForEntity(client.entityId, settings);
      }
          
      //AREA SPAWNERS
      // todo:
    }
  }
}
