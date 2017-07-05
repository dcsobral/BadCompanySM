using System.Collections.Generic;

namespace BCM.Commands
{
  public class ListQuests : BCCommandAbstract
  {
    public virtual Dictionary<string, string> jsonObject()
    {
      Dictionary<string, string> data = new Dictionary<string, string>();

      var i = 0;
      foreach (QuestClass questclass in QuestClass.s_Quests.Values)
      {
        Dictionary<string, string> details = new Dictionary<string, string>();

        details.Add("ID", (questclass.ID != null ? questclass.ID : ""));
        details.Add("Name", (questclass.Name != null ? questclass.Name : ""));

        details.Add("Category", (questclass.Category != null ? questclass.Category : ""));
        //details.Add("CompletionType", questclass.CompletionType.ToString());//all AutoComplete
        //details.Add("CurrentVersion", questclass.CurrentVersion.ToString());//all 0
        details.Add("Description", (questclass.Description != null ? questclass.Description : ""));
        details.Add("Difficulty", (questclass.Difficulty != null ? questclass.Difficulty : ""));
        details.Add("GroupName", (questclass.GroupName != null ? questclass.GroupName : ""));
        details.Add("Icon", (questclass.Icon != null ? questclass.Icon : ""));
        details.Add("Offer", (questclass.Offer != null ? questclass.Offer : ""));
        details.Add("PreviousQuest", (questclass.PreviousQuest != null ? questclass.PreviousQuest : ""));
        details.Add("Repeatable", questclass.Repeatable.ToString());
        details.Add("SubTitle", (questclass.SubTitle != null ? questclass.SubTitle : ""));

        if (questclass.Actions != null)
        {
          List<string> actions = new List<string>();
          foreach (BaseQuestAction bqact in questclass.Actions)
          {
            Dictionary<string, string> _action = new Dictionary<string, string>();

            _action.Add("ID", (bqact.ID != null ? bqact.ID : ""));
            _action.Add("OwnerQuest", (bqact.OwnerQuest != null ? bqact.OwnerQuest.ID : ""));
            _action.Add("Value:", (bqact.Value != null ? bqact.Value : ""));

            string jsonAction = BCUtils.toJson(_action);
            actions.Add(jsonAction);
          }
          string jsonActions = BCUtils.toJson(actions);
          details.Add("Actions", jsonActions);
        }

        if (questclass.Objectives != null)
        {
          List<string> objectives = new List<string>();
          foreach (BaseObjective bobj in questclass.Objectives)
          {
            Dictionary<string, string> _objective = new Dictionary<string, string>();

            _objective.Add("ID", (bobj.ID != null ? bobj.ID : ""));
            //_objective.Add("CurrentVersion", bobj.CurrentVersion.ToString());
            //_objective.Add("Description:", (bobj.Description != null ? bobj.Description : ""));//error
            _objective.Add("ObjectiveValueType", bobj.ObjectiveValueType.ToString());
            _objective.Add("Optional", bobj.Optional.ToString());
            _objective.Add("OwnerQuest", (bobj.OwnerQuest != null ? bobj.OwnerQuest.ID : ""));
            //_objective.Add("StatusText", (bobj.StatusText != null ? bobj.StatusText : ""));//error
            _objective.Add("Value", (bobj.Value != null ? bobj.Value : ""));

            string jsonObjective = BCUtils.toJson(_objective);
            objectives.Add(jsonObjective);
          }
          string jsonObjectives = BCUtils.toJson(objectives);
          details.Add("Objectives", jsonObjectives);
        }

        if (questclass.Requirements != null)
        {
          List<string> requirements = new List<string>();
          foreach (BaseRequirement breq in questclass.Requirements)
          {
            Dictionary<string, string> _requirement = new Dictionary<string, string>();

            _requirement.Add("ID", (breq.ID != null ? breq.ID : ""));
            _requirement.Add("Description", (breq.Description != null ? breq.Description : ""));
            _requirement.Add("OwnerQuest", (breq.OwnerQuest != null ? breq.OwnerQuest.ID : ""));
            _requirement.Add("StatusText", (breq.StatusText != null ? breq.StatusText : ""));
            _requirement.Add("Value", (breq.Value != null ? breq.Value : ""));

            string jsonRequirement = BCUtils.toJson(_requirement);
            requirements.Add(jsonRequirement);
          }
          string jsonRequirements = BCUtils.toJson(requirements);
          details.Add("Requirements", jsonRequirements);
        }

        if (questclass.Rewards != null)
        {
          List<string> rewards = new List<string>();
          foreach (BaseReward brew in questclass.Rewards)
          {
            Dictionary<string, string> _reward = new Dictionary<string, string>();

            _reward.Add("ID", (brew.ID != null ? brew.ID : ""));
            _reward.Add("Description", (brew.Description != null ? brew.Description : ""));
            _reward.Add("HiddenReward", brew.HiddenReward.ToString());
            _reward.Add("Icon", (brew.Icon != null ? brew.Icon : ""));
            _reward.Add("IconAtlas", (brew.IconAtlas != null ? brew.IconAtlas : ""));
            _reward.Add("Optional", brew.Optional.ToString());
            _reward.Add("OwnerQuest", (brew.OwnerQuest != null ? brew.OwnerQuest.ID : ""));
            _reward.Add("ReceiveStage", brew.ReceiveStage.ToString());
            _reward.Add("Value", (brew.Value != null ? brew.Value : ""));
            _reward.Add("ValueText", (brew.ValueText != null ? brew.ValueText : ""));

            string jsonReward = BCUtils.toJson(_reward);
            rewards.Add(jsonReward);
          }
          string jsonRewards = BCUtils.toJson(rewards);
          details.Add("Rewards", jsonRewards);
        }


        var jsonDetails = BCUtils.toJson(details);
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
            _options["tag"] = "bc-quests";
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
        foreach (QuestClass qc in QuestClass.s_Quests.Values)
        {
          output += qc.Name + "(" + qc.ID + "):" + qc.SubTitle;
        }
        SendOutput(output);
      }
    }
  }
}
