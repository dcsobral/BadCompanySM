//using BCM.Models;
//using System.Collections.Generic;

//namespace BCM.Commands
//{
//  public class ListPlayersSpawns : ListPlayers
//  {
//    public override Dictionary<string, string> jsonPlayer(PlayerInfo _pInfo)
//    {
//      Dictionary<string, string> spawnpoints = new SpawnpointList(_pInfo, _options).GetSpawnpoints();
//      return spawnpoints;
//    }
//    public override string displayPlayer(PlayerInfo _pInfo)
//    {
//      string output = "";
//      output += new ClientInfoList(_pInfo, _options).DisplayShort(_sep);
//      output += _sep;
//      output += new SpawnpointList(_pInfo, _options).Display(_sep);

//      return output;
//    }
//  }
//}
