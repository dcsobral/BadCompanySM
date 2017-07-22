using System;
using System.Collections.Generic;

namespace BCM.Models.Legacy
{
  [Serializable]
  public class CraftingQueue : AbstractList
  {
    private List<RecipeQueueItem> queueItems = new List<RecipeQueueItem>();

    public CraftingQueue(PlayerInfo _pInfo, Dictionary<string, string> _options) : base(_pInfo, _options)
    {
    }

    public override void Load(PlayerInfo _pInfo)
    {
      foreach (RecipeQueueItem rqi in _pInfo.PDF.craftingData.RecipeQueueItems)
      {
        queueItems.Add(rqi);
      }
    }

    public override string Display(string sep = " ")
    {
      string output = "CraftingQueue:{";
      bool first = true;
      foreach (RecipeQueueItem rqi in queueItems)
      {
        if (rqi.Recipe != null)
        {
          if (!first) { output += sep; } else { first = false; }
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
      output += "}";

      return output;
    }
    public Dictionary<string, string> GetQueueItems()
    {
      Dictionary<string, string> qi = new Dictionary<string, string>();
      int idx = 0;
      foreach (RecipeQueueItem queue in queueItems)
      {
        if (queue != null)
        {
          string str = null;
          str += "{";
          str += "\"Multiplier\":\"" + queue.Multiplier.ToString() + "\",";
          str += "\"CraftingTimeLeft\":\"" + queue.CraftingTimeLeft.ToString("F2") + "\"";
          if (queue.Recipe != null)
          {
            str += ",\"Name\":\"" + queue.Recipe.GetName() + "\",";
            str += "\"itemValue\":\"" + queue.Recipe.itemValueType.ToString() + "\",";
            // todo: ingredients
            if (queue.IsCrafting)
            {
              str += ",\"craftingTime\":\"" + queue.Recipe.craftingTime.ToString("F2") + "\"";
            }
          }
          str += "}";
          qi.Add(idx.ToString(), str);
        }
        idx++;
      }
      return qi;
    }
  }
}
