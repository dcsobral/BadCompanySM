using System;
using System.Collections.Generic;

namespace BCM.Models
{
  [Serializable]
  public class UnlockedRecipeList : AbstractList
  {
    private List<string> unlockedRecipes = new List<string>();

    public UnlockedRecipeList(PlayerInfo _pInfo, Dictionary<string, string> _options) : base(_pInfo, _options)
    {
    }

    public override void Load(PlayerInfo _pInfo)
    {
      foreach (string ur in _pInfo.PDF.unlockedRecipeList)
      {
        unlockedRecipes.Add(ur);
      }
    }

    public override string Display(string sep = " ")
    {
      bool first = true;
      string output = "UnlockedRecipe:{";
      foreach (string ur in unlockedRecipes)
      {
        if (!first) { output += sep; } else { first = false; }
        output += ur;
      }
      output += "}" + sep;

      return output;
    }
  }
}
