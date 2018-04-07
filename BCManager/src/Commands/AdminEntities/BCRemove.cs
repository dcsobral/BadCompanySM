using System.Collections.Generic;
using BCM.Models;
using JetBrains.Annotations;

namespace BCM.Commands
{
  [UsedImplicitly]
  public class BCRemove : BCCommandAbstract
  {
    protected override void Process()
    {
      if (!BCUtils.CheckWorld(out var world)) return;

      if (Options.ContainsKey("all") || Options.ContainsKey("istype") || Options.ContainsKey("type") || Options.ContainsKey("minibike") || Options.ContainsKey("ecname"))
      {
        var count = new Dictionary<string, int>();

        foreach (var key in BCUtils.FilterEntities(world.Entities.dict, Options).Keys)
        {
          RemoveEntity(world, key, count);
        }

        SendJson(count);
      }
      else if (Params.Count == 1)
      {
        if (!int.TryParse(Params[0], out var entityId))
        {
          SendOutput("Unable to parse entity id");

          return;
        }

        RemoveEntity(world, entityId);
      }
      else
      {
        SendOutput(GetHelp());
      }
    }

    private static void RemoveEntity(World world, int entityId, IDictionary<string, int> count = null)
    {
      if (!world.Entities.dict.ContainsKey(entityId))
      {
        if (!Options.ContainsKey("all"))
        {
          SendOutput($"Invalid entityId, entity not found: {entityId}");
        }

        return;
      }

      var e = world.Entities.dict[entityId];
      switch (e)
      {
        case null:
          if (!Options.ContainsKey("all"))
          {
            SendOutput($"Invalid entity, entity not found: {entityId}");
          }
          return;

        case EntityPlayer _:
          if (!Options.ContainsKey("all"))
          {
            SendOutput("You can't remove a player!");
          }
          return;

        case EntityMinibike _:
          if (!Options.ContainsKey("minibike"))
          {
            SendOutput("Minibike not removed, use /minibike to remove minibikes");

            return;
          }
          break;
      }
      var entityClass = EntityClass.list[e.entityClass];

      world.RemoveEntity(entityId, EnumRemoveEntityReason.Despawned);

      if (Options.ContainsKey("all") || Options.ContainsKey("istype") || Options.ContainsKey("type") || Options.ContainsKey("minibike") || Options.ContainsKey("ecname"))
      {
        if (count == null) return;

        if (count.ContainsKey($"{e.GetType()}:{entityClass.entityClassName}"))
        {
          count[$"{e.GetType()}:{entityClass.entityClassName}"]++;
        }
        else
        {
          count.Add($"{e.GetType()}:{entityClass.entityClassName}", 1);
        }
      }
      else
      {
        var pos = new BCMVector3(e.position);
        SendOutput($"Entity Removed: {e.GetType()}:{(entityClass != null ? entityClass.entityClassName : "")} @{pos}");
      }
    }
  }
}
