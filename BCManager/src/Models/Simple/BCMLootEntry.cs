using System;
using JetBrains.Annotations;

namespace BCM.Models
{
  public class BCMLootEntry
  {
    [UsedImplicitly] public int Item;
    [UsedImplicitly] public string Group;
    [UsedImplicitly] public double Prob;
    [UsedImplicitly] public string Template;
    [UsedImplicitly] public int Min;
    [UsedImplicitly] public int Max;
    [UsedImplicitly] public int MinQual;
    [UsedImplicitly] public int MaxQual;
    [UsedImplicitly] public double MinLevel;
    [UsedImplicitly] public double MaxLevel;

    public BCMLootEntry([NotNull] LootContainer.LootEntry lootEntry)
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
