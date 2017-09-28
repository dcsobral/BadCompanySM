using System;
using UnityEngine;

namespace BCM.Commands
{
  public class BCSpawn : BCCommandAbstract
  {
    private static bool GetPos(out Vector3 position)
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
            break;
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
            break;
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

            var p = GameManager.Instance.World?.Players.dict[ci.entityId]?.position;
            if (p == null)
            {
              SendOutput("Unable to get player position from client entity");

              return false;
            }

            position = (Vector3)p;
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
          }
          else if (SenderInfo.RemoteClientInfo != null)
          {
            var ci = SenderInfo.RemoteClientInfo;
            if (ci == null)
            {
              SendOutput("Unable to get player position from remote client");

              return false;
            }

            var p = GameManager.Instance.World?.Players.dict[ci.entityId]?.position;
            if (p == null)
            {
              SendOutput("Unable to get player position from client entity");

              return false;
            }

            position = (Vector3)p;
          }
          break;
      }
      return true;
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

    private static bool GetCount(out int count)
    {
      count = 25;

      if (!Options.ContainsKey("count")) return true;

      if (int.TryParse(Options["count"], out count)) return true;

      SendOutput($"Unable to parse count '{Options["count"]}'");
      return false;
    }

    private static bool GetMinMax(out int min, out int max)
    {
      var valid = true;
      min = 40;
      max = 60;

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

    public override void Process()
    {
      if (Params.Count == 0)
      {
        SendOutput(GetHelp());

        return;
      }

      switch (Params[0])
      {
        case "horde":
          {
            if (!GetPos(out var position)) return;
            if (!GetGroup(out var groupName)) return;
            if (!GetCount(out var count)) return;
            if (!GetMinMax(out var min, out var max)) return;

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
                TargetPos = position,
                MinRange = min,
                MaxRange = max
              };

              EntitySpawner.SpawnQueue.Enqueue(spawn);
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
            if (!GetPos(out var position)) return;
            if (!GetMinMax(out var min, out var max)) return;

            //todo: refine method to ensure unique id
            var spawnerId = DateTime.UtcNow.Ticks;

            var classId = Params[1].GetHashCode();

            if (!EntityClass.list.ContainsKey(classId))
            {
              SendOutput($"Entity class not found '{Params[1]}'");

              return;
            }

            var spawn = new Spawn
            {
              EntityClassId = classId,
              SpawnerId = spawnerId,
              TargetPos = position,
              MinRange = min,
              MaxRange = max
            };

            EntitySpawner.SpawnQueue.Enqueue(spawn);

            return;
          }

        default:
          SendOutput($"Unknown Sub Command {Params[0]}");
          SendOutput(GetHelp());

          return;
      }

    }
  }
}
