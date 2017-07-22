using UnityEngine;

namespace BCM
{
  public struct Spawn
  {
    public int spawnerId;
    public int entityId;
    public int targetId;
    public int entityClassId;
    public BCMVector3 pos;
    public int minRange;
    public int maxRange;
    public bool isObserver;
    public bool isFeral;
    public float speedMul;
    public float speedBase;
    public bool nightRun;
  }
}
