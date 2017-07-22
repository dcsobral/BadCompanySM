//using BCM.Models;
//using BCM.PersistentData;
//using System.Collections.Generic;

//namespace BCM.Commands
//{
//  public class ListPlayersId : ListPlayers
//  {
//    public override Dictionary<string, string> jsonPlayer(string _steamId)
//    {
//      Dictionary<string, string> data = new Dictionary<string, string>();

//      var player = PersistentContainer.Instance.Players[_steamId, false];
//      if (player != null)
//      {
//        data.Add("playerName", player.Name);
//        data.Add("lastOnline", player.LastOnline.ToString("u"));
//        data.Add("isOnline", player.IsOnline.ToString());
//        data.Add("steamId", _steamId);
//      }
//      else
//      {
//        data.Add("steamId", _steamId);
//      }

//      return data;
//    }

//    public override Dictionary<string, string> jsonPlayer(PlayerInfo _pInfo)
//    {
//      Dictionary<string, string> data = new Dictionary<string, string>();

//      Dictionary<string, string> info = new ClientInfoList(_pInfo, _options).GetInfo();
//      foreach (string key in info.Keys)
//      {
//        data.Add(key, info[key]);
//      }
//      return data;
//    }
//    public override string displayPlayer(PlayerInfo _pInfo)
//    {
//      string output = "";
//      output += new ClientInfoList(_pInfo, _options).DisplayShort(_sep);

//      return output;
//    }
//  }
//}
