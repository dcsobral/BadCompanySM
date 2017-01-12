using System;
using System.Collections.Generic;

namespace BCM.Models
{
  [Serializable]
  public class QuestList : AbstractList
  {
    public List<Quest> quests = new List<Quest>();

    public QuestList(PlayerInfo _pInfo, Dictionary<string, string> _options) : base(_pInfo, _options)
    {
    }

    public override void Load(PlayerInfo _pInfo)
    {
      foreach (Quest q in _pInfo.PDF.questJournal.Clone().quests)
      {
        quests.Add(q);
      }
    }

    public override string Display(string sep = " ")
    {
      bool first = true;
      string output = "Quests:{";
      foreach (Quest q in quests)
      {
        if (!first) { output += sep; } else { first = false; }
        if (QuestClass.s_Quests.ContainsKey(q.ID)) {
          var qc = QuestClass.s_Quests[q.ID];
          output += qc.Name + "(" + q.ID + "):" + q.CurrentState;
        } else
        {
          output += "Unknown Quest";
        }
      }
      output += "}";

      return output;
    }
  }
}
