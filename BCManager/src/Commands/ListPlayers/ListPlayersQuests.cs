using BCM.Models;
using System.Collections.Generic;

namespace BCM.Commands
{
  public class ListPlayersQuests : ListPlayers
  {
    public override Dictionary<string, string> jsonPlayer(PlayerInfo _pInfo)
    {
      Dictionary<string, string> fullquestdict = null;
      QuestList ql = new QuestList(_pInfo, _options);
      if (ql != null)
      {
        fullquestdict = ql.GetQuests();
      }
      return fullquestdict;
    }
    public override string displayPlayer(PlayerInfo _pInfo)
    {
      string output = "";
      output += new ClientInfoList(_pInfo, _options).DisplayShort(_sep);
      output += _sep;
      output += new QuestList(_pInfo, _options).Display(_sep);

      return output;
    }
  }
}
