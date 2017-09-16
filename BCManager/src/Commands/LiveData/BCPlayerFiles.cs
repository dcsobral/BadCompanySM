using BCM.PersistentData;
using System.Collections.Generic;
using System.IO;

namespace BCM.Commands
{
  public class BCPlayerFiles : BCCommandAbstract
  {
    public class BCMPlayerDataFile
    {
      public string Name;
      public string SteamId;
      public bool IsOnline;
      public string LastOnline;
      public string LastWrite;
    }

    private static FileSystemInfo[] GetFiles(string path)
    {
      var root = new DirectoryInfo(path);
      if (!root.Exists) { return null; }
      var files = root.GetFileSystemInfos();
      return files;
    }

    public override void Process()
    {
      var players = new List<object>();
      var path = GameUtils.GetPlayerDataDir();
      var files = GetFiles(path);

      if (files != null)
      {
        foreach (var file in files)
        {
          if (file.Extension != ".ttp") continue;
          var pdf = new BCMPlayerDataFile
          {
            SteamId = file.Name.Substring(0, file.Name.Length - file.Extension.Length),
            LastWrite = file.LastWriteTimeUtc.ToString("yyyy-MM-ddTHH:mm:ssZ")
          };
          var player = PersistentContainer.Instance.Players[pdf.SteamId, false];
          if (player != null)
          {
            pdf.Name = player.Name;
            pdf.LastOnline = player.LastOnline.ToString("yyyy-MM-ddTHH:mm:ssZ");
            pdf.IsOnline = player.IsOnline;
          }

          players.Add(pdf);
        }
      }
      SendJson(players);
    }
  }
}
