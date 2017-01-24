using System;
using System.Collections.Generic;
using UnityEngine;

namespace BCM.Models
{
  [Serializable]
  public class BCMBuff
  {
    public string Name;
    public bool Expired;
    public bool IsOverriden;
    public int InstigatorId;
    public string MultiBuffClass_Id;
    public float MultiBuffClass_FDuration;
    public float Timer_TimeFraction;

    public BCMBuff(MultiBuff b)
    {
      Name = b.Name;
      Expired = b.Expired;
      IsOverriden = b.IsOverriden;
      InstigatorId = b.InstigatorId;
      MultiBuffClass_Id = b.MultiBuffClass.Id;
      Timer_TimeFraction = b.Timer.TimeFraction;
    }
    public string GetJson()
    {
      return JsonUtility.ToJson(this);
    }
  }
}
