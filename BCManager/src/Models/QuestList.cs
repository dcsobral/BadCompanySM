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

    public QuestList(PlayerInfo _pInfo)
    {
      Load(_pInfo);
    }

    public void Load(PlayerInfo _pInfo)
    {
      foreach (Quest q in _pInfo.PDF.questJournal.Clone().quests)
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
  }
}
