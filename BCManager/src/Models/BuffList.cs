using System;
using System.Collections.Generic;

namespace BCM.Models
{
  [Serializable]
  public class BuffList
  {
    private List<MultiBuff> buffs = new List<MultiBuff>();
    private List<MultiBuff> sdbuffs = new List<MultiBuff>();

    public BuffList()
    {
    }

    public BuffList(PlayerInfo _pInfo)
    {
      Load(_pInfo);
    }

    public void Load(PlayerInfo _pInfo)
    {
      if (_pInfo.EP != null)
      {
        foreach (MultiBuff b in _pInfo.EP.Stats.Buffs)
        {
          buffs.Add(b);
        }
      }
      foreach (MultiBuff b in _pInfo.PDF.ecd.stats.Buffs)
      {
        sdbuffs.Add(b);
      }
    }

    public string Display()
    {
      // todo: make the list into a single list with a flag for saved buffs since the timer only updates when the save updates (30 sec interval)
      bool first = true;
      string output = "Buffs={\n";
      foreach (MultiBuff b in buffs)
      {
        if (!first) { output += ",\n"; } else { first = false; }
        output += " " + b.Name + "(" + b.MultiBuffClass.Id + ")" + ":" + (b.MultiBuffClass.FDuration * b.Timer.TimeFraction).ToString("0") + "/" + b.MultiBuffClass.FDuration + "(s) (" + (b.Timer.TimeFraction * 100).ToString("0.0") + "%)";
      }
      output += "\n}\n";

      first = true;
      output += "SavedBuffs={\n";
      foreach (MultiBuff b in sdbuffs)
      {
        if (!first) { output += ",\n"; } else { first = false; }
        output += " " + b.Name + "(" + b.MultiBuffClass.Id + ")" + ":" + (b.MultiBuffClass.FDuration * b.Timer.TimeFraction).ToString("0") + "/" + b.MultiBuffClass.FDuration + "(s) (" + (b.Timer.TimeFraction * 100).ToString("0.0") + "%)";
      }
      output += "\n}\n";

      return output;
    }
  }
}
