namespace BCM.Commands
{
  public class ListRecipes : BCCommandAbstract
  {
    public override void Process()
    {
      string output = "";
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
