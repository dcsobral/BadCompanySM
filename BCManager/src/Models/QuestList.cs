using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BCM.Models
{
  [Serializable]
  public class QuestList
  {
    private List<Quest> quests = new List<Quest>();
    //private Dictionary<string, QuestClass> qc = QuestClass.s_Quests;

    public QuestList()
    {
    }

    public QuestList(PlayerDataFile _pdf)
    {
      LoadQuests(_pdf);
    }

    public void LoadQuests(PlayerDataFile _pdf)
    {
      foreach (Quest q in _pdf.questJournal.Clone().quests)
      {
        var qc = QuestClass.s_Quests[q.ID];
        quests.Add(q);
      }
    }

    public string DisplayQuests()
    {
      string output = "Quests={\n";
      bool first = true;

      foreach (Quest q in quests)
      {
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
      // todo: check the passes questlist with the this.quests for changes (new quests, status changed, removed quests)
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
