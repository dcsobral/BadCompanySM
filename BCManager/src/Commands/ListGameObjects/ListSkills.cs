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

    public virtual Dictionary<string, string> jsonObject()
    {
      Dictionary<string, string> data = new Dictionary<string, string>();

      IEnumerable<Skill> skills = Skills.AllSkills.Values;
      Func<Skill, string> skillnames = new Func<Skill, string>(OrderByCallback);
      List<Skill> sortedskills = skills.OrderBy(skillnames).ToList();

      var i = 0;
      foreach (Skill skill in sortedskills)
      {
        Dictionary<string, string> details = new Dictionary<string, string>();

        details.Add("Id", skill.Id.ToString());
        details.Add("Name", (skill.Name != null ? skill.Name : ""));
        details.Add("TitleKey", (skill.TitleKey != null ? skill.TitleKey : ""));
        details.Add("AlwaysFire", skill.AlwaysFire.ToString());
        details.Add("IsCrafting", skill.IsCrafting.ToString());
        details.Add("IsDefaultValues", skill.IsDefaultValues.ToString());
        details.Add("IsLocked", skill.IsLocked.ToString());
        details.Add("IsPassive", skill.IsPassive.ToString());
        details.Add("IsPerk", skill.IsPerk.ToString());
        details.Add("BaseExpToLevel", skill.BaseExpToLevel.ToString());
        details.Add("MaxLevel", skill.MaxLevel.ToString());
        details.Add("SkillPointCostPerLevel", skill.SkillPointCostPerLevel.ToString());
        details.Add("ExpMultiplier", skill.ExpMultiplier.ToString());
        details.Add("SkillPointCostMultiplier", skill.SkillPointCostMultiplier.ToString());
        details.Add("Group", (skill.Group != null ? skill.Group : ""));
        details.Add("Icon", (skill.Icon != null ? skill.Icon : ""));
        details.Add("DescriptionKey", (skill.DescriptionKey != null ? skill.DescriptionKey : ""));

        //EFFECTS
        List<string> effects = new List<string>();
        if (skill.effects != null)
        {
          foreach (Skill.Effect skilleffect in skill.effects.Values)
          {
            Dictionary<string, string> effect = new Dictionary<string, string>();

            List<string> modifiers = new List<string>();
            if (skilleffect != null && skilleffect.Modifiers != null)
            {
              foreach (Skill.IModifier modifier in skilleffect.Modifiers)
              {
                Dictionary<string, string> _modifier = new Dictionary<string, string>();
                _modifier.Add("MinLevel", modifier.MinLevel.ToString());
                _modifier.Add("MaxLevel", modifier.MaxLevel.ToString());
                _modifier.Add("MinValue", modifier.MinValue.ToString());
                _modifier.Add("MaxValue", modifier.MaxValue.ToString());
                string jsonModifier = BCUtils.toJson(_modifier);
                modifiers.Add(jsonModifier);
              }
            }
            string jsonModifiers = BCUtils.toJson(modifiers);
            effect.Add("Name", (skilleffect.Name != null ? skilleffect.Name : ""));
            effect.Add("Modifiers", jsonModifiers);
            string jsonEffect = BCUtils.toJson(effect);
            effects.Add(jsonEffect);
          }
        }
        string jsonEffects = BCUtils.toJson(effects);
        details.Add("effects", jsonEffects);

        //LOCKEDITEMS
        List<string> lockedItems = new List<string>();
        if (skill.LockedItems != null)
        {
          foreach (Skill.LockedItem item in skill.LockedItems)
          {
            Dictionary<string, string> lockedItem = new Dictionary<string, string>();
            lockedItem.Add("Name", (item.Name != null ? item.Name : ""));
            lockedItem.Add("UnlockLevel", item.UnlockLevel.ToString());
            string jsonLockedRecipe = BCUtils.toJson(lockedItem);
            lockedItems.Add(jsonLockedRecipe);
          }
        }
        string jsonLockedItems = BCUtils.toJson(lockedItems);
        details.Add("LockedItems", jsonLockedItems);

        //SKILLREQUIREMENTS
        List<string> skillRequirements = new List<string>();
        if (skill.SkillRequirements != null)
        {
          foreach (Skill.Requirement req in skill.SkillRequirements)
          {
            Dictionary<string, string> skillRequirement = new Dictionary<string, string>();
            skillRequirement.Add("SkillRequired", (req.SkillRequired != null ? req.SkillRequired : ""));
            skillRequirement.Add("SkillLevelRequired", req.SkillLevelRequired.ToString());
            skillRequirement.Add("Level", req.Level.ToString());
            string jsonSkillRequirement = BCUtils.toJson(skillRequirement);
            skillRequirements.Add(jsonSkillRequirement);
          }
        }
        string jsonSkillRequirements = BCUtils.toJson(skillRequirements);
        details.Add("SkillRequirements", jsonSkillRequirements);


        string jsonDetails = BCUtils.toJson(details);
        data.Add(i.ToString(), jsonDetails);
        i++;
      }

      return data;
    }

    public override void Process()
    {
      string output = "";
      if (_options.ContainsKey("json"))
      {
        if (_options.ContainsKey("tag"))
        {
          if (_options["tag"] == null)
          {
            _options["tag"] = "bc-skills";
          }

          SendOutput("{\"tag\":\"" + _options["tag"] + "\",\"data\":" + BCUtils.toJson(jsonObject()) + "}");
        }
        else
        {
          SendOutput(BCUtils.toJson(jsonObject()));
        }
      }
      else
      {
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
}
