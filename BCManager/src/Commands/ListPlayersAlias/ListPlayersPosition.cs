//using BCM.Models;
//using System;
//using System.Collections.Generic;
//using UnityEngine;

//namespace BCM.Commands
//{
//  public class ListPlayersPosition : ListPlayers
//  {
//    public override Dictionary<string, string> jsonPlayer(PlayerInfo _pInfo)
//    {
//      var data = new Dictionary<string, string>();
//      data.Add("error", "Not Implemented");
//      return data;
//    }
//    public override string displayPlayer(PlayerInfo _pInfo)
//    {
//      if (_options.ContainsKey("fast"))
//      {
//        Vector3 p;
//        p = _pInfo.EP != null ? _pInfo.EP.position : (_pInfo.PDF != null ? _pInfo.PDF.ecd.pos : new Vector3(int.MinValue, 0, int.MinValue));

//        return _pInfo._steamId + ":" + string.Format("{0} {1} {2}", (int)Math.Round(p.x), (int)Math.Round(p.y), (int)Math.Round(p.z));
//      }

//      string output = "";
//      output += new ClientInfoList(_pInfo, _options).DisplayShortWithPos();

//      return output;
//    }
//  }
//}
