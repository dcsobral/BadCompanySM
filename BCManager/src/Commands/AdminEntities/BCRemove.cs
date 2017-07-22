using System;
using System.Collections.Generic;

namespace BCM.Commands
{
  public class BCRemove : BCCommandAbstract
  {
    public override void Process()
    {
      if (_params.Count != 1)
      {
        if (_options.ContainsKey("all") || _options.ContainsKey("type"))
        {
          foreach(var _e in GameManager.Instance.World.Entities.dict)
          {
            if (_options.ContainsKey("all"))
            {
              RemoveEntity(_e.Key);
            }
            else
            {
              if (_e.Value.entityType.ToString() == _options["type"])
              {
                RemoveEntity(_e.Key);
              }
            }
          }
        }
        else
        {
          SendOutput(GetHelp());
        }
      }
      else
      {
        int entityId = -1;
        if (int.TryParse(_params[0], out entityId))
        {
          RemoveEntity(entityId);
        }
        else
        {
          SendOutput("Invalid enityId, 1st param is not a valid number");
        }
      }
    }

    private void RemoveEntity(int entityId)
    {
      if (GameManager.Instance.World.Entities.dict.ContainsKey(entityId))
      {
        var _e = GameManager.Instance.World.Entities.dict[entityId];
        if (_e != null)
        {
          var v = new Vector3i();
          v.RoundToInt(_e.position);
          var _ec = EntityClass.list[_e.entityClass];

          SendOutput("Entity Removed: " + _e.entityType.ToString() + ":" + (_ec != null ? _ec.entityClassName : "") + " @" + v.x.ToString() + " " + v.x.ToString() + " " + v.x.ToString());
          GameManager.Instance.World.RemoveEntity(entityId, EnumRemoveEntityReason.Unloaded);
        }
        else
        {
          SendOutput("Invalid entity, entity not found: " + entityId);
        }
      }
      else
      {
        SendOutput("Invalid entityId, entity not found: " + entityId);
      }
    }
  }
}
