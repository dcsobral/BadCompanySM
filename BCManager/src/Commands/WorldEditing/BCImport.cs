using System;
using System.Collections.Generic;
using System.IO;

namespace BCM.Commands
{
  public class BCImport : BCCommandAbstract
  {
    public class PrefabCache
    {
      public string Filename;
      public Vector3i Pos;
    }

    public Dictionary<int, List<PrefabCache>> UndoCache = new Dictionary<int, List<PrefabCache>>();

    private static bool GetXyz(Prefab prefab, Entity entity, ref int x, ref int y, ref int z)
    {
      //todo: use loc position for imports with /loc option
      if (entity == null) return false;

      var loc = new Vector3i((int)Math.Floor(entity.serverPos.x / 32f), (int)Math.Floor(entity.serverPos.y / 32f), (int)Math.Floor(entity.serverPos.z / 32f));

      x = loc.x - prefab.size.x / 2;
      y = loc.y;
      z = loc.z - prefab.size.z / 2;
      if (Options.ContainsKey("csw") || Options.ContainsKey("ne"))
      {
        x = loc.x;
        z = loc.z;
      }
      else if (Options.ContainsKey("cse") || Options.ContainsKey("nw"))
      {
        x = loc.x - prefab.size.x;
        z = loc.z;
      }
      else if (Options.ContainsKey("cnw") || Options.ContainsKey("se"))
      {
        x = loc.x;
        z = loc.z - prefab.size.z;
      }
      else if (Options.ContainsKey("cne") || Options.ContainsKey("sw"))
      {
        x = loc.x - prefab.size.x;
        z = loc.z - prefab.size.z;
      }

      return true;
    }

    private bool GetXyzRot(Prefab prefab, ref int r, ref int x, ref int y, ref int z)
    {
      if (Params.Count > 1)
      {
        switch (Params.Count)
        {
          case 2:
            if (!int.TryParse(Params[1], out r))
            {
              SendOutput("<rot> param could not be parsed as a number.");

              return false;
            }

            break;

          case 4:
            // specific spawnpoint
            if (!int.TryParse(Params[1], out x) || !int.TryParse(Params[2], out y) || !int.TryParse(Params[3], out z))
            {
              SendOutput("One of <x> <y> <z> params could not be parsed as a number.");

              return false;
            }
            r = 0;

            break;

          case 5:
            // specific spawnpoint
            if (!int.TryParse(Params[1], out x) || !int.TryParse(Params[2], out y) || !int.TryParse(Params[3], out z) || !int.TryParse(Params[4], out r))
            {
              SendOutput("One of <x> <y> <z> <rot> params could not be parsed as a number.");

              return false;
            }

            break;

          default:
            if (Params.Count != 6)
            {
              SendOutput("Error: Incorrect command format.");
              SendOutput(GetHelp());

              return false;
            }

            break;

        }

        // spin the prefab
        prefab.RotateY(false, r % 4);
      }

      //bounds
      //todo: make overridable (prefab wont replace blocks outside bounds but will partial spawn)
      if (y < 0)
      {
        SendOutput($"Y position is too low by {y * -1} blocks");
      }
      if (y + prefab.size.y > 255)
      {
        SendOutput($"Y position is too high by {y + prefab.size.y - 255} blocks");
      }

      return true;
    }

    private static void BlockTranslations(Prefab prefab, Vector3i pos)
    {
      // todo: custom block map via configs and select with /map=<mapname>, also a few options like wood->metal->concrete->steel upgrades (/upgrade=2 (steps))

      // ENTITIES
      var entities = new List<int>();
      //      bool bSpawnEnemies = true;//todo: toggle for sleepers?
      //entities.Clear();
      prefab.CopyEntitiesIntoWorld(GameManager.Instance.World, pos, entities, prefab.bSleeperVolumes);// bSpawnEnemies

      //BLOCK TRANSLATIONS
      var map = LootContainer.lootPlaceholderMap;
      for (var px = 0; px < prefab.size.x; px++)
      {
        for (var py = 0; py < prefab.size.y; py++)
        {
          for (var pz = 0; pz < prefab.size.z; pz++)
          {
            var bv = prefab.GetBlock(px, py, pz);
            // ENTITIES
            //List<EntityCreationData> entities = prefab.GetEntities();
            //foreach (EntityCreationData _ecd in entities)
            //{
            //  _ecd.id = -1;
            //  Entity entity = EntityFactory.CreateEntity(_ecd);
            //  entity.SetPosition(entity.position + pos.ToVector3());
            //  GameManager.Instance.World.SpawnEntityInWorld(entity);
            //}


            // LOOT PLACEHOLDERS
            if (bv.type == 0) continue;

            var random = new Random(Guid.NewGuid().GetHashCode());
            var bvr = new BlockValue(map.Replace(bv, random).rawData);
            if (bv.type == bvr.type) continue;

            prefab.SetBlock(px, py, pz, bvr);
          }
        }
      }
    }

    public static void InsertPrefab(Prefab prefab, int x, int y, int z, Vector3i pos)
    {
      var world = GameManager.Instance.World;
      if (world == null) return;

      if (prefab == null)
      {
        SendOutput("No Prefab loaded.");

        return;
      }

      //GET AFFECTED CHUNKS
      var modifiedChunks = new Dictionary<long, Chunk>();
      for (var cx = -1; cx <= prefab.size.x + 16; cx = cx + 16)
      {
        for (var cz = -1; cz <= prefab.size.z + 16; cz = cz + 16)
        {
          var chunk = world.GetChunkFromWorldPos(x + cx, y, z + cz) as Chunk;
          if (chunk == null)
          {
            // todo: generate and observe chunks required
            SendOutput("Unable to load chunk for prefab @ " + (x + cx) + "," + (z + cz));
          }
          else
          {
            if (modifiedChunks.ContainsKey(chunk.Key)) continue;

            modifiedChunks.Add(chunk.Key, chunk);
          }
        }
      }

      //INSERT PREFAB
      //todo: set owner
      //todo: spawn sleeper blocks
      //todo: disable spawns
      if (Options.ContainsKey("snap"))
      {
        //todo:test
        prefab.SnapTerrainToArea(world.ChunkCache, pos);
      }
      prefab.CopyIntoLocal(world.ChunkCache, pos, true, true);

      //RELOAD CHUNKS
      BCChunks.ReloadForClients(modifiedChunks);
    }

    private void CreateUndo(EntityPlayer sender, Vector3i size, Vector3i p0)
    {
      var steamId = "_server";
      if (SenderInfo.RemoteClientInfo != null)
      {
        steamId = SenderInfo.RemoteClientInfo.ownerId;
      }

      var areaCache = new Prefab();
      var userId = 0; // id will be 0 for web console issued commands
      areaCache.CopyFromWorld(GameManager.Instance.World, p0, new Vector3i(p0.x + size.x, p0.y + size.y, p0.z + size.z));
      areaCache.bCopyAirBlocks = true;

      if (sender != null)
      {
        userId = sender.entityId;
      }
      var filename = $"{steamId}_{DateTime.UtcNow.ToFileTime()}";
      Directory.CreateDirectory(Utils.GetGameDir("Data/Prefabs/BCM"));
      areaCache.Save(Utils.GetGameDir("Data/Prefabs/BCM"), filename);

      if (UndoCache.ContainsKey(userId))
      {
        UndoCache[userId].Add(new PrefabCache { Filename = filename, Pos = p0 });
      }
      else
      {
        UndoCache.Add(userId, new List<PrefabCache> { new PrefabCache { Filename = filename, Pos = p0 } });
      }
    }

    private void UndoInsert(EntityPlayer sender)
    {
      const string dirbase = "Data/Prefabs/BCM";
      var userId = 0;
      if (sender != null)
      {
        userId = sender.entityId;
      }
      if (!UndoCache.ContainsKey(userId)) return;

      if (UndoCache[userId].Count <= 0) return;

      var pCache = UndoCache[userId][UndoCache[userId].Count - 1];
      if (pCache != null)
      {
        var p = new Prefab();
        p.Load(Utils.GetGameDir(dirbase), pCache.Filename);
        InsertPrefab(p, pCache.Pos.x, pCache.Pos.y, pCache.Pos.z, pCache.Pos);

        //workaround for multi dim blocks, insert undo prefab twice
        //todo: clear all blocks (turn to air) before inserting the prefab instead
        InsertPrefab(p, pCache.Pos.x, pCache.Pos.y, pCache.Pos.z, pCache.Pos);

        var cacheFile = Utils.GetGameDir($"{dirbase}{pCache.Filename}");
        if (Utils.FileExists($"{cacheFile}.tts"))
        {
          Utils.FileDelete($"{cacheFile}.tts");
        }
        if (Utils.FileExists($"{cacheFile}.xml"))
        {
          Utils.FileDelete($"{cacheFile}.xml");
        }
      }
      UndoCache[userId].RemoveAt(UndoCache[userId].Count - 1);
    }

    public override void Process()
    {
      // todo: clear out miltidim blocks properly
      // todo: remove LCB's from persistent players
      // todo: remove loot container contents before inserting new prefab
      // todo: add map visitor to load chunks if required

      // optional todo: allow for partial names for prefab, provide list if more then one result, allow for partial + # from list to specify
      // optional todo: refresh nearest prefab
      // optional todo: store last x commands for each player, with linked undo data add a repeat option so that prefab inserts at a players location can be repeated at the same location even if player moves


      EntityPlayer sender = null;
      if (SenderInfo.RemoteClientInfo != null)
      {
        sender = GameManager.Instance.World.Entities.dict[SenderInfo.RemoteClientInfo.entityId] as EntityPlayer;
      }

      if (Options.ContainsKey("undo"))
      {
        UndoInsert(sender);

        return;
      }

      if (Params.Count == 0) return;

      var prefab = new Prefab();
      if (!prefab.Load(Params[0])) return;

      var rot = 0;
      int x = 0, y = 0, z = 0;
      if (!GetXyz(prefab, sender, ref x, ref y, ref z) && Params.Count < 5)
      {
        SendOutput("Command requires <name> <x> <y> <z> <rot> params if not sent by an online player");

        return;
      }

      if (!GetXyzRot(prefab, ref rot, ref x, ref y, ref z)) return;

      var pos = Options.ContainsKey("nooffset") ? new Vector3i(x, y, z) : new Vector3i(x, y + prefab.yOffset, z);

      // todo: create an entity observer and spawn prefab once chunks are loaded?


      if (Options.ContainsKey("air"))
      {
        prefab.bCopyAirBlocks = true;
      }
      if (Options.ContainsKey("noair"))
      {
        prefab.bCopyAirBlocks = false;
      }
      if (Options.ContainsKey("sleepers"))
      {
        prefab.bSleeperVolumes = true;
      }
      if (Options.ContainsKey("nosleepers"))
      {
        prefab.bSleeperVolumes = false;
      }

      // todo: create a copy of the chunks and the bounded dimensions of the prefab size for an undo
      //       should work better than a prefab copy undo as it will preserve block ownership and state?
      // optional todo: option to carve terrain where prefab will spawn, maybe reapply decorations?
      //                insert lot into cell data and regenerate chunk area from world/seed defaults

      BlockTranslations(prefab, pos);

      //CREATE UNDO
      //create backup of area prefab will insert to
      if (!Options.ContainsKey("noundo"))
      {
        CreateUndo(sender, prefab.size, pos);
      }

      // SPAWN PREFAB
      Log.Out($"{Config.ModPrefix}Spawning prefab {prefab.filename} @ {pos}, size={prefab.size}");
      SendOutput($"Spawning prefab {prefab.filename} @ {pos}, size={prefab.size}");
      SendOutput("use bc-import /undo to revert the changes");

      InsertPrefab(prefab, x, y, z, pos);
    }
  }
}
