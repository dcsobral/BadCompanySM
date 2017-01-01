using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace BCM.Commands
{
  public class ListPlayersGamestage : BCCommandAbstract
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
          ClientInfo ci = ConsoleHelper.ParseParamIdOrName(_params[0]);
          if (ci == null)
          {
            SdtdConsole.Instance.Output("Playername or entity id not found.");
            return;
          }

          EntityPlayer p1 = GameManager.Instance.World.Players.dict[ci.entityId];
          if (p1 == null)
          {
            SdtdConsole.Instance.Output("Playername or entity id not found.");
            return;
          }
          printPlayerGamestage(p1);
        }
        else
        {
          List<EntityPlayer> players = GameManager.Instance.World.Players.list;
          foreach (EntityPlayer player in players)
          {
            printPlayerGamestage(player);
          }
        }
      }
      catch (Exception e)
      {
        Log.Out("" + Config.ModPrefix + " Error in " + GetType().Name + "." + MethodBase.GetCurrentMethod().Name + ": " + e);
      }
    }

    private void printPlayerGamestage(EntityPlayer player)
    {
      int gamestage = player.gameStage;
      string playerGamestage = "Gamestage for " + player.EntityName + " (Id:" + player.entityId + "): " + gamestage;
      SdtdConsole.Instance.Output(playerGamestage);
    }
  }
}
