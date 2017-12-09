using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

namespace BCM.Commands
{
  public class BCHubCellData : BCCommandAbstract
  {
    public class BCMVector2i
    {
      public int x;
      public int y;
      public BCMVector2i()
      {
        x = 0;
        y = 0;
      }
      public BCMVector2i(int x, int y)
      {
        this.x = x;
        this.y = y;
      }
      public BCMVector2i(Vector2 v)
      {
        x = Mathf.RoundToInt(v.x);
        y = Mathf.RoundToInt(v.y);
      }
      public BCMVector2i(Vector2i v)
      {
        x = v.x;
        y = v.y;
      }
    }
    public class BCMVector3i
    {
      public int x;
      public int y;
      public int z;
      public BCMVector3i()
      {
        x = 0;
        y = 0;
        z = 0;
      }
      public BCMVector3i(Vector3 v)
      {
        x = (int)Mathf.Floor(v.x);
        y = (int)Mathf.Floor(v.y);
        z = (int)Mathf.Floor(v.z);
      }
      public BCMVector3i(Vector3i v)
      {
        x = v.x;
        y = v.y;
        z = v.z;
      }

      public override string ToString()
      {
        return $"{x} {y} {z}";
      }
    }

    public enum BCMLotType
    {
      Hub,
      Wilderness
    }

    public class BCMBasicLot
    {
      public string Prefab;
      public string Position;

      public BCMBasicLot(BCMLot lot)
      {
        Prefab = lot.Prefab;
        Position = string.Format($"{lot.Position.x} {lot.Position.y} {lot.Position.z}");
      }
    }
    public class BCMLot
    {
      public string Prefab;
      public string Township;
      public string Type;
      public int InstanceId;
      public BCMVector3i Position;
      public int Rotation;
      public string LotType;

      public BCMLot(BCMLot lot)
      {
        Prefab = lot.Prefab;
        Township = lot.Township;
        Type = lot.Type;
        InstanceId = lot.InstanceId;
        Position = lot.Position;
        Rotation = lot.Rotation;
        LotType = lot.LotType;
      }

      public BCMLot(RWG2.HubCell.Lot lot, BCMLotType type)
      {
        Prefab = lot.PrefabName;
        Township = lot.Township.ToString();
        Type = lot.Type.ToString();
        InstanceId = lot.PrefabInstance.id;
        Position = new BCMVector3i(lot.PrefabSpawnPos);
        Rotation = lot.RoadDirection;
        LotType = type.ToString();
      }
    }

    public class BCMHubCell
    {
      public BCMVector2i GridPos;
      public string Township;
      //public string HubName;
      public string CellRule;
      public string HubRule;
      public string WildernessRule;
      public List<BCMLot> Lots;

      public BCMHubCell(BCMHubCell hubCell)
      {
        GridPos = hubCell.GridPos;
        Township = hubCell.Township;
        CellRule = hubCell.CellRule;
        HubRule = hubCell.HubRule;
        WildernessRule = hubCell.WildernessRule;
        Lots = new List<BCMLot>();
      }

      public BCMHubCell(RWG2.HubCell hubCell, Vector2i gridPos)
      {
        GridPos = new BCMVector2i(gridPos);
        CellRule = hubCell.CellRule.Name;
        HubRule = hubCell.HubRule.Name;
        WildernessRule = hubCell.WildernessRule.Name;
        Lots = new List<BCMLot>();

        //WILDERNESS LOTS
        var wildLots = hubCell.lots;
        if (wildLots != null)
        {
          foreach (var wl in wildLots)
          {
            Lots.Add(new BCMLot(wl, BCMLotType.Wilderness));
          }
        }

        //HUB LOTS
        var hubLots = new List<RWG2.HubCell.Lot>();
        if (hubCell.HubGenerator != null)
        {
          var lots = hubCell.HubGenerator.GetLots();
          if (lots != null)
          {
            hubLots = lots;
          }
          Township = hubCell.HubGenerator.township.ToString();
          //HubName = hubCell.HubGenerator.GetHubName();
        }
        foreach (var hl in hubLots)
        {
          Lots.Add(new BCMLot(hl, BCMLotType.Hub));
        }

      }
    }

    private readonly List<BCMHubCell> _hubCellData = new List<BCMHubCell>();
    private readonly List<Vector2i> _loaded = new List<Vector2i>();

    private void GetHubCellData()
    {
      GetCellsRange(out var r, out var xOff, out var yOff);

      var hcdDir = GameUtils.GetSaveGameDir() + "/HubCellData/";
      for (var x = -r + xOff; x <= r + xOff; x++)
      {
        for (var y = -r + yOff; y <= r + yOff; y++)
        {
          var gridPos = new Vector2i(x, y);
          if (_loaded.Contains(gridPos)) continue;

          var hcdFile = string.Format($"{hcdDir}{x}.{y}.hcd");
          if (!File.Exists(hcdFile)) continue;

          var binaryReader = new BinaryReader(File.OpenRead(hcdFile));
          var hubCell = new RWG2.HubCell(gridPos);
          hubCell.Read(binaryReader);
          binaryReader.Close();

          _loaded.Add(gridPos);
          _hubCellData.Add(new BCMHubCell(hubCell, gridPos));
        }
      }
    }

    private static void GetCellsRange(out int r, out int xOff, out int yOff)
    {
      r = 0;
      if (Options.ContainsKey("r"))
      {
        int.TryParse(Options["r"], out r);
      }
      xOff = 0;
      yOff = 0;
      if (!Options.ContainsKey("o")) return;

      var o = Options["o"].Split(',');
      if (o.Length != 2) return;

      int.TryParse(o[0], out xOff);
      int.TryParse(o[1], out yOff);
    }

    public override void Process()
    {
      if (Options.ContainsKey("reset"))
      {
        _hubCellData.Clear();
        _loaded.Clear();
      }

      GetHubCellData();
      var filteredHCD = new List<BCMHubCell>();

      if (Params.Count == 1)
      {
        //filter results with param text
        GetMinMax(out var minX, out var minY, out var maxX, out var maxY);

        foreach (var hubCell in _hubCellData)
        {
          if (!(minX <= hubCell.GridPos.x && hubCell.GridPos.x <= maxX &&
                minY <= hubCell.GridPos.y && hubCell.GridPos.y <= maxY))
          {
            continue;
          }

          var newHubCell = new BCMHubCell(hubCell);
          foreach (var lot in hubCell.Lots)
          {
            if (lot.Prefab.IndexOf(Params[0], StringComparison.OrdinalIgnoreCase) == -1) continue;

            newHubCell.Lots.Add(new BCMLot(lot));
          }
          filteredHCD.Add(newHubCell);
        }
      }
      else
      {
        filteredHCD = _hubCellData;
      }

      if (Options.ContainsKey("full"))
      {
        SendJson(filteredHCD);
      }
      else if (Options.ContainsKey("min"))
      {
        var keyless = new List<object>();
        foreach (var d in filteredHCD)
        {
          var obj = d.Lots.Select(l => $"{l.Prefab}:{l.Position}").Cast<object>().ToList();
          if (obj.Count <= 0) continue;

          keyless.AddRange(obj);
        }

        SendJson(keyless);

      }
      else
      {
        SendJson(filteredHCD.SelectMany(hubCell => hubCell.Lots, (hubCell, lot) => new BCMBasicLot(lot)).ToList());
      }

      GC.Collect();
      GC.WaitForPendingFinalizers();
    }

    private static void GetMinMax(out int minX, out int minY, out int maxX, out int maxY)
    {
      GetCellsRange(out var r, out var xOff, out var yOff);
      minX = -r + xOff;
      minY = -r + yOff;
      maxX = r + xOff;
      maxY = r + yOff;
    }
  }
}
