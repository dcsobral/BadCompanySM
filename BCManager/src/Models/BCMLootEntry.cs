using System;

namespace BCM.Models
{
  public class BCMLootEntry
  {
    public int Item;
    public string Group;
    public double Prob;
    public string Template;
    public int Min;
    public int Max;
    public int MinQual;
    public int MaxQual;
    public double MinLevel;
    public double MaxLevel;

    public BCMLootEntry(LootContainer.LootEntry lootEntry)
    {
      if (lootEntry.item != null) Item = lootEntry.item.itemValue.type;
      if (lootEntry.group != null) Group = lootEntry.group.name;
      Prob = Math.Round(lootEntry.prob, 6);
      Template = lootEntry.lootProbTemplate;
      Min = lootEntry.minCount;
      Max = lootEntry.maxCount;
      MinQual = lootEntry.minQuality;
      MaxQual = lootEntry.maxQuality;
      MinLevel = Math.Round(lootEntry.minLevel, 6);
      MaxLevel = Math.Round(lootEntry.maxLevel, 6);
    }
  }
}
