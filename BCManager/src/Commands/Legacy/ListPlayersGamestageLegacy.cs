//using BCM.Models;
//using System.Collections.Generic;

//namespace BCM.Commands
//{
//  public class ListPlayersGamestageLegacy : ListPlayers
//  {
//    public override Dictionary<string, string> jsonPlayer(PlayerInfo _pInfo)
//    {
//      Dictionary<string, string> data = new Dictionary<string, string>();

//      Dictionary<string, string> stats = new StatsList(_pInfo, _options).GetStats();
//      data.Add(_pInfo._steamId, stats["Gamestage"]);

//      return data;
//    }
//    public override string displayPlayer(PlayerInfo _pInfo)
//    {
//      string output = "";
//      output += new StatsList(_pInfo, _options).DisplayGamestage(_pInfo, _sep);
//      if (output != "")
//      {
//        return output;
//      }
//      return string.Empty;
//    }
//  }
//}
