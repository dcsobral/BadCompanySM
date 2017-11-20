using System.Collections.Generic;
using System.Linq;

namespace BCM.Commands
{
  public class BCVersions : BCCommandAbstract
  {
    public class BCMModInfo
    {
      public string Name;
      public string Version;
      public string Website;
      public string Description;
      public string Author;
      public string Path;

      public BCMModInfo()
      {
      }

      public BCMModInfo(Mod mod)
      {
        Name = mod.ModInfo.Name.ToString();
        Version = mod.ModInfo.Version.ToString();
        Website = mod.ModInfo.Website.ToString();
        Description = mod.ModInfo.Description.ToString();
        Author = mod.ModInfo.Author.ToString();
        Path = System.IO.Path.GetFileName(mod.Path);
      }
    }

    public override void Process()
    {
      var loadedMods = ModManager.GetLoadedMods();
      var mods = new List<BCMModInfo>();

      var gameVer = new BCMModInfo
      {
        Name = $"{Constants.cProduct} ({Constants.cProductAbbrev})",
        Version = $"{Constants.cVersionType} {Constants.cVersionMajor}.{Constants.cVersionMinor} {Constants.cVersionBuild}",
        Website = "http://7daystodie.com"
      };

      mods.Add(gameVer);
      mods.AddRange(loadedMods.Select(mod => new BCMModInfo(mod)));

      SendJson(mods);
    }
  }
}
