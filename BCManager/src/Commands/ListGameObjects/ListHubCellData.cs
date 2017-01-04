using System;
using System.Collections.Generic;
using UnityEngine;
using BCM.PersistentData;
using BCM.Models;
using System.Reflection;
using System.IO;

namespace BCM.Commands
{
  public class ListHubCellData : BCCommandAbstract
  {
    public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
    {
      try
      {
        if (_params.Count > 3)
        {
          SdtdConsole.Instance.Output("Wrong number of arguments, expected between 0 and 3, found " + _params.Count + ".");
          return;
        }

        RWG2.PCRWGDataLoader loader = new RWG2.PCRWGDataLoader();
        RWG2.RWGDataInfo rwgInfo = new RWG2.RWGDataInfo();
        rwgInfo.worldName = GamePrefs.GetString(EnumGamePrefs.GameWorld);
        rwgInfo.gameName = GamePrefs.GetString(EnumGamePrefs.GameName);
        rwgInfo.seed = GameManager.Instance.World.Seed;
        rwgInfo.isClient = false;
        rwgInfo.isDebugMode = false;
        rwgInfo.isPregenerating = false;
        string SaveDir = GameUtils.GetSaveGameDir() + "/HubCellData/";

        string output = "\n";
        //find all prefabs
        if (_params.Count == 0)
        {
          output += "\nHubCellLots - All:\n";
          int radius = 6;
          for (int x = -radius; x <= radius; x++)
          {
            for (int y = -radius; y <= radius; y++)
            {
              if (File.Exists(SaveDir + x.ToString() + "." + y.ToString() + ".hcd"))
              {
                RWG2.HubCell hc = loader.LoadHubCell(new Vector2i(x, y), rwgInfo);
                foreach (RWG2.HubCell.Lot lot in hc.lots)
                {
                  output += lot.PrefabName + ":" + lot.PrefabSpawnPos + "\n";
                }
              }
            }
          }
          //SdtdConsole.Instance.Output(output);
          SdtdConsole.Instance.Output("Data sent to log file");
          Log.Out(output);
        }

        if (_params.Count == 1)
        {
          output += "\nHubCellLots with filter:" + _params[0] + ":\n";
          int radius = 6;
          for (int x = -radius; x <= radius; x++)
          {
            for (int y = -radius; y <= radius; y++)
            {
              if (File.Exists(SaveDir + x.ToString() + "." + y.ToString() + ".hcd"))
              {
                RWG2.HubCell hc = loader.LoadHubCell(new Vector2i(x, y), rwgInfo);
                foreach (RWG2.HubCell.Lot lot in hc.lots)
                {
                  if (lot.PrefabName.Contains(_params[0]))
                  {
                    output += lot.PrefabName + ":" + lot.PrefabSpawnPos + "\n";
                  }
                }
              }
            }
          }
          SdtdConsole.Instance.Output(output);
          Log.Out(output);
        }

        if (_params.Count == 2 || _params.Count == 3)
        {
          int x, y = 0;
          int.TryParse(_params[0], out x);
          int.TryParse(_params[1], out y);
          output += "HubCellLots for " + _params[0] + "," + _params[1] + "";
          if (_params.Count == 3)
          {
            output += " with filter:" + _params[2];
          }
          output += "\n";

          //try
          //{
          RWG2.HubCell hc = loader.LoadHubCell(new Vector2i(x, y), rwgInfo);
            foreach (RWG2.HubCell.Lot lot in hc.lots)
            {
              if (_params.Count == 3)
              {
                if (!lot.PrefabName.Contains(_params[2]))
                {
                  continue;
                }
              }
              output += lot.PrefabName + ":" + lot.PrefabSpawnPos + "\n";
            }
          //}
          //catch
          //{ //hubcelldata not found, skipping
          //}
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
