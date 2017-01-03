using System;
using System.Collections.Generic;

namespace BCM.Models
{
  [Serializable]
  public class QuestList
  {
    public List<Quest> quests = new List<Quest>();

    public QuestList()
    {
    }

    public QuestList(PlayerDataFile _pdf)
    {
      Load(_pdf);
    }

    public void Load(PlayerDataFile _pdf)
    {
      foreach (Quest q in _pdf.questJournal.Clone().quests)
      {
        quests.Add(q);
      }
    }

    public string Display()
    {
      bool first = true;
      string output = "Quests={\n";
      foreach (Quest q in quests)
      {
        // todo: add checks for existance of quests before trying to output them.
        var qc = QuestClass.s_Quests[q.ID];
        if (!first) { output += ",\n"; } else { first = false; }
        output += " " + qc.Name + "(" + q.ID + "):" + q.CurrentState;
      }
      output += "\n}\n";

      return output;
    }

    public bool IsChanged ()
    {
      // checks for a change in the quest list and returns true if there is a difference.
      // todo: check the past questlist with the this.quests for changes (new quests, status changed, removed quests). Requires quests to be pushed to persistent data
      return false;
    }

    public List<Quest> ChangedQuests()
    {
      // checks for a change in the quest list and returns true if there is a difference.
      if (IsChanged())
      {
        // todo: only return the quests that have changed status
        return quests;
      }
      return null;
    }

    public List<Quest> NewQuests()
    {
      // checks for a change in the quest list and returns true if there is a difference.
      if (IsChanged())
      {
        // todo: only return the quests that are new
        return quests;
      }
      return null;
    }

  }
}
