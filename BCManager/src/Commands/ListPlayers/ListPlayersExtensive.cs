using BCM.Models;
using System.Collections.Generic;

namespace BCM.Commands
{
  public class ListPlayersExtensive : ListPlayers
  {
    public override Dictionary<string, string> jsonPlayer(PlayerInfo _pInfo)
    {
      return null;
    }
    public override string displayPlayer(PlayerInfo _pInfo)
    {
      string output = "";
      output += new ClientInfoList(_pInfo, _options).Display(_sep);
      output += _sep;
      output += new StatsList(_pInfo, _options).Display(_sep);
      output += _sep;
      output += new BuffList(_pInfo, _options).Display(_sep);
      output += _sep;
      output += new SkillList(_pInfo, _options).Display(_sep);
      output += _sep;
      output += new QuestList(_pInfo, _options).Display(_sep);
      output += _sep;
      output += new SpawnpointList(_pInfo, _options).Display(_sep);
      output += _sep;
      output += new WaypointList(_pInfo, _options).Display(_sep);
      output += _sep;
      output += new FavoriteRecipeList(_pInfo, _options).Display(_sep);
      output += _sep;
      output += new UnlockedRecipeList(_pInfo, _options).Display(_sep);
      output += _sep;
      output += new CraftingQueue(_pInfo, _options).Display(_sep);
      output += _sep;
      output += new ToolbeltList(_pInfo, _options).Display(_sep);
      output += _sep;
      output += new EquipmentList(_pInfo, _options).Display(_sep);
      output += _sep;
      output += new BagList(_pInfo, _options).Display(_sep);
      output += _sep;

      // todo: 
      // friends
      // tracked friends // _pdf.trackedFriendEntityIds //List<int>
      // claims (_ppd.LPBlocks)
      // list owned storage containers and doors for the players

      return output;
    }
  }
}
