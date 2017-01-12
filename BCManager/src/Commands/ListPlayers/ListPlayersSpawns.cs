using BCM.Models;

namespace BCM.Commands
{
  public class ListPlayersSpawns : ListPlayers
  {
    public override void displayPlayer(PlayerInfo _pInfo)
    {
      string output = "";
      output += new ClientInfoList(_pInfo, _options).DisplayShort(_sep);
      output += _sep;
      output += new SpawnpointList(_pInfo, _options).Display(_sep);

      SendOutput(output);
    }
  }
}
