using System;
using System.Collections.Generic;
using System.Linq;
using BCM.Models;

namespace BCM.Commands
{
  public class BCTileEntity : BCCommandAbstract
  {
    #region Properties
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

      var command = new BCMCmdArea(Params, Options, "TileEntity");
      if (!BCUtils.ProcessParams(command, 14))
      {
        SendOutput(GetHelp());

        return;
      }

      if (!BCUtils.GetIds(world, command, out var entity))
      {
        SendOutput("Command requires a position when not run by a player.");

        return;
      }

      if (!command.HasChunkPos && !command.HasPos && !BCUtils.GetEntPos(command, entity))
      {
        SendOutput("Unable to get position.");

        return;
      }

      BCUtils.DoProcess(world, command, this);
    }

    public override void ProcessSwitch(World world, BCMCmdArea command, out ReloadMode reload)
    {
      reload = ReloadMode.None;

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
          command.Opts.TryGetValue("pwd", out var pwd);
          SetLocked(command, true, command.Opts.ContainsKey("pwd"), pwd ?? string.Empty, world);
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
          if (!command.Opts.ContainsKey("confirm"))
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
        case "settext":
          SetText(command, world);
          break;
        default:
          SendOutput($"Unknown param {command.Command}");
          SendOutput(GetHelp());
          break;
      }
    }

    #region Actions
    private static void SetText(BCMCmdArea command, World world)
    {
      command.Opts.TryGetValue("text", out var t);
      var text = "";
      if (t != null)
      {
        var placeHolder = "_";
        if (Options.ContainsKey("p"))
        {
          placeHolder = Options["p"].Substring(0, 1);
        }
        text = t.Replace(placeHolder, " ");
      }

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
                break;
              case TileEntityType.Forge:
                break;
              case TileEntityType.Campfire:
                break;
              case TileEntityType.SecureLoot:
                break;
              case TileEntityType.SecureDoor:
                break;
              case TileEntityType.Workstation:
                break;
              case TileEntityType.Sign:
                var sign = (kvp.Value as TileEntitySign);
                if (sign == null) continue;

                sign.SetText(text);
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
      SendOutput($"Set {text} on {count} secure blocks");

    }

    private static void AddLoot(BCMCmdArea command, World world)
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

    private static void ScanTiles(BCMCmdArea command, World world)
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

    private static void RemoveTiles(BCMCmdArea command, World world)
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

    private static void ResetTouched(BCMCmdArea command, World world)
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

    private static void EmptyContainers(BCMCmdArea command, World world)
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

    private static void SetLocked(BCMCmdArea command, bool locked, bool setPwd, string pwd, World world)
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

    private static void GrantAccess(BCMCmdArea command, World world)
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

    private static void RevokeAccess(BCMCmdArea command, World world)
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

    private static void SetOwner(BCMCmdArea command, World world)
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
