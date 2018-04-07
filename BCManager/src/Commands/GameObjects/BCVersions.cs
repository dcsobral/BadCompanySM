using System.Collections.Generic;
using System.Linq;
using BCM.Models;
using JetBrains.Annotations;

namespace BCM.Commands
{
  [UsedImplicitly]
  public class BCVersions : BCCommandAbstract
  {
    protected override void Process()
    {
      var loadedMods = ModManager.GetLoadedMods();
      var mods = new List<BCMModInfo>();

      var gameVer = new BCMModInfo(
        $"{Constants.cProduct} ({Constants.cProductAbbrev})",
        $"{Constants.cVersionType} {Constants.cVersionMajor}.{Constants.cVersionMinor} {Constants.cVersionBuild}",
        "http://7daystodie.com",
        "The Fun Pimps",
        "The survival horde crafting game"
      );

      mods.Add(gameVer);
      mods.AddRange(loadedMods.Select(mod => new BCMModInfo(mod)));

      SendJson(mods);
    }
  }
}
