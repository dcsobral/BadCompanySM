using BCM.Models;

namespace BCM.Commands
{
  public class ListPlayersEquipment : ListPlayers
  {
    public override void displayPlayer(PlayerInfo _pInfo)
    {
      string output = "\n";
      output += new ClientInfoList(_pInfo).DisplayShort();
      output += new EquipmentList(_pInfo).Display();

      SendOutput(output);
    }
  }
}
