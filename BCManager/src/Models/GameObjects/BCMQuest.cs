using System;
using System.Collections.Generic;
using JetBrains.Annotations;

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

    private static readonly Dictionary<int, string> _filterMap = new Dictionary<int, string>
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
    [UsedImplicitly] public string Id;
    [UsedImplicitly] public string Name;
    [UsedImplicitly] public string Group;
    [UsedImplicitly] public string SubTitle;
    [UsedImplicitly] public string Desc;
    [UsedImplicitly] public string Offer;
    [UsedImplicitly] public string Difficulty;
    [UsedImplicitly] public string Icon;
    [UsedImplicitly] public bool Repeatable;
    [UsedImplicitly] public string Category;
    [NotNull] [UsedImplicitly] public List<BCMQuestAction> Actions = new List<BCMQuestAction>();
    [NotNull] [UsedImplicitly] public List<BCMQuestRequirement> Requirements = new List<BCMQuestRequirement>();
    [NotNull] [UsedImplicitly] public List<BCMQuestObjective> Objectives = new List<BCMQuestObjective>();
    [NotNull] [UsedImplicitly] public List<BCMQuestReward> Rewards = new List<BCMQuestReward>();
    #endregion;

    public BCMQuest(object obj, string typeStr, Dictionary<string, string> options, List<string> filters) : base(obj, typeStr, options, filters)
    {
    }

    protected override void GetData(object obj)
    {
      if (!(obj is QuestClass quest)) return;

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
        Rewards.Add(new BCMQuestReward(reward));
      }
      Bin.Add("Rewards", Rewards);
    }

    private void GetObjectives(QuestClass quest)
    {
      foreach (var objective in quest.Objectives)
      {
        Objectives.Add(new BCMQuestObjective(objective));
      }
      Bin.Add("Objectives", Objectives);
    }

    private void GetRequirements(QuestClass quest)
    {
      foreach (var requirement in quest.Requirements)
      {
        Requirements.Add(new BCMQuestRequirement(requirement));
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
