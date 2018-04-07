using System;
using System.Collections.Generic;
using SystemInformation;
using JetBrains.Annotations;

namespace BCM.Commands
{
  public class BCMTime
  {
    public readonly Dictionary<string, int> Time;
    [UsedImplicitly] public double Ticks;
    [UsedImplicitly] public double Fps;
    [UsedImplicitly] public int Clients;
    [UsedImplicitly] public int Entities;
    [UsedImplicitly] public int EntityInst;
    [UsedImplicitly] public int Players;
    [UsedImplicitly] public int Enemies;
    [UsedImplicitly] public int Observers;
    [UsedImplicitly] public int Chunks;
    [UsedImplicitly] public int ChunkInst;
    [UsedImplicitly] public int Objects;
    [UsedImplicitly] public int Items;
    [UsedImplicitly] public double Heap;
    [UsedImplicitly] public double Max;
    [UsedImplicitly] public double Rss;

    public BCMTime(IDictionary<string, string> options)
    {
      if (!BCUtils.CheckWorld(out var world)) return;

      var worldTime = world.worldTime;
      Time = new Dictionary<string, int>
        {
          {"D", GameUtils.WorldTimeToDays(worldTime)},
          {"H", GameUtils.WorldTimeToHours(worldTime)},
          {"M", GameUtils.WorldTimeToMinutes(worldTime)}
        };

      if (options.ContainsKey("t") || options.ContainsKey("s")) return;

      Ticks = Math.Round(UnityEngine.Time.timeSinceLevelLoad, 2);
      Fps = Math.Round(GameManager.Instance.fps.Counter, 2);
      Clients = ConnectionManager.Instance.ClientCount();
      Entities = world.Entities.Count;
      EntityInst = Entity.InstanceCount;
      Players = world.Players.list.Count;
      Enemies = GameStats.GetInt(EnumGameStats.EnemyCount);
      Observers = world.m_ChunkManager.m_ObservedEntities.Count;
      Chunks = world.ChunkClusters[0].Count();
      ChunkInst = Chunk.InstanceCount;
      Objects = world.m_ChunkManager.GetDisplayedChunkGameObjectsCount();
      Items = EntityItem.ItemInstanceCount;

      if (!options.ContainsKey("mem")) return;

      //properties will be null without /mem option
      var totalMemory = GC.GetTotalMemory(false);
      Heap = Math.Floor(totalMemory / 1048576f);
      Max = Math.Floor(GameManager.MaxMemoryConsumption / 1048576f);
      Rss = Math.Floor(GetRSS.GetCurrentRSS() / 1048576f);
    }
  }
}
