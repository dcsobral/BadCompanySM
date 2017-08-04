//using System;
//using System.Collections.Generic;

//namespace BCM.Models.Legacy
//{
//  [Serializable]
//  public class SkillList : AbstractList
//  {
//    private List<Skill> skills = new List<Skill>();
//    private string points;

//    public SkillList(PlayerInfo _pInfo, Dictionary<string, string> _options) : base(_pInfo, _options)
//    {
//    }

//    public override void Load(PlayerInfo _pInfo)
//    {
//      points = _pInfo.PDF.skillPoints.ToString();
//      if (_pInfo.EP != null)
//      {
//        skills = _pInfo.EP.Skills.GetAllSkills();
//      }
//      else
//      {
//        skills = _pInfo.PDF.skills;
//      }
//    }

//    public override string Display(string sep = " ")
//    {
//      string output = "SkillPoints:" + points + sep;
//      if (skills != null)
//      {
//        output += "Skills:{";
//        bool first = true;
//        foreach (Skill s in skills)
//        {
//          // Note: don't use s.TitleKey as it will break the command if there is no localisation for the skill
//          if (!first) { output += sep; } else { first = false; }
//          //workaround for some skills missing the Level setting
//          int lvl;
//          try
//          {
//            lvl = s.Level;
//          }
//          catch
//          {
//            lvl = 0;
//          }
//          output += " " + s.Name + ":" + lvl + " +" + (s.PercentThisLevel * 100).ToString("0.0") + "%";
//        }
//        output += "}";

//        return output;
//      }
//      return string.Empty;
//    }
//    public Dictionary<string, string> GetSkills()
//    {
//      Dictionary<string, string> skillsDict = new Dictionary<string, string>();
//      foreach (Skill s in skills)
//      {
//        // check for null and empty skill names in case a skill was removed that is in player data
//        if (s.Name != null && s.Name.Length > 0)
//        {
//          int lvl;
//          try
//          {
//            lvl = s.Level;
//          }
//          catch
//          {
//            lvl = 0;
//          }
//          float skill = lvl + s.PercentThisLevel;
//          skillsDict.Add(s.Name, skill.ToString());
//        }
//      }

//      return skillsDict;
//    }
//    public Dictionary<string, string> GetSkillPoints()
//    {
//      Dictionary<string, string> sp = new Dictionary<string, string>();
//      sp.Add("SkillPoints", points);
//      return sp;
//    }
//  }
//}
