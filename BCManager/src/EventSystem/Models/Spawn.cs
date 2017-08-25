using UnityEngine;

namespace BCM
{
  public struct Spawn
  {
    public int SpawnerId;
    public int EntityId;
    public int TargetId;
    public int EntityClassId;
    public Vector3 Pos;
    public int MinRange;
    public int MaxRange;
    public bool IsObserver;
    public bool IsFeral;
    public float SpeedMul;
    public float SpeedBase;
    public bool NightRun;
  }
}
