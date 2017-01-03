using System;
using System.Collections.Generic;
using UnityEngine;
using BCM.PersistentData;
using BCM.Models;
using System.Reflection;

namespace BCM.Commands
{
  public class ListHubCellData : BCCommandAbstract
  {
    public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
    {
      try
      {
        if (_params.Count != 0 && _params.Count != 2)
        {
          SdtdConsole.Instance.Output("Wrong number of arguments, expected 0 or 2, found " + _params.Count + ".");
          return;
        }

        if (_params.Count == 2)
        {
          RWG2.PCRWGDataLoader loader = new RWG2.PCRWGDataLoader();
          RWG2.RWGDataInfo rwgInfo = new RWG2.RWGDataInfo();
          rwgInfo.worldName = GamePrefs.GetString(EnumGamePrefs.GameWorld);
          rwgInfo.gameName = GamePrefs.GetString(EnumGamePrefs.GameName);
          rwgInfo.seed = GameManager.Instance.World.Seed;
          rwgInfo.isClient = false;
          rwgInfo.isDebugMode = false;
          rwgInfo.isPregenerating = false;

          int x, y = 0;
          int.TryParse(_params[0], out x);
          int.TryParse(_params[1], out y);
          string output = "\nHubCellLots for " + _params[0] + "," + _params[1] + ":\n";

          RWG2.HubCell hc = loader.LoadHubCell(new Vector2i(x, y), rwgInfo);
          foreach (RWG2.HubCell.Lot lot in hc.lots)
          {
            output += lot.PrefabName + ":" + lot.PrefabSpawnPos + "\n";
          }
          SdtdConsole.Instance.Output(output);
          Log.Out(output);
        }
      }
      catch (Exception e)
      {
        Log.Out(Config.ModPrefix + " Error in " + GetType().Name + "." + MethodBase.GetCurrentMethod().Name + ": " + e);
      }
    }
  }
}
