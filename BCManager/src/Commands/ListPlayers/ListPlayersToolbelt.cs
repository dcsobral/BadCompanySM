using BCM.Models;
using System.Collections.Generic;

namespace BCM.Commands
{
  public class ListPlayersToolbelt : ListPlayers
  {
    public override Dictionary<string, string> jsonPlayer(PlayerInfo _pInfo)
    {
      Dictionary<string, string> bl = null;
      ToolbeltList tbl = new ToolbeltList(_pInfo, _options);
      if (tbl != null)
      {
        bl = tbl.GetInventory();
        int slot = tbl.GetSelecteditemSlot();
        if (slot != -1) {
          bl.Add("SelectedSlot", slot.ToString());
        }
      }
      return bl;
    }
    public override string displayPlayer(PlayerInfo _pInfo)
    {
      string output = "";
      output += new ClientInfoList(_pInfo, _options).DisplayShort(_sep);
      output += _sep;
      output += new ToolbeltList(_pInfo, _options).Display(_sep);

      return output;
    }
  }
}
