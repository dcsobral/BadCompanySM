using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BCM.Commands
{
  public class BCTileEntity : BCCommandAbstract
  {
    #region Properties
    private enum ReloadMode
    {
      None,
      Target,
      All
    }

    private class BCMTileEntity
    {
      public string Type;
      public BCMVector3 Pos;

      public BCMTileEntity(Vector3i pos, TileEntity te)
      {
        Type = te.GetTileEntityType().ToString();
        Pos = new BCMVector3(pos);
      }
    }

    private class BCMVector2
    {
      public int x;
      public int y;
      public BCMVector2()
      {
        x = 0;
        y = 0;
      }
      public BCMVector2(int x, int y)
      {
        this.x = x;
        this.y = y;
      }
      public BCMVector2(Vector2 v)
      {
        x = Mathf.RoundToInt(v.x);
        y = Mathf.RoundToInt(v.y);
      }
      public BCMVector2(Vector2i v)
      {
        x = v.x;
        y = v.y;
      }
    }

    private class BCMVector3
    {
      public int x;
      public int y;
      public int z;
      public BCMVector3()
      {
        x = 0;
        y = 0;
        z = 0;
      }
      public BCMVector3(Vector3 v)
      {
        x = Mathf.RoundToInt(v.x);
        y = Mathf.RoundToInt(v.y);
        z = Mathf.RoundToInt(v.z);
      }
      public BCMVector3(Vector3i v)
      {
        x = v.x;
        y = v.y;
        z = v.z;
      }

      public BCMVector3(int x, int y, int z)
      {
        this.x = x;
        this.y = y;
        this.z = z;
      }
    }

    private class BCMVector4
    {
      public int x;
      public int y;
      public int z;
      public int w;
      public BCMVector4()
      {
        x = 0;
        y = 0;
        z = 0;
        w = 0;
      }
      public BCMVector4(int x, int y, int z, int w)
      {
        this.x = x;
        this.y = y;
        this.z = z;
        this.w = w;
      }
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

      var command = new CmdParams();
      if (!ProcessParams(command)) return;

      if (!GetIds(world, out var steamId, out var entity)) return;

      if (!command.HasChunkPos && !command.HasPos && !GetEntPos(command, entity))
      {
        SendOutput("Unable to get position.");

        return;
      }

      SendOutput("Processing Command...");

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
          SetOwner(command, steamId, world);
          reload = ReloadMode.Target;
          break;
        case "access":
          GrantAccess(command, steamId, world);
          reload = ReloadMode.Target;
          break;
        case "revoke":
          RevokeAccess(command, steamId, world);
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
          //todo: adds items to a lootcontainer
          //either add to first empty, or add at specific slot. Specific slot can be use to adjust stack size or type of itemvalue
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
      if (reload == ReloadMode.None) return;
      BCChunks.ReloadForClients(affectedChunks, reload == ReloadMode.Target ? steamId : string.Empty);
    }

    #region Params
    private class CmdParams
    {
      public string Command = string.Empty;
      public string Filter;
      public ushort Radius;

      public bool HasPos;
      public BCMVector3 Position;

      public bool HasSize;
      public BCMVector3 Size;

      public bool HasChunkPos;
      public BCMVector4 ChunkBounds;

      public bool IsWithinPosSize(Vector3i pos)
      {
        if (!HasPos) return false;

        var size = HasSize ? Size : new BCMVector3(0, 0, 0);
        return (Position.x <= pos.x && pos.x <= Position.x + size.x) &&
               (Position.y <= pos.y && pos.y <= Position.y + size.y) &&
               (Position.z <= pos.z && pos.z <= Position.z + size.z);
      }
    }

    private static bool ProcessCmdAndBounds(CmdParams command)
    {
      if (Options.ContainsKey("type"))
      {
        command.Filter = Options["type"];
      }
      switch (Params.Count)
      {
        case 1:
          //command with no extras
          command.Command = Params[0];
          return true;
        case 3:
          //XZ single chunk with /r
          command.Command = Params[0];
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
          return false;
      }
    }

    private static bool GetChunkSizeXyzw(CmdParams command)
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

    private static bool GetChunkPosXz(CmdParams command)
    {
      if (!int.TryParse(Params[1], out var x) || !int.TryParse(Params[2], out var z))
      {
        SendOutput("Unable to parse x,z into ints");

        return false;
      }

      if (Options.ContainsKey("r"))
      {
        ushort.TryParse(Options["r"], out command.Radius);
        if (command.Radius > 5)
        {
          command.Radius = 5;
          SendOutput("Setting radius to maximum of +5 chunks");
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

      command.ChunkBounds = new BCMVector4(x - command.Radius, z - command.Radius, x + command.Radius, z + command.Radius);
      command.HasChunkPos = true;

      return true;
    }

    private static bool GetPositionXyz(CmdParams command)
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
      return true;
    }

    private static bool GetPosSizeXyz(CmdParams command)
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

      command.Size = new BCMVector3(Math.Abs(x - x2), Math.Abs(y - y2), Math.Abs(z - z2));
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

    private bool ProcessParams(CmdParams command)
    {
      switch (Params.Count)
      {
        case 1:
        case 3:
        case 4:
        case 5:
        case 7:
          return ProcessCmdAndBounds(command);

        default:
          SendOutput(GetHelp());

          return false;
      }
    }

    private static bool GetIds(World world, out string steamId, out EntityPlayer entity)
    {
      steamId = null;
      entity = null;
      int? entityId = null;

      if (Options.ContainsKey("id"))
      {
        if (!PlayerStore.GetId(Options["id"], out steamId, "CON")) return false;

        entityId = ConsoleHelper.ParseParamSteamIdOnline(steamId)?.entityId;
      }

      if (steamId == null)
      {
        entityId = SenderInfo.RemoteClientInfo?.entityId;
        steamId = SenderInfo.RemoteClientInfo?.playerId;
      }

      if (entityId != null)
      {
        entity = world.Players.dict[(int)entityId];
      }

      return entity != null || Params.Count >= 3;
    }

    private static bool GetEntPos(CmdParams command, EntityPlayer entity)
    {
      //todo: if /loc and a loc is set then use loc and player pos as the bounds instad of radius, /loc and /r connot be used together
      //todo: if /h=#,# then set y to pos + [0], y2 to pos + [1], if only 1 number then y=pos y2=pos+#
      if (entity != null)
      {
        command.ChunkBounds = new BCMVector4
        {
          x = World.toChunkXZ((int)entity.position.x - command.Radius),
          y = World.toChunkXZ((int)entity.position.z - command.Radius),
          z = World.toChunkXZ((int)entity.position.x + command.Radius),
          w = World.toChunkXZ((int)entity.position.z + command.Radius)
        };
      }
      else
      {
        SendOutput("Unable to get a position");

        return false;
      }

      return true;
    }

    private static Dictionary<long, Chunk> GetAffectedChunks(CmdParams command, World world)
    {
      //request any unloaded chunks in area
      for (var x = command.ChunkBounds.x; x <= command.ChunkBounds.z; x++)
      {
        for (var z = command.ChunkBounds.y; z <= command.ChunkBounds.w; z++)
        {
          var chunkKey = WorldChunkCache.MakeChunkKey(x, z);
          if (!world.ChunkCache.ContainsChunkSync(chunkKey))
          {
            world.ChunkCache.ChunkProvider.RequestChunk(x, z);
          }
        }
      }

      var modifiedChunks = new Dictionary<long, Chunk>();
      var chunkCount = (command.ChunkBounds.z - command.ChunkBounds.x) * (command.ChunkBounds.w - command.ChunkBounds.y);

      var count = 0;
      var sw = System.Diagnostics.Stopwatch.StartNew();
      var timeout = 2000;
      if (Options.ContainsKey("timeout"))
      {
        if (Options["timeout"] != null)
        {
          int.TryParse(Options["timeout"], out timeout);
        }
      }

      while (count < chunkCount && sw.ElapsedMilliseconds < timeout)
      {
        for (var x = command.ChunkBounds.x; x <= command.ChunkBounds.z; x++)
        {
          for (var z = command.ChunkBounds.y; z <= command.ChunkBounds.w; z++)
          {
            //check if already loaded
            var chunkKey = WorldChunkCache.MakeChunkKey(x, z);
            if (modifiedChunks.ContainsKey(chunkKey) && modifiedChunks[chunkKey] != null) continue;

            //check if chunk has loaded, add to dict if it has
            modifiedChunks[chunkKey] = world.GetChunkSync(chunkKey) as Chunk;
            if (modifiedChunks[chunkKey] != null)
            {
              count++;
            }
          }
        }

        //short delay to give chunks a chance to load
        System.Threading.Thread.Sleep(10);
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
    #endregion

    #region Actions
    private static void ScanTiles(CmdParams command, World world)
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
            if (command.HasPos && !command.IsWithinPosSize(pos)) continue;
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

    private static void RemoveTiles(CmdParams command, World world)
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
            if (command.HasPos && !command.IsWithinPosSize(pos)) continue;
            if (command.Filter != null &&
                !(command.Filter.Equals("all", StringComparison.OrdinalIgnoreCase) ||
                  command.Filter.Equals(kvp.Value.GetTileEntityType().ToString(), StringComparison.OrdinalIgnoreCase
                  ))) continue;

            (kvp.Value as TileEntityLootContainer)?.SetEmpty();
            locations.Add(pos);
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

    private static void ResetTouched(CmdParams command, World world)
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
            if (command.HasPos && !command.IsWithinPosSize(pos)) continue;
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

    private static void EmptyContainers(CmdParams command, World world)
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
            if (command.HasPos && !command.IsWithinPosSize(pos)) continue;
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

    private static void SetLocked(CmdParams command, bool locked, bool setPwd, string pwd, World world)
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
            if (command.HasPos && !command.IsWithinPosSize(pos)) continue;
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

    private static void GrantAccess(CmdParams command, string steamId, World world)
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
            if (command.HasPos && !command.IsWithinPosSize(pos)) continue;
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
                  if (users.Contains(steamId)) continue;
                  users.Add(steamId);
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
                  if (users.Contains(steamId)) continue;
                  users.Add(steamId);
                }
                count++;
                break;
              case TileEntityType.SecureDoor:
                var secureDoor = (kvp.Value as TileEntitySecureDoor);
                if (secureDoor != null)
                {
                  var users = secureDoor.GetUsers();
                  if (users.Contains(steamId)) continue;
                  users.Add(steamId);
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
                  if (users.Contains(steamId)) continue;
                  users.Add(steamId);
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

    private static void RevokeAccess(CmdParams command, string steamId, World world)
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
            if (command.HasPos && !command.IsWithinPosSize(pos)) continue;
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
                  if (!users.Contains(steamId)) continue;
                  users.Remove(steamId);
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
                  if (!users.Contains(steamId)) continue;
                  users.Remove(steamId);
                }
                count++;
                break;
              case TileEntityType.SecureDoor:
                var secureDoor = (kvp.Value as TileEntitySecureDoor);
                if (secureDoor != null)
                {
                  var users = secureDoor.GetUsers();
                  if (!users.Contains(steamId)) continue;
                  users.Remove(steamId);
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
                  if (!users.Contains(steamId)) continue;
                  users.Remove(steamId);
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

    private static void SetOwner(CmdParams command, string steamId, World world)
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
            if (command.HasPos && !command.IsWithinPosSize(pos)) continue;
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
                (kvp.Value as TileEntityVendingMachine)?.SetOwner(steamId);
                count++;
                break;
              case TileEntityType.Forge:
                break;
              case TileEntityType.Campfire:
                break;
              case TileEntityType.SecureLoot:
                (kvp.Value as TileEntitySecureLootContainer)?.SetOwner(steamId);
                count++;
                break;
              case TileEntityType.SecureDoor:
                (kvp.Value as TileEntitySecureDoor)?.SetOwner(steamId);
                count++;
                break;
              case TileEntityType.Workstation:
                break;
              case TileEntityType.Sign:
                (kvp.Value as TileEntitySign)?.SetOwner(steamId);
                count++;
                break;
              case TileEntityType.GoreBlock:
                break;
              case TileEntityType.Powered:
                break;
              case TileEntityType.PowerSource:
                (kvp.Value as TileEntityPowerSource)?.SetOwner(steamId);
                count++;
                break;
              case TileEntityType.PowerRangeTrap:
                (kvp.Value as TileEntityPoweredRangedTrap)?.SetOwner(steamId);
                count++;
                break;
              case TileEntityType.Trigger:
                (kvp.Value as TileEntityPoweredTrigger)?.SetOwner(steamId);
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
