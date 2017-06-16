using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using UnityEngine;

namespace BCM.Commands
{
  public class BCPrefab : BCCommandAbstract
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
      if (_e == null)
      {
        return false;
      }
      _x = (int)_e.position.x - (_p.size.x / 2);
      _y = (int)_e.position.y;
      _z = (int)_e.position.z - (_p.size.z / 2);
      if (_options.ContainsKey("cornersw") || _options.ContainsKey("ne"))
      {
        _x = (int)_e.position.x;
        _z = (int)_e.position.z;
      }
      else if (_options.ContainsKey("cornerse") || _options.ContainsKey("nw"))
      {
        _x = (int)_e.position.x - _p.size.x;
        _z = (int)_e.position.z;
      }
      else if (_options.ContainsKey("cornernw") || _options.ContainsKey("se"))
      {
        _x = (int)_e.position.x;
        _z = (int)_e.position.z - _p.size.z;
      }
      else if (_options.ContainsKey("cornerne") || _options.ContainsKey("sw"))
      {
        _x = (int)_e.position.x - _p.size.x;
        _z = (int)_e.position.z - _p.size.z;
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

        // specific spawnpoint
        if (_params.Count == 5)
        {
          if (!int.TryParse(_params[1], out _x) || !int.TryParse(_params[2], out _y) || !int.TryParse(_params[3], out _z) || !int.TryParse(_params[4], out _r))
          {
            SendOutput("One of <x> <y> <z> <rot> params could not be parsed as a number.");
            return false;
          }
        }

        // spin the prefab
        for (int r = 0; r < _r % 4; r++)
        {
          _prefab.RotateY(false);
        }
      }

      //bounds
      //todo: make overridable (prefab wont replace blocks outside bounds but will partial spawn)
      if (_y < 3)
      {
        _y = 3;
      }
      if (_y + _prefab.size.y > 255)
      {
        _y = 255 - _prefab.size.y;
      }

      return true;
    }

    private static void BlockTranslations(Prefab prefab)
    {
      //BLOCK TRANSLATIONS
      LootPlaceholderMap _map = LootContainer.lootPlaceholderMap;
      for (int px = 0; px < prefab.size.x; px++)
      {
        for (int py = 0; py < prefab.size.y; py++)
        {
          for (int pz = 0; pz < prefab.size.z; pz++)
          {
            BlockValue bv = prefab.GetBlock(px, py, pz);
            // todo: copy entities to world?
            // ENTITIES


            // LOOT PLACEHOLDERS
            // todo: custom block map via configs and select with /map=<mapname>
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

    private static void InsertPrefab(Prefab prefab, int x, int y, int z, Vector3i pos)
    {
      if (prefab == null)
      {
        SdtdConsole.Instance.Output("No Prefab loaded.");
        return;
      }

      //GET AFFECTED CHUNKS
      Dictionary<long, Chunk> chunks = new Dictionary<long, Chunk>();
      for (int cx = -1; cx <= prefab.size.x + 16; cx = cx + 16)
      {
        for (int cz = -1; cz <= prefab.size.z + 16; cz = cz + 16)
        {
          // todo: pre generate chunks required
          if (GameManager.Instance.World.IsChunkAreaLoaded(x + cx, y, z + cz))
          {
            Chunk _chunk = GameManager.Instance.World.GetChunkFromWorldPos(x + cx, y, z + cz) as Chunk;
            if (!chunks.ContainsKey(_chunk.Key))
            {
              chunks.Add(_chunk.Key, _chunk);
            }
          }
          else
          {
            SdtdConsole.Instance.Output("Unable to load chunk for prefab @ " + (x + cx) + "," + (z + cz));
          }
        }
      }

      //INSERT PREFAB
      prefab.CopyIntoLocal(GameManager.Instance.World.ChunkCache, pos, true, true);

      //RESET STABILITY
      StabilityInitializer _si = new StabilityInitializer(GameManager.Instance.World);
      foreach (Chunk _chunk in chunks.Values)
      {
        _chunk.ResetStability();
      }
      foreach (Chunk _chunk in chunks.Values)
      {
        _si.DistributeStability(_chunk);
        _chunk.NeedsRegeneration = true;
      }

      // todo: confirm - a list of chunks loaded, is it for the server or just that player?
      //HashSet<long> cl = sender.ChunkObserver.chunksLoaded;
      //foreach (long l in cl)
      //{
      //  //Log.Out("long:" + l.ToString());
      //  Chunk cs = GameManager.Instance.World.GetChunkSync(l) as Chunk;
      //  //Log.Out("cs:" + cs.X.ToString() + " " + cs.Z.ToString());
      //}

      // REFRESH CLIENTS CHUNKS
      List<ClientInfo> clients = ConnectionManager.Instance.GetClients();
      List<ClientInfo> reloadforclients = new List<ClientInfo>();
      foreach (ClientInfo client in clients)
      {
        EntityPlayer clientEntity = GameManager.Instance.World.Entities.dict[client.entityId] as EntityPlayer;
        float distance = Vector3.Distance(clientEntity.position, pos.ToVector3());
        // todo: change to use clientEntity.ChunkObserver.chunksLoaded list to see if client has chunk loaded and requires refresh
        if (distance < 200)
        {
          reloadforclients.Add(client);
          //Log.Out(Config.ModPrefix + " Reloading " + chunks.Count + " chunks for " + client.playerName + ":" + distance);
          SdtdConsole.Instance.Output(Config.ModPrefix + " Reloading " + chunks.Count + " chunks for " + client.playerName + ":" + distance);
          foreach (Chunk _chunk in chunks.Values)
          {
            client.SendPackage(new NetPackageChunkRemove(_chunk.Key));
          }
        }
      }

      // delay to allow packets to reach clients
      Thread.Sleep(50);
      foreach (ClientInfo client in reloadforclients)
      {
        foreach (Chunk _chunk in chunks.Values)
        {
          client.SendPackage(new NetPackageChunk(_chunk));
        }
        //todo: after chunks have loaded on client need to check for falling and telelport on top.
        //Vector3 clientpos = GameManager.Instance.World.Players.dict[client.entityId].position;
        //clientpos.y = -1;
        //NetPackageTeleportPlayer netPackageTeleportPlayer = new NetPackageTeleportPlayer(clientpos);
        //client.SendPackage(netPackageTeleportPlayer);
      }
    }

    public override void Process()
    {
      // todo: allow for partial names for prefab, provide list if more then one result, allow for partial + # from list to specify
      // todo: refresh nearest prefab
      // todo: store last x commands for each player, with linked undo data add a repeat option so that prefab inserts at a players location can be repeated at the same location even if player moves

      EntityPlayer sender = null;
      string steamId = "_server";
      if (_senderInfo.RemoteClientInfo != null)
      {
        sender = GameManager.Instance.World.Entities.dict[_senderInfo.RemoteClientInfo.entityId] as EntityPlayer;
        steamId = _senderInfo.RemoteClientInfo.ownerId.ToString();
      }

      if (_options.ContainsKey("undo"))
      {
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
              _p.Load(Utils.GetGameDir("Data/Prefabs/BCM"), prefabCache.filename);
              InsertPrefab(_p, prefabCache.pos.x, prefabCache.pos.y, prefabCache.pos.z, prefabCache.pos);
            }
            _cache[_userID].RemoveAt(_cache[_userID].Count - 1);
            if (Utils.FileExists(Utils.GetGameDir("Data/Prefabs/BCM/" + prefabCache.filename + ".tts")))
            {
              Utils.FileDelete(Utils.GetGameDir("Data/Prefabs/BCM/" + prefabCache.filename + ".tts"));
            }
            if (Utils.FileExists(Utils.GetGameDir("Data/Prefabs/BCM/" + prefabCache.filename + ".xml")))
            {
              Utils.FileDelete(Utils.GetGameDir("Data/Prefabs/BCM/" + prefabCache.filename + ".xml"));
            }
          }
        }
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

          //todo: is this needed? seems broken
          //bool bPhysicsActive = GameManager.bPhysicsActive;
          //GameManager.bPhysicsActive = false;

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


          if (_options.ContainsKey("offset"))
          {
            //Chunk c = GameManager.Instance.World.GetChunkFromWorldPos(x, y, z) as Chunk;
            //int h = c.GetTerrainHeight(0, 0);
            pos = new Vector3i(x, y + prefab.yOffset, z);
          }
          else
          {
            pos = new Vector3i(x, y, z);
          }

          if (_options.ContainsKey("air"))
          {
            prefab.bCopyAirBlocks = true;
          }
          if (_options.ContainsKey("noair"))
          {
            prefab.bCopyAirBlocks = false;
          }

          // todo: create a copy of the chunks and the bounded dimensions of the prefab size for an undo
          //       should work better than a prefab copy undo as it will preserve block ownership and state?
          // todo: option to carve terrain where prefab will spawn, maybe reapply decorations?
          //       insert lot into cell data and regenerate chunk area from world/seed defaults

          BlockTranslations(prefab);

          //UNDO
          //create backup of area prefab will insert to
          Prefab _areaCache = new Prefab();

          if (!_options.ContainsKey("noundo"))
          {
            int _userID = 0; // id will be 0 for web console issued commands
            _areaCache.CopyFromWorld(GameManager.Instance.World, pos, new Vector3i(pos.x + prefab.size.x, pos.y + prefab.size.y, pos.z + prefab.size.z));
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

          // SPAWN PREFAB
          Log.Out(Config.ModPrefix + "Spawning prefab " + prefab.filename + " @ " + pos + ", size=" + prefab.size);
          SdtdConsole.Instance.Output("Spawning prefab " + prefab.filename + " @ " + pos + ", size=" + prefab.size);

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

      //DynamicPrefabDecorator dynamicPrefabDecorator = GameManager.Instance.GetDynamicPrefabDecorator();
      //Dictionary<string, Prefab> prefabDict = dynamicPrefabDecorator.GetAllPrefabs(); // a list of prefabs currently loaded in chunks
      //foreach (Prefab prefab in prefabDict.Values)
      //{
      //  SdtdConsole.Instance.Output(prefab.filename);
      //}
      //dynamicPrefabDecorator.RemovePrefab(GameManager.Instance.World, prefab, true);
      //dynamicPrefabDecorator.CopyPrefabIntoWorld(GameManager.Instance.World, prefab, prefab.lastCopiedPrefabPosition, 0);//prefab.rotation
    }

  }
}
