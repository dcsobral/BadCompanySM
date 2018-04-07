using System;
using JetBrains.Annotations;
using UnityEngine;

namespace BCM.Commands
{
  [UsedImplicitly]
  public class BCSpawn : BCCommandAbstract
  {
    protected override void Process()
    {
      if (!BCUtils.CheckWorld(out var world)) return;

      if (Params.Count == 0)
      {
        SendOutput(GetHelp());

        return;
      }

      switch (Params[0])
      {
        case "horde":
          {
            if (!GetPos(world, out var position)) return;
            if (!GetGroup(out var groupName)) return;
            if (!GetCount(out var count)) return;
            if (!GetMinMax(out var min, out var max)) return;
            if (!GetSpawnPos(position, out var targetPos)) return;

            //todo: refine method to ensure unique id
            var spawnerId = DateTime.UtcNow.Ticks;

            for (var i = 0; i < count; i++)
            {
              var classId = EntityGroups.GetRandomFromGroup(groupName);
              if (!EntityClass.list.ContainsKey(classId))
              {
                SendOutput($"Entity class not found '{classId}', from group '{groupName}'");

                return;
              }

              var spawn = new Spawn
              {
                EntityClassId = classId,
                SpawnerId = spawnerId,
                SpawnPos = position,
                TargetPos = targetPos,
                MinRange = min,
                MaxRange = max
              };

              EntitySpawner.SpawnQueue.Enqueue(spawn);
            }
            SendOutput($"Spawning horde of {count} around {position.x} {position.y} {position.z}");
            if (targetPos != position)
            {
              SendOutput($"Moving towards {targetPos.x} {targetPos.y} {targetPos.z}");
            }

            return;
          }

        case "entity":
          {
            if (Params.Count != 2 && Params.Count != 5)
            {
              SendOutput("Spawn entity requires an entity class name");

              return;
            }
            if (!GetPos(world, out var position)) return;
            if (!GetMinMax(out var min, out var max)) return;
            if (!GetSpawnPos(position, out var targetPos)) return;

            //todo: refine method to ensure unique id
            var spawnerId = DateTime.UtcNow.Ticks;

            var classId = Params[1].GetHashCode();

            if (!EntityClass.list.ContainsKey(classId))
            {
              SendOutput($"Entity class not found '{Params[1]}'");

              return;
            }


            EntitySpawner.SpawnQueue.Enqueue(new Spawn
            {
              EntityClassId = classId,
              SpawnerId = spawnerId,
              SpawnPos = position,
              TargetPos = position,
              MinRange = min,
              MaxRange = max
            });

            SendOutput($"Spawning entity {Params[1]} around {position.x} {position.y} {position.z}");
            if (targetPos != position)
            {
              SendOutput($"Moving towards {targetPos.x} {targetPos.y} {targetPos.z}");
            }

            return;
          }

        case "item":
          {
            if (!GetPos(world, out var position)) return;
            if (!GetItemValue(out var item)) return;
            if (!GetCount(out var count, 1)) return;
            if (!GetMinMax(out var min, out var max, "qual")) return;

            if (item == null || item.type == ItemValue.None.type)
            {
              SendOutput("Item class not found'");

              return;
            }

            var itemValue = new ItemValue(item.type, true);

            var quality = UnityEngine.Random.Range(min, max);
            if (ItemClass.list[itemValue.type].HasParts)
            {
              count = 1;
              for (var i = 0; i < itemValue.Parts.Length; i++)
              {
                var item2 = itemValue.Parts[i];
                item2.Quality = quality;
                itemValue.Parts[i] = item2;
              }
            }
            else if (itemValue.HasQuality)
            {
              count = 1;
              itemValue.Quality = quality;
            }

            var itemStack = new ItemStack(itemValue, count);
            GameManager.Instance.ItemDropServer(itemStack, position, Vector3.zero);

            SendOutput($"Spawning {count}x {itemValue.ItemClass.Name} at {position.x} {position.y} {position.z}");

            return;
          }

        default:
          SendOutput($"Unknown Sub Command {Params[0]}");
          SendOutput(GetHelp());

          return;
      }
    }

    private static bool GetPos(World world, out Vector3 position)
    {
      position = new Vector3(0, 0, 0);

      switch (Params.Count)
      {
        case 5:
          {
            if (!int.TryParse(Params[2], out var x) || !int.TryParse(Params[3], out var y) ||
                !int.TryParse(Params[4], out var z))
            {
              SendOutput("Unable to parse x y z for numbers");

              return false;
            }
            position = new Vector3(x, y, z);
            return true;
          }
        case 4:
          {
            if (!int.TryParse(Params[1], out var x) || !int.TryParse(Params[2], out var y) ||
                !int.TryParse(Params[3], out var z))
            {
              SendOutput("Unable to parse x y z for numbers");

              return false;
            }
            position = new Vector3(x, y, z);
            return true;
          }
        default:
          if (Options.ContainsKey("player"))
          {
            ConsoleHelper.ParseParamPartialNameOrId(Options["player"], out string _, out var ci);
            if (ci == null)
            {
              SendOutput("Unable to get player position from remote client");

              return false;
            }

            var p = world.Players.dict[ci.entityId]?.position;
            if (p == null)
            {
              SendOutput("Unable to get player position from client entity");

              return false;
            }

            position = (Vector3)p;
            return true;
          }
          else if (Options.ContainsKey("p"))
          {
            var pos = Options["p"].Split(',');
            if (pos.Length != 3)
            {
              SendOutput($"Unable to get position from {Options["p"]}, incorrect number of co-ords: /p=x,y,z");

              return false;
            }
            if (!int.TryParse(pos[0], out var x) ||
                !int.TryParse(pos[1], out var y) ||
                !int.TryParse(pos[2], out var z)
            )
            {
              SendOutput($"Unable to get x y z for {Options["p"]}");

              return false;
            }
            position = new Vector3(x, y, z);
            return true;
          }
          else if (Options.ContainsKey("position"))
          {
            var pos = Options["position"].Split(',');
            if (pos.Length != 3)
            {
              SendOutput($"Unable to get position from '{Options["position"]}', incorrect number of co-ords: /position=x,y,z");

              return false;
            }
            if (!int.TryParse(pos[0], out var x) ||
                !int.TryParse(pos[1], out var y) ||
                !int.TryParse(pos[2], out var z)
            )
            {
              SendOutput($"Unable to get x y z from '{Options["position"]}'");

              return false;
            }

            position = new Vector3(x, y, z);
            return true;
          }
          else if (SenderInfo.RemoteClientInfo != null)
          {
            var ci = SenderInfo.RemoteClientInfo;
            if (ci == null)
            {
              SendOutput("Unable to get player position from remote client");

              return false;
            }

            var p = world.Players.dict[ci.entityId]?.position;
            if (p == null)
            {
              SendOutput("Unable to get player position from client entity");

              return false;
            }

            position = (Vector3)p;
            return true;
          }
          return false;
      }
    }

    private static bool GetGroup(out string groupName)
    {
      groupName = "ZombiesAll";
      if (Options.ContainsKey("group"))
      {
        groupName = Options["group"];
      }

      if (EntityGroups.list.ContainsKey(groupName)) return true;

      SendOutput($"Entity group not found '{groupName}'");
      return false;
    }

    //todo: byNameOrId
    private static bool GetItemValue(out ItemValue item)
    {
      var name = "";
      item = null;
      if (Options.ContainsKey("name"))
      {
        name = Options["name"];
        item = ItemClass.GetItem(name, true);

        if (ItemClass.list[item.type] != null) return true;
      }

      SendOutput($"ItemClass not found '{name}'");
      return false;
    }

    private static bool GetCount(out int count, int c = 25)
    {
      count = c;

      if (!Options.ContainsKey("count")) return true;

      if (int.TryParse(Options["count"], out count)) return true;

      SendOutput($"Unable to parse count '{Options["count"]}'");
      return false;
    }

    private static bool GetMinMax(out int min, out int max, string mode = "")
    {
      var valid = true;
      min = mode == "qual" ? 600 : 40;
      max = mode == "qual" ? 600 : 60;

      if (!Options.ContainsKey("min") && !Options.ContainsKey("max")) return true;

      if (Options.ContainsKey("min") && !int.TryParse(Options["min"], out min))
      {
        SendOutput($"Unable to parse min '{Options["min"]}'");
        valid = false;
      }

      if (Options.ContainsKey("max") && !int.TryParse(Options["max"], out max))
      {
        SendOutput($"Unable to parse max '{Options["max"]}'");
        valid = false;
      }

      return valid;
    }

    private static bool GetSpawnPos(Vector3 position, out Vector3 targetPos)
    {
      targetPos = position;

      if (Options.ContainsKey("target") || Options.ContainsKey("t"))
      {
        var pos = (Options.ContainsKey("t") ? Options["t"] : Options["target"]).Split(',');
        if (pos.Length == 3)
        {
          if (!int.TryParse(pos[0], out var x) ||
              !int.TryParse(pos[1], out var y) ||
              !int.TryParse(pos[2], out var z))
          {
            SendOutput("Unable to parse target");

            return false;
          }
          targetPos = new Vector3(x, y, z);
        }
      }
      else if (Options.ContainsKey("vector") || Options.ContainsKey("v"))
      {
        var sv = (Options.ContainsKey("v") ? Options["v"] : Options["vector"]).Split(',');
        if (sv.Length == 2)
        {
          if (!int.TryParse(sv[0], out var dist) ||
              !int.TryParse(sv[1], out var angle))
          {
            SendOutput("Unable to parse spawnvector");

            return false;
          }

          var x = Math.Sin(angle * Math.PI / 180) * dist;
          var z = Math.Cos(angle * Math.PI / 180) * dist;
          targetPos = new Vector3(position.x + Mathf.Round((float)x), position.y, position.z + Mathf.Round((float)z));
        }
      }

      return true;
    }
  }
}
