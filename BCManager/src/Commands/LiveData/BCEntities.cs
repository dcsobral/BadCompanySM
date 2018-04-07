using System.Collections.Generic;
using BCM.Models;
using System.Linq;
using JetBrains.Annotations;

namespace BCM.Commands
{
  [UsedImplicitly]
  public class BCEntities : BCCommandAbstract
  {
    private static void Filters() => SendJson(typeof(BCMEntity.StrFilters).GetFields()
      .ToDictionary(field => field.Name, field => $"{field.GetValue(typeof(BCMEntity.StrFilters))}"));

    private static void Indexed() => SendJson(BCMEntity.FilterMap
      .GroupBy(kvp => kvp.Value)
      .Select(group => group.First()).ToDictionary(kvp => kvp.Value, kvp => kvp.Key));

    protected override void Process()
    {
      if (!BCUtils.CheckWorld(out var world)) return;

      if (Options.ContainsKey("filters"))
      {
        Filters();

        return;
      }

      if (Options.ContainsKey("index"))
      {
        Indexed();

        return;
      }

      if (Params.Count > 1)
      {
        SendOutput("Wrong number of arguments");
        SendOutput(Config.GetHelp(GetType().Name));

        return;
      }

      if (Params.Count == 1)
      {
        // specific entity
        Entity e = null;
        if (int.TryParse(Params[0], out var entityId))
        {
          if (world.Entities.dict.ContainsKey(entityId)) e = world.Entities.dict[entityId];
        }
        if (e == null)
        {
          SendOutput("Entity id not found.");

          return;
        }

        var entity = new BCMEntity(e, Options, GetFilters(BCMGameObject.GOTypes.Entities));
        if (Options.ContainsKey("min"))
        {
          SendJson(new List<List<object>>
          {
            entity.Data().Select(d => d.Value).ToList()
          });
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
        var data = new List<object>();
        foreach (var entity in BCUtils.FilterEntities(world.Entities.dict, Options).Values.Select(en => new BCMEntity(en, Options, GetFilters(BCMGameObject.GOTypes.Entities))))
        {
          if (Options.ContainsKey("min"))
          {
            data.Add(entity.Data().Select(d => d.Value).ToList());
          }
          else
          {
            data.Add(entity.Data());
          }
        }

        SendJson(data);
      }
    }
  }
}
