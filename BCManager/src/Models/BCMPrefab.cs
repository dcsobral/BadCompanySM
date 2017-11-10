using System;
using System.Collections.Generic;
using System.IO;

namespace BCM.Models
{
  public class BCMPrefabSleeperVolume
  {
    public bool Used;
    public BCMVector3 Start;
    public BCMVector3 Size;
    public string Group;
    public string Adjust;
    public bool IsLoot;
  }

  [Serializable]
  public class BCMPrefab : BCMAbstract
  {
    [NonSerialized]
    public static Dictionary<string, BCMPrefab> cache = new Dictionary<string, BCMPrefab>();

    #region Filters
    public static class StrFilters
    {
      public const string Name = "name";
      public const string Size = "size";
      public const string YOffset = "yoffset";
      public const string Rotation = "rotation";
      public const string AirBlocks = "airblocks";
      public const string AllowTopSoil = "topsoil";
      public const string HasMeshFile = "hasmesh";
      public const string ExcludePoiMesh = "poimesh";
      public const string DistYOffset = "distyoff";
      public const string DistOverride = "distover";
      public const string IsTrader = "trader";
      public const string TraderProtect = "protect";
      public const string TraderTpSize = "tpsize";
      public const string TraderTpCenter = "tpcenter";
      public const string Biomes = "biomes";
      public const string Townships = "townships";
      public const string Zoning = "zoning";
      public const string HasVolumes = "hasvols";
      public const string SleeperVolumes = "sleepers";
      public const string Blocks = "blocks";
      //public const string Stats = "stats";
    }

    private static readonly Dictionary<int, string> _filterMap = new Dictionary<int, string>
    {
      { 0,  StrFilters.Name },
      { 1,  StrFilters.Size },
      { 2,  StrFilters.YOffset },
      { 3,  StrFilters.Rotation },
      { 4,  StrFilters.AirBlocks },
      { 5,  StrFilters.AllowTopSoil },
      { 6,  StrFilters.HasMeshFile },
      { 7,  StrFilters.ExcludePoiMesh },
      { 8,  StrFilters.DistYOffset },
      { 9,  StrFilters.DistOverride },
      { 10,  StrFilters.IsTrader },
      { 11,  StrFilters.TraderProtect },
      { 12,  StrFilters.TraderTpSize },
      { 13,  StrFilters.TraderTpCenter },
      { 14,  StrFilters.Biomes },
      { 15,  StrFilters.Townships },
      { 16,  StrFilters.Zoning },
      { 17,  StrFilters.HasVolumes },
      { 18,  StrFilters.SleeperVolumes },
      { 19,  StrFilters.Blocks }
      //{ 20,  StrFilters.Stats }
    };
    public static Dictionary<int, string> FilterMap => _filterMap;
    #endregion

    #region Properties
    public string Name;
    public BCMVector3 Size;
    public int YOffset;
    public int Rotation;
    public bool AirBlocks;
    public bool AllowTopSoil;
    public bool HasMeshFile;
    public bool ExcludePoiMesh;
    public double DistYOffset;
    public string DistOverride;
    public bool IsTrader;
    public BCMVector3 TraderProtect;
    public BCMVector3 TraderTpSize;
    public BCMVector3 TraderTpCenter;
    public string[] Biomes;
    public string[] Townships;
    public string[] Zoning;
    public bool HasVolumes;
    public List<bool> VolumesUsed;
    public List<BCMVector3> VolumesStart;
    public List<BCMVector3> VolumesSize;
    public List<string> VolumesGroup;
    public List<string> VolumeGSAdjust;
    public List<bool> VolumeIsLoot;
    public List<BCMPrefabSleeperVolume> SleeperVolumes = new List<BCMPrefabSleeperVolume>();
    //public List<BCMBlockValue> Blocks = new List<BCMBlockValue>();
    //public string Stats;

    //public List<EntityCreationData> Entities;
    //public string Condition;
    //public string Age;
    #endregion;

    public BCMPrefab(object obj, string typeStr, Dictionary<string, string> options, List<string> filters) : base(obj, typeStr, options, filters)
    {
    }

    public override void GetData(object obj)
    {
      if (!(obj is string prefabname)) return;

      if (!Options.ContainsKey("filter") && !Options.ContainsKey("info") && 
        !Options.ContainsKey("blocks") && !Options.ContainsKey("full"))
      {
        Bin.Add("Name", Name = prefabname);

        return;
      }

      if (Options.ContainsKey("full") && cache.ContainsKey(prefabname))
      {
        Bin = cache[prefabname].Bin;

        return;
      }

      if (Options.ContainsKey("filter") && cache.ContainsKey(prefabname))
      {

        foreach (var f in StrFilter)
        {
          switch (f)
          {
            case StrFilters.Name:
              Bin.Add("Name", cache[prefabname].Name);
              break;
            case StrFilters.Size:
              Bin.Add("Size", cache[prefabname].Size);
              break;
            case StrFilters.YOffset:
              Bin.Add("YOffset", cache[prefabname].YOffset);
              break;
            case StrFilters.Rotation:
              Bin.Add("Rotation", cache[prefabname].Rotation);
              break;
            case StrFilters.AirBlocks:
              Bin.Add("AirBlocks", cache[prefabname].AirBlocks);
              break;
            case StrFilters.AllowTopSoil:
              Bin.Add("AllowTopSoil", cache[prefabname].AllowTopSoil);
              break;
            case StrFilters.HasMeshFile:
              Bin.Add("HasMeshFile", cache[prefabname].HasMeshFile);
              break;
            case StrFilters.ExcludePoiMesh:
              Bin.Add("ExcludePoiMesh", cache[prefabname].ExcludePoiMesh);
              break;
            case StrFilters.DistYOffset:
              Bin.Add("DistYOffset", cache[prefabname].DistYOffset);
              break;
            case StrFilters.DistOverride:
              Bin.Add("DistOverride", cache[prefabname].DistOverride);
              break;
            case StrFilters.IsTrader:
              Bin.Add("IsTrader", cache[prefabname].IsTrader);
              break;
            case StrFilters.TraderProtect:
              Bin.Add("TraderProtect", cache[prefabname].TraderProtect);
              break;
            case StrFilters.TraderTpSize:
              Bin.Add("TraderTpSize", cache[prefabname].TraderTpSize);
              break;
            case StrFilters.TraderTpCenter:
              Bin.Add("TraderTpCenter", cache[prefabname].TraderTpCenter);
              break;
            case StrFilters.Biomes:
              Bin.Add("Biomes", cache[prefabname].Biomes);
              break;
            case StrFilters.Townships:
              Bin.Add("Townships", cache[prefabname].Townships);
              break;
            case StrFilters.Zoning:
              Bin.Add("Zoning", cache[prefabname].Zoning);
              break;
            case StrFilters.HasVolumes:
              Bin.Add("HasVolumes", cache[prefabname].HasVolumes);
              break;
            case StrFilters.SleeperVolumes:
              Bin.Add("SleeperVolumes", cache[prefabname].SleeperVolumes);
              break;
            case StrFilters.Blocks:
//              Bin.Add("Blocks", cache[prefabname].Blocks);
              break;
            default:
              Log.Out($"{Config.ModPrefix} Unknown filter {f}");
              break;
          }
        }
        return;
      }

      var prefab = new Prefab();
      if (!prefab.Load(prefabname))
      {
        return;
      }

      if (IsOption("filter"))
      {
        foreach (var f in StrFilter)
        {
          switch (f)
          {
            case StrFilters.Name:
              GetName(prefab);
              break;
            case StrFilters.Size:
              GetSize(prefab);
              break;
            case StrFilters.YOffset:
              GetYOffset(prefab);
              break;
            case StrFilters.Rotation:
              GetRotation(prefab);
              break;
            case StrFilters.AirBlocks:
              GetAirBlocks(prefab);
              break;
            case StrFilters.AllowTopSoil:
              GetTopSoil(prefab);
              break;
            case StrFilters.HasMeshFile:
              GetHasMeshFile(prefab);
              break;
            case StrFilters.ExcludePoiMesh:
              GetExcludePoiMesh(prefab);
              break;
            case StrFilters.DistYOffset:
              GetDistYOffset(prefab);
              break;
            case StrFilters.DistOverride:
              GetDistOverride(prefab);
              break;
            case StrFilters.IsTrader:
              GetIsTrader(prefab);
              break;
            case StrFilters.TraderProtect:
              GetTraderProtect(prefab);
              break;
            case StrFilters.TraderTpSize:
              GetTraderTpSize(prefab);
              break;
            case StrFilters.TraderTpCenter:
              GetTraderTpCenter(prefab);
              break;
            case StrFilters.Biomes:
              GetBiomes(prefab);
              break;
            case StrFilters.Townships:
              GetTownships(prefab);
              break;
            case StrFilters.Zoning:
              GetZoning(prefab);
              break;
            case StrFilters.HasVolumes:
              GetHasVolumes(prefab);
              break;
            case StrFilters.SleeperVolumes:
              GetSleeperVolumes(prefab);
              break;
            case StrFilters.Blocks:
              //GetBlocks(prefab);
              break;
            //case StrFilters.Stats:
            //  GetStats(prefab);
            //  break;
            default:
              Log.Out($"{Config.ModPrefix} Unknown filter {f}");
              break;
          }
        }
      }
      else
      {
        GetName(prefab);

        if (!Options.ContainsKey("info") && !Options.ContainsKey("blocks") && !Options.ContainsKey("full")) return;

        if (Options.ContainsKey("info") || Options.ContainsKey("full"))
        {
          GetSize(prefab);
          GetYOffset(prefab);
          GetRotation(prefab);
        }

        if (Options.ContainsKey("full"))
        {
          GetAirBlocks(prefab);
          GetTopSoil(prefab);
          GetHasMeshFile(prefab);
          GetExcludePoiMesh(prefab);
          GetDistYOffset(prefab);
          GetDistOverride(prefab);
          GetIsTrader(prefab);
          GetTraderProtect(prefab);
          GetTraderTpSize(prefab);
          GetTraderTpCenter(prefab);
          GetBiomes(prefab);
          GetTownships(prefab);
          GetZoning(prefab);
          GetHasVolumes(prefab);
          GetSleeperVolumes(prefab);
        }

        if (Options.ContainsKey("blocks"))
        {
          //GetBlocks(prefab);
        }
        //GetStats(prefab);
      }
      if (Options.ContainsKey("full"))
      {
        cache.Add(prefabname, this);
      }
    }

    //private void GetStats(Prefab prefab) => Bin.Add("Stats", Stats = prefab.GetBlockStatistics().ToString());

    private void GetZoning(Prefab prefab) => Bin.Add("Zoning", Zoning = prefab.GetAllowedZones());

    private void GetTownships(Prefab prefab) => Bin.Add("Townships", Townships = prefab.GetAllowedTownships());

    private void GetBiomes(Prefab prefab) => Bin.Add("Biomes", Biomes = prefab.GetAllowedBiomes());

    private void GetSleeperVolumes(Prefab prefab)
    {
      var sleeperVolumes = new List<BCMPrefabSleeperVolume>();

      for (var x = 0; x <= prefab.SleeperVolumeUsed.Count - 1; x++)
      {
        sleeperVolumes.Add(new BCMPrefabSleeperVolume
        {
          Used = prefab.SleeperVolumeUsed[x],
          Start = new BCMVector3(prefab.SleeperVolumesStart[x]),
          Size = new BCMVector3(prefab.SleeperVolumesSize[x]),
          Group = prefab.SleeperVolumesGroup[x],
          Adjust = prefab.SleeperVolumeGameStageAdjust[x],
          IsLoot = prefab.SleeperIsLootVolume[x]
        });
      }

      Bin.Add("SleeperVolumes", SleeperVolumes = sleeperVolumes);
    }

    private void GetHasVolumes(Prefab prefab) => Bin.Add("HasVolumes", HasVolumes = prefab.bSleeperVolumes);

    private void GetTraderTpCenter(Prefab prefab) => Bin.Add("TraderTpCenter", TraderTpCenter = new BCMVector3(prefab.TraderAreaTeleportCenter));

    private void GetTraderTpSize(Prefab prefab) => Bin.Add("TraderTpSize", TraderTpSize = new BCMVector3(prefab.TraderAreaTeleportSize));

    private void GetTraderProtect(Prefab prefab) => Bin.Add("TraderProtect", TraderProtect = new BCMVector3(prefab.TraderAreaProtect));

    private void GetIsTrader(Prefab prefab) => Bin.Add("IsTrader", IsTrader = prefab.bTraderArea);

    private void GetDistOverride(Prefab prefab) => Bin.Add("DistOverride", DistOverride = prefab.distantPOIOverride);

    private void GetDistYOffset(Prefab prefab) => Bin.Add("DistYOffset", DistYOffset = Math.Round(prefab.distantPOIYOffset, 3));

    private void GetExcludePoiMesh(Prefab prefab) => Bin.Add("ExcludePoiMesh", ExcludePoiMesh = prefab.bExcludeDistantPOIMesh);

    private void GetHasMeshFile(Prefab prefab) => Bin.Add("HasMeshFile", HasMeshFile = File.Exists(Utils.GetGameDir("Data/Prefabs") + "/" + prefab.filename + ".mesh"));

    private void GetTopSoil(Prefab prefab) => Bin.Add("AllowTopSoil", AllowTopSoil = prefab.bAllowTopSoilDecorations);

    private void GetAirBlocks(Prefab prefab) => Bin.Add("AirBlocks", AirBlocks = prefab.bCopyAirBlocks);

    private void GetRotation(Prefab prefab) => Bin.Add("Rotation", Rotation = prefab.rotationToFaceNorth);

    private void GetYOffset(Prefab prefab) => Bin.Add("YOffset", YOffset = prefab.yOffset);

    private void GetSize(Prefab prefab) => Bin.Add("Size", Size = new BCMVector3(prefab.size));

    private void GetName(Prefab prefab) => Bin.Add("Name", Name = prefab.filename);
  }
}
