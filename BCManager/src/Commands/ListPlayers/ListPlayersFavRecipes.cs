using BCM.Models;

namespace BCM.Commands
{
  public class ListPlayersFavRecipes : ListPlayers
  {
    public override void displayPlayer(PlayerInfo _pInfo)
    {
      string output = "";
      output += new ClientInfoList(_pInfo, _options).DisplayShort(_sep);
      output += new FavoriteRecipeList(_pInfo, _options).Display(_sep);

      SendOutput(output);
    }
  }
}
