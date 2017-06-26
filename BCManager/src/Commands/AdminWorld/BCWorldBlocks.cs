using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using UnityEngine;

namespace BCM.Commands
{
  public class BCWorldBlocks : BCCommandAbstract
  {
    public override void Process()
    {
      if (_params.Count == 0)
      {
        //bc-wblocks - lists options / help details (see below)
        //  <co-ords> = 2x vector3i pos

        //bc-wblocks <co-ords> /fill <blockid> - fills the area with the specified block
        //bc-wblocks <co-ords> /fill <blockid> [/face0=textureId] [/face1=textureId] [/face2=textureId] [/face3=textureId] [/face4=textureId] [/face5=textureId]
        //bc-wblocks <co-ords> /swap <targetblockid> <replacementblockid> [/face0=textureId] [/face1=textureId] [/face2=textureId] [/face3=textureId] [/face4=textureId] [/face5=textureId]
        //bc-wblocks <co-ords> /chown <entityid> (or /self) - sets the owner of the tileentity blocks and land claims in the area to the entityid listed (or the commander if /self used)
        //bc-wblocks <co-ords> /densify <density> - set the density of blocks within the area. If <density> is neg: set terrain blocks, positive: set cube blocks (or is that reversed?)
        //  /noair - option means it will only swap the blocks if original block is not air (applies to /fill and /swap)
        //  /nowet - option means it will only swap the blocks if original block is not water (applies to /fill and /swap)
        //  /noclaim - skips processing of claim blocks in target area
        //  /random=1,2,3,4,5 a list of blocks to use for /fill and /swap to randomly replace blocks in target area
        //  /circle - instead of two vector3i provide a single vector3i and an inner+outer radius and height, optionally a pair of values for arc degrees
        //  /prefab - a prefab to insert repeatedly within the area


        //loc - shows player current location (/worldpos /etc), and sets the pos for prafab and block commands
        //bc-rb, bc-renderblocks, bc-block, block
        //bc-block upgrade - upgrades the blocks 1 step within an area
        //bc-block repair - repairs all blocks in the area
        //bc-block swap - swaps source block with target block for blocks in the area
        //bc-block randomdam - randomly damages all blocks in the area
        //bc-block downgrade /nodestroy - downgrades all blocks in area, optional for no destroy but only those not on the last stage
        //bc-block insert /fill=terrain /fill=air /fill=cube[/texture=0,1,2,3,4,5] [default]/fill=all - changes all blocks in an area with the block specified, options act as filters what blocks get replaced in the target area
        //bc-block prefab /nopartial /2d - renders a prefab in the area defined repeating the prefab to fill the area, optional on nopartial to prevent the insertion of partial prefabs at the edges of the area. 2d optional to only draw 1 layer of prefabs rather than stacking them (default)
        //bc-chunk reset - resets the chunk to its rwg original state
        //bc-chunk reload <player> - reloads the chunks in that players loaded chunk list


      }

      if (_params.Count != 7)
      {
        SendOutput("Param error, use bc-wblocks <type> <x1> <y1> <z1> <x2> <y2> <z2> [/options]");
      }
      else
      {
        int x1, x2, y1, y2, z1, z2;
        if (!int.TryParse(_params[0], out x1) || !int.TryParse(_params[1], out y1) || !int.TryParse(_params[2], out z1) || !int.TryParse(_params[3], out x2) || !int.TryParse(_params[4], out y2) || !int.TryParse(_params[5], out z2))
        {
          SendOutput("Error parsing coordinates");
        }
        else
        {
          if (_options.ContainsKey("fill"))
          {

          }
          else if (_options.ContainsKey("swap"))
          {

          }
          else if (_options.ContainsKey("chown"))
          {

          }
          else if (_options.ContainsKey("densify"))
          {
            //options: randomise, smooth, default

          }
        }
      }
    }

    private void FillBlocks()
    {

    }

    private void SwapBlocks()
    {

    }

    private void ChownBlocks()
    {

    }

    private void DensifyBlocks()
    {

    }

  }
}
