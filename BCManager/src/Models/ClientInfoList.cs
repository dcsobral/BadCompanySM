using System;
using System.Collections.Generic;
using UnityEngine;

namespace BCM.Models
{
  [Serializable]
  public class ClientInfoList : AbstractList
  {
    private Dictionary<string, string> info = new Dictionary<string, string>();

    public ClientInfoList(PlayerInfo _pInfo, Dictionary<string, string> _options) : base(_pInfo, _options)
    {
    }

    public override void Load(PlayerInfo _pInfo)
    {
      string postype = GetPosType();

      info.Add("Name", (_pInfo.CI != null ? _pInfo.CI.playerName : _pInfo.PCP != null ? _pInfo.PCP.Name : string.Empty));
      info.Add("SteamId", _pInfo._steamId);
      info.Add("EntityId", _pInfo.PDF.id.ToString());
      info.Add("IP", (_pInfo.CI != null ? _pInfo.CI.ip.ToString() : _pInfo.PCP != null ? _pInfo.PCP.IP.ToString() : string.Empty));
      //todo: add last ping to persistent data
      info.Add("Ping", (_pInfo.CI != null ? _pInfo.CI.ping.ToString() : "Offline"));

      long totalPlayTime = (_pInfo.PCP != null ? _pInfo.PCP.TotalPlayTime : 0);
      if (_pInfo.EP == null)
      {
        info.Add("LastOnline", (_pInfo.PCP != null ? _pInfo.PCP.LastOnline.ToString("yyyy-MM-dd HH:mm") : ""));
        info.Add("LastPosition", (_pInfo.PCP != null ? Convert.PosToStr(_pInfo.PCP.LastPosition, postype) : ""));
        //todo: add lastrotation to persistent data?
        info.Add("TotalPlayTime", totalPlayTime.ToString());
      }
      else if (_pInfo.EP != null)
      {
        info.Add("SessionPlayTime", ((Time.timeSinceLevelLoad - _pInfo.EP.CreationTimeSinceLevelLoad) / 60).ToString("0.0") + "(mins)");
        info.Add("TotalPlayTime", (totalPlayTime / 60).ToString("0.0") + "(mins)");
        info.Add("Position", Convert.PosToStr(_pInfo.EP.position, postype));
        info.Add("Rotation", _pInfo.EP.rotation.ToString());
      }
    }

    public override string Display(string sep = " ")
    {
      string output = "";
      bool first = true;
      foreach (KeyValuePair<string,string> kvp in info)
      {
        if (!first) { output += sep; } else { first = false; }
        output += kvp.Key + ":" + kvp.Value;
      }
      return output;
    }
    public string DisplayShort(string sep = " ")
    {
      string output = "";
      output += "Name:" + info["Name"] + "," + "SteamId:" + info["SteamId"] + "," + "EntityId:" + info["EntityId"] + sep;
      return output;
    }
    public string DisplayShortWithPos(string sep = " ")
    {
      string output = "";
      output += "Name:" + info["Name"] + "," + "SteamId:" + info["SteamId"] + "," + "EntityId:" + info["EntityId"] + "," + (info["Position"] != "" ? "Position:" + info["Position"] : "LastPosition:" + info["LastPosition"]) + sep;
      return output;
    }
  }
}
