using System;
using System.Collections.Generic;

namespace BCM.Commands
{
  public class BCTileEntity : BCCommandAbstract
  {
    public override void Process()
    {
      if (_params.Count == 0)
      {
        SendOutput(GetHelp());
        
        return;
      }

      //use loc and current location, or 2x vector3i to define area, or current loc and a radius

      if (_params[0] == "owner")
      {
        //grants the target ownership of secure tiles in the area
      }
      else if (_params[0] == "access")
      {
        //grants the target access to secure tiles in the area
      }
      else if (_params[0] == "add")
      {
        //adds items to a secure tile in the area
      }
      else if (_params[0] == "empty")
      {
        //empties secure tiles in the area or a type or all
      }
      else if (_params[0] == "remove")
      {
        //removes secure tiles in the area or a type or all
      }

    }
  }
}
