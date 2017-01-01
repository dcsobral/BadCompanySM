using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BCM.Models
{
  [Serializable]
  public class FavoriteRecipeList
  {
    private List<string> favoriteRecipes = new List<string>();

    public FavoriteRecipeList()
    {
    }

    public FavoriteRecipeList(PlayerDataFile _pdf)
    {
      Load(_pdf);
    }

    public void Load(PlayerDataFile _pdf)
    {
      foreach (string fr in _pdf.favoriteRecipeList)
      {
        favoriteRecipes.Add(fr);
      }
    }

    public string Display()
    {
      // todo: filter duplicate entrys for recipes with multiple versions?
      bool first = true;
      string output = "FavoriteRecipe(saved)={\n";
      foreach (string fr in favoriteRecipes)
      {
        if (!first) { output += ",\n"; } else { first = false; }
        output += fr;
      }
      output += "\n}\n";

      return output;
    }
  }
}
