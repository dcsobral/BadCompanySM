using System;
using System.Collections.Generic;

namespace BCM.Commands
{
  public class BCRemove : BCCommandAbstract
  {
    public override void Process()
    {
      if (_options.ContainsKey("all") || _options.ContainsKey("istype") || _options.ContainsKey("type"))
      {
        var _count = new Dictionary<string, int>();
        var _entKeys = new List<int>();
        foreach (var _e in GameManager.Instance.World.Entities.dict)
        {
          if (_options.ContainsKey("all"))
          {
            _entKeys.Add(_e.Key);
          }
          else if (_options.ContainsKey("type"))
          {
            if (_e.Value != null)
            {
              if (_e.Value.GetType().ToString() == _options["type"])
              {
                _entKeys.Add(_e.Key);
              }
            }
            else
            {
              SendOutput("Entity was null");
            }
          }
          else if (_options.ContainsKey("istype"))
          {
            if (_e.Value != null)
            {

              Type type = Type.GetType(_e.Value.GetType().AssemblyQualifiedName.Replace(_e.Value.GetType().ToString(), _options["istype"]));
                
              if (type != null)
              {
                if (type.IsInstanceOfType(_e.Value))
                {
                  _entKeys.Add(_e.Key);
                }
              }
            } else
            {
              SendOutput("Entity was null");
            }
          }
        }

        foreach (var key in _entKeys)
        {
          RemoveEntity(key, _count);
        }

        SendJson(_count);
      }
      else if (_params.Count == 1)
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
      else
      {
        SendOutput(GetHelp());
      }

    }

    private void RemoveEntity(int entityId, Dictionary<string,int> _count = null)
    {
      if (GameManager.Instance.World.Entities.dict.ContainsKey(entityId))
      {
        var _e = GameManager.Instance.World.Entities.dict[entityId];
        if (_e is EntityPlayer)
        {
          if (!_options.ContainsKey("all"))
          {
            SendOutput("You can't remove a player!");
          }

          return;
        }
        if (_e != null)
        {
          var v = new Vector3i();
          v.RoundToInt(_e.position);
          var _ec = EntityClass.list[_e.entityClass];

          if (!(_options.ContainsKey("all") || _options.ContainsKey("istype") || _options.ContainsKey("type")))
          {
            SendOutput("Entity Removed: " + _e.GetType().ToString() + ":" + (_ec != null ? _ec.entityClassName : "") + " @" + v.x.ToString() + " " + v.x.ToString() + " " + v.x.ToString());
          }
          else if (_count != null)
          {
            if (_count.ContainsKey(_e.GetType().ToString() + ":" + _ec.entityClassName))
            {
              _count[_e.GetType().ToString() + ":" + _ec.entityClassName]++;
            }
            else
            {
              _count.Add(_e.GetType().ToString() + ":" + _ec.entityClassName, 1);
            }
          }
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
