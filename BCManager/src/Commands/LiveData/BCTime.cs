using System;
using System.Collections.Generic;

namespace BCM.Commands
{
  public class BCTime : BCCommandAbstract
  {
    public class BCMTime
    {
      public readonly Dictionary<string, int> Time;
      public double Ticks;
      public double Fps;
      public int Clients;
      public int Entities;

      public BCMTime()
      {
        var worldTime = GameManager.Instance.World.worldTime;
        Time = new Dictionary<string, int>
        {
          {"D", GameUtils.WorldTimeToDays(worldTime)},
          {"H", GameUtils.WorldTimeToHours(worldTime)},
          {"M", GameUtils.WorldTimeToMinutes(worldTime)}
        };

        if (Options.ContainsKey("t") || Options.ContainsKey("s")) return;

        Ticks = Math.Round(UnityEngine.Time.timeSinceLevelLoad, 2);
        Fps = Math.Round(GameManager.Instance.fps.Counter, 2);
        Clients = ConnectionManager.Instance.ClientCount();
        Entities = GameManager.Instance.World.Entities.Count;
      }
    }
    public override void Process()
    {
      var time = new BCMTime();

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
