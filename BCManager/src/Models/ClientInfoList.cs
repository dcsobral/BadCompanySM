using System;
using System.Collections.Generic;
using UnityEngine;

namespace BCM.Models
{
  [Serializable]
  public class ClientInfoList
  {
    private Dictionary<string, string> info = new Dictionary<string, string>();

    public ClientInfoList()
    {
    }

    public ClientInfoList(PlayerInfo _pInfo)
    {
      Load(_pInfo);
    }

    public void Load(PlayerInfo _pInfo)
    {
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
        info.Add("LastPosition", (_pInfo.PCP != null ? GameUtils.WorldPosToStr(_pInfo.PCP.LastPosition.ToVector3(), " ") : ""));
        //todo: add lastrotation to persistent data
        info.Add("TotalPlayTime", totalPlayTime.ToString());
      }
      else if (_pInfo.EP != null)
      {
        info.Add("SessionPlayTime", ((Time.timeSinceLevelLoad - _pInfo.EP.CreationTimeSinceLevelLoad) / 60).ToString("0.0") + "(mins)");
        info.Add("TotalPlayTime", (totalPlayTime / 60).ToString("0.0") + "(mins)");
        info.Add("Position", _pInfo.EP.position.x.ToString("0.0") + " " + _pInfo.EP.position.y.ToString("0.0") + " " + _pInfo.EP.position.z.ToString("0.0"));
        info.Add("Rotation", _pInfo.EP.rotation.ToString());
      }
    }

    public string Display()
    {
      string output = "";
      foreach (KeyValuePair<string,string> kvp in info)
      {
        output += kvp.Key + ":" + kvp.Value + "\n";
      }
      return output;
    }
    public string DisplayShort()
    {
      string output = "";
      output += "Name:" + info["Name"] + "," + "SteamId:" + info["SteamId"] + "," + "EntityId:" + info["EntityId"] + "\n";
      return output;
    }
  }
}
