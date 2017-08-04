using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace BCM.Commands
{
  public class BCImport : BCCommandAbstract
  {
    public class PrefabCache
    {
      public string filename;
      public Vector3i pos;
    }

    //todo: persist cache to disk
    public Dictionary<int, List<PrefabCache>> _cache = new Dictionary<int, List<PrefabCache>>();

    private bool GetXYZPreEnt(Prefab _p, Entity _e, ref int _x, ref int _y, ref int _z)
    {
      //todo: use loc position for imports with /loc option
      if (_e == null)
      {
        return false;
      }
      var loc = new Vector3i((int)Math.Floor(_e.serverPos.x / 32f), (int)Math.Floor(_e.serverPos.y / 32f), (int)Math.Floor(_e.serverPos.z / 32f));

      _x = loc.x - (_p.size.x / 2);
      _y = loc.y;
      _z = loc.z - (_p.size.z / 2);
      if (_options.ContainsKey("cornersw") || _options.ContainsKey("ne"))
      {
        _x = loc.x;
        _z = loc.z;
      }
      else if (_options.ContainsKey("cornerse") || _options.ContainsKey("nw"))
      {
        _x = loc.x - _p.size.x;
        _z = loc.z;
      }
      else if (_options.ContainsKey("cornernw") || _options.ContainsKey("se"))
      {
        _x = loc.x;
        _z = loc.z - _p.size.z;
      }
      else if (_options.ContainsKey("cornerne") || _options.ContainsKey("sw"))
      {
        _x = loc.x - _p.size.x;
        _z = loc.z - _p.size.z;
      }

      return true;
    }

    private bool GetXYZRotSpin(Prefab _prefab, ref int _r, ref int _x, ref int _y, ref int _z)
    {
      if (_params.Count > 1)
      {
        if (_params.Count == 2)
        {
          if (!int.TryParse(_params[1], out _r))
          {
            SendOutput("<rot> param could not be parsed as a number.");
            return false;
          }
        }
        else if (_params.Count == 5)
        {
          // specific spawnpoint
          if (!int.TryParse(_params[1], out _x) || !int.TryParse(_params[2], out _y) || !int.TryParse(_params[3], out _z) || !int.TryParse(_params[4], out _r))
          {
            SendOutput("One of <x> <y> <z> <rot> params could not be parsed as a number.");
            return false;
          }
        }
        else if (_params.Count != 6)
        {
          SendOutput("Error: Incorrect command format.");
          SendOutput(GetHelp());
          return false;
        }


        // spin the prefab
        for (int r = 0; r < _r % 4; r++)
        {
          _prefab.RotateY(false);
        }
      }

      //bounds
      //todo: make overridable (prefab wont replace blocks outside bounds but will partial spawn)
      if (_y < 0)
      {
        SdtdConsole.Instance.Output("Y position is too low by " + (_y * -1).ToString() + " blocks");
      }
      if (_y + _prefab.size.y > 255)
      {
        SdtdConsole.Instance.Output("Y position is too high by " + (_y + _prefab.size.y - 255).ToString() + " blocks");
      }

      return true;
    }

    private static void BlockTranslations(Prefab prefab, Vector3i pos)
    {
      // todo: custom block map via configs and select with /map=<mapname>, also a few options like wood->metal->concrete->steel upgrades (/upgrade=2 (steps))

      // ENTITIES
      List<int> entities = new List<int>();
//      bool bSpawnEnemies = true;//todo: toggle for sleepers?
      //entities.Clear();
      prefab.CopyEntitiesIntoWorld(GameManager.Instance.World, pos, entities, prefab.bSleeperVolumes);// bSpawnEnemies

      //BLOCK TRANSLATIONS
      LootPlaceholderMap _map = LootContainer.lootPlaceholderMap;
      for (int px = 0; px < prefab.size.x; px++)
      {
        for (int py = 0; py < prefab.size.y; py++)
        {
          for (int pz = 0; pz < prefab.size.z; pz++)
          {
            BlockValue bv = prefab.GetBlock(px, py, pz);
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
            if (bv.type != 0)
            {
              System.Random random = new System.Random(Guid.NewGuid().GetHashCode());
              BlockValue bvr = new BlockValue(_map.Replace(bv, random).rawData);
              if (bv.type != bvr.type)
              {
                prefab.SetBlock(px, py, pz, bvr);
              }
            }
          }
        }
      }
    }

    public static void InsertPrefab(Prefab prefab, int x, int y, int z, Vector3i pos)
    {
      if (prefab == null)
      {
        SdtdConsole.Instance.Output("No Prefab loaded.");
        return;
      }

      //GET AFFECTED CHUNKS
      Dictionary<long, Chunk> modifiedChunks = new Dictionary<long, Chunk>();
      for (int cx = -1; cx <= prefab.size.x + 16; cx = cx + 16)
      {
        for (int cz = -1; cz <= prefab.size.z + 16; cz = cz + 16)
        {
          if (GameManager.Instance.World.IsChunkAreaLoaded(x + cx, y, z + cz))
          {
            Chunk _chunk = GameManager.Instance.World.GetChunkFromWorldPos(x + cx, y, z + cz) as Chunk;
            if (!modifiedChunks.ContainsKey(_chunk.Key))
            {
              modifiedChunks.Add(_chunk.Key, _chunk);
            }
          }
          else
          {
            // todo: generate and observe chunks required

            //var mapVisitor = new MapVisitor(new Vector3i(x, 0, z), new Vector3i(x + prefab.size.x, 0, z + prefab.size.z));
            //mapVisitor.OnVisitChunk += new MapVisitor.VisitChunkDelegate(CreateUndoChunkBlocks);
            //mapVisitor.OnVisitChunk += new MapVisitor.VisitChunkDelegate(CreateUndoChunkTileEntities);
            //mapVisitor.OnVisitChunk += new MapVisitor.VisitChunkDelegate(CreateUndoChunkSleepers);
            //mapVisitor.OnVisitChunk += new MapVisitor.VisitChunkDelegate(UpdateChunkBlocks);
            //mapVisitor.OnVisitMapDone += new MapVisitor.VisitMapDoneDelegate(ReloadChunksForClients);
            //mapVisitor.Start();

            SdtdConsole.Instance.Output("Unable to load chunk for prefab @ " + (x + cx) + "," + (z + cz));
          }
        }
      }

      //INSERT PREFAB
      prefab.CopyIntoLocal(GameManager.Instance.World.ChunkCache, pos, true, true);
      
      //RELOAD CHUNKS
      BCChunks.ReloadForClients(modifiedChunks);
    }

    private void CreateUndo(EntityPlayer sender, Vector3i size, Vector3i pos)
    {
      string steamId = "_server";
      if (_senderInfo.RemoteClientInfo != null)
      {
        steamId = _senderInfo.RemoteClientInfo.ownerId.ToString();
      }

      Prefab _areaCache = new Prefab();
      int _userID = 0; // id will be 0 for web console issued commands
      _areaCache.CopyFromWorld(GameManager.Instance.World, pos, new Vector3i(pos.x + size.x, pos.y + size.y, pos.z + size.z));
      _areaCache.bCopyAirBlocks = true;

      if (sender != null)
      {
        _userID = sender.entityId;
      }
      string _filename = steamId + "_" + DateTime.Now.ToFileTime().ToString();
      Directory.CreateDirectory(Utils.GetGameDir("Data/Prefabs/BCM"));
      _areaCache.Save(Utils.GetGameDir("Data/Prefabs/BCM"), _filename);

      if (_cache.ContainsKey(_userID))
      {
        var pl = _cache[_userID];
        pl.Add(new PrefabCache { filename = _filename, pos = pos });
      }
      else
      {
        _cache.Add(_userID, new List<PrefabCache> { new PrefabCache { filename = _filename, pos = pos } });
      }
    }

    private void UndoInsert(EntityPlayer sender)
    {
      string dirbase = "Data/Prefabs/BCM";
      int _userID = 0;
      if (sender != null)
      {
        _userID = sender.entityId;
      }
      if (_cache.ContainsKey(_userID))
      {
        //history exists
        if (_cache[_userID].Count > 0)
        {
          var prefabCache = _cache[_userID][_cache[_userID].Count - 1];
          if (prefabCache != null)
          {
            Prefab _p = new Prefab();
            _p.Load(Utils.GetGameDir(dirbase), prefabCache.filename);
            InsertPrefab(_p, prefabCache.pos.x, prefabCache.pos.y, prefabCache.pos.z, prefabCache.pos);

            //workaround for multi dim blocks, insert undo prefab twice
            //todo: clear all blocks (turn to air) before inserting the prefab instead
            InsertPrefab(_p, prefabCache.pos.x, prefabCache.pos.y, prefabCache.pos.z, prefabCache.pos);
            if (Utils.FileExists(Utils.GetGameDir(dirbase + prefabCache.filename + ".tts")))
            {
              Utils.FileDelete(Utils.GetGameDir(dirbase + prefabCache.filename + ".tts"));
            }
            if (Utils.FileExists(Utils.GetGameDir(dirbase + prefabCache.filename + ".xml")))
            {
              Utils.FileDelete(Utils.GetGameDir(dirbase + prefabCache.filename + ".xml"));
            }
          }
          _cache[_userID].RemoveAt(_cache[_userID].Count - 1);
        }
      }
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
      if (_senderInfo.RemoteClientInfo != null)
      {
        sender = GameManager.Instance.World.Entities.dict[_senderInfo.RemoteClientInfo.entityId] as EntityPlayer;
      }

      if (_options.ContainsKey("undo"))
      {
        UndoInsert(sender);
        return;
      }

      if (_params.Count > 0)
      {
        Prefab prefab = new Prefab();
        if (prefab.Load(_params[0]))
        {
          int rot = 0;
          int x = 0, y = 0, z = 0;
          Vector3i pos = new Vector3i(0, 0, 0);

          if (!GetXYZPreEnt(prefab, sender, ref x, ref y, ref z) && _params.Count < 5)
          {
            SendOutput("Command requires <name> <x> <y> <z> <rot> params if not sent by an online player");
            return;
          }
          if (!GetXYZRotSpin(prefab, ref rot, ref x, ref y, ref z))
          {
            return;
          }

          // todo: create an entity observer and spawn prefab once chunks are loaded?

          if (_options.ContainsKey("nooffset"))
          {
            pos = new Vector3i(x, y, z);
          }
          else
          {
            pos = new Vector3i(x, y + prefab.yOffset, z);
          }

          if (_options.ContainsKey("air"))
          {
            prefab.bCopyAirBlocks = true;
          }
          if (_options.ContainsKey("noair"))
          {
            prefab.bCopyAirBlocks = false;
          }
          if (_options.ContainsKey("sleepers"))
          {
            prefab.bSleeperVolumes = true;
          }
          if (_options.ContainsKey("nosleepers"))
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
          if (!_options.ContainsKey("noundo"))
          {
            CreateUndo(sender, prefab.size, pos);
          }

          // SPAWN PREFAB
          Log.Out(Config.ModPrefix + "Spawning prefab " + prefab.filename + " @ " + pos + ", size=" + prefab.size);
          SdtdConsole.Instance.Output("Spawning prefab " + prefab.filename + " @ " + pos + ", size=" + prefab.size);
          SdtdConsole.Instance.Output("use bc-import /undo to revert the changes");

          InsertPrefab(prefab, x, y, z, pos);

        }
        else
        {
          // todo: list prefabs, _params[0] as a filter
        }
      }
      else
      {
        // todo: list prefabs
      }
    }
  }
}
