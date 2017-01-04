using System;
using System.Collections.Generic;

namespace BCM.Models
{
  [Serializable]
  public class UnlockedRecipeList
  {
    private List<string> unlockedRecipes = new List<string>();

    public UnlockedRecipeList()
    {
    }

    public UnlockedRecipeList(PlayerInfo _pInfo)
    {
      Load(_pInfo);
    }

    public void Load(PlayerInfo _pInfo)
    {
      foreach (string ur in _pInfo.PDF.unlockedRecipeList)
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
