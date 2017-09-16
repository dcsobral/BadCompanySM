using System.Collections.Generic;
using BCM.Models;

namespace BCM
{
  public struct HordeSpawner
  {
    public int SpawnerId;
    public string Type;
    public int AliveCount;
    public ulong LastSpawntick;
    public int SpawnDelay;
    public int TotalCount;
    public int Wave;
    public int WaveCount;
    public int WaveDelay;
    public BCMVector3 FixedPos;
    public Dictionary<string, Spawn> Spawns;
  }
}
