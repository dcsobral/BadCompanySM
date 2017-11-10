using System;

namespace BCM.Models
{
  public class BCMGroupSpawn
  {
    public int EntityClassId;
    public double Prob;
    public int ReqMin;
    public int ReqMax;

    public BCMGroupSpawn(SEntityClassAndProb spawn)
    {
      EntityClassId = spawn.entityClassId;
      Prob = Math.Round(spawn.prob, 3);
      ReqMin = spawn.reqMin;
      ReqMax = spawn.reqMax;
    }
  }
}
