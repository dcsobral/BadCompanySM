using BCM.Models;
using System.Collections.Generic;

namespace BCM.Commands
{
  public class ListPlayersSkills : ListPlayers
  {
    public override Dictionary<string, string> jsonPlayer(PlayerInfo _pInfo)
    {
      SkillList sl = new SkillList(_pInfo, _options);
      Dictionary<string, string> data = sl.GetSkillPoints();
      Dictionary<string, string> skills = sl.GetSkills();
      foreach (string key in skills.Keys)
      {
        data.Add(key, skills[key]);
      }

      return data;
    }
    public override string displayPlayer(PlayerInfo _pInfo)
    {
      string output = "";
      output += new ClientInfoList(_pInfo, _options).DisplayShort(_sep);
      output += _sep;
      output += new SkillList(_pInfo, _options).Display(_sep);

      return output;
    }
  }
}
