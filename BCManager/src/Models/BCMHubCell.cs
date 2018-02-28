using System.Collections.Generic;

namespace BCM.Models
{
  public class BCMHubCell
  {
    public BCMVector2 GridPos;
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
      GridPos = new BCMVector2(gridPos);
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
}