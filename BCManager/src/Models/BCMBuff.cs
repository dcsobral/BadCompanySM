using System;
using System.Collections.Generic;
using System.Linq;

namespace BCM.Models
{
  [Serializable]
  public class BCMBuff : BCMAbstract
  {
    #region Filters
    public static class StrFilters
    {
      public const string Id = "id";
      public const string Name = "name";
      public const string Duration = "duration";
      public const string DurMode = "durmode";
      public const string StackMode = "stackmode";
      public const string StackMax = "stackmax";
      public const string Icon = "icon";
      public const string BuffConditions = "buffcon";
      public const string DebuffConditions = "debuffcon";
      public const string Actions = "actions";
      public const string DebuffActions = "debuffactions";
      public const string Modifiers = "modifiers";
      public const string DebuffBuff = "debuffbuff";
      public const string DebuffBuffChance = "debuffbuffp";
      public const string ExpiryBuff = "expirybuff";
      public const string ExpiryBuffChance = "expirybuffp";
      public const string HitMasks = "hitmasks";
      public const string Requires = "requires";
      public const string Mutex = "mutex";
      public const string Description = "desc";
      public const string Tooltip = "tooltip";
      public const string Descriptor = "descriptor";
      public const string Cures = "cures";
      public const string Causes = "causes";
      public const string Smell = "smell";
      public const string CastSound = "castsound";
      public const string DebuffSound = "debuffsound";
      public const string ExpirySound = "expirysound";
      public const string CritOnly = "crit";
      public const string FFCheck = "ff";
    }

    private static Dictionary<int, string> _filterMap = new Dictionary<int, string>
    {
      {0, StrFilters.Id},
      {1, StrFilters.Name},
      {2, StrFilters.Duration},
      {3, StrFilters.DurMode},
      {4, StrFilters.StackMode},
      {5, StrFilters.StackMax},
      {6, StrFilters.Icon},
      {7, StrFilters.BuffConditions},
      {8, StrFilters.DebuffConditions},
      {9, StrFilters.Actions},
      {10, StrFilters.DebuffActions},
      {11, StrFilters.Modifiers},
      {12, StrFilters.DebuffBuff},
      {13, StrFilters.DebuffBuffChance},
      {14, StrFilters.ExpiryBuff},
      {15, StrFilters.ExpiryBuffChance},
      {16, StrFilters.HitMasks},
      {17, StrFilters.Requires},
      {18, StrFilters.Mutex},
      {19, StrFilters.Description},
      {20, StrFilters.Tooltip},
      {21, StrFilters.Descriptor},
      {22, StrFilters.Cures},
      {23, StrFilters.Causes},
      {24, StrFilters.Smell},
      {25, StrFilters.CastSound},
      {26, StrFilters.DebuffSound},
      {27, StrFilters.ExpirySound},
      {28, StrFilters.CritOnly},
      {29, StrFilters.FFCheck}
    };
    public static Dictionary<int, string> FilterMap => _filterMap;
    #endregion

    #region Properties
    public string Id;
    public string Name;
    public string Description;
    public string Tooltip;
    public string Icon;
    public string DebuffBuff;
    public string ExpiryBuff;
    public string CastSound;
    public string DebuffSound;
    public string ExpiredSound;
    public string Smell;
    public bool CritOnly;
    public bool FFCheck;
    public string Descriptor;
    public double Duration;
    public int StackMax;
    public string DurMode;
    public string StackMode;
    public double DebuffBuffChance;
    public double ExpiryBuffChance;
    public List<string> HitMasks = new List<string>();
    public List<string> Requires = new List<string>();
    public List<string> Actions = new List<string>();
    public List<string> DebuffActions = new List<string>();
    public List<string> Mutex = new List<string>();
    public List<string> Cures = new List<string>();
    public List<string> Causes = new List<string>();
    public List<BCMBuffCondition> BuffConditions = new List<BCMBuffCondition>();
    public List<BCMBuffCondition> DebuffConditions = new List<BCMBuffCondition>();
    public List<BCMBuffModifier> Modifiers = new List<BCMBuffModifier>();

    public class BCMBuffCondition
    {
      public string Counter;
      public string Type;
      public double Value;

      public BCMBuffCondition(MultiBuffClassCondition condition)
      {
        if (condition == null) return;

        Counter = condition.Counter;
        Type = condition.ConditionType.ToString();
        Value = condition.Value;
      }
    }

    public class BCMBuffModifier
    {
      public string Stat;
      public string Type;
      public string Cat;
      public int Max;
      public int IDur;
      public int UID;
      public double FDur;
      public double ValStart;
      public double ValEnd;
      public double Freq;
      public double ApplyTime;
      public string Target;

      public BCMBuffModifier(MultiBuffClass.Modifier modifier)
      {
        if (modifier == null) return;

        Stat = modifier.TargetStat.ToString();
        Type = modifier.TypeOfModifier.ToString();
        Cat = modifier.CategoryFlags.ToString();
        Max = modifier.ModifierStackMax;
        IDur = modifier.ModifierIDuration;
        UID = modifier.ModifierUID;
        FDur = Math.Round(modifier.ModifierFDuration, 6);
        ValStart = Math.Round(modifier.ModifierValueStart, 6);
        ValEnd = Math.Round(modifier.ModifierValueEnd, 6);
        Freq = Math.Round(modifier.ModifierFrequency, 6);
        ApplyTime = Math.Round(modifier.ModifierApplyTime, 6);
        Target = modifier.TargetBuff;
      }
    }
    #endregion;

    public BCMBuff(object obj, string typeStr, Dictionary<string, string> options, List<string> filters) : base(obj, typeStr, options, filters)
    {
    }

    public override void GetData(object obj)
    {
      var buff = obj as MultiBuffClass;
      if (buff == null) return;

      if (IsOption("filter"))
      {
        foreach (var f in StrFilter)
        {
          switch (f)
          {
            case StrFilters.Id:
              GetId(buff);
              break;
            case StrFilters.Name:
              GetName(buff);
              break;

            case StrFilters.Duration:
              GetDuration(buff);
              break;
            case StrFilters.DurMode:
              GetDurMode(buff);
              break;
            case StrFilters.StackMode:
              GetStackMode(buff);
              break;
            case StrFilters.StackMax:
              GetStackMax(buff);
              break;
            case StrFilters.Icon:
              GetIcon(buff);
              break;
            case StrFilters.BuffConditions:
              GetBuffConditions(buff);
              break;
            case StrFilters.DebuffConditions:
              GetDebuffConditions(buff);
              break;
            case StrFilters.Actions:
              GetActions(buff);
              break;
            case StrFilters.DebuffActions:
              GetDebuffActions(buff);
              break;
            case StrFilters.Modifiers:
              GetModifiers(buff);
              break;
            case StrFilters.DebuffBuff:
              GetDebuffBuff(buff);
              break;
            case StrFilters.DebuffBuffChance:
              GetDebuffBuffChance(buff);
              break;
            case StrFilters.ExpiryBuff:
              GetExpiryBuff(buff);
              break;
            case StrFilters.ExpiryBuffChance:
              GetExpiryBuffChance(buff);
              break;
            case StrFilters.HitMasks:
              GetHitMasks(buff);
              break;
            case StrFilters.Requires:
              GetRequires(buff);
              break;
            case StrFilters.Mutex:
              GetMutex(buff);
              break;
            case StrFilters.Description:
              GetDescription(buff);
              break;
            case StrFilters.Tooltip:
              GetTooltip(buff);
              break;
            case StrFilters.Descriptor:
              GetDescriptor(buff);
              break;
            case StrFilters.Cures:
              GetCures(buff);
              break;
            case StrFilters.Causes:
              GetCauses(buff);
              break;
            case StrFilters.Smell:
              GetSmell(buff);
              break;
            case StrFilters.CastSound:
              GetCastSound(buff);
              break;
            case StrFilters.DebuffSound:
              GetDebuffSound(buff);
              break;
            case StrFilters.ExpirySound:
              GetExpirySound(buff);
              break;
            case StrFilters.CritOnly:
              GetCritOnly(buff);
              break;
            case StrFilters.FFCheck:
              GetFFCheck(buff);
              break;
            default:
              Log.Out($"{Config.ModPrefix} Unknown filter {f}");
              break;
          }
        }
      }
      else
      {
        GetId(buff);
        GetName(buff);
        if (!IsOption("full")) return;

        GetDuration(buff);
        GetDurMode(buff);
        GetStackMode(buff);
        GetStackMax(buff);
        GetIcon(buff);
        GetBuffConditions(buff);
        GetDebuffConditions(buff);
        GetActions(buff);
        GetDebuffActions(buff);
        GetModifiers(buff);
        GetDebuffBuff(buff);
        GetDebuffBuffChance(buff);
        GetExpiryBuff(buff);
        GetExpiryBuffChance(buff);
        GetHitMasks(buff);
        GetRequires(buff);
        GetMutex(buff);
        GetDescription(buff);
        GetTooltip(buff);
        GetDescriptor(buff);
        GetCures(buff);
        GetCauses(buff);
        GetSmell(buff);
        GetCastSound(buff);
        GetDebuffSound(buff);
        GetExpirySound(buff);
        GetCritOnly(buff);
        GetFFCheck(buff);
      }
    }

    private void GetModifiers(MultiBuffClass buff)
    {
      if (buff.Modifiers != null)
      {
        foreach (var modifier in buff.Modifiers)
        {
          Modifiers.Add(new BCMBuffModifier(modifier));
        }
      }
      Bin.Add("Modifiers", Modifiers);
    }

    private void GetDebuffConditions(MultiBuffClass buff)
    {
      if (buff.DebuffConditions != null)
      {
        foreach (var key in buff.DebuffConditions.Keys)
        {
          DebuffConditions.Add(new BCMBuffCondition(buff.DebuffConditions[key]));
        }
      }
      Bin.Add("DebuffConditions", DebuffConditions);
    }

    private void GetBuffConditions(MultiBuffClass buff)
    {
      if (buff.BuffConditions != null)
      {
        foreach (var key in buff.BuffConditions.Keys)
        {
          BuffConditions.Add(new BCMBuffCondition(buff.BuffConditions[key]));
        }
      }
      Bin.Add("BuffConditions", BuffConditions);
    }

    private void GetCauses(MultiBuffClass buff) => Bin.Add("Causes", Causes = buff.Causes);

    private void GetCures(MultiBuffClass buff) => Bin.Add("Cures", Cures = buff.Cures);

    private void GetMutex(MultiBuffClass buff) => Bin.Add("Mutex", Mutex = buff.Mutex?.ToList());

    private void GetDebuffActions(MultiBuffClass buff) => Bin.Add("DebuffActions", DebuffActions = buff.DebuffActions?.ToList());

    private void GetActions(MultiBuffClass buff) => Bin.Add("Actions", Actions = buff.Actions?.ToList());

    private void GetRequires(MultiBuffClass buff) => Bin.Add("Requires", Requires = buff.Requires?.ToList());

    private void GetHitMasks(MultiBuffClass buff)
    {
      if (buff.HitLocationMasks != null)
      {
        foreach (var mask in buff.HitLocationMasks)
        {
          HitMasks.Add(mask.ToString());
        }
      }
      Bin.Add("HitMasks", HitMasks);
    }

    private void GetExpiryBuffChance(MultiBuffClass buff) => Bin.Add("ExpiryBuffChance", ExpiryBuffChance = buff.ExpiryBuffChance);

    private void GetDebuffBuffChance(MultiBuffClass buff) => Bin.Add("DebuffBuffChance", DebuffBuffChance = buff.DebuffBuffChance);

    private void GetStackMode(MultiBuffClass buff) => Bin.Add("StackMode", StackMode = buff.StackMode.ToString());

    private void GetDurMode(MultiBuffClass buff) => Bin.Add("DurMode", DurMode = buff.DurationMode.ToString());

    private void GetStackMax(MultiBuffClass buff) => Bin.Add("StackMax", StackMax = buff.StackMax);

    private void GetDuration(MultiBuffClass buff) => Bin.Add("Duration", Duration = Math.Round(buff.FDuration, 6));

    private void GetDescriptor(MultiBuffClass buff)
    {
      //todo: 
      Bin.Add("Descriptor", Descriptor = buff.Descriptor?.NotificationClass);
      //public string NotificationClass;
      //public EnumBuffCategoryFlags CategoryFlags;
      //public HashSet<string> Overrides;
    }

    private void GetFFCheck(MultiBuffClass buff) => Bin.Add("FFCheck", FFCheck = buff.FriendlyFireCheck);

    private void GetCritOnly(MultiBuffClass buff) => Bin.Add("CritOnly", CritOnly = buff.CriticalHitOnly);

    private void GetSmell(MultiBuffClass buff) => Bin.Add("Smell", Smell = buff.SmellName);

    private void GetExpirySound(MultiBuffClass buff) => Bin.Add("ExpiredSound", ExpiredSound = buff.ExpiredSound);

    private void GetDebuffSound(MultiBuffClass buff) => Bin.Add("DebuffSound", DebuffSound = buff.DebuffSound);

    private void GetCastSound(MultiBuffClass buff) => Bin.Add("CastSound", CastSound = buff.CastSound);

    private void GetExpiryBuff(MultiBuffClass buff) => Bin.Add("ExpiryBuff", ExpiryBuff = buff.ExpiryBuff);

    private void GetDebuffBuff(MultiBuffClass buff) => Bin.Add("DebuffBuff", DebuffBuff = buff.DebuffBuff);

    private void GetIcon(MultiBuffClass buff) => Bin.Add("Icon", Icon = buff.Icon);

    private void GetTooltip(MultiBuffClass buff) => Bin.Add("Tooltip", Tooltip = buff.Tooltip);

    private void GetDescription(MultiBuffClass buff) => Bin.Add("Description", Description = buff.Description);

    private void GetName(MultiBuffClass buff) => Bin.Add("Name", Name = buff.Name);

    private void GetId(MultiBuffClass buff) => Bin.Add("Id", Id = buff.Id);
  }
}
