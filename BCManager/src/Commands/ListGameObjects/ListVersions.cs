using System.Collections.Generic;

namespace BCM.Commands
{
  public class ListVersions : BCCommandAbstract
  {
    public virtual Dictionary<string, Dictionary<string, string>> jsonObject()
    {
      var LoadedMods = ModManager.GetLoadedMods();
      var Mods = new Dictionary<string, Dictionary<string, string>>();

      var gameVer = new Dictionary<string, string>();

      gameVer.Add("name", "Vanilla");
      gameVer.Add("version", Constants.cVersion);
      gameVer.Add("website", "http://7daystodie.com");

      Mods["GameVersion"] = gameVer;

      int index = 0;
      foreach (Mod _mod in LoadedMods)
      {
        var mod = new Dictionary<string, string>();

        //mod.Add("path", _mod.Path.Replace("\"", "\\\""));
        //mod.Add("folderName", _mod.FolderName.Replace("\"", "\\\""));
        mod.Add("name", _mod.ModInfo.Name.Value.Replace("\"", "\\\""));
        mod.Add("version", _mod.ModInfo.Version.Value.Replace("\"", "\\\""));
        mod.Add("website", _mod.ModInfo.Website.Value.Replace("\"", "\\\""));
        mod.Add("description", _mod.ModInfo.Description.Value.Replace("\"", "\\\""));
        mod.Add("author", _mod.ModInfo.Author.Value.Replace("\"", "\\\""));

        Mods[index.ToString()] = new Dictionary<string, string>(mod);
        index++;
      }

      return Mods;
    }
    public override void Process()
    {
      string output = "";
      if (_options.ContainsKey("json"))
      {
        output = BCUtils.toJson(jsonObject());
        SendOutput(output);
      }
      else
      {
        output = "Not Implemented, use /json";
        SendOutput(output);
      }
    }
  }
}
