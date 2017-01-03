using System;
using System.Collections.Generic;
using UnityEngine;
using BCM.PersistentData;
using BCM.Models;
using System.Reflection;

namespace BCM.Commands
{
  public class ListQuests : BCCommandAbstract
  {
    public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
    {
      try
      {
        foreach (QuestClass qc in QuestClass.s_Quests.Values)
        {
          SdtdConsole.Instance.Output(qc.Name + "(" + qc.ID + "):" + qc.SubTitle);
          //Log.Out(qc.Name + "(" + qc.ID + "):" + qc.SubTitle);
        }
      }
      catch (Exception e)
      {
        Log.Out(Config.ModPrefix + " Error in " + GetType().Name + "." + MethodBase.GetCurrentMethod().Name + ": " + e);
      }
    }
  }
}
