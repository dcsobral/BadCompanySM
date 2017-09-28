using UnityEngine;

namespace BCM
{
  public class Spawn
  {
    public int EntityClassId;
    public long SpawnerId;
    public Vector3 TargetPos;
    public int EntityId = -1;
    public int TargetId = -1;
    public int MinRange = 40;
    public int MaxRange = 60;
    public bool IsObserver = true;
    public bool IsFeral = false;
    public float SpeedMul = 1f;
    public float SpeedBase = 0f;
    public bool NightRun = false;
  }
}
