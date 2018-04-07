using JetBrains.Annotations;

namespace BCM.Models
{
  public class BCMModInfo
  {
    [UsedImplicitly] public string Name;
    [UsedImplicitly] public string Version;
    [UsedImplicitly] public string Website;
    [UsedImplicitly] public string Description;
    [UsedImplicitly] public string Author;
    [UsedImplicitly] public string Path;

    public BCMModInfo([NotNull] Mod mod)
    {
      Name = mod.ModInfo.Name.ToString();
      Version = mod.ModInfo.Version.ToString();
      Website = mod.ModInfo.Website.ToString();
      Description = mod.ModInfo.Description.ToString();
      Author = mod.ModInfo.Author.ToString();
      Path = System.IO.Path.GetFileName(mod.Path);
    }

    public BCMModInfo(string name, string version, string website, string description, string author)
    {
      Name = name;
      Version = version;
      Website = website;
      Description = description;
      Author = author;
    }
  }
}