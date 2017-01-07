using System;
using System.Collections.Generic;

namespace BCM.Models
{
  [Serializable]
  public class FavoriteRecipeList : AbstractList
  {
    private List<string> favoriteRecipes = new List<string>();

    public FavoriteRecipeList(PlayerInfo _pInfo, Dictionary<string, string> _options) : base(_pInfo, _options)
    {
    }

    public override void Load(PlayerInfo _pInfo)
    {
      foreach (string fr in _pInfo.PDF.favoriteRecipeList)
      {
        favoriteRecipes.Add(fr);
      }
    }

    public override string Display(string sep = " ")
    {
      // todo: filter duplicate entrys for recipes with multiple versions, or display ingredients as a /details option?
      bool first = true;
      string output = "FavoriteRecipe:{";
      foreach (string fr in favoriteRecipes)
      {
        if (!first) { output += sep; } else { first = false; }
        output += fr + sep;
      }
      output += "}";

      return output;
    }
  }
}
