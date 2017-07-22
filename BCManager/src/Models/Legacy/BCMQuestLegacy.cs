using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

namespace BCM.Models.Legacy
{
  [Serializable]
  public class BCMQuestLegacy
  {
    public string id;
    public string currState;
    public bool hasPosition;
    public string position;
    public ulong finishtime;
    public bool optionalComplete;
    public string baseReqsJson;
    public string baseObjsJson;

    public List<BaseRequirement> baseReqs;
    public List<BaseObjective> baseObjs;

    public BCMQuestLegacy(Quest quest)
    {
      if (quest != null && quest.ID.Length != 0)
      {
        Parse(quest);
      }
    }

    public void Parse(Quest quest)
    {
      id = quest.ID;
      currState = quest.CurrentState.ToString();
      hasPosition = quest.HasPosition;
      if (hasPosition)
      {
        position = quest.Position.ToString();
      }
      finishtime = quest.FinishTime;
      optionalComplete = quest.OptionalComplete;

      baseReqs = quest.Requirements;
      string json_reqs = null;
      int idx = 0;
      bool first = true;
      foreach (BaseRequirement basereq in baseReqs)
      {
        if (basereq != null)
        {
          string json = "";
          if (!first) { json += ","; } else { json += "["; first = false; }
          json += "{\"" + idx.ToString() + "\":{\"ID\":\"" + basereq.ID + "\",\"Value\":\"" + basereq.Value + "\",\"StatusText\":\"" + basereq.StatusText + "\",\"Complete\":\"" + basereq.Complete + "\",\"Description\":\"" + basereq.Description + "\"}";
          json_reqs += json;
        }
      }
      if (json_reqs != null && json_reqs.Length > 0)
      {
        json_reqs += "]";
        baseReqsJson = json_reqs;
      }

      baseObjs = quest.Objectives;
      string json_objs = null;
      idx = 0;
      first = true;
      foreach (BaseObjective baseobj in baseObjs)
      {
        if (baseobj != null)
        {
          string json = "";
          if (!first) { json += ","; } else { json += "["; first = false; }
          json += "{\"" + idx.ToString() + "\":{\"ID\":\"" + baseobj.ID + "\",\"Value\":\"" + baseobj.Value + "\",\"Complete\":\"" + baseobj.Complete + "\"}";//\"StatusText\":\"" + baseobj.StatusText + "\",\"Description\":\"" + baseobj.Description + "\",
          json_reqs += json;
        }
      }
      if (json_objs != null && json_objs.Length > 0)
      {
        json_objs += "]";
        baseObjsJson = json_objs;
      }
    }

    public string GetJson()
    {
      return JsonUtility.ToJson(this);
    }

  }
}
