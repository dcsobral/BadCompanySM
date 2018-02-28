using UnityEngine;

namespace BCM.Commands
{
  public class BCGiveItemToPlayer : BCCommandAbstract
  {
    public override void Process()
    {
      var world = GameManager.Instance.World;
      if (world == null)
      {
        SendOutput("World not initialized");

        return;
      }

      if (Params.Count != 2)
      {
        SendOutput(GetHelp());

        return;
      }

      var matches = ConsoleHelper.ParseParamPartialNameOrId(Params[0], out string _, out var clientInfo);
      if (matches == 1)
      {
        if (clientInfo == null)
        {
          SendOutput("Unable to locate player.");

          return;
        }

        var min = 1;
        var max = 600;
        if (Options.ContainsKey("q"))
        {
          if (int.TryParse(Options["q"], out var quality))
          {
            min = quality;
            max = quality;
          }
        }
        else
        {
          if (Options.ContainsKey("min"))
          {
            int.TryParse(Options["min"], out min);
          }
          if (Options.ContainsKey("max"))
          {
            int.TryParse(Options["max"], out max);
          }
        }

        ItemValue itemValue;
        if (int.TryParse(Params[1], out var itemId))
        {
          itemValue = ItemClass.list[itemId] == null ? ItemValue.None : new ItemValue(itemId, min, max, true);
        }
        else
        {
          if (!ItemClass.ItemNames.Contains(Params[1]))
          {
            SendOutput($"Unable to find item '{Params[1]}'");

            return;
          }

          itemValue = new ItemValue(ItemClass.GetItem(Params[1]).type, min, max, true);
        }

        if (Equals(itemValue, ItemValue.None))
        {
          SendOutput($"Unable to find item '{Params[1]}'");

          return;
        }

        var count = 1;
        if (Options.ContainsKey("c"))
        {
          int.TryParse(Options["c"], out count);
        }
        if (Options.ContainsKey("count"))
        {
          int.TryParse(Options["count"], out count);
        }

        var playerId = clientInfo.entityId;
        if (world.Players.dict.ContainsKey(playerId) && world.Players.dict[playerId].IsSpawned())
        {
          var entityItem = (EntityItem)EntityFactory.CreateEntity(new EntityCreationData
          {
            entityClass = EntityClass.FromString("item"),
            id = EntityFactory.nextEntityID++,
            itemStack = new ItemStack(itemValue, count),
            pos = world.Players.dict[playerId].position,
            rot = new Vector3(20f, 0f, 20f),
            lifetime = 60f,
            belongsPlayerId = playerId
          });

          world.SpawnEntityInWorld(entityItem);
          clientInfo.SendPackage(new NetPackageEntityCollect(entityItem.entityId, playerId));
          world.RemoveEntity(entityItem.entityId, EnumRemoveEntityReason.Killed);
          SendOutput($"Gave {count}x {itemValue.ItemClass.localizedName ?? itemValue.ItemClass.Name} to player: {clientInfo.playerName}");
          if (!Options.ContainsKey("silent"))
          {
            clientInfo.SendPackage(new NetPackageGameMessage(EnumGameMessages.Chat, $"{count}x {itemValue.ItemClass.localizedName ?? itemValue.ItemClass.Name} recived. If your bag is full check the ground.", "(Server)", false, "", false));
          }
        }
        else
        {
          SendOutput("Player not spawned");
        }
      }
      else if (matches > 1)
      {
        SendOutput($"{matches} matches found, please refine your search text.");
      }
      else
      {
        SendOutput("Player not found.");
      }
    }
  }
}
