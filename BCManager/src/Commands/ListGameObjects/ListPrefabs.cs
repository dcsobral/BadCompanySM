using RWG2.Rules;
using System.Collections.Generic;
using System.IO;

namespace BCM.Commands
{
  public class ListPrefabs : BCCommandAbstract
  {
    public override void Process()
    {
      string output = "";
      string prefabsGameDir = Utils.GetGameDir("Data/Prefabs");
      List<string> prefabs = GetStoredPrefabs(prefabsGameDir);

      if (_params.Count == 1)
      {
        Prefab prefab = new Prefab();
        if (File.Exists(prefabsGameDir + "/" + _params[0] + ".tts"))
        {
          if (prefab.Load(_params[0]))
          {
            output += _params[0] + "[size=" + prefab.size + ",yoffset=" + prefab.yOffset + "]" + _sep;
            SortedDictionary<int, int> blockstat = new SortedDictionary<int, int>();
            for (int i = 0; i < prefab.size.x; i++)
            {
              for (int j = 0; j < prefab.size.z; j++)
              {
                for (int k = 0; k < prefab.size.y; k++)
                {
                  BlockValue block = prefab.GetBlock(i, k, j);
                  if (blockstat.ContainsKey(block.type))
                  {
                    blockstat[block.type]++;
                  } else
                  {
                    blockstat[block.type] = 1;
                  }
                }
              }
            }

            foreach (int blockid in blockstat.Keys)
            {
              if (blockid == 0)
              {
                output += "air(" + blockid + "):" + blockstat[blockid] + _sep;
                continue;
              }
              try
              {
                ItemClass ic = ItemClass.list[blockid];
                output += ic.Name + "(" + blockid + "):" + blockstat[blockid] + _sep;
              }
              catch
              {
                output += "NULL(" + blockid + "):" + blockstat[blockid] + _sep;
              }
            }
          }
        } else
        {
          // todo: return results for partial matches to the prefabname string entered for param0
          output += "Prefab not found";
        }

      }
      else
      {
        foreach (string prefabname in prefabs)
        {
          output += prefabname + _sep;
        }
      }
      SendOutput(output);
    }
    public static List<string> GetStoredPrefabs(string prefabsGameDir)
    {
      string[] files = Directory.GetFiles(prefabsGameDir);
      List<string> prefabs = new List<string>();

      for (int i = files.Length - 1; i >= 0; i--)
      {
        string file = files[i];
        string ext = Path.GetExtension(file);
        if (ext == ".tts")
        {
          int start = prefabsGameDir.Length + 1;
          int len = file.Length - start - 4;
          if (start + len <= file.Length)
          {
            prefabs.Add(file.Substring(start, len));
          }
        }
      }
      return prefabs;
    }

  }
}
