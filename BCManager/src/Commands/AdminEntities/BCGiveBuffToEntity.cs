using System.Collections.Generic;
using System.Linq;

namespace BCM.Commands
{
  public class BCGiveBuffToEntity : BCCommandAbstract
  {
    public class BCMBuff
    {
      public string Name;
      public string Id;
      public string Duration;
      public string Percent;
    }
    public class BCMEntity
    {
      public int EntityId;
      public string Name;
      public List<BCMBuff> Buffs;
    }

    public override void Process()
    {
      var entities = GameManager.Instance.World.Entities;

      switch (Params.Count)
      {
        case 0:
          var data = new List<object>();

          for (var i = entities.list.Count - 1; i >= 0; i--)
          {
            var entity = entities.list[i] as EntityAlive;
            if (entity == null) continue;

            var e = new BCMEntity
            {
              EntityId = entity.entityId,
              Name = entity.name,
              Buffs = new List<BCMBuff>()
            };

            foreach (var current in entity.Stats.Buffs)
            {
              var buff = (MultiBuff) current;
              if (buff == null) continue;

              e.Buffs.Add(new BCMBuff
              {
                Name = buff.Name,
                Id = buff.MultiBuffClass.Id,
                Duration = $"{buff.MultiBuffClass.FDuration * buff.Timer.TimeFraction:0}/{buff.MultiBuffClass.FDuration}(s)",
                Percent = $"{buff.Timer.TimeFraction * 100:0.0}%"
              });
            }
            data.Add(e);
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

              foreach (var target in entities.list.OfType<EntityAlive>())
              {
                if (target.GetType().ToString() != type) continue;

                multiBuffClassAction.Execute(target.entityId, target, false, EnumBodyPartHit.None, null);
              }
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

            if (!entities.dict.ContainsKey(entityId))
            {
              SendOutput("Entity not found");

              break;
            }

            var multiBuffClassAction = MultiBuffClassAction.NewAction(buffid);
            var target = entities.dict[entityId] as EntityAlive;
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
  }
}
