namespace BCM.Commands
{
  public class GiveBuffToEntity : BCCommandAbstract
  {
    public override void Process()
    {
      if (_params.Count == 0)
      {
        for (int i = GameManager.Instance.World.Entities.list.Count - 1; i >= 0; i--)
        {
          EntityAlive entity = GameManager.Instance.World.Entities.list[i] as EntityAlive;
          if (entity != null)
          {
            bool first = true;
            string output = "Buffs for "+ entity.entityId + ":{";
            foreach (MultiBuff b in entity.Stats.Buffs)
            {
              if (!first) { output += _sep; } else { first = false; }
              output += " " + b.Name + "(" + b.MultiBuffClass.Id + ")" + ":" + (b.MultiBuffClass.FDuration * b.Timer.TimeFraction).ToString("0") + "/" + b.MultiBuffClass.FDuration + "(s) (" + (b.Timer.TimeFraction * 100).ToString("0.0") + "%)";
            }
            output += "}" + _sep;
            SendOutput(output);
          }
        }

      }
      else if (_params.Count == 2)
      {
        if (_options.ContainsKey("type"))
        {
          string type = _params[0];
          string buffid = _params[1];
          if (MultiBuffClass.s_classes.ContainsKey(buffid))
          {
            MultiBuffClassAction multiBuffClassAction = MultiBuffClassAction.NewAction(buffid);
            if (multiBuffClassAction != null)
            {
              foreach (Entity e in GameManager.Instance.World.Entities.list)
              {
                EntityAlive target = e as EntityAlive;
                if (target != null && target.GetType().ToString() == type)
                {
                  multiBuffClassAction.Execute(target.entityId, target, false, EnumBodyPartHit.None, null);
                }
              }
            }
          }
          else
          {
            SdtdConsole.Instance.Output("Unknown Buff " + buffid);
          }
        }
        else
        {
          int entityId = 0;
          if (int.TryParse(_params[0], out entityId))
          {
            string buffid = _params[1];
            if (MultiBuffClass.s_classes.ContainsKey(buffid))
            {
              MultiBuffClassAction multiBuffClassAction = MultiBuffClassAction.NewAction(buffid);
              if (multiBuffClassAction != null)
              {
                EntityAlive target = GameManager.Instance.World.Entities.dict[entityId] as EntityAlive;
                if (target != null)
                {
                  multiBuffClassAction.Execute(entityId, target, false, EnumBodyPartHit.None, null);
                }
              }
            }
            else
            {
              SdtdConsole.Instance.Output("Unknown Buff " + buffid);
            }
          }
          else
          {
            SdtdConsole.Instance.Output("Error Parsing entityId");
          }
        }
      }
      else
      {
        SdtdConsole.Instance.Output("Invalid arguments");
        SdtdConsole.Instance.Output(GetHelp());
      }
    }
  }
}
