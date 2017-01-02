using System;
using System.Collections.Generic;

namespace BCM.Models
{
  [Serializable]
  public class SpawnpointList
  {
    private List<Vector3i> spawnpoints = new List<Vector3i>();

    public SpawnpointList()
    {
    }

    public SpawnpointList(PlayerDataFile _pdf)
    {
      Load(_pdf);
    }

    public void Load(PlayerDataFile _pdf)
    {
      foreach (Vector3i sp in _pdf.spawnPoints)
      {
        spawnpoints.Add(sp);
      }
    }

    public string Display()
    {
      bool first = true;
      string output = "Spawnpoints(saved)={\n";
      foreach (Vector3i sp in spawnpoints)
      {
        if (!first) { output += ",\n"; } else { first = false; }
        output += " Bed:" + GameUtils.WorldPosToStr(sp.ToVector3(), " ");
      }
      output += "\n}\n";

      return output;
    }
  }
}
