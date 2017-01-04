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

    public SpawnpointList(PlayerInfo _pInfo)
    {
      Load(_pInfo);
    }

    public void Load(PlayerInfo _pInfo)
    {
      foreach (Vector3i sp in _pInfo.PDF.spawnPoints)
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
