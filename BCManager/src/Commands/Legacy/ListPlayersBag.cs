//using BCM.Models;
//using System.Collections.Generic;

//namespace BCM.Commands
//{
//  public class ListPlayersBag : ListPlayers
//  {
//    public override Dictionary<string, string> jsonPlayer(PlayerInfo _pInfo)
//    {
//      Dictionary<string, string> fullbagdict = null;
//      BagList bl = new BagList(_pInfo, _options);
//      if (bl != null)
//      {
//        fullbagdict = bl.GetBag();
//      }
//      return fullbagdict;
//    }

//    public override string displayPlayer(PlayerInfo _pInfo)
//    {
//      string output = "";
//      output += new ClientInfoList(_pInfo, _options).DisplayShort(_sep);
//      output += _sep;
//      output += new BagList(_pInfo, _options).Display(_sep);

//      return output;
//    }
//  }
//}
