using System;
using System.Collections.Generic;
using System.Linq;

namespace BCM.Models
{
  [Serializable]
  public class BCMRecipe : BCMAbstract
  {
    #region Filters
    public static class StrFilters
    {
      public const string Type = "type";
      public const string Count = "count";
      public const string CraftExp = "exp";
      public const string CraftArea = "area";
      public const string CraftTime = "time";
      public const string CraftTool = "tool";
      public const string Tooltip = "tooltip";
      public const string IsMatBased = "matbased";
      public const string IsScrappable = "scrappable";
      public const string IsWildCamp = "wildcamp";
      public const string IsWildForge = "wildforge";
      public const string Skill = "skill";
      public const string Ingredients = "ingredients";
    }

    private static readonly Dictionary<int, string> _filterMap = new Dictionary<int, string>
    {
      { 0,  StrFilters.Type },
      { 1,  StrFilters.Count },
      { 2,  StrFilters.CraftExp },
      { 3,  StrFilters.CraftArea },
      { 4,  StrFilters.CraftTime },
      { 5,  StrFilters.CraftTool },
      { 6,  StrFilters.Tooltip },
      { 7,  StrFilters.IsMatBased },
      { 8,  StrFilters.IsScrappable },
      { 9,  StrFilters.IsWildCamp },
      { 10,  StrFilters.IsWildForge },
      { 11,  StrFilters.Skill },
      { 12,  StrFilters.Ingredients }
    };
    public static Dictionary<int, string> FilterMap => _filterMap;
    #endregion

    #region Properties
    public int Type;
    public int Count;
    public int CraftExp;
    public string CraftArea;
    public double CraftTime;
    public int CraftTool;
    public string Tooltip;
    public bool IsMatBased;
    public bool IsScrappable;
    public bool IsWildCamp;
    public bool IsWildForge;
    public string Skill;
    public class BCMIngredient
    {
      public int Type;
      public int Count;
    }
    public List<BCMIngredient> Ingredients = new List<BCMIngredient>();
    #endregion;

    public BCMRecipe(object obj, string typeStr, Dictionary<string, string> options, List<string> filters) : base(obj, typeStr, options, filters)
    {
    }

    public override void GetData(object obj)
    {
      if (!(obj is Recipe recipe)) return;

      if (IsOption("filter"))
      {
          foreach (var f in StrFilter)
          {
            switch (f)
            {
              case StrFilters.Type:
                GetType(recipe);
                break;
              case StrFilters.Count:
                GetCount(recipe);
                break;
              case StrFilters.CraftArea:
                GetCraftArea(recipe);
                break;
              case StrFilters.CraftExp:
                GetCraftExp(recipe);
                break;
              case StrFilters.CraftTime:
                GetCraftTime(recipe);
                break;
              case StrFilters.CraftTool:
                GetCraftTool(recipe);
                break;
              case StrFilters.IsMatBased:
                GetMatsBased(recipe);
                break;
              case StrFilters.IsScrappable:
                GetScrappable(recipe);
                break;
              case StrFilters.IsWildCamp:
                GetWildCamp(recipe);
                break;
              case StrFilters.IsWildForge:
                GetWildForge(recipe);
                break;
              case StrFilters.Skill:
                GetSkill(recipe);
                break;
              case StrFilters.Tooltip:
                GetTooltip(recipe);
                break;
              case StrFilters.Ingredients:
                GetIngredients(recipe);
                break;
              default:
                Log.Out($"{Config.ModPrefix} Unknown filter {f}");
                break;
            }
        }
      }
      else
      {
        GetType(recipe);
        GetCount(recipe);
        GetCraftArea(recipe);
        GetCraftExp(recipe);
        GetCraftTime(recipe);
        GetCraftTool(recipe);
        GetMatsBased(recipe);
        GetScrappable(recipe);
        GetWildCamp(recipe);
        GetWildForge(recipe);
        GetSkill(recipe);
        GetTooltip(recipe);
        GetIngredients(recipe);
      }
    }

    private void GetTooltip(Recipe recipe) => Bin.Add("Tooltip", Tooltip = recipe.tooltip);

    private void GetSkill(Recipe recipe) => Bin.Add("Skill", Skill = ItemClass.list[recipe.itemValueType]?.CraftingSkillGroup);

    private void GetWildForge(Recipe recipe) => Bin.Add("IsWildForge", IsWildForge = recipe.wildcardForgeCategory);

    private void GetWildCamp(Recipe recipe) => Bin.Add("IsWildCamp", IsWildCamp = recipe.wildcardCampfireCategory);

    private void GetScrappable(Recipe recipe) => Bin.Add("IsScrappable", IsScrappable = recipe.scrapable);

    private void GetMatsBased(Recipe recipe) => Bin.Add("IsMatBased", IsMatBased = recipe.materialBasedRecipe);

    private void GetCraftTool(Recipe recipe) => Bin.Add("CraftTool", CraftTool = recipe.craftingToolType);

    private void GetCraftTime(Recipe recipe) => Bin.Add("CraftTime", CraftTime = Math.Round(recipe.craftingTime, 2));

    private void GetCraftExp(Recipe recipe) => Bin.Add("CraftExp", CraftExp = recipe.craftExpGain);

    private void GetCraftArea(Recipe recipe) => Bin.Add("CraftArea", CraftArea = recipe.craftingArea);

    private void GetCount(Recipe recipe) => Bin.Add("Count", Count = recipe.count);

    private void GetType(Recipe recipe) => Bin.Add("Type", Type = recipe.itemValueType);

    private void GetIngredients(Recipe recipe)
    {
      foreach (var itemStack in recipe.ingredients)
      {
        Ingredients.Add(new BCMIngredient { Type = itemStack.itemValue.type, Count = itemStack.count });
      }
      if (Options.ContainsKey("min"))
      {
        Bin.Add("Ingredients", Ingredients.Select(ing => new[] { ing.Type, ing.Count }).Cast<object>().ToList());
      }
      else
      {
        Bin.Add("Ingredients", Ingredients);
      }
    }
  }
}
