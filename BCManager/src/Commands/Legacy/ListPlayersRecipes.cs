//using BCM.Models;
//using System.Collections.Generic;

//namespace BCM.Commands
//{
//  public class ListPlayersRecipes : ListPlayers
//  {
//    public override Dictionary<string, string> jsonPlayer(PlayerInfo _pInfo)
//    {
//      Dictionary<string, string> recipes = new UnlockedRecipeList(_pInfo, _options).GetUnlockedRecipes();
//      return recipes;
//    }
//    public override string displayPlayer(PlayerInfo _pInfo)
//    {
//      string output = "";
//      output += new ClientInfoList(_pInfo, _options).DisplayShort(_sep);
//      output += _sep;
//      output += new UnlockedRecipeList(_pInfo, _options).Display(_sep);

//      return output;
//    }
//  }
//}
