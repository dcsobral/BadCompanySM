using System;
using JetBrains.Annotations;

namespace BCM.Models
{
  public class BCMGroupSpawn
  {
    [UsedImplicitly] public int EntityClassId;
    [UsedImplicitly] public double Prob;
    [UsedImplicitly] public int ReqMin;
    [UsedImplicitly] public int ReqMax;

    public BCMGroupSpawn(SEntityClassAndProb spawn)
    {
      EntityClassId = spawn.entityClassId;
      Prob = Math.Round(spawn.prob, 3);
      ReqMin = spawn.reqMin;
      ReqMax = spawn.reqMax;
    }
  }
}
