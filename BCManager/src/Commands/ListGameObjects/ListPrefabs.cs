using RWG2.Rules;
using System.Collections.Generic;
using System.IO;

namespace BCM.Commands
{
  public class ListPrefabs : BCCommandAbstract
  {
    public virtual Dictionary<string, string> jsonObject()
    {
      Dictionary<string, string> data = new Dictionary<string, string>();

      string prefabsGameDir = Utils.GetGameDir("Data/Prefabs");
      List<string> prefabs = GetStoredPrefabs(prefabsGameDir);

      var i = 0;
      foreach (string _name in prefabs)
      {
        Prefab prefab = new Prefab();
        if (File.Exists(prefabsGameDir + "/" + _name + ".tts"))
        {
          if (prefab.Load(_name))
          {
            Dictionary<string, string> details = new Dictionary<string, string>();
            details.Add("filename", (prefab.filename != null ? prefab.filename : ""));
            details.Add("bAllowTopSoilDecorations", prefab.bAllowTopSoilDecorations.ToString());
            details.Add("bCopyAirBlocks", prefab.bCopyAirBlocks.ToString());
            details.Add("bExcludeDistantPOIMesh", prefab.bExcludeDistantPOIMesh.ToString());
            details.Add("bSleeperVolumes", prefab.bSleeperVolumes.ToString());
            details.Add("bTraderArea", prefab.bTraderArea.ToString());
            details.Add("distantPOIOverride", (prefab.distantPOIOverride != null ? prefab.distantPOIOverride : ""));
            details.Add("distantPOIYOffset", prefab.distantPOIYOffset.ToString());
            details.Add("rotationToFaceNorth", prefab.rotationToFaceNorth.ToString());
            details.Add("size", (prefab.size != null ? prefab.size.x.ToString() + "-" + prefab.size.y.ToString() : ""));
            details.Add("StaticSpawnerClass", (prefab.StaticSpawnerClass != null ? prefab.StaticSpawnerClass : ""));
            details.Add("StaticSpawnerCreated", prefab.StaticSpawnerCreated.ToString());
            details.Add("StaticSpawnerSize", (prefab.StaticSpawnerSize != null ? prefab.StaticSpawnerSize.x.ToString() + "-" + prefab.StaticSpawnerSize.y.ToString() + "-" + prefab.StaticSpawnerSize.z.ToString() : ""));
            details.Add("StaticSpawnerTrigger", prefab.StaticSpawnerTrigger.ToString());
            details.Add("TraderAreaProtect", (prefab.TraderAreaProtect != null ? prefab.TraderAreaProtect.x.ToString() + "-" + prefab.TraderAreaProtect.y.ToString() + "-" + prefab.TraderAreaProtect.z.ToString() : ""));
            details.Add("TraderAreaTeleportCenter", (prefab.TraderAreaTeleportCenter != null ? prefab.TraderAreaTeleportCenter.x.ToString() + "-" + prefab.TraderAreaTeleportCenter.y.ToString() + "-" + prefab.TraderAreaTeleportCenter.z.ToString() : ""));
            details.Add("TraderAreaTeleportSize", (prefab.TraderAreaTeleportSize != null ? prefab.TraderAreaTeleportSize.x.ToString() + "-" + prefab.TraderAreaTeleportSize.y.ToString() + "-" + prefab.TraderAreaTeleportSize.z.ToString() : ""));
            details.Add("Transient_NumSleeperSpawns", prefab.Transient_NumSleeperSpawns.ToString());
            details.Add("yOffset", prefab.yOffset.ToString());

            Dictionary<string, Dictionary<string,string>> _sleeperVolumes = new Dictionary<string, Dictionary<string, string>>();
            Dictionary<string, string> _isLoot = new Dictionary<string, string>();
            List<bool> SleeperIsLootVolume = prefab.SleeperIsLootVolume;
            var h = 0;
            foreach(var o in SleeperIsLootVolume)
            {
              _isLoot.Add(h.ToString(), o.ToString());
              h++;
            }
            Dictionary<string, string> _gameStageAdjust = new Dictionary<string, string>();
            List<string> SleeperVolumeGameStageAdjust = prefab.SleeperVolumeGameStageAdjust;
            h = 0;
            foreach (var o in SleeperVolumeGameStageAdjust)
            {
              _gameStageAdjust.Add(h.ToString(), o);
              h++;
            }
            Dictionary<string, string> _group = new Dictionary<string, string>();
            List<string> SleeperVolumesGroup = prefab.SleeperVolumesGroup;
            h = 0;
            foreach (var o in SleeperVolumesGroup)
            {
              _group.Add(h.ToString(), o);
              h++;
            }
            Dictionary<string, string> _size = new Dictionary<string, string>();
            List<Vector3i> SleeperVolumesSize = prefab.SleeperVolumesSize;
            h = 0;
            foreach (var o in SleeperVolumesSize)
            {
              _size.Add(h.ToString(), o.x.ToString() + "," + o.y.ToString() + "," + o.z.ToString());
              h++;
            }
            Dictionary<string, string> _start = new Dictionary<string, string>();
            List<Vector3i> SleeperVolumesStart = prefab.SleeperVolumesStart;
            h = 0;
            foreach (var o in SleeperVolumesStart)
            {
              _start.Add(h.ToString(), o.x.ToString() + "," + o.y.ToString() + "," + o.z.ToString());
              h++;
            }
            Dictionary<string, string> _used = new Dictionary<string, string>();
            List<bool> SleeperVolumeUsed = prefab.SleeperVolumeUsed;
            h = 0;
            foreach (var o in SleeperVolumeUsed)
            {
              _used.Add(h.ToString(), o.ToString());
              h++;
            }
            for (var j = 0; j < _group.Count; j++)
            {
              Dictionary<string, string> volume = new Dictionary<string, string>();
              string group = null;
              if (_group.TryGetValue(j.ToString(), out group))
              {
                volume.Add("group", group);
              }
              string gameStageAdjust = null;
              if (_gameStageAdjust.TryGetValue(j.ToString(), out gameStageAdjust))
              {
                volume.Add("gameStageAdjust", gameStageAdjust);
              }
              string isLoot = null;
              if (_isLoot.TryGetValue(j.ToString(), out isLoot))
              {
                volume.Add("isLoot", isLoot);
              }
              string size = null;
              if (_size.TryGetValue(j.ToString(), out size))
              {
                volume.Add("size", size);
              }
              string start = null;
              if (_start.TryGetValue(j.ToString(), out start))
              {
                volume.Add("start", start);
              }
              string used = null;
              if (_used.TryGetValue(j.ToString(), out used))
              {
                volume.Add("used", used);
              }

              _sleeperVolumes.Add(j.ToString(), volume);
            }
            details.Add("SleeperVolumes", BCUtils.toJson(_sleeperVolumes));

            string[] AllowedBiomes = prefab.GetAllowedBiomes();
            List<string> allowedBiomes = new List<string>();
            foreach (var biome in AllowedBiomes)
            {
              allowedBiomes.Add(biome);
            }
            details.Add("allowedBiomes", BCUtils.toJson(allowedBiomes));

            string[] AllowedTownships = prefab.GetAllowedTownships();
            List<string> allowedTownships = new List<string>();
            foreach (var township in AllowedTownships)
            {
              allowedTownships.Add(township);
            }
            details.Add("allowedTownships", BCUtils.toJson(allowedTownships));

            string[] AllowedZones = prefab.GetAllowedZones();
            List<string> allowedZones = new List<string>();
            foreach (var zones in AllowedZones)
            {
              allowedZones.Add(zones);
            }
            details.Add("allowedZones", BCUtils.toJson(allowedZones));

            var BlockStatistics = prefab.GetBlockStatistics();
            details.Add("cntWindows", BlockStatistics.cntWindows.ToString());
            details.Add("cntDoors", BlockStatistics.cntDoors.ToString());
            details.Add("cntBlockEntities", BlockStatistics.cntBlockEntities.ToString());
            details.Add("cntBlockModels", BlockStatistics.cntBlockModels.ToString());
            details.Add("cntSolid", BlockStatistics.cntSolid.ToString());

            List<EntityCreationData> Entities = prefab.GetEntities();
            List<string> _entities = new List<string>();
            foreach (var entity in Entities)
            {
              Dictionary<string, string> _entity = new Dictionary<string, string>();
              _entity.Add("blockPos", entity.blockPos.x.ToString() + "-" + entity.blockPos.y.ToString() + "-" + entity.blockPos.z.ToString());
              _entity.Add("pos", entity.pos.x.ToString() + " - " + entity.pos.y.ToString() + "-" + entity.pos.z.ToString());
              _entity.Add("lootListIndex", entity.lootContainer != null ? entity.lootContainer.lootListIndex.ToString() : "");

              _entity.Add("entityName", (entity.entityName != null ? entity.entityName : ""));
              _entity.Add("entityClassId", entity.entityClass.ToString());
              _entity.Add("blockValue", entity.blockValue.type.ToString());

              //ItemStack[] items = entity.lootContainer.items;
              _entities.Add(BCUtils.toJson(_entity));
            }
            details.Add("Entities", BCUtils.toJson(_entities));

            //List<string> _blocks = new List<string>();
            //for (var x = 0; x < prefab.size.x; x++)
            //{
            //  for (var y = 0; y < prefab.size.y; y++)
            //  {
            //    for (var z = 0; z < prefab.size.z; z++)
            //    {
            //      Dictionary<string, string> _block = new Dictionary<string, string>();
            //      BlockValue Block = prefab.GetBlock(x, y, z);
            //      //_block.Add("pos", x.ToString() + "-" + y.ToString() + "-" + z.ToString());
            //      //_block.Add("type", Block.type.ToString());
            //      //_block.Add("rotation", Block.rotation.ToString());
            //      //_block.Add("damage", Block.damage.ToString());
            //      //_block.Add("decalface", Block.decalface.ToString());
            //      //_block.Add("decaltex", Block.decaltex.ToString());
            //      //_block.Add("texture", prefab.GetTexture(x, y, z).ToString());
            //      //_block.Add("density", prefab.GetDensity(x, y, z).ToString());

            //      _block.Add("p", x.ToString() + "-" + y.ToString() + "-" + z.ToString());
            //      _block.Add("t", Block.type.ToString());
            //      _block.Add("r", Block.rotation.ToString());
            //      _block.Add("dm", Block.damage.ToString());
            //      _block.Add("df", Block.decalface.ToString());
            //      _block.Add("dt", Block.decaltex.ToString());
            //      _block.Add("tx", prefab.GetTexture(x, y, z).ToString());
            //      _block.Add("dn", prefab.GetDensity(x, y, z).ToString());
            //      _blocks.Add(BCUtils.toJson(_block));
            //    }
            //  }
            //}
            //details.Add("Blocks", BCUtils.toJson(_blocks));

            data.Add(i.ToString(), BCUtils.toJson(details));
            i++;
          }
        }
      }
      return data;
    }

    public override void Process()
    {
      string output = "";
      if (_options.ContainsKey("json"))
      {
        if (_options.ContainsKey("tag"))
        {
          if (_options["tag"] == null)
          {
            _options["tag"] = "bc-prefabs";
          }

          SendOutput("{\"tag\":\"" + _options["tag"] + "\",\"data\":" + BCUtils.toJson(jsonObject()) + "}");
        }
        else
        {
          SendOutput(BCUtils.toJson(jsonObject()));
        }
      }
      else
      {
        string prefabsGameDir = Utils.GetGameDir("Data/Prefabs");
        List<string> prefabs = GetStoredPrefabs(prefabsGameDir);

        if (_params.Count == 1)
        {
          Prefab prefab = new Prefab();
          if (File.Exists(prefabsGameDir + "/" + _params[0] + ".tts"))
          {
            if (prefab.Load(_params[0]))
            {
              output += _params[0] + "[size=" + prefab.size + ",yoffset=" + prefab.yOffset + "]" + _sep;
              SortedDictionary<int, int> blockstat = new SortedDictionary<int, int>();
              for (int i = 0; i < prefab.size.x; i++)
              {
                for (int j = 0; j < prefab.size.z; j++)
                {
                  for (int k = 0; k < prefab.size.y; k++)
                  {
                    BlockValue block = prefab.GetBlock(i, k, j);
                    if (blockstat.ContainsKey(block.type))
                    {
                      blockstat[block.type]++;
                    }
                    else
                    {
                      blockstat[block.type] = 1;
                    }
                  }
                }
              }

              foreach (int blockid in blockstat.Keys)
              {
                if (blockid == 0)
                {
                  output += "air(" + blockid + "):" + blockstat[blockid] + _sep;
                  continue;
                }
                try
                {
                  ItemClass ic = ItemClass.list[blockid];
                  output += ic.Name + "(" + blockid + "):" + blockstat[blockid] + _sep;
                }
                catch
                {
                  output += "NULL(" + blockid + "):" + blockstat[blockid] + _sep;
                }
              }
            }
          }
          else
          {
            // todo: return results for partial matches to the prefabname string entered for param0
            output += "Prefab not found";
          }

        }
        else
        {
          foreach (string prefabname in prefabs)
          {
            output += prefabname + _sep;
          }
        }
        SendOutput(output);
      }

    }
    public static List<string> GetStoredPrefabs(string prefabsGameDir)
    {
      string[] files = Directory.GetFiles(prefabsGameDir);
      List<string> prefabs = new List<string>();

      for (int i = files.Length - 1; i >= 0; i--)
      {
        string file = files[i];
        string ext = Path.GetExtension(file);
        if (ext == ".tts")
        {
          int start = prefabsGameDir.Length + 1;
          int len = file.Length - start - 4;
          if (start + len <= file.Length)
          {
            prefabs.Add(file.Substring(start, len));
          }
        }
      }
      return prefabs;
    }

  }
}
