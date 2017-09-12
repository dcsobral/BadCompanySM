using System;
using System.Collections.Generic;
using System.Reflection;
using LitJson;
using System.Linq;
using BCM.Models;

namespace BCM.Commands
{
  public class BCCommandAbstract : ConsoleCmdAbstract
  {
    public static CommandSenderInfo SenderInfo;
    public static List<string> Params = new List<string>();
    public static Dictionary<string,string> Options = new Dictionary<string, string>();

    [Obsolete]
    public string Sep = "";

    public override string GetDescription() => Config.GetDescription(GetType().Name);

    public override string GetHelp() => Config.GetHelp(GetType().Name);

    public override string[] GetCommands() => Config.GetCommands(GetType().Name);

    public override int DefaultPermissionLevel => Config.GetDefaultPermissionLevel(GetType().Name);

    public override void Execute(List<string> _params, CommandSenderInfo senderInfo)
    {
      SenderInfo = senderInfo;
      Params = new List<string>();
      Options = new Dictionary<string, string>();
      ParseParams(_params);
      try
      {
        Process();
      }
      catch (Exception e)
      {
        SendOutput("Error while executing command.");
        Log.Out($"{Config.ModPrefix} Error in {GetType().Name}.{MethodBase.GetCurrentMethod().Name}: {e}");
      }
    }

    public virtual void Process(Dictionary<string, string> o, List<string> p)
    {
      Options = o;
      Params = p;
      Process();
    }

    public virtual void Process()
    {
      // function to override in extention commands instead of Execute
      // this allows param parsing and exception handling to be done in this class
    }

    private void ParseParams(List<string> _params)
    {
      foreach (var param in _params)
      {
        if (param.IndexOf('/', 0) != 0)
        {
          Params.Add(param);
        }
        else
        {
          if (param.IndexOf('=', 1) == -1)
          {
            Options.Add(param.Substring(1).ToLower(), null);
          }
          else
          {
            var p1 = param.Substring(1).Split('=');
            Options.Add(p1[0] == "f" ? "filter" : p1[0], p1[1]);
          }
        }
      }

      var defaultoptions = Config.GetDefaultOptions(GetType().Name);
      foreach (var _default in defaultoptions.Split(','))
      {
        var add = _default.Trim().ToLower();
        if ((add != "online" || !Options.ContainsKey("offline") && !Options.ContainsKey("all")) &&
            (add != "offline" || !Options.ContainsKey("online") && !Options.ContainsKey("all")) &&
            (add != "nl" || !Options.ContainsKey("nonl")) && (add != "csv" || !Options.ContainsKey("nocsv")) &&
            (add != "details" || !Options.ContainsKey("nodetails")) &&
            (add != "strpos" || !Options.ContainsKey("vectors")) &&
            (add != "strpos" || !Options.ContainsKey("csvpos")) &&
            (add != "strpos" || !Options.ContainsKey("worldpos")) && !Options.ContainsKey(add))
        {
          Options.Add(add, null);
        }
      }
    }

    public static void SendJsonError(string err)
    {
      SendJson(new { error = err });
    }

    public static void SendJson(object data)
    {
      var writer = new JsonWriter();
      if (Options.ContainsKey("pp") && !Options.ContainsKey("1l"))
      {
        writer.IndentValue = 2;
        writer.PrettyPrint = true;
      }

      var jsonOut = new Dictionary<string, object>();
      if (Options.ContainsKey("tag"))
      {
        jsonOut.Add("tag", Options["tag"]);
        jsonOut.Add("data", data);
        JsonMapper.ToJson(jsonOut, writer);
      }
      else
      {
        JsonMapper.ToJson(data, writer);
      }

      SendOutput(writer.ToString().TrimStart());
    }

    public static void SendOutput(string output)
    {
      if (Options.ContainsKey("log"))
      {
        Log.Out(output);
      }
      else if (Options.ContainsKey("chat"))
      {
        if (Options.ContainsKey("color"))
        {
          output = $"[{Options["color"]}]{output}[-]";
        }
        foreach (var text in output.Split('\n'))
        {
          GameManager.Instance.GameMessageServer(null, EnumGameMessages.Chat, text, "Server", false, string.Empty, false);
        }
      }
      else
      {
        foreach (var text in output.Split('\n'))
        {
          SdtdConsole.Instance.Output(text);
        }
      }
    }

    public static List<string> GetFilters(string type)
    {
      if (!Options.ContainsKey("filter")) return new List<string>();

      var filter = new List<string>();
      var filters = Options["filter"].ToLower().Split(',').ToList();
      foreach (var cur in filters)
      {
        var str = int.TryParse(cur, out int intFilter) ? GetFilter(intFilter, type) : cur;
        if (str == null) continue;

        if (filter.Contains(str))
        {
          Log.Out($"{Config.ModPrefix} Duplicate filter index *{str}* in {filters.Aggregate("", (c, f) => c + (f == cur ? $"*{f}*," : $"{f},"))} skipping");

          continue;
        }

        filter.Add(str);
      }
      return filter;
    }

    public static void SendObject(BCMAbstract gameobj)
    {
      if (Options.ContainsKey("min"))
      {
        SendJson(new List<List<object>> { gameobj.Data().Select(d => d.Value).ToList() });
      }
      else
      {
        SendJson(gameobj.Data());
      }
    }

    private static string GetFilter(int f, string type)
    {
      switch (type)
      {
        case BCMGameObject.GOTypes.Players:
          return BCMPlayer.FilterMap.ContainsKey(f) ? BCMPlayer.FilterMap[f] : null;
        case BCMGameObject.GOTypes.Entities:
          return BCMEntity.FilterMap.ContainsKey(f) ? BCMEntity.FilterMap[f] : null;

        case BCMGameObject.GOTypes.Archetypes:
          return BCMArchetype.FilterMap.ContainsKey(f) ? BCMArchetype.FilterMap[f] : null;
        case BCMGameObject.GOTypes.BiomeSpawning:
          return BCMBiomeSpawn.FilterMap.ContainsKey(f) ? BCMBiomeSpawn.FilterMap[f] : null;
        case BCMGameObject.GOTypes.Biomes:
          return BCMBiome.FilterMap.ContainsKey(f) ? BCMBiome.FilterMap[f] : null;
        case BCMGameObject.GOTypes.Blocks:
          return BCMItemClass.FilterMap.ContainsKey(f) ? BCMItemClass.FilterMap[f] : null;
        case BCMGameObject.GOTypes.Buffs:
          return BCMBuff.FilterMap.ContainsKey(f) ? BCMBuff.FilterMap[f] : null;
        case BCMGameObject.GOTypes.EntityClasses:
          return BCMEntityClass.FilterMap.ContainsKey(f) ? BCMEntityClass.FilterMap[f] : null;
        case BCMGameObject.GOTypes.EntityGroups:
          return BCMEntityGroup.FilterMap.ContainsKey(f) ? BCMEntityGroup.FilterMap[f] : null;
        case BCMGameObject.GOTypes.ItemClasses:
          return BCMItemClass.FilterMap.ContainsKey(f) ? BCMItemClass.FilterMap[f] : null;
        case BCMGameObject.GOTypes.Items:
          return BCMItemClass.FilterMap.ContainsKey(f) ? BCMItemClass.FilterMap[f] : null;
        case BCMGameObject.GOTypes.LootContainers:
          return BCMLootContainer.FilterMap.ContainsKey(f) ? BCMLootContainer.FilterMap[f] : null;
        case BCMGameObject.GOTypes.LootGroups:
          return BCMLootGroup.FilterMap.ContainsKey(f) ? BCMLootGroup.FilterMap[f] : null;
        case BCMGameObject.GOTypes.LootProbabilityTemplates:
          return BCMLootProbabilityTemplate.FilterMap.ContainsKey(f) ? BCMLootProbabilityTemplate.FilterMap[f] : null;
        case BCMGameObject.GOTypes.LootQualityTemplates:
          return BCMLootQualityTemplate.FilterMap.ContainsKey(f) ? BCMLootQualityTemplate.FilterMap[f] : null;
        case BCMGameObject.GOTypes.Materials:
          return BCMMaterial.FilterMap.ContainsKey(f) ? BCMMaterial.FilterMap[f] : null;
        case BCMGameObject.GOTypes.Prefabs:
          return BCMPrefab.FilterMap.ContainsKey(f) ? BCMPrefab.FilterMap[f] : null;
        case BCMGameObject.GOTypes.Quests:
          return BCMQuest.FilterMap.ContainsKey(f) ? BCMQuest.FilterMap[f] : null;
        case BCMGameObject.GOTypes.Recipes:
          return BCMRecipe.FilterMap.ContainsKey(f) ? BCMRecipe.FilterMap[f] : null;
        case BCMGameObject.GOTypes.Rwg:
          return BCMRWG.FilterMap.ContainsKey(f) ? BCMRWG.FilterMap[f] : null;
        case BCMGameObject.GOTypes.Skills:
          return BCMSkill.FilterMap.ContainsKey(f) ? BCMSkill.FilterMap[f] : null;
        case BCMGameObject.GOTypes.Spawners:
          return BCMSpawner.FilterMap.ContainsKey(f) ? BCMSpawner.FilterMap[f] : null;
        default:
          return null;
      }
    }

  }
}
