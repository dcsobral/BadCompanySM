using BCM.Models;
using System.Collections.Generic;

namespace BCM.Commands
{
  public class ListPlayersClientInfo : ListPlayers
  {
    public override Dictionary<string, string> jsonPlayer(PlayerInfo _pInfo)
    {
      Dictionary<string, string> info = new ClientInfoList(_pInfo, _options).GetInfo();
      return info;
    }
    public override string displayPlayer(PlayerInfo _pInfo)
    {
      string output = "";
      output += new ClientInfoList(_pInfo, _options).Display(_sep);

      return output;
    }
  }
}
