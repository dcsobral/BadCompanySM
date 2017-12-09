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

        var r = (int)Math.Floor(player.rotation.y);
        PersistentContainer.Instance.PlayerLogs[steamId, true]?.LogPosition(player.position, r < 0 ? 360 + r : r);
      }
    }

    public PositionTracker(Synapse s) : base(s)
    {
    }
  }
}
