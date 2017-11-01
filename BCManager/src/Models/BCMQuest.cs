using System;
using System.Collections.Generic;

namespace BCM.Models
{
  [Serializable]
  public class BCMQuest : BCMAbstract
  {
    #region Filters
    public static class StrFilters
    {
      public const string Id = "id";
      public const string Name = "name";
      public const string Group = "group";
      public const string SubTitle = "title";
      public const string Desc = "desc";
      public const string Offer = "offer";
      public const string Difficulty = "difficulty";
      public const string Icon = "icon";
      public const string Repeatable = "repeatable";
      public const string Category = "category";
      public const string Actions = "actions";
      public const string Requirements = "requirements";
      public const string Objectives = "objectives";
      public const string Rewards = "rewards";
    }

    private static Dictionary<int, string> _filterMap = new Dictionary<int, string>
    {
      {0, StrFilters.Id},
      {1, StrFilters.Name},
      {2, StrFilters.Group},
      {3, StrFilters.SubTitle},
      {4, StrFilters.Desc},
      {5, StrFilters.Offer},
      {6, StrFilters.Difficulty},
      {7, StrFilters.Icon},
      {8, StrFilters.Repeatable},
      {9, StrFilters.Category},
      {10, StrFilters.Actions},
      {11, StrFilters.Requirements},
      {12, StrFilters.Objectives},
      {13, StrFilters.Rewards}
    };
    public static Dictionary<int, string> FilterMap => _filterMap;
    #endregion

    #region Properties
    public string Id;
    public string Name;
    public string Group;
    public string SubTitle;
    public string Desc;
    public string Offer;
    public string Difficulty;
    public string Icon;
    public bool Repeatable;
    public string Category;
    //public byte Version;
    //public string Previous;
    //public byte HighestPhase;
    //public string Completion;

      //todo: additional properties from derived types
    public List<BCMQuestAction> Actions = new List<BCMQuestAction>();
    public List<BCMRequirement> Requirements = new List<BCMRequirement>();
    public List<BCMObjective> Objectives = new List<BCMObjective>();
    public List<BCMReward> Rewards = new List<BCMReward>();

    public class BCMQuestAction
    {
      public string Type;
      public string Id;
      public string Value;
      //public string OwnerQuest;

      public BCMQuestAction(BaseQuestAction action)
      {
        Type = action.GetType().ToString();
        Id = action.ID;
        Value = action.Value;
      }
    }

    public class BCMRequirement
    {
      public string Type;
      public string Id;
      public string Value;
      //public bool Complete;
      //public string Description;
      //public string StatusText;
      //public Quest OwnerQuest;

      public BCMRequirement(BaseRequirement requirement)
      {
        Type = requirement.GetType().ToString();
        Id = requirement.ID;
        Value = requirement.Value;
      }
    }

    public class BCMObjective
    {
      public string Type;
      public string Id;
      public string Value;
      //public byte Version;
      //public bool Complete;
      //public byte Phase;
      //public Quest OwnerQuest;

      public BCMObjective(BaseObjective objective)
      {
        Type = objective.GetType().ToString();
        Id = objective.ID;
        Value = objective.Value;
      }
    }

    public class BCMReward
    {
      public string Type;
      public string Id;
      public string Value;
      //public string OwnerQuest;

      public BCMReward(BaseReward reward)
      {
        Type = reward.GetType().ToString();
        Id = reward.ID;
        Value = reward.Value;
      }
    }
    #endregion;

    public BCMQuest(object obj, string typeStr, Dictionary<string, string> options, List<string> filters) : base(obj, typeStr, options, filters)
    {
    }

    public override void GetData(object obj)
    {
      var quest = obj as QuestClass;
      if (quest == null) return;

      if (IsOption("filter"))
      {
        foreach (var f in StrFilter)
        {
          switch (f)
          {
            case StrFilters.Id:
              GetId(quest);
              break;
            case StrFilters.Name:
              GetName(quest);
              break;
            case StrFilters.Group:
              GetGroup(quest);
              break;
            case StrFilters.SubTitle:
              GetSubTitle(quest);
              break;
            case StrFilters.Desc:
              GetDesc(quest);
              break;
            case StrFilters.Offer:
              GetOffer(quest);
              break;
            case StrFilters.Difficulty:
              GetDifficulty(quest);
              break;
            case StrFilters.Icon:
              GetIcon(quest);
              break;
            case StrFilters.Repeatable:
              GetRepeatable(quest);
              break;
            case StrFilters.Category:
              GetCategory(quest);
              break;
            case StrFilters.Actions:
              GetActions(quest);
              break;
            case StrFilters.Requirements:
              GetRequirements(quest);
              break;
            case StrFilters.Objectives:
              GetObjectives(quest);
              break;
            case StrFilters.Rewards:
              GetRewards(quest);
              break;
            default:
              Log.Out($"{Config.ModPrefix} Unknown filter {f}");
              break;
          }
        }
      }
      else
      {
        GetId(quest);
        GetName(quest);
        GetGroup(quest);
        GetSubTitle(quest);
        GetDesc(quest);
        GetOffer(quest);
        GetDifficulty(quest);
        GetIcon(quest);
        GetRepeatable(quest);
        GetCategory(quest);
        GetActions(quest);
        GetRequirements(quest);
        GetObjectives(quest);
        GetRewards(quest);
      }
    }

    private void GetRewards(QuestClass quest)
    {
      foreach (var reward in quest.Rewards)
      {
        Rewards.Add(new BCMReward(reward));
      }
      Bin.Add("Rewards", Rewards);
    }

    private void GetObjectives(QuestClass quest)
    {
      foreach (var objective in quest.Objectives)
      {
        Objectives.Add(new BCMObjective(objective));
      }
      Bin.Add("Objectives", Objectives);
    }

    private void GetRequirements(QuestClass quest)
    {
      foreach (var requirement in quest.Requirements)
      {
        Requirements.Add(new BCMRequirement(requirement));
      }
      Bin.Add("Requirements", Requirements);
    }

    private void GetActions(QuestClass quest)
    {
      foreach (var action in quest.Actions)
      {
        Actions.Add(new BCMQuestAction(action));
      }
      Bin.Add("Actions", Actions);
    }

    private void GetCategory(QuestClass quest) => Bin.Add("Category", Category = quest.Category);

    private void GetRepeatable(QuestClass quest) => Bin.Add("Repeatable", Repeatable = quest.Repeatable);

    private void GetIcon(QuestClass quest) => Bin.Add("Icon", Icon = quest.Icon);

    private void GetDifficulty(QuestClass quest) => Bin.Add("Difficulty", Difficulty = quest.Difficulty);

    private void GetOffer(QuestClass quest) => Bin.Add("Offer", Offer = quest.Offer);

    private void GetDesc(QuestClass quest) => Bin.Add("Desc", Desc = quest.Description);

    private void GetSubTitle(QuestClass quest) => Bin.Add("SubTitle", SubTitle = quest.SubTitle);

    private void GetGroup(QuestClass quest) => Bin.Add("Group", Group = quest.GroupName);

    private void GetName(QuestClass questClass) => Bin.Add("Name", Name = questClass.Name);

    private void GetId(QuestClass questClass) => Bin.Add("Id", Id = questClass.ID);
  }
}
