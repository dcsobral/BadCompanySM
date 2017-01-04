using BCM.Models;

namespace BCM.Commands
{
  public class ListPlayersSkills : ListPlayers
  {
    public override void displayPlayer(PlayerInfo _pInfo)
    {
      string output = "\n";
      output += new ClientInfoList(_pInfo).DisplayShort();
      output += new SkillList(_pInfo).Display();

      SendOutput(output);
    }
  }
}
