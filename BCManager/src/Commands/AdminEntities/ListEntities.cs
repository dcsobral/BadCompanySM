using System.Collections.Generic;
using BCM.Models;
using System;

namespace BCM.Commands
{
  public class ListEntities : BCCommandAbstract
  {
    private void Filters()
    {
      var data = new Dictionary<string, string>();
      var obj = typeof(BCMEntity.StrFilters);
      foreach (var field in obj.GetFields())
      {
        data.Add(field.Name, field.GetValue(obj).ToString());
      }

      SendJson(data);
    }
    private void Index()
    {
      var data = new Dictionary<string, int>();
      foreach (var value in Enum.GetValues(typeof(BCMEntity.Filters)))
      {
        data.Add(value.ToString(), (int)value);
      }

      SendJson(data);
    }

    public override void Process()
    {
      if (GameManager.Instance.World == null)
      {
        SendOutput("The world isn't loaded");

        return;
      }

      if (_options.ContainsKey("filters"))
      {
        Filters();
        return;
      }
      if (_options.ContainsKey("index"))
      {
        Index();
        return;
      }

      if (_params.Count > 1)
      {
        SendOutput("Wrong number of arguments");
        SendOutput(Config.GetHelp(GetType().Name));

        return;
      }

      if (_params.Count == 1)
      {
        // specific entity
        Entity _entity = null;
        int _entityId = -1;

        if (int.TryParse(_params[0], out _entityId))
        {
          _entity = GameManager.Instance.World.Entities.dict[_entityId];
        }
        if (_entity == null)
        {
          SdtdConsole.Instance.Output("Entity id not found.");

          return;
        }

        var entity = new BCMEntity(_entity, _options);
        if (_options.ContainsKey("nokeys"))
        {
          List<List<object>> keyless = new List<List<object>>();
          List<object> obj = new List<object>();
          foreach (var d in entity.Data())
          {
            obj.Add(d.Value);
          }
          keyless.Add(obj);
          SendJson(keyless);
        }
        else
        {
          SendJson(entity.Data());
        }
      }
      else
      {
        //todo: /entity=id1,id2,id3

        // All entities
        List<object> data = new List<object>();
        List<List<object>> keyless = new List<List<object>>();
        var _entities = BCUtils.filterEntities(GameManager.Instance.World.Entities.dict, _options).Values;

        foreach (Entity _entity in _entities)
        {
          var entity = new BCMEntity(_entity, _options);
          if (_options.ContainsKey("nokeys"))
          {
            var obj = new List<object>();
            foreach (var d in entity.Data())
            {
              obj.Add(d.Value);
            }
            keyless.Add(obj);
          }
          else
          {
            data.Add(entity.Data());
          }
        }

        if (_options.ContainsKey("nokeys"))
        {
          SendJson(keyless);
        }
        else
        {
          SendJson(data);
        }
      }
    }
  }
}
