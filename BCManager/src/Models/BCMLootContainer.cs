using System;
using System.Collections.Generic;
using UnityEngine;

namespace BCM.Models
{
  [Serializable]
  public class BCMLootContainer : BCMAbstract
  {
    #region Filters
    public static class StrFilters
    {
      public const string Id = "id";
      public const string Size = "size";
      public const string Min = "min";
      public const string Max = "max";
      public const string Quality = "quality";
      public const string OpenTime = "time";
      public const string SoundOpen = "soundopen";
      public const string SoundClose = "soundclose";
      public const string Destroy = "destroy";
      public const string Buffs = "buffs";
      public const string Items = "items";
    }

    private static Dictionary<int, string> _filterMap = new Dictionary<int, string>
    {
      { 0,  StrFilters.Id },
      { 1,  StrFilters.Size },
      { 2,  StrFilters.Min },
      { 3,  StrFilters.Max },
      { 4,  StrFilters.Quality },
      { 5,  StrFilters.OpenTime },
      { 6,  StrFilters.SoundOpen },
      { 7,  StrFilters.SoundClose },
      { 8,  StrFilters.Destroy },
      { 9,  StrFilters.Buffs },
      { 10,  StrFilters.Items }

    };
    public static Dictionary<int, string> FilterMap => _filterMap;

    #endregion

    #region Properties
    public int Id;
    public BCMVector2 Size;
    public int Min;
    public int Max;
    public string Quality;
    public double OpenTime;
    public string SoundOpen;
    public string SoundClose;
    public bool Destroy;
    public List<BCMBuffAction> Buffs = new List<BCMBuffAction>();
    public List<BCMLootEntry> Items = new List<BCMLootEntry>();

    public class BCMBuffAction
    {
      public string BuffId;
      public double Chance;
      public bool IsDebuff;

      public BCMBuffAction(MultiBuffClassAction buffAction)
      {
        BuffId = buffAction.Class.Id;
        Chance = Math.Round(buffAction.Chance, 6);
        IsDebuff = buffAction.IsDebuffAction;
      }
    }
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
      //public string parentGroup;

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
    public class BCMVector2
    {
      public int x;
      public int y;
      public BCMVector2()
      {
        x = 0;
        y = 0;
      }
      public BCMVector2(int x, int y)
      {
        this.x = x;
        this.y = y;
      }
      public BCMVector2(Vector2 v)
      {
        x = Mathf.RoundToInt(v.x);
        y = Mathf.RoundToInt(v.y);
      }
      public BCMVector2(Vector2i v)
      {
        x = v.x;
        y = v.y;
      }
    }
    #endregion;

    public BCMLootContainer(object obj, string typeStr, Dictionary<string, string> options, List<string> filters) : base(obj, typeStr, options, filters)
    {
    }

    public override void GetData(object obj)
    {
      var loot = obj as LootContainer;
      if (loot == null) return;

      if (IsOption("filter"))
      {
        foreach (var f in StrFilter)
        {
          switch (f)
          {
            case StrFilters.Id:
              GetId(loot);
              break;
            case StrFilters.Size:
              GetSize(loot);
              break;
            case StrFilters.Min:
              GetMin(loot);
              break;
            case StrFilters.Max:
              GetMax(loot);
              break;
            case StrFilters.Quality:
              GetQuality(loot);
              break;
            case StrFilters.OpenTime:
              GetOpenTime(loot);
              break;
            case StrFilters.SoundOpen:
              GetSoundOpen(loot);
              break;
            case StrFilters.SoundClose:
              GetSoundClose(loot);
              break;
            case StrFilters.Destroy:
              GetDestroy(loot);
              break;
            case StrFilters.Buffs:
              GetBuffs(loot);
              break;
            case StrFilters.Items:
              GetItems(loot);
              break;
            default:
              Log.Out($"{Config.ModPrefix} Unknown filter {f}");
              break;
          }
        }
      }
      else
      {
        GetId(loot);
        GetSize(loot);
        GetMin(loot);
        GetMax(loot);
        GetQuality(loot);
        GetOpenTime(loot);
        GetSoundOpen(loot);
        GetSoundClose(loot);
        GetDestroy(loot);
        GetBuffs(loot);
        GetItems(loot);
      }
    }

    private void GetItems(LootContainer loot)
    {
      if (loot.itemsToSpawn != null)
      {
        foreach (var lootEntry in loot.itemsToSpawn)
        {
          Items.Add(new BCMLootEntry(lootEntry));
        }
      }
      Bin.Add("Items", Items);
    }

    private void GetBuffs(LootContainer loot)
    {
      if (loot.BuffActions != null)
      {
        foreach (var buff in loot.BuffActions)
        {
          Buffs.Add(new BCMBuffAction(buff));
        }
      }
      Bin.Add("Buffs", Buffs);
    }

    private void GetDestroy(LootContainer loot)
    {
      Bin.Add("Destroy", Destroy = loot.bDestroyOnClose);
    }

    private void GetSoundClose(LootContainer loot)
    {
      Bin.Add("SoundClose", SoundClose = loot.soundClose);
    }

    private void GetSoundOpen(LootContainer loot)
    {
      Bin.Add("SoundOpen", SoundOpen = loot.soundOpen);
    }

    private void GetOpenTime(LootContainer loot)
    {
      Bin.Add("OpenTime", OpenTime = Math.Round(loot.openTime, 6));
    }

    private void GetQuality(LootContainer loot)
    {
      Bin.Add("Quality", Quality = loot.lootQualityTemplate);
    }

    private void GetMax(LootContainer loot)
    {
      Bin.Add("Max", Max = loot.maxCount);
    }

    private void GetMin(LootContainer loot)
    {
      Bin.Add("Min", Min = loot.minCount);
    }

    private void GetSize(LootContainer loot)
    {
      Bin.Add("Size", Size = new BCMVector2(loot.size));
    }

    private void GetId(LootContainer loot)
    {
      Bin.Add("Id", Id = loot.Id);
    }
  }
}
