using System.Collections.Generic;
using JetBrains.Annotations;

namespace BCM.Models
{
  public class BCMHubCell
  {
    [UsedImplicitly] public BCMVector2 GridPos;
    [UsedImplicitly] public string Township;
    [UsedImplicitly] public string CellRule;
    [UsedImplicitly] public string HubRule;
    [UsedImplicitly] public string WildernessRule;
    [NotNull] [UsedImplicitly] public List<BCMLot> Lots = new List<BCMLot>();

    public BCMHubCell(BCMHubCell hubCell)
    {
      GridPos = hubCell.GridPos;
      Township = hubCell.Township;
      CellRule = hubCell.CellRule;
      HubRule = hubCell.HubRule;
      WildernessRule = hubCell.WildernessRule;
    }

    public BCMHubCell([NotNull] RWG2.HubCell hubCell, Vector2i gridPos)
    {
      GridPos = new BCMVector2(gridPos);
      CellRule = hubCell.CellRule.Name;
      HubRule = hubCell.HubRule.Name;
      WildernessRule = hubCell.WildernessRule.Name;

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
      }
      foreach (var hl in hubLots)
      {
        Lots.Add(new BCMLot(hl, BCMLotType.Hub));
      }
    }
  }
}