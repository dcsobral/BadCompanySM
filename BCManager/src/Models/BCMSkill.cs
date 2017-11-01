using System;
using System.Collections.Generic;

namespace BCM.Models
{
  [Serializable]
  public class BCMSkill : BCMAbstract
  {
    #region Filters
    public static class StrFilters
    {
      public const string Id = "id";
      public const string Name = "name";
      public const string Local = "local";
      public const string ExpGainMult = "expgainmult";
      public const string Group = "group";
      public const string ExpToNext = "exptonext";
      public const string Level = "level";
      public const string IsLocked = "islocked";
      public const string TitleKey = "title";
      public const string DescKey = "desc";
      public const string Icon = "icon";
      public const string MaxLevel = "maxlevel";
      public const string BaseExpTo = "baseexp";
      public const string ExpMult = "expmult";
      public const string IsPassive = "passive";
      public const string IsPerk = "perk";
      public const string IsCrafting = "crafting";
      public const string AlwaysFire = "always";
      public const string CostPer = "costper";
      public const string CostMult = "costmult";
      public const string SkillReqs = "reqs";
      public const string LockedItems = "locked";
      public const string Effects = "effects";
    }

    private static Dictionary<int, string> _filterMap = new Dictionary<int, string>
    {
      { 0,  StrFilters.Id },
      { 1,  StrFilters.Name },
      { 2,  StrFilters.Local},
      { 3,  StrFilters.ExpGainMult},
      { 4,  StrFilters.Group},
      { 5,  StrFilters.ExpToNext},
      { 6,  StrFilters.Level},
      { 7,  StrFilters.IsLocked},
      { 8,  StrFilters.TitleKey},
      { 9,  StrFilters.DescKey},
      { 10,  StrFilters.Icon},
      { 11,  StrFilters.MaxLevel},
      { 12,  StrFilters.BaseExpTo},
      { 13,  StrFilters.ExpMult},
      { 14,  StrFilters.IsPassive},
      { 15,  StrFilters.IsPerk},
      { 16,  StrFilters.IsCrafting},
      { 17,  StrFilters.AlwaysFire},
      { 18,  StrFilters.CostPer},
      { 19,  StrFilters.CostMult},
      { 20,  StrFilters.SkillReqs},
      { 21,  StrFilters.LockedItems},
      { 22,  StrFilters.Effects}
    };
    public static Dictionary<int, string> FilterMap => _filterMap;
    #endregion

    #region Properties
    public int Id;
    public string Name;
    public string Local;
    public double ExpGainMult;
    public string Group;
    public int ExpToNext;
    public int Level;
    public bool IsLocked;
    public string TitleKey;
    public string DescKey;
    public string Icon;
    public int MaxLevel;
    public int BaseExpTo;
    public double ExpMult;
    public bool IsPassive;
    public bool IsPerk;
    public bool IsCrafting;
    public bool AlwaysFire;
    public int CostPer;
    public double CostMult;
    public List<BCMRequirement> SkillReqs = new List<BCMRequirement>();
    public List<BCMLockedItem> LockedItems = new List<BCMLockedItem>();
    public List<BCMEffect> Effects = new List<BCMEffect>();
    //public Skills Parent;

    public class BCMRequirement
    {
      public int Level;
      public string SkillRequired;
      public int SkillLevelRequired;

      public BCMRequirement(Skill.Requirement requirement)
      {
        Level = requirement.Level;
        SkillRequired = requirement.SkillRequired;
        SkillLevelRequired = requirement.SkillLevelRequired;
      }
    }
    public struct BCMLockedItem
    {
      public string Name;
      public int UnlockLevel;
      public string ItemType;

      public BCMLockedItem(Skill.LockedItem item)
      {
        Name = item.Name;
        UnlockLevel = item.UnlockLevel;
        ItemType = item.ItemType.ToString();
      }
    }

    public class BCMEffect
    {
      public string Name;
      public List<BCMModifier> Modifiers = new List<BCMModifier>();

      public BCMEffect(Skill.Effect effect)
      {
        Name = effect.Name;
        foreach (var modifier in effect.Modifiers)
        {
          Modifiers.Add(new BCMModifier(modifier));
        }
      }
    }
    public class BCMModifier
    {
      public int MinLevel;
      public int MaxLevel;
      public double MinValue;
      public double MaxValue;
      public BCMModifier(Skill.IModifier modifier)
      {
        MinLevel = modifier.MinLevel;
        MaxLevel = modifier.MaxLevel;
        MinValue = Math.Round(modifier.MinValue, 6);
        MaxValue = Math.Round(modifier.MaxValue, 6);
      }
    }

    #endregion;

    public BCMSkill(object obj, string typeStr, Dictionary<string, string> options, List<string> filters) : base(obj, typeStr, options, filters)
    {
    }

    public override void GetData(object obj)
    {
      var skill = obj as Skill;
      if (skill == null) return;

      if (IsOption("filter"))
      {
        foreach (var f in StrFilter)
        {
          switch (f)
          {
            case StrFilters.Id:
              GetId(skill);
              break;
            case StrFilters.Name:
              GetName(skill);
              break;
            case StrFilters.Local:
              GetLocalized(skill);
              break;
            case StrFilters.ExpGainMult:
              GetExpGainMult(skill);
              break;
            case StrFilters.Group:
              GetGroup(skill);
              break;
            case StrFilters.ExpToNext:
              GetExpToNext(skill);
              break;
            case StrFilters.Level:
              GetLevel(skill);
              break;
            case StrFilters.IsLocked:
              GetIsLocked(skill);
              break;
            case StrFilters.TitleKey:
              GetTitleKey(skill);
              break;
            case StrFilters.DescKey:
              GetDescKey(skill);
              break;
            case StrFilters.Icon:
              GetIcon(skill);
              break;
            case StrFilters.MaxLevel:
              GetMaxLevel(skill);
              break;
            case StrFilters.BaseExpTo:
              GetBaseExpTo(skill);
              break;
            case StrFilters.ExpMult:
              GetExpMult(skill);
              break;
            case StrFilters.IsPassive:
              GetIsPassive(skill);
              break;
            case StrFilters.IsPerk:
              GetIsPerk(skill);
              break;
            case StrFilters.IsCrafting:
              GetIsCrafting(skill);
              break;
            case StrFilters.AlwaysFire:
              GetAlwaysFire(skill);
              break;
            case StrFilters.CostPer:
              GetCostPer(skill);
              break;
            case StrFilters.CostMult:
              GetCostMult(skill);
              break;
            case StrFilters.SkillReqs:
              GetSkillReqs(skill);
              break;
            case StrFilters.LockedItems:
              GetLockedItems(skill);
              break;
            case StrFilters.Effects:
              GetEffects(skill);
              break;
            default:
              Log.Out($"{Config.ModPrefix} Unknown filter {f}");
              break;
          }
        }
      }
      else
      {
        GetId(skill);
        GetName(skill);
        GetLocalized(skill);
        GetExpGainMult(skill);
        GetGroup(skill);
        GetExpToNext(skill);
        GetLevel(skill);
        GetIsLocked(skill);
        GetTitleKey(skill);
        GetDescKey(skill);
        GetIcon(skill);
        GetMaxLevel(skill);
        GetBaseExpTo(skill);
        GetExpMult(skill);
        GetIsPassive(skill);
        GetIsPerk(skill);
        GetIsCrafting(skill);
        GetAlwaysFire(skill);
        GetCostPer(skill);
        GetCostMult(skill);
        GetSkillReqs(skill);
        GetLockedItems(skill);
        GetEffects(skill);
      }
    }

    private void GetEffects(Skill skill)
    {
      if (skill.effects != null)
      {
        foreach (var effect in skill.effects)
        {
          Effects.Add(new BCMEffect(effect.Value));
        }
      }
      Bin.Add("Effects", Effects);
    }

    private void GetLockedItems(Skill skill)
    {
      if (skill.LockedItems != null)
      {
        foreach (var item in skill.LockedItems)
        {
          LockedItems.Add(new BCMLockedItem(item));
        }
      }
      Bin.Add("LockedItems", LockedItems);
    }

    private void GetSkillReqs(Skill skill)
    {
      if (skill.SkillRequirements != null)
      {
        foreach (var req in skill.SkillRequirements)
        {
          SkillReqs.Add(new BCMRequirement(req));
        }
      }
      Bin.Add("SkillReqs", SkillReqs);
    }

    private void GetCostMult(Skill skill)
    {
      Bin.Add("CostMult", CostMult = Math.Round(skill.SkillPointCostMultiplier, 6));
    }

    private void GetCostPer(Skill skill)
    {
      Bin.Add("CostPer", CostPer = skill.SkillPointCostPerLevel);
    }

    private void GetAlwaysFire(Skill skill)
    {
      Bin.Add("AlwaysFire", AlwaysFire = skill.AlwaysFire);
    }

    private void GetIsCrafting(Skill skill)
    {
      Bin.Add("IsCrafting", IsCrafting = skill.IsCrafting);
    }

    private void GetIsPerk(Skill skill)
    {
      Bin.Add("IsPerk", IsPerk = skill.IsPerk);
    }

    private void GetIsPassive(Skill skill)
    {
      Bin.Add("IsPassive", IsPassive = skill.IsPassive);
    }

    private void GetExpMult(Skill skill)
    {
      Bin.Add("ExpMult", ExpMult = Math.Round(skill.ExpMultiplier, 6));
    }

    private void GetBaseExpTo(Skill skill)
    {
      Bin.Add("BaseExpTo", BaseExpTo = skill.BaseExpToLevel);
    }

    private void GetMaxLevel(Skill skill)
    {
      Bin.Add("MaxLevel", MaxLevel = skill.MaxLevel);
    }

    private void GetIcon(Skill skill)
    {
      Bin.Add("Icon", Icon = skill.Icon);
    }

    private void GetDescKey(Skill skill)
    {
      Bin.Add("DescKey", DescKey = skill.DescriptionKey);
    }

    private void GetTitleKey(Skill skill)
    {
      Bin.Add("TitleKey", TitleKey = skill.TitleKey);
    }

    private void GetIsLocked(Skill skill)
    {
      Bin.Add("IsLocked", IsLocked = skill.IsLocked);
    }

    private void GetLevel(Skill skill)
    {
      Bin.Add("Level", Level = skill.Level);
    }

    private void GetExpToNext(Skill skill)
    {
      Bin.Add("ExpToNext", ExpToNext = skill.ExpToNextLevel);
    }

    private void GetGroup(Skill skill)
    {
      Bin.Add("Group", Group = skill.Group);
    }

    private void GetExpGainMult(Skill skill)
    {
      Bin.Add("ExpGainMult", ExpGainMult = Math.Round(skill.ExpGainMultiplier, 6));
    }

    private void GetLocalized(Skill skill)
    {
      Bin.Add("Localized", Local = skill.LocalizedName);
    }

    private void GetName(Skill skill) => Bin.Add("Name", Name = skill.Name);

    private void GetId(Skill skill) => Bin.Add("Id", Id = skill.Id);
  }
}
