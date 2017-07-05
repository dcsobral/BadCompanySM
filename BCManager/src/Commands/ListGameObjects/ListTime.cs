using System.Collections.Generic;
using UnityEngine;

namespace BCM.Commands
{
  public class ListTime : BCCommandAbstract
  {
    public virtual Dictionary<string, string> jsonObject()
    {
      ulong worldTime = GameManager.Instance.World.worldTime;
      float fps = GameManager.Instance.fps.Counter;
      int clients = ConnectionManager.Instance.ClientCount();
      int entities = GameManager.Instance.World.Entities.Count;
      float ticks = Time.timeSinceLevelLoad;

      var time = new Dictionary<string, string>();
      time.Add("days", GameUtils.WorldTimeToDays(worldTime).ToString());
      time.Add("hours", GameUtils.WorldTimeToHours(worldTime).ToString());
      time.Add("minutes", GameUtils.WorldTimeToMinutes(worldTime).ToString());
      time.Add("fps", fps.ToString("#.##"));
      time.Add("clients", clients.ToString());
      time.Add("entities", entities.ToString());
      time.Add("ticks", ticks.ToString("#.##"));

      return time;
    }
    public override void Process()
    {
      string output = "";
      if (_options.ContainsKey("json"))
      {
        if (_options.ContainsKey("tag"))
        {
          if (_options["tag"] == null)
          {
            _options["tag"] = "bc-time";
          }

          SendOutput("{\"tag\":\"" + _options["tag"] + "\",\"data\":" + BCUtils.toJson(jsonObject()) + "}");
        }
        else
        {
          SendOutput(BCUtils.toJson(jsonObject()));
        }
      }
      else
      {
        output = "Not Implemented, use /json";
        SendOutput(output);
      }
    }
  }
}
