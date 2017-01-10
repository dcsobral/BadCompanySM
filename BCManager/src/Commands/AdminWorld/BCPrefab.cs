using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using UnityEngine;

namespace BCM.Commands
{
  public class BCPrefab : BCCommandAbstract
  {
    public override void Process()
    {
      EntityPlayer sender = GameManager.Instance.World.Entities.dict[_senderInfo.RemoteClientInfo.entityId] as EntityPlayer;
      //ChunkProviderGenerateWorldRandom2 _chunkProvider = new ChunkProviderGenerateWorldRandom2(GameManager.Instance.World.ChunkCache, GameManager.Instance.World.GetWorldCreationData(), GamePrefs.GetString(EnumGamePrefs.GameWorld));

      if (_params.Count > 0)
      {
        Prefab prefab = new Prefab();
        if (prefab.Load(_params[0]))
        {
          Vector3i pos = new Vector3i(0, 0, 0);
          int rot = 0;
          int x = (int)sender.position.x - (prefab.size.x / 2);
          int y = (int)sender.position.y;
          int z = (int)sender.position.z - (prefab.size.z / 2);
          if (_options.ContainsKey("cornersw") || _options.ContainsKey("ne"))
          {
            x = (int)sender.position.x;
            z = (int)sender.position.z;
          }
          else if (_options.ContainsKey("cornerse") || _options.ContainsKey("nw"))
          {
            x = (int)sender.position.x - prefab.size.x;
            z = (int)sender.position.z;
          }
          else if (_options.ContainsKey("cornernw") || _options.ContainsKey("se"))
          {
            x = (int)sender.position.x;
            z = (int)sender.position.z - prefab.size.z;
          }
          else if (_options.ContainsKey("cornerne") || _options.ContainsKey("sw"))
          {
            x = (int)sender.position.x - prefab.size.x;
            z = (int)sender.position.z - prefab.size.z;
          }

          if (_params.Count > 1)
          {
            if (_params.Count == 2)
            {
              int.TryParse(_params[1], out rot);
            }

            // specific spawnpoint
            if (_params.Count == 5)
            {
              int.TryParse(_params[1], out x);
              int.TryParse(_params[2], out y);
              int.TryParse(_params[3], out z);
              int.TryParse(_params[4], out rot);
            }

            // spin the prefab
            for (int r = 0; r < rot % 4; r++)
            {
              prefab.RotateY(true);
            }

          }

          if (y < 3)
          {
            y = 3;
          }
          if (y + prefab.size.y > 255)
          {
            y = 255 - prefab.size.y;
          }


          // todo: spawn at position x,z with y=average terrain height?
          // todo: create an entity observer and spawn prefab once chunks are loaded?
          // todo: allow for partial names for prefab, provide list if more then one result, allow for partial + # from list to specify
          // todo: refresh nearest prefab

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

          Dictionary<long, Chunk> chunks = new Dictionary<long, Chunk>();
          for (int cx = -1; cx <= prefab.size.x + 16; cx = cx + 16)
          {
            for (int cz = -1; cz <= prefab.size.z + 16; cz = cz + 16)
            {
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
                // todo: give warning and exit. require /force option to load prefab with chunks missing
                SdtdConsole.Instance.Output("Unable to load chunk for prefab @ " + (x + cx) + "," + (z + cz));
              }
            }
          }

          //if (_options.ContainsKey("deco"))
          //{
          //  prefab.bAllowTopSoilDecorations = true;
          //}
          //if (_options.ContainsKey("nodeco"))
          //{
          //  prefab.bAllowTopSoilDecorations = false;
          //}
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
          // todo: copy entities to world, add spawner?
          // todo: option to carve terrain where prefab will spawn, maybe reapply decorations?


          // todo: debug NRE's
          //// LOOT PLACEHOLDERS
          //LootPlaceholderMap lootPlaceholderMap = LootContainer.lootPlaceholderMap;
          //for (int px = 0; px < prefab.size.x; px++)
          //{
          //  for (int py = 0; py < prefab.size.y; py++)
          //  {
          //    for (int pz = 0; pz < prefab.size.z; pz++)
          //    {
          //      BlockValue bv = prefab.GetBlock(px, py, pz);
          //      BlockValue bvr = lootPlaceholderMap.Replace(bv, new System.Random());
          //      if (bv.type != bvr.type)
          //      {
          //        prefab.SetBlock(x, y, z, bvr);
          //      }
          //    }
          //  }
          //}

          // SPAWN PREFAB
          Log.Out(Config.ModPrefix + "Spawning prefab " + prefab.filename + " @ " + pos + ", size=" + prefab.size);
          SdtdConsole.Instance.Output("Spawning prefab " + prefab.filename + " @ " + pos + ", size=" + prefab.size);
          prefab.CopyIntoLocal(GameManager.Instance.World.ChunkCache, pos, true, true);


          // todo: check - does this do anything?
          StabilityInitializer _si = new StabilityInitializer(GameManager.Instance.World);
          foreach (Chunk _chunk in chunks.Values)
          {
            _si.DistributeStability(_chunk);
          }


          // todo: confirm - a list of chunks loaded, is it for the server or just that player?
          HashSet<long> cl = sender.ChunkObserver.chunksLoaded;
          foreach (long l in cl)
          {
            //Log.Out("long:" + l.ToString());
            Chunk cs = GameManager.Instance.World.GetChunkSync(l) as Chunk;
            //Log.Out("cs:" + cs.X.ToString() + " " + cs.Z.ToString());
          }


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
          }

          // todo: if not in god mode, and location is not air blocks then teleport to same location with y=-1

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
