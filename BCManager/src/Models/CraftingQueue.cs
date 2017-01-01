using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BCM.Models
{
  [Serializable]
  public class CraftingQueue
  {
    private List<RecipeQueueItem> queueItems = new List<RecipeQueueItem>();

    public CraftingQueue()
    {
    }

    public CraftingQueue(PlayerDataFile _pdf)
    {
      Load(_pdf);
    }

    public void Load(PlayerDataFile _pdf)
    {

      foreach (RecipeQueueItem rqi in _pdf.craftingData.RecipeQueueItems)
      {
        queueItems.Add(rqi);
      }
    }

    public string Display()
    {
      string output = "CraftingQueue={\n";
      bool first = true;
      foreach (RecipeQueueItem rqi in queueItems)
      {
        if (!first) { output += ",\n"; } else { first = false; }
        output += rqi.Recipe.GetName() + ":(" + rqi.Multiplier + ")CraftTime:";
        if (rqi.IsCrafting)
        {
          output += (rqi.Recipe.craftingTime * (rqi.Multiplier - 1) + rqi.CraftingTimeLeft).ToString("0.0") + "s/CurrentItem:" + rqi.CraftingTimeLeft.ToString("0.0") + "/";
        }
        else
        {
          output += (rqi.CraftingTimeLeft * rqi.Multiplier).ToString("0.0") + "s/PerItem:";
        }
        output += rqi.Recipe.craftingTime.ToString("0.0") + "s";
      }
      output += "\n}\n";

      return output;
    }
  }
}
