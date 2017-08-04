using BCM.PersistentData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using UMA;
using UnityEngine;

namespace BCM.Commands
{
  public class ListPlayerFilesInfo : BCCommandAbstract
  {
    public static FileSystemInfo[] GetFiles(string path)
    {
      DirectoryInfo root = new DirectoryInfo(path);
      if (!root.Exists) { return null; }
      var files = root.GetFileSystemInfos();
      return files;
    }

    public override void Process()
    {
      string path = GameUtils.GetPlayerDataDir();
      var files = GetFiles(path);

      string output = "{";

      if (files != null)
      {

        foreach (var file in files)
        {
          if (file.Extension == ".ttp")
          {
            var _steamId = file.Name.Substring(0, file.Name.Length - file.Extension.Length);
            var p = "";
            var player = PersistentContainer.Instance.Players[_steamId, false];
            if (player != null)
            {
              p += "\"playerName\":\"" + player.Name + "\"," +
              "\"lastOnlineUtc\":\"" + player.LastOnline.ToUniversalTime().ToString("yyyy'-'MMM'-'dd' 'HH':'mm':'ss") + "\"," +//"u"
              "\"isOnline\":\"" + player.IsOnline.ToString() + "\",";
            };

            output += "\"" + _steamId + "\":{" +
              "\"steamId\":" + "\"" + _steamId + "\"," +
              p +
              "\"lastWriteTimeUtc\":" + "\"" + file.LastWriteTimeUtc.ToString("yyyy'-'MMM'-'dd' 'HH':'mm':'ss") + "\"" + "" +

            //"\"Extension\":" + "\"" + file.Extension + "\"" + ", " +
            //"\"CreationTime\":" + "\"" + file.CreationTime + "\"" + ", " +
            //"\"LastAccessTime\":" + "\"" + file.LastAccessTime + "\"" + ", " + 
            //"\"Attributes\":" + "\"" + file.Attributes + "\"" + "" +
            "},";
          }
        }
      }
      output = output.Substring(0, output.Length - 1);
      output += "}";

      if (_options.ContainsKey("tag"))
      {
        if (_options["tag"] == null)
        {
          _options["tag"] = "bc-pfinfo";
        }

        SendOutput("{\"tag\":\"" + _options["tag"] + "\",\"data\":" + output + "}");
      }
      else
      {
        SendOutput(output);
      }


    }
  }
}
