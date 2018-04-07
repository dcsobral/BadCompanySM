using System.Collections.Generic;
using System.Linq;

namespace BCM.Neurons
{
  public class FlyClipTracker : NeuronAbstract
  {
    private static readonly List<List<Vector3i>> jitter = new List<List<Vector3i>>();
    //private static List<Vector2i> FlyList = new List<Vector2i>();

    private static int jitterRadius = 2;
    private static int flyRadius = 2;
    //private static int yoffset = 2;
    //private static int history = 5;

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

        var ci = ConnectionManager.Instance.GetClientInfoForEntityId(player.entityId);
        var steamId = ci?.playerId;
        if (string.IsNullOrEmpty(steamId)) continue;


        //player.lastTickPos[0]; //todo: can look back at last 5 ticks
        var playerpos = new Vector3i(player.position);

        ReportClipping(world, player.entityId, playerpos, ci.playerName);
        FlyCheck(world, player.entityId, playerpos, ci.playerName);
      }
    }

    private static void FlyCheck(WorldBase world, int entityId, Vector3i playerpos, string playerName)
    {
      const int offset = 1;
      var nearestH = 0;
      var distY = 0;
      for (var i = 0; i < flyRadius; i++)
      {
        var jx = jitter[i].Where(jj => jj.y == 0).ToList();
        foreach (var t in jx)
        {
          var xz = playerpos + t;
          var h = world.GetHeight(xz.x, xz.z);
          if (playerpos.y - offset <= h) return;

          if (h <= nearestH) continue;

          nearestH = h;
          distY = playerpos.y - nearestH;
        }
      }

      var text = $"FLY:({entityId}) ({distY}) {playerName} {playerpos} - {GameTimer.Instance.ticks}";

      GameManager.Instance.GameMessageServer(null, EnumGameMessages.Chat, text, "Server", false, string.Empty, false);
    }

    private static void ReportClipping(IBlockAccess world, int entityId, Vector3i playerpos, string playerName)
    {
      var hasGap = false;
      var k = 0;
      var l = 0;
      for (var i = 0; i < jitterRadius; i++)
      {
        for (var j = 0; j < jitter[i].Count; j++)
        {
          if (IsTerrain(world, playerpos + jitter[i][j])) continue;

          hasGap = true;
          k = i;
          l = j;
          break;
        }
        if (hasGap) break;
      }
      if (hasGap && k == 0) return;

      var text = $"CLIP:({entityId}) ({k},{l}) {playerName} {playerpos} - {GameTimer.Instance.ticks}";
      ConnectionManager.Instance.GetClients();
      GameManager.Instance.GameMessageServer(null, EnumGameMessages.Chat, text, "Server", false, string.Empty, false);
    }

    private static bool IsTerrain(IBlockAccess world, Vector3i pos)
    {
      return world.GetBlock(pos.x, pos.y, pos.z).Block.shape.IsTerrain();
    }

    //private static bool IsInsideClearSpace(IBlockAccess world, Vector3i pos, int count) =>
    //  count != 0 && (Block.list[world.GetBlock(pos.x, pos.y, pos.z).type].shape.IsSolidSpace
    //    ? IsInsideClearSpace(world, pos + Vector3i.up, count - 1)
    //    : !Block.list[world.GetBlock(pos.x, pos.y + 1, pos.z).type].shape.IsSolidSpace ||
    //      IsInsideClearSpace(world, pos + Vector3i2up, count - 1));

    public FlyClipTracker(Synapse s) : base(s)
    {
      jitter.Add(new List<Vector3i>
      {
        new Vector3i(0, 0, 0)
      });
      jitter.Add(new List<Vector3i>
      {
        new Vector3i(0, 0, 1),
        new Vector3i(0, 0, -1),
        new Vector3i(1, 0, 0),
        new Vector3i(1, 0, 1),
        new Vector3i(1, 0, -1),
        new Vector3i(-1, 0, 0),
        new Vector3i(-1, 0, 1),
        new Vector3i(-1, 0, -1),
        new Vector3i(0, 1, 0),
        new Vector3i(0, 1, 1),
        new Vector3i(0, 1, -1),
        new Vector3i(1, 1, 0),
        new Vector3i(1, 1, 1),
        new Vector3i(1, 1, -1),
        new Vector3i(-1, 1, 0),
        new Vector3i(-1, 1, 1),
        new Vector3i(-1, 1, -1),
        new Vector3i(0, -1, 0),
        new Vector3i(0, -1, 1),
        new Vector3i(0, -1, -1),
        new Vector3i(1, -1, 0),
        new Vector3i(1, -1, 1),
        new Vector3i(1, -1, -1),
        new Vector3i(-1, -1, 0),
        new Vector3i(-1, -1, 1),
        new Vector3i(-1, -1, -1)
      });
      jitter.Add(new List<Vector3i>
      {
        new Vector3i(-2, -1, 0),
        new Vector3i(0, -1, 2),
        new Vector3i(0, -1, -2),
        new Vector3i(2, -1, 0),
        new Vector3i(-2, -1, -1),
        new Vector3i(-2, -1, 1),
        new Vector3i(-1, -1, -2),
        new Vector3i(-1, -1, 2),
        new Vector3i(1, -1, -2),
        new Vector3i(1, -1, 2),
        new Vector3i(2, -1, -1),
        new Vector3i(2, -1, 1),
        new Vector3i(-2, -1, -2),
        new Vector3i(-2, -1, 2),
        new Vector3i(2, -1, -2),
        new Vector3i(2, -1, 2)
      });
    }

    //todo: set yoffset and jitterRadius in config and bc-events
  }
}
