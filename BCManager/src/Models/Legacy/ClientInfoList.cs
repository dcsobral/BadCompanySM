using System;
using System.Collections.Generic;
using UnityEngine;

namespace BCM.Models.Legacy
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
      //todo: add last ping to persistent data?
      info.Add("Ping", (_pInfo.CI != null ? _pInfo.CI.ping.ToString() : "Offline"));

      long totalPlayTime = (_pInfo.PCP != null ? _pInfo.PCP.TotalPlayTime : 0);
      info.Add("TotalPlayTime", (totalPlayTime / 60).ToString("F1") + "");
      if (_pInfo.EP == null)
      {
        info.Add("LastOnline", (_pInfo.PCP != null ? _pInfo.PCP.LastOnline.ToString("yyyy-MM-dd HH:mm") : ""));
      }
      else if (_pInfo.EP != null)
      {
        info.Add("SessionPlayTime", ((Time.timeSinceLevelLoad - _pInfo.EP.CreationTimeSinceLevelLoad) / 60).ToString("F1") + "");
      }
      info.Add("Position", (_pInfo.EP != null ? Convert.PosToStr(_pInfo.EP.position, postype) : (_pInfo.PDF != null ? Convert.PosToStr(_pInfo.PDF.ecd.pos, postype) : "")));
      info.Add("UndergroundDistance", (_pInfo.EP != null ? (_pInfo.EP.position.y - (int)Math.Floor(_pInfo.EP.serverPos.y / 32f)).ToString() : "0"));
      info.Add("Rotation", (_pInfo.EP != null ? Convert.PosToStr(_pInfo.EP.rotation, postype) : (_pInfo.PDF != null ? Convert.PosToStr(_pInfo.PDF.ecd.rot, postype) : "")));

    }

    public override string Display(string sep = " ")
    {
      string output = "";
      bool first = true;
      //foreach (string key in info.Keys)
      //{
      //  if (!first) { output += sep; } else { first = false; }
      //  output += key + ":" + info[key];
      //}
      foreach (KeyValuePair<string, string> kvp in info)
      {
        if (!first) { output += sep; } else { first = false; }
        output += kvp.Key + ":" + kvp.Value;
      }
      return output;
    }
    public Dictionary<string, string> GetInfo()
    {
      return info;
    }
    public string DisplayShort(string sep = " ")
    {
      string output = "";
      output += "Name:" + info["Name"] + "," + "SteamId:" + info["SteamId"] + "," + "EntityId:" + info["EntityId"];
      return output;
    }
    public string DisplayShortWithPos(string sep = " ")
    {
      string output = "";
      output += "Name:" + info["Name"] + "," + "SteamId:" + info["SteamId"] + "," + "EntityId:" + info["EntityId"] + "," + "Position:" + info["Position"];
      return output;
    }
    public string DisplaySteamIdAndPos(string sep = " ")
    {
      string output = "";
      output += info["SteamId"] + ":" + info["Position"] + sep;
      return output;
    }
  }
}
