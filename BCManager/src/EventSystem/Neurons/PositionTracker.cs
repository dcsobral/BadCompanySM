using System;
using BCM.PersistentData;

namespace BCM.Neurons
{
  public class PositionTracker : NeuronAbstract
  {
    public override void Fire(int b)
    {
      var world = GameManager.Instance.World;
      if (world == null)
      {
        return;
      }

      if (world.Players.Count == 0) return;

      foreach (var player in world.Players.dict.Values)
      {
        if (!player.IsSpawned()) continue;

        var steamId = ConnectionManager.Instance.GetClientInfoForEntityId(player.entityId)?.playerId;
        if (string.IsNullOrEmpty(steamId)) continue;

        //track the rotation in the range 0-359 only
        PersistentContainer.Instance.PlayerLogs[steamId, true]
          ?.LogPosition(player.position, (int)Math.Floor(player.rotation.y) % 360);
      }
    }

    public PositionTracker(Synapse s) : base(s)
    {
    }
  }
}
