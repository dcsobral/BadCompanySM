using BCM.PersistentData;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
      public string LastLogPos;
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
      var players = new List<BCMPlayerDataFile>();
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
            LastWrite = file.LastWriteTimeUtc.ToUtcStr()
          };
          var player = PersistentContainer.Instance.Players[pdf.SteamId, false];
          if (player != null)
          {
            pdf.Name = player.Name;
            pdf.LastOnline = player.LastOnline.ToUtcStr();
            pdf.IsOnline = player.IsOnline;
            pdf.LastLogPos = player.LastLogoutPos?.ToString();
          }

          players.Add(pdf);
        }
      }
      if (Options.ContainsKey("min"))
      {
        SendJson(players.Select(player => new[]
          {
            player.Name, player.SteamId, player.IsOnline.ToString(), player.LastOnline, player.LastWrite, player.LastLogPos
          }
        ).ToList());
      }
      else
      {
        SendJson(players);
      }
    }
  }
}
