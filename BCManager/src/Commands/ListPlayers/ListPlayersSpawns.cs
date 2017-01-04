using BCM.Models;

namespace BCM.Commands
{
  public class ListPlayersSpawns : ListPlayers
  {
    public override void displayPlayer(PlayerInfo _pInfo)
    {
      string output = "\n";
      output += new ClientInfoList(_pInfo).DisplayShort();
      output += new SpawnpointList(_pInfo).Display();

      SendOutput(output);
    }
  }
}
