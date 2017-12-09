using System.Collections.Generic;

namespace BCM.Commands
{
  public class BCRemove : BCCommandAbstract
  {
    public override void Process()
    {
      var world = GameManager.Instance.World;
      if (world == null) return;

      if (Options.ContainsKey("all") || Options.ContainsKey("istype") || Options.ContainsKey("type"))
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
          SendOutput("Invalid enity id, 1st param is not a valid number");

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
      }

      world.RemoveEntity(entityId, EnumRemoveEntityReason.Despawned);

      var entityClass = EntityClass.list[e.entityClass];
      if (Options.ContainsKey("all") || Options.ContainsKey("istype") || Options.ContainsKey("type"))
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
        var v = new Vector3i();
        v.RoundToInt(e.position);
        SendOutput($"Entity Removed: {e.GetType()}:{(entityClass != null ? entityClass.entityClassName : "")} @{v.x} {v.y} {v.z}");
      }
    }
  }
}
