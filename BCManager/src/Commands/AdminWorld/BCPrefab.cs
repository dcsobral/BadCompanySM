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
      EntityPlayer sender = null;
      if (_senderInfo.RemoteClientInfo != null)
      {
        sender = GameManager.Instance.World.Entities.dict[_senderInfo.RemoteClientInfo.entityId] as EntityPlayer;
      }
      //ChunkProviderGenerateWorldRandom2 _chunkProvider = new ChunkProviderGenerateWorldRandom2(GameManager.Instance.World.ChunkCache, GameManager.Instance.World.GetWorldCreationData(), GamePrefs.GetString(EnumGamePrefs.GameWorld));
      //_chunkProvider.RequestChunk(_x, _y);
      //_chunkProvider.DoGenerateChunks();

      if (_params.Count > 0)
      {

        Prefab prefab = new Prefab();
        if (prefab.Load(_params[0]))
        {
          bool bPhysicsActive = GameManager.bPhysicsActive;
          GameManager.bPhysicsActive = false;

          Vector3i pos = new Vector3i(0, 0, 0);
          int rot = 0;
          int x = 0;
          int y = 0;
          int z = 0;
          if (sender != null)
          {
            x = (int)sender.position.x - (prefab.size.x / 2);
            y = (int)sender.position.y;
            z = (int)sender.position.z - (prefab.size.z / 2);
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
          } else if (_params.Count < 5)
          {
            Log.Out(Config.ModPrefix + " Command requires name x y z rot params if not sent by an online player");
            return;
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
              prefab.RotateY(false);
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

          // todo: create an entity observer and spawn prefab once chunks are loaded?
          
          // todo: define a box first, then spawn prefab centered on middle of the box
          // todo: spawn at position x,z with y=average terrain height?
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


          LootPlaceholderMap _map = LootContainer.lootPlaceholderMap;
          for (int px = 0; px < prefab.size.x; px++)
          {
            for (int py = 0; py < prefab.size.y; py++)
            {
              for (int pz = 0; pz < prefab.size.z; pz++)
              {
                BlockValue bv = prefab.GetBlock(px, py, pz);
                // todo: copy entities to world, add dynamic spawner?
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


          // SPAWN PREFAB
          Log.Out(Config.ModPrefix + "Spawning prefab " + prefab.filename + " @ " + pos + ", size=" + prefab.size);
          SdtdConsole.Instance.Output("Spawning prefab " + prefab.filename + " @ " + pos + ", size=" + prefab.size);
          prefab.CopyIntoLocal(GameManager.Instance.World.ChunkCache, pos, true, true);


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

          GameManager.bPhysicsActive = bPhysicsActive;
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
