using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace BCM.Models
{
  [Serializable]
  public class BCMLootContainer : BCMAbstract
  {
    #region Filters
    private static class StrFilters
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

    private static readonly Dictionary<int, string> _filterMap = new Dictionary<int, string>
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
    [UsedImplicitly] public int Id;
    [UsedImplicitly] public BCMVector2 Size;
    [UsedImplicitly] public int Min;
    [UsedImplicitly] public int Max;
    [UsedImplicitly] public string Quality;
    [UsedImplicitly] public double OpenTime;
    [UsedImplicitly] public string SoundOpen;
    [UsedImplicitly] public string SoundClose;
    [UsedImplicitly] public bool Destroy;
    [NotNull] [UsedImplicitly] public List<BCMLootBuffAction> Buffs = new List<BCMLootBuffAction>();
    [NotNull] [UsedImplicitly] public List<BCMLootEntry> Items = new List<BCMLootEntry>();
    #endregion;

    public BCMLootContainer(object obj, string typeStr, Dictionary<string, string> options, List<string> filters) : base(obj, typeStr, options, filters)
    {
    }

    protected override void GetData(object obj)
    {
      if (!(obj is LootContainer loot)) return;

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
          Buffs.Add(new BCMLootBuffAction(buff));
        }
      }
      Bin.Add("Buffs", Buffs);
    }

    private void GetDestroy(LootContainer loot) => Bin.Add("Destroy", Destroy = loot.bDestroyOnClose);

    private void GetSoundClose(LootContainer loot) => Bin.Add("SoundClose", SoundClose = loot.soundClose);

    private void GetSoundOpen(LootContainer loot) => Bin.Add("SoundOpen", SoundOpen = loot.soundOpen);

    private void GetOpenTime(LootContainer loot) => Bin.Add("OpenTime", OpenTime = Math.Round(loot.openTime, 6));

    private void GetQuality(LootContainer loot) => Bin.Add("Quality", Quality = loot.lootQualityTemplate);

    private void GetMax(LootContainer loot) => Bin.Add("Max", Max = loot.maxCount);

    private void GetMin(LootContainer loot) => Bin.Add("Min", Min = loot.minCount);

    private void GetSize(LootContainer loot) => Bin.Add("Size", Size = new BCMVector2(loot.size));

    private void GetId(LootContainer loot) => Bin.Add("Id", Id = loot.Id);
  }
}
