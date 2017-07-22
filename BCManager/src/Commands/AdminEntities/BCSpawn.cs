using UnityEngine;

namespace BCM.Commands
{
  public class BCSpawn : BCCommandAbstract
  {
    public override void Process()
    {
      int count = 1;
      Vector3i targetPos = Vector3i.zero;

      if (_params.Count == 0)
      {

      }
      else if (_params.Count == 1)
      {
          count = 25;
          //target = comand sender
        }
        else
        {
          if (_params.Count == 2)
          {
            if (!int.TryParse(_params[1], out count))
            {
              SendOutput("Count was not a number");

              return;
            }
            //target = comand sender
          }
          if (_params.Count == 3)
          {
            if (!int.TryParse(_params[2], out count))
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

      if (_params.Count < 3 && _senderInfo.RemoteClientInfo != null)
      {
        ci = _senderInfo.RemoteClientInfo;
      }
      else
      {
        var nameOrId = _params[0];

        string id;
        ConsoleHelper.ParseParamPartialNameOrId(nameOrId, out id, out ci);
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
