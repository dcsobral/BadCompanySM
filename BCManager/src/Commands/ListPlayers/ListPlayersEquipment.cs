using BCM.Models;
using System.Collections.Generic;

namespace BCM.Commands
{
  public class ListPlayersEquipment : ListPlayers
  {
    public override Dictionary<string, string> jsonPlayer(PlayerInfo _pInfo)
    {
      Dictionary<string, string> fullequipdict = null;
      EquipmentList el = new EquipmentList(_pInfo, _options);
      if (el != null)
      {
        fullequipdict = el.GetEquipment();
      }
      return fullequipdict;
    }
    public override string displayPlayer(PlayerInfo _pInfo)
    {
      string output = "";
      output += new ClientInfoList(_pInfo, _options).DisplayShort(_sep);
      output += _sep;
      output += new EquipmentList(_pInfo, _options).Display(_sep);

      return output;
    }
  }
}
