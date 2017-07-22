using System.Collections.Generic;
using UnityEngine;

namespace BCM
{
  public struct HordeSpawner
  {
    public int spawnerId;
    public string type;
    public int aliveCount;
    public ulong lastSpawntick;
    public int spawnDelay;
    public int totalCount;
    public int wave;
    public int waveCount;
    public int waveDelay;
    public BCMVector3 fixedPos;
    public Dictionary<string, Spawn> spawns;
  }
}
