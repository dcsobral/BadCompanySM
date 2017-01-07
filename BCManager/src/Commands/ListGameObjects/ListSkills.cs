using System;
using System.Collections.Generic;
using System.Linq;

namespace BCM.Commands
{
  public class ListSkills : BCCommandAbstract
  {
    private static string OrderByCallback(Skill skill)
    {
      string name;
      if (!skill.IsPerk)
      {
        name = skill.Name;
      }
      else if (skill.SkillRequirements.Count > 0)
      {
        name = skill.SkillRequirements[0].SkillRequired;
      }
      else
      {
        name = skill.IsPerk.ToString();
      }
      return name;
    }

    public override void Process()
    {
      string output = "";
      IEnumerable<Skill> skills = Skills.AllSkills.Values;
      Func<Skill, string> skillnames = new Func<Skill, string>(OrderByCallback);
      List<Skill> sortedskills = skills.OrderBy(skillnames).ToList();
      foreach (Skill skill in sortedskills)
      {
        output += "'" + skill.Name + "'";
        if (_options.ContainsKey("details"))
        {
          output += "[Icon=" + skill.Icon + ",Group=" + skill.Group + ",Maxlvl=" + skill.MaxLevel + ",BaseExp=" + skill.BaseExpToLevel + ",ExpMult=" + skill.ExpMultiplier;
          if (skill.IsCrafting)
          {
            output += ",Type=Crafting";
          }
          if (skill.IsPassive)
          {
            output += ",Type=Passive";
          }
          if (skill.IsPerk)
          {
            output += ",Type=Perk";
          }
          output += "]";
        }
        output += _sep;
      }
      SendOutput(output);
    }
  }
}
