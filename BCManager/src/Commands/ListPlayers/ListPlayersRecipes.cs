using BCM.Models;

namespace BCM.Commands
{
  public class ListPlayersRecipes : ListPlayers
  {
    public override void displayPlayer(PlayerInfo _pInfo)
    {
      string output = "";
      output += new ClientInfoList(_pInfo, _options).DisplayShort(_sep);
      output += new UnlockedRecipeList(_pInfo, _options).Display(_sep);

      SendOutput(output);
    }
  }
}
