using System.Collections.Generic;

namespace BCM.Commands
{
  public class ListRecipes : BCCommandAbstract
  {
    public virtual Dictionary<string, string> jsonObject()
    {
      Dictionary<string, string> data = new Dictionary<string, string>();

      var i = 0;
      foreach (Recipe recipe in CraftingManager.GetAllRecipes())
      {
        Dictionary<string, string> details = new Dictionary<string, string>();

        details.Add("itemValueType", recipe.itemValueType.ToString());
        details.Add("count", recipe.count.ToString());
        details.Add("craftExpGain", recipe.craftExpGain.ToString());
        details.Add("craftingArea", recipe.craftingArea.ToString());
        details.Add("craftingTime", recipe.craftingTime.ToString());
        details.Add("craftingToolType", recipe.craftingToolType.ToString());
        details.Add("materialBasedRecipe", recipe.materialBasedRecipe.ToString());
        details.Add("scrapable", recipe.scrapable.ToString());
        details.Add("tooltip", (recipe.tooltip != null ? recipe.tooltip : ""));
        details.Add("wildcardCampfireCategory", recipe.wildcardCampfireCategory.ToString());
        details.Add("wildcardForgeCategory", recipe.wildcardForgeCategory.ToString());
        details.Add("CraftingSkillGroup", ItemClass.GetForId(recipe.itemValueType).CraftingSkillGroup);

        //recipe.unlockExpGain
        //recipe.unlockItemType

        List<string> ingredients = new List<string>();
        foreach (ItemStack itemStack in recipe.ingredients)
        {
          Dictionary<string, string> ingredient = new Dictionary<string, string>();
          ingredient.Add("itemValueType", itemStack.itemValue.type.ToString());
          ingredient.Add("count", itemStack.count.ToString());
          string jsonIngredient = BCUtils.toJson(ingredient);
          ingredients.Add(jsonIngredient);
        }
        string jsonIngredients = BCUtils.toJson(ingredients);
        details.Add("ingredients", jsonIngredients);

        var jsonDetails = BCUtils.toJson(details);
        data.Add(i.ToString(), jsonDetails);
        i++;
      }

      return data;
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
        Recipe[] recipes = CraftingManager.GetAllRecipes();
        foreach (Recipe recipe in recipes)
        {
          output += recipe.GetName();
          output += "(";
          output += "count=" + recipe.count;
          output += ",exp=" + recipe.craftExpGain;
          output += ",time=" + recipe.craftingTime;
          output += ",area=" + (recipe.craftingArea == "" ? "Backpack" : recipe.craftingArea);
          if (recipe.craftingToolType != 0)
          {
            ItemClass tic = ItemClass.list[recipe.craftingToolType];
            output += ",tool=" + tic.Name;
            if (_options.ContainsKey("itemids"))
            {
              int tivt = recipe.craftingToolType;
              if (tivt > 4096)
              {
                tivt = tivt - 4096;
              }
              output += "(" + tivt + ")";
            }
          }
          output += ")";
          output += "[";
          bool first2 = true;
          foreach (ItemStack i in recipe.GetIngredientsSummedUp())
          {
            int ivt = i.itemValue.type;
            if (ivt != 0)
            {
              ItemClass ic = ItemClass.list[ivt];
              if (ivt > 4096)
              {
                ivt = ivt - 4096;
              }
              if (!first2) { output += ","; } else { first2 = false; }
              output += ic.Name;
              if (_options.ContainsKey("itemids"))
              {
                output += "(" + ivt + ")";
              }
              output += "*" + i.count + "";
            }
          }
          output += "]";

          output += _sep;
        }
        SendOutput(output);
      }
    }
  }
}
