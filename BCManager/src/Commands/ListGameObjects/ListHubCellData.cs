using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using RWG2;

namespace BCM.Commands
{
  public class ListHubCellData : BCCommandAbstract
  {
    //public Dictionary<Vector2i, RWG2.HubCell> HCD = new Dictionary<Vector2i, RWG2.HubCell>();
    public List<Vector2i> HCDLotGrid = new List<Vector2i>();
    public List<string> HCDLotName = new List<string>();
    public List<Vector3i> HCDLotPos = new List<Vector3i>();

    public void GetHubCellData()
    {
      int radius = 6;
      string hcdDir = GameUtils.GetSaveGameDir() + "/HubCellData/";
      BaseRWGDataLoader rwgDataLoader = new BaseRWGDataLoader(GameManager.Instance.World.Seed, new PCRWGDataLoader(), false);

      //load HubCell Data if not already loaded
      if (HCDLotName.Count == 0)
      {
        for (int x = -radius; x < radius; x++)
        {
          for (int y = -radius; y < radius; y++)
          {
            string hcdFile = string.Format("{0}{1}.{2}.hcd", hcdDir, x, y);
            if (File.Exists(hcdFile))
            {
              Vector2i v = new Vector2i(x, y);
              BinaryReader binaryReader = new BinaryReader(File.OpenRead(hcdFile));
              RWG2.HubCell hubCell = new RWG2.HubCell(v);
              hubCell.Read(binaryReader);
              binaryReader.Close();

              //HCD.Add(v, hubCell);
              foreach (RWG2.HubCell.Lot lot in hubCell.GetAllLots())
              {
                HCDLotName.Add(lot.PrefabName);
                HCDLotPos.Add(lot.PrefabSpawnPos);
                HCDLotGrid.Add(v);
              }
              binaryReader = null;
              hubCell = null;
            }
          }
        }
      }
      rwgDataLoader = null;
    }
    public virtual Dictionary<string, string> jsonObject()
    {
      Dictionary<string, string> data = new Dictionary<string, string>();

      GetHubCellData();
      var worldpos = "";
      var csvpos = "";
      if (_options.ContainsKey("worldpos"))
      {
        worldpos = "worldpos";
      }
      if (_options.ContainsKey("csvpos"))
      {
        csvpos = "csvpos";
      }

      if (HCDLotName.Count > 0)
      {
        Dictionary<string, string> lotData = new Dictionary<string, string>();
        for (var i = 0; i < HCDLotName.Count; i++)
        {
          data.Add(i.ToString(), "{ \"name\":\"" + HCDLotName[i] + "\", \"pos\":\"" + Convert.PosToStr(HCDLotPos[i], worldpos) + "\", \"grid\":\"" + Convert.PosToStr(HCDLotGrid[i], csvpos) + "\" }");
        }
      }

      return data;
    }

    public override void Process()
    {
      string output = "";
      if (_options.ContainsKey("json"))
      {
        output = BCUtils.toJson(jsonObject());
        SendOutput(output);
      }
      else
      {
        if (_params.Count > 3)
        {
          SdtdConsole.Instance.Output("Wrong number of arguments, expected between 0 and 3, found " + _params.Count + ".");
          return;
        }
        if (_options.ContainsKey("current"))
        {
          try
          {
            int EntityId = _senderInfo.RemoteClientInfo.entityId;
            EntityPlayer EP = GameManager.Instance.World.Players.dict[EntityId];
            Vector2i playergrid = RWGCore.Instance.WorldPosToGridPos(new Vector2(EP.position.x, EP.position.z));
            string _filter = "";
            if (_params.Count != 0)
            {
              _filter = _params[0];
              _params.Clear();
            }
            _params.Add(playergrid.x.ToString());
            _params.Add(playergrid.y.ToString());
            _params.Add(_filter);
          }
          catch
          {
            SdtdConsole.Instance.Output("Unable to use /current option when caller not in game.");
          }
        }

        var postype = "";
        if (_options.ContainsKey("worldpos"))
        {
          postype = "worldpos";
        }
        if (_options.ContainsKey("csvpos"))
        {
          postype = "csvpos";
        }

        GetHubCellData();

        // GET PREFABS
        if (_params.Count == 0)
        {
          if (_options.ContainsKey("all"))
          {
            output += "All HubCellLots" + _sep;
            int index = 0;
            foreach (string lotName in HCDLotName)
            {
              output += lotName + (_options.ContainsKey("csv") ? "," : ":") + Convert.PosToStr(HCDLotPos[index], postype) + _sep;
              index++;
            }
          }
          else
          {
            output += GetHelp();
          }
        }

        if (_params.Count == 1)
        {
          output += "All HubCellLots with filter '" + _params[0] + "'" + _sep;
          int index = 0;
          foreach (string lotName in HCDLotName)
          {
            if (lotName.ToLower().Contains(_params[0].ToLower()))
            {
              output += lotName + (_options.ContainsKey("csv") ? "," : ":") + Convert.PosToStr(HCDLotPos[index], postype) + _sep;
            }
            index++;
          }
        }

        if (_params.Count == 2 || _params.Count == 3)
        {
          int x, y = 0;
          int.TryParse(_params[0], out x);
          int.TryParse(_params[1], out y);
          output += "HubCellLots for " + x + "," + y;
          if (_params.Count == 3)
          {
            output += " with filter:" + _params[2];
          }
          output += _sep;

          int index = 0;
          foreach (string lotName in HCDLotName)
          {
            if (HCDLotGrid[index] == new Vector2i(x, y))
            {
              if (_params.Count == 3)
              {
                if (!lotName.ToLower().Contains(_params[2].ToLower()))
                {
                  continue;
                }
              }
              output += lotName + (_options.ContainsKey("csv") ? "," : ":") + Convert.PosToStr(HCDLotPos[index], postype) + _sep;
            }
            index++;
          }
        }

        SendOutput(output);

      }
      GC.Collect();
      GC.WaitForPendingFinalizers();
    }
  }
}
