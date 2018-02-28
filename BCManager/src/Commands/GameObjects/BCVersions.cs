using System.Collections.Generic;
using System.Linq;
using BCM.Models;

namespace BCM.Commands
{
  public class BCVersions : BCCommandAbstract
  {
    public override void Process()
    {
      var loadedMods = ModManager.GetLoadedMods();
      var mods = new List<BCMModInfo>();

      var gameVer = new BCMModInfo
      {
        Name = $"{Constants.cProduct} ({Constants.cProductAbbrev})",
        Version = $"{Constants.cVersionType} {Constants.cVersionMajor}.{Constants.cVersionMinor} {Constants.cVersionBuild}",
        Website = "http://7daystodie.com",
        Author = "The Fun Pimps",
        Description = "The survival horde crafting game"
      };

      mods.Add(gameVer);
      mods.AddRange(loadedMods.Select(mod => new BCMModInfo(mod)));

      SendJson(mods);
    }
  }
}
