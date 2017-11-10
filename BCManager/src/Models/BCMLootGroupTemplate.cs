using System;
using System.Collections.Generic;

namespace BCM.Models
{
  public class BCMLootGroupTemplate
  {
    public string Name;
    public int MinCount;
    public int MaxCount;
    public string Template;
    public int MinQual;
    public int MaxQual;
    public double MinLevel;
    public double MaxLevel;
    public List<BCMLootEntry> Items = new List<BCMLootEntry>();

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
