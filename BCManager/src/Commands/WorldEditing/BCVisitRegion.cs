using System;
using BCM.Models;
using JetBrains.Annotations;
using UnityEngine;

namespace BCM.Commands
{
  [UsedImplicitly]
  public class BCVisitRegion : BCCommandAbstract
  {
    private MapVisitor _mapVisitor;
    private ClientInfo _lastSender;
    private int _hash;
    private static int completePercent;

    protected override void Process()
    {
      if (!BCUtils.CheckWorld()) return;

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
        SendOutput($"VisitRegion already running ({completePercent}%). You can stop it with \"bc-visitregion /stop\".");

        return;
      }

      if (Params.Count < 2)
      {
        SendOutput("VisitRegion isn't running. Provide some co-ords to explore some regions");

        return;
      }

      if (!int.TryParse(Params[0], out var x))
      {
        SendOutput("The given x1 coordinate is not a valid integer");

        return;
      }
      if (!int.TryParse(Params[1], out var z))
      {
        SendOutput("The given z1 coordinate is not a valid integer");

        return;
      }

      if (x > 19 || x < -20)
      {
        SendOutput("Note: The given x1 coordinate is beyond the recommended range (-20 to 19)");
      }

      if (z > 19 || z < -20)
      {
        SendOutput("Note: The given z1 coordinate is beyond the recommended range (-20 to 19)");
      }

      completePercent = 0;
      var x2 = x;
      var z2 = z;
      switch (Params.Count)
      {
        case 2:
          SendOutput($"Sending a visitor to region {x},{z}");
          break;
        case 3:
          if (!int.TryParse(Params[2], out var r))
          {
            SendOutput("The given radius is not a valid integer");
          }

          if (r < 0)
          {
            SendOutput("The given radius can't be less than 0, the recommended range is 0 to 20");

            return;
          }

          if (r > 20)
          {
            SendOutput("Note: The given radius is beyond the recommended range (0 to 20)");
          }

          x -= r;
          z -= r;
          x2 += r;
          z2 += r;
          SendOutput($"Sending a visitor to regions between {x},{z} and {x2},{z2}");
          break;
        case 4:
          if (!int.TryParse(Params[2], out x2))
          {
            SendOutput("The given x2 coordinate is not a valid integer");
          }
          else if (!int.TryParse(Params[3], out z2))
          {
            SendOutput("The given z2 coordinate is not a valid integer");
          }

          if (x2 > 19 || x2 < -20)
          {
            SendOutput("Note: The given x2 coordinate is beyond the recommended range (-20 to 19)");
          }

          if (z2 > 19 || z2 < -20)
          {
            SendOutput("Note: The given z2 coordinate is beyond the recommended range (-20 to 19)");
          }

          SendOutput($"Sending a visitor to regions between {x},{z} and {x2},{z2}");
          break;
        default:
          SendOutput("Invalid param count");
          SendOutput(GetHelp());
          break;
      }

      _lastSender = SenderInfo.RemoteClientInfo;

      _mapVisitor = new MapVisitor(new Vector3i(x * 512, 0, z * 512), new Vector3i(x2 * 512 + 511, 0, z2 * 512 + 511));
      _mapVisitor.OnVisitChunk += ReportStatus;
      _mapVisitor.OnVisitChunk += GetMapColors;
      _mapVisitor.OnVisitMapDone += ReportCompletion;
      _mapVisitor.Start();

      _hash = _mapVisitor.GetHashCode();
      BCTask.AddTask("MapVisitor", _mapVisitor.GetHashCode(), null);
    }

    private void ReportStatus(Chunk chunk, int count, int total, float elapsedTime)
    {
      var bcmTask = BCTask.GetTask("MapVisitor", _hash);
      if (bcmTask != null) bcmTask.Output = new { Count = count, Total = total, Time = Math.Round(elapsedTime, 2) };

      if (count % 128 != 0) return;

      completePercent = Mathf.RoundToInt(100f * (count / (float) total));
      Log.Out($"VisitRegion ({completePercent:00}%): {count} / {total} chunks done (estimated time left {(total - count) * (elapsedTime / count):0.00} seconds)");
    }

    private static void GetMapColors(Chunk chunk, int count, int total, float elapsedTime)
    {
      chunk.GetMapColors();
    }

    private void ReportCompletion(int total, float elapsedTime)
    {
      var bcmTask = BCTask.GetTask("MapVisitor", _hash);
      if (bcmTask != null)
      {
        bcmTask.Status = BCMTaskStatus.Complete;
        bcmTask.Output = $"VisitRegion done, visited {total} chunks in {elapsedTime:0.00} seconds (average {total / elapsedTime:0.00} chunks/sec).";
        BCTask.DelTask("MapVisitor", _hash);
      }

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
