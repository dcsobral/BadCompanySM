using System;
using System.Collections.Generic;

namespace BCM.Models
{
  [Serializable]
  public class SkillList : AbstractList
  {
    private List<Skill> skills = new List<Skill>();
    private string points;

    public SkillList(PlayerInfo _pInfo, Dictionary<string, string> _options) : base(_pInfo, _options)
    {
    }

    public override void Load(PlayerInfo _pInfo)
    {
      points = _pInfo.PDF.skillPoints.ToString();
      if (_pInfo.EP != null)
      {
        skills = _pInfo.EP.Skills.GetAllSkills();
      }
      else
      {
        // todo: add skills to persistent data, or find a way to read binary steam from save file
        skills = null;
      }
    }

    public override string Display(string sep = " ")
    {
      string output = "SkillPoints:" + points + sep;
      if (skills != null)
      {
        output += "Skills:{";
        bool first = true;
        foreach (Skill s in skills)
        {
          // Note: don't use s.TitleKey as it will break the command if there is no localisation for the skill
          if (!first) { output += sep; } else { first = false; }
          output += " " + s.Name + ":" + s.Level + " +" + (s.PercentThisLevel * 100).ToString("0.0") + "%";
        }
        output += "}" + sep;

        return output;
      }
      return string.Empty;
    }

  }
}
