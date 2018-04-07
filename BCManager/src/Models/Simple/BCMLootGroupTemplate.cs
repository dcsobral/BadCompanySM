using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace BCM.Models
{
  public class BCMLootGroupTemplate
  {
    [UsedImplicitly] public string Name;
    [UsedImplicitly] public int MinCount;
    [UsedImplicitly] public int MaxCount;
    [UsedImplicitly] public string Template;
    [UsedImplicitly] public int MinQual;
    [UsedImplicitly] public int MaxQual;
    [UsedImplicitly] public double MinLevel;
    [UsedImplicitly] public double MaxLevel;
    [NotNull] [UsedImplicitly] public List<BCMLootEntry> Items = new List<BCMLootEntry>();

    public BCMLootGroupTemplate(LootContainer.LootGroup lootGroup)
    {
      Name = lootGroup.name;
      MinCount = lootGroup.minCount;
      MaxCount = lootGroup.maxCount;
      Template = lootGroup.lootQualityTemplate;
      MinQual = lootGroup.minQuality;
      MaxQual = lootGroup.maxQuality;
      MinLevel = Math.Round(lootGroup.minLevel, 6);
      MaxLevel = Math.Round(lootGroup.maxLevel, 6);

      foreach (var item in lootGroup.items)
      {
        Items.Add(new BCMLootEntry(item));
      }
    }
  }
}
