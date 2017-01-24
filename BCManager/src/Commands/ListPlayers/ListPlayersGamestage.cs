using BCM.Models;
using System.Collections.Generic;

namespace BCM.Commands
{
  public class ListPlayersGamestage : ListPlayers
  {
    public override Dictionary<string, string> jsonPlayer(PlayerInfo _pInfo)
    {
      return null;
    }
    public override string displayPlayer(PlayerInfo _pInfo)
    {
      string output = "";
      output += new StatsList(_pInfo, _options).DisplayGamestage(_pInfo, _sep);
      if (output != "")
      {
        return output;
      }
      return string.Empty;
    }
  }
}
