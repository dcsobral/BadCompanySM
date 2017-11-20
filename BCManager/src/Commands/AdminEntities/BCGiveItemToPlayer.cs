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

        var itemValue = int.TryParse(Params[1], out var itemId)
          ? ItemClass.list[itemId] == null
            ? null
            : new ItemValue(itemId, min, max, true)
          : ItemClass.GetItem(Params[1]);
        if (itemValue == null)
        {
          SendOutput($"Unable to find item '{Params[1]}'");

          return;
        }

        var count = 1;
        if (Options.ContainsKey("c"))
        {
          int.TryParse(Options["c"], out count);
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
          SendOutput($"Gave {count}x {itemValue.ItemClass.localizedName} to player: {clientInfo.playerName}");
        }
        else
        {
          SendOutput("Player not spawned");
        }
      }
      else if (matches > 1)
      {
        SendOutput("Multiple matches found: " + matches);
      }
      else
      {
        SendOutput("Player not found.");
      }
    }
  }
}
