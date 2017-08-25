using UnityEngine;

namespace BCM.Commands
{
  public class BCVisitRegion : BCCommandAbstract
  {
    private MapVisitor _mapVisitor;
    private ClientInfo _lastSender;

    public override void Process()
    {
      //Visits a region of the map and runs the specified sub command if any.
      //default (no extra params) mode - takes vector2i of region co-ords. Runs a map visitor on that region to expose map and generate chunks

      if (Options.ContainsKey("stop"))
      {
        if (_mapVisitor == null)
        {
          SendOutput("VisitRegion not running.");

          return;
        }

        _mapVisitor.Stop();
        _mapVisitor = null;
        SendOutput("VisitRegion stopped.");

        return;
      }

      if (_mapVisitor != null && _mapVisitor.IsRunning())
      {
        SendOutput("VisitRegion already running. You can stop it with \"bc-visitregion /stop\".");
      }
      else if (!int.TryParse(Params[0], out int x))
      {
        SendOutput("The given x1 coordinate is not a valid integer");
      }
      else if (!int.TryParse(Params[1], out int z))
      {
        SendOutput("The given z1 coordinate is not a valid integer");
      }
      else
      {
        if (x > 19 || x < -20)
        {
          SendOutput("Note: The given x1 coordinate is beyond the recommended range (-20 to 19)");
        }
        if (z > 19 || z < -20)
        {
          SendOutput("Note: The given z1 coordinate is beyond the recommended range (-20 to 19)");
        }

        SendOutput("Sending a visitor to region " + x + " " + z);

        _lastSender = SenderInfo.RemoteClientInfo;

        _mapVisitor = new MapVisitor(new Vector3i(x * 512, 0, z * 512), new Vector3i(x * 512 + 511, 0, z * 512 + 511));
        _mapVisitor.OnVisitChunk += ReportStatus;
        _mapVisitor.OnVisitChunk += GetMapColors;
        _mapVisitor.OnVisitMapDone += ReportCompletion;
        _mapVisitor.Start();
      }
    }

    private static void ReportStatus(Chunk chunk, int count, int total, float elapsedTime)
    {
      if (count % 128 != 0) return;

      Log.Out($"VisitRegion ({Mathf.RoundToInt(100f * (count / (float) total)):00}%): {count} / {total} chunks done (estimated time left {(total - count) * (elapsedTime / count):0.00} seconds)");
    }

    private static void GetMapColors(Chunk chunk, int count, int total, float elapsedTime)
    {
      chunk.GetMapColors();
    }

    private void ReportCompletion(int total, float elapsedTime)
    {
      Log.Out($"VisitRegion done, visited {total} chunks in {elapsedTime:0.00} seconds (average {total / elapsedTime:0.00} chunks/sec).");

      if (_lastSender != null)
      {
        _lastSender.SendPackage(new NetPackageGameMessage(EnumGameMessages.Chat, "(PM) Visit Region Completed", "Server", false, "", false));
        _lastSender = null;
      }
      _mapVisitor = null;
    }
  }
}
