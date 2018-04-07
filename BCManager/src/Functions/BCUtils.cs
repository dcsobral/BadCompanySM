using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using BCM.Commands;
using BCM.Models;
using LitJson;
using UnityEngine;
using XMLData.Item;

namespace BCM
{
  public static class BCUtils
  {
    private const string UndoDir = "Data/Prefabs/BCMUndoCache";
    private static readonly Dictionary<int, List<BCMPrefabCache>> _undoCache = new Dictionary<int, List<BCMPrefabCache>>();

    public static bool CheckWorld(out World world)
    {
      world = GameManager.Instance.World;
      if (world != null) return true;

      BCCommandAbstract.SendOutput("World not loaded");
      return false;
    }

    public static bool CheckWorld()
    {
      if (GameManager.Instance.World != null) return true;

      BCCommandAbstract.SendOutput("World not loaded");
      return false;
    }

    public static void CreateUndo(EntityPlayer sender, Vector3i pos, Vector3i size)
    {
      var steamId = "_server";
      if (BCCommandAbstract.SenderInfo.RemoteClientInfo != null)
      {
        steamId = BCCommandAbstract.SenderInfo.RemoteClientInfo.ownerId;
      }

      var userId = 0; // id will be 0 for web console issued commands
      var areaCache = BCExport.CopyFromWorld(GameManager.Instance.World, pos, size);

      if (sender != null)
      {
        userId = sender.entityId;
      }
      Directory.CreateDirectory(Utils.GetGameDir(UndoDir));
      var filename = $"{steamId}.{pos.x}.{pos.y}.{pos.z}.{DateTime.UtcNow.ToFileTime()}";
      areaCache.Save(Utils.GetGameDir(UndoDir), filename);

      if (_undoCache.ContainsKey(userId))
      {
        _undoCache[userId].Add(new BCMPrefabCache(filename, pos));
      }
      else
      {
        _undoCache.Add(userId, new List<BCMPrefabCache> { new BCMPrefabCache(filename, pos) });
      }
    }

    public static bool UndoSetBlocks(EntityPlayer sender)
    {
      var userId = 0;
      if (sender != null)
      {
        userId = sender.entityId;
      }
      if (!_undoCache.ContainsKey(userId)) return false;

      if (_undoCache[userId].Count <= 0) return false;

      var pCache = _undoCache[userId][_undoCache[userId].Count - 1];
      if (pCache != null)
      {
        var prefab = new Prefab();
        prefab.Load(Utils.GetGameDir(UndoDir), pCache.Filename);
        BCImport.InsertPrefab(GameManager.Instance.World, prefab, pCache.Pos);

        var cacheFile = Utils.GetGameDir($"{UndoDir}{pCache.Filename}");
        if (Utils.FileExists($"{cacheFile}.tts"))
        {
          Utils.FileDelete($"{cacheFile}.tts");
        }
        if (Utils.FileExists($"{cacheFile}.xml"))
        {
          Utils.FileDelete($"{cacheFile}.xml");
        }
      }
      _undoCache[userId].RemoveAt(_undoCache[userId].Count - 1);

      return true;
    }

    public static Vector3i GetMaxPos(Vector3i pos, Vector3i size) => new Vector3i(pos.x + size.x - 1, pos.y + size.y - 1, pos.z + size.z - 1);

    public static Vector3i GetSize(Vector3i pos1, Vector3i pos2) => new Vector3i(Math.Abs(pos1.x - pos2.x) + 1, Math.Abs(pos1.y - pos2.y) + 1, Math.Abs(pos1.z - pos2.z) + 1);


    public static void WriteVector3i(Vector3i v, JsonWriter w, string format)
    {
      switch (format)
      {
        case "V":
          w.WriteObjectStart();
          w.WritePropertyName("x");
          w.Write(v.x);
          w.WritePropertyName("y");
          w.Write(v.y);
          w.WritePropertyName("z");
          w.Write(v.z);
          w.WriteObjectEnd();
          return;
        case "S":
          w.Write($"{v.x} {v.y} {v.z}");
          return;
        case "W":
          w.Write($"{Math.Abs(v.x)}{(v.x < 0 ? "W" : "E")} {Math.Abs(v.z)}{(v.z > 0 ? "N" : "S")}");
          return;
        case "C":
          w.Write($"{v.x}, {v.y}, {v.z}");
          return;
      }
    }

    public static void WriteVector3(Vector3 v, JsonWriter w, string format)
    {
      switch (format)
      {
        case "V":
          w.WriteObjectStart();
          w.WritePropertyName("x");
          w.Write(Math.Round(v.x, 2));
          w.WritePropertyName("y");
          w.Write(Math.Round(v.y, 2));
          w.WritePropertyName("z");
          w.Write(Math.Round(v.z, 2));
          w.WriteObjectEnd();
          return;
        case "S":
          w.Write($"{Math.Floor(v.x)} {Math.Floor(v.y)} {Math.Floor(v.z)}");
          return;
        case "W":
          w.Write($"{Math.Abs(v.x):0}{(v.x < 0f ? "W" : "E")} {Math.Abs(v.z):0}{(v.z > 0f ? "N" : "S")}");
          return;
        case "C":
          w.Write($"{Math.Floor(v.x)}, {Math.Floor(v.y)}, {Math.Floor(v.z)}");
          return;
      }
    }

    public static object GetVectorObj(BCMVector3 p, IDictionary<string, string> o)
    {
      if (o.ContainsKey("strpos"))
      {
        return p.x + " " + p.y + " " + p.z;
      }
      if (o.ContainsKey("worldpos"))
      {
        return GameUtils.WorldPosToStr(new Vector3(p.x, p.y, p.z), " ");
      }
      if (o.ContainsKey("csvpos"))
      {
        return new[] { p.x, p.y, p.z };
      }
      return p;//vectors
    }

    public static string UIntToHex(uint c)
    {
      return ColorToHex(UIntToColor(c));
    }

    public static Color UIntToColor(uint c)
    {
      //var a = (byte)(c >> 24);
      var r = (byte)(c >> 16);
      var g = (byte)(c >> 8);
      var b = (byte)c;
      return new Color32(r, g, b, 255);
    }

    public static string ColorToHex(Color color)
    {
      return $"{(int)(color.r * 255):X02}{(int)(color.g * 255):X02}{(int)(color.b * 255):X02}";
    }

    public static Dictionary<int, Entity> FilterEntities(Dictionary<int, Entity> entities, Dictionary<string, string> options)
    {
      var filteredEntities = new Dictionary<int, Entity>();

      foreach (var e in entities)
      {
        if (options.ContainsKey("all"))
        {
          if (!options.ContainsKey("minibike") && e.Value is EntityMinibike) continue;

          filteredEntities.Add(e.Key, e.Value);
        }
        else if (options.ContainsKey("type"))
        {
          if (e.Value == null) continue;

          if (e.Value.GetType().ToString() != options["type"]) continue;

          filteredEntities.Add(e.Key, e.Value);
        }
        else if (options.ContainsKey("istype"))
        {
          if (e.Value == null) continue;

          var name = e.Value.GetType().AssemblyQualifiedName;
          if (name == null) continue;

          var type = Type.GetType(name.Replace(e.Value.GetType().ToString(), options["istype"]));
          if (type == null) continue;

          if (!type.IsInstanceOfType(e.Value)) continue;

          filteredEntities.Add(e.Key, e.Value);
        }
        else if (options.ContainsKey("minibike"))
        {
          if (e.Value is EntityMinibike)
          {
            filteredEntities.Add(e.Key, e.Value);
          }
        }
        else if (options.ContainsKey("ecname"))
        {
          var entityClass = EntityClass.list[e.Value.entityClass];
          if (entityClass == null) continue;

          if (options["ecname"] != entityClass.entityClassName) continue;

          filteredEntities.Add(e.Key, e.Value);
        }
        else
        {
          if (!(e.Value is EntityEnemy) && !(e.Value is EntityAnimal)) continue;

          filteredEntities.Add(e.Key, e.Value);
        }
      }

      return filteredEntities;
    }

    public static void GetRadius(BCMCmdArea command, ushort max)
    {
      if (command.Opts.ContainsKey("r"))
      {
        ushort.TryParse(command.Opts["r"], out command.Radius);
        if (command.Radius > max)
        {
          command.Radius = max;
        }
        BCCommandAbstract.SendOutput($"Setting radius to +{command.Radius}");
      }
      else if (!command.Opts.ContainsKey("loc"))
      {
        BCCommandAbstract.SendOutput("Setting radius to default of +0");
      }
      else
      {
        BCCommandAbstract.SendOutput("Using stored location for bounding box");
      }
    }

    public static bool GetChunkSizeXyzw(BCMCmdArea command)
    {
      if (!int.TryParse(command.Pars[1], out var x) || !int.TryParse(command.Pars[2], out var y) || !int.TryParse(command.Pars[3], out var z) || !int.TryParse(command.Pars[4], out var w))
      {
        BCCommandAbstract.SendOutput("Unable to parse x,z x2,z2 into ints");

        return false;
      }

      command.ChunkBounds = new BCMVector4(Math.Min(x, z), Math.Min(y, w), Math.Max(x, z), Math.Max(y, w));
      command.HasChunkPos = true;

      return true;
    }

    public static bool GetChunkPosXz(BCMCmdArea command)
    {
      if (!int.TryParse(command.Pars[1], out var x) || !int.TryParse(command.Pars[2], out var z))
      {
        BCCommandAbstract.SendOutput("Unable to parse x,z into ints");

        return false;
      }

      command.ChunkBounds = new BCMVector4(x - command.Radius, z - command.Radius, x + command.Radius, z + command.Radius);
      command.HasChunkPos = true;

      return true;
    }

    public static bool GetPositionXyz(BCMCmdArea command)
    {
      if (!int.TryParse(command.Pars[1], out var x) || !int.TryParse(command.Pars[2], out var y) || !int.TryParse(command.Pars[3], out var z))
      {
        BCCommandAbstract.SendOutput("Unable to parse x,y,z into ints");

        return false;
      }

      command.Position = new BCMVector3(x, y, z);
      command.HasPos = true;

      command.ChunkBounds = new BCMVector4(World.toChunkXZ(x), World.toChunkXZ(z), World.toChunkXZ(x), World.toChunkXZ(z));
      command.HasChunkPos = true;

      return !command.Opts.ContainsKey("item") || command.Command != "additem" || GetItemStack(command);
    }

    public static bool GetItemStack(BCMCmdArea command)
    {
      var quality = -1;
      var count = 1;
      if (command.Opts.ContainsKey("q"))
      {
        if (!int.TryParse(command.Opts["q"], out quality))
        {
          BCCommandAbstract.SendOutput("Unable to parse quality, using random value");
        }
      }
      if (command.Opts.ContainsKey("c"))
      {
        if (!int.TryParse(command.Opts["c"], out count))
        {
          BCCommandAbstract.SendOutput($"Unable to parse count, using default value of {count}");
        }
      }

      var ic = int.TryParse(command.Opts["item"], out var id) ? ItemClass.GetForId(id) : ItemData.GetForName(command.Opts["item"]);
      if (ic == null)
      {
        BCCommandAbstract.SendOutput($"Unable to get item or block from given value '{command.Opts["item"]}'");

        return false;
      }

      command.ItemStack = new ItemStack
      {
        itemValue = new ItemValue(ic.Id, true),
        count = count <= ic.Stacknumber.Value ? count : ic.Stacknumber.Value
      };
      if (command.ItemStack.count < count)
      {
        BCCommandAbstract.SendOutput("Using max stack size for " + ic.Name + " of " + command.ItemStack.count);
      }
      if (command.ItemStack.itemValue.HasQuality && quality > 0)
      {
        command.ItemStack.itemValue.Quality = quality;
      }

      return true;
    }

    public static bool GetPosSizeXyz(BCMCmdArea command)
    {
      if (!int.TryParse(command.Pars[1], out var x) || !int.TryParse(command.Pars[2], out var y) || !int.TryParse(command.Pars[3], out var z))
      {
        BCCommandAbstract.SendOutput("Unable to parse x,y,z into ints");

        return false;
      }

      if (!int.TryParse(command.Pars[4], out var x2) || !int.TryParse(command.Pars[5], out var y2) || !int.TryParse(command.Pars[6], out var z2))
      {
        BCCommandAbstract.SendOutput("Unable to parse x2,y2,z2 into ints");

        return false;
      }

      command.Position = new BCMVector3(Math.Min(x, x2), Math.Min(y, y2), Math.Min(z, z2));
      command.HasPos = true;

      command.Size = new BCMVector3(Math.Abs(x - x2) + 1, Math.Abs(y - y2) + 1, Math.Abs(z - z2) + 1);
      command.HasSize = true;

      command.ChunkBounds = new BCMVector4(
        World.toChunkXZ(Math.Min(x, x2)),
        World.toChunkXZ(Math.Min(z, z2)),
        World.toChunkXZ(Math.Max(x, x2)),
        World.toChunkXZ(Math.Max(z, z2))
      );
      command.HasChunkPos = true;

      return true;
    }

    public static bool GetIds(World world, BCMCmdArea command, out EntityPlayer entity)
    {
      entity = null;
      int? entityId = null;

      if (command.Opts.ContainsKey("id"))
      {
        if (!PlayerStore.GetId(command.Opts["id"], out command.SteamId, "CON")) return false;

        entityId = ConsoleHelper.ParseParamSteamIdOnline(command.SteamId)?.entityId;
      }

      if (command.SteamId == null)
      {
        entityId = BCCommandAbstract.SenderInfo.RemoteClientInfo?.entityId;
        command.SteamId = BCCommandAbstract.SenderInfo.RemoteClientInfo?.playerId;
      }

      if (entityId != null)
      {
        entity = world.Players.dict[(int)entityId];
      }

      return entity != null || BCCommandAbstract.Params.Count >= 3;
    }

    public static bool GetEntPos(BCMCmdArea command, EntityPlayer entity)
    {
      //todo: if /h=#,# then set y to pos + [0], y2 to pos + [1], if only 1 number then y=pos y2=pos+#
      if (entity != null)
      {
        var loc = new Vector3i(int.MinValue, 0, int.MinValue);
        var hasLoc = false;
        if (command.Opts.ContainsKey("loc"))
        {
          loc = BCLocation.GetPos(BCCommandAbstract.SenderInfo.RemoteClientInfo?.playerId);
          if (loc.x == int.MinValue)
          {
            BCCommandAbstract.SendOutput("No location stored or player not found. Use bc-loc to store a location.");

            return false;
          }
          hasLoc = true;

          command.Position = new BCMVector3(Math.Min(loc.x, (int)entity.position.x), Math.Min(loc.y, (int)entity.position.y), Math.Min(loc.z, (int)entity.position.z));
          command.HasPos = true;

          command.Size = new BCMVector3(
            Math.Abs(loc.x - Utils.Fastfloor(entity.position.x)) + 1,
            Math.Abs(loc.y - Utils.Fastfloor(entity.position.y)) + 1,
            Math.Abs(loc.z - Utils.Fastfloor(entity.position.z)) + 1
          );
          command.HasSize = true;
        }

        command.ChunkBounds = new BCMVector4
        {
          x = World.toChunkXZ(hasLoc ? Math.Min(loc.x, (int)entity.position.x) : (int)entity.position.x - command.Radius),
          y = World.toChunkXZ(hasLoc ? Math.Min(loc.z, (int)entity.position.z) : (int)entity.position.z - command.Radius),
          z = World.toChunkXZ(hasLoc ? Math.Max(loc.x, (int)entity.position.x) : (int)entity.position.x + command.Radius),
          w = World.toChunkXZ(hasLoc ? Math.Max(loc.z, (int)entity.position.z) : (int)entity.position.z + command.Radius)
        };
        command.HasChunkPos = true;
      }
      else
      {
        BCCommandAbstract.SendOutput("Unable to get a position");

        return false;
      }

      return true;
    }

    public static Dictionary<long, Chunk> GetAffectedChunks(BCMCmdArea command, World world)
    {
      var modifiedChunks = new Dictionary<long, Chunk>();

      var timeout = 2000;
      if (command.Opts.ContainsKey("timeout"))
      {
        if (command.Opts["timeout"] != null)
        {
          int.TryParse(command.Opts["timeout"], out timeout);
        }
      }

      ChunkObserver(command, world, timeout / 2000);

      //todo: needs to calc ChunkBounds if not set but has pos and size?

      //request any unloaded chunks in area
      for (var x = command.ChunkBounds.x; x <= command.ChunkBounds.z; x++)
      {
        for (var z = command.ChunkBounds.y; z <= command.ChunkBounds.w; z++)
        {
          var chunkKey = WorldChunkCache.MakeChunkKey(x, z);
          modifiedChunks.Add(chunkKey, null);
        }
      }

      var chunkCount = (command.ChunkBounds.z - command.ChunkBounds.x + 1) * (command.ChunkBounds.w - command.ChunkBounds.y + 1);
      var count = 0;
      var sw = Stopwatch.StartNew();

      while (count < chunkCount && sw.ElapsedMilliseconds < timeout)
      {
        for (var x = command.ChunkBounds.x; x <= command.ChunkBounds.z; x++)
        {
          for (var z = command.ChunkBounds.y; z <= command.ChunkBounds.w; z++)
          {
            //check if already in list
            var chunkKey = WorldChunkCache.MakeChunkKey(x, z);
            if (modifiedChunks.ContainsKey(chunkKey) && modifiedChunks[chunkKey] != null) continue;

            //check if chunk has loaded
            if (!world.ChunkCache.ContainsChunkSync(chunkKey)) continue;

            modifiedChunks[chunkKey] = world.GetChunkSync(chunkKey) as Chunk;
            if (modifiedChunks[chunkKey] != null)
            {
              count++;
            }
          }
        }
      }

      sw.Stop();

      if (count < chunkCount)
      {
        BCCommandAbstract.SendOutput($"Unable to load {chunkCount - count}/{chunkCount} chunks");

        return null;
      }

      BCCommandAbstract.SendOutput($"Loading {chunkCount} chunks took {Math.Round(sw.ElapsedMilliseconds / 1000f, 2)} seconds");

      return modifiedChunks;
    }

    public static void ChunkObserver(BCMCmdArea command, World world, int timeoutSec)
    {
      var pos = command.HasPos ? command.Position.ToV3() : command.ChunkBounds.ToV3();
      var viewDim = !command.Opts.ContainsKey("r") ? command.Radius : command.ChunkBounds.GetRadius();
      var chunkObserver = world.m_ChunkManager.AddChunkObserver(pos, false, viewDim, -1);

      var timerSec = 60;
      if (command.Opts.ContainsKey("ts") && command.Opts["ts"] != null)
      {
        int.TryParse(command.Opts["ts"], out timerSec);
      }
      timerSec += timeoutSec;
      BCTask.AddTask(
        command.CmdType,
        ThreadManager.AddSingleTask(
          info => DoCleanup(world, chunkObserver, command.CmdType, timerSec, info),
          null,
          (info, e) => BCTask.DelTask(command.CmdType, info.GetHashCode(), 120)
        ).GetHashCode(),
        command);
    }

    public static void DoCleanup(World world, ChunkManager.ChunkObserver co, string commandType, int ts = 0, ThreadManager.TaskInfo taskInfo = null)
    {
      var bcmTask = BCTask.GetTask(commandType, taskInfo?.GetHashCode());
      for (var i = 0; i < ts; i++)
      {
        if (bcmTask != null) bcmTask.Output = new { timer = i, total = ts };
        Thread.Sleep(1000);
      }
      world.m_ChunkManager.RemoveChunkObserver(co);
    }

    public static bool ProcessParams(BCMCmdArea command, ushort maxRadius)
    {
      if (command.Opts.ContainsKey("type"))
      {
        command.Filter = command.Opts["type"];
      }
      switch (command.Pars.Count)
      {
        case 1:
          //command with no extras, blocks if /loc, chunks if /r= or nothing
          GetRadius(command, maxRadius);
          command.Command = command.Pars[0];
          return true;

        case 3:
          //XZ single chunk with /r
          command.Command = command.Pars[0];
          GetRadius(command, maxRadius);
          return GetChunkPosXz(command);

        case 4:
          //XYZ single block
          command.Command = command.Pars[0];
          return GetPositionXyz(command);

        case 5:
          //XY-ZW multi chunk
          command.Command = command.Pars[0];
          return GetChunkSizeXyzw(command);

        case 7:
          //XYZ-XYZ world pos bounds
          command.Command = command.Pars[0];
          return GetPosSizeXyz(command);

        default:
          return false;
      }
    }

    public static void DoProcess(World world, BCMCmdArea command, BCCommandAbstract cmdRef)
    {
      if (command.Opts.ContainsKey("forcesync"))
      {
        BCCommandAbstract.SendOutput("Processing Command synchronously...");
        ProcessCommand(world, command, cmdRef);

        return;
      }

      if (BCCommandAbstract.SenderInfo.NetworkConnection != null && !(BCCommandAbstract.SenderInfo.NetworkConnection is TelnetConnection))
      {
        BCCommandAbstract.SendOutput("Processing Async Command... Sending output to log");
      }
      else
      {
        BCCommandAbstract.SendOutput("Processing Async Command...");
      }

      BCTask.AddTask(
        command.CmdType,
        ThreadManager.AddSingleTask(
            info => ProcessCommand(world, command, cmdRef),
            null,
            (info, e) => BCTask.DelTask(command.CmdType, info.GetHashCode()))
          .GetHashCode(),
        command);
    }

    public static void ProcessCommand(World world, BCMCmdArea command, BCCommandAbstract cmdRef)
    {
      var affectedChunks = GetAffectedChunks(command, world);
      if (affectedChunks == null)
      {
        BCCommandAbstract.SendOutput("Aborting, unable to load all chunks in area before timeout.");
        BCCommandAbstract.SendOutput("Use /timeout=#### to override the default 2000 millisecond limit, or wait for the requested chunks to finish loading and try again.");

        return;
      }

      var location = "";
      if (command.HasPos)
      {
        location += $"Pos {command.Position.x} {command.Position.y} {command.Position.z} ";
        if (command.HasSize)
        {
          var maxPos = command.MaxPos;
          location += $"to {maxPos.x} {maxPos.y} {maxPos.z} ";
        }
      }
      if (command.HasChunkPos)
      {
        location += $"Chunks {command.ChunkBounds.x} {command.ChunkBounds.y} to {command.ChunkBounds.z} {command.ChunkBounds.w}";
      }
      if (!string.IsNullOrEmpty(location))
      {
        BCCommandAbstract.SendOutput(location);
      }

      cmdRef.ProcessSwitch(world, command, out var reload);

      //RELOAD CHUNKS FOR PLAYER(S) - If steamId is empty then all players in area will get reload
      if (reload != ReloadMode.None && !(command.Opts.ContainsKey("noreload") || command.Opts.ContainsKey("nr")))
      {
        BCChunks.ReloadForClients(affectedChunks, reload == ReloadMode.Target ? command.SteamId : string.Empty);
      }
    }

    public static string GetIPAddress()
    {
      var host = Dns.GetHostEntry(Dns.GetHostName());
      foreach (var ip in host.AddressList)
      {
        if (ip.AddressFamily == AddressFamily.InterNetwork)
        {
          return ip.ToString();
        }
      }
      return "";
    }

  }
}
