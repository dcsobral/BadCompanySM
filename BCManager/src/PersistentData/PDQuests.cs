using BCM.Models;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

namespace BCM.PersistentData
{
  [Serializable]
  public class PDQuests
  {
    public List<PDQuest> quests = new List<PDQuest>();

    public bool Update(PlayerDataFile _pdf)
    {
      ProcessQuests(quests, _pdf.questJournal.quests);
      return true;
    }
    private bool ProcessQuests (List<PDQuest> target, List<Quest> sourceQuests)
    {
      target.Clear();
      foreach(Quest gq in sourceQuests)
      {
        PDQuest q = new PDQuest(gq.ID);
        q.CurrentState = (PDQuest.QuestState)gq.CurrentState;
        target.Add(q);
      }

      return true;
    }
  }
}
