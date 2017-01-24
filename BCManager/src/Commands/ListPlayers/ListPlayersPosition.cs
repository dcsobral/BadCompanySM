using BCM.Models;
using System.Collections.Generic;

namespace BCM.Commands
{
  public class ListPlayersPosition : ListPlayers
  {
    public override Dictionary<string, string> jsonPlayer(PlayerInfo _pInfo)
    {
      return null;
    }
    public override string displayPlayer(PlayerInfo _pInfo)
    {
      string output = "";
      output += new ClientInfoList(_pInfo, _options).DisplayShortWithPos();
      
      return output;
    }
  }
}
