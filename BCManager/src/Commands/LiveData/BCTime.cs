using System;
using System.Collections.Generic;

namespace BCM.Commands
{
  public class BCTime : BCCommandAbstract
  {
    //public Dictionary<string, object> _bin = new Dictionary<string, object>();

    public class BCMTime
    {
      public Dictionary<string, int> Time;
      public double Ticks;
      public double Fps;
      public int Clients;
      public int Entities;

      public BCMTime(Dictionary<string, string> options)
      {
        var worldTime = GameManager.Instance.World.worldTime;
        Time = new Dictionary<string, int>
        {
          {"D", GameUtils.WorldTimeToDays(worldTime)},
          {"H", GameUtils.WorldTimeToHours(worldTime)},
          {"M", GameUtils.WorldTimeToMinutes(worldTime)}
        };

        if (options.ContainsKey("t") || options.ContainsKey("s")) return;

        Ticks = Math.Round(UnityEngine.Time.timeSinceLevelLoad, 2);
        Fps = Math.Round(GameManager.Instance.fps.Counter, 2);
        Clients = ConnectionManager.Instance.ClientCount();
        Entities = GameManager.Instance.World.Entities.Count;
      }
    }
    public override void Process()
    {
      var time = new BCMTime(Options);

      if (Options.ContainsKey("t"))
      {
        SendJson(time.Time);
      }
      else if (Options.ContainsKey("s"))
      {
        SendJson(new[] { time.Time["D"], time.Time["H"], time.Time["M"] });
      }
      else
      {
        SendJson(time);
      }
    }
  }
}
