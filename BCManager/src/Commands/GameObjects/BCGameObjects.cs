using System.Collections.Generic;
using BCM.Models;
using System;
using System.IO;
using RWG2.Rules;
using System.Linq;
using JetBrains.Annotations;

namespace BCM.Commands
{
  [UsedImplicitly]
  public class BCGameObjects : BCCommandAbstract
  {
    //todo: count sub command that returns a count of each object type

    protected override void Process()
    {
      if (!BCUtils.CheckWorld()) return;

      string type;
      switch (Params.Count)
      {
        case 0:
          //LIST OBJECT TYPES
          var o = typeof(BCMGameObject.GOTypes);
          SendJson(
            o.GetFields()
            .AsQueryable()
            .Where(f => f.Name != "Players" && f.Name != "Entities")
            .ToDictionary(field => field.Name, field => field.GetValue(o).ToString()));
          return;

        case 1:
          //ALL OBJECTS OF A TYPE
          type = Params[0].ToLower();

          if (Options.ContainsKey("filters"))
          {
            DisplayFilters(type);

            return;
          }

          if (Options.ContainsKey("index"))
          {
            DisplayIndex(type);

            return;
          }

          //if (type == "count")
          //{
          //  CountObjects();
          //  return;
          //}
          GetObjects(type, out var objects);
          ProcessObjects(type, objects, out var data, GetFilters(type));
          SendJson(type == BCMGameObject.GOTypes.Rwg ? data[0] : data);
          return;

        case 2:
          //SPECIFIC OBJECT
          type = Params[0].ToLower();
          if (!GetObject(type, out var obj)) { return; }
          var gameObject = new BCMGameObject(obj, type, Options, GetFilters(type));
          SendObject(gameObject);
          return;

        default:
          SendOutput("Wrong number of arguments");
          SendOutput(Config.GetHelp(GetType().Name));
          return;
      }
    }

    private static void DisplayIndex(string type)
    {
      switch (type)
      {
        case BCMGameObject.GOTypes.Archetypes:
          SendJson(BCMArchetype.FilterMap.GroupBy(kvp => kvp.Value).Select(group => group.First()).ToDictionary(kvp => kvp.Value, kvp => kvp.Key));
          break;

        case BCMGameObject.GOTypes.Biomes:
          SendJson(BCMBiome.FilterMap.GroupBy(kvp => kvp.Value).Select(group => group.First()).ToDictionary(kvp => kvp.Value, kvp => kvp.Key));
          break;

        case BCMGameObject.GOTypes.BiomeSpawning:
          SendJson(BCMBiomeSpawn.FilterMap.GroupBy(kvp => kvp.Value).Select(group => group.First()).ToDictionary(kvp => kvp.Value, kvp => kvp.Key));
          break;

        case BCMGameObject.GOTypes.Blocks:
          SendJson(BCMItemClass.FilterMap.GroupBy(kvp => kvp.Value).Select(group => group.First()).ToDictionary(kvp => kvp.Value, kvp => kvp.Key));
          break;

        case BCMGameObject.GOTypes.Buffs:
          SendJson(BCMBuff.FilterMap.GroupBy(kvp => kvp.Value).Select(group => group.First()).ToDictionary(kvp => kvp.Value, kvp => kvp.Key));
          break;

        case BCMGameObject.GOTypes.EntityClasses:
          SendJson(BCMEntityClass.FilterMap.GroupBy(kvp => kvp.Value).Select(group => group.First()).ToDictionary(kvp => kvp.Value, kvp => kvp.Key));
          break;

        case BCMGameObject.GOTypes.EntityGroups:
          SendJson(BCMEntityGroup.FilterMap.GroupBy(kvp => kvp.Value).Select(group => group.First()).ToDictionary(kvp => kvp.Value, kvp => kvp.Key));
          break;

        case BCMGameObject.GOTypes.ItemClasses:
          SendJson(BCMItemClass.FilterMap.GroupBy(kvp => kvp.Value).Select(group => group.First()).ToDictionary(kvp => kvp.Value, kvp => kvp.Key));
          break;

        case BCMGameObject.GOTypes.Items:
          SendJson(BCMItemClass.FilterMap.GroupBy(kvp => kvp.Value).Select(group => group.First()).ToDictionary(kvp => kvp.Value, kvp => kvp.Key));
          break;

        case BCMGameObject.GOTypes.LootContainers:
          SendJson(BCMLootContainer.FilterMap.GroupBy(kvp => kvp.Value).Select(group => group.First()).ToDictionary(kvp => kvp.Value, kvp => kvp.Key));
          break;

        case BCMGameObject.GOTypes.LootGroups:
          SendJson(BCMLootGroup.FilterMap.GroupBy(kvp => kvp.Value).Select(group => group.First()).ToDictionary(kvp => kvp.Value, kvp => kvp.Key));
          break;

        case BCMGameObject.GOTypes.LootProbabilityTemplates:
          SendJson(BCMLootProbabilityTemplate.FilterMap.GroupBy(kvp => kvp.Value).Select(group => group.First()).ToDictionary(kvp => kvp.Value, kvp => kvp.Key));
          break;

        case BCMGameObject.GOTypes.LootQualityTemplates:
          SendJson(BCMLootQualityTemplate.FilterMap.GroupBy(kvp => kvp.Value).Select(group => group.First()).ToDictionary(kvp => kvp.Value, kvp => kvp.Key));
          break;

        case BCMGameObject.GOTypes.Materials:
          SendJson(BCMMaterial.FilterMap.GroupBy(kvp => kvp.Value).Select(group => group.First()).ToDictionary(kvp => kvp.Value, kvp => kvp.Key));
          break;

        case BCMGameObject.GOTypes.Prefabs:
          SendJson(BCMPrefab.FilterMap.GroupBy(kvp => kvp.Value).Select(group => group.First()).ToDictionary(kvp => kvp.Value, kvp => kvp.Key));
          break;

        case BCMGameObject.GOTypes.Quests:
          SendJson(BCMQuest.FilterMap.GroupBy(kvp => kvp.Value).Select(group => group.First()).ToDictionary(kvp => kvp.Value, kvp => kvp.Key));
          break;

        case BCMGameObject.GOTypes.Recipes:
          SendJson(BCMRecipe.FilterMap.GroupBy(kvp => kvp.Value).Select(group => group.First()).ToDictionary(kvp => kvp.Value, kvp => kvp.Key));
          break;

        case BCMGameObject.GOTypes.Rwg:
          SendJson(BCMRWG.FilterMap.GroupBy(kvp => kvp.Value).Select(group => group.First()).ToDictionary(kvp => kvp.Value, kvp => kvp.Key));
          break;

        case BCMGameObject.GOTypes.Skills:
          SendJson(BCMSkill.FilterMap.GroupBy(kvp => kvp.Value).Select(group => group.First()).ToDictionary(kvp => kvp.Value, kvp => kvp.Key));
          break;

        case BCMGameObject.GOTypes.Spawners:
          SendJson(BCMSpawner.FilterMap.GroupBy(kvp => kvp.Value).Select(group => group.First()).ToDictionary(kvp => kvp.Value, kvp => kvp.Key));
          break;

        case BCMGameObject.GOTypes.Players:
          SendOutput("use bc-lp instead for player data");
          break;

        case BCMGameObject.GOTypes.Entities:
          SendOutput("use bc-le instead for entity data");
          break;

        default:
          SendJsonError("Unknown Type");
          break;
      }
    }

    private static void DisplayFilters(string type)
    {
      switch (type)
      {
        case BCMGameObject.GOTypes.Archetypes:
          SendJson(typeof(BCMArchetype.StrFilters).GetFields().ToDictionary(field => field.Name, field => $"{field.GetValue(typeof(BCMPlayer.StrFilters))}"));
          break;

        case BCMGameObject.GOTypes.Biomes:
          SendJson(typeof(BCMBiome.StrFilters).GetFields().ToDictionary(field => field.Name, field => $"{field.GetValue(typeof(BCMPlayer.StrFilters))}"));
          break;

        case BCMGameObject.GOTypes.BiomeSpawning:
          SendJson(typeof(BCMBiomeSpawn.StrFilters).GetFields().ToDictionary(field => field.Name, field => $"{field.GetValue(typeof(BCMPlayer.StrFilters))}"));
          break;

        case BCMGameObject.GOTypes.Blocks:
          SendJson(typeof(BCMItemClass.StrFilters).GetFields().ToDictionary(field => field.Name, field => $"{field.GetValue(typeof(BCMPlayer.StrFilters))}"));
          break;

        case BCMGameObject.GOTypes.Buffs:
          SendJson(typeof(BCMBuff.StrFilters).GetFields().ToDictionary(field => field.Name, field => $"{field.GetValue(typeof(BCMPlayer.StrFilters))}"));
          break;

        case BCMGameObject.GOTypes.EntityClasses:
          SendJson(typeof(BCMEntityClass.StrFilters).GetFields().ToDictionary(field => field.Name, field => $"{field.GetValue(typeof(BCMPlayer.StrFilters))}"));
          break;

        case BCMGameObject.GOTypes.EntityGroups:
          SendJson(typeof(BCMEntityGroup.StrFilters).GetFields().ToDictionary(field => field.Name, field => $"{field.GetValue(typeof(BCMPlayer.StrFilters))}"));
          break;

        case BCMGameObject.GOTypes.ItemClasses:
          SendJson(typeof(BCMItemClass.StrFilters).GetFields().ToDictionary(field => field.Name, field => $"{field.GetValue(typeof(BCMPlayer.StrFilters))}"));
          break;

        case BCMGameObject.GOTypes.Items:
          SendJson(typeof(BCMItemClass.StrFilters).GetFields().ToDictionary(field => field.Name, field => $"{field.GetValue(typeof(BCMPlayer.StrFilters))}"));
          break;

        case BCMGameObject.GOTypes.LootContainers:
          SendJson(typeof(BCMLootContainer.StrFilters).GetFields().ToDictionary(field => field.Name, field => $"{field.GetValue(typeof(BCMPlayer.StrFilters))}"));
          break;

        case BCMGameObject.GOTypes.LootGroups:
          SendJson(typeof(BCMLootGroup.StrFilters).GetFields().ToDictionary(field => field.Name, field => $"{field.GetValue(typeof(BCMPlayer.StrFilters))}"));
          break;

        case BCMGameObject.GOTypes.LootProbabilityTemplates:
          SendJson(typeof(BCMLootProbabilityTemplate.StrFilters).GetFields().ToDictionary(field => field.Name, field => $"{field.GetValue(typeof(BCMPlayer.StrFilters))}"));
          break;

        case BCMGameObject.GOTypes.LootQualityTemplates:
          SendJson(typeof(BCMLootQualityTemplate.StrFilters).GetFields().ToDictionary(field => field.Name, field => $"{field.GetValue(typeof(BCMPlayer.StrFilters))}"));
          break;

        case BCMGameObject.GOTypes.Materials:
          SendJson(typeof(BCMMaterial.StrFilters).GetFields().ToDictionary(field => field.Name, field => $"{field.GetValue(typeof(BCMPlayer.StrFilters))}"));
          break;

        case BCMGameObject.GOTypes.Prefabs:
          SendJson(typeof(BCMPrefab.StrFilters).GetFields().ToDictionary(field => field.Name, field => $"{field.GetValue(typeof(BCMPlayer.StrFilters))}"));
          break;

        case BCMGameObject.GOTypes.Quests:
          SendJson(typeof(BCMQuest.StrFilters).GetFields().ToDictionary(field => field.Name, field => $"{field.GetValue(typeof(BCMPlayer.StrFilters))}"));
          break;

        case BCMGameObject.GOTypes.Recipes:
          SendJson(typeof(BCMRecipe.StrFilters).GetFields().ToDictionary(field => field.Name, field => $"{field.GetValue(typeof(BCMPlayer.StrFilters))}"));
          break;

        case BCMGameObject.GOTypes.Rwg:
          SendJson(typeof(BCMRWG.StrFilters).GetFields().ToDictionary(field => field.Name, field => $"{field.GetValue(typeof(BCMPlayer.StrFilters))}"));
          break;

        case BCMGameObject.GOTypes.Skills:
          SendJson(typeof(BCMSkill.StrFilters).GetFields().ToDictionary(field => field.Name, field => $"{field.GetValue(typeof(BCMPlayer.StrFilters))}"));
          break;

        case BCMGameObject.GOTypes.Spawners:
          SendJson(typeof(BCMSpawner.StrFilters).GetFields().ToDictionary(field => field.Name, field => $"{field.GetValue(typeof(BCMPlayer.StrFilters))}"));
          break;

        case BCMGameObject.GOTypes.Players:
          SendOutput("use bc-lp instead for player data");
          break;

        case BCMGameObject.GOTypes.Entities:
          SendOutput("use bc-le instead for entity data");
          break;

        default:
          SendJsonError("Unknown Type");
          break;
      }
    }

    private static void ProcessObjects(string type, IEnumerable<object> objects, out List<object> data, List<string> filter)
    {
      data = new List<object>();

      foreach (var obj in objects)
      {
        var go = new BCMGameObject(obj, type, Options, filter);
        if (Options.ContainsKey("min"))
        {
          data.Add(go.Data().Select(d => d.Value).ToList());
        }
        else
        {
          data.Add(go.Data());
        }
      }
    }

    private static bool GetObject(string type, out object obj)
    {
      obj = null;
      var key = Params[1];
      var world = GameManager.Instance.World;

      switch (type)
      {
        case BCMGameObject.GOTypes.Archetypes:
          obj = Archetypes.Instance.GetArchetype(key);
          break;
        case BCMGameObject.GOTypes.Biomes:
          if (byte.TryParse(key, out var id))
          {
            obj = world.Biomes.GetBiome(id);
          }
          else
          {
            SendOutput("<id> must be a number for biomes");

            return false;
          }
          break;
        case BCMGameObject.GOTypes.BiomeSpawning:
          break;
        case BCMGameObject.GOTypes.Blocks:
          break;
        case BCMGameObject.GOTypes.Buffs:
          break;
        case BCMGameObject.GOTypes.EntityClasses:
          break;
        case BCMGameObject.GOTypes.EntityGroups:
          break;
        case BCMGameObject.GOTypes.ItemClasses:
          break;
        case BCMGameObject.GOTypes.Items:
          break;
        //case BCMGameObject.GOTypes.Loot:
        //  break;
        case BCMGameObject.GOTypes.LootContainers:
          break;
        case BCMGameObject.GOTypes.LootGroups:
          break;
        //case BCMGameObject.GOTypes.LootPlaceholders:
        //  break;
        case BCMGameObject.GOTypes.LootProbabilityTemplates:
          break;
        case BCMGameObject.GOTypes.LootQualityTemplates:
          break;
        case BCMGameObject.GOTypes.Materials:
          break;
        case BCMGameObject.GOTypes.Prefabs:
          break;
        case BCMGameObject.GOTypes.Quests:
          break;
        case BCMGameObject.GOTypes.Recipes:
          break;
        case BCMGameObject.GOTypes.Rwg:
          break;
        case BCMGameObject.GOTypes.Skills:
          break;
        case BCMGameObject.GOTypes.Spawners:
          break;
        default:
          SendJsonError("Unknown Type");
          break;
      }

      if (obj != null) return true;

      SendOutput("Object not found.");

      return false;
    }

    private static void GetObjects(string type, out List<object> objects)
    {
      objects = new List<object>();
      switch (type)
      {
        case BCMGameObject.GOTypes.Archetypes:
          GetArchetypes(objects);
          break;

        case BCMGameObject.GOTypes.Biomes:
          GetBiomes(objects);
          break;

        case BCMGameObject.GOTypes.BiomeSpawning:
          GetSpawning(objects);
          break;

        case BCMGameObject.GOTypes.Blocks:
          GetBlocks(objects);
          break;

        case BCMGameObject.GOTypes.Buffs:
          GetBuffs(objects);
          break;

        case BCMGameObject.GOTypes.EntityClasses:
          GetEntityClasses(objects);
          break;

        case BCMGameObject.GOTypes.EntityGroups:
          GetEntityGroups(objects);
          break;

        case BCMGameObject.GOTypes.ItemClasses:
          GetItemClasses(objects);
          break;

        case BCMGameObject.GOTypes.Items:
          GetItems(objects);
          break;

        //case BCMGameObject.GOTypes.Loot:
        //  GetLoot(objects);
        //  break;

        case BCMGameObject.GOTypes.LootContainers:
          GetLootContainers(objects);
          break;

        case BCMGameObject.GOTypes.LootGroups:
          GetLootGroups(objects);
          break;

        //case BCMGameObject.GOTypes.LootPlaceholders:
        //  //todo
        //  break;

        case BCMGameObject.GOTypes.LootProbabilityTemplates:
          GetLootProbabilityTemplates(objects);
          break;

        case BCMGameObject.GOTypes.LootQualityTemplates:
          GetLootQualityTemplates(objects);
          break;

        case BCMGameObject.GOTypes.Materials:
          GetMaterials(objects);
          break;

        case BCMGameObject.GOTypes.Prefabs:
          GetPrefabs(objects);
          break;

        case BCMGameObject.GOTypes.Quests:
          GetQuests(objects);
          break;

        case BCMGameObject.GOTypes.Recipes:
          GetRecipes(objects);
          break;

        case BCMGameObject.GOTypes.Rwg:
          GetRWG(objects);
          break;

        case BCMGameObject.GOTypes.Skills:
          GetSkills(objects);
          break;

        case BCMGameObject.GOTypes.Spawners:
          GetSpawners(objects);
          break;

        case BCMGameObject.GOTypes.Players:
          SendOutput("use bc-lp instead for player data");
          break;

        case BCMGameObject.GOTypes.Entities:
          SendOutput("use bc-le instead for entity data");
          break;

        default:
          SendJsonError("Unknown Type");
          break;
      }
    }

    //todo: aggregate days
    private static void GetSpawners(List<object> objects)
    {
      objects.AddRange(EntitySpawnerClass.list.Keys.Select(b => new KeyValuePair<string, EntitySpawnerClassForDay>(b, EntitySpawnerClass.list[b])).Cast<object>());
    }

    private static void GetSkills(List<object> objects)
    {
      IEnumerable<Skill> skills = Skills.AllSkills.Values;
      var skillnames = new Func<Skill, string>(OrderByCallback);
      objects.AddRange(skills.OrderBy(skillnames).Cast<object>());
    }

    private static void GetRWG(List<object> objects)
    {
      //todo: sections GOTypes
      var sections = new Dictionary<string, object>
      {
        {"Rulesets", RWGRules.Instance.Rulesets},
        {"CellRules", RWGRules.Instance.CellRules},
        {"HubRules", RWGRules.Instance.HubRules},
        {"WildernessRules", RWGRules.Instance.WildernessRules},
        {"PrefabSpawnRules", RWGRules.Instance.PrefabSpawnRules},
        {"BiomeSpawnRules", RWGRules.Instance.BiomeSpawnRules},
        {"HubLayouts", RWGRules.Instance.HubLayouts},
        {"RulesetName", RWGRules.Instance.RulesetName}
      };

      objects.Add(sections);
    }

    private static void GetRecipes(List<object> objects)
    {
      objects.AddRange(CraftingManager.GetAllRecipes());
    }

    private static void GetQuests(List<object> objects)
    {
      objects.AddRange(QuestClass.s_Quests.Values.Cast<object>());
    }

    private static void GetPrefabs(List<object> objects)
    {
      var prefabsGameDir = Utils.GetGameDir("Data/Prefabs");
      var files = Directory.GetFiles(prefabsGameDir, "*");
      for (var i = files.Length - 1; i >= 0; i--)
      {
        var file = files[i];
        if (Path.GetExtension(file) != ".tts") continue;

        var dirLen = prefabsGameDir.Length + 1;
        var fileLen = file.Length - dirLen - 4;
        if (dirLen + fileLen > file.Length) continue;

        objects.Add(file.Substring(dirLen, fileLen));
      }
    }

    private static void GetMaterials(List<object> objects)
    {
      objects.AddRange(MaterialBlock.materials.Values.Cast<object>());
    }

    private static void GetLootQualityTemplates(List<object> objects)
    {
      objects.AddRange(LootContainer.lootQualityTemplates.Values.Cast<object>());
    }

    private static void GetLootProbabilityTemplates(List<object> objects)
    {
      objects.AddRange(LootContainer.lootProbTemplates.Values.Cast<object>());
    }

    private static void GetLootGroups(List<object> objects)
    {
      objects.AddRange(LootContainer.lootGroups.Values.Cast<object>());
    }

    private static void GetLootContainers(List<object> objects)
    {
      for (var i = 0; i <= LootContainer.lootList.Length - 1; i++)
      {
        if (LootContainer.lootList[i] == null) continue;

        objects.Add(LootContainer.lootList[i]);
      }
    }

    //private static void GetLoot(List<object> objects)
    //{
    //  //todo: summary option for listing sub types and primary keys
    //  objects.Add(null);
    //}

    private static void GetItems(List<object> objects)
    {
      const int start = 4097;
      var end = ItemClass.list.Length;

      for (var i = start; i <= end - 1; i++)
      {
        if (ItemClass.list[i] == null) continue;

        objects.Add(ItemClass.list[i]);
      }
    }

    private static void GetItemClasses(List<object> objects)
    {
      const int start = 0;
      var end = ItemClass.list.Length;

      for (var i = start; i <= end - 1; i++)
      {
        if (ItemClass.list[i] == null) continue;

        objects.Add(ItemClass.list[i]);
      }
    }

    private static void GetEntityGroups(List<object> objects)
    {
      objects.AddRange(EntityGroups.list.Keys.Select(k => new KeyValuePair<string, List<SEntityClassAndProb>>(k, EntityGroups.list[k])).Cast<object>());
    }

    private static void GetEntityClasses(List<object> objects)
    {
      objects.AddRange(EntityClass.list.Keys.Select(e => EntityClass.list[e]).Cast<object>());
    }

    private static void GetBuffs(List<object> objects)
    {
      objects.AddRange(MultiBuffClass.s_classes.Values.Cast<object>());
    }

    private static void GetBlocks(List<object> objects)
    {
      const int start = 0;
      const int end = 4097;

      for (var i = start; i <= end - 1; i++)
      {
        if (ItemClass.list[i] == null) continue;

        objects.Add(ItemClass.list[i]);
      }
    }

    private static void GetSpawning(List<object> objects)
    {
      objects.AddRange(BiomeSpawningClass.list.Keys.Select(b => new KeyValuePair<string, BiomeSpawnEntityGroupList>(b, BiomeSpawningClass.list[b])).Cast<object>());
    }

    private static void GetBiomes(List<object> objects)
    {
      objects.AddRange(GameManager.Instance.World.Biomes.GetBiomeMap().Select(b => b.Value).Cast<object>());
    }

    private static void GetArchetypes(List<object> objects)
    {
      objects.AddRange(Archetypes.Instance.GetArchetypeNames().Select(n => Archetypes.Instance.GetArchetype(n)).Cast<object>());
    }

    private static string OrderByCallback(Skill skill) => skill.IsPerk
      ? (skill.SkillRequirements.Count > 0 ? skill.SkillRequirements[0].SkillRequired : skill.IsPerk.ToString()) : skill.Name;
  }
}
