using System;
using System.Collections.Generic;
using UnityEngine;
using BCM.PersistentData;
using BCM.Models;
using System.Reflection;

namespace BCM.Commands
{
  public class RemoveQuestFromPlayer : BCCommandAbstract
  {
    public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
    {
      try
      {
        if (_params.Count != 2)
        {
          SdtdConsole.Instance.Output("Invalid arguments");
        }
        else
        {
          string str = _params[1];
          ClientInfo clientInfo = ConsoleHelper.ParseParamIdOrName(_params[0], true, false);
          if (clientInfo != null)
          {
            clientInfo.SendPackage(new NetPackageConsoleCmdClient("removequest " + str, true));
          }
          else if (_senderInfo.IsLocalGame)
          {
            SdtdConsole.Instance.Output("Use the \"removequest\" command for the local player.");
          }
          else
          {
            SdtdConsole.Instance.Output("Playername or entity ID not found.");
          }
        }
      }
      catch (Exception e)
      {
        Log.Out(Config.ModPrefix + " Error in " + GetType().Name + "." + MethodBase.GetCurrentMethod().Name + ": " + e);
      }
    }
  }
}
