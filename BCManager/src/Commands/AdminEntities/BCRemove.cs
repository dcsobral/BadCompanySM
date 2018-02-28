using System.Collections.Generic;
using BCM.Models;

namespace BCM.Commands
{
  public class BCRemove : BCCommandAbstract
  {
    public override void Process()
    {
      var world = GameManager.Instance.World;
      if (world == null) return;

      if (Options.ContainsKey("all") || Options.ContainsKey("istype") || Options.ContainsKey("type") || Options.ContainsKey("minibike"))
      {
        var count = new Dictionary<string, int>();

        foreach (var key in BCUtils.FilterEntities(world.Entities.dict, Options).Keys)
        {
          RemoveEntity(key, count);
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

        RemoveEntity(entityId);
      }
      else
      {
        SendOutput(GetHelp());
      }
    }

    private static void RemoveEntity(int entityId, IDictionary<string, int> count = null)
    {
      var world = GameManager.Instance.World;
      if (world == null) return;

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
          }

          return;
      }

      world.RemoveEntity(entityId, EnumRemoveEntityReason.Despawned);

      var entityClass = EntityClass.list[e.entityClass];
      if (Options.ContainsKey("all") || Options.ContainsKey("istype") || Options.ContainsKey("type") || Options.ContainsKey("minibike"))
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
