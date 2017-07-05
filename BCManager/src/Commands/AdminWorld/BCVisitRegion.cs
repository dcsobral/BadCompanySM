using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using UnityEngine;

namespace BCM.Commands
{
  public class BCVisitRegion : BCCommandAbstract
  {
    private MapVisitor mapVisitor;

    public override void Process()
    {
      //Visits a region of the map and runs the specified sub command if any.
      //default (no extra params) mode - takes vector2i of region co-ords. Runs a map visitor on that region to expose map and generate chunks

      if (_options.ContainsKey("stop"))
      {

        if (this.mapVisitor != null)
        {
          this.mapVisitor.Stop();
          this.mapVisitor = null;

          SendOutput("VisitRegion stopped.");
          return;
        }
        SendOutput("VisitRegion not running.");
        return;
      }

      int x;
      int z;
      if (this.mapVisitor != null && this.mapVisitor.IsRunning())
      {
        SendOutput("VisitMap already running. You can stop it with \"bc-visitregion stop\".");
      }
      else if (!int.TryParse(_params[0], out x)) //todo: check for in range (-20 to 20?)
      {
        SendOutput("The given x1 coordinate is not a valid integer");
      }
      else if (!int.TryParse(_params[1], out z)) //todo: check for in range (-20 to 20?)
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

        this.mapVisitor = new MapVisitor(new Vector3i(x * 512, 0, z * 512), new Vector3i(x * 512 + 511, 0, z * 512 + 511));
        this.mapVisitor.OnVisitChunk += new MapVisitor.VisitChunkDelegate(this.ReportStatus);
        this.mapVisitor.OnVisitChunk += new MapVisitor.VisitChunkDelegate(this.GetMapColors);
        this.mapVisitor.OnVisitMapDone += new MapVisitor.VisitMapDoneDelegate(this.md0008);
        this.mapVisitor.Start();
      }
    }

    private void ReportStatus(Chunk chunk, int count, int total, float elapsedTime)
    {
      if (count % 200 == 0)
      {
        float value = (float)(total - count) * (elapsedTime / (float)count);
        Log.Out("VisitRegion ({3:00}%): {0} / {1} chunks done (estimated time left {2} seconds)", new object[]
        {
        count,
        total,
        value.ToString("0.00"),
        Mathf.RoundToInt(100f * ((float)count / (float)total))
        });
      }
    }

    private void GetMapColors(Chunk chunk, int count, int total, float elapsedTime)
    {
      chunk.GetMapColors();
    }

    private void md0008(int total, float elapsedTime)
    {
      Log.Out("VisitRegion done, visited {0} chunks in {1} seconds (average {2} chunks/sec).", new object[]
      {
        total,
        elapsedTime.ToString("0.00"),
        ((float)total / elapsedTime).ToString("0.00")
      });
      this.mapVisitor = null;
    }
  }
}
