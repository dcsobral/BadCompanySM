using System.Collections.Generic;

namespace BCM.Commands
{
  public class ListBuffs : BCCommandAbstract
  {
    public virtual Dictionary<string, string> jsonObject()
    {
      Dictionary<string, string> data = new Dictionary<string, string>();
      Dictionary<string, MultiBuffClass> multiBuffClasses = MultiBuffClass.s_classes;

      //for (var i = 0; i <= classes.Count - 1; i++)
      foreach (string key in multiBuffClasses.Keys)
      {
        Dictionary<string, string> details = new Dictionary<string, string>();

        details.Add("Id", (multiBuffClasses[key].Id != null ? multiBuffClasses[key].Id : ""));
        details.Add("Name", (multiBuffClasses[key].Name != null ? multiBuffClasses[key].Name : ""));
        details.Add("Icon", (multiBuffClasses[key].Icon != null ? multiBuffClasses[key].Icon : ""));
        details.Add("CastSound", (multiBuffClasses[key].CastSound != null ? multiBuffClasses[key].CastSound : ""));
        details.Add("CriticalHitOnly", multiBuffClasses[key].CriticalHitOnly.ToString());
        details.Add("DebuffBuffChance", multiBuffClasses[key].DebuffBuffChance.ToString());//all 1
        details.Add("DurationMode", multiBuffClasses[key].DurationMode.ToString());//automatic or manual
        details.Add("ExpiryBuffChance", multiBuffClasses[key].ExpiryBuffChance.ToString());//all 1
        details.Add("FDuration", multiBuffClasses[key].FDuration.ToString());
        details.Add("FriendlyFireCheck", multiBuffClasses[key].FriendlyFireCheck.ToString());//all true
        details.Add("IDuration", multiBuffClasses[key].IDuration.ToString());//all 0
        details.Add("StackMax", multiBuffClasses[key].StackMax.ToString());//all 1
        details.Add("StackMode", multiBuffClasses[key].StackMode.ToString());
        details.Add("DebuffBuff", (multiBuffClasses[key].DebuffBuff != null ? multiBuffClasses[key].DebuffBuff : ""));
        details.Add("DebuffSound", (multiBuffClasses[key].DebuffSound != null ? multiBuffClasses[key].DebuffSound : ""));//all null
        details.Add("Description", (multiBuffClasses[key].Description != null ? multiBuffClasses[key].Description : ""));
        details.Add("ExpiredSound", (multiBuffClasses[key].ExpiredSound != null ? multiBuffClasses[key].ExpiredSound : ""));//all null
        details.Add("ExpiryBuff", (multiBuffClasses[key].ExpiryBuff != null ? multiBuffClasses[key].ExpiryBuff : ""));
        details.Add("SmellName", (multiBuffClasses[key].SmellName != null ? multiBuffClasses[key].SmellName : ""));
        details.Add("Tooltip", (multiBuffClasses[key].Tooltip != null ? multiBuffClasses[key].Tooltip : ""));
        details.Add("CategoryFlags", (multiBuffClasses[key].Descriptor != null ? multiBuffClasses[key].Descriptor.CategoryFlags.ToString() : ""));
        details.Add("NotificationClass", (multiBuffClasses[key].Descriptor != null ? multiBuffClasses[key].Descriptor.NotificationClass : ""));

        //BUFFCONDITIONS
        List<string> buffConditions = new List<string>();
        if (multiBuffClasses[key].BuffConditions != null)
        {
          foreach (string buffCondition_key in multiBuffClasses[key].BuffConditions.Keys)
          {
            var buffCondition = multiBuffClasses[key].BuffConditions[buffCondition_key];
            if (buffCondition != null)
            {
              buffConditions.Add("\"" + buffCondition.Counter + " " + buffCondition.ConditionType.ToString() + " " + buffCondition.Value.ToString() + "\"");
            }
          }
        }
        string jsonBuffConditions = BCUtils.toJson(buffConditions);
        details.Add("BuffConditions", jsonBuffConditions);

        //ACTIONS
        List<string> actions = new List<string>();
        foreach (string action in multiBuffClasses[key].Actions)
        {
          if (action != null)
          {
            actions.Add("\"" + action + "\"");
          }
        }
        string jsonActions = BCUtils.toJson(actions);
        details.Add("Actions", jsonActions);

        //DEBUFFCONDITIONS
        List<string> debuffConditions = new List<string>();
        if (multiBuffClasses[key].DebuffConditions != null)
        {
          foreach (string debuffCondition_key in multiBuffClasses[key].DebuffConditions.Keys)
          {
            var debuffCondition = multiBuffClasses[key].DebuffConditions[debuffCondition_key];
            if (debuffCondition != null)
            {
              debuffConditions.Add("\"" + debuffCondition.Counter + " " + debuffCondition.ConditionType.ToString() + " " + debuffCondition.Value.ToString() + "\"");
            }
          }
        }
        string jsonDebuffConditions = BCUtils.toJson(debuffConditions);
        details.Add("DebuffConditions", jsonDebuffConditions);

        //DEBUFF ACTIONS
        List<string> debuffactions = new List<string>();
        foreach (string debuffaction in multiBuffClasses[key].DebuffActions)
        {
          if (debuffaction != null)
          {
            debuffactions.Add("\"" + debuffaction + "\"");
          }
        }
        string jsonDebuffActions = BCUtils.toJson(debuffactions);
        details.Add("DebuffActions", jsonDebuffActions);

        //MUTEX
        List<string> mutexs = new List<string>();
        foreach (string mutex in multiBuffClasses[key].Mutex)
        {
          if (mutex != null)
          {
            mutexs.Add("\"" + mutex + "\"");
          }
        }
        string jsonMutex = BCUtils.toJson(mutexs);
        details.Add("Mutex", jsonMutex);

        //HITLOCATIONMASKS
        List<string> hitLocationMasks = new List<string>();
        if (multiBuffClasses[key].HitLocationMasks != null)
        {
          foreach (EnumBodyPartHit hitLocationMask in multiBuffClasses[key].HitLocationMasks)
          {
            hitLocationMasks.Add("\"" + hitLocationMask.ToString() + "\"");
          }
        }
        string jsonHitLocationMasks = BCUtils.toJson(hitLocationMasks);
        details.Add("HitLocationMasks", jsonHitLocationMasks);

        //REQUIRES
        List<string> requires = new List<string>();
        if (multiBuffClasses[key].Requires != null)
        {
          foreach (string require in multiBuffClasses[key].Requires)
          {
            if (require != null)
            {
              requires.Add("\"" + require + "\"");
            }
          }
        }
        string jsonRequires = BCUtils.toJson(requires);
        details.Add("Requires", jsonRequires);

        //CURES
        List<string> cures = new List<string>();
        foreach (string cure in multiBuffClasses[key].Cures)
        {
          if (cure != null)
          {
            cures.Add("\"" + cure + "\"");
          }
        }
        string jsonCures = BCUtils.toJson(cures);
        details.Add("Cures", jsonCures);

        //CAUSES
        List<string> causes = new List<string>();
        foreach (string cause in multiBuffClasses[key].Causes)
        {
          if (cause != null)
          {
            causes.Add("\"" + cause + "\"");
          }
        }
        string jsonCauses = BCUtils.toJson(causes);
        details.Add("Causes", jsonCauses);

        //OVERRIDES
        List<string> overrides = new List<string>();
        if (multiBuffClasses[key].Descriptor.Overrides != null)
        {
          foreach (string _override in multiBuffClasses[key].Descriptor.Overrides)
          {
            if (_override != null)
            {
              overrides.Add("\"" + _override + "\"");
            }
          }
        }
        string jsonOverrides = BCUtils.toJson(overrides);
        details.Add("Overrides", jsonOverrides);

        //MODIFIERS
        Dictionary<string, Dictionary<string, string>> modifiers = new Dictionary<string, Dictionary<string, string>>();
        if (multiBuffClasses[key].Modifiers != null)
        {
          var z = 0;
          foreach (MultiBuffClass.Modifier modifier in multiBuffClasses[key].Modifiers)
          {
            if (modifier != null)
            {
              Dictionary<string, string> _modifier = new Dictionary<string, string>();
              _modifier.Add("CategoryFlags", modifier.CategoryFlags.ToString());
              //_modifier.Add("ModifierApplyTime", modifier.ModifierApplyTime.ToString());//all 0
              //_modifier.Add("ModifierFDuration", modifier.ModifierFDuration.ToString());//all 0
              _modifier.Add("ModifierFrequency", modifier.ModifierFrequency.ToString());
              //_modifier.Add("ModifierIDuration", modifier.ModifierIDuration.ToString());//all 0
              //_modifier.Add("ModifierStackMax", modifier.ModifierStackMax.ToString());//all -1
              _modifier.Add("ModifierUID", modifier.ModifierUID.ToString());
              _modifier.Add("ModifierValueEnd", modifier.ModifierValueEnd.ToString());
              _modifier.Add("ModifierValueStart", modifier.ModifierValueStart.ToString());
              _modifier.Add("TargetBuff", (modifier.TargetBuff != null ? modifier.TargetBuff : ""));
              _modifier.Add("TargetStat", modifier.TargetStat.ToString());
              _modifier.Add("TypeOfModifier", modifier.TypeOfModifier.ToString());
              modifiers.Add(z.ToString(), _modifier);
              z++;
            }
          }
        }
        string jsonModifiers = BCUtils.toJson(modifiers);
        details.Add("Modifiers", jsonModifiers);

        var jsonDetails = BCUtils.toJson(details);
        data.Add(key.ToString(), jsonDetails);
      }

      return data;
    }

    public override void Process()
    {
      string output = "";
      if (_options.ContainsKey("json"))
      {
        output = BCUtils.toJson(jsonObject());
        SendOutput(output);
      }
      else
      {
        foreach (MultiBuffClass mbc in MultiBuffClass.s_classes.Values)
        {
          output += mbc.Id + _sep;
        }
        SendOutput(output);
      }
    }
  }
}
