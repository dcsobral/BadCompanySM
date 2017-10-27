using System;
using System.Collections.Generic;
using System.Linq;
using SystemInformation;
using UnityEngine;

namespace BCM.Commands
{
  public class BCTime : BCCommandAbstract
  {
    public class BCMTime
    {
      public readonly Dictionary<string, int> Time;
      public double Ticks;
      public double Fps;
      public int Clients;
      public int Entities;
      public int EntityInst;
      public int Players;
      public int Enemies;
      public int Observers;
      public int Chunks;
      public int ChunkInst;
      public int Objects;
      public int Items;
      public double Heap;
      public double Max;
      public double Rss;

      public BCMTime()
      {
        var world = GameManager.Instance.World;
        if (world == null) return;

        var worldTime = world.worldTime;
        Time = new Dictionary<string, int>
        {
          {"D", GameUtils.WorldTimeToDays(worldTime)},
          {"H", GameUtils.WorldTimeToHours(worldTime)},
          {"M", GameUtils.WorldTimeToMinutes(worldTime)}
        };

        if (Options.ContainsKey("t") || Options.ContainsKey("s")) return;

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

        if (!Options.ContainsKey("mem")) return;

        //properties will be null without /mem option
        var totalMemory = GC.GetTotalMemory(false);
        Heap = Math.Floor(totalMemory / 1048576f);
        Max = Math.Floor(GameManager.MaxMemoryConsumption / 1048576f);
        Rss = Math.Floor(GetRSS.GetCurrentRSS() / 1048576f);
      }
    }

    public override void Process()
    {
      var time = new BCMTime();

      if (Options.ContainsKey("t"))
      {
        SendJson(time.Time);
      }
      else if (Options.ContainsKey("s"))
      {
        SendJson(new[] { time.Time["D"], time.Time["H"], time.Time["M"] });
      }
      else
      {
        SendJson(time);
      }
    }
  }
}
