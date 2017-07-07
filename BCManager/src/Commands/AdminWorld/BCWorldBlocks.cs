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
            BCImport.InsertPrefab(_p, prefabCache.pos.x, prefabCache.pos.y, prefabCache.pos.z, prefabCache.pos);

            //workaround for multi dim blocks, insert undo prefab twice
            //todo: clear all blocks (turn to air) before inserting the prefab instead?
            //      multidim blocks with parent blocks outside the area need to be stored in undo data and then removed
            //      other multidim blocks need to have checks for child block and if found remove parent and all children 
            BCImport.InsertPrefab(_p, prefabCache.pos.x, prefabCache.pos.y, prefabCache.pos.z, prefabCache.pos);
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
      //todo: multidim blocks
      //      claim stones replaced need to be removed from persistent data.
      //      map visitor for unloaded chunks
      //todo: damage
      Vector3i p1 = new Vector3i(int.MinValue, 0, int.MinValue);
      Vector3i p2 = new Vector3i(int.MinValue, 0, int.MinValue);
      string blockname = null;
      string blockname2 = null;
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

      if (_params.Count == 1 || _params.Count == 2)
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
          if (_params.Count == 2)
          {
            blockname2 = _params[1];
          }
        }
        else
        {
          SendOutput("Error: unable to get player location");

          return;
        }
      }
      else if (_params.Count == 7 || _params.Count == 8)
      {
        //parse params
        if (!int.TryParse(_params[0], out p1.x) || !int.TryParse(_params[1], out p1.y) || !int.TryParse(_params[2], out p1.z) || !int.TryParse(_params[3], out p2.x) || !int.TryParse(_params[4], out p2.y) || !int.TryParse(_params[5], out p2.z))
        {
          SendOutput("Error: unable to parse coordinates");

          return;
        }
        blockname = _params[6];
        if (_params.Count == 8)
        {
          blockname2 = _params[7];
        }
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
      //todo: check that can just use p3 + size instead
      var p4 = new Vector3i(
        (p1.x == p2.x ? p1.x + 1 : p1.x > p2.x ? p1.x : p2.x),
        (p1.y == p2.y ? p1.y + 1 : p1.y > p2.y ? p1.y : p2.y),
        (p1.z == p2.z ? p1.z + 1 : p1.z > p2.z ? p1.z : p2.z)
      );

      //**************** GET BLOCKVALUE
      BlockValue _bv = BlockValue.Air;
      if (int.TryParse(blockname, out _blockID))
      {
        _bv = Block.GetBlockValue(_blockID);
      }
      else
      {
        _bv = Block.GetBlockValue(blockname);
      }

      //**************** CREATE PREFAB
      //Prefab _prefab = new Prefab(size);
      //_prefab.bCopyAirBlocks = true;
      //_prefab.bExcludeDistantPOIMesh = true;
      //_prefab.distantPOIYOffset = 0;
      //_prefab.bAllowTopSoilDecorations = false;
      //_prefab.bTraderArea = false;
      //_prefab.SleeperVolumesStart = new List<Vector3i>();

      Dictionary<long, Chunk> modifiedChunks = GetAffectedChunks(p3, size);

      //CREATE UNDO
      //create backup of area prefab will insert to
      if (!_options.ContainsKey("noundo"))
      {
        //todo: use BlockTools.CopyIntoStorage to get prefab, then save to cache
        CreateUndo(sender, size, p3);
      }

      if (_options.ContainsKey("swap"))
      {
        if (!SwapBlocks(size, p3, p4, _bv, blockname2, modifiedChunks)) { return; }
      }
      else if (_options.ContainsKey("chown"))
      {

      }
      else if (_options.ContainsKey("densify"))
      {
        //options: randomise, smooth, default
      }
      else if (_options.ContainsKey("scan"))
      {
        if (!ScanBlocks(size, p3, _bv)) { return; }
      }
      else //if (_options.ContainsKey("fill"))
      {
        if (!FillBlocks(size, p3, p4, _bv, /*_prefab, */modifiedChunks)) { return; }
      }
    }

    private static Dictionary<long, Chunk> GetAffectedChunks(Vector3i p3, Vector3i size)
    {
      //GET AFFECTED CHUNKS
      Dictionary<long, Chunk> modifiedChunks = new Dictionary<long, Chunk>();
      for (int cx = -1; cx <= size.x + 16; cx = cx + 16)
      {
        for (int cz = -1; cz <= size.z + 16; cz = cz + 16)
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
            //GameManager.Instance.World.m_ChunkManager.ReloadAllChunks();
            //var key = WorldChunkCache.MakeChunkKey(World.toChunkXZ(p3.x), World.toChunkXZ(p3.z));
            SdtdConsole.Instance.Output("Unable to load chunk for insert @ " + (p3.x + cx) + "," + (p3.z + cz));
          }
        }
      }

      return modifiedChunks;
    }

    private bool SwapBlocks(Vector3i size, Vector3i p3, Vector3i p4, BlockValue _newbv, string blockname, Dictionary<long, Chunk> modifiedChunks)
    {
      int _blockID = 0;
      BlockValue _targetbv = BlockValue.Air;
      if (int.TryParse(blockname, out _blockID))
      {
        _targetbv = Block.GetBlockValue(_blockID);
      }
      else
      {
        _targetbv = Block.GetBlockValue(blockname);
      }

      var _clrIdx = 0;
      var counter = 0;

      //todo: fix this code
      Block block1 = Block.list[_targetbv.type];
      if (block1 == null)
      {
        SdtdConsole.Instance.Output("Unable to find target block by id or name");

        return false;
      }

      Block block2 = Block.list[_newbv.type];
      if (block2 == null)
      {
        SdtdConsole.Instance.Output("Unable to find replacement block by id or name");

        return false;
      }
      
      for (int i = 0; i < size.x; i++)
      {
        for (int j = 0; j < size.y; j++)
        {
          for (int k = 0; k < size.z; k++)
          {
            sbyte _density = 1;
            long _textureFull = 0L;

            if (_newbv.Equals(BlockValue.Air))
            {
              _density = MarchingCubes.DensityAir;
            }
            else if (block1.shape.IsTerrain())
            {
              _density = MarchingCubes.DensityTerrain;
            }
            else if (!block1.shape.IsTerrain())
            {
              _density = 1;
            }

            Vector3i p5 = new Vector3i(i + p3.x, j + p3.y, k + p3.z);
            if (GameManager.Instance.World.GetBlock(p5).Block.GetBlockName() == block1.GetBlockName())
            {
              GameManager.Instance.World.SetBlock(_clrIdx, p5, _newbv, false, false);
              GameManager.Instance.World.SetDensity(_clrIdx, p5, _density, false);
              GameManager.Instance.World.SetTexture(_clrIdx, p5.x, p5.y, p5.z, _textureFull);
              counter++;
            }
          }
        }
      }

      SendOutput("Replaced " + counter + " '" + block1.GetBlockName() + "' blocks with '" + block2.GetBlockName() + "' @ " + p3 + " to " + p4);
      SendOutput("Use bc-wblock /undo to revert the changes");

      //INSERT PREFAB
      //_prefab.CopyIntoLocal(GameManager.Instance.World.ChunkCache, p3, true, true);

      //RELOAD CHUNKS
      BCChunks.ReloadForClients(modifiedChunks);

      return true;
    }

    private bool FillBlocks(Vector3i size, Vector3i p3, Vector3i p4, BlockValue _bv, Dictionary<long, Chunk> modifiedChunks)
    {
      var _clrIdx = 0;

      Block block1 = Block.list[_bv.type];
      if (block1 == null)
      {
        SdtdConsole.Instance.Output("Unable to find block by id or name");

        return false;
      }

      //MULTIDIM REMOVE
      for (int i = 0; i < size.x; i++)
      {
        for (int k = 0; k < size.z; k++)
        {
          for (int j = 0; j < size.y; j++) {
            Vector3i p5 = new Vector3i(i + p3.x, j + p3.y, k + p3.z);

            Chunk _chunk = GameManager.Instance.World.GetChunkFromWorldPos(p5) as Chunk;
            var _bv_curr = GameManager.Instance.World.GetBlock(p5);
            if (!_bv_curr.ischild)
            {
              _bv_curr.Block.shape.OnBlockRemoved(GameManager.Instance.World, _chunk, p5, _bv_curr);
              if (_bv_curr.Block.isMultiBlock)
              {
                _bv_curr.Block.multiBlockPos.RemoveChilds(GameManager.Instance.World, _chunk.ClrIdx, p5, _bv_curr);
              }
            }
            else if (_bv_curr.Block.isMultiBlock)
            {
              _bv_curr.Block.multiBlockPos.RemoveParentBlock(GameManager.Instance.World, _chunk.ClrIdx, p5, _bv_curr);
            }
            //todo: need to store these blocks in undo data if parent is outside area
          }
        }
      }
      
      //repeat process twice because of issues with multidim
      for (int a = 0; a < 2; a++)
      {
        for (int j = 0; j < size.y; j++)
        {
          for (int i = 0; i < size.x; i++)
          {
            for (int k = 0; k < size.z; k++)
            {
              sbyte _density = 1;
              long _textureFull = 0L;

              if (_bv.Equals(BlockValue.Air))
              {
                _density = MarchingCubes.DensityAir;
              }
              else if (block1.shape.IsTerrain())
              {
                _density = MarchingCubes.DensityTerrain;
              }
              else if (!block1.shape.IsTerrain())
              {
                _density = 1;
              }

              Vector3i p5 = new Vector3i(i + p3.x, j + p3.y, k + p3.z);
              GameManager.Instance.World.SetBlock(_clrIdx, p5, _bv, false, false);
              GameManager.Instance.World.SetDensity(_clrIdx, p5, _density, false);
              GameManager.Instance.World.SetTexture(_clrIdx, p5.x, p5.y, p5.z, _textureFull);
            }
          }
        }
      }

      SendOutput("Inserting block '" + block1.GetBlockName() + "' @ " + p3 + " to " + p4);
      SendOutput("Use bc-wblock /undo to revert the changes");

      //RELOAD CHUNKS
      BCChunks.ReloadForClients(modifiedChunks);

      return true;
    }

    private bool ScanBlocks(Vector3i size, Vector3i p3, BlockValue _bv)
    {
      Block block1 = Block.list[_bv.type];
      if (block1 == null && _params[0] != "*")
      {
        SdtdConsole.Instance.Output("Unable to find block by id or name");

        return false;
      }

      var stats = new Dictionary<string, int>();
      //var density = new Dictionary<string, List<int>>();
      var _clrIdx = 0;
      for (int j = 0; j < size.y; j++)
      {
        for (int i = 0; i < size.x; i++)
        {
          for (int k = 0; k < size.z; k++)
          {
            Vector3i p5 = new Vector3i(i + p3.x, j + p3.y, k + p3.z);
            var b = GameManager.Instance.World.GetBlock(_clrIdx, p5);
            //var d = GameManager.Instance.World.GetDensity(_clrIdx, p5);
            //var t = GameManager.Instance.World.GetTexture(i + p3.x, j + p3.y, k + p3.z);
            string name = "";
            if (ItemClass.list[b.type] != null)
            {
              name = ItemClass.list[b.type].Name;
              if (name == null || name == "")
              {
                name = "air";
              }
            }

            if (_params[0] == "*")
            {
              if (stats.ContainsKey(name))
              {
                stats[name] += 1;
              }
              else
              {
                stats.Add(name, 1);
              }
            }
            else
            {
              if (name == _bv.Block.GetBlockName())
              {
                if (stats.ContainsKey(name))
                {
                  stats[name] += 1;
                }
                else
                {
                  stats.Add(name, 1);
                }
              }
            }
          }
        }
      }

      if (block1 != null)
      {
        SendOutput("Block stats for block " + block1.GetBlockName());
      }
      else
      {
        SendOutput("Block stats for all blocks");
      }
      foreach (var stat in stats)
      {
        SendOutput(stat.Key + ":" + stat.Value.ToString());
      }
      //if (_options.ContainsKey("density"))
      //{
      //  if (block1 != null)
      //  {
      //    SendOutput("Block density for block " + block1.GetBlockName());
      //  }
      //  else
      //  {
      //    SendOutput("Block density for all blocks");
      //  }
      //  foreach (var den in density)
      //  {
      //    string o = "";
      //    int t = 0;
      //    den.Value.Sort();
      //    foreach (var i in den.Value)
      //    {
      //      o += i.ToString() + ",";
      //      t += i;
      //    }
      //    SendOutput(den.Key + ":" + o.Substring(0, o.Length - 2) + " - Av:" + (t / den.Value.Count).ToString());
      //  }
      //}

      return true;
    }

    private void Notes ()
    {
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
