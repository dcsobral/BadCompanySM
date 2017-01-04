using BCM.Models;

namespace BCM.Commands
{
  public class ListPlayersExtensive : ListPlayers
  {
    public override void displayPlayer(PlayerInfo _pInfo)
    {
      string output = "\n";
      output += new ClientInfoList(_pInfo).Display();
      output += new StatsList(_pInfo).Display();

      output += new BuffList(_pInfo).Display();

      output += new SkillList(_pInfo).Display();

      output += new QuestList(_pInfo).Display();

      output += new SpawnpointList(_pInfo).Display();

      output += new WaypointList(_pInfo).Display();

      output += new FavoriteRecipeList(_pInfo).Display();
      output += new UnlockedRecipeList(_pInfo).Display();
      output += new CraftingQueue(_pInfo).Display();

      output += new ToolbeltList(_pInfo).Display();
      output += new EquipmentList(_pInfo).Display();
      output += new BagList(_pInfo).Display();

      // todo: 
      // friends
      // tracked friends // _pdf.trackedFriendEntityIds //List<int>
      // claims (_ppd.LPBlocks)
      // list owned storage containers and doors for the players

      SendOutput(output);
    }
  }
}
