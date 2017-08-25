namespace BCM.Commands
{
  public class BCSpawn : BCCommandAbstract
  {
    public override void Process()
    {
      int count = 1;
      Vector3i targetPos = Vector3i.zero;

      if (Params.Count == 0)
      {

      }
      else if (Params.Count == 1)
      {
          count = 25;
          //target = comand sender
        }
        else
        {
          if (Params.Count == 2)
          {
            if (!int.TryParse(Params[1], out count))
            {
              SendOutput("Count was not a number");

              return;
            }
            //target = comand sender
          }
          if (Params.Count == 3)
          {
            if (!int.TryParse(Params[2], out count))
            {
              SendOutput("Count was not a number");

              return;
            }
            //target = _params[1];
          }
        }
    }

    private Vector3i GetTarget()
    {
      Vector3i v = Vector3i.zero;
      ClientInfo ci = null;

      if (Params.Count < 3 && SenderInfo.RemoteClientInfo != null)
      {
        ci = SenderInfo.RemoteClientInfo;
      }
      else
      {
        var nameOrId = Params[0];

        ConsoleHelper.ParseParamPartialNameOrId(nameOrId, out string _, out ci);
        if (ci != null)
        {

        }
      }

      EntityPlayer target = null;
      if (ci != null)
      {
        target = GameManager.Instance.World.Entities.dict[ci.entityId] as EntityPlayer;
      }

      if (target != null)
      {
        v.RoundToInt(target.position);
      }
      else
      {
        SendOutput("Unable to find target to get location");
      }

      return v;
    }
  }
}
