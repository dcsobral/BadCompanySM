using System;
using System.Collections.Generic;

namespace BCM.Models
{
  [Serializable]
  public class CraftingQueue
  {
    private List<RecipeQueueItem> queueItems = new List<RecipeQueueItem>();

    public CraftingQueue()
    {
    }

    public CraftingQueue(PlayerInfo _pInfo)
    {
      Load(_pInfo);
    }

    public void Load(PlayerInfo _pInfo)
    {

      foreach (RecipeQueueItem rqi in _pInfo.PDF.craftingData.RecipeQueueItems)
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
        if (rqi.Recipe != null)
        {
          if (!first) { output += ",\n"; } else { first = false; }
          int it = rqi.Recipe.itemValueType;
          if (it > 4096)
          {
            it = it - 4096;
          }
          output += rqi.Recipe.GetName() + "(" + it + ")[*" + rqi.Multiplier + "]:CraftTime:";
          if (rqi.IsCrafting)
          {
            output += (rqi.Recipe.craftingTime * (rqi.Multiplier - 1) + rqi.CraftingTimeLeft).ToString("0.0") + "s - CurrentItem:" + rqi.CraftingTimeLeft.ToString("0.0") + "/";
          }
          else
          {
            output += (rqi.CraftingTimeLeft * rqi.Multiplier).ToString("0.0") + "s - PerItem:";
          }
          output += rqi.Recipe.craftingTime.ToString("0.0") + "s";

          output += " [";
          bool first2 = true;
          foreach (ItemStack i in rqi.Recipe.GetIngredientsSummedUp())
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
              output += ic.Name + "(" + ivt + ")*" + i.count + "";
            }
          }
          output += "]";
        }
      }
      output += "\n}\n";

      return output;
    }
  }
}
