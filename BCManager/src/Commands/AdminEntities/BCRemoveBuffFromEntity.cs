namespace BCM.Commands
{
  public class BCRemoveBuffFromEntity : BCCommandAbstract
  {
    public override void Process()
    {
      if (Params.Count != 2)
      {
        SendOutput("Invalid arguments");
        SendOutput(GetHelp());

        return;
      }

      var _world = GameManager.Instance.World;
      if (_world == null) return;

      if (!int.TryParse(Params[0], out var entityId))
      {
        SendOutput($"Unable to parse entity id {Params[0]}");

        return;
      }


      if (!(_world.GetEntity(entityId) is EntityAlive entityAlive))
      {
        SendOutput("Unable to find entity");

        return;
      }

      if (MultiBuffClass.s_classes.ContainsKey(Params[1]))
      {
        entityAlive.Stats.Debuff(Params[1]);
        SendOutput($"Buff {Params[1]} removed from entity ({entityAlive.entityId}) {entityAlive.EntityName}");
      }
      else
      {
        SendOutput($"Unable to find buff {Params[1]}");
      }
    }
  }
}
