namespace BCM.Models
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
}