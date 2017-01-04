using System;
using System.Collections.Generic;

namespace BCM.Models
{
  [Serializable]
  public class FavoriteRecipeList
  {
    private List<string> favoriteRecipes = new List<string>();

    public FavoriteRecipeList()
    {
    }

    public FavoriteRecipeList(PlayerInfo _pInfo)
    {
      Load(_pInfo);
    }

    public void Load(PlayerInfo _pInfo)
    {
      foreach (string fr in _pInfo.PDF.favoriteRecipeList)
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
