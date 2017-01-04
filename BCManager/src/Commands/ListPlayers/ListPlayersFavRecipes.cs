using BCM.Models;

namespace BCM.Commands
{
  public class ListPlayersFavRecipes : ListPlayers
  {
    public override void displayPlayer(PlayerInfo _pInfo)
    {
      string output = "\n";
      output += new ClientInfoList(_pInfo).DisplayShort();
      output += new FavoriteRecipeList(_pInfo).Display();

      SendOutput(output);
    }
  }
}
