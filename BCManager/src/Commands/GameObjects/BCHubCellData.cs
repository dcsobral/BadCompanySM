using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BCM.Models;

namespace BCM.Commands
{
  public class BCHubCellData : BCCommandAbstract
  {
    private static readonly List<BCMHubCell> _hubCellData = new List<BCMHubCell>();
    private static readonly List<Vector2i> _loaded = new List<Vector2i>();

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

    private static void GetHubCellData()
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
