using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using BCM.Models;

namespace BCM.Commands
{
  public class BCTileEntity : BCCommandAbstract
  {
    #region Properties
    public class BCTileEntityCmd : BCCmd
    {
      public string Filter;
      public ushort Radius;

      public bool HasPos;
      public BCMVector3 Position;

      public bool HasSize;
      public BCMVector3 Size;

      public bool HasChunkPos;
      public BCMVector4 ChunkBounds;
      public ItemStack ItemStack;

      public bool IsWithinBounds(Vector3i pos)
      {
        if (!HasPos) return false;

        var size = HasSize ? Size : new BCMVector3(0, 0, 0);
        return (Position.x <= pos.x && pos.x <= Position.x + size.x) &&
               (Position.y <= pos.y && pos.y <= Position.y + size.y) &&
               (Position.z <= pos.z && pos.z <= Position.z + size.z);
      }
    }

    private enum ReloadMode
    {
      None,
      Target,
      All
    }

    private class BCMParts
    {
      public int Type;
      public int Quality;
      public int UseTimes;
      public int MaxUse;
    }

    private class BCMAttachment
    {
      public int Type;
      public int Quality;
      public int UseTimes;
      public int MaxUse;
    }

    private class BCMItemStack
    {
      public int Type;
      public int Quality;
      public int UseTimes;
      public int MaxUse;
      public int AmmoIndex;
      public int Count;
      public List<BCMAttachment> Attachments;
      public List<BCMParts> Parts;

      public BCMItemStack(ItemStack item)
      {
        if (item.itemValue.type == 0) return;

        Type = item.itemValue.type;
        Quality = item.itemValue.Quality;
        UseTimes = item.itemValue.UseTimes;
        MaxUse = item.itemValue.MaxUseTimes;
        AmmoIndex = item.itemValue.SelectedAmmoTypeIndex;

        if (item.itemValue.Attachments != null && item.itemValue.Attachments.Length > 0)
        {
          Attachments = new List<BCMAttachment>();
          foreach (var attachment in item.itemValue.Attachments)
          {
            if (attachment != null && attachment.type != 0)
            {
              Attachments.Add(new BCMAttachment
              {
                Type = attachment.type,
                Quality = attachment.Quality,
                MaxUse = attachment.MaxUseTimes,
                UseTimes = attachment.UseTimes
              });
            }
          }
        }

        if (item.itemValue.Parts != null && item.itemValue.Parts.Length > 0)
        {
          Parts = new List<BCMParts>();
          foreach (var part in item.itemValue.Parts)
          {
            if (part != null && part.type != 0)
            {
              Parts.Add(new BCMParts
              {
                Type = part.type,
                Quality = part.Quality,
                MaxUse = part.MaxUseTimes,
                UseTimes = part.UseTimes
              });
            }
          }
        }

        Count = item.count;
      }
    }

    private class BCMTileEntityLootContainer : BCMTileEntity
    {
      public int LootList;
      public bool Touched;
      public ulong TimeTouched;
      public BCMVector2 Size;
      public double OpenTime;
      public List<BCMItemStack> Items;

      public BCMTileEntityLootContainer(Vector3i pos, TileEntityLootContainer te) : base(pos, te)
      {
        LootList = te.lootListIndex;
        Touched = te.bWasTouched;
        TimeTouched = te.worldTimeTouched;
        Size = new BCMVector2(te.GetContainerSize());
        OpenTime = te.GetOpenTime();

        Items = new List<BCMItemStack>();
        foreach (var itemStack in te.GetItems())
        {
          if (itemStack.itemValue.type == 0) continue;

          Items.Add(new BCMItemStack(itemStack));
        }
      }
    }

    private class BCMTileEntitySecureLootContainer : BCMTileEntityLootContainer
    {
      public string Owner;
      public List<string> Users;
      public bool HasPassword;
      public bool IsLocked;

      public BCMTileEntitySecureLootContainer(Vector3i pos, TileEntitySecureLootContainer te) : base(pos, te)
      {
        Owner = te.GetOwner();
        Users = te.GetUsers();
        HasPassword = te.HasPassword();
        IsLocked = te.IsLocked();
      }
    }

    private class BCMTileEntityTrader : BCMTileEntity
    {
      public int Money;
      public ulong ResetTime;
      public int TraderId;
      public bool IsOpen;
      public bool PlayerOwned;
      public List<BCMItemStack> Inventory;
      public List<List<BCMItemStack>> TierGroups;

      public BCMTileEntityTrader(Vector3i pos, TileEntityTrader te) : base(pos, te)
      {
        Money = te.TraderData.AvailableMoney;
        ResetTime = te.TraderData.NextResetTime;
        TraderId = te.TraderData.TraderID;
        IsOpen = te.TraderData.TraderInfo.IsOpen;
        PlayerOwned = te.TraderData.TraderInfo.PlayerOwned;
        Inventory = new List<BCMItemStack>();
        foreach (var itemStack in te.TraderData.PrimaryInventory)
        {
          Inventory.Add(new BCMItemStack(itemStack));
        }

        TierGroups = new List<List<BCMItemStack>>();
        foreach (var tierGroup in te.TraderData.TierItemGroups)
        {
          TierGroups.Add(tierGroup.Select(itemStack => new BCMItemStack(itemStack)).ToList());
        }
      }
    }

    private class BCMTileEntityVendingMachine : BCMTileEntityTrader
    {
      public string Owner;
      public List<string> Users;
      public bool HasPassword;
      public bool IsLocked;

      public BCMTileEntityVendingMachine(Vector3i pos, TileEntityVendingMachine te) : base(pos, te)
      {
        Owner = te.GetOwner();
        Users = te.GetUsers();
        HasPassword = te.HasPassword();
        IsLocked = te.IsLocked();
      }
    }

    private class BCMTileEntityForge : BCMTileEntity
    {
      public BCMTileEntityForge(Vector3i pos, TileEntityForge te) : base(pos, te)
      {
        //todo
        //te.GetBurningItemValue();
        //te.GetFuel();
        //te.GetInput();
        //te.GetOutput();
      }
    }

    private class BCMTileEntityCampfire : BCMTileEntity
    {
      public BCMTileEntityCampfire(Vector3i pos, TileEntityCampfire te) : base(pos, te)
      {
        //todo
        //te.GetBurningItemValue();
        //te.GetFuel();
        //te.GetInput();
        //te.GetOutput();
        //te.IsBurning;
        //te.isCooking;
        //te.GetCurrentRecipe();
        //te.GetUtensil();
      }
    }

    private class BCMTileEntitySecureDoor : BCMTileEntity
    {
      public string Owner;
      public List<string> Users;
      public bool HasPassword;
      public bool IsLocked;

      public BCMTileEntitySecureDoor(Vector3i pos, TileEntitySecureDoor te) : base(pos, te)
      {
        Owner = te.GetOwner();
        Users = te.GetUsers();
        HasPassword = te.HasPassword();
        IsLocked = te.IsLocked();
      }
    }

    private class BCMTileEntityWorkstation : BCMTileEntity
    {
      public BCMTileEntityWorkstation(Vector3i pos, TileEntityWorkstation te) : base(pos, te)
      {
        //todo
        //te.BurnTimeLeft;
        //te.BurnTotalTimeLeft;
        //te.Fuel;
        //te.Input;
        //te.IsBurning;
        //te.MaterialNames;
        //te.Output;
        //te.Queue;
        //te.Tools;
      }
    }

    private class BCMTileEntitySign : BCMTileEntity
    {
      public string Owner;
      public List<string> Users;
      public bool HasPassword;
      public bool IsLocked;
      public string Text;

      public BCMTileEntitySign(Vector3i pos, TileEntitySign te) : base(pos, te)
      {
        Owner = te.GetOwner();
        Users = te.GetUsers();
        HasPassword = te.HasPassword();
        IsLocked = te.IsLocked();
        Text = te.GetText();
      }
    }

    private class BCMTileEntityGoreBlock : BCMTileEntity
    {
      public int LootList;
      public bool Touched;
      public ulong TimeTouched;
      public BCMVector2 Size;
      public double OpenTime;
      public List<BCMItemStack> Items;

      public BCMTileEntityGoreBlock(Vector3i pos, TileEntityGoreBlock te) : base(pos, te)
      {
        LootList = te.lootListIndex;
        Touched = te.bWasTouched;
        TimeTouched = te.worldTimeTouched;
        Size = new BCMVector2(te.GetContainerSize());
        OpenTime = te.GetOpenTime();

        Items = new List<BCMItemStack>();
        foreach (var itemStack in te.GetItems())
        {
          if (itemStack.itemValue.type == 0) continue;

          Items.Add(new BCMItemStack(itemStack));
        }
      }
    }

    private class BCMTileEntityPowered : BCMTileEntity
    {
      public BCMTileEntityPowered(Vector3i pos, TileEntityPowered te) : base(pos, te)
      {
        //todo
      }
    }

    private class BCMTileEntityPowerSource : BCMTileEntity
    {
      public BCMTileEntityPowerSource(Vector3i pos, TileEntityPowerSource te) : base(pos, te)
      {
        //var powerItem = te.GetPowerItem();
        //todo
      }
    }

    private class BCMTileEntityPoweredRangeTrap : BCMTileEntity
    {
      public BCMTileEntityPoweredRangeTrap(Vector3i pos, TileEntityPoweredRangedTrap te) : base(pos, te)
      {
        //todo
      }
    }

    private class BCMTileEntityPoweredTrigger : BCMTileEntity
    {
      public BCMTileEntityPoweredTrigger(Vector3i pos, TileEntityPoweredTrigger te) : base(pos, te)
      {
        //todo
      }
    }
    #endregion

    public override void Process()
    {
      var world = GameManager.Instance.World;
      if (world == null)
      {
        SendOutput("World not initialized.");

        return;
      }

      var command = new BCTileEntityCmd();
      if (!ProcessParams(command)) return;

      if (!GetIds(world, command, out var entity))
      {
        SendOutput("Command requires a position when not run by a player.");

        return;
      }

      if (!command.HasChunkPos && !command.HasPos && !GetEntPos(command, entity))
      {
        SendOutput("Unable to get position.");

        return;
      }

      DoProcess(world, command);
    }

    private void DoProcess(World world, BCTileEntityCmd command)
    {
      if (Options.ContainsKey("forcesync"))
      {
        SendOutput("Processing Command synchronously...");
        ProcessCommand(world, command);

        return;
      }

      if (SenderInfo.NetworkConnection != null && !(SenderInfo.NetworkConnection is TelnetConnection))
      {
        SendOutput("Processing Async Command... Sending output to log");
      }
      else
      {
        SendOutput("Processing Async Command...");
      }

      BCTask.AddTask(
        "TileEntity",
        ThreadManager.AddSingleTask(
            info => ProcessCommand(world, command), 
            null,
            (info, e) => BCTask.DelTask("TileEntity", info.GetHashCode()))
          .GetHashCode(),
        command);
    }

    private void ProcessCommand(World world, BCTileEntityCmd command)
    {
      var affectedChunks = GetAffectedChunks(command, world);
      if (affectedChunks == null)
      {
        SendOutput("Aborting, unable to load all chunks in area before timeout.");
        SendOutput("Use /timeout=#### to override the default 2000 millisecond limit, or wait for the requested chunks to finish loading and try again.");

        return;
      }

      var location = "";
      if (command.HasPos)
      {
        location += $"Pos {command.Position.x} {command.Position.y} {command.Position.z} ";
        if (command.HasSize)
        {
          location += $"to {command.Position.x + command.Size.x} {command.Position.y + command.Size.y} {command.Position.z + command.Size.z} ";
        }
      }
      if (command.HasChunkPos)
      {
        location += $"Chunks {command.ChunkBounds.x} {command.ChunkBounds.y} to {command.ChunkBounds.z} {command.ChunkBounds.w}";
      }
      if (!string.IsNullOrEmpty(location))
      {
        SendOutput(location);
      }

      var reload = ReloadMode.None;

      switch (command.Command)
      {
        case "owner":
          SetOwner(command, world);
          reload = ReloadMode.Target;
          break;
        case "access":
          GrantAccess(command, world);
          reload = ReloadMode.Target;
          break;
        case "revoke":
          RevokeAccess(command, world);
          reload = ReloadMode.Target;
          break;
        case "lock":
          Options.TryGetValue("pwd", out var pwd);
          SetLocked(command, true, Options.ContainsKey("pwd"), pwd ?? string.Empty, world);
          reload = ReloadMode.All;
          break;
        case "unlock":
          SetLocked(command, false, false, "", world);
          reload = ReloadMode.All;
          break;
        case "empty":
          EmptyContainers(command, world);
          break;
        case "reset":
          ResetTouched(command, world);
          break;
        case "remove":
          if (!Options.ContainsKey("confirm"))
          {
            SendOutput("Please use /confirm to confirm that you understand the impact of this command (removes all targeted tile entities)");
          }
          else
          {
            RemoveTiles(command, world);
            reload = ReloadMode.All;
          }
          break;
        case "scan":
          ScanTiles(command, world);
          break;
        case "additem":
          AddLoot(command, world);
          //todo: more options
          //      add to first empty
          //      or add at specific slot
          //      Specific slot can be use to adjust stack size or type of itemvalue
          break;
        case "removeitem":
          //todo: removes items from a lootcontainer
          // clears the given slot, or the first stack of a type give, or reduces the total inventory count for a type starting at smallest stack first.
          break;
        default:
          SendOutput($"Unknown param {command.Command}");
          SendOutput(GetHelp());
          break;
      }

      //RELOAD CHUNKS FOR PLAYER(S) - If steamId is empty then all players in area will get reload
      if (reload != ReloadMode.None && !(Options.ContainsKey("noreload") || Options.ContainsKey("nr")))
      {
        BCChunks.ReloadForClients(affectedChunks, reload == ReloadMode.Target ? command.SteamId : string.Empty);
      }
    }

    private static void DoCleanup(World world, ChunkManager.ChunkObserver co, int ts = 0, ThreadManager.TaskInfo taskInfo = null)
    {
      var bcmTask = BCTask.GetTask("TileEntity", taskInfo?.GetHashCode());
      for (var i = 0; i < ts; i++)
      {
        if (bcmTask != null) bcmTask.Output = new { timer = i, total = ts };
        Thread.Sleep(1000);
      }
      world.m_ChunkManager.RemoveChunkObserver(co);
    }

    #region Params

    private void GetRadius(BCTileEntityCmd command)
    {
      if (Options.ContainsKey("r"))
      {
        ushort.TryParse(Options["r"], out command.Radius);
        if (command.Radius > 14)
        {
          command.Radius = 14;
          SendOutput("Setting radius to maximum of +14 chunks");
        }
        else
        {
          SendOutput($"Setting radius to +{command.Radius} chunks");
        }
      }
      else
      {
        SendOutput("Setting radius to default of +0 chunk");
      }
    }

    private static bool GetChunkSizeXyzw(BCTileEntityCmd command)
    {
      if (!int.TryParse(Params[1], out var x) || !int.TryParse(Params[2], out var y) || !int.TryParse(Params[3], out var z) || !int.TryParse(Params[4], out var w))
      {
        SendOutput("Unable to parse x,z x2,z2 into ints");

        return false;
      }

      command.ChunkBounds = new BCMVector4(Math.Min(x, z), Math.Min(y, w), Math.Max(x, z), Math.Max(y, w));
      command.HasChunkPos = true;

      return true;
    }

    private static bool GetChunkPosXz(BCTileEntityCmd command)
    {
      if (!int.TryParse(Params[1], out var x) || !int.TryParse(Params[2], out var z))
      {
        SendOutput("Unable to parse x,z into ints");

        return false;
      }
      
      command.ChunkBounds = new BCMVector4(x - command.Radius, z - command.Radius, x + command.Radius, z + command.Radius);
      command.HasChunkPos = true;

      return true;
    }

    private static bool GetPositionXyz(BCTileEntityCmd command)
    {
      if (!int.TryParse(Params[1], out var x) || !int.TryParse(Params[2], out var y) || !int.TryParse(Params[3], out var z))
      {
        SendOutput("Unable to parse x,y,z into ints");

        return false;
      }

      command.Position = new BCMVector3(x, y, z);
      command.HasPos = true;

      command.ChunkBounds = new BCMVector4(World.toChunkXZ(x), World.toChunkXZ(z), World.toChunkXZ(x), World.toChunkXZ(z));
      command.HasChunkPos = true;

      return !Options.ContainsKey("item") || command.Command != "additem" || GetItemStack(command);
    }

    private static bool GetItemStack(BCTileEntityCmd command)
    {
      var quality = -1;
      var count = 1;
      if (Options.ContainsKey("q"))
      {
        if (!int.TryParse(Options["q"], out quality))
        {
          SendOutput("Unable to parse quality, using random value");
        }
      }
      if (Options.ContainsKey("c"))
      {
        if (!int.TryParse(Options["c"], out count))
        {
          SendOutput($"Unable to parse count, using default value of {count}");
        }
      }

      var ic = int.TryParse(Options["item"], out var id) ? ItemClass.GetForId(id) : XMLData.Item.ItemData.GetForName(Options["item"]);
      if (ic == null)
      {
        SendOutput($"Unable to get item or block from given value '{Options["item"]}'");

        return false;
      }

      command.ItemStack = new ItemStack
      {
        itemValue = new ItemValue(ic.Id, true),
        count = count <= ic.Stacknumber.Value ? count : ic.Stacknumber.Value
      };
      if (command.ItemStack.count < count)
      {
        SendOutput("Using max stack size for " + ic.Name + " of " + command.ItemStack.count);
      }
      if (command.ItemStack.itemValue.HasQuality && quality > 0)
      {
        command.ItemStack.itemValue.Quality = quality;
      }

      return true;
    }

    private static bool GetPosSizeXyz(BCTileEntityCmd command)
    {
      if (!int.TryParse(Params[1], out var x) || !int.TryParse(Params[2], out var y) || !int.TryParse(Params[3], out var z))
      {
        SendOutput("Unable to parse x,y,z into ints");

        return false;
      }

      if (!int.TryParse(Params[4], out var x2) || !int.TryParse(Params[5], out var y2) || !int.TryParse(Params[6], out var z2))
      {
        SendOutput("Unable to parse x2,y2,z2 into ints");

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

    private bool ProcessParams(BCTileEntityCmd command)
    {
      if (Options.ContainsKey("type"))
      {
        command.Filter = Options["type"];
      }
      switch (Params.Count)
      {
        case 1:
          //command with no extras, blocks if /loc, chunks if /r= or nothing
          GetRadius(command);
          command.Command = Params[0];
          return true;

        case 3:
          //XZ single chunk with /r
          command.Command = Params[0];
          GetRadius(command);
          return GetChunkPosXz(command);

        case 4:
          //XYZ single block
          command.Command = Params[0];
          return GetPositionXyz(command);

        case 5:
          //XZ multi chunk
          command.Command = Params[0];
          return GetChunkSizeXyzw(command);

        case 7:
          //XZYXYZ world pos bounds
          command.Command = Params[0];
          return GetPosSizeXyz(command);

        default:
          SendOutput(GetHelp());
          return false;
      }
    }

    private static bool GetIds(World world, BCTileEntityCmd command, out EntityPlayer entity)
    {
      entity = null;
      int? entityId = null;

      if (Options.ContainsKey("id"))
      {
        if (!PlayerStore.GetId(Options["id"], out command.SteamId, "CON")) return false;

        entityId = ConsoleHelper.ParseParamSteamIdOnline(command.SteamId)?.entityId;
      }

      if (command.SteamId == null)
      {
        entityId = SenderInfo.RemoteClientInfo?.entityId;
        command.SteamId = SenderInfo.RemoteClientInfo?.playerId;
      }

      if (entityId != null)
      {
        entity = world.Players.dict[(int)entityId];
      }

      return entity != null || Params.Count >= 3;
    }

    private static bool GetEntPos(BCTileEntityCmd command, EntityPlayer entity)
    {
      //todo: if /h=#,# then set y to pos + [0], y2 to pos + [1], if only 1 number then y=pos y2=pos+#
      if (entity != null)
      {
        var loc = new Vector3i(int.MinValue, 0, int.MinValue);
        var hasLoc = false;
        if (Options.ContainsKey("loc"))
        {
          loc = BCLocation.GetPos(SenderInfo.RemoteClientInfo?.playerId);
          if (loc.x == int.MinValue)
          {
            SendOutput("No location stored or player not found. Use bc-loc to store a location.");

            return false;
          }
          hasLoc = true;

          command.Position = new BCMVector3((int)entity.position.x, (int)entity.position.y, (int)entity.position.z);
          command.HasPos = true;
          command.Position = new BCMVector3(Math.Min(loc.x, (int)entity.position.x), Math.Min(loc.y, (int)entity.position.y), Math.Min(loc.z, (int)entity.position.z));
          command.HasPos = true;

          command.Size = new BCMVector3(Math.Abs(loc.x - (int)entity.position.x) + 1, Math.Abs(loc.y - (int)entity.position.y) + 1, Math.Abs(loc.z - (int)entity.position.z) + 1);
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
        SendOutput("Unable to get a position");

        return false;
      }

      return true;
    }

    private static Dictionary<long, Chunk> GetAffectedChunks(BCTileEntityCmd command, World world)
    {
      var modifiedChunks = new Dictionary<long, Chunk>();

      var timeout = 1000;
      if (Options.ContainsKey("timeout"))
      {
        if (Options["timeout"] != null)
        {
          int.TryParse(Options["timeout"], out timeout);
        }
      }

      ChunkObserver(command, world, timeout / 1000);

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
      var sw = System.Diagnostics.Stopwatch.StartNew();

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
        SendOutput($"Unable to load {chunkCount - count}/{chunkCount} chunks");

        return null;
      }

      SendOutput($"Loading {chunkCount} chunks took {Math.Round(sw.ElapsedMilliseconds / 1000f, 2)} seconds");

      return modifiedChunks;
    }

    private static void ChunkObserver(BCTileEntityCmd command, World world, int timeoutSec)
    {
      var pos = command.HasPos ? command.Position.ToV3() : command.ChunkBounds.ToV3();
      var viewDim = !Options.ContainsKey("r") ? command.Radius : command.ChunkBounds.GetRadius();
      var chunkObserver = world.m_ChunkManager.AddChunkObserver(pos, false, viewDim, -1);

      var timerSec = 60;
      if (Options.ContainsKey("ts") && Options["ts"] != null)
      {
        int.TryParse(Options["ts"], out timerSec);
      }
      timerSec += timeoutSec;
      BCTask.AddTask(
        "TileEntity",
        ThreadManager.AddSingleTask(
          info => DoCleanup(world, chunkObserver, timerSec, info),
          null,
          (info, e) => BCTask.DelTask("TileEntity", info.GetHashCode(), 120)
        ).GetHashCode(),
        command);
    }
    #endregion

    #region Actions
    private static void AddLoot(BCTileEntityCmd command, World world)
    {
      if (!command.HasPos || command.ItemStack == null)
      {
        SendOutput("AddLoot option requires a specific x y z and a value /item value");

        return;
      }

      for (var x = command.ChunkBounds.x; x <= command.ChunkBounds.z; x++)
      {
        for (var z = command.ChunkBounds.y; z <= command.ChunkBounds.w; z++)
        {
          var chunkSync = world.ChunkCache.GetChunkSync(x, z);
          var tileEntities = chunkSync?.GetTileEntities();
          if (tileEntities == null) return;

          var chunkXyz = new Vector3i(World.toBlockXZ(command.Position.x), command.Position.y, World.toBlockXZ(command.Position.z));
          if (!tileEntities.dict.ContainsKey(chunkXyz))
          {
            SendOutput($"Tile Entity not found at given location {command.Position}");

            return;
          }
          var te = tileEntities.dict[chunkXyz];
          if (te.IsUserAccessing())
          {
            SendOutput("Tile Entity is currently being accessed");

            return;
          }

          if (command.Filter != null &&
              !(command.Filter.Equals("all", StringComparison.OrdinalIgnoreCase) ||
                command.Filter.Equals(te.GetTileEntityType().ToString(), StringComparison.OrdinalIgnoreCase
                ))) return;

          switch (te.GetTileEntityType())
          {
            case TileEntityType.None:
              break;
            case TileEntityType.Loot:
              if (te is TileEntityLootContainer loot)
              {
                if (!loot.AddItem(command.ItemStack))
                {
                  SendOutput("Couldn't add item, container is full?");
                }
              }
              break;
            case TileEntityType.Trader:
              break;
            case TileEntityType.VendingMachine:
              break;
            case TileEntityType.Forge:
              break;
            case TileEntityType.Campfire:
              break;
            case TileEntityType.SecureLoot:
              if (te is TileEntitySecureLootContainer secureLoot)
              {
                if (!secureLoot.AddItem(command.ItemStack))
                {
                  SendOutput("Couldn't add item, container is full?");
                }
              }
              break;
            case TileEntityType.SecureDoor:
              break;
            case TileEntityType.Workstation:
              break;
            case TileEntityType.Sign:
              break;
            case TileEntityType.GoreBlock:
              if (te is TileEntityGoreBlock goreBlock)
              {
                if (!goreBlock.AddItem(command.ItemStack))
                {
                  SendOutput("Couldn't add item, container is full?");
                }
              }
              break;
            case TileEntityType.Powered:
              break;
            case TileEntityType.PowerSource:
              break;
            case TileEntityType.PowerRangeTrap:
              break;
            case TileEntityType.Trigger:
              break;
            default:
              SendOutput($"Error finding TileEntity Type at {command.Position}");
              break;
          }
        }
      }
      SendOutput($"Added to loot container: {command.ItemStack.itemValue.ItemClass.Name} x{command.ItemStack.count} at {command.Position}");
    }

    private static void ScanTiles(BCTileEntityCmd command, World world)
    {
      var count = 0;
      var tiles = new Dictionary<string, List<BCMTileEntity>>();

      for (var x = command.ChunkBounds.x; x <= command.ChunkBounds.z; x++)
      {
        for (var z = command.ChunkBounds.y; z <= command.ChunkBounds.w; z++)
        {
          //todo: get from cache generated when calculating chunks affected
          var chunkSync = world.ChunkCache.GetChunkSync(x, z);
          var tileEntities = chunkSync?.GetTileEntities();
          if (tileEntities == null) continue;

          var worldPos = new Vector3i(x << 4, 0, z << 4);
          foreach (var kvp in tileEntities.dict)
          {
            var pos = kvp.Key + worldPos;
            if (command.HasPos && !command.IsWithinBounds(pos)) continue;
            if (command.Filter != null &&
                !(command.Filter.Equals("all", StringComparison.OrdinalIgnoreCase) ||
                  command.Filter.Equals(kvp.Value.GetTileEntityType().ToString(), StringComparison.OrdinalIgnoreCase
                  ))) continue;

            TileEntity te;
            switch (kvp.Value.GetTileEntityType())
            {
              case TileEntityType.None:
                te = kvp.Value;
                if (te != null)
                {
                  if (!tiles.ContainsKey("None"))
                  {
                    tiles.Add("None", new List<BCMTileEntity>());
                  }
                  tiles["None"].Add(new BCMTileEntity(pos, te));
                }
                count++;
                break;
              case TileEntityType.Loot:
                te = (kvp.Value as TileEntityLootContainer);
                if (te != null)
                {
                  if (!tiles.ContainsKey("Loot"))
                  {
                    tiles.Add("Loot", new List<BCMTileEntity>());
                  }
                  tiles["Loot"].Add(new BCMTileEntityLootContainer(pos, (TileEntityLootContainer)te));
                }
                count++;
                break;
              case TileEntityType.Trader:
                te = (kvp.Value as TileEntityTrader);
                if (te != null)
                {
                  if (!tiles.ContainsKey("Trader"))
                  {
                    tiles.Add("Trader", new List<BCMTileEntity>());
                  }
                  tiles["Trader"].Add(new BCMTileEntityTrader(pos, (TileEntityTrader)te));
                }
                count++;
                break;
              case TileEntityType.VendingMachine:
                te = (kvp.Value as TileEntityVendingMachine);
                if (te != null)
                {
                  if (!tiles.ContainsKey("VendingMachine"))
                  {
                    tiles.Add("VendingMachine", new List<BCMTileEntity>());
                  }
                  tiles["VendingMachine"].Add(new BCMTileEntityVendingMachine(pos, (TileEntityVendingMachine)te));
                }
                count++;
                break;
              case TileEntityType.Forge:
                te = (kvp.Value as TileEntityForge);
                if (te != null)
                {
                  if (!tiles.ContainsKey("Forge"))
                  {
                    tiles.Add("Forge", new List<BCMTileEntity>());
                  }
                  tiles["Forge"].Add(new BCMTileEntityForge(pos, (TileEntityForge)te));
                }
                count++;
                break;
              case TileEntityType.Campfire:
                te = (kvp.Value as TileEntityCampfire);
                if (te != null)
                {
                  if (!tiles.ContainsKey("Campfire"))
                  {
                    tiles.Add("Campfire", new List<BCMTileEntity>());
                  }
                  tiles["Campfire"].Add(new BCMTileEntityCampfire(pos, (TileEntityCampfire)te));
                }
                count++;
                break;
              case TileEntityType.SecureLoot:
                te = (kvp.Value as TileEntitySecureLootContainer);
                if (te != null)
                {
                  if (!tiles.ContainsKey("SecureLoot"))
                  {
                    tiles.Add("SecureLoot", new List<BCMTileEntity>());
                  }
                  tiles["SecureLoot"].Add(new BCMTileEntitySecureLootContainer(pos, (TileEntitySecureLootContainer)te));
                }
                count++;
                break;
              case TileEntityType.SecureDoor:
                te = (kvp.Value as TileEntitySecureDoor);
                if (te != null)
                {
                  if (!tiles.ContainsKey("SecureDoor"))
                  {
                    tiles.Add("SecureDoor", new List<BCMTileEntity>());
                  }
                  tiles["SecureDoor"].Add(new BCMTileEntitySecureDoor(pos, (TileEntitySecureDoor)te));
                }
                count++;
                break;
              case TileEntityType.Workstation:
                te = (kvp.Value as TileEntityWorkstation);
                if (te != null)
                {
                  if (!tiles.ContainsKey("Workstation"))
                  {
                    tiles.Add("Workstation", new List<BCMTileEntity>());
                  }
                  tiles["Workstation"].Add(new BCMTileEntityWorkstation(pos, (TileEntityWorkstation)te));
                }
                count++;
                break;
              case TileEntityType.Sign:
                te = (kvp.Value as TileEntitySign);
                if (te != null)
                {
                  if (!tiles.ContainsKey("Sign"))
                  {
                    tiles.Add("Sign", new List<BCMTileEntity>());
                  }
                  tiles["Sign"].Add(new BCMTileEntitySign(pos, (TileEntitySign)te));
                }
                count++;
                break;
              case TileEntityType.GoreBlock:
                te = (kvp.Value as TileEntityGoreBlock);
                if (te != null)
                {
                  if (!tiles.ContainsKey("GoreBlock"))
                  {
                    tiles.Add("GoreBlock", new List<BCMTileEntity>());
                  }
                  tiles["GoreBlock"].Add(new BCMTileEntityGoreBlock(pos, (TileEntityGoreBlock)te));
                }
                count++;
                break;
              case TileEntityType.Powered:
                te = (kvp.Value as TileEntityPowered);
                if (te != null)
                {
                  if (!tiles.ContainsKey("Powered"))
                  {
                    tiles.Add("Powered", new List<BCMTileEntity>());
                  }
                  tiles["Powered"].Add(new BCMTileEntityPowered(pos, (TileEntityPowered)te));
                }
                count++;
                break;
              case TileEntityType.PowerSource:
                te = (kvp.Value as TileEntityPowerSource);
                if (te != null)
                {
                  if (!tiles.ContainsKey("PowerSource"))
                  {
                    tiles.Add("PowerSource", new List<BCMTileEntity>());
                  }
                  tiles["PowerSource"].Add(new BCMTileEntityPowerSource(pos, (TileEntityPowerSource)te));
                }
                count++;
                break;
              case TileEntityType.PowerRangeTrap:
                te = (kvp.Value as TileEntityPoweredRangedTrap);
                if (te != null)
                {
                  if (!tiles.ContainsKey("PowerRangeTrap"))
                  {
                    tiles.Add("PowerRangeTrap", new List<BCMTileEntity>());
                  }
                  tiles["PowerRangeTrap"].Add(new BCMTileEntityPoweredRangeTrap(pos, (TileEntityPoweredRangedTrap)te));
                }
                count++;
                break;
              case TileEntityType.Trigger:
                te = (kvp.Value as TileEntityPoweredTrigger);
                if (te != null)
                {
                  if (!tiles.ContainsKey("Trigger"))
                  {
                    tiles.Add("Trigger", new List<BCMTileEntity>());
                  }
                  tiles["Trigger"].Add(new BCMTileEntityPoweredTrigger(pos, (TileEntityPoweredTrigger)te));
                }
                count++;
                break;
              default:
                SendOutput($"Error finding TileEntity Type at {pos}");
                break;
            }
          }
        }
      }
      if (tiles.Count > 0)
      {
        SendJson(new { Count = count, Tiles = tiles });
      }
      else
      {
        SendOutput("No tile entities found in area");
      }
    }

    private static void RemoveTiles(BCTileEntityCmd command, World world)
    {
      var count = 0;
      for (var x = command.ChunkBounds.x; x <= command.ChunkBounds.z; x++)
      {
        for (var z = command.ChunkBounds.y; z <= command.ChunkBounds.w; z++)
        {
          var chunkSync = world.ChunkCache.GetChunkSync(x, z);
          var tileEntities = chunkSync?.GetTileEntities();
          if (tileEntities == null) continue;

          var locations = new List<Vector3i>();
          var worldPos = new Vector3i(x << 4, 0, z << 4);
          foreach (var kvp in tileEntities.dict)
          {
            var pos = kvp.Key + worldPos;
            if (command.HasPos && !command.IsWithinBounds(pos)) continue;
            if (command.Filter != null &&
                !(command.Filter.Equals("all", StringComparison.OrdinalIgnoreCase) ||
                  command.Filter.Equals(kvp.Value.GetTileEntityType().ToString(), StringComparison.OrdinalIgnoreCase
                  ))) continue;

            (kvp.Value as TileEntityLootContainer)?.SetEmpty();
            locations.Add(kvp.Key);
          }
          foreach (var p in locations)
          {
            chunkSync.SetBlock(world, p.x, p.y, p.z, BlockValue.Air);
            count++;
          }
        }
      }
      SendOutput($"Removed {count} blocks");
    }

    private static void ResetTouched(BCTileEntityCmd command, World world)
    {
      var count = 0;
      for (var x = command.ChunkBounds.x; x <= command.ChunkBounds.z; x++)
      {
        for (var z = command.ChunkBounds.y; z <= command.ChunkBounds.w; z++)
        {
          var chunkSync = world.ChunkCache.GetChunkSync(x, z);
          var tileEntities = chunkSync?.GetTileEntities();
          if (tileEntities == null) continue;

          var worldPos = new Vector3i(x << 4, 0, z << 4);
          foreach (var kvp in tileEntities.dict)
          {
            if (kvp.Value.IsUserAccessing()) continue;

            var pos = kvp.Key + worldPos;
            if (command.HasPos && !command.IsWithinBounds(pos)) continue;
            if (command.Filter != null &&
                !(command.Filter.Equals("all", StringComparison.OrdinalIgnoreCase) ||
                  command.Filter.Equals(kvp.Value.GetTileEntityType().ToString(), StringComparison.OrdinalIgnoreCase
                  ))) continue;

            switch (kvp.Value.GetTileEntityType())
            {
              case TileEntityType.None:
                break;
              case TileEntityType.Loot:
                var loot = (kvp.Value as TileEntityLootContainer);
                if (loot != null)
                {
                  loot.SetEmpty();
                  loot.bWasTouched = false;
                  loot.bTouched = false;
                  loot.SetModified();
                }
                count++;
                break;
              case TileEntityType.Trader:
                break;
              case TileEntityType.VendingMachine:
                break;
              case TileEntityType.Forge:
                break;
              case TileEntityType.Campfire:
                break;
              case TileEntityType.SecureLoot:
                var secureLoot = (kvp.Value as TileEntitySecureLootContainer);
                if (secureLoot != null)
                {
                  secureLoot.SetEmpty();
                  secureLoot.bWasTouched = false;
                  secureLoot.bTouched = false;
                  secureLoot.SetModified();
                }
                count++;
                break;
              case TileEntityType.SecureDoor:
                break;
              case TileEntityType.Workstation:
                break;
              case TileEntityType.Sign:
                break;
              case TileEntityType.GoreBlock:
                var goreBlock = (kvp.Value as TileEntityGoreBlock);
                if (goreBlock != null)
                {
                  goreBlock.SetEmpty();
                  goreBlock.bWasTouched = false;
                  goreBlock.bTouched = false;
                  goreBlock.SetModified();
                }
                count++;
                break;
              case TileEntityType.Powered:
                break;
              case TileEntityType.PowerSource:
                break;
              case TileEntityType.PowerRangeTrap:
                break;
              case TileEntityType.Trigger:
                break;
              default:
                SendOutput($"Error finding TileEntity Type at {pos}");
                break;
            }
          }
        }
      }
      SendOutput($"Reset {count} loot containers");
    }

    private static void EmptyContainers(BCTileEntityCmd command, World world)
    {
      var count = 0;
      for (var x = command.ChunkBounds.x; x <= command.ChunkBounds.z; x++)
      {
        for (var z = command.ChunkBounds.y; z <= command.ChunkBounds.w; z++)
        {
          var chunkSync = world.ChunkCache.GetChunkSync(x, z);
          var tileEntities = chunkSync?.GetTileEntities();
          if (tileEntities == null) continue;

          var worldPos = new Vector3i(x << 4, 0, z << 4);
          foreach (var kvp in tileEntities.dict)
          {
            if (kvp.Value.IsUserAccessing()) continue;

            var pos = kvp.Key + worldPos;
            if (command.HasPos && !command.IsWithinBounds(pos)) continue;
            if (command.Filter != null &&
                !(command.Filter.Equals("all", StringComparison.OrdinalIgnoreCase) ||
                  command.Filter.Equals(kvp.Value.GetTileEntityType().ToString(), StringComparison.OrdinalIgnoreCase
                  ))) continue;

            switch (kvp.Value.GetTileEntityType())
            {
              case TileEntityType.None:
                break;
              case TileEntityType.Loot:
                (kvp.Value as TileEntityLootContainer)?.SetEmpty();
                count++;
                break;
              case TileEntityType.Trader:
                break;
              case TileEntityType.VendingMachine:
                break;
              case TileEntityType.Forge:
                break;
              case TileEntityType.Campfire:
                break;
              case TileEntityType.SecureLoot:
                (kvp.Value as TileEntitySecureLootContainer)?.SetEmpty();
                count++;
                break;
              case TileEntityType.SecureDoor:
                break;
              case TileEntityType.Workstation:
                break;
              case TileEntityType.Sign:
                break;
              case TileEntityType.GoreBlock:
                (kvp.Value as TileEntityGoreBlock)?.SetEmpty();
                count++;
                break;
              case TileEntityType.Powered:
                break;
              case TileEntityType.PowerSource:
                break;
              case TileEntityType.PowerRangeTrap:
                break;
              case TileEntityType.Trigger:
                break;
              default:
                SendOutput($"Error finding TileEntity Type at {pos}");
                break;
            }
          }
        }
      }
      SendOutput($"Emptied {count} loot containers");
    }

    private static void SetLocked(BCTileEntityCmd command, bool locked, bool setPwd, string pwd, World world)
    {
      var count = 0;
      for (var x = command.ChunkBounds.x; x <= command.ChunkBounds.z; x++)
      {
        for (var z = command.ChunkBounds.y; z <= command.ChunkBounds.w; z++)
        {
          var chunkSync = world.ChunkCache.GetChunkSync(x, z);
          var tileEntities = chunkSync?.GetTileEntities();
          if (tileEntities == null) continue;

          var worldPos = new Vector3i(x << 4, 0, z << 4);
          foreach (var kvp in tileEntities.dict)
          {
            var pos = kvp.Key + worldPos;
            if (command.HasPos && !command.IsWithinBounds(pos)) continue;
            if (command.Filter != null &&
                !(command.Filter.Equals("all", StringComparison.OrdinalIgnoreCase) ||
                  command.Filter.Equals(kvp.Value.GetTileEntityType().ToString(), StringComparison.OrdinalIgnoreCase
                  ))) continue;

            switch (kvp.Value.GetTileEntityType())
            {
              case TileEntityType.None:
                break;
              case TileEntityType.Loot:
                break;
              case TileEntityType.Trader:
                break;
              case TileEntityType.VendingMachine:
                var vm = (kvp.Value as TileEntityVendingMachine);
                if (vm == null) continue;
                if (vm.IsLocked() == locked && !setPwd) continue;

                vm.SetLocked(locked);
                if (locked && setPwd)
                {
                  vm.CheckPassword(pwd, vm.GetOwner(), out bool _);
                }
                count++;
                break;
              case TileEntityType.Forge:
                break;
              case TileEntityType.Campfire:
                break;
              case TileEntityType.SecureLoot:
                var sl = (kvp.Value as TileEntitySecureLootContainer);
                if (sl == null) continue;
                if (sl.IsLocked() == locked && !setPwd) continue;

                sl.SetLocked(locked);
                if (locked && setPwd)
                {
                  sl.CheckPassword(pwd, sl.GetOwner(), out bool _);
                }
                count++;
                break;
              case TileEntityType.SecureDoor:
                var sd = (kvp.Value as TileEntitySecureDoor);
                if (sd == null) continue;
                if (sd.IsLocked() == locked && !setPwd) continue;

                sd.SetLocked(locked);
                if (locked && setPwd)
                {
                  sd.CheckPassword(pwd, sd.GetOwner(), out bool _);
                }
                count++;
                break;
              case TileEntityType.Workstation:
                break;
              case TileEntityType.Sign:
                var sign = (kvp.Value as TileEntitySign);
                if (sign == null) continue;
                if (sign.IsLocked() == locked && !setPwd) continue;

                sign.SetLocked(locked);
                if (locked && setPwd)
                {
                  sign.CheckPassword(pwd, sign.GetOwner(), out bool _);
                }
                count++;
                break;
              case TileEntityType.GoreBlock:
                break;
              case TileEntityType.Powered:
                break;
              case TileEntityType.PowerSource:
                break;
              case TileEntityType.PowerRangeTrap:
                break;
              case TileEntityType.Trigger:
                break;
              default:
                SendOutput($"Error finding TileEntity Type at {pos}");
                break;
            }
          }
        }
      }
      SendOutput($"Set {(locked ? "locked" : "unlocked")} on {count} secure blocks {(setPwd ? " with new password" : "")}");
    }

    private static void GrantAccess(BCTileEntityCmd command, World world)
    {
      var count = 0;
      for (var x = command.ChunkBounds.x; x <= command.ChunkBounds.z; x++)
      {
        for (var z = command.ChunkBounds.y; z <= command.ChunkBounds.w; z++)
        {
          var chunkSync = world.ChunkCache.GetChunkSync(x, z);
          var tileEntities = chunkSync?.GetTileEntities();
          if (tileEntities == null) continue;

          var worldPos = new Vector3i(x << 4, 0, z << 4);
          foreach (var kvp in tileEntities.dict)
          {
            var pos = kvp.Key + worldPos;
            if (command.HasPos && !command.IsWithinBounds(pos)) continue;
            if (command.Filter != null &&
                !(command.Filter.Equals("all", StringComparison.OrdinalIgnoreCase) ||
                  command.Filter.Equals(kvp.Value.GetTileEntityType().ToString(), StringComparison.OrdinalIgnoreCase
                  ))) continue;

            switch (kvp.Value.GetTileEntityType())
            {
              case TileEntityType.None:
                break;
              case TileEntityType.Loot:
                break;
              case TileEntityType.Trader:
                break;
              case TileEntityType.VendingMachine:
                var vendingMachine = (kvp.Value as TileEntityVendingMachine);
                if (vendingMachine != null)
                {
                  var users = vendingMachine.GetUsers();
                  if (users.Contains(command.SteamId)) continue;
                  users.Add(command.SteamId);
                }
                count++;
                break;
              case TileEntityType.Forge:
                break;
              case TileEntityType.Campfire:
                break;
              case TileEntityType.SecureLoot:
                var secureLoot = (kvp.Value as TileEntitySecureLootContainer);
                if (secureLoot != null)
                {
                  var users = secureLoot.GetUsers();
                  if (users.Contains(command.SteamId)) continue;
                  users.Add(command.SteamId);
                }
                count++;
                break;
              case TileEntityType.SecureDoor:
                var secureDoor = (kvp.Value as TileEntitySecureDoor);
                if (secureDoor != null)
                {
                  var users = secureDoor.GetUsers();
                  if (users.Contains(command.SteamId)) continue;
                  users.Add(command.SteamId);
                }
                count++;
                break;
              case TileEntityType.Workstation:
                break;
              case TileEntityType.Sign:
                var sign = (kvp.Value as TileEntitySign);
                if (sign != null)
                {
                  var users = sign.GetUsers();
                  if (users.Contains(command.SteamId)) continue;
                  users.Add(command.SteamId);
                }
                count++;
                break;
              case TileEntityType.GoreBlock:
                break;
              case TileEntityType.Powered:
                break;
              case TileEntityType.PowerSource:
                break;
              case TileEntityType.PowerRangeTrap:
                break;
              case TileEntityType.Trigger:
                break;
              default:
                SendOutput($"Error finding TileEntity Type at {pos}");
                break;
            }
          }
        }
      }
      SendOutput($"Granted access to {count} secure blocks");
    }

    private static void RevokeAccess(BCTileEntityCmd command, World world)
    {
      var count = 0;
      for (var x = command.ChunkBounds.x; x <= command.ChunkBounds.z; x++)
      {
        for (var z = command.ChunkBounds.y; z <= command.ChunkBounds.w; z++)
        {
          var chunkSync = world.ChunkCache.GetChunkSync(x, z);
          var tileEntities = chunkSync?.GetTileEntities();
          if (tileEntities == null) continue;

          var worldPos = new Vector3i(x << 4, 0, z << 4);
          foreach (var kvp in tileEntities.dict)
          {
            var pos = kvp.Key + worldPos;
            if (command.HasPos && !command.IsWithinBounds(pos)) continue;
            if (command.Filter != null &&
                !(command.Filter.Equals("all", StringComparison.OrdinalIgnoreCase) ||
                  command.Filter.Equals(kvp.Value.GetTileEntityType().ToString(), StringComparison.OrdinalIgnoreCase
                  ))) continue;

            switch (kvp.Value.GetTileEntityType())
            {
              case TileEntityType.None:
                break;
              case TileEntityType.Loot:
                break;
              case TileEntityType.Trader:
                break;
              case TileEntityType.VendingMachine:
                var vendingMachine = (kvp.Value as TileEntityVendingMachine);
                if (vendingMachine != null)
                {
                  var users = vendingMachine.GetUsers();
                  if (!users.Contains(command.SteamId)) continue;
                  users.Remove(command.SteamId);
                }
                count++;
                break;
              case TileEntityType.Forge:
                break;
              case TileEntityType.Campfire:
                break;
              case TileEntityType.SecureLoot:
                var secureLoot = (kvp.Value as TileEntitySecureLootContainer);
                if (secureLoot != null)
                {
                  var users = secureLoot.GetUsers();
                  if (!users.Contains(command.SteamId)) continue;
                  users.Remove(command.SteamId);
                }
                count++;
                break;
              case TileEntityType.SecureDoor:
                var secureDoor = (kvp.Value as TileEntitySecureDoor);
                if (secureDoor != null)
                {
                  var users = secureDoor.GetUsers();
                  if (!users.Contains(command.SteamId)) continue;
                  users.Remove(command.SteamId);
                }
                count++;
                break;
              case TileEntityType.Workstation:
                break;
              case TileEntityType.Sign:
                var sign = (kvp.Value as TileEntitySign);
                if (sign != null)
                {
                  var users = sign.GetUsers();
                  if (!users.Contains(command.SteamId)) continue;
                  users.Remove(command.SteamId);
                }
                count++;
                break;
              case TileEntityType.GoreBlock:
                break;
              case TileEntityType.Powered:
                break;
              case TileEntityType.PowerSource:
                break;
              case TileEntityType.PowerRangeTrap:
                break;
              case TileEntityType.Trigger:
                break;
              default:
                SendOutput($"Error finding TileEntity Type at {pos}");
                break;
            }
          }
        }
      }
      SendOutput($"Revoked access for {count} secure blocks");
    }

    private static void SetOwner(BCTileEntityCmd command, World world)
    {
      var count = 0;
      for (var x = command.ChunkBounds.x; x <= command.ChunkBounds.z; x++)
      {
        for (var z = command.ChunkBounds.y; z <= command.ChunkBounds.w; z++)
        {
          var chunkSync = world.ChunkCache.GetChunkSync(x, z);
          var tileEntities = chunkSync?.GetTileEntities();
          if (tileEntities == null) continue;

          var worldPos = new Vector3i(x << 4, 0, z << 4);
          foreach (var kvp in tileEntities.dict)
          {
            var pos = kvp.Key + worldPos;
            if (command.HasPos && !command.IsWithinBounds(pos)) continue;
            if (command.Filter != null &&
                !(command.Filter.Equals("all", StringComparison.OrdinalIgnoreCase) ||
                  command.Filter.Equals(kvp.Value.GetTileEntityType().ToString(), StringComparison.OrdinalIgnoreCase
                  ))) continue;

            switch (kvp.Value.GetTileEntityType())
            {
              case TileEntityType.None:
                break;
              case TileEntityType.Loot:
                break;
              case TileEntityType.Trader:
                break;
              case TileEntityType.VendingMachine:
                (kvp.Value as TileEntityVendingMachine)?.SetOwner(command.SteamId);
                count++;
                break;
              case TileEntityType.Forge:
                break;
              case TileEntityType.Campfire:
                break;
              case TileEntityType.SecureLoot:
                (kvp.Value as TileEntitySecureLootContainer)?.SetOwner(command.SteamId);
                count++;
                break;
              case TileEntityType.SecureDoor:
                (kvp.Value as TileEntitySecureDoor)?.SetOwner(command.SteamId);
                count++;
                break;
              case TileEntityType.Workstation:
                break;
              case TileEntityType.Sign:
                (kvp.Value as TileEntitySign)?.SetOwner(command.SteamId);
                count++;
                break;
              case TileEntityType.GoreBlock:
                break;
              case TileEntityType.Powered:
                break;
              case TileEntityType.PowerSource:
                (kvp.Value as TileEntityPowerSource)?.SetOwner(command.SteamId);
                count++;
                break;
              case TileEntityType.PowerRangeTrap:
                (kvp.Value as TileEntityPoweredRangedTrap)?.SetOwner(command.SteamId);
                count++;
                break;
              case TileEntityType.Trigger:
                (kvp.Value as TileEntityPoweredTrigger)?.SetOwner(command.SteamId);
                count++;
                break;
              default:
                SendOutput($"Error finding TileEntity Type at {pos}");
                break;
            }
          }
        }
      }
      SendOutput($"Set owner on {count} secure blocks");
    }
    #endregion
  }
}
