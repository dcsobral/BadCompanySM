using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BCM.Models;
using JetBrains.Annotations;

namespace BCM.Commands
{
  [UsedImplicitly]
  public class BCPrefab : BCCommandAbstract
  {
    //todo: add a layer filter for block stat commands so that a slice can be loaded with /y=2 (all blocks on layer y=2) or /z=2,5 (all blocks with z between 2 and 5 inclusive)
    //      could use /x=1,2 /y=0 /z=1,2 to get blocks {"1,0,1", "1,0,2", "2,0,1", "2,0,2"]

    //todo: finish work in progress sub commands

    private static readonly char _s = Path.DirectorySeparatorChar;
    private static readonly string prefabDir = Utils.GetGameDir($"Data{_s}Prefabs{_s}");

    protected override void Process()
    {
      if (!BCUtils.CheckWorld()) return;

      if (Params.Count < 2 && (Params.Count == 0 || Params[0] != "list"))
      {
        SendOutput(GetHelp());

        return;
      }

      switch (Params[0])
      {
        case "list":
          ListPrefabs();
          break;
        case "create":
          CreatePrefab();
          break;
        case "clone":
          ClonePrefab();
          break;
        case "backup":
          BackupPrefab();
          break;
        case "restore":
          RestorePrefab();
          break;
        case "delete":
          DeletePrefab();
          break;
        case "setprop":
          SetProperty();
          break;

        case "insert"://todo
          InsertLayers();
          break;
        case "cut"://todo
          CutLayers();
          break;
        case "add":
          AddLayers();
          break;
        case "trim":
          TrimLayers();
          break;
        case "setsize":
          SetSize();
          break;

        case "swap":
          SwapBlocks();
          break;
        case "swapmap"://todo
          SwapMap();
          break;
        case "copylayer"://todo
          CopyLayer();
          break;
        case "swaplayer"://todo
          SwapLayer();
          break;
        case "setblock"://todo
          SetBlock();
          break;
        case "repair"://todo
          RepairBlocks();
          break;
        case "lock"://todo
          LockSecureBlocks();
          break;

        case "dim":
          GetDimensions();
          break;
        case "blockcount":
          GetBlockCounts();
          break;
        case "blocks":
          GetBlocks();
          break;
        case "paint":
          GetPaint();
          break;
        case "type":
          GetBlocks("type");
          break;
        case "density":
          GetBlocks("density");
          break;
        case "rotation":
          GetBlocks("rotation");
          break;
        case "damage":
          GetBlocks("damage");
          break;
        case "meta":
          GetBlocks("meta");
          break;
        case "meta2":
          GetBlocks("meta2");
          break;
        case "meta3":
          GetBlocks("meta3");
          break;
        case "stats":
          GetStats();
          break;
        case "xml":
          GetXml();
          break;
        default:
          SendOutput($"Unknown sub command {Params[0]}");
          break;
      }
    }

    #region Helpers
    [CanBeNull]
    private static Prefab GetPrefab()
    {
      var prefab = new Prefab();
      if (File.Exists($"{prefabDir}{Params[1]}.tts") && prefab.Load(Params[1])) return prefab;

      SendOutput($"Unable to load prefab {Params[1]}");
      return null;
    }

    [NotNull]
    private static string GetFaceTex(long n, int f) => $"{(n >> f * 8) & 255L}";

    [NotNull]
    public static string GetTexStr(long n)
    {
      var d = new string[6];
      var same = true;
      for (var i = 0; i < 6; i++)
      {
        d[i] = GetFaceTex(n, i);
        if (d[0] != d[i]) same = false;
      }

      return same ? d[0] : string.Join(",", d);
    }

    [NotNull]
    private static string GetFilename(string file)
    {
      var num = file.LastIndexOf(_s);
      if (num >= 0)
      {
        file = file.Substring(num + 1);
      }
      return file.Replace(".tts", "");
    }

    [NotNull]
    private static Dictionary<string, object> ReadBinaryFile(string filepath, bool sizeOnly = false)
    {
      BinaryReader binaryReader = null;
      var data = new Dictionary<string, object>();
      try
      {
        binaryReader = new BinaryReader(File.OpenRead(filepath));
        if (binaryReader.ReadChar() == 't')
        {
          if (binaryReader.ReadChar() == 't')
          {
            if (binaryReader.ReadChar() == 's')
            {
              if (binaryReader.ReadChar() == '\0')
              {
                var version = binaryReader.ReadUInt32();
                data = ReadPrefab(binaryReader, version, sizeOnly);
              }
            }
          }
        }
      }
      catch (Exception e)
      {
        SendOutput("Error reading file");
        Log.Exception(e);
      }
      binaryReader?.Close();
      return data;
    }

    [NotNull]
    private static Dictionary<string, object> ReadPrefab(BinaryReader binaryReader, uint version, bool sizeOnly)
    {
      var data = new Dictionary<string, object>();

      var size = new Vector3i
      {
        x = binaryReader.ReadInt16(),
        y = binaryReader.ReadInt16(),
        z = binaryReader.ReadInt16()
      };
      data.Add("Size", size);
      if (sizeOnly) return data;

      if (version >= 2u && version < 7u) binaryReader.ReadBoolean();
      if (version >= 3u && version < 7u) binaryReader.ReadBoolean();

      //blocks
      var blockArray = new BCMBlockValue[size.x * size.y * size.z];
      if (version <= 4u)
      {
        for (var l = 0; l < blockArray.Length; l++)
        {
          blockArray[l] = new BCMBlockValue(binaryReader.ReadUInt32());
        }
      }
      else
      {
        for (var l = 0; l < blockArray.Length; l++)
        {
          blockArray[l] = new BCMBlockValue(version < 8u
            ? BlockValue.ConvertOldRawData(binaryReader.ReadUInt32())
            : binaryReader.ReadUInt32());
        }
      }
      data.Add("Blocks", blockArray);

      //density
      var densityArray = version <= 4u ? new byte[blockArray.Length] : binaryReader.ReadBytes(blockArray.Length);
      data.Add("Density", densityArray);

      //damage
      var damageArray = new ushort[blockArray.Length];
      if (version > 8u)
      {
        var a = binaryReader.ReadBytes(blockArray.Length * 2);
        for (var l = 0; l < blockArray.Length; l++)
        {
          damageArray[l] = (ushort)(a[l * 2] | a[l * 2 + 1] << 8);
        }
      }
      data.Add("Damage", damageArray);

      //paint
      //var paintArray = new long[blockArray.Length];
      //if (version >= 10u)
      //{
      //  ReadPaint(binaryReader);
      //  for (var n = 0; n < blockArray.Length; n++)
      //  {
      //    if (HasPaint())
      //    {
      //      paintArray[n] = binaryReader.ReadInt64();
      //    }
      //  }
      //}
      //Data.Add("Paint", paintArray);

      return data;
    }
    #endregion

    #region EditCommands
    private static void CreatePrefab()
    {
      if (Params.Count != 2 && Params.Count != 5)
      {
        SendOutput("Incorrect params for create");

        return;
      }

      if (File.Exists($"{prefabDir}{Params[1]}.tts"))
      {
        SendOutput($"A prefab with the name {Params[1]} already exists");

        return;
      }

      int x = 1, y = 1, z = 1;
      if (Params.Count == 5 && (!int.TryParse(Params[2], out x) || !int.TryParse(Params[3], out y) || !int.TryParse(Params[4], out z)))
      {
        SendOutput("Unable to parse prefab size");

        return;
      }

      var newPrefab = new Prefab(new Vector3i(x, y, z))
      {
        bCopyAirBlocks = true,
        filename = Params[1]
      };
      newPrefab.Save(Params[1]);
      SendOutput($"Prefab created {Params[1]}, size={x},{y},{z}");
    }

    private static void ClonePrefab()
    {
      if (Params.Count != 3)
      {
        SendOutput("Incorrect params for clone");

        return;
      }

      var s = $"{prefabDir}{Params[1]}";
      if (!File.Exists($"{s}.tts"))
      {
        SendOutput($"Source prefab name {Params[1]} doesn't exist");

        return;
      }

      var d = $"{prefabDir}{Params[2]}";
      if (File.Exists($"{d}.tts"))
      {
        SendOutput($"Target prefab name {Params[2]} already exists");

        return;
      }

      File.Copy($"{s}.tts", $"{d}.tts");

      if (File.Exists($"{s}.xml"))
      {
        File.Copy($"{s}.xml", $"{d}.xml");
      }
      if (File.Exists($"{s}.mesh"))
      {
        File.Copy($"{s}.mesh", $"{d}.mesh");
      }

      SendOutput($"Prefab cloned from {Params[1]} to {Params[2]}");
    }

    private static void BackupPrefab(bool suppressErr = false)
    {
      if (Params.Count != 2 && !suppressErr)
      {
        SendOutput("Incorrect params for backup");

        return;
      }

      if (!File.Exists($"{prefabDir}{Params[1]}.tts"))
      {
        if (!suppressErr) SendOutput($"No prefab with the name {Params[1]} found");

        return;
      }

      var backupDir = $"{prefabDir}BCMBackups{_s}{Params[1]}{_s}";
      if (!Directory.Exists(backupDir))
      {
        Directory.CreateDirectory(backupDir);
      }

      var timestamp = DateTime.UtcNow;
      var newfile = $"{backupDir}{timestamp.Year}_{timestamp.Month + 1}_{timestamp.Day}_{timestamp.Hour}_{timestamp.Minute}_{timestamp.Second}";

      File.Copy($"{prefabDir}{Params[1]}.tts", $"{newfile}.tts", true);
      if (File.Exists($"{prefabDir}{Params[1]}.xml"))
      {
        File.Copy($"{prefabDir}{Params[1]}.xml", $"{newfile}.xml", true);
      }
      if (File.Exists($"{prefabDir}{Params[1]}.mesh"))
      {
        File.Copy($"{prefabDir}{Params[1]}.mesh", $"{newfile}.mesh", true);
      }

      SendOutput($"Backup saved for prefab {Params[1]} - {GetFilename(newfile)}");
    }

    private static void RestorePrefab()
    {
      if (Params.Count < 2)
      {
        SendOutput("Incorrect params for restore");

        return;
      }

      var backupDir = $"{prefabDir}BCMBackups{_s}{Params[1]}{_s}";
      if (!Directory.Exists(backupDir))
      {
        SendOutput($"No prefab backups found for {Params[1]}");

        return;
      }

      var files = Directory.GetFiles(backupDir, "*.tts");
      if (files.Length == 0)
      {
        SendOutput($"No prefab backups found for {Params[1]}");

        return;
      }

      switch (Params.Count)
      {
        case 2:
          {
            var l = 0;
            foreach (var file in files)
            {
              SendOutput($"{l}: {GetFilename(file)}");
              l++;
            }
            break;
          }
        case 3:
          {
            int l;
            if (Params[2] == "last")
            {
              l = files.Length - 1;
            }
            else if (!int.TryParse(Params[2], out l))
            {
              SendOutput("Unable to parse index");

              return;
            }

            if (l < files.Length)
            {
              BackupPrefab(true);
              var fileBase = files[l].Replace(".tts", "");
              File.Copy(files[l], $"{prefabDir}{Params[1]}.tts", true);
              if (File.Exists($"{fileBase}.xml"))
              {
                File.Copy($"{fileBase}.xml", $"{prefabDir}{Params[1]}.xml", true);
              }
              if (File.Exists($"{fileBase}.mesh"))
              {
                File.Copy($"{fileBase}.mesh", $"{prefabDir}{Params[1]}.mesh", true);
              }

              SendOutput($"Prefab restored from backup {Params[1]} from {GetFilename(fileBase)}");
            }

            break;
          }
        case 4:
          {
            if (Params[2] != "delete")
            {
              SendOutput("Incorrect params for restore");

              return;
            }

            int l;
            switch (Params[3])
            {
              case "all":
                l = -1;
                break;
              case "last":
                l = files.Length - 1;
                break;
              default:
                if (!int.TryParse(Params[3], out l))
                {
                  SendOutput("Unable to parse index");

                  return;
                }
                break;
            }

            for (var i = l == -1 ? 0 : l; i <= (l == -1 ? files.Length - 1 : l); i++)
            {
              if (i >= files.Length) continue;

              var fileBase = files[i].Replace(".tts", "");
              if (File.Exists($"{fileBase}.tts"))
              {
                File.Delete($"{fileBase}.tts");
              }
              if (File.Exists($"{fileBase}.xml"))
              {
                File.Delete($"{fileBase}.xml");
              }
              if (File.Exists($"{fileBase}.mesh"))
              {
                File.Delete($"{fileBase}.mesh");
              }

              SendOutput($"Backup {GetFilename(fileBase)} deleted for {Params[1]}");
            }

            if (Directory.GetFiles(backupDir, "*.tts").Length == 0)
            {
              Directory.Delete(backupDir);
            }

            break;
          }
      }
    }

    private static void DeletePrefab()
    {
      if (Params.Count != 2)
      {
        SendOutput("Incorrect params for delete");

        return;
      }

      if (!File.Exists($"{prefabDir}{Params[1]}.tts"))
      {
        SendOutput($"No prefab with the name {Params[1]} found");

        return;
      }

      File.Delete($"{prefabDir}{Params[1]}.tts");
      if (File.Exists($"{prefabDir}{Params[1]}.xml"))
      {
        File.Delete($"{prefabDir}{Params[1]}.xml");
      }
      if (File.Exists($"{prefabDir}{Params[1]}.mesh"))
      {
        File.Delete($"{prefabDir}{Params[1]}.mesh");
      }

      SendOutput($"Prefab deleted {Params[1]}");
    }

    private static void SetProperty()
    {
      if (Params.Count < 2)
      {
        SendOutput("Incorrect params for set");

        return;
      }

      var prefab = GetPrefab();
      if (prefab == null)
      {
        return;
      }

      switch (Params.Count)
      {
        case 2:
          //list all properties
          SendJson(new Dictionary<string, object>
          {
            {
              "CopyAirBlocks",
              prefab.properties.Values.ContainsKey("CopyAirBlocks") ? prefab.properties.GetStringValue("CopyAirBlocks") : null
            },
            {
              "AllowTopSoilDecorations",
              prefab.properties.Values.ContainsKey("AllowTopSoilDecorations") ? prefab.properties.GetStringValue("AllowTopSoilDecorations") : null
            },
            {
              "YOffset",
              prefab.properties.Values.ContainsKey("YOffset") ? prefab.properties.GetStringValue("YOffset") : null
            },
            {
              "RotationToFaceNorth",
              prefab.properties.Values.ContainsKey("RotationToFaceNorth") ? prefab.properties.GetStringValue("RotationToFaceNorth") : null
            },
            {
              "ExcludeDistantPOIMesh",
              prefab.properties.Values.ContainsKey("ExcludeDistantPOIMesh") ? prefab.properties.GetStringValue("ExcludeDistantPOIMesh") : null
            },
            {
              "DistantPOIYOffset",
              prefab.properties.Values.ContainsKey("DistantPOIYOffset") ? prefab.properties.GetStringValue("DistantPOIYOffset") : null
            },
            {
              "DistantPOIOverride",
              prefab.properties.Values.ContainsKey("DistantPOIOverride") ? prefab.properties.GetStringValue("DistantPOIOverride") : null
            },
            {
              "TraderArea",
              prefab.properties.Values.ContainsKey("TraderArea") ? prefab.properties.GetStringValue("TraderArea") : null
            },
            {
              "TraderAreaProtect",
              prefab.properties.Values.ContainsKey("TraderAreaProtect") ? prefab.properties.GetStringValue("TraderAreaProtect") : null
            },
            {
              "TraderAreaTeleportSize",
              prefab.properties.Values.ContainsKey("TraderAreaTeleportSize") ? prefab.properties.GetStringValue("TraderAreaTeleportSize") : null
            },
            {
              "TraderAreaTeleportCenter",
              prefab.properties.Values.ContainsKey("TraderAreaTeleportCenter") ? prefab.properties.GetStringValue("TraderAreaTeleportCenter") : null
            },
            {
              "SleeperVolumeStart",
              prefab.properties.Values.ContainsKey("SleeperVolumeStart") ? prefab.properties.GetStringValue("SleeperVolumeStart") : null
            },
            {
              "SleeperVolumeSize",
              prefab.properties.Values.ContainsKey("SleeperVolumeSize") ? prefab.properties.GetStringValue("SleeperVolumeSize") : null
            },
            {
              "SleeperVolumeGroup",
              prefab.properties.Values.ContainsKey("SleeperVolumeGroup") ? prefab.properties.GetStringValue("SleeperVolumeGroup") : null
            },
            {
              "SleeperIsLootVolume",
              prefab.properties.Values.ContainsKey("SleeperIsLootVolume") ? prefab.properties.GetStringValue("SleeperIsLootVolume") : null
            },
            {
              "SleeperVolumeGameStageAdjust",
              prefab.properties.Values.ContainsKey("SleeperVolumeGameStageAdjust") ? prefab.properties.GetStringValue("SleeperVolumeGameStageAdjust") : null
            },
            {
              "Zoning",
              prefab.properties.Values.ContainsKey("Zoning") ? prefab.properties.GetStringValue("Zoning") : null
            },
            {
              "AllowedBiomes",
              prefab.properties.Values.ContainsKey("AllowedBiomes") ? prefab.properties.GetStringValue("AllowedBiomes") : null
            },
            {
              "AllowedTownships",
              prefab.properties.Values.ContainsKey("AllowedTownships") ? prefab.properties.GetStringValue("AllowedTownships") : null
            },
            {
              "Tags",
              prefab.properties.Values.ContainsKey("Tags") ? prefab.properties.GetStringValue("Tags") : null
            },
            {
              "Condition",
              prefab.properties.Values.ContainsKey("Condition") ? prefab.properties.GetStringValue("Condition") : null
            },
            {
              "Age",
              prefab.properties.Values.ContainsKey("Age") ? prefab.properties.GetStringValue("Age") : null
            },
            {
              "StaticSpawner.Class",
              prefab.properties.Values.ContainsKey("StaticSpawner.Class") ? prefab.properties.GetStringValue("StaticSpawner.Class") : null
            },
            {
              "StaticSpawner.Size",
              prefab.properties.Values.ContainsKey("StaticSpawner.Size") ? prefab.properties.GetStringValue("StaticSpawner.Size") : null
            },
            {
              "StaticSpawner.Trigger",
              prefab.properties.Values.ContainsKey("StaticSpawner.Trigger") ? prefab.properties.GetStringValue("StaticSpawner.Trigger") : null
            }
          });
          break;
        case 3:
          //show property value
          SendJson(new { Property = Params[2], Value = prefab.properties.Values.ContainsKey(Params[2]) ? prefab.properties.GetStringValue(Params[2]) : null });
          break;
        case 4:
          //set property value
          var oldVal = prefab.properties.Values.ContainsKey(Params[2]) ? prefab.properties.Values[Params[2]] : null;
          prefab.properties.Values[Params[2]] = Params[3];
          prefab.properties.Save("prefab", prefabDir, prefab.filename);
          SendJson(new { Property = Params[2], OldValue = oldVal, Value = prefab.properties.GetStringValue(Params[2]) });
          break;
      }
    }

    //todo: add below to all prefab editing commands
    //BackupPrefab(true);

    private static void SetSize()
    {
      if (Params.Count != 5)
      {
        SendOutput("Incorrect params for setsize");

        return;
      }

      var prefab = GetPrefab();
      if (prefab == null) return;

      if (!int.TryParse(Params[2], out var sx) || !int.TryParse(Params[3], out var sy) || !int.TryParse(Params[4], out var sz))
      {
        SendOutput("Unable to parse x,y,z for size");

        return;
      }

      if (sx < prefab.size.x || sy < prefab.size.y || sz < prefab.size.z)
      {
        SendOutput("Prefabs can only be made bigger with the size command, use trim to remove layers");

        return;
      }

      var newPrefab = new Prefab(new Vector3i(sx, sy, sz));
      BackupPrefab(true);
      for (var x = 0; x < prefab.size.x; x++)
      {
        for (var z = 0; z < prefab.size.z; z++)
        {
          for (var y = 0; y < prefab.size.y; y++)
          {
            newPrefab.SetBlock(x, y, z, prefab.GetBlock(x, y, z));
            newPrefab.SetDensity(x, y, z, prefab.GetDensity(x, y, z));
            newPrefab.SetTexture(x, y, z, prefab.GetTexture(x, y, z));
          }
        }
      }

      newPrefab.filename = Params[1];
      newPrefab.bCopyAirBlocks = true;
      newPrefab.Save(Params[1]);

      newPrefab.properties = prefab.properties;
      newPrefab.properties.Save("prefab", prefabDir, Params[1]);

      SendOutput($"Size set to {sx},{sy},{sz} for prefab {Params[1]}");
    }

    private static void AddLayers()
    {
      if (Params.Count != 4 || Params.Count == 3 && Params[2] != "all")
      {
        SendOutput("Incorrect params for add");

        return;
      }

      var prefab = GetPrefab();
      if (prefab == null) return;

      //top, left, right, front, back, bottom
      var side = Params[2];
      var amount = 0;

      if (Params.Count > 3 && !int.TryParse(Params[3], out amount))
      {
        SendOutput($"Unable to parse amount: {Params[3]}");

        return;
      }

      var lx = side == "all" || side == "left" ? amount : 0;
      var rx = side == "all" || side == "right" ? amount : 0;
      var fz = side == "all" || side == "front" ? amount : 0;
      var bz = side == "all" || side == "back" ? amount : 0;
      var by = side == "all" || side == "bottom" ? amount : 0;
      var ty = side == "all" || side == "top" ? amount : 0;

      var newPrefab = new Prefab(new Vector3i(prefab.size.x + lx + rx, prefab.size.y + by + ty, prefab.size.z + fz + bz));
      BackupPrefab(true);
      for (var x = 0; x < prefab.size.x; x++)
      {
        for (var z = 0; z < prefab.size.z; z++)
        {
          for (var y = 0; y < prefab.size.y; y++)
          {
            newPrefab.SetBlock(x + lx, y + by, z + fz, prefab.GetBlock(x, y, z));
            newPrefab.SetDensity(x + lx, y + by, z + fz, prefab.GetDensity(x, y, z));
            newPrefab.SetTexture(x + lx, y + by, z + fz, prefab.GetTexture(x, y, z));
          }
        }
      }

      newPrefab.filename = Params[1];
      newPrefab.bCopyAirBlocks = true;
      newPrefab.Save(Params[1]);

      newPrefab.properties = prefab.properties;
      if (lx > 0 || by > 0 || fz > 0)
      {
        newPrefab.properties.Values["SleeperVolumeStart"] =
          Utils.ToStringVector3iList(
            prefab.SleeperVolumesStart.Select(
              v => new Vector3i(v.x + lx, v.y + by, v.z + fz)
            ).ToList(),
            prefab.SleeperVolumeUsed
          );
      }
      newPrefab.properties.Save("prefab", prefabDir, Params[1]);

      SendOutput($"{amount} blocks added to {(side == "all" ? "all sides" : side)} of prefab");
    }

    private static void TrimLayers()
    {
      if (Params.Count == 3 && Params[2] != "air")
      {
        SendOutput("Incorrect params for trim");

        return;
      }

      var prefab = GetPrefab();
      if (prefab == null) return;

      //top, left, right, front, back, bottom, air
      var side = Params[2];
      var amount = 0;

      if (Params.Count > 3 && !int.TryParse(Params[3], out amount))
      {
        SendOutput($"Unable to parse amount: {Params[3]}");

        return;
      }

      var lx = side != "air" ? (side == "left" ? amount : 0) : GetLeftX(prefab);
      var rx = side != "air" ? (side == "right" ? amount : 0) : GetRightX(prefab);
      var fz = side != "air" ? (side == "front" ? amount : 0) : GetFrontZ(prefab);
      var bz = side != "air" ? (side == "back" ? amount : 0) : GetBackZ(prefab);
      var by = side != "air" ? (side == "bottom" ? amount : 0) : GetBottomY(prefab);
      var ty = side != "air" ? (side == "top" ? amount : 0) : GetTopY(prefab);

      var newPrefab = new Prefab(new Vector3i(prefab.size.x - lx - rx, prefab.size.y - by - ty, prefab.size.z - fz - bz));
      BackupPrefab(true);
      for (var x = lx; x < prefab.size.x - rx; x++)
      {
        for (var z = fz; z < prefab.size.z - bz; z++)
        {
          for (var y = by; y < prefab.size.y - ty; y++)
          {
            newPrefab.SetBlock(x - lx, y - by, z - fz, prefab.GetBlock(x, y, z));
            newPrefab.SetDensity(x - lx, y - by, z - fz, prefab.GetDensity(x, y, z));
            newPrefab.SetTexture(x - lx, y - by, z - fz, prefab.GetTexture(x, y, z));
          }
        }
      }

      newPrefab.filename = Params[1];
      newPrefab.bCopyAirBlocks = true;
      newPrefab.Save(Params[1]);

      newPrefab.properties = prefab.properties;
      if (lx > 0 || by > 0 || fz > 0)
      {
        newPrefab.properties.Values["SleeperVolumeStart"] =
          Utils.ToStringVector3iList(
            prefab.SleeperVolumesStart.Select(
              v => new Vector3i(v.x - lx, v.y - by, v.z - fz)
            ).ToList(),
            prefab.SleeperVolumeUsed
          );
      }
      newPrefab.properties.Save("prefab", prefabDir, Params[1]);

      SendOutput($"{(side == "air" ? "Air" : $"{amount}")} blocks trimmed from {(side == "air" ? "prefab" : $"{side} of prefab")}");
    }

    private void InsertLayers()
    {
      SendOutput("Work in Progress");

    }

    private void CutLayers()
    {
      SendOutput("Work in Progress");

    }

    private void CopyLayer()
    {
      SendOutput("Work in Progress");
    }

    private void SwapLayer()
    {
      SendOutput("Work in Progress");
    }

    private void SetBlock()
    {
      SendOutput("Work in Progress");
    }

    private static void SwapBlocks()
    {
      if (Params.Count != 4)
      {
        SendOutput("Incorrect params for swap");

        return;
      }

      if (!int.TryParse(Params[2], out var source))
      {
        SendOutput($"Unable to parse source: {Params[2]}");

        return;
      }

      if (!int.TryParse(Params[3], out var target))
      {
        SendOutput($"Unable to parse target: {Params[3]}");

        return;
      }

      var itemClass = ItemClass.GetForId(target);
      if (itemClass == null || !itemClass.IsBlock())
      {
        SendOutput("Unable to get target block");

        return;
      }

      var p = ReadBinaryFile($"{prefabDir}{Params[1]}.tts");
      if (!(p["Size"] is Vector3i size)) return;

      var prefab = GetPrefab();
      if (prefab == null) return;

      BackupPrefab(true);
      var l = 0;
      for (var x = 0; x < size.x; x++)
      {
        for (var z = 0; z < size.z; z++)
        {
          for (var y = 0; y < size.y; y++)
          {
            if (!(p["Blocks"] is BCMBlockValue[] blocks)) continue;

            var type = blocks[l].type;
            if (type != source) continue;

            //todo: check damage < maxhp
            var blockValue = prefab.GetBlock(x, y, z);
            if (blockValue.type == 0 && type != 0)
            {
              blockValue.rotation = blocks[l].rotation;
              blockValue.meta = blocks[l].meta;
            }
            blockValue.type = target;
            prefab.SetBlock(x, y, z, blockValue);
            l++;
          }
        }
      }
      prefab.Save(Params[1]);
    }

    private void SwapMap()
    {
      SendOutput("Work in progress");
    }

    private void RepairBlocks()
    {
      SendOutput("Work in progress");
    }

    private void LockSecureBlocks()
    {
      SendOutput("Work in progress");
    }
    #endregion

    #region PrefabInfoCommands
    private static void ListPrefabs()
    {
      if (!Directory.Exists(prefabDir)) return;

      var prefabs = Directory.GetFiles(prefabDir, "*.tts").Select(GetFilename);

      if (Params.Count == 2)
      {
        prefabs = prefabs.Where(p => p.IndexOf(Params[1], StringComparison.OrdinalIgnoreCase) > -1);
      }

      if (!Options.ContainsKey("dim"))
      {
        var prefabsList = prefabs.ToList();
        SendJson(new { prefabsList.Count, Filter = Params.Count == 2 ? Params[1] : null, Prefabs = prefabsList });

        return;
      }

      var prefabDetails = new Dictionary<string, string>();
      foreach (var pName in prefabs)
      {
        var p = ReadBinaryFile($"{prefabDir}{pName}.tts", true);
        if (p["Size"] is Vector3i size)
        {
          prefabDetails.Add(pName, $"{size.x},{size.y},{size.z}:{size.x * size.y * size.z}");
        }
      }
      SendJson(new { prefabDetails.Count, Filter = Params.Count == 2 ? Params[1] : null, Prefabs = prefabDetails });
    }

    private static void GetDimensions()
    {
      if (!File.Exists($"{prefabDir}{Params[1]}.tts"))
      {
        SendOutput("Prefab file not found");

        return;
      }

      var prefabData = ReadBinaryFile($"{prefabDir}{Params[1]}.tts", true);
      if (prefabData["Size"] is Vector3i size)
      {
        SendOutput($"{size.x},{size.y},{size.z}:{size.x * size.y * size.z}");
      }
    }

    private static void GetBlockCounts()
    {
      if (!File.Exists($"{prefabDir}{Params[1]}.tts"))
      {
        SendOutput("Prefab file not found");

        return;
      }

      var prefabData = ReadBinaryFile($"{prefabDir}{Params[1]}.tts");

      var d = new Dictionary<int, int>();
      if (prefabData["Size"] is Vector3i size)
      {
        if (prefabData["Blocks"] is BCMBlockValue[] blocks)
        {
          var l = 0;
          for (var x = 0; x < size.x; x++)
          {
            for (var y = 0; y < size.y; y++)
            {
              for (var z = 0; z < size.z; z++)
              {
                var key = blocks[l].type;
                if (d.ContainsKey(key))
                {
                  d[key] = d[key] + 1;
                }
                else
                {
                  d.Add(key, 1);
                }
                l++;
              }
            }
          }
          SendJson(new
          {
            Size = $"{size.x},{size.y},{size.z}",
            Total = $"{size.x * size.y * size.z}",
            Blocks = d.OrderBy(b => Options.ContainsKey("bycount") ? -b.Value : b.Key).Select(b =>
            {
              var ic = ItemClass.GetForId(b.Key);

              return new
              {
                Id = (ic == null && b.Key != 0 ? "NULL_" : "") + b.Key,
                Name = b.Key == 0 ? "air" : (ic == null ? "NULL" : ic.Name),
                Count = b.Value
              };
            }).ToList()
          });

          return;
        }
      }
      SendOutput("No data to display");
    }

    private static void GetBlocks(string type = "all")
    {
      if (!File.Exists($"{prefabDir}{Params[1]}.tts"))
      {
        SendOutput("Prefab file not found");

        return;
      }

      var prefabData = ReadBinaryFile($"{prefabDir}{Params[1]}.tts");

      var d = new Dictionary<string, string>();
      if (prefabData["Size"] is Vector3i size)
      {
        if (prefabData["Blocks"] is BCMBlockValue[] blocks && prefabData["Damage"] is ushort[] damage && prefabData["Density"] is byte[] density)
        {
          var l = 0;
          for (var x = 0; x < size.x; x++)
          {
            for (var y = 0; y < size.y; y++)
            {
              for (var z = 0; z < size.z; z++)
              {
                var key = $"{x},{y},{z}";
                switch (type)
                {
                  case "all":
                    d[key] = $"{blocks[l].type},{blocks[l].rotation},{damage[l]},{density[l]},{blocks[l].meta},{blocks[l].meta2},{blocks[l].meta3}";
                    break;
                  case "type":
                    d[key] = $"{blocks[l].type}";
                    break;
                  case "rotation":
                    d[key] = $"{blocks[l].rotation}";
                    break;
                  case "damage":
                    d[key] = $"{damage[l]}";
                    break;
                  case "density":
                    d[key] = $"{density[l]}";
                    break;
                  case "meta":
                    d[key] = $"{blocks[l].meta}";
                    break;
                  case "meta2":
                    d[key] = $"{blocks[l].meta2}";
                    break;
                  case "meta3":
                    d[key] = $"{blocks[l].meta3}";
                    break;
                }
                l++;
              }
            }
          }
          SendJson(new
          {
            Size = $"{size.x},{size.y},{size.z}",
            Total = $"{size.x * size.y * size.z}",
            Key = type == "all" ? "type,rotation,damage,density,meta,meta2,meta3" : type,
            Blocks = d
          });

          return;
        }
      }
      SendOutput("No data to display");
    }

    //todo: get paint in binary reader
    private static void GetPaint()
    {
      var d = new Dictionary<string, string>();
      var prefab = GetPrefab();
      if (prefab == null) return;

      for (var x = 0; x < prefab.size.x; x++)
      {
        for (var y = 0; y < prefab.size.y; y++)
        {
          for (var z = 0; z < prefab.size.z; z++)
          {
            d.Add($"{x},{y},{z}", GetTexStr(prefab.GetTexture(x, y, z)));
          }
        }
      }
      SendJson(d);
    }

    private static void GetStats()
    {
      var prefab = GetPrefab();
      if (prefab == null) return;

      SendJson(prefab.GetBlockStatistics());
    }

    private static void GetXml()
    {
      var file = $"{prefabDir}{Params[1]}.xml";
      if (File.Exists(file))
      {
        SendOutput(File.ReadAllText(file));
      }
    }
    #endregion

    #region PaintHelper
    //private readonly List<byte> paintBytes = new List<byte>();
    //private int paintBits;
    //private int paintCounter;
    //private byte paintByte;

    //private void ReadPaint(BinaryReader binaryReader)
    //{
    //  var count = binaryReader.ReadInt32();
    //  paintBytes.Clear();
    //  paintBytes.AddRange(binaryReader.ReadBytes(count));
    //  paintBits = 8;
    //  paintCounter = 0;
    //  paintByte = 0;
    //}

    //private bool HasPaint()
    //{
    //  if (paintBits > 7)
    //  {
    //    paintByte = paintBytes[paintCounter++];
    //    paintBits = 0;
    //  }
    //  var result = (paintByte & 1 << paintBits) != 0;
    //  paintBits++;
    //  return result;
    //}
    #endregion

    private static int GetBottomY(Prefab prefab)
    {
      var y = 0;

      while (y < prefab.size.y)
      {
        for (var x = 0; x < prefab.size.x; x++)
        {
          for (var z = 0; z < prefab.size.z; z++)
          {
            if (prefab.GetBlock(x, y, z).Equals(BlockValue.Air)) continue;

            return y;
          }
        }
        y++;
      }

      return 0;
    }
    private static int GetTopY(Prefab prefab)
    {
      var y = prefab.size.y - 1;

      while (y >= 0)
      {
        for (var x = 0; x < prefab.size.x; x++)
        {
          for (var z = 0; z < prefab.size.z; z++)
          {
            if (prefab.GetBlock(x, y, z).type == 0) continue;

            return prefab.size.y - y - 1;
          }
        }
        y--;
      }

      return 0;
    }
    private static int GetLeftX(Prefab prefab)
    {
      var x = 0;

      while (x < prefab.size.x)
      {
        for (var y = 0; y < prefab.size.y; y++)
        {
          for (var z = 0; z < prefab.size.z; z++)
          {
            if (prefab.GetBlock(x, y, z).type == 0) continue;

            return x;
          }
        }
        x++;
      }

      return 0;
    }
    private static int GetRightX(Prefab prefab)
    {
      var x = prefab.size.x - 1;

      while (x >= 0)
      {
        for (var y = 0; y < prefab.size.y; y++)
        {
          for (var z = 0; z < prefab.size.z; z++)
          {
            if (prefab.GetBlock(x, y, z).type == 0) continue;

            return prefab.size.x - x - 1;
          }
        }
        x--;
      }

      return 0;
    }
    private static int GetFrontZ(Prefab prefab)
    {
      var z = 0;

      while (z < prefab.size.z)
      {
        for (var x = 0; x < prefab.size.x; x++)
        {
          for (var y = 0; y < prefab.size.y; y++)
          {
            if (prefab.GetBlock(x, y, z).type == 0) continue;

            return z;
          }
        }
        z++;
      }

      return 0;
    }
    private static int GetBackZ(Prefab prefab)
    {
      var z = prefab.size.z - 1;

      while (z >= 0)
      {
        for (var x = 0; x < prefab.size.x; x++)
        {
          for (var y = 0; y < prefab.size.y; y++)
          {
            if (prefab.GetBlock(x, y, z).type == 0) continue;

            return prefab.size.z - z - 1;
          }
        }
        z--;
      }

      return 0;
    }
  }
}
