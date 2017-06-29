using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using UnityEngine;

namespace BCM.Commands
{
  public class BCWorldBlocks : BCCommandAbstract
  {
    public class PrefabCache
    {
      public string filename;
      public Vector3i pos;
    }

    public Dictionary<int, List<PrefabCache>> _cache = new Dictionary<int, List<PrefabCache>>();


    private void CreateUndo(EntityPlayer sender, Prefab prefab, Vector3i pos)
    {
      string steamId = "_server";
      if (_senderInfo.RemoteClientInfo != null)
      {
        steamId = _senderInfo.RemoteClientInfo.ownerId.ToString();
      }

      Prefab _areaCache = new Prefab();
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
            BCPrefab.InsertPrefab(_p, prefabCache.pos.x, prefabCache.pos.y, prefabCache.pos.z, prefabCache.pos);

            //workaround for multi dim blocks, insert undo prefab twice
            //todo: clear all blocks (turn to air) before inserting the prefab instead
            BCPrefab.InsertPrefab(_p, prefabCache.pos.x, prefabCache.pos.y, prefabCache.pos.z, prefabCache.pos);
          }
          _cache[_userID].RemoveAt(_cache[_userID].Count - 1);
          if (Utils.FileExists(Utils.GetGameDir(dirbase + prefabCache.filename + ".tts")))
          {
            Utils.FileDelete(Utils.GetGameDir(dirbase + prefabCache.filename + ".tts"));
          }
          if (Utils.FileExists(Utils.GetGameDir(dirbase + prefabCache.filename + ".xml")))
          {
            Utils.FileDelete(Utils.GetGameDir(dirbase + prefabCache.filename + ".xml"));
          }
        }
      }
    }

    public override void Process()
    {
      Vector3i p1 = new Vector3i(int.MinValue, 0, int.MinValue);
      Vector3i p2 = new Vector3i(int.MinValue, 0, int.MinValue);
      string blockname = null;
      int _blockID = 0;


      //get loc and player current pos
      EntityPlayer sender = null;
      string steamId = null;
      if (_senderInfo.RemoteClientInfo != null)
      {
        steamId = _senderInfo.RemoteClientInfo.ownerId.ToString();
        sender = GameManager.Instance.World.Entities.dict[_senderInfo.RemoteClientInfo.entityId] as EntityPlayer;
        if (sender != null)
        {
          p2 = new Vector3i((int)Math.Floor(sender.serverPos.x / 32f), (int)Math.Floor(sender.serverPos.y / 32f), (int)Math.Floor(sender.serverPos.z / 32f));
        }
        else
        {
          SendOutput("Error: unable to get player location");

          return;
        }
      }
      else
      {
        SendOutput("Error: unable to get player location");

        return;
      }

      if (_options.ContainsKey("undo"))
      {
        UndoInsert(sender);
        return;
      }

      if (_params.Count == 1)
      {

        if (steamId != null)
        {
          p1 = BCLocation.GetPos(steamId);
          if (p1.x == int.MinValue)
          {
            SendOutput("No location stored. Use bc-loc to store a location.");

            return;
          }

          blockname = _params[0];
        }
        else
        {
          SendOutput("Error: unable to get player location");

          return;
        }
      }
      else if (_params.Count == 7)
      {
        //parse params
        if (!int.TryParse(_params[0], out p1.x) || !int.TryParse(_params[1], out p1.y) || !int.TryParse(_params[2], out p1.z) || !int.TryParse(_params[3], out p2.x) || !int.TryParse(_params[4], out p2.y) || !int.TryParse(_params[5], out p2.z))
        {
          SendOutput("Error: unable to parse coordinates");

          return;
        }
        blockname = _params[6];
      }
      else
      {
        SendOutput("Error: Incorrect command format.");
        SendOutput(GetHelp());

        return;
      }

      Vector3i size = new Vector3i(Math.Abs(p1.x - p2.x) + 1, Math.Abs(p1.y - p2.y) + 1, Math.Abs(p1.z - p2.z) + 1);

      var p3 = new Vector3i(
        (p1.x < p2.x ? p1.x : p2.x),
        (p1.y < p2.y ? p1.y : p2.y),
        (p1.z < p2.z ? p1.z : p2.z)
      );
      var p4 = new Vector3i(
        (p1.x == p2.x ? p1.x + 1 : p1.x > p2.x ? p1.x : p2.x),
        (p1.y == p2.y ? p1.y + 1 : p1.y > p2.y ? p1.y : p2.y),
        (p1.z == p2.z ? p1.z + 1 : p1.z > p2.z ? p1.z : p2.z)
      );


      //Get BlockValue
      BlockValue _bv = BlockValue.Air;
      if (int.TryParse(blockname, out _blockID))
      {
        _bv = Block.GetBlockValue(_blockID);
      }
      else
      {
        _bv = Block.GetBlockValue(blockname);
      }

      //Make prefab
      Prefab _prefab = new Prefab(size);
      if (_options.ContainsKey("swap"))
      {

      }
      else if (_options.ContainsKey("chown"))
      {

      }
      else if (_options.ContainsKey("densify"))
      {
        //options: randomise, smooth, default
      }
      else
      //if (_options.ContainsKey("fill"))
      {
        for (int i = 0; i < size.x; i++)
        {
          for (int j = 0; j < size.y; j++)
          {
            for (int k = 0; k < size.z; k++)
            {
              _prefab.SetBlock(i, j, k, _bv);

              //sbyte density = this.GetDensity(i, k, j);
              Block block2 = Block.list[_bv.type];
              if (block2 == null)
              {
                SdtdConsole.Instance.Output("Unable to find block by id or name");

                return;
              }
              if (block2.shape.IsTerrain())
              {
                _prefab.SetDensity(i, k, j, MarchingCubes.DensityTerrain);//-128
              }
              else
              {
                if (_bv.Equals(BlockValue.Air))
                {
                  _prefab.SetDensity(i, k, j, MarchingCubes.DensityAir);//127
                }
                //_prefab.SetDensity(i, k, j, 1);
              }
              //_prefab.SetTexture(i, j, k, 0);
            }
          }
        }
      }


      //todo: make a function that can be called on Import to place the prefab, returns the undo data to be stored in this func for bc-block /undo
      _prefab.bCopyAirBlocks = true;
      _prefab.bExcludeDistantPOIMesh = true;
      _prefab.distantPOIYOffset = 0;
      _prefab.bAllowTopSoilDecorations = false;
      _prefab.bTraderArea = false;
      _prefab.SleeperVolumesStart = new List<Vector3i>();

      //CREATE UNDO
      //create backup of area prefab will insert to
      if (!_options.ContainsKey("noundo"))
      {
        CreateUndo(sender, _prefab, p3);
      }

      //GET AFFECTED CHUNKS
      Dictionary<long, Chunk> modifiedChunks = new Dictionary<long, Chunk>();
      for (int cx = -1; cx <= _prefab.size.x + 16; cx = cx + 16)
      {
        for (int cz = -1; cz <= _prefab.size.z + 16; cz = cz + 16)
        {
          if (GameManager.Instance.World.IsChunkAreaLoaded(p3.x + cx, p3.y, p3.z + cz))
          {
            Chunk _chunk = GameManager.Instance.World.GetChunkFromWorldPos(p3.x + cx, p3.y, p3.z + cz) as Chunk;
            if (!modifiedChunks.ContainsKey(_chunk.Key))
            {
              modifiedChunks.Add(_chunk.Key, _chunk);
            }
          }
          else
          {
            SdtdConsole.Instance.Output("Unable to load chunk for insert @ " + (p3.x + cx) + "," + (p3.z + cz));
          }
        }
      }

      SendOutput("Inserting block '" + _bv.Block.GetBlockName() + "' @ " + p3 + " to " + p4);
      SendOutput("Use bc-wblock /undo to revert the changes");

      //INSERT PREFAB
      _prefab.CopyIntoLocal(GameManager.Instance.World.ChunkCache, p3, true, true);

      //RELOAD CHUNKS
      BCChunks.ReloadForClients(modifiedChunks);

      //  //bc-wblocks - lists options / help details (see below)
      //  //  <co-ords> = 2x vector3i pos

      //  //bc-wblocks <co-ords> /fill <blockid> - fills the area with the specified block
      //  //bc-wblocks <co-ords> /fill <blockid> [/face0=textureId] [/face1=textureId] [/face2=textureId] [/face3=textureId] [/face4=textureId] [/face5=textureId]
      //  //bc-wblocks <co-ords> /swap <targetblockid> <replacementblockid> [/face0=textureId] [/face1=textureId] [/face2=textureId] [/face3=textureId] [/face4=textureId] [/face5=textureId]
      //  //bc-wblocks <co-ords> /chown <entityid> (or /self) - sets the owner of the tileentity blocks and land claims in the area to the entityid listed (or the commander if /self used)
      //  //bc-wblocks <co-ords> /densify <density> - set the density of blocks within the area. If <density> is neg: set terrain blocks, positive: set cube blocks (or is that reversed?)
      //  //  /noair - option means it will only swap the blocks if original block is not air (applies to /fill and /swap)
      //  //  /nowet - option means it will only swap the blocks if original block is not water (applies to /fill and /swap)
      //  //  /noclaim - skips processing of claim blocks in target area
      //  //  /random=1,2,3,4,5 a list of blocks to use for /fill and /swap to randomly replace blocks in target area
      //  //  /circle - instead of two vector3i provide a single vector3i and an inner+outer radius and height, optionally a pair of values for arc degrees
      //  //  /prefab - a prefab to insert repeatedly within the area


      //  //loc - shows player current location (/worldpos /etc), and sets the pos for prafab and block commands
      //  //bc-rb, bc-renderblocks, bc-block, block
      //  //bc-block upgrade - upgrades the blocks 1 step within an area
      //  //bc-block repair - repairs all blocks in the area
      //  //bc-block swap - swaps source block with target block for blocks in the area
      //  //bc-block randomdam - randomly damages all blocks in the area
      //  //bc-block downgrade /nodestroy - downgrades all blocks in area, optional for no destroy but only those not on the last stage
      //  //bc-block insert /fill=terrain /fill=air /fill=cube[/texture=0,1,2,3,4,5] [default]/fill=all - changes all blocks in an area with the block specified, options act as filters what blocks get replaced in the target area
      //  //bc-block prefab /nopartial /2d - renders a prefab in the area defined repeating the prefab to fill the area, optional on nopartial to prevent the insertion of partial prefabs at the edges of the area. 2d optional to only draw 1 layer of prefabs rather than stacking them (default)
      //  //bc-chunk reset - resets the chunk to its rwg original state
      //  //bc-chunk reload <player> - reloads the chunks in that players loaded chunk list




    }
  }
}
