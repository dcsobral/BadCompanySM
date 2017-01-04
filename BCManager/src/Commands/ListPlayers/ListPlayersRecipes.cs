using BCM.Models;

namespace BCM.Commands
{
  public class ListPlayersRecipes : ListPlayers
  {
    public override void displayPlayer(PlayerInfo _pInfo)
    {
      string output = "\n";
      output += new ClientInfoList(_pInfo).DisplayShort();
      output += new UnlockedRecipeList(_pInfo).Display();

      SendOutput(output);
    }
  }
}
