using System;
using System.Collections.Generic;

namespace BCM.Models
{
  [Serializable]
  public class BCMGameObject : BCMAbstract
  {
    public static class GOTypes
    {
      public const string Players = "players";
      public const string Entities = "entities";
      public const string Archetypes = "archetypes";
      public const string Biomes = "biomes";
      public const string BiomeSpawning = "biomespawning";
      public const string Blocks = "blocks";
      public const string Buffs = "buffs";
      public const string EntityClasses = "entityclasses";
      public const string EntityGroups = "entitygroups";
      public const string ItemClasses = "itemclasses";
      public const string Items = "items";
      //public const string Loot = "loot";
      public const string LootContainers = "lootco";
      public const string LootGroups = "lootgr";
      //public const string LootPlaceholders = "lootpl";
      public const string LootQualityTemplates = "lootqt";
      public const string LootProbabilityTemplates = "lootpt";
      public const string Materials = "materials";
      public const string Prefabs = "prefabs";
      public const string Quests = "quests";
      public const string Recipes = "recipes";
      public const string Rwg = "rwg";
      public const string Skills = "skills";
      public const string Spawners = "spawners";
    }

    public BCMGameObject(object obj, string typeStr, Dictionary<string, string> options, List<string> filters) : base(obj, typeStr, options, filters)
    {
    }

    public override void GetData(object obj)
    {
      switch (TypeStr)
      {
        case GOTypes.Archetypes:
          GetArchetypes(obj as Archetype);
          break;
        case GOTypes.Biomes:
          GetBiomes(obj as BiomeDefinition);
          break;
        case GOTypes.BiomeSpawning:
          GetSpawning((KeyValuePair<string, BiomeSpawnEntityGroupList>)obj);
          break;
        case GOTypes.Blocks:
          GetItemClasses(obj as ItemClass);
          break;
        case GOTypes.Buffs:
          GetBuffs(obj as MultiBuffClass);
          break;
        case GOTypes.EntityClasses:
          GetEntityClasses(obj as EntityClass);
          break;
        case GOTypes.EntityGroups:
          GetEntityGroups((KeyValuePair<string, List<SEntityClassAndProb>>)obj);
          break;
        case GOTypes.ItemClasses:
          GetItemClasses(obj as ItemClass);
          break;
        case GOTypes.Items:
          GetItemClasses(obj as ItemClass);
          break;
        //case GOTypes.Loot:
        //  GetLoot(obj as Archetype);
        //  break;
        case GOTypes.LootContainers:
          GetLootContainers(obj as LootContainer);
          break;
        case GOTypes.LootGroups:
          GetLootGroups(obj as LootContainer.LootGroup);
          break;
        case GOTypes.LootProbabilityTemplates:
          GetLootProbabilityTemplates(obj as LootContainer.LootProbabilityTemplate);
          break;
        case GOTypes.LootQualityTemplates:
          GetLootQualityTemplates(obj as LootContainer.LootQualityTemplate);
          break;
        case GOTypes.Materials:
          GetMaterials(obj as MaterialBlock);
          break;
        case GOTypes.Prefabs:
          GetPrefabs(obj as string);
          break;
        case GOTypes.Quests:
          GetQuests(obj as QuestClass);
          break;
        case GOTypes.Recipes:
          GetRecipes(obj as Recipe);
          break;
        case GOTypes.Rwg:
          GetRwg(obj as Dictionary<string, object>);
          break;
        case GOTypes.Skills:
          GetSkills(obj as Skill);
          break;
        case GOTypes.Spawners:
          GetSpawners((KeyValuePair<string, EntitySpawnerClassForDay>)obj);
          break;
        default:
          break;
      }
    }

    private void GetArchetypes(Archetype obj) => Bin = new BCMArchetype(obj, TypeStr, Options, StrFilter).Data();

    private void GetBiomes(BiomeDefinition obj) => Bin = new BCMBiome(obj, TypeStr, Options, StrFilter).Data();

    private void GetSpawning(KeyValuePair<string, BiomeSpawnEntityGroupList> obj) => Bin = new BCMBiomeSpawn(obj, TypeStr, Options, StrFilter).Data();

    private void GetBuffs(MultiBuffClass obj) => Bin = new BCMBuff(obj, TypeStr, Options, StrFilter).Data();

    private void GetEntityClasses(EntityClass obj) => Bin = new BCMEntityClass(obj, TypeStr, Options, StrFilter).Data();

    private void GetEntityGroups(KeyValuePair<string, List<SEntityClassAndProb>> obj) => Bin = new BCMEntityGroup(obj, TypeStr, Options, StrFilter).Data();

    private void GetItemClasses(ItemClass obj) => Bin = new BCMItemClass(obj, TypeStr, Options, StrFilter).Data();

    //private void GetLoot(object obj) => Bin = new BCMLoot(obj, TypeStr, Options).Data();

    private void GetLootContainers(LootContainer obj) => Bin = new BCMLootContainer(obj, TypeStr, Options, StrFilter).Data();

    private void GetLootGroups(LootContainer.LootGroup obj) => Bin = new BCMLootGroup(obj, TypeStr, Options, StrFilter).Data();

    private void GetLootProbabilityTemplates(LootContainer.LootProbabilityTemplate obj) => Bin = new BCMLootProbabilityTemplate(obj, TypeStr, Options, StrFilter).Data();

    private void GetLootQualityTemplates(LootContainer.LootQualityTemplate obj) => Bin = new BCMLootQualityTemplate(obj, TypeStr, Options, StrFilter).Data();

    private void GetMaterials(MaterialBlock obj) => Bin = new BCMMaterial(obj, TypeStr, Options, StrFilter).Data();

    private void GetPrefabs(string obj) => Bin = new BCMPrefab(obj, TypeStr, Options, StrFilter).Data();

    private void GetQuests(QuestClass obj) => Bin = new BCMQuest(obj, TypeStr, Options, StrFilter).Data();

    private void GetRecipes(Recipe obj) => Bin = new BCMRecipe(obj, TypeStr, Options, StrFilter).Data();

    private void GetRwg(Dictionary<string, object> obj) => Bin = new BCMRWG(obj, TypeStr, Options, StrFilter).Data();

    private void GetSkills(Skill obj) => Bin = new BCMSkill(obj, TypeStr, Options, StrFilter).Data();

    //todo: aggregate for day
    private void GetSpawners(KeyValuePair<string, EntitySpawnerClassForDay> obj) => Bin = new BCMSpawner(obj, TypeStr, Options, StrFilter).Data();
  }
}
