using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace BCM.Commands
{
  public class ListEntitiesDataJSON : BCCommandAbstract
  {
    public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
    {
      try
      {
        if (_params.Count != 0 && _params.Count != 1)
        {
          SdtdConsole.Instance.Output("Wrong number of arguments, expected 0 or 1, found " + _params.Count + ".");
          return;
        }

        if (_params.Count == 1)
        {
          //ClientInfo ci = ConsoleHelper.ParseParamIdOrName(_params[0]);
          //if (ci == null)
          //{
          //  SdtdConsole.Instance.Output("Playername or entity id not found.");
          //  return;
          //}

          int n = int.MinValue;
          int.TryParse(_params[0], out n);
          Entity entity = GameManager.Instance.World.Entities.dict[n]; //ci.entityId
          if (entity == null)
          {
            SdtdConsole.Instance.Output("Entity id not found.");
            return;
          }
          printEntityData(entity);

        } else { 
          for (int i = GameManager.Instance.World.Entities.list.Count - 1; i >= 0; i--)
          {
            Entity entity = GameManager.Instance.World.Entities.list[i];
            printEntityData(entity);
          }
          SdtdConsole.Instance.Output("Total of " + GameManager.Instance.World.Entities.Count + " entities in the world");
        }
      }
      catch (Exception e)
      {
        Log.Out("" + Config.ModPrefix + " Error in " + GetType().Name + "." + MethodBase.GetCurrentMethod().Name + ": " + e);
      }
    }

    private void printEntityData(Entity entity)
    {
      string entitydata = JsonUtility.ToJson(entity);
      //var json = AllocsFixes.JSON.Parser.Parse(entitydata);
      //string output = json.ToString(true);
      SdtdConsole.Instance.Output(entitydata);
    }
  }
}
