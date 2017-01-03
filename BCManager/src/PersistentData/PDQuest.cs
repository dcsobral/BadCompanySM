using BCM.Models;
using System;
using System.Runtime.Serialization;
using UnityEngine;

namespace BCM.PersistentData
{
  [Serializable]
  public class PDQuest
  {
    public PDQuest(string id)
    {
      ID = id;
      CurrentState = QuestState.InProgress;
    }

    public enum QuestState
    {
      InProgress,
      Completed,
      Failed
    }

    //public Dictionary<string, string> DataVariables = new Dictionary<string, string>();

    //public List<BaseQuestAction> Actions = new List<BaseQuestAction>();

    //public List<BaseRequirement> Requirements = new List<BaseRequirement>();

    //public List<BaseObjective> Objectives = new List<BaseObjective>();

    //public List<BaseReward> Rewards = new List<BaseReward>();

    public QuestState CurrentState
    {
      get;
      set;
    }

    public string ID
    {
      get;
      private set;
    }

    public byte CurrentQuestVersion
    {
      get;
      set;
    }

    public byte CurrentFileVersion
    {
      get;
      set;
    }

    public string PreviousQuest
    {
      get;
      set;
    }

    public bool OptionalComplete
    {
      get;
      private set;
    }

    public ulong FinishTime
    {
      get;
      set;
    }

    public bool Active
    {
      get
      {
        return CurrentState == QuestState.InProgress;
      }
    }
  }
}
