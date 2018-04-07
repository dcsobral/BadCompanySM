using System.Collections.Generic;
using System.Linq;
using BCM.Models;
using JetBrains.Annotations;

namespace BCM.Commands
{
  [UsedImplicitly]
  public class BCGiveBuffToEntity : BCCommandAbstract
  {
    protected override void Process()
    {
      if (!BCUtils.CheckWorld(out var world)) return;

      switch (Params.Count)
      {
        case 0:
          SendOutput(GetHelp());
          break;

        case 1:
          var data = new List<object>();

          if (Params[0] != "list")
          {
            if (!int.TryParse(Params[0], out var entityId))
            {
              SendOutput($"Couldn't parse param '{Params[0]}' for entityId");

              return;
            }

            var entity = world.Entities.dict[entityId] as EntityAlive;
            if (entity == null)
            {
              SendOutput($"Unable to find entity by id {Params[0]}");

              return;
            }

            GetEntityBuffs(data, entity);
          }
          else
          {
            for (var i = world.Entities.list.Count - 1; i >= 0; i--)
            {
              var entity = world.Entities.list[i] as EntityAlive;
              if (entity == null) continue;

              GetEntityBuffs(data, entity);
            }
          }

          SendJson(data);
          break;

        case 2:
          if (Options.ContainsKey("type"))
          {
            var type = Params[0];
            var buffid = Params[1];
            if (!MultiBuffClass.s_classes.ContainsKey(buffid))
            {
              SendOutput($"Unknown Buff {buffid}");
            }
            else
            {
              var multiBuffClassAction = MultiBuffClassAction.NewAction(buffid);
              if (multiBuffClassAction == null)
              {
                break;
              }

              var count = 0;
              foreach (var target in world.Entities.list.OfType<EntityAlive>())
              {
                if (target.GetType().ToString() != type) continue;

                multiBuffClassAction.Execute(target.entityId, target, false, EnumBodyPartHit.None, null);
                count++;
              }
              SendOutput($"{count} {(count == 1 ? "entity" : "entities")} buffed with {buffid}");
            }
          }
          else
          {
            if (!int.TryParse(Params[0], out var entityId))
            {
              SendOutput("Error parsing entity id");

              break;
            }

            var buffid = Params[1];
            if (!MultiBuffClass.s_classes.ContainsKey(buffid))
            {
              SendOutput($"Unknown buff {buffid}");

              break;
            }

            if (!world.Entities.dict.ContainsKey(entityId))
            {
              SendOutput("Entity not found");

              break;
            }

            var multiBuffClassAction = MultiBuffClassAction.NewAction(buffid);
            var target = world.Entities.dict[entityId] as EntityAlive;
            if (multiBuffClassAction != null && target != null)
            {
              multiBuffClassAction.Execute(entityId, target, false, EnumBodyPartHit.None, null);
              SendOutput($"Buffed entity {entityId} with {buffid}");
            }
          }
          break;

        default:
          SendOutput("Invalid arguments");
          SendOutput(GetHelp());
          break;
      }
    }

    private static void GetEntityBuffs(ICollection<object> data, EntityAlive entity)
    {
      var name = "";
      if (entity is EntityPlayer)
      {
        name = entity.EntityName;
      }
      else if (EntityClass.list.ContainsKey(entity.entityClass))
      {
        name = EntityClass.list[entity.entityClass].entityClassName;
      }

      var entityBuffs = new BCMBuffEntity(entity.entityId, name);

      foreach (var current in entity.Stats.Buffs)
      {
        var buff = (MultiBuff)current;
        if (buff == null) continue;

        entityBuffs.Buffs.Add(new BCMBuffInfo(buff));
      }
      data.Add(entityBuffs);
    }
  }
}
