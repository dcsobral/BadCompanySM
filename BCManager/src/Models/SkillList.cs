using System;
using System.Collections.Generic;

namespace BCM.Models
{
  [Serializable]
  public class SkillList
  {
    private List<Skill> skills = new List<Skill>();
    private string points;

    public SkillList()
    {
    }

    public SkillList(PlayerInfo _pInfo)
    {
      Load(_pInfo);
    }

    public void Load(PlayerInfo _pInfo)
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

    public string Display()
    {
      string output = "SkillPoints:" + points + "\n";
      if (skills != null)
      {
        output += "Skills={\n";
        bool first = true;
        foreach (Skill s in skills)
        {
          // Note: don't use s.TitleKey as it will break the command if there is no localisation for the skill
          if (!first) { output += ",\n"; } else { first = false; }
          output += " " + s.Name + ":" + s.Level + " +" + (s.PercentThisLevel * 100).ToString("0.0") + "%";
        }
        output += "\n}\n";
        return output;
      }
      return string.Empty;
    }

  }
}
