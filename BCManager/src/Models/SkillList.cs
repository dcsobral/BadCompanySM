using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BCM.Models
{
  [Serializable]
  public class SkillList
  {
    private List<Skill> skills = new List<Skill>();

    public SkillList()
    {
    }

    public SkillList(PlayerDataFile _pdf, EntityPlayer _pl)
    {
      Load(_pdf, _pl);
    }

    public void Load(PlayerDataFile _pdf, EntityPlayer _pl)
    {
      if (_pl != null)
      {
        skills = _pl.Skills.GetAllSkills();
      }
      else
      {
        // todo: add skills to persistent data, or find a way to read binary steam from save file
        skills = null;
      }
    }

    public string Display()
    {
      if (skills != null)
      {
        string output = "Skills={\n";
        bool first = true;
        foreach (Skill s in skills)
        {
          // Note: don't use s.TitleKey as it will break the command if there is no localisation for the skill
          if (!first) { output += ", "; } else { first = false; }
          output += " " + s.Name + ":" + s.Level + " +" + (s.PercentThisLevel * 100).ToString("0.0") + "%";
        }
        output += "\n}\n";
        return output;
      }
      return string.Empty;
    }

  }
}
