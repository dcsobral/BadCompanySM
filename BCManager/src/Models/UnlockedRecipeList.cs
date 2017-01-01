using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BCM.Models
{
  [Serializable]
  public class UnlockedRecipeList
  {
    private List<string> unlockedRecipes = new List<string>();

    public UnlockedRecipeList()
    {
    }

    public UnlockedRecipeList(PlayerDataFile _pdf)
    {
      Load(_pdf);
    }

    public void Load(PlayerDataFile _pdf)
    {
      foreach (string ur in _pdf.unlockedRecipeList)
      {
        unlockedRecipes.Add(ur);
      }
    }

    public string Display()
    {
      // todo: filter duplicate entrys for recipes with multiple versions?
      bool first = true;
      string output = "UnlockedRecipe(saved)={\n";
      foreach (string ur in unlockedRecipes)
      {
        if (!first) { output += ",\n"; } else { first = false; }
        output += ur;
      }
      output += "\n}\n";

      return output;
    }
  }
}
